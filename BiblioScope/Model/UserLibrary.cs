using System.Collections.ObjectModel;

namespace BiblioScope.Model;

public class UserLibrary
{
    private ObservableCollection<Book> Books { get; set; }

    /// <summary> Represents a user's library of books </summary>
    public UserLibrary()
    {
        Books = new ObservableCollection<Book>(); //Creates a new book collection 
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

    public Book GetBookByTitle(string title)
    {
        foreach (var book in Books)
        {
            if (book.Title != title) continue;
            Console.WriteLine($"Found book: {book.Title}");
            return book;

        }
        throw new Exception($"Book with title: {title}, NOT found");
    }
}