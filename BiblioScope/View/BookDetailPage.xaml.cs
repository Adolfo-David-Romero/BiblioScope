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
    public UserLibrary UserLibrary { get; set; }

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
        if (SelectedBook == null) return;

        var library = UserLibrary.Instance;

        if (library.Contains(SelectedBook))
        {
            DisplayAlert("Already in Library", "You’ve already added this book.", "OK");
            return;
        }

        library.AddBook(SelectedBook);
        DisplayAlert("Success", $"“{SelectedBook.Title}” was added to your library!", "Nice!");
    }
}