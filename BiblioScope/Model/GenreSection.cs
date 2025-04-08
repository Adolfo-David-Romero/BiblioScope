namespace BiblioScope.Model;

/// <summary>GenreSection used to model genre row in home view UI</summary>
public class GenreSection
{
    public string Genre { get; set; }
    public List<Book> Books { get; set; }
}