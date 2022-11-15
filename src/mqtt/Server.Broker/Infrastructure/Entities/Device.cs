using Infrastructure.Core.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Server.Broker.Infrastructure.Entities
{
    public class Device : CoreEntity
    {
        public string Name { get; set; }
        public string IP { get; set; }
        public DateTime LastPing { get; set; }
        public string Firmware { get; set; }
        public int FirmwareVersion { get; set; }
        public string Status { get; set; }
        public string BoardInformation { get; set; }
    }
}
