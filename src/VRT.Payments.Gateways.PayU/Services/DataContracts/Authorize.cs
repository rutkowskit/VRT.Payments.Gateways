using Refit;
using System.Text.Json.Serialization;

namespace VRT.Payments.Gateways.PayU.Services.DataContracts;
internal static class Authorize
{
    internal sealed class Request
    {
        [AliasAs("grant_type")]
        [JsonPropertyName("grant_type")]
        public string GrantType { get; set; } = "client_credentials";

        [AliasAs("client_id")]
        [JsonPropertyName("client_id")]
        public string ClientId { get; set; } = null!;

        [AliasAs("client_secret")]
        [JsonPropertyName("client_secret")]
        public string ClientSecret { get; set; } = null!;
    }

    internal sealed class Response
    {
        [JsonPropertyName("error")]
        public string Error { get; set; } = null!;
        [JsonPropertyName("error_description")]
        public string ErrorDescription { get; set; } = null!;

        [JsonPropertyName("access_token")]
        public string AccessToken { get; set; } = null!;
        [JsonPropertyName("token_type")]
        public string TokenType { get; set; } = null!;
        [JsonPropertyName("refresh_token")]
        public string RefreshToken { get; set; } = null!;
        [JsonPropertyName("expires_in")]
        public long ExpiresIn { get; set; }
        [JsonPropertyName("grant_type")]
        public string GrantType { get; set; } = null!;

        public long ResponseTimestampSeconds { get; } = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
        public bool IsExpired => (ResponseTimestampSeconds + ExpiresIn) >= DateTimeOffset.UtcNow.ToUnixTimeSeconds();
        public bool ShouldRefresh => (ResponseTimestampSeconds + ExpiresIn) - DateTimeOffset.UtcNow.ToUnixTimeSeconds() < 120;
    }
}
