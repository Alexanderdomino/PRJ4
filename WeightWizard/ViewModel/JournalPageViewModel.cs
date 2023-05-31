using System.Collections.ObjectModel;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Headers;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Mopups.Services;
using Newtonsoft.Json;
using WeightWizard.Model;
using WeightWizard.Model.DTOs;
using WeightWizard.Model.Interfaces;
using WeightWizard.View.Popups;

namespace WeightWizard.ViewModel
{
    public partial class JournalPageViewModel : ObservableObject
    {
        // Current selected date
        // ReSharper disable once InconsistentNaming
        [ObservableProperty] public ICalenderItems selectedItem;

        // ReSharper disable once InconsistentNaming
        [ObservableProperty] public DateTime selectedMonth = DateTime.Today;

        private bool _isLoading;

        private List<CalenderModel> _reportDays = new();

        //HttpClient for getting daily data
        private readonly HttpClient _httpClient = new();
        
        private int _userid;
        
        public void DecodeJwtToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var jwtToken = tokenHandler.ReadJwtToken(token);

            // Access the claims from the decoded token
            var claims = jwtToken.Claims;

            //Get the userid claim
            var nameidentifier = claims.FirstOrDefault(c => c.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier")?.Value;

            if (nameidentifier != null) _userid = int.Parse(nameidentifier);
        }

        partial void OnSelectedMonthChanged(DateTime SelectedMonth)
        {
            Dates.Clear();
            BindDates(SelectedMonth);
        }

        // Collection of dates for the calendar
        public ObservableCollection<ICalenderItems> Dates { get; set; } = new();

        // Constructor
        public JournalPageViewModel()
        {
            // Bind dates for the current month
            BindDates(SelectedMonth);
        }

        // Method to bind dates to the calendar
        private async void BindDates(DateTime selectedDate)
        {
            _isLoading = true;
            
            var token = await SecureStorage.GetAsync("jwt_token");
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", token);
            DecodeJwtToken(token);
            
            // Check if there is a successful connection to the server
            var isConnected = await CheckServerConnectionAsync();
            if (!isConnected)
            {
                Console.WriteLine("Error connecting to server");
                return;
            }
            
            // Get the number of days in the month
            var daysCount = DateTime.DaysInMonth(selectedDate.Year, selectedDate.Month)+1;

            // Get the first day of the month
            var firstDayOfMonth = new DateTime(selectedDate.Year, selectedDate.Month, 1);

            // Get the number of days before the first day of the month
            var daysBeforeMonth = (int)firstDayOfMonth.DayOfWeek - 1;

            var loggedDays = 0;

            // Add day names to the collection
            for (var i = 1; i < 8; i++)
            {
                Dates.Add(new DayNameModel()
                {
                    Date = new DateTime(2021, 2, i)
                });
            }

            // Add an empty day to the collection
            Dates.Add(new EmptyDayModel());

            // If the first day of the month is not a Monday, add empty days to the collection
            if (firstDayOfMonth.DayOfWeek != DayOfWeek.Monday)
            {
                for (var spoofDay = 0; spoofDay < daysBeforeMonth; spoofDay++)
                {
                    Dates.Add(new EmptyDayModel());
                }
            }

            // Add days of the month to the collection
            for (var day = 1; day < daysCount; day++)
            {
                var dateToGetOn = new DateTime(selectedDate.Year, selectedDate.Month, day);
                try
                {
                    var dailyDataObj = await GetDailyDataAsync(_userid,dateToGetOn);

                    if (dailyDataObj != null)
                    {
                        Dates.Add(new CalenderModel
                        {
                            IsLogged = true,
                            Date = dailyDataObj.Date,
                            CalorieIntake = dailyDataObj.CalorieIntake,
                            Steps = dailyDataObj.Steps,
                            MorningWeight = dailyDataObj.MorningWeight
                        });
                        loggedDays++;

                        if(loggedDays==1)
                        {
                            _reportDays.Add(new CalenderModel
                            {
                                IsLogged = true,
                                Date = dailyDataObj.Date,
                                CalorieIntake = dailyDataObj.CalorieIntake,
                                Steps = dailyDataObj.Steps,
                                MorningWeight = dailyDataObj.MorningWeight

                            });
                        }

                        else if (loggedDays>4)
                        {
                            _reportDays.Add(new CalenderModel
                            {
                                IsLogged = true,
                                Date = dailyDataObj.Date,
                                CalorieIntake = dailyDataObj.CalorieIntake,
                                Steps = dailyDataObj.Steps,
                                MorningWeight = dailyDataObj.MorningWeight

                            });
                        }
                    }
                    else
                    {
                        Dates.Add(new CalenderModel
                        {
                            Date = new DateTime(selectedDate.Year, selectedDate.Month, day)
                        });
                    }
                
                    // Add a report model after every 7 days
                    if ((day + daysBeforeMonth) % 7 == 0)
                    {
                        if (loggedDays > 4)
                        {
                            Dates.Add(new ReportModel
                            {
                                Unlocked = true,
                                ReportDays = new List<CalenderModel>(_reportDays)

                            });
                            loggedDays = 0;
                            _reportDays.Clear();
                            
                        }
                        else
                        {
                            Dates.Add(new ReportModel());
                            loggedDays = 0;
                            _reportDays.Clear();
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error connecting to server: {ex.Message}");
                }
            }

            _isLoading = false;
        }

        // Command to handle selection change
        [RelayCommand]
        public void CurrentDate()
        {
            if (SelectedItem is CalenderModel)
            {
                // Show popup of selected item
                MopupService.Instance.PushAsync(new DatePopupPage(SelectedItem));
            }
            else if (SelectedItem is ReportModel)
            {
                MopupService.Instance.PushAsync(new ReportPopupPage(SelectedItem));
            }
        }

        //Swiping on month left
        [RelayCommand]
        public void MonthSwipeLeft()
        {
            if (_isLoading)
            {
                return;
            }
            SelectedMonth = SelectedMonth.AddMonths(1);
        }
        
        //Swiping on month right
        [RelayCommand]
        public void MonthSwipeRight()
        {
            if (_isLoading)
            {
                return;
            }
            SelectedMonth = SelectedMonth.AddMonths(-1);
        }

        #region BackendCalls

        //GET dailyData
        private async Task<DailyDataDto> GetDailyDataAsync(int userId, DateTime date)
        {
            var formattedDate = date.ToString("yyyy-MM-dd");
            var response = await _httpClient.GetAsync("https://prj4backend.azurewebsites.net/api/DailyData/" + userId + "/" +
                                                      formattedDate + "T00%3A00%3A00");
            if (response.IsSuccessStatusCode)
            {
                var content = response.Content;
                
                // Read the content as a string
                var result = await content.ReadAsStringAsync();
            
                // Deserialize the JSON content into a strongly-typed object
                var dailyDataDto = JsonConvert.DeserializeObject<DailyDataDto>(result);

                return dailyDataDto;
            }
            else
            {
                return null;
            }
        }
        
        //GET check connection
        private async Task<bool> CheckServerConnectionAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync("https://prj4backend.azurewebsites.net/api/DailyData");
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error connecting to server: {ex.Message}");
                return false;
            }
        }
        #endregion
    }
}