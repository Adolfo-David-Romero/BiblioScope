using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using BiblioScope.Model;
using BiblioScope.View;

namespace BiblioScope.ViewModel;

public class SearchViewModel: INotifyPropertyChanged
{
    private readonly HardcoverBookService _bookService;

    public ObservableCollection<Book> SearchResults { get; set; } = new();

    private string _searchQuery;
    public string SearchQuery
    {
        get => _searchQuery;
        set
        {
            _searchQuery = value;
            OnPropertyChanged();
        }
    }

    private bool _isBusy;
    public bool IsBusy
    {
        get => _isBusy;
        set
        {
            _isBusy = value;
            OnPropertyChanged();
        }
    }

    public ICommand SearchCommand { get; }
    public ICommand BookSelectedCommand { get; }

    public SearchViewModel()
    {
        _bookService = new HardcoverBookService();
        SearchCommand = new Command(async () => await PerformSearchAsync());
        BookSelectedCommand = new Command<Book>(OnBookSelected);
    }

    private async Task PerformSearchAsync()
    {
        if (string.IsNullOrWhiteSpace(SearchQuery))
            return;

        IsBusy = true;

        try
        {
            SearchResults.Clear();

            var result = await _bookService.SearchBooksAsync(SearchQuery, 1, 5);
            var hits = result?.data?.search?.results?.hits;

            if (hits != null)
            {
                foreach (var hit in hits)
                {
                    var doc = hit.document;

                    if (doc != null)
                    {
                        var book = BookMapper.FromDocument(doc);
                        SearchResults.Add(book);
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[ERROR] Search failed: {ex.Message}");
        }
        finally
        {
            IsBusy = false;
        }
    }

    private async void OnBookSelected(Book selectedBook)
    {
        if (selectedBook == null) return;

        await Shell.Current.GoToAsync(nameof(BookDetailPage), true, new Dictionary<string, object>
        {
            { "SelectedBook", selectedBook }
        });
    }

    public event PropertyChangedEventHandler PropertyChanged;
    protected void OnPropertyChanged([CallerMemberName] string name = null)
        => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
}