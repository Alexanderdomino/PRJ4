using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeightWizard.ViewModel.PopupViewmodel;

namespace WeightWizard.View.Popups;

public partial class DatePopupPage
{
    public DatePopupPage(DateTime selectedItem)
    {
        InitializeComponent();
        BindingContext = new DatePopupViewModel(selectedItem);
    }
}