using System;
using System.Text.Json.Serialization;

namespace Service.Shared.Contracts.Response
{
    public class FirmwareItemResponse
    {
        [JsonPropertyName("id")]
        public Guid Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("description")]
        public string Description { get; set; }

        [JsonPropertyName("version")]
        public int? Version { get; set; }

        [JsonPropertyName("lastVersionDate")]
        public DateTime? LastVersionDate { get; set; }
    }
}
