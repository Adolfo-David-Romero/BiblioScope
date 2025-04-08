using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace BiblioScope.Model;

public class FirestoreService
{
    private readonly HttpClient _httpClient;
    private readonly string _projectId = "biblioscope"; 
    private readonly string _userId;
    private readonly string _idToken;

    public FirestoreService(string userId, string idToken)
    {
        _userId = userId;
        _idToken = idToken;
        _httpClient = new HttpClient();
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _idToken);
    }
    public async Task DeleteBookAsync(string isbn)
    {
        var url = $"https://firestore.googleapis.com/v1/projects/{_projectId}/databases/(default)/documents/users/{_userId}/library/{isbn}";
        var response = await _httpClient.DeleteAsync(url);

        if (response.IsSuccessStatusCode)
        {
            Console.WriteLine($"[Firestore] Deleted book {isbn}");
        }
        else
        {
            var error = await response.Content.ReadAsStringAsync();
            Console.WriteLine($"[Firestore] Error deleting: {response.StatusCode} - {error}");
        }
    }

    public async Task SaveBookAsync(Book book)
    {
        string documentId = book.Isbn ?? Guid.NewGuid().ToString();
        var url =
            $"https://firestore.googleapis.com/v1/projects/{_projectId}/databases/(default)/documents/users/{_userId}/library/{documentId}";

        var firestoreBook = new
        {
            fields = new
            {
                isbn = new { stringValue = book.Isbn },
                title = new { stringValue = book.Title },
                subtitle = new { stringValue = book.Subtitle },
                author = new { stringValue = book.Author },
                description = new { stringValue = book.Description },
                coverImageUrl = new { stringValue = book.CoverImageUrl },
                publisher = new { stringValue = book.Publisher },
                seriesName = new { stringValue = book.SeriesName },
                releaseDate = new { stringValue = book.ReleaseDate },
                rating = new { doubleValue = book.Rating },
                pages = new { integerValue = book.Pages },
                genres = new
                {
                    arrayValue = new
                    {
                        values = book.Genres.Select(g => new { stringValue = g }).ToArray()
                    }
                },
                tags = new
                {
                    arrayValue = new
                    {
                        values = book.Tags.Select(t => new { stringValue = t }).ToArray()
                    }
                },
                moods = new
                {
                    arrayValue = new
                    {
                        values = book.Moods.Select(m => new { stringValue = m }).ToArray()
                    }
                }
            }
        };

        var json = JsonSerializer.Serialize(firestoreBook);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        var response = await _httpClient.PatchAsync(url, content); // PATCH allows upsert

        if (response.IsSuccessStatusCode)
        {
            Console.WriteLine($"Book saved to Firestore: {book.Title}");
        }
        else
        {
            var error = await response.Content.ReadAsStringAsync();
            Console.WriteLine($"Firestore error: {response.StatusCode} - {error}");
        }
    }
    
    //loading the library on sign in 
    public async Task<List<Book>> GetUserBooksAsync()
    {
        var url =
            $"https://firestore.googleapis.com/v1/projects/{_projectId}/databases/(default)/documents/users/{_userId}/library";
        var response = await _httpClient.GetAsync(url);

        if (!response.IsSuccessStatusCode)
        {
            Console.WriteLine($"[Firestore] Failed to fetch books: {response.StatusCode}");
            return new List<Book>();
        }

        var json = await response.Content.ReadAsStringAsync();
        var document = JsonDocument.Parse(json);
        var books = new List<Book>();

        if (!document.RootElement.TryGetProperty("documents", out var docArray)) return books;

        foreach (var doc in docArray.EnumerateArray())
        {
            if (!doc.TryGetProperty("fields", out var fields)) continue;

            var book = new Book
            {
                Isbn = fields.TryGetProperty("isbn", out var isbn) ? isbn.GetProperty("stringValue").GetString() : "",
                Title = fields.TryGetProperty("title", out var title)
                    ? title.GetProperty("stringValue").GetString()
                    : "",
                Author = fields.TryGetProperty("author", out var author)
                    ? author.GetProperty("stringValue").GetString()
                    : "",
                Description = fields.TryGetProperty("description", out var desc)
                    ? desc.GetProperty("stringValue").GetString()
                    : "",
                CoverImageUrl = fields.TryGetProperty("coverImageUrl", out var cover)
                    ? cover.GetProperty("stringValue").GetString()
                    : "",
                Publisher = fields.TryGetProperty("publisher", out var pub)
                    ? pub.GetProperty("stringValue").GetString()
                    : "",
                SeriesName = fields.TryGetProperty("seriesName", out var series)
                    ? series.GetProperty("stringValue").GetString()
                    : "",
                ReleaseDate = fields.TryGetProperty("releaseDate", out var rd)
                    ? rd.GetProperty("stringValue").GetString()
                    : "",
                Rating = fields.TryGetProperty("rating", out var rating)
                    ? rating.GetProperty("doubleValue").GetDouble()
                    : 0.0,
                Pages = fields.TryGetProperty("pages", out var pages) &&
                        pages.TryGetProperty("integerValue", out var pageVal)
                    ? int.Parse(pageVal.GetString() ?? "0")
                    : 0,
                Genres = ExtractStringArray(fields, "genres"),
                Tags = ExtractStringArray(fields, "tags"),
                Moods = ExtractStringArray(fields, "moods")
            };

            books.Add(book);
        }

        return books;
    }

    private static List<string> ExtractStringArray(JsonElement fields, string property)
    {
        if (!fields.TryGetProperty(property, out var arrayProp)) return new List<string>();

        return arrayProp.TryGetProperty("arrayValue", out var arrayValue) &&
               arrayValue.TryGetProperty("values", out var values)
            ? values.EnumerateArray().Select(v => v.GetProperty("stringValue").GetString() ?? "").ToList()
            : new List<string>();
    }

}