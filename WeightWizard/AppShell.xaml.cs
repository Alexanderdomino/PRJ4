using WeightWizard.View;

namespace WeightWizard;

public partial class AppShell : Shell
{
    public AppShell()
    {
        InitializeComponent();
        
        Routing.RegisterRoute("login",typeof(LoginPage));
        Routing.RegisterRoute("main", typeof(MainPage));
        Routing.RegisterRoute("trend", typeof(MainPage));
        Routing.RegisterRoute("journal", typeof(MainPage));
        Routing.RegisterRoute("profile", typeof(MainPage));
    }
}