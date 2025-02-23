using System.Net.Http;

namespace CookWhatYouHave
{
    // Manages API key and basic API interactions
    public class ApiClient
    {
        private readonly HttpClient _client;
        private readonly string _apiKey;

        // Constructor to initialize HttpClient and API key
        public ApiClient(HttpClient client, string apiKey)
        {
            _client = client;
            _apiKey = apiKey;
        }

        // Provides read-only access to the API key
        public string ApiKey => _apiKey;

        // Provides read-only access to the HttpClient instance
        public HttpClient Client => _client;
    }
}