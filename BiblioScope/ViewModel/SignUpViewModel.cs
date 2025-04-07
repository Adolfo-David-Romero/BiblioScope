using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Firebase.Auth;

namespace BiblioScope.ViewModel;

public partial class SignUpViewModel: ObservableObject
{
    //Inject Firebase Auth Client
    private readonly FirebaseAuthClient _authClient;
    
    [ObservableProperty] private string _email;
    
    [ObservableProperty] private string _name;
    
    [ObservableProperty] private string _password;
    
    [ObservableProperty] private bool _isBusy; //flag
    
    public SignUpViewModel(FirebaseAuthClient authClient)
    {
        _authClient = authClient;
    }
    
    [RelayCommand]
    private async Task SignUp()
    {
        if (string.IsNullOrWhiteSpace(Email) || string.IsNullOrWhiteSpace(Password) || string.IsNullOrWhiteSpace(Name))
        {
            await Shell.Current.DisplayAlert("Missing Fields", "All fields are required.", "OK");
            return;
        }

        IsBusy = true;

        try
        {
            await _authClient.CreateUserWithEmailAndPasswordAsync(Email, Password, Name);
            await Shell.Current.DisplayAlert("Account Created", "You can now sign in.", "OK");
            await Shell.Current.GoToAsync("//SignInPage");
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlert("Sign Up Failed", ex.Message, "OK");
        }
        finally
        {
            IsBusy = false;
        }
    }
    
    [RelayCommand]
    private async Task NavigateSignIn()
    {
        await Shell.Current.GoToAsync("//SignInPage");
        
    }
}