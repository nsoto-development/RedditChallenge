using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace RedditChallenge.Services.RedditApi
{
    public class AuthService
    {
        private readonly HttpClient _httpClient;
        private readonly string _clientId;
        private readonly string _clientSecret;
        private readonly string _userAgent;

        public AuthService(HttpClient httpClient, string clientId, string clientSecret, string userAgent)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            _clientId = clientId ?? throw new ArgumentNullException(nameof(clientId));
            _clientSecret = clientSecret ?? throw new ArgumentNullException(nameof(clientSecret));
            _userAgent = userAgent ?? throw new ArgumentNullException(nameof(userAgent));
        }

        public async Task<string?> GetAccessTokenAsync()
        {
            // Reddit OAuth token endpoint
            var tokenEndpoint = "https://www.reddit.com/api/v1/access_token";

            // Construct the request body with client credentials
            var requestBody = $"grant_type=client_credentials";

            // Add client ID and client secret to be used with basic authentication
            var clientCredentials = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{_clientId}:{_clientSecret}"));

            // Add basic authentication header
            _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", clientCredentials);

            // Create a POST request to obtain the access token
            var request = new HttpRequestMessage(HttpMethod.Post, tokenEndpoint);
            request.Headers.TryAddWithoutValidation("User-Agent", _userAgent);
            request.Content = new StringContent(requestBody, Encoding.UTF8, "application/x-www-form-urlencoded");

            // Send the request and parse the response
            var response = await _httpClient.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                // Extract the access token from the response
                var tokenResponse = JsonConvert.DeserializeObject<TokenResponse>(responseContent);
                var accessToken = tokenResponse.access_token;
                return accessToken;
            }
            else
            {
                throw new HttpRequestException($"Failed to obtain access token. Status code: {response.StatusCode}");
            }
        }
    }

    internal class TokenResponse
    {
        public string? access_token { get; set; }
    }
}
