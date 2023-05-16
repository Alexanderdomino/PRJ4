using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeightWizard.Model;
using WeightWizard.Model.Interfaces;
using WeightWizard.ViewModel.PopupViewmodel;

namespace WeightWizard.View.Popups;

public partial class DatePopupPage
{
    public DatePopupPage(ICalenderItems selectedItem)
    {
        InitializeComponent();
        BindingContext = new DatePopupViewModel(selectedItem);
    }
}