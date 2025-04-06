using System.Net.Http.Json;
using System.Text.Json;

namespace BiblioScope.Model;

//Utilized Hardcover.app API documentation here --> https://docs.hardcover.app/
public class HardcoverBookService
{
    private readonly HttpClient _httpClient;
    
    string? token = Environment.GetEnvironmentVariable("HARDCOVER_API_TOKEN"); //stored in launchSettings.json
    
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