using System.Text.Json.Serialization;

namespace Service.Shared.Models
{
    public class WorkerSchema
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("description")]
        public string Description { get; set; }
    }
}
