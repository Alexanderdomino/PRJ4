using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Markup;
using WeightWizard.Model;
using WeightWizard.Model.Drawables;
using WeightWizard.Model.DTOs;
using WeightWizard.View;
using static Microsoft.Maui.Controls.Internals.Profile;
using static System.Runtime.InteropServices.JavaScript.JSType;
//using WeightWizard.View.WeightWizard;

namespace WeightWizard.ViewModel
{
    public partial class TrendPageViewModel : ObservableObject
    {
        [ObservableProperty]
        public ObservableCollection<weightModel> data;


        public ObservableCollection<weightModel> webdata;

        //HttpClient for getting daily data
        private readonly HttpClient _httpClient = new HttpClient();
        private readonly string _token = await SecureStorage.GetAsync("jwt_token");

        public enum ShowStates
        {
            All,
            ThreeMonths,
            Month
        }

        public ShowStates state;

        public TrendPageViewModel()
        {
            Data = new ObservableCollection<weightModel>();

            webdata = new ObservableCollection<weightModel>();

            GetWebDataAsync();

            state = ShowStates.Month;

            ShowData();

            
        }

        public void getData()
        {
            var today = DateTime.Now;
            double weight = 70;
            int steps = 10000;
            int calories = 2500;
            Random ran = new Random();

            for (DateTime date = today.AddDays(-365); date <= DateTime.Now.Date; date = date.AddDays(1))
            {

                webdata.Add(new weightModel(date.Date, weight, steps, calories));
                if (ran.NextDouble() < 0.5)
                {
                    weight += ran.NextDouble();
                    steps -= 600;
                    calories += 10;
                }
                else
                {
                    weight -= ran.NextDouble();
                    steps += 600;
                    calories -= 10;
                }
            }
        }

        private async void GetWebDataAsync()
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", _token);
            
            for (DateTime date = DateTime.Now.AddDays(-365); date <= DateTime.Now; date = date.AddDays(1))
            {
                var dailyDataObj = await GetDailyDataAsync(1, date);
                if (dailyDataObj != null)
                {
                    webdata.Add(new weightModel(
                        date.Date,
                        (double)dailyDataObj.MorningWeight,
                        dailyDataObj.Steps,
                        dailyDataObj.CalorieIntake));
                
                
                }

               
            }
           

            
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

    }
}
