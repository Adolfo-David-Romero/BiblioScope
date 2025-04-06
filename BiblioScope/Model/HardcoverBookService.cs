using System.Text.Json;

namespace BiblioScope.Model;
//user id -->  28578
//Utilized Hardcover.app API documentation here --> https://docs.hardcover.app/
public class HardcoverBookService
{
    private readonly HttpClient _httpClient;
    
    /// <summary>Service used to initiate Hardcover.app API </summary>
    public HardcoverBookService()
    {
        _httpClient = new HttpClient(); //used to send web requests
        
        _httpClient.BaseAddress = new Uri("https://hardcover.app/api/");
    }

    public async Task<Book> SearchBookByTitle(string title)
    {
        var response = await _httpClient.GetAsync($"");
        if (response.IsSuccessStatusCode)
        {
            //TODO: Study API
        }
        return null;
    }
    
}