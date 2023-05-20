using System.Net.Http.Headers;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Newtonsoft.Json;
using System.Text;
using WeightWizard.Model.DTOs;

namespace WeightWizard.ViewModel
{
    public partial class LoggerPageViewModel : ObservableObject
    {
        [ObservableProperty] private decimal _morningWeight;
        [ObservableProperty] private int _steps;
        [ObservableProperty] private int _dailyCalorieIntake;
        
        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(GetExistingDailyDataCommand))]
        private DateTime _selectedDate = DateTime.Today;
        
        private readonly HttpClient _httpClient = new();

        [RelayCommand]
        public async void LogData()
        {
            var token = await SecureStorage.GetAsync("jwt_token");
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", token);
            
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

                    //display data logged popup
                    Console.WriteLine(postSuccessful ? "Data Successfully Logged" : "error during logging");
                }
                catch (HttpRequestException ex)
                {
                    Console.WriteLine($"Error connecting to server: {ex.Message}");
                }
            }
        }

        [RelayCommand]
        public async Task GetExistingDailyDataAsync()
        {
            var token = await SecureStorage.GetAsync("jwt_token");
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", token);
            var isLogged = await CheckIfDayIsEmptyAsync(1, SelectedDate);
            if (!isLogged) return;
            try
            {
                var response = await GetDailyDataAsync(1, SelectedDate);

                MorningWeight = response.MorningWeight;
                Steps = response.Steps;
                DailyCalorieIntake = response.CalorieIntake;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Something bad happened: {ex.Message}");
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

                var response = await _httpClient.PostAsync("https://prj4backend.azurewebsites.net/api/DailyData", postData);

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
        
        public async Task UpdateUserAsync(int userId, DateTime date, DailyDataDto dailyData) {
            var formattedDate = date.ToString("yyyy-MM-dd");
            
            var uri = new Uri($"https://prj4backend.azurewebsites.net/api/DailyData/" + userId + "/" +
                              formattedDate + "T00%3A00%3A00");
    
            var json = JsonConvert.SerializeObject(dailyData);
            var content = new StringContent(json, Encoding.UTF8, "application/json-patch+json");
    
            var response = await _httpClient.PatchAsync(uri, content);
            response.EnsureSuccessStatusCode();
        }
        
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
    }
}