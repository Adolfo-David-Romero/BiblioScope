using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;
using BiblioScope.Model;
using BiblioScope.View;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Firebase.Auth;

namespace BiblioScope.ViewModel;


public partial class LibraryViewModel : ObservableObject
{
    private readonly FirebaseAuthClient _authClient;
    private FirestoreService _firestoreService;

    public ObservableCollection<Book> Books => UserLibrary.Instance.Books;

    public ICommand ViewBookCommand { get; private set; }

    public LibraryViewModel(FirebaseAuthClient authClient)
    {
        _authClient = authClient;

        ViewBookCommand = new Command<Book>(OnViewBook); 

        var user = _authClient.User;
        if (user != null && user.Info != null)
        {
            Task.Run(async () =>
            {
                try
                {
                    var token = await user.GetIdTokenAsync();
                    var uid = user.Info.Uid;
                    _firestoreService = new FirestoreService(uid, token);
                    Console.WriteLine($"[DEBUG] Firestore initialized for {uid}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"[ERROR] Firebase token error: {ex.Message}");
                }
            });
        }
    }

    [RelayCommand]
    public async Task AddBook(Book book)
    {
        if (book == null) return;

        if (UserLibrary.Instance.Contains(book))
        {
            await Shell.Current.DisplayAlert("Already Added", "This book is already in your library.", "OK");
            return;
        }

        UserLibrary.Instance.AddBook(book);
        await Shell.Current.DisplayAlert("Added", $"“{book.Title}” has been added to your library!", "Nice!");

        if (_firestoreService != null)
        {
            await _firestoreService.SaveBookAsync(book);
        }
    }

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
            if (_firestoreService != null)
            {
                await _firestoreService.DeleteBookAsync(book.Isbn);
            }
        }
    }

    private async void OnViewBook(Book book)
    {
        if (book == null) return;

        await Shell.Current.GoToAsync(nameof(View.LibraryBookDetailPage), true, new Dictionary<string, object>
        {
            { "SelectedBook", book }
        });
    }
}