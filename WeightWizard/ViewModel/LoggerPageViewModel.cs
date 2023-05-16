using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Newtonsoft.Json;
using System.Text;
using WeightWizard.Model.DTOs;

namespace WeightWizard.ViewModel
{
    public partial class LoggerPageViewModel : ObservableObject
    {
        //[ObservableProperty] private int _userId;
        [ObservableProperty] private decimal _morningWeight;
        [ObservableProperty] private int _steps;
        //[ObservableProperty] private double _desiredWeight;
        [ObservableProperty] private int _dailyCalorieIntake;
        [ObservableProperty] private DateTime _selectedDate = DateTime.Today;
        
        private readonly HttpClient _httpClient = new HttpClient();

        [RelayCommand]
        public async void LogData()
        {
            Console.WriteLine("executing post logdata");
            if (MorningWeight<=0 ||Steps<=0||DailyCalorieIntake<=0)
            {
                Console.WriteLine("please enter all data");
            }
            else
            {
                var isLogged = await CheckIfDayIsEmptyAsync(1, SelectedDate);
                if (isLogged) return;
                try
                {
                    var postSuccessful = await LogAsync(1, 100);

                    if (postSuccessful)
                    {
                        //display data logged popup
                        Console.WriteLine("Data Successfully Logged");
                    }
                    else
                    {
                        Console.WriteLine("error during logging");
                    }
                }
                catch (HttpRequestException ex)
                {
                    Console.WriteLine($"Error connecting to server: {ex.Message}");
                }
            }
        }

        private async Task<bool> LogAsync(int userId, decimal desiredWeight)
        {
            try
            {
                var logDataDto = new DailyDataDto();

                logDataDto.UserId = userId;
                logDataDto.MorningWeight = MorningWeight;
                logDataDto.CalorieIntake = DailyCalorieIntake;
                logDataDto.DesiredWeight = desiredWeight;
                logDataDto.Date = SelectedDate;
                logDataDto.Steps = Steps;
                var jsonLoginData = JsonConvert.SerializeObject(logDataDto);

                var postData = new StringContent(jsonLoginData, Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync("http://localhost:7023/api/DailyData", postData);

                if (response.IsSuccessStatusCode)
                {
                    Console.WriteLine("Data logged successfully.");
                    return true;
                }
                else
                {
                    Console.WriteLine("Failed to log data. StatusCode: " + response.StatusCode);
                    return false;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred while logging data: " + ex.Message);
                return false;
            }
        }

        
        private async Task<bool> CheckIfDayIsEmptyAsync(int userId, DateTime date)
        {
            var formattedDate = date.ToString("yyyy-MM-dd");
            try
            {
                var response = await _httpClient.GetAsync("https://prj4backend.azurewebsites.net/api/DailyData/" +
                                                          userId + "/" +
                                                          formattedDate + "T00%3A00%3A00");
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
        
        public static async Task UpdateUserAsync(int userId, DateTime date, DailyDataDto dailyData) {
            var client = new HttpClient();
            var formattedDate = date.ToString("yyyy-MM-dd");
            
            var uri = new Uri($"https://prj4backend.azurewebsites.net/api/DailyData/" + userId + "/" +
                              formattedDate + "T00%3A00%3A00");
    
            var json = Newtonsoft.Json.JsonConvert.SerializeObject(dailyData);
            var content = new StringContent(json, Encoding.UTF8, "application/json-patch+json");
    
            var response = await client.PatchAsync(uri, content);
            response.EnsureSuccessStatusCode();
        }
    }
}