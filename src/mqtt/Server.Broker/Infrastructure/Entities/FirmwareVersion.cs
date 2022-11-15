using Infrastructure.Core.Persistence;
using System;

namespace Server.Broker.Infrastructure.Entities
{
    public class FirmwareVersion : CoreEntity
    {
        public Guid FirmwareId { get; set; }
        public int Version { get; set; }
        public string Checksum { get; set; }
        public byte[] Content { get; set; }
    }
}
