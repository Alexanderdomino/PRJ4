using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Plugin.LocalNotification;

namespace WeightWizard.ViewModel;

public partial class ProfilePageViewModel: ObservableObject
{
    
    
    // Command to handle selection change
    [RelayCommand]
    public void AllowNotifications()
    {
        var request = new NotificationRequest()
        {
            NotificationId = 1,
            BadgeNumber = 1,
            Title = "Weight Wizard",
            Description = "Remember to log your Data, to unlock weekly Report",
            Schedule = new NotificationRequestSchedule()
            {
                NotifyTime = DateTime.Today.AddHours(21)
            }
        };
    }
}