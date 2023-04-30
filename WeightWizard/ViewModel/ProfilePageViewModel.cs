using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Plugin.LocalNotification;

namespace WeightWizard.ViewModel;

public partial class ProfilePageViewModel: ObservableObject
{
    [ObservableProperty]
    private bool _allowNotificationsIsChecked = true;
    
    // Command to handle selection change
    [RelayCommand]
    public async void SaveChanges()
    {
        //Do something with goal weight...
        
        
        //Check if user has un-/checked notifications
        if (!AllowNotificationsIsChecked) return;
        var request = new NotificationRequest()
        {
            NotificationId = 1,
            BadgeNumber = 1,
            Title = "Weight Wizard",
            Description = "Remember to log your Data, to unlock weekly Report",
            Schedule = new NotificationRequestSchedule()
            {
                //NotifyTime = DateTime.Now.AddSeconds(5)
                NotifyTime = DateTime.Today.AddHours(21),
                RepeatType = NotificationRepeat.Daily
            }
        };
        await LocalNotificationCenter.Current.Show(request);
    }
}