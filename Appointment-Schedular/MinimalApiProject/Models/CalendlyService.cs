using System.Net.Http;
using System.Threading.Tasks;
using MinimalApiProject.Models;

public class CalendlyService
{
    private readonly HttpClient _httpClient;
    private readonly string _apiKey;

    public CalendlyService(IHttpClientFactory httpClientFactory, IConfiguration configuration)
    {
        _httpClient = httpClientFactory.CreateClient();
        _apiKey = configuration["CalendlyApiKey"];
        _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {_apiKey}");
    }

    public async Task CreateEventAsync(PriestAvailabilityInput availability)
    {
        // Implement logic to convert availability to a Calendly event
        var content = new StringContent("...");
        var response = await _httpClient.PostAsync("https://api.calendly.com/scheduled_events", content);
        response.EnsureSuccessStatusCode();
    }
}
