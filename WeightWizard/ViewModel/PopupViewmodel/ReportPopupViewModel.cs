using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Headers;
using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Mvvm.ComponentModel;
using Newtonsoft.Json;
using WeightWizard.Model;
using WeightWizard.Model.DTOs;
using WeightWizard.Model.Interfaces;

namespace WeightWizard.ViewModel.PopupViewmodel;

public partial class ReportPopupViewModel : ObservableObject
{
    
    [ObservableProperty] private string _weeklyreport;
    [ObservableProperty] private int _calories;
    [ObservableProperty] private int _steps;
    private decimal _desiredWeight;
    private int _userid;
    
    // HttpClient used to make HTTP requests
    private readonly HttpClient _httpClient = new();

    public ReportPopupViewModel(ICalenderItems selectedItem)
    {
        initialize(selectedItem);
    }

    private async void initialize( ICalenderItems selectedItem) 
    {
        var token = await SecureStorage.GetAsync("jwt_token");
        DecodeJwtToken(token);
        await GetUserDataAsync();
        
        var temp = selectedItem as ReportModel;
        if (temp != null)
        {
            if (temp.ReportDays.Last().MorningWeight > _desiredWeight)
            {
                if(temp.ReportDays.First().MorningWeight < temp.ReportDays.Last().MorningWeight)
                {
                    Weeklyreport = "you've gained "+ (temp.ReportDays.Last().MorningWeight-temp.ReportDays.First().MorningWeight) + " kg\n"
                        + "- Try decreasing your caloric intake by 100 kcal\n"
                        + "- And increasing your daily steps by 100 steps";
                }
                else
                {
                    Weeklyreport = "you've lost "+ (temp.ReportDays.First().MorningWeight - temp.ReportDays.Last().MorningWeight) + " kg\n"
                        + "- Keep up the good work champ!";
                }
            }
            else if(temp.ReportDays.Last().MorningWeight < _desiredWeight)
            {
                if(temp.ReportDays.First().MorningWeight < temp.ReportDays.Last().MorningWeight)
                {
                    Weeklyreport = "You've gained "+ (temp.ReportDays.Last().MorningWeight-temp.ReportDays.First().MorningWeight) + " kg\n"
                                   + "- Keep up the good work champ!";
                }
                else
                {
                    Weeklyreport = "You've lost "+ (temp.ReportDays.First().MorningWeight - temp.ReportDays.Last().MorningWeight) + " kg\n"
                                   + "- Try increasing your caloric intake by 100 kcal\n"
                                   + "- And/or decreasing your daily steps by 100 steps";
                }
            }

        }
    }
    
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
}