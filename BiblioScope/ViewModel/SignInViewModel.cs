using BiblioScope.Model;
using BiblioScope.View;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Firebase.Auth;
using BiblioScope.View;

namespace BiblioScope.ViewModel;
//Source --> https://www.youtube.com/watch?v=3DQMQ9Vuk0c&t=167s
/// <summary> View Model that mediates authentication (SignIn) functionality with UI </summary>
public partial class SignInViewModel: ObservableObject //Utilizes the "ComunityToolkit.Mvvm" Package for unique VM implimentation
{
    //Inject Firebase Auth Client
    private readonly FirebaseAuthClient _authClient;
    
    [ObservableProperty] private string _email;
    
    [ObservableProperty] private string _password;
    
    [ObservableProperty] private bool _isBusy; //flag
    
    public string Name => _authClient.User?.Info?.DisplayName ?? "Reader";
    
    public SignInViewModel(FirebaseAuthClient authClient)
    {
        _authClient = authClient;
    }
    
    [RelayCommand]
    private async Task SignIn()
    {
        Console.WriteLine("SignIn Triggered");
        if (string.IsNullOrWhiteSpace(Email) || string.IsNullOrWhiteSpace(Password))
        {
            await Shell.Current.DisplayAlert("Missing Fields", "Please enter both email and password.", "OK");
            return;
        }

        IsBusy = true;

        try
        {
            await _authClient.SignInWithEmailAndPasswordAsync(Email, Password);
            var user = _authClient.User;

            if (user != null)
            {
                var idToken = await user.GetIdTokenAsync();
                var uid = user.Info.Uid;

                var firestoreService = new FirestoreService(uid, idToken);
                var books = await firestoreService.GetUserBooksAsync();

                
                // Clear and reload local UserLibrary
                var library = UserLibrary.Instance;
                library.Books.Clear();
                foreach (var book in books)
                {
                    library.AddBook(book); // avoids duplicates
                }

                Console.WriteLine($"[SignIn] Loaded {books.Count} books from Firestore.");
            }

            await Shell.Current.GoToAsync("//HomePage");
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlert("Login Failed", ex.Message, "OK");
            Console.WriteLine($"[SignIn] Error: {ex.Message}");
        }
        finally
        {
            IsBusy = false;
        }
    }

    [RelayCommand]
    private void SignOut()
    {
        Console.WriteLine("SignOut Triggered");
        //PasswordEntry.Text = "";
        _authClient.SignOut();
        Shell.Current.GoToAsync("//SignInPage");
    }
    
    [RelayCommand]
    public async Task DeleteAccountAsync()
    {
        Console.WriteLine("Delete Account Triggered");
        var user = _authClient.User;

        if (user != null)
        {
            try
            {
                await user.DeleteAsync();
                await Shell.Current.DisplayAlert("Account Deleted", "Your account has been permanently removed.", "OK");
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Account Deletion Failed:", ex.Message, "OK");
            }
            finally
            {
                IsBusy = false;
            }

            // Navigate to auth flow
            await Shell.Current.GoToAsync("//SignInPage");
        }
    }

    
    [RelayCommand]
    private async Task NavigateSignUp()
    {
        await Shell.Current.GoToAsync("//SignUpPage");
    }
}