using System.Text;
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

    [RelayCommand]
    private async void SignUp()
    {
        try
        {
            var registerSuccessful = await RegisterUserAsync();

            if (registerSuccessful)
            {
                await Shell.Current.GoToAsync("///main");
            }
            else
            {
                Console.WriteLine("Something went wrong");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error connecting to server: {ex.Message}");
        }
    }
    
    [RelayCommand]
    private async void Cancel()
    {
        await Shell.Current.GoToAsync("///login");
    }
    
    
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
            var response = await _httpClient.PostAsync("https://prj4backend.azurewebsites.net/api/Users/register", requestContent);
        
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
}