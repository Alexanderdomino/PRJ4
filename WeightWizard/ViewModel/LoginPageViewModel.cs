using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace WeightWizard.ViewModel;

public partial class LoginPageViewModel : ObservableObject
{
    [ObservableProperty] private string _username;
    [ObservableProperty] private string _password;
    
    [RelayCommand]
    private async void SignIn()
    {
        if (Username == null || Password == null)
        {
            Console.WriteLine("no username or password");
        }   
        else await Shell.Current.GoToAsync("///main");
    }
}