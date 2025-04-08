using System.Collections.ObjectModel;
using System.ComponentModel;
using BiblioScope.Model;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Firebase.Auth;

namespace BiblioScope.ViewModel;

public partial class LibraryViewModel : ObservableObject
{
    
    private readonly FirebaseAuthClient _authClient;
    private FirestoreService _firestoreService;

    public ObservableCollection<Book> Books => UserLibrary.Instance.Books;

    public LibraryViewModel(FirebaseAuthClient authClient)
    {
        _authClient = authClient;

        var user = _authClient.User;
        if (user != null && user.Info != null)
        {
            Task.Run(async () =>
            {
                try
                {
                    var freshToken = await user.GetIdTokenAsync(); // ✅ fetch new token
                    var uid = user.Info.Uid;

                    Console.WriteLine($"[DEBUG] Initializing FirestoreService: UID={uid}, Token Exists={freshToken != null}");

                    _firestoreService = new FirestoreService(uid, freshToken);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error: Failed to get Firebase ID token: {ex.Message}");
                }
            });
        }
        else
        {
            Console.WriteLine("Error: User or credential is null. FirestoreService not initialized.");
        }
    }

    [RelayCommand]
    public async Task AddBook(Book book)
    {
        if (book == null) return;

        if (UserLibrary.Instance.Contains(book))
        {
            await Shell.Current.DisplayAlert("Already Added", "This book is already in your library.", "OK");
        }
        else
        {
            UserLibrary.Instance.AddBook(book);
            await Shell.Current.DisplayAlert("Added", $"\"{book.Title}\" has been added to your library!", "Nice!");
            Console.WriteLine($"[DEBUG] Trying to save to Firestore: {book.Title}");
            if (_firestoreService != null)
            {
                await _firestoreService.SaveBookAsync(book);
            }
            else
            {
                Console.WriteLine(" _firestoreService is null - not saving.");
            }
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
        }
    }
}