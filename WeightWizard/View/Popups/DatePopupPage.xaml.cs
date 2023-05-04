using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeightWizard.Model;
using WeightWizard.ViewModel.PopupViewmodel;

namespace WeightWizard.View.Popups;

public partial class DatePopupPage
{
    public DatePopupPage(CalenderModel selectedItem)
    {
        InitializeComponent();
        BindingContext = new DatePopupViewModel(selectedItem);
    }
}