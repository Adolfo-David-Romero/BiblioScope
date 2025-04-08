using System.Collections.ObjectModel;
using System.Text.Json;

namespace BiblioScope.Model;

/// <summary> Represents a user's library of books </summary>
public class UserLibrary
{
    public ObservableCollection<Book> Books { get; set; }

    // Singleton instance
    private static UserLibrary _instance;
    public static UserLibrary Instance => _instance ??= new UserLibrary();

    private UserLibrary()
    {
        Books = new ObservableCollection<Book>();
        Console.WriteLine("Loading Books...");
    }

    public void AddBook(Book book)
    {
        if (!Contains(book))
        {
            Books.Add(book);
            Console.WriteLine($"Added book: {book.Title}");
        }
        else
        {
            Console.WriteLine($"Book '{book.Title}' is already in the library.");
        }
    }

    public void RemoveBook(Book book)
    {
        if (book != null)
        {
            Books.Remove(book);
            Console.WriteLine($"Removed book: {book.Title}");
        }else{Console.WriteLine($"Book '{book.Title}' is not in the library.");}
    }

    public bool Contains(Book book)
    {
        return Books.Any(b => b.Isbn == book.Isbn);
    }

    public Book? GetBookByTitle(string title)
    {
        var match = Books.FirstOrDefault(b => b.Title.Equals(title, StringComparison.OrdinalIgnoreCase));
        Console.WriteLine(match != null
            ? $"Found book: {match.Title}"
            : $"Book with title '{title}' not found.");
        return match;
    }

    // Filtering and sorting
    public IEnumerable<Book> FindByGenre(string genre) =>
        Books.Where(b => b.Genres.Contains(genre, StringComparer.OrdinalIgnoreCase));

    public IEnumerable<Book> SortedByTitle() =>
        Books.OrderBy(b => b.Title);

    // Local storage methods
    public async Task SaveToFileAsync(string filePath)
    {
        var json = JsonSerializer.Serialize(Books);
        await File.WriteAllTextAsync(filePath, json);
    }

    public async Task LoadFromFileAsync(string filePath)
    {
        if (!File.Exists(filePath)) return;

        var json = await File.ReadAllTextAsync(filePath);
        var loadedBooks = JsonSerializer.Deserialize<List<Book>>(json);
        Books.Clear();
        foreach (var book in loadedBooks)
            Books.Add(book);
    }
}