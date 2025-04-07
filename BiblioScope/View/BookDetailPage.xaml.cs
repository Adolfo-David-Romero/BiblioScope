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
        base.OnAppearing();
        InitializeComponent();
    }
    protected override void OnAppearing()
    {
        base.OnAppearing();
        BindingContext = SelectedBook;
    }
    private void OnSaveClicked(object sender, EventArgs e)
    {
        if (SelectedBook != null)
        {
            //UserLibrary.AddBook(SelectedBook);
            DisplayAlert("Saved", $"{SelectedBook.Title} added to your library!", "OK");
        }
    }
}