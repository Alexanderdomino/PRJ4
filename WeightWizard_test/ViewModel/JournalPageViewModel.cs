using System.Collections.ObjectModel;
using System.Runtime.InteropServices.JavaScript;
using System.Windows.Input;
using WeightWizard_test.Model;
using XCalendar.Core.Models;

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

        for (int day = 1; day < daysCount; day++)
        {
            Dates.Add(new CalenderModel
            {
                Date = new DateTime(selectedDate.Year, selectedDate.Month, day)
            });
        }
    }
}

