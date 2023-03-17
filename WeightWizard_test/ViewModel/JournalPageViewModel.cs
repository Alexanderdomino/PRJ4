using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using WeightWizard_test.Model;
using WeightWizard_test.Model.Interfaces;

namespace WeightWizard_test.ViewModel;

public partial class JournalPageViewModel : ObservableObject
{

    [ObservableProperty] private DateTime selectedDate = DateTime.Now;
    
    
    
    public ObservableCollection<ICalenderItems> Dates { get; set; } = new();

    public JournalPageViewModel()
    {
        BindDates(DateTime.Now);
    }

    private void BindDates(DateTime selectedDate)
    {
        var daysCount = DateTime.DaysInMonth(selectedDate.Year, selectedDate.Month);
        var firstDayOfMonth = new DateTime(selectedDate.Year, selectedDate.Month, 1);
        var daysBeforeMonth = (int)firstDayOfMonth.DayOfWeek - 1;

        for (var i = 1; i < 8; i++)
        {
            Dates.Add(new DayNameModel()
            {
                Date = new DateTime(2021, 2, i)
            });

        }
        Dates.Add(new EmptyDayModel());
        
        if (firstDayOfMonth.DayOfWeek != DayOfWeek.Monday)
        {
            for (var spoofDay = 0; spoofDay < daysBeforeMonth; spoofDay++)
            {
                Dates.Add(new EmptyDayModel());
            }
        }
        
        for (var day = 1; day < daysCount; day++)
        {
            Dates.Add(new CalenderModel
            {
                Date = new DateTime(selectedDate.Year, selectedDate.Month, day)
            });
            if ((day+daysBeforeMonth)%7 == 0)
            {
                Dates.Add(new ReportModel());
            }
        }
    }

    [RelayCommand]
    private void CurrentDate(CalenderModel currentDate)
    {
        SelectedDate = currentDate.Date;
    }
    
}

