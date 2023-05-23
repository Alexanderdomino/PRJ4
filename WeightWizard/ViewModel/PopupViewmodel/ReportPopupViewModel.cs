using System.IdentityModel.Tokens.Jwt;
using CommunityToolkit.Mvvm.ComponentModel;
using WeightWizard.Model;
using WeightWizard.Model.Interfaces;

namespace WeightWizard.ViewModel.PopupViewmodel;

public partial class ReportPopupViewModel : ObservableObject
{
    
    [ObservableProperty] private string _weeklyreport;
    [ObservableProperty] private int _calories;
    [ObservableProperty] private int _steps;
    private decimal _desiredWeight;

    public ReportPopupViewModel(ICalenderItems selectedItem)
    {
        initialize(selectedItem);
    }

    private async void initialize( ICalenderItems selectedItem) 
    {
        var token = await SecureStorage.GetAsync("jwt_token");
        DecodeJwtToken(token);
        
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
        var tempDesiredWeight = claims.FirstOrDefault(c => c.Type == "DesiredWeight")?.Value;

        if (tempDesiredWeight != null) _desiredWeight = decimal.Parse(tempDesiredWeight);
    }
}