using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Dashboard.Frontend.Models
{
    public class BaseCommandModel
    {
        [JsonPropertyName("command")]
        public string Command { get; set; }

        [JsonPropertyName("bind")]
        public string Bind { get; set; }

        [JsonPropertyName("value")]
        public object Value { get; set; }
    }
}
