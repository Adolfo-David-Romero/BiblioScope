namespace BiblioScope.Model;

/// <summary> Maps HardcoverSearchResponse to user book class</summary>
public class BookMapper
{
    public static Book FromDocument(Document doc)
    {
        return new Book
        {
            Isbn = doc.isbns?.FirstOrDefault() ?? "N/A",
            Title = doc.title,
            Subtitle = doc.subtitle,
            Author = doc.author_names?.FirstOrDefault() ?? "Unknown",
            Description = doc.description,
            CoverImageUrl = doc.image?.url,
            Publisher = doc.contributions?.FirstOrDefault()?.author?.name, 
            SeriesName = doc.series_names?.FirstOrDefault()?.ToString() ?? "",
            ReleaseDate = doc.release_date,
            Rating = doc.rating,
            Pages = doc.pages,
            Genres = doc.genres?.ToList() ?? new List<string>(),
            Tags = doc.tags?.ToList() ?? new List<string>(),
            Moods = doc.moods?.ToList() ?? new List<string>()
        };
    }
}