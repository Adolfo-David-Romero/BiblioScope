using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using BiblioScope.Model;
using BiblioScope.View;
using CommunityToolkit.Mvvm.ComponentModel;

namespace BiblioScope.ViewModel;

public class PossibleMatchesViewModel : INotifyPropertyChanged, IQueryAttributable
{
    private readonly HardcoverBookService _bookService = new();
    public ObservableCollection<Book> SearchResults { get; } = new();
    public ICommand BookSelectedCommand { get; }

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

    public PossibleMatchesViewModel()
    {
        BookSelectedCommand = new Command<Book>(OnBookSelected);
    }

    public async void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        if (query.TryGetValue("Query", out var rawQuery) && rawQuery is string searchText)
        {
            await PerformSearchAsync(searchText);
        }
    }

    private async Task PerformSearchAsync(string query)
    {
        if (string.IsNullOrWhiteSpace(query)) return;

        IsBusy = true;
        SearchResults.Clear();

        try
        {
            var result = await _bookService.SearchBooksAsync(query, 1, 5);
            var hits = result?.data?.search?.results?.hits;

            if (hits != null)
            {
                foreach (var hit in hits)
                {
                    var book = BookMapper.FromDocument(hit.document);
                    SearchResults.Add(book);
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[ERROR] Match search failed: {ex.Message}");
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
