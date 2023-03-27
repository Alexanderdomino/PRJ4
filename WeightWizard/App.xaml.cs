using WeightWizard.View;

namespace WeightWizard;

public partial class App : Application
{
    public App()
    {
        InitializeComponent();

        MainPage = new LoginPage();
        //MainPage = new AppShell();
    }
}