using System;
using System.Text.Json.Serialization;

namespace Service.Shared.Contracts.Response
{
    public class FirmwareVersionItemResponse
    {
        [JsonPropertyName("id")]
        public Guid Id { get; set; }

        [JsonPropertyName("version")]
        public int Version { get; set; }
    }
}
