using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Headers;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Newtonsoft.Json;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Maui.Alerts;
using Microsoft.Maui.Storage;
using WeightWizard.Model.DTOs;

namespace WeightWizard.ViewModel
{
    // This class represents the ViewModel for the LoggerPage view
    public partial class LoggerPageViewModel : ObservableObject
    {
        // Observable properties represent data to be displayed in the view
        [ObservableProperty] private decimal _morningWeight;
        [ObservableProperty] private int _steps;
        [ObservableProperty] private int _dailyCalorieIntake;
        [ObservableProperty] private DateTime _selectedDate = DateTime.Now.Date.AddHours(2);
        
        // HttpClient used to make HTTP requests
        private readonly HttpClient _httpClient = new();
        
        // User details
        private int _userid;
        private decimal _desiredWeight = 0;

        // Constructor for LoggerPageViewModel
        public LoggerPageViewModel()
        {
            GetExistingDailyDataAsync();
            GetUserDataAsync();
        }
        
        // This method is used to decode the JWT token and extract the user's ID
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


        // Event handler for when the selected date changes
        partial void OnSelectedDateChanged(DateTime value)
        {
            GetExistingDailyDataAsync();
        }

        // RelayCommand that triggers the data logging process
        [RelayCommand]
        public async void LogData()
        {
            var token = await SecureStorage.GetAsync("jwt_token");
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", token);
            DecodeJwtToken(token);
            
            Console.WriteLine("executing post logdata");
            if (MorningWeight<=0 ||Steps<=0||DailyCalorieIntake<=0)
            {
                Console.WriteLine("please enter all data");
                
                var alert = Toast.Make($"please enter all data", CommunityToolkit.Maui.Core.ToastDuration.Long, 14);
                await alert.Show();
            }
            else
            {
                var isLogged = await CheckIfDayIsEmptyAsync(_userid, SelectedDate);
                if (isLogged)
                {
                    try
                    {
                        DailyDataDto dailyDataDto = new()
                        {
                            MorningWeight = MorningWeight,
                            CalorieIntake = DailyCalorieIntake,
                            Steps = Steps
                        };

                        try {
                            await UpdateUserAsync(_userid, SelectedDate, dailyDataDto);
                            Console.WriteLine("Daily Data updated successfully");
                            var alert = Toast.Make($"Successfully updated data", CommunityToolkit.Maui.Core.ToastDuration.Long, 14);
                            await alert.Show();
                        } catch (HttpRequestException ex) {
                            Console.WriteLine($"Error updating Daily Data: {ex.Message}");
                        } catch (Exception ex) {
                            Console.WriteLine($"Unexpected error: {ex.Message}");
                        }
                    }
                    catch (HttpRequestException ex)
                    {
                        Console.WriteLine($"Error connecting to server: {ex.Message}");
                    }
                    return;
                }
                try
                {
                    var postSuccessful = await LogAsync();

                    //display data logged popup
                    Console.WriteLine(postSuccessful ? "Data Successfully Logged" : "error during logging");
                    if (!postSuccessful)
                    {
                        var failAlert = Toast.Make($"Failed to updated data\nPlease check your internet connection", CommunityToolkit.Maui.Core.ToastDuration.Long, 14);
                        await failAlert.Show();
                        return;
                    }
                    var successAlert = Toast.Make($"Successfully logged data", CommunityToolkit.Maui.Core.ToastDuration.Long, 14);
                    await successAlert.Show();
                    return;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error connecting to server: {ex.Message}");
                    var successAlert = Toast.Make($"Something bad happened", CommunityToolkit.Maui.Core.ToastDuration.Long, 14);
                    await successAlert.Show();
                }
            }
        }
        
        // Fetch existing daily data for the current user and selected date
        public async Task GetExistingDailyDataAsync()
        {
            var token = await SecureStorage.GetAsync("jwt_token");
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", token);
            DecodeJwtToken(token);
            
            var isLogged = await CheckIfDayIsEmptyAsync(_userid, SelectedDate);
            if (!isLogged)
            {
                MorningWeight = 0;
                Steps = 0;
                DailyCalorieIntake = 0;
                return;
            }
            try
            {
                var response = await GetDailyDataAsync(_userid, SelectedDate);

                MorningWeight = response.MorningWeight;
                Steps = response.Steps;
                DailyCalorieIntake = response.CalorieIntake;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Something bad happened: {ex.Message}");
            }
        }

