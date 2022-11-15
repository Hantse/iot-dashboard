using Infrastructure.Core.Persistence;
using System;

namespace Server.Broker.Infrastructure.Entities
{
    public class GpsData : CoreEntity
    {
        public Guid DeviceId { get; set; }
        public string CorrelationId { get; set; }
        public string DeviceName { get; set; }
        public float Latitude { get; set; }
        public float Longitude { get; set; }
        public float MSLAltitude { get; set; }
        public string UTCDateAndTime { get; set; }
        public float SpeedOverGround { get; set; }
        public float CourseOverGround { get; set; }
        public int GNSSSatellitesInView { get; set; }
        public int GPSSatellitesUsed { get; set; }
        public int CNMax { get; set; }
        public float HDop { get; set; }
        public string FlatContent { get; set; }
    }
}
