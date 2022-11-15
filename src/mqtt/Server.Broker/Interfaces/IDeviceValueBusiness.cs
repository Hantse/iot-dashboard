using System.Threading.Tasks;

namespace Server.Broker.Interfaces
{
    public interface IDeviceValueBusiness
    {
        Task InsertDeviceValue(string correlationId, string deviceName, string content);
        Task InsertDeviceGpsValue(string correlationId, string deviceName, string content);
    }
}
