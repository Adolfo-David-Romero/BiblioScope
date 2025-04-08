using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BiblioScope.ViewModel;

namespace BiblioScope.View;



public partial class PossibleMatchesPage : ContentPage
{
    public PossibleMatchesPage()
    {
        InitializeComponent();
        BindingContext = new PossibleMatchesViewModel();
    }
}