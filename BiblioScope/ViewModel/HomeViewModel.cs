using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using BiblioScope.Model;
using BiblioScope.View;

namespace BiblioScope.ViewModel;

public class HomeViewModel : INotifyPropertyChanged
{
    private readonly HardcoverBookService _bookService = new();
    public ObservableCollection<GenreSection> GenreSections { get; set; } = new();
    public ICommand BookSelectedCommand { get; }

    public HomeViewModel()
    {
        BookSelectedCommand = new Command<Book>(OnBookSelected);
        LoadGenresAsync();
    }

    private async void LoadGenresAsync()
    {
        var genres = new[] { "Horror", "Science Fiction", "Fantasy", "Romance", "Thriller" };

        foreach (var genre in genres)
        {
            var result = await _bookService.SearchBooksAsync(genre, 1, 5);
            var hits = result?.data?.search?.results?.hits;

            if (hits != null)
            {
                var books = hits.Select(hit => BookMapper.FromDocument(hit.document)).ToList();
                GenreSections.Add(new GenreSection { Genre = genre, Books = books });
            }
        }
    }

    private async void OnBookSelected(Book book)
    {
        if (book == null) return;

        await Shell.Current.GoToAsync(nameof(BookDetailPage), true, new Dictionary<string, object>
        {
            { "SelectedBook", book }
        });
    }

    public event PropertyChangedEventHandler PropertyChanged;
    protected void OnPropertyChanged([CallerMemberName] string name = null)
        => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
}