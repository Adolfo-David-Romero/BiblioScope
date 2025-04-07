using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Firebase.Auth;

namespace BiblioScope.ViewModel;

public partial class SignUpViewModel: ObservableObject
{
    //Inject Firebase Auth Client
    private readonly FirebaseAuthClient _authClient;
    
    [ObservableProperty]
    private string _email;
    
    [ObservableProperty]
    private string _name;
    
    [ObservableProperty]
    private string _password;
    
    public SignUpViewModel(FirebaseAuthClient authClient)
    {
        _authClient = authClient;
    }
    
    [RelayCommand]
    private async Task SignUp()
    {
        await _authClient.CreateUserWithEmailAndPasswordAsync(Email, Password, Name);
        await Shell.Current.GoToAsync("//SignInPage");
        
    }
    
    [RelayCommand]
    private async Task NavigateSignIn()
    {
        await Shell.Current.GoToAsync("//SignInPage");
        
    }
}