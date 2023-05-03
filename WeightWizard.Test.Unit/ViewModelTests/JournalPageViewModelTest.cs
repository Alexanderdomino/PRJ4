using WeightWizard.Model;
using WeightWizard.ViewModel;

namespace WeightWizard.Test.Unit.ViewModelTests;

[TestFixture]
public class JournalPageViewModelTests
{
    private JournalPageViewModel _viewModel;

    [SetUp]
    public void SetUp()
    {
        _viewModel = new JournalPageViewModel();
    }

    [Test]
    public void BindDates_Adds_Days_To_DatesCollection()
    {
        // Arrange
        var selectedMonth = new DateTime(2021, 1, 1);

        // Act
        _viewModel.BindDates(selectedMonth);

        // Assert
        Assert.That(_viewModel.Dates, Is.Not.Null);
        Assert.That(_viewModel.Dates.Count, Is.EqualTo(38));
    }

    [Test]
    public void CurrentDate_Shows_SelectedItem_Popup()
    {
        // Arrange
        var selectedItem = new CalenderModel { Date = DateTime.Now };
        _viewModel.SelectedItem = selectedItem;

        // Act
        _viewModel.CurrentDate();

        // Assert
        //need to find out how to test the mopup popup plugin
    }

    [Test]
    public void MonthSwipeLeft_Changes_SelectedMonth_To_Next_Month()
    {
        // Arrange
        var initialMonth = _viewModel.SelectedMonth;

        // Act
        _viewModel.MonthSwipeLeft();

        // Assert
        Assert.That(_viewModel.SelectedMonth, Is.EqualTo(initialMonth.AddMonths(1)));
    }

    [Test]
    public void MonthSwipeRight_Changes_SelectedMonth_To_Previous_Month()
    {
        // Arrange
        var initialMonth = _viewModel.SelectedMonth;

        // Act
        _viewModel.MonthSwipeRight();

        // Assert
        Assert.That(_viewModel.SelectedMonth, Is.EqualTo(initialMonth.AddMonths(-1)));
    }
}