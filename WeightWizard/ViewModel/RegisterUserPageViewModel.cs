using System.Text;
using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Newtonsoft.Json;
using WeightWizard.Model.DTOs;

namespace WeightWizard.ViewModel;

public partial class RegisterUserPageViewModel : ObservableObject
{
    [ObservableProperty] private string _username;
    [ObservableProperty] private string _password;
    [ObservableProperty] private string _gender;
    [ObservableProperty] private int _height;
    [ObservableProperty] private decimal _weight;
    [ObservableProperty] private decimal _desiredWeight;
    
    private readonly HttpClient _httpClient = new();
    
    // Toast for loading alert
    private readonly IToast _loadingAlert = Toast.Make("Please wait...", CommunityToolkit.Maui.Core.ToastDuration.Long, 14);

    [RelayCommand]
    private async void SignUp()
    {
        try
        {
            // Show the loading alert
            await _loadingAlert.Show();
            var registerSuccessful = await RegisterUserAsync();

            if (registerSuccessful)
            {
                await _loadingAlert.Dismiss();
                await Shell.Current.GoToAsync("///main");
            }
            else
            {
                Console.WriteLine("Something went wrong");
                var alert = Toast.Make($"Something went wrong\nPlease check your internet connection", CommunityToolkit.Maui.Core.ToastDuration.Long, 14);
                await alert.Show();
            }
        }
        catch (Exception ex)
        {
            var alert = Toast.Make($"Error connecting to server: {ex.Message}", CommunityToolkit.Maui.Core.ToastDuration.Long, 14);
            await alert.Show();
        }
    }
    
    [RelayCommand]
    private async void Cancel()
    {
        await Shell.Current.GoToAsync("///login");
    }

    #region BackendCalls
    //POST on register
    private async Task<bool> RegisterUserAsync()
    {
        var userData = new UserDto
        {
            Username = Username,
            PasswordHash = Password,
            DesiredWeight = DesiredWeight,
            Gender = Gender,
            Height = Height,
            InitialWeight = Weight
        };

        var jsonLoginData = JsonConvert.SerializeObject(userData);
        var requestContent = new StringContent(jsonLoginData, Encoding.UTF8, "application/json");

        try
        {
            var response = await _httpClient.PostAsync("https://weightwizard.azurewebsites.net/api/Users/register", requestContent);
        
            if (!response.IsSuccessStatusCode)
            {
                // Handle unsuccessful response here, e.g., display error message or take appropriate action
                return false;
            }

            var responseContent = await response.Content.ReadAsStringAsync();

            if (string.IsNullOrEmpty(responseContent))
            {
                // Handle empty response content here, e.g., display error message or take appropriate action
                return false;
            }
            
            await SecureStorage.Default.SetAsync("jwt_token", responseContent);
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error");
            // Handle exceptions here, e.g., display error message or take appropriate action
            return false;
        }
    }
    #endregion
}