namespace BiblioScope.Model;

public class Book
{
    //fields
    private string _isbn;
    private string _title;
    private string _author;
    private string _publisher;
    private string _genre;
    
    /// <summary> Represents a singular Book. </summary>
    public Book(string isbn, string title, string author, string publisher, string genre)
    {
        _isbn = isbn;
        _title = title;
        _author = author;
        _publisher = publisher;
        _genre = genre;
    }
    
    //Properties
    public string Isbn
    {
        get => _isbn;
        set => _isbn = value; //TODO: Add exeption handling 
    }

    public string Title
    {
        get => _title;
        set => _title = value;
    }

    public string Author //TODO: Figure out what happens when there's multiple authors
    {
        get => _author;
        set => _author = value;
    }

    public string Publisher
    {
        get => _publisher;
        set => _publisher = value;
    }

    public string Genre
    {
        get => _genre;
        set => _genre = value;
    }
    
    
    
}