using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;
using BiblioScope.Model;
using BiblioScope.View;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Firebase.Auth;

namespace BiblioScope.ViewModel;


public partial class LibraryViewModel : ObservableObject
{
    private readonly FirebaseAuthClient _authClient;
    private FirestoreService _firestoreService;

    
    public ObservableCollection<Book> Books => UserLibrary.Instance.Books;
    public ObservableCollection<Book> FilteredBooks { get; } = new();
    public ObservableCollection<string> TopGenres { get; } = new();

    [ObservableProperty] private string searchText;
    [ObservableProperty] private string selectedGenre;
    
    //Commands
    public ICommand ViewBookCommand { get; private set; }
    public ICommand FilterByGenreCommand { get; }
    public ICommand ClearGenreFilterCommand { get; }

    public LibraryViewModel(FirebaseAuthClient authClient)
    {
        _authClient = authClient;
        ViewBookCommand = new Command<Book>(OnViewBook);
        ClearGenreFilterCommand = new Command(() => SelectedGenre = null);
        
        //changes in genres
        FilterByGenreCommand = new Command<string>(genre =>
        {
            SelectedGenre = genre;
        });
        ClearGenreFilterCommand = new Command(() => SelectedGenre = null);

        // Watch for changes in the user's library
        Books.CollectionChanged += (_, __) =>
        {
            UpdateGenreFilters();
            ApplyFilter();
        };

        foreach (var book in Books)
            FilteredBooks.Add(book);

        InitFirestore();
    }

    private async void InitFirestore()
    {
        try
        {
            var user = _authClient.User;
            var token = await user?.GetIdTokenAsync()!;
            _firestoreService = new FirestoreService(user.Info.Uid, token);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[ERROR] Firestore init failed: {ex.Message}");
        }
    }

    partial void OnSearchTextChanged(string value) => ApplyFilter();
    partial void OnSelectedGenreChanged(string value) => ApplyFilter();

    private void ApplyFilter()
    {
        FilteredBooks.Clear();

        var query = Books.AsEnumerable();

        if (!string.IsNullOrWhiteSpace(SearchText))
        {
            var lower = SearchText.ToLower();
            query = query.Where(b => b.Title.ToLower().Contains(lower) || b.Author.ToLower().Contains(lower));
        }

        if (!string.IsNullOrWhiteSpace(SelectedGenre))
        {
            query = query.Where(b => b.Genres.Contains(SelectedGenre, StringComparer.OrdinalIgnoreCase));
        }

        foreach (var book in query)
            FilteredBooks.Add(book);
    }

    private void UpdateGenreFilters()
    {
        var genres = Books.SelectMany(b => b.Genres)
                          .Distinct(StringComparer.OrdinalIgnoreCase)
                          .OrderBy(g => g)
                          .ToList();

        TopGenres.Clear();
        foreach (var genre in genres)
            TopGenres.Add(genre);
    }

    private async void OnViewBook(Book book)
    {
        if (book == null) return;

        await Shell.Current.GoToAsync(nameof(LibraryBookDetailPage), true, new Dictionary<string, object>
        {
            { "SelectedBook", book }
        });
    }

    [RelayCommand]
    public async Task RemoveBook(Book book)
    {
        if (book == null) return;

        bool confirm = await Shell.Current.DisplayAlert("Remove Book",
            $"Remove \"{book.Title}\" from your library?",
            "Remove", "Cancel");

        if (confirm)
        {
            UserLibrary.Instance.RemoveBook(book);
            if (_firestoreService != null && !string.IsNullOrWhiteSpace(book.Isbn))
                await _firestoreService.DeleteBookAsync(book.Isbn);
        }
    }
}