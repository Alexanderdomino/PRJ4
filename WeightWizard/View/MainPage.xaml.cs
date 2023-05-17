using WeightWizard.ViewModel;

namespace WeightWizard;

public partial class MainPage : ContentPage
{
    public MainPage()
    {
        InitializeComponent();
        BindingContext = new LoggerPageViewModel();
    }
}