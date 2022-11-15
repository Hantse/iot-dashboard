using System.Text.Json.Serialization;

namespace DeviceConfigurator.Contracts.Response
{
    public class LocalDevicePing
    {
        [JsonPropertyName("hostname")]
        public string Hostname { get; set; }
    }
}
