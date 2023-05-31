using System.Net.Http.Headers;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Newtonsoft.Json;
using System.Text;
using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using CommunityToolkit.Maui.Core.Views;
using WeightWizard.Model.DTOs;


namespace WeightWizard.ViewModel
{
    public partial class LoginPageViewModel : ObservableObject
    {
        // Observable properties for username and password
        [ObservableProperty] private string _username;
        [ObservableProperty] private string _password;

        // HttpClient instance for network operations
        private readonly HttpClient _httpClient = new();

        // Toast for loading alert
        private readonly IToast _loadingAlert = Toast.Make("Please wait...", CommunityToolkit.Maui.Core.ToastDuration.Long, 14);
        
        // Command for sign in operation
        [RelayCommand]
        private async void SignIn()
        {
            // Checking if the username or password fields are empty
            if (string.IsNullOrEmpty(Username) || string.IsNullOrEmpty(Password))
            {
                var alert = Toast.Make("Please enter Username and Password", CommunityToolkit.Maui.Core.ToastDuration.Long, 14);
                await alert.Show();
            }
            // For admin login
            else if (Username == "Admin" && Password == "Admin")
            {
                await Shell.Current.GoToAsync("///main");
            }
            else
            {
                try
                {
                    // Attempt to login
                    var loginSuccessful = await LoginAsync(Username, Password);

                    // If login is successful, navigate to main page
                    if (loginSuccessful)
                    {
                        await _loadingAlert.Dismiss();
                        await Shell.Current.GoToAsync("///main");
                    }
                    // If login is unsuccessful, show an alert
                    else
                    {
                        var alert = Toast.Make("Invalid username or password", CommunityToolkit.Maui.Core.ToastDuration.Long, 14);
                        await alert.Show();
                    }
                }
                catch (Exception ex)
                {
                    var alert = Toast.Make($"Something bad happened\nPlease check your internet connection", CommunityToolkit.Maui.Core.ToastDuration.Long, 14);
                    await alert.Show();
                }
            }
        }

        // Command for sign up operation
        [RelayCommand]
        private async void SignUp()
        {
            // Navigate to register page
            await Shell.Current.GoToAsync("///register");
        }

        #region BackendCalls
        //POST on login
        private async Task<bool> LoginAsync(string email, string password)
        {
            // Show the loading alert
            await _loadingAlert.Show();
            
            // Create login data DTO
            var loginData = new LoginDto
            {
                Username = email,
                Password = password
            };

            // Convert the DTO to JSON and create a string content object
            var jsonLoginData = JsonConvert.SerializeObject(loginData);
            var requestContent = new StringContent(jsonLoginData, Encoding.UTF8, "application/json");

            try
            {
                // Make the HTTP POST request
                var response = await _httpClient.PostAsync("https://weightwizard.azurewebsites.net/api/Users/login", requestContent);
        
                // Check for success status code in the response
                if (!response.IsSuccessStatusCode)
                {
                    return false;
                }

                // Read the response content
                var responseContent = await response.Content.ReadAsStringAsync();

                // Check if the response content is empty
                if (string.IsNullOrEmpty(responseContent))
                {
                    return false;
                }

                // Store the JWT token securely
                await SecureStorage.Default.SetAsync("jwt_token", responseContent);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error");
                return false;
            }
        }
        #endregion
    }
}
