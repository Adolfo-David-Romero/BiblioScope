using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BiblioScope.Model;
using Firebase.Auth;

namespace BiblioScope.View;

[QueryProperty(nameof(SelectedBook), "SelectedBook")]
public partial class LibraryBookDetailPage : ContentPage
{
    public Book SelectedBook { get; set; }

    public LibraryBookDetailPage()
    {
        InitializeComponent();
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        BindingContext = SelectedBook;
    }

    private async void OnDeleteClicked(object sender, EventArgs e)
    {
        bool confirm = await DisplayAlert("Confirm Delete", $"Remove \"{SelectedBook.Title}\" from your library?", "Yes", "Cancel");
        if (confirm)
        {
            UserLibrary.Instance.RemoveBook(SelectedBook);
            await Shell.Current.GoToAsync("..");
        }
    }
}