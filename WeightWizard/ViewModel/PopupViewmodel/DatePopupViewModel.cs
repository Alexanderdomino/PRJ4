using CommunityToolkit.Mvvm.ComponentModel;
using WeightWizard.Model;

namespace WeightWizard.ViewModel.PopupViewmodel;

public partial class DatePopupViewModel : ObservableObject
{
    // ReSharper disable once InconsistentNaming
    [ObservableProperty] private DateTime _selectedDate;
    [ObservableProperty] private decimal _morningWeight;
    [ObservableProperty] private int _calories;
    [ObservableProperty] private int _steps;
    
    public DatePopupViewModel(CalenderModel selectedItem)
    {
        BindItem(selectedItem);
    }

    private void BindItem(CalenderModel selectedItem)
    {
        SelectedDate = selectedItem.Date;
        MorningWeight = selectedItem.MorningWeight;
        Calories = selectedItem.CalorieIntake;
        Steps = selectedItem.Steps;
    }
}