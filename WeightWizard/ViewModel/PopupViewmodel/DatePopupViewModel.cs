using CommunityToolkit.Mvvm.ComponentModel;
using WeightWizard.Model;
using WeightWizard.Model.Interfaces;

namespace WeightWizard.ViewModel.PopupViewmodel;

public partial class DatePopupViewModel : ObservableObject
{
    // ReSharper disable once InconsistentNaming
    [ObservableProperty] private DateTime _selectedDate;
    [ObservableProperty] private decimal _morningWeight;
    [ObservableProperty] private int _calories;
    [ObservableProperty] private int _steps;
    
    public DatePopupViewModel(ICalenderItems selectedItem)
    {
        BindItem(selectedItem);
    }

    private void BindItem(ICalenderItems selectedItem)
    {
        var temp = selectedItem as CalenderModel;
        if (temp != null)
        {
            SelectedDate = temp.Date;
            MorningWeight = temp.MorningWeight;
            Calories = temp.CalorieIntake;
            Steps = temp.Steps;
            
        }
        
    }
}