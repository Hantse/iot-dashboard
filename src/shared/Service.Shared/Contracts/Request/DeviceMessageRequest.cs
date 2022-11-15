using System.Text.Json.Serialization;

namespace Service.Shared.Contracts.Request
{
    public class DeviceMessageRequest
    {
        [JsonPropertyName("topic")]
        public string Topic { get; set; } 

        [JsonPropertyName("message")]
        public string Message { get; set; } = string.Empty;
    }
}
