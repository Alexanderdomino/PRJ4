using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeightWizard.ViewModel;

namespace WeightWizard.View;

public partial class JournalPage : ContentPage
{
    public JournalPage()
    {
        InitializeComponent();
        BindingContext = new JournalPageViewModel();
    }
}