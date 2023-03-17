using System.Collections.ObjectModel;
using System.Runtime.InteropServices.JavaScript;
using System.Windows.Input;
using WeightWizard_test.Model;
using WeightWizard_test.Model.Interfaces;

namespace WeightWizard_test.ViewModel;

public partial class JournalPageViewModel
{
    public ObservableCollection<ICalenderItems> Dates { get; set; } = new();

    public JournalPageViewModel()
    {
        BindDates(DateTime.Now);
    }

    private void BindDates(DateTime selectedDate)
    {
        int daysCount = DateTime.DaysInMonth(selectedDate.Year, selectedDate.Month);
        var firstDayOfMonth = new DateTime(selectedDate.Year, selectedDate.Month, 1);
        int daysBeforeMonth = (int)firstDayOfMonth.DayOfWeek - 1;

        for (int i = 1; i < 8; i++)
        {
            Dates.Add(new DayNameModel()
            {
                Date = new DateTime(2021, 2, i)
            });

        }
        Dates.Add(new EmptyDayModel());
        
        if (firstDayOfMonth.DayOfWeek != DayOfWeek.Monday)
        {
            for (int spoofDay = 0; spoofDay < daysBeforeMonth; spoofDay++)
            {
                Dates.Add(new EmptyDayModel());
            }
        }
        
        for (int day = 1; day < daysCount; day++)
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
}

