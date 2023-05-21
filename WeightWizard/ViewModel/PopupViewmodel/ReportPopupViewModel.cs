using CommunityToolkit.Mvvm.ComponentModel;
using WeightWizard.Model;
using WeightWizard.Model.Interfaces;

namespace WeightWizard.ViewModel.PopupViewmodel;

public partial class ReportPopupViewModel : ObservableObject
{
    
    [ObservableProperty] private string _weeklyreport;
    [ObservableProperty] private int _calories;
    [ObservableProperty] private int _steps;

    public ReportPopupViewModel(ICalenderItems selectedItem)
    {
        initialize(selectedItem);
    }

    private void initialize( ICalenderItems selectedItem) 
    {
        var temp = selectedItem as ReportModel;
        if (temp != null)
        {
            if(temp.ReportDays.First().MorningWeight<temp.ReportDays.Last().MorningWeight)
            {
                Weeklyreport = "you gained "+ (temp.ReportDays.Last().MorningWeight-temp.ReportDays.First().MorningWeight) + "kg";
            }
            else if(temp.ReportDays.First().MorningWeight > temp.ReportDays.Last().MorningWeight)
            {
                Weeklyreport = "you lost "+ (temp.ReportDays.First().MorningWeight - temp.ReportDays.Last().MorningWeight) + " kg";
            }
            else
            {
                Weeklyreport = "There has been no change i weight";
            }

        }
    }
}