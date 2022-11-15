using System;
using System.Text.Json.Serialization;

namespace Service.Shared.Contracts.Response
{
    public class DeviceLogItemResponse
    {
        [JsonPropertyName("routineKey")]
        public string RoutineKey { get; set; }

        [JsonPropertyName("createAt")]
        public DateTime CreateAt { get; set; }

        [JsonPropertyName("grouped")]
        public DeviceLogGroupedItemResponse[] Grouped { get; set; }
    }
}
