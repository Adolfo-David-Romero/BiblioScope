using BiblioScope.View;

namespace BiblioScope
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            //Trouble making page routing 
            Routing.RegisterRoute(nameof(BookDetailPage), typeof(BookDetailPage));
            Routing.RegisterRoute(nameof(LibraryBookDetailPage), typeof(LibraryBookDetailPage));
            Routing.RegisterRoute(nameof(PossibleMatchesPage), typeof(PossibleMatchesPage));
            
        }
    }
}
