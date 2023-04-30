using CommunityToolkit.Mvvm.ComponentModel;

namespace WeightWizard.ViewModel.PopupViewmodel;

public partial class DatePopupViewModel : ObservableObject
{
    // ReSharper disable once InconsistentNaming
    [ObservableProperty] public DateTime selectedDate;
    public DatePopupViewModel(DateTime selectedItem)
    {
        BindItem(selectedItem);
    }

    public void BindItem(DateTime selectedItem)
    {
        SelectedDate = selectedItem;
    }
}