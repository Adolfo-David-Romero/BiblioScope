namespace BiblioScope.Model;

public class Book
{
    // Collections
    public List<string> Genres { get; set; } = new();
    public List<string> Tags { get; set; } = new();
    public List<string> Moods { get; set; } = new();
    
    // Constructors
    public Book() { }

    /// <summary> Represents a singular user book </summary>
    public Book(string isbn, string title, string author, string publisher, List<string> genres)
    {
        Isbn = isbn;
        Title = title;
        Author = author;
        Publisher = publisher;
        Genres = genres;
    }

    // Properties
    public string Isbn { get; set; }
    public string Title { get; set; }
    public string Subtitle { get; set; }
    public string Author { get; set; }
    public string Description { get; set; }
    public string CoverImageUrl { get; set; }
    public string Publisher { get; set; }
    public string SeriesName { get; set; }
    public string ReleaseDate { get; set; } 
    public double Rating { get; set; }
    public int Pages { get; set; }
}