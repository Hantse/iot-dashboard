using System.Text.Json.Serialization;

namespace DeviceConfigurator.Contracts.Response
{
    public class AvailableNetworkResponse
    {
        [JsonPropertyName("ssid")]
        public string Ssid { get; set; }
    }
}
