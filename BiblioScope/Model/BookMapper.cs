namespace BiblioScope.Model;

/// <summary> Maps HardcoverSearchResponse to user book class</summary>
public class BookMapper
{
    public static Book Map(HardcoverDocument document)
    {
        return new Book
        {
            Title = document.Title,
            Subtitle = document.Subtitle,
            Author = document.Author_Names?.FirstOrDefault() ?? "Unknown",
            Description = document.Description,
            CoverImageUrl = document.Image?.Url ??
                            "https://thumbs.dreamstime.com/z/question-mark-confusion-d-people-man-person-32590363.jpg",
            ReleaseDate = document.Release_Date,
            Rating = document.Rating,
            Pages = document.Pages,
            SeriesName = document.Series_Names?.FirstOrDefault() ?? "Unknown",
            Isbn = document.Isbns?.FirstOrDefault() ?? "",
            Genres = document.Genres?.ToList() ?? new List<string>(),
            Tags = document.Tags?.ToList() ?? new List<string>(),
            Moods = document.Moods?.ToList() ?? new List<string>()

        };
    }
}