using MQTTnet.Certificates;
using System.Security.Cryptography.X509Certificates;

namespace Server.Broker.Security
{
    public class CertificateProvider : ICertificateProvider
    {
        public X509Certificate2 GetCertificate()
        {
            throw new System.NotImplementedException();
        }
    }
}
