using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BiblioScope.Model;
using Firebase.Auth;

namespace BiblioScope.View;

[QueryProperty(nameof(SelectedBook), "SelectedBook")]
public partial class BookDetailPage : ContentPage
{
    private readonly FirebaseAuthClient _authClient;
    public Book SelectedBook { get; set; }
    public UserLibrary UserLibrary { get; set; }

    public BookDetailPage(FirebaseAuthClient authClient)
    {
        base.OnAppearing();
        InitializeComponent();
        _authClient = authClient;
    }
    protected override void OnAppearing()
    {
        base.OnAppearing();
        BindingContext = SelectedBook;
    }
    private async void OnSaveClicked(object sender, EventArgs e)
    {
        if (SelectedBook == null) return;

        var library = UserLibrary.Instance;

        if (library.Contains(SelectedBook))
        {
            await DisplayAlert("Already in Library", "You’ve already added this book.", "OK");
            return;
        }

        library.AddBook(SelectedBook);
        await DisplayAlert("Success", $"“{SelectedBook.Title}” was added to your library!", "Nice!");
        
        try
        {
            var authClient = _authClient;
            var user = authClient?.User;
            var token = await user?.GetIdTokenAsync()!;
            var uid = user?.Info?.Uid;

            if (!string.IsNullOrEmpty(token) && !string.IsNullOrEmpty(uid))
            {
                var firestoreService = new FirestoreService(uid, token);
                await firestoreService.SaveBookAsync(SelectedBook);
                Console.WriteLine($"Firestore: Saved {SelectedBook.Title}");
            }
            else
            {
                Console.WriteLine("FirestoreService not initialized - user/token missing.");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Failed to save book to Firestore: {ex.Message}");
        }
    }
}