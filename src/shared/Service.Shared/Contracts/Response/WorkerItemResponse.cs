using Service.Shared.Models;
using System;
using System.Text.Json.Serialization;

namespace Service.Shared.Contracts.Response
{
    public class WorkerItemResponse
    {
        [JsonPropertyName("id")]
        public Guid Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("schema")]
        public WorkerSchema Schema { get; set; }
    }
}
