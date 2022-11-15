using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Service.Shared.Contracts.Response
{
    public class DeviceItemResponse
    {
        [JsonPropertyName("id")]
        public Guid Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("online")]
        public bool Online { get; set; }

        [JsonPropertyName("ip")]
        public string Ip { get; set; }

        [JsonPropertyName("connectionRoutineCount")]
        public int ConnectionRoutineCount { get; set; }

        [JsonPropertyName("pingRoutineCount")]
        public int PingRoutineCount { get; set; }

        [JsonPropertyName("subscribeRoutineCount")]
        public int SubscribeRoutineCount { get; set; }

        [JsonPropertyName("unsubscribeRoutineCount")]
        public int UnsubscribeRoutineCount { get; set; }

        [JsonPropertyName("messageRoutineCount")]
        public int MessageRoutineCount { get; set; }

        [JsonPropertyName("topics")]
        public string[] Topics { get; set; }

        [JsonPropertyName("lastPicture")]
        public string LastPicture { get; set; }

        [JsonPropertyName("boardInformation")]
        public string BoardInformation { get; set; }

        public Dictionary<string, string> MappedBoardInformationValues
        {
            get
            {
                if (string.IsNullOrEmpty(BoardInformation))
                    return new Dictionary<string, string>();

                return Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, string>>(BoardInformation);
            }
        }
    }
}
