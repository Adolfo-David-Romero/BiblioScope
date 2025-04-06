using System.Net.Http.Json;
using System.Text.Json;

namespace BiblioScope.Model;

//Utilized Hardcover.app API documentation here --> https://docs.hardcover.app/
public class HardcoverBookService
{
    private readonly HttpClient _httpClient;
    
    string token = "Bearer eyJhbGciOiJIUzI1NiJ9.eyJpc3MiOiJIYXJkY292ZXIiLCJ2ZXJzaW9uIjoiOCIsImp0aSI6ImNjNmRkNGFlLWYyODYtNDljMC04OTNmLTY2MjJhMzk5ODhkOSIsImFwcGxpY2F0aW9uSWQiOjIsInN1YiI6IjI4NTc4IiwiYXVkIjoiMSIsImlkIjoiMjg1NzgiLCJsb2dnZWRJbiI6dHJ1ZSwiaWF0IjoxNzQzODk1Mzk5LCJleHAiOjE3NzU0MzEzOTksImh0dHBzOi8vaGFzdXJhLmlvL2p3dC9jbGFpbXMiOnsieC1oYXN1cmEtYWxsb3dlZC1yb2xlcyI6WyJ1c2VyIl0sIngtaGFzdXJhLWRlZmF1bHQtcm9sZSI6InVzZXIiLCJ4LWhhc3VyYS1yb2xlIjoidXNlciIsIlgtaGFzdXJhLXVzZXItaWQiOiIyODU3OCJ9LCJ1c2VyIjp7ImlkIjoyODU3OH19.IpVBpzFOa0rukUfqnG45uqmo7IfO3i-uI7O6DOvAcz4"; //stored in launchSettings.json
    
    /// <summary>Service used to initiate Hardcover.app API </summary>
    public HardcoverBookService()
    {
        _httpClient = new HttpClient
        {
            BaseAddress = new Uri("https://hardcover.app/api/")
        };

        _httpClient.DefaultRequestHeaders.Authorization = 
            new System.Net.Http.Headers.AuthenticationHeaderValue("Token", token);
    }

    public async Task<Book?> SearchBookByTitleAsync(string title)
    {
        try
        {
            var response = await _httpClient.GetAsync($"search?q={Uri.EscapeDataString(title)}");

            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine($"API request failed: {response.StatusCode}");
                return null;
            }

            var searchResult = await response.Content.ReadFromJsonAsync<HardcoverSearchResponse>();
            var doc = searchResult?.Data?.Search?.Results?.Hits?.FirstOrDefault()?.Document;

            return doc != null ? BookMapper.Map(doc) : null;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
            return null;
        }
    }
    
}