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
    private readonly FirebaseAuthClient _authClient;
    public LibraryBookDetailPage(FirebaseAuthClient authClient)
    {
        InitializeComponent();
        _authClient = authClient;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        BindingContext = SelectedBook;
    }

    private async void OnDeleteClicked(object sender, EventArgs e)
    {
        bool confirm = await DisplayAlert(
            "Confirm Delete", 
            $"Remove \"{SelectedBook.Title}\" from your library?", 
            "Yes", "Cancel");

        if (!confirm) return;

        try
        {
            // Remove locally
            UserLibrary.Instance.RemoveBook(SelectedBook);

            // Get Firebase user/token
            var authClient = _authClient;
            var user = authClient?.User;
            var token = await user?.GetIdTokenAsync()!;
            var uid = user?.Info?.Uid;

            if (!string.IsNullOrEmpty(token) && !string.IsNullOrEmpty(uid))
            {
                var firestoreService = new FirestoreService(uid, token);
                await firestoreService.DeleteBookAsync(SelectedBook.Isbn);
                await DisplayAlert("Deleted", $"Deleted Book: {SelectedBook.Title}.", "OK");
                Console.WriteLine($"[Firestore] Deleted: {SelectedBook.Title}");
            }
            else
            {
                Console.WriteLine("[Firestore] Delete failed - missing auth.");
            }

            // Navigate back
            await Shell.Current.GoToAsync("..");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[Delete Error] {ex.Message}");
            await DisplayAlert("Error", "Failed to delete book.", "OK");
        }
    }

}