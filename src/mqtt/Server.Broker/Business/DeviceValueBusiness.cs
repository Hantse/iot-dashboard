using JsonFlatten;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using Server.Broker.Infrastructure.Entities;
using Server.Broker.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Server.Broker.Business
{
    public class DeviceValueBusiness : IDeviceValueBusiness
    {
        private readonly ILogger<DeviceValueBusiness> logger;
        private readonly IDeviceValueRepository deviceValueRepository;
        private readonly IDeviceRepository deviceRepository;
        private readonly IGpsDataRepository gpsDataRepository;

        public DeviceValueBusiness(ILogger<DeviceValueBusiness> logger, IDeviceValueRepository deviceValueRepository, IDeviceRepository deviceRepository, IGpsDataRepository gpsDataRepository)
        {
            this.logger = logger;
            this.deviceValueRepository = deviceValueRepository;
            this.deviceRepository = deviceRepository;
            this.gpsDataRepository = gpsDataRepository;
        }

        public async Task InsertDeviceValue(string correlationId, string deviceName, string content)
        {
            var jObject = JObject.Parse(content);
            var flattened = jObject.Flatten();
            var device = await deviceRepository.QueryOneAsync(new Infrastructure.Entities.Device()
            {
                Name = deviceName
            });

            foreach (var kvp in flattened)
            {
                await deviceValueRepository.InsertAsync(new Infrastructure.Entities.DeviceValue()
                {
                    DeliverAt = System.DateTime.UtcNow,
                    CorrelationId = correlationId,
                    DeviceId = device.Id,
                    DeviceName = deviceName,
                    Key = kvp.Key,
                    Value = kvp.Value.ToString(),
                    ValueType = ConvertKeyToType(kvp.Key),
                    CreateBy = deviceName
                });
            }
        }

        //GNSS run status
        //Fix status
        //UTC date and time
        //Latitude
        //Longitude
        //MSL altitude
        //Speed over ground
        //Course over ground
        //Fix mode
        //Reserver1
        //HDOP
        //PDOP
        //VDOP
        //Reserved2
        //GNSS Satellites in View
        //GPS Satellites used
        //GLONASS Satellites used
        //Reserver3
        //C/N0 max
        //HPA
        //VPA
        public async Task InsertDeviceGpsValue(string correlationId, string deviceName, string content)
        {
            var device = await deviceRepository.QueryOneAsync(new Infrastructure.Entities.Device()
            {
                Name = deviceName
            });

            var values = content.Split(",");

            var dataGps = new GpsData()
            {
                DeviceName = device.Name,
                DeviceId = device.Id
            };

            var dataMapped = new Dictionary<string, string>();

            foreach (var v in values.Select((value, i) => new { i, value }))
            {
                if (!string.IsNullOrEmpty(v.value))
                {
                    dataMapped.Add(ConvertGpsIndexToKey(v.i), v.value);
                }
            }

            if (dataMapped.ContainsKey("Latitude"))
            {
                dataGps.Latitude = float.Parse(dataMapped["Latitude"]);
            }

            if (dataMapped.ContainsKey("Longitude"))
            {
                dataGps.Longitude = float.Parse(dataMapped["Longitude"]);
            }

            if (dataMapped.ContainsKey("MSLAltitude"))
            {
                dataGps.MSLAltitude = float.Parse(dataMapped["MSLAltitude"]);
            }

            if (dataMapped.ContainsKey("SpeedOverGround"))
            {
                dataGps.SpeedOverGround = float.Parse(dataMapped["SpeedOverGround"]);
            }

            if (dataMapped.ContainsKey("GNSSSatellitesInView"))
            {
                dataGps.GNSSSatellitesInView = int.Parse(dataMapped["GNSSSatellitesInView"]);
            }

            if (dataMapped.ContainsKey("GPSSatellitesUsed"))
            {
                dataGps.GPSSatellitesUsed = int.Parse(dataMapped["GPSSatellitesUsed"]);
            }

            await gpsDataRepository.InsertAsync(dataGps);
        }

        public string ConvertGpsIndexToKey(int index)
        {
            var gpsValues = new string[] { "GNSSRunStatus", "FixStatus", "UTCDateAndTime",
                "Latitude", "Longitude", "MSLAltitude", "SpeedOverGround", "CourseOverGround", "FixMode",
                "Reserver1", "HDOP", "PDOP", "VDOP", "Reserved2", "GNSSSatellitesInView", "GPSSatellitesUsed",
                "GLONASSSatellitesUsed", "Reserver3", "C/N0Max", "HPA", "VPA"};

            if (index > gpsValues.Length)
                return "Unkown";

            return gpsValues[index];
        }

        public string ConvertKeyToType(string key)
        {
            if (key.Contains("humidity") || key.Contains("celsius")
                || key.Contains("fahrenheit") || key.Contains("battery"))
                return "FLOAT";

            if (key.Contains("groundMoisure"))
                return "INTEGER";

            return "STRING";
        }
    }
}
