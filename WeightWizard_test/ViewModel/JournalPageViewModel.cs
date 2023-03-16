using System.Collections.ObjectModel;
using System.Runtime.InteropServices.JavaScript;
using System.Windows.Input;
using WeightWizard_test.Model;

namespace WeightWizard_test.ViewModel;

public partial class JournalPageViewModel
{
    public ObservableCollection<CalenderModel> Dates { get; set; } = new();

    public JournalPageViewModel()
    {
        BindDates(DateTime.Now);
    }

    private void BindDates(DateTime selectedDate)
    {
        int daysCount = DateTime.DaysInMonth(selectedDate.Year, selectedDate.Month);
        var firstDayOfMonth = new DateTime(selectedDate.Year, selectedDate.Month, 1);

        if (firstDayOfMonth.DayOfWeek != DayOfWeek.Monday)
        {
            int daysBeforeMonth = (int)firstDayOfMonth.DayOfWeek - 1;
            for (int spoofDay = 0; spoofDay < daysBeforeMonth; spoofDay++)
            {
                Dates.Add(new CalenderModel
                {
                    Date = new DateTime(selectedDate.Year, selectedDate.Month, 30)
                });
            }
        }
        
        for (int day = 1; day < daysCount; day++)
        {
            Dates.Add(new CalenderModel
            {
                Date = new DateTime(selectedDate.Year, selectedDate.Month, day)
            });
        }
    }
}
