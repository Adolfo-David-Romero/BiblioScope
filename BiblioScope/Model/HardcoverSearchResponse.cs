namespace BiblioScope.Model;

//Copy-pasted full json response and used "Edit > Paste > Paste Special: JSON as classes" to convert to class
/// <summary>API model represents the json response for a book search</summary>
public class HardcoverSearchResponse
{
    public HardcoverData Data { get; set; }
}

public class HardcoverData
{
    public HardcoverSearch Search { get; set; }
}

public class HardcoverSearch
{
    public HardcoverResults Results { get; set; }
}

public class HardcoverResults
{
    public int Found { get; set; }
    public HardcoverHit[] Hits { get; set; }
}

public class HardcoverHit
{
    public HardcoverDocument Document { get; set; }
}

public class HardcoverDocument
{
    public string Title { get; set; }
    public string Subtitle { get; set; }
    public string Description { get; set; }
    public string[] Author_Names { get; set; }
    public HardcoverImage Image { get; set; }
    public string[] Genres { get; set; }
    public string[] Tags { get; set; }
    public string[] Moods { get; set; }
    public string Release_Date { get; set; }
    public double Rating { get; set; }
    public int Pages { get; set; }
    public string[] Series_Names { get; set; }
    public string[] Isbns { get; set; }
}

public class HardcoverImage
{
    public string Url { get; set; }
}