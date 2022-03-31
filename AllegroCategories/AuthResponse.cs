using System.Text.Json.Serialization;

namespace AllegroCategories;

public class AuthResponse
{
    [JsonPropertyName("access_token")]
    public string AccessToken { get; set; }

    [JsonPropertyName("token_type")]
    public string TokenType { get; set; }

    [JsonPropertyName("expires_in")]
    public int ExpiresIn { get; set; }

    [JsonPropertyName("scope")]
    public string Scope { get; set; }

    [JsonPropertyName("allegro_api")]
    public bool AllegroApi { get; set; }

    [JsonPropertyName("jti")]
    public string Jti { get; set; }
}