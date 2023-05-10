using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeightWizard.ViewModel.PopupViewmodel;

namespace WeightWizard.View.Popups;

public partial class ReportPopupPage
{
    public ReportPopupPage()
    {
        InitializeComponent();
        BindingContext = new ReportPopupViewModel();
    }
}