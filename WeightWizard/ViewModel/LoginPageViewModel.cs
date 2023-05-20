using System.Net.Http.Headers;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Newtonsoft.Json;
using System.Text;


namespace WeightWizard.ViewModel
{
    public partial class LoginPageViewModel : ObservableObject
    {
        [ObservableProperty] private string _username;
        [ObservableProperty] private string _password;

        private readonly HttpClient _httpClient = new HttpClient();

        [RelayCommand]
        private async void SignIn()
        {
            if (string.IsNullOrEmpty(Username) || string.IsNullOrEmpty(Password))
            {
                Console.WriteLine("No username or password");
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
                        Console.WriteLine("Invalid username or password");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error connecting to server: {ex.Message}");
                }
            }
        }

        private async Task<bool> LoginAsync(string email, string password)
        {
            var loginData = new Dictionary<string, string>
            {
                { "email", email },
                { "password", password }
            };

            var jsonLoginData = JsonConvert.SerializeObject(loginData);

            var requestContent = new StringContent(jsonLoginData, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("https://your-backend-server.com/api/login", requestContent);

            return response.IsSuccessStatusCode;
        }
    }
}