        #region BackendCalls
        
        //POST dailyData
        private async Task<bool> LogAsync()
        {
            try
            {
                var logDataDto = new DailyDataDto
                {
                    UserId = _userid,
                    MorningWeight = MorningWeight,
                    CalorieIntake = DailyCalorieIntake,
                    DesiredWeight = _desiredWeight,
                    Date = SelectedDate,
                    Steps = Steps
                };

                var jsonLoginData = JsonConvert.SerializeObject(logDataDto);

                var postData = new StringContent(jsonLoginData, Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync("https://weightwizard.azurewebsites.net/api/DailyData", postData);

                if (response.IsSuccessStatusCode)
                {
                    Console.WriteLine("Data logged successfully.");
                    var alert = Toast.Make($"Successfully logged data", CommunityToolkit.Maui.Core.ToastDuration.Long, 14);
                    await alert.Show();
                    return true;
                }
                else
                {
                    Console.WriteLine("Failed to log data. StatusCode: " + response.StatusCode);
                    var alert = Toast.Make($"Failed to log data\nPlease Check your internet connection", CommunityToolkit.Maui.Core.ToastDuration.Long, 14);
                    await alert.Show();
                    return false;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred while logging data: " + ex.Message);
                var alert = Toast.Make($"Something went wrong\nPlease try again later", CommunityToolkit.Maui.Core.ToastDuration.Long, 14);
                await alert.Show();
                return false;
            }
        }

        //GET check dailyData
        private async Task<bool> CheckIfDayIsEmptyAsync(int userId, DateTime date)
        {
            var formattedDate = date.ToString("yyyy-MM-dd");
            try
            {
                var response = await _httpClient.GetAsync("https://weightwizard.azurewebsites.net/api/DailyData/" +
                                                          userId + "/" + formattedDate + "T00%3A00%3A00");
                return response.IsSuccessStatusCode;
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"Bad Request: {ex.Message}");
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Unknown Error " + ex.Message);
                return false;
            }
        }
        
        //PATCH dailyData
        public async Task UpdateUserAsync(int userId, DateTime date, DailyDataDto dailyData) {
            var formattedDate = date.ToString("yyyy-MM-dd");
            
            var uri = new Uri($"https://weightwizard.azurewebsites.net/api/DailyData/" + userId + "/" +
                              formattedDate + "T00%3A00%3A00");
    
            var json = JsonConvert.SerializeObject(dailyData);
            var content = new StringContent(json, Encoding.UTF8, "application/json-patch+json");
    
            var response = await _httpClient.PatchAsync(uri, content);
            response.EnsureSuccessStatusCode();
        }
        
        //GET dailyData
        private async Task<DailyDataDto> GetDailyDataAsync(int userId, DateTime date)
        {
            var formattedDate = date.ToString("yyyy-MM-dd");
            var response = await _httpClient.GetAsync("https://weightwizard.azurewebsites.net/api/DailyData/" + userId + "/" +
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
        
        //GET user
        private async Task GetUserDataAsync()
        {
            try
            {
                var token = await SecureStorage.GetAsync("jwt_token");
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", token);
                DecodeJwtToken(token);

                var response = await _httpClient.GetAsync("https://weightwizard.azurewebsites.net/api/Users/" + _userid);

                if (!response.IsSuccessStatusCode)
                {
                    var alert = Toast.Make($"Couldn't get your current Goal\nPlease check your internet connection", CommunityToolkit.Maui.Core.ToastDuration.Long, 14);
                    await alert.Show();
                    return;
                }

                var content = response.Content;

                // Read the content as a string
                var result = await content.ReadAsStringAsync();

                // Deserialize the JSON content into a strongly-typed object
                var userDto = JsonConvert.DeserializeObject<UserDto>(result);

                _desiredWeight = userDto.DesiredWeight;
            }
            catch (Exception ex)
            {
                var alert = Toast.Make($"Something bad happened\nPlease Check your internet connection", CommunityToolkit.Maui.Core.ToastDuration.Long, 14);
                await alert.Show();
            }
        }
        #endregion
    }
}