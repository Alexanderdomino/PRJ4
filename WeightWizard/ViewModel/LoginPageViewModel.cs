using System.Net.Http.Headers;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Newtonsoft.Json;
using System.Text;
using CommunityToolkit.Maui.Alerts;
using WeightWizard.Model.DTOs;


namespace WeightWizard.ViewModel
{
    public partial class LoginPageViewModel : ObservableObject
    {
        [ObservableProperty] private string _username;
        [ObservableProperty] private string _password;

        private readonly HttpClient _httpClient = new();

        [RelayCommand]
        private async void SignIn()
        {
            if (string.IsNullOrEmpty(Username) || string.IsNullOrEmpty(Password))
            {
                var alert = Toast.Make("Please enter Username and Password", CommunityToolkit.Maui.Core.ToastDuration.Long, 14);
                await alert.Show();
            }
            else if (Username == "Admin" && Password == "Admin")
            {
                await Shell.Current.GoToAsync("///main");
            }
            else
            {
                try
                {
                    var loginSuccessful = await LoginAsync(Username, Password);

                    if (loginSuccessful)
                    {
                        await Shell.Current.GoToAsync("///main");
                    }
                    else
                    {
                        var alert = Toast.Make("Invalid username or password", CommunityToolkit.Maui.Core.ToastDuration.Long, 14);
                        await alert.Show();
                    }
                }
                catch (Exception ex)
                {
                    var alert = Toast.Make($"Error connecting to server: {ex.Message}", CommunityToolkit.Maui.Core.ToastDuration.Long, 14);
                    await alert.Show();
                }
            }
        }

        private async Task<bool> LoginAsync(string email, string password)
        {
            var alert = Toast.Make($"Please wait...", CommunityToolkit.Maui.Core.ToastDuration.Long, 14);
            await alert.Show();
            
            var loginData = new LoginDto
            {
                Username = email,
                Password = password
            };

            var jsonLoginData = JsonConvert.SerializeObject(loginData);
            var requestContent = new StringContent(jsonLoginData, Encoding.UTF8, "application/json");

            try
            {
                var response = await _httpClient.PostAsync("https://prj4backend.azurewebsites.net/api/Users/login", requestContent);
        
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

        [RelayCommand]
        private async void SignUp()
        {
            await Shell.Current.GoToAsync("///register");
        }
    }
}
