using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Headers;
using System.Text;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Newtonsoft.Json;
using Plugin.LocalNotification;
using WeightWizard.Model.DTOs;

namespace WeightWizard.ViewModel;

public partial class ProfilePageViewModel: ObservableObject
{
    [ObservableProperty]
    private bool _allowNotificationsIsChecked = true;

    [ObservableProperty]
    private decimal _desiredWeight = 0;
    
    //HttpClient for patching goal weight
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
    
    // Command to handle selection change
    [RelayCommand]
    // ReSharper disable once MemberCanBePrivate.Global
    public async void SaveChanges()
    {
        var token = await SecureStorage.GetAsync("jwt_token");
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", token);
        
        UserDto user = new()
        {
            DesiredWeight = DesiredWeight
        };

        try {
            // Do something with goal weight...
            await UpdateUserAsync(_userid, user);
            Console.WriteLine("User updated successfully");
        } catch (HttpRequestException ex) {
            Console.WriteLine($"Error updating user: {ex.Message}");
        } catch (Exception ex) {
            Console.WriteLine($"Unexpected error: {ex.Message}");
        }
        
        //Check if user has un-/checked notifications
        if (!AllowNotificationsIsChecked) return;
        var request = new NotificationRequest()
        {
            NotificationId = 1,
            BadgeNumber = 1,
            Title = "Weight Wizard",
            Description = "Remember to log your Data, to unlock weekly Report",
            Schedule = new NotificationRequestSchedule()
            {
                //NotifyTime = DateTime.Now.AddSeconds(5)
                NotifyTime = DateTime.Today.AddHours(21),
                RepeatType = NotificationRepeat.Daily
            }
        };
        await LocalNotificationCenter.Current.Show(request);
    }

    private async Task UpdateUserAsync(int userId, UserDto updatedUser) {
        var uri = new Uri($"https://prj4backend.azurewebsites.net/api/Users/{userId}");
    
        var json = JsonConvert.SerializeObject(updatedUser);
        var content = new StringContent(json, Encoding.UTF8, "application/json-patch+json");
    
        var response = await _httpClient.PatchAsync(uri, content);
        response.EnsureSuccessStatusCode();
    }

    [RelayCommand]
    public static async void SignOut()
    {
        SecureStorage.Default.RemoveAll();
        await Shell.Current.GoToAsync("///login");
    }
}