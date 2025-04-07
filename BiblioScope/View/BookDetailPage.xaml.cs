using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BiblioScope.Model;

namespace BiblioScope.View;

[QueryProperty(nameof(SelectedBook), "SelectedBook")]
public partial class BookDetailPage : ContentPage
{
    public Book SelectedBook { get; set; }

    public BookDetailPage()
    {
        InitializeComponent();
    }
    protected override void OnAppearing()
    {
        base.OnAppearing();
        BindingContext = SelectedBook;
    }
}