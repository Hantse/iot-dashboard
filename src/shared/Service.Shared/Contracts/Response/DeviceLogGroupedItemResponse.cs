using System;
using System.Text.Json.Serialization;

namespace Service.Shared.Contracts.Response
{
    public class DeviceLogGroupedItemResponse
    {
        [JsonPropertyName("id")]
        public Guid Id { get; set; }

        [JsonPropertyName("type")]
        public string Type { get; set; }

        [JsonPropertyName("content")]
        public string Content { get; set; }

        [JsonPropertyName("createAt")]
        public DateTime CreateAt { get; set; }
    }
}
