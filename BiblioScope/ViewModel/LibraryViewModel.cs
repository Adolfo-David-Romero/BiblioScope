using System.Collections.ObjectModel;
using System.ComponentModel;
using BiblioScope.Model;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace BiblioScope.ViewModel;

public partial class LibraryViewModel : ObservableObject
{
    public ObservableCollection<Book> Books => UserLibrary.Instance.Books;

    [RelayCommand]
    public async Task RemoveBook(Book book)
    {
        if (book == null) return;

        bool confirm = await Shell.Current.DisplayAlert(
            "Remove Book",
            $"Are you sure you want to remove \"{book.Title}\"?",
            "Remove", "Cancel");

        if (confirm)
        {
            UserLibrary.Instance.RemoveBook(book);
        }
    }

    [RelayCommand]
    public void AddBook(Book book)
    {
        if (book == null) return;

        if (UserLibrary.Instance.Contains(book))
        {
            Shell.Current.DisplayAlert("Already Added", "This book is already in your library.", "OK");
        }
        else
        {
            UserLibrary.Instance.AddBook(book);
            Shell.Current.DisplayAlert("Added", $"\"{book.Title}\" has been added to your library!", "OK");
        }
    }
}