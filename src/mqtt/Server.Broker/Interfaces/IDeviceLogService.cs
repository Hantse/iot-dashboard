using System.Threading.Tasks;

namespace Server.Broker.Interfaces
{
    public interface IDeviceLogService
    {
        Task LogPing(string deviceId);
        Task LogFile(string deviceId);

        Task LogConnection(string deviceId);
        Task LogDisconnection(string deviceId);

        Task LogSubscribe(string deviceId, string topic);
        Task LogUnsubscribe(string deviceId, string topic);

        Task LogMessage(string deviceId, string message, string topic = null);
    }
}
