using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeightWizard.ViewModel;

namespace WeightWizard.View;

public partial class RegisterUserPage : ContentPage
{
    public RegisterUserPage()
    {
        InitializeComponent();
        BindingContext = new RegisterUserPageViewModel();
    }
}