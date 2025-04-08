using BiblioScope.View;

namespace BiblioScope
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute(nameof(BookDetailPage), typeof(BookDetailPage));
            Routing.RegisterRoute(nameof(LibraryBookDetailPage), typeof(LibraryBookDetailPage));
        }
    }
}
