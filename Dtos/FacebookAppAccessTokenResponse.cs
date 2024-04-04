using Newtonsoft.Json;

namespace Blocks_api.Dtos
{
    public class FacebookAppAccessTokenResponse
    {
        [JsonProperty("access_token")]
        public string AccessToken { get; set; } = null!;
        [JsonProperty("token_type")]
        public string TokenType { get; set; } = null!;
    }
}
