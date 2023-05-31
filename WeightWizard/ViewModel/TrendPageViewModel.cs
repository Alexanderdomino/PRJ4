using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Newtonsoft.Json;
using System.Collections.ObjectModel;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Headers;
using WeightWizard.Model;
using WeightWizard.Model.DTOs;

namespace WeightWizard.ViewModel
{
    public partial class TrendPageViewModel : ObservableObject
    {
        [ObservableProperty]
        public ObservableCollection<weightModel> data = new();

        //[ObservableProperty]
        public ObservableCollection<weightModel> webdata;

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

        public enum ShowStates
        {
            All,
            ThreeMonths,
            Month
        }

        public ShowStates state;

        public TrendPageViewModel()
        {

            webdata = new ObservableCollection<weightModel>();

            GetWebDataAsync();

            
        }

        private async void GetWebDataAsync()
        {
            var token = await SecureStorage.GetAsync("jwt_token");
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", token);
            DecodeJwtToken(token);
            
            for (DateTime date = DateTime.Now.AddDays(-365); date <= DateTime.Now; date = date.AddDays(1))
            {
                var dailyDataObj = await GetDailyDataAsync(_userid, date);
                if (dailyDataObj != null)
                {
                    webdata.Add(new weightModel(
                        date.Date,
                        (double)dailyDataObj.MorningWeight,
                        dailyDataObj.Steps,
                        dailyDataObj.CalorieIntake));
                
                
                }
            }

            state = ShowStates.Month;

            ShowData();
        }

        [RelayCommand]
        public void SeeThreeMonths()
        {
            state = ShowStates.ThreeMonths;
            ShowData();
        }

        [RelayCommand]
        public void SeeAll()
        {
            state = ShowStates.All;
            ShowData();
        }


        [RelayCommand]
        public void SeeMonth()
        {
            state = ShowStates.Month;
            ShowData();
        }


        [RelayCommand]
        public void ShowData()
        {
            switch (state)
            {
                case ShowStates.All:
                    Data.Clear();
                    Data = new ObservableCollection<weightModel>(webdata);
                    break;
                case ShowStates.ThreeMonths:
                    Data.Clear();
                    foreach (var item in webdata)
                        if (item.Date >= DateTime.Now.AddDays(-90))
                        {
                            Data.Add(item);
                        }
                    break;
                case ShowStates.Month:
                    Data.Clear();
                    foreach (var item in webdata)
                        if (item.Date >= DateTime.Now.AddDays(-30))
                        {
                            Data.Add(item);
                        }
                    break;
                default:
                    break;
            }
        }

        #region BackendCalls
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
        #endregion
    }
}
