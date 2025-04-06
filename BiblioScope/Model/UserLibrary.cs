using System.Collections.ObjectModel;
using System.Text.Json;

namespace BiblioScope.Model;

/// <summary> Represents a user's library of books </summary>
public class UserLibrary
{
    public ObservableCollection<Book> Books { get; set; }
    
    public UserLibrary()
    {
        Books = new ObservableCollection<Book>();
    }

    public void AddBook(Book book)
    {
        Books.Add(book);
        Console.WriteLine($"Added book: {book.Title}");
    }

    public void RemoveBook(Book book)
    {
        Books.Remove(book);
        Console.WriteLine($"Removed book: {book.Title}");
    }

    public Book? GetBookByTitle(string title)
    {
        var match = Books.FirstOrDefault(b => b.Title.Equals(title, StringComparison.OrdinalIgnoreCase));
        if (match != null)
            Console.WriteLine($"Found book: {match.Title}");
        else
            Console.WriteLine($"Book with title '{title}' not found.");
        return match;
    }
    
    //filtering methods
    public IEnumerable<Book> FindByGenre(string genre) =>
        Books.Where(b => b.Genres.Contains(genre, StringComparer.OrdinalIgnoreCase));

    public IEnumerable<Book> SortedByTitle() =>
        Books.OrderBy(b => b.Title);
    
    //used for local storage if i choose to use that later
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