using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;

namespace BiblioScope.Model;

//Utilized Hardcover.app API documentation here --> https://docs.hardcover.app/
public class HardcoverBookService
{
    private readonly HttpClient _httpClient;

    public HardcoverBookService()
    {
        _httpClient = new HttpClient();
        _httpClient.BaseAddress = new Uri("https://api.hardcover.app/v1/graphql/");
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", "[INSERT YOUR HARDCOVER API HERE]");
        _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

        Console.WriteLine("[INIT] HardcoverBookService initialized with base URL: " + _httpClient.BaseAddress);
    }

    public async Task<RootObject> SearchBooksAsync(string queryText, int page = 1, int perPage = 5)
    {
        Console.WriteLine($"[INFO] Sending GraphQL request: Query='{queryText}', Page={page}, PerPage={perPage}");

        var graphqlQuery = new
        {
            query = @"
        query Search($query: String!, $page: Int!, $per_page: Int!) {
            search(query: $query, page: $page, per_page: $per_page) {
                results
            }
        }",
            variables = new
            {
                query = queryText,
                page = page,
                per_page = perPage
            }
        };

        string requestBody = JsonSerializer.Serialize(graphqlQuery);
        Console.WriteLine($"[DEBUG] Request JSON: {requestBody}");

        var content = new StringContent(requestBody, Encoding.UTF8, "application/json");

        try
        {
            var response = await _httpClient.PostAsync("", content);
            Console.WriteLine($"[DEBUG] HTTP Status Code: {(int)response.StatusCode} - {response.ReasonPhrase}");

            var jsonString = await response.Content.ReadAsStringAsync();
            Console.WriteLine($"[DEBUG] Response JSON: {jsonString}");

            if (response.IsSuccessStatusCode)
            {
                var result = JsonSerializer.Deserialize<RootObject>(jsonString, new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                });

                Console.WriteLine($"[SUCCESS] Parsed {result?.data?.search?.results?.hits?.Length ?? 0} book(s).");

                return result;
            }
            else
            {
                Console.WriteLine($"[ERROR] Failed GraphQL query. StatusCode: {response.StatusCode}");
                Console.WriteLine($"[ERROR] Response Content: {jsonString}");
                throw new HttpRequestException($"GraphQL query failed with status code {response.StatusCode}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[EXCEPTION] {ex.GetType().Name}: {ex.Message}");
            throw;
        }
    }
    
}
