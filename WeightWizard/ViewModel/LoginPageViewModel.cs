using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace WeightWizard.ViewModel;

public partial class LoginPageViewModel : ObservableObject
{
    
    [RelayCommand]
    private async void SignIn()
    {
        await Shell.Current.GoToAsync("///main");
    }
}