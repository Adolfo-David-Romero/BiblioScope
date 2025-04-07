using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BiblioScope.ViewModel;

namespace BiblioScope.View;

public partial class SignUpPage : ContentPage
{
    public SignUpPage(SignUpViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}