using System.Text.Json.Serialization;

namespace DeviceConfigurator.Contracts.Response
{
    public class ConfigureResponse
    {
        [JsonPropertyName("success")]
        public bool Success { get; set; }
    }
}
