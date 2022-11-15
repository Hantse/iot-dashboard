using Service.Shared.Models;
using System.Text.Json.Serialization;

namespace Service.Shared.Contracts.Request
{
    public class WorkerRequest
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("schema")]
        public WorkerSchema Schema { get; set; }
    }
}
