using BiblioScope.ViewModel;

namespace BiblioScope.View;

public partial class UserLibraryPage : ContentPage
{
    public LibraryViewModel ViewModel { get; }

    public UserLibraryPage()
    {
        InitializeComponent();
        ViewModel = new LibraryViewModel();
        BindingContext = ViewModel;
    }
}