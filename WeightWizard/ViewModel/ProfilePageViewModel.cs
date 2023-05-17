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
    private readonly HttpClient _httpClient = new HttpClient();
    
    // Command to handle selection change
    [RelayCommand]
    // ReSharper disable once MemberCanBePrivate.Global
    public async void SaveChanges()
    {
        UserDto user = new();

        user.DesiredWeight = DesiredWeight;

        try {
            // Do something with goal weight...
            await UpdateUserAsync(1, user);
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
    
    public static async Task UpdateUserAsync(int userId, UserDto updatedUser) {
        var client = new HttpClient();
        var uri = new Uri($"https://prj4backend.azurewebsites.net/api/Users/{userId}");
    
        var json = Newtonsoft.Json.JsonConvert.SerializeObject(updatedUser);
        var content = new StringContent(json, Encoding.UTF8, "application/json-patch+json");
    
        var response = await client.PatchAsync(uri, content);
        response.EnsureSuccessStatusCode();
    }
}