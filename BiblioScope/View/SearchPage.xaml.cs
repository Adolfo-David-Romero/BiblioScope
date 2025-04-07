using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using BiblioScope.Model;
using BiblioScope.ViewModel;

namespace BiblioScope.View;

public partial class SearchPage : ContentPage
{
    private readonly HardcoverBookService _bookService;

    public SearchPage()
    {
        InitializeComponent();
        BindingContext = new SearchViewModel();
    }

    private void OnSearchButtonPressed(object? sender, EventArgs e)
    {
        if (BindingContext is SearchViewModel vm && vm.SearchCommand.CanExecute(null))
        {
            vm.SearchCommand.Execute(null);
        }
    }
}