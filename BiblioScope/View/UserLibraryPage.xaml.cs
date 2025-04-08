using BiblioScope.ViewModel;
using Firebase.Auth;

namespace BiblioScope.View;

public partial class UserLibraryPage : ContentPage
{
    //private readonly FirebaseAuthClient _auth;
    public UserLibraryPage(LibraryViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}