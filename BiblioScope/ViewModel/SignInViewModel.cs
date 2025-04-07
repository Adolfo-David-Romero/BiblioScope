using BiblioScope.View;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Firebase.Auth;

namespace BiblioScope.ViewModel;
//Source --> https://www.youtube.com/watch?v=3DQMQ9Vuk0c&t=167s
/// <summary> View Model that mediates authentication (SignIn) functionality with UI </summary>
public partial class SignInViewModel: ObservableObject //Utilizes the "ComunityToolkit.Mvvm" Package for unique VM implimentation
{
    //Inject Firebase Auth Client
    private readonly FirebaseAuthClient _authClient;
    
    [ObservableProperty]
    private string _email;
    
    [ObservableProperty]
    private string _password;
    
    public SignInViewModel(FirebaseAuthClient authClient)
    {
        _authClient = authClient;
    }
    
    [RelayCommand]
    private async Task SignIn()
    {
       await _authClient.SignInWithEmailAndPasswordAsync(Email, Password);
       await Shell.Current.GoToAsync("//HomePage");
    }
    
    [RelayCommand]
    private async Task NavigateSignUp()
    {
        await Shell.Current.GoToAsync("//SignUpPage");
    }
}