using System;
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Mopups.Services;
using WeightWizard.Model;
using WeightWizard.Model.Interfaces;
using WeightWizard.View.Popups;

namespace WeightWizard.ViewModel
{
    public partial class JournalPageViewModel : ObservableObject
    {
        // Current selected date
        [ObservableProperty] public CalenderModel selectedItem;

        [ObservableProperty] public DateTime selectedMonth = DateTime.Now;
        
        partial void OnSelectedMonthChanged(DateTime SelectedMonth)
        {
            Dates.Clear();
            BindDates(SelectedMonth);
        }

        // Collection of dates for the calendar
        public ObservableCollection<ICalenderItems> Dates { get; set; } = new();

        // Constructor
        public JournalPageViewModel()
        {
            // Bind dates for the current month
            BindDates(SelectedMonth);
            
        }

        // Method to bind dates to the calendar
        private void BindDates(DateTime selectedDate)
        {
            // Get the number of days in the month
            var daysCount = DateTime.DaysInMonth(selectedDate.Year, selectedDate.Month);

            // Get the first day of the month
            var firstDayOfMonth = new DateTime(selectedDate.Year, selectedDate.Month, 1);

            // Get the number of days before the first day of the month
            var daysBeforeMonth = (int)firstDayOfMonth.DayOfWeek - 1;

            // Add day names to the collection
            for (var i = 1; i < 8; i++)
            {
                Dates.Add(new DayNameModel()
                {
                    Date = new DateTime(2021, 2, i)
                });
            }

            // Add an empty day to the collection
            Dates.Add(new EmptyDayModel());

            // If the first day of the month is not a Monday, add empty days to the collection
            if (firstDayOfMonth.DayOfWeek != DayOfWeek.Monday)
            {
                for (var spoofDay = 0; spoofDay < daysBeforeMonth; spoofDay++)
                {
                    Dates.Add(new EmptyDayModel());
                }
            }

            // Add days of the month to the collection
            for (var day = 1; day < daysCount; day++)
            {
                Dates.Add(new CalenderModel
                {
                    Date = new DateTime(selectedDate.Year, selectedDate.Month, day)
                });

                // Add a report model after every 7 days
                if ((day + daysBeforeMonth) % 7 == 0)
                {
                    Dates.Add(new ReportModel());
                }
            }
        }

        // Command to handle selection change
        [RelayCommand]
        public void CurrentDate()
        {
            // Show popup of selected item
            MopupService.Instance.PushAsync(new DatePopupPage());
        }

        [RelayCommand]
        public void MonthSwipeLeft()
        {
            Console.WriteLine("Swpied Left");
            SelectedMonth = SelectedMonth.AddMonths(1);
        }
        
        [RelayCommand]
        public void MonthSwipeRight()
        {
            Console.WriteLine("Swpied Right");
            SelectedMonth = SelectedMonth.AddMonths(-1);
        }
    }
}
