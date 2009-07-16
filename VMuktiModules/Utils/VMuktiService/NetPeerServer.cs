using System;
using System.ServiceModel;
using System.ServiceModel.PeerResolvers;

namespace VMuktiService
{
    public class NetPeerServer
    {
        private CustomPeerResolverService cprs;
        private ServiceHost host;
        public string Name = "";
        
        public NetPeerServer(string MyBaseAddress)
        {
            cprs = new CustomPeerResolverService();
            cprs.ControlShape = true;

            host = new ServiceHost(cprs, new Uri[] { new Uri(MyBaseAddress) });
        }

        public void AddEndPoint(string EndPointAddress)
        {
            host.AddServiceEndpoint(typeof(System.ServiceModel.PeerResolvers.IPeerResolverContract), NewNetTcpBinding(), EndPointAddress);
        }

        public void OpenServer()
        {
            cprs.Open();
            host.Open();
        }

        public void CloseServer()
        {
            cprs.Close();
            host.Close();
        }
        //before applying performance rule.
        //private NetTcpBinding NewNetTcpBinding()
        //afer applying rule.
        static NetTcpBinding NewNetTcpBinding()
        {
            NetTcpBinding bin = new NetTcpBinding();

            //bin.PortSharingEnabled = true;
            bin.Security.Mode = SecurityMode.None;
            bin.MaxBufferPoolSize = long.MaxValue;
            bin.MaxBufferSize = int.MaxValue;
            bin.MaxReceivedMessageSize = int.MaxValue;
            bin.ReceiveTimeout = new TimeSpan(0, 5, 0);
            bin.ListenBacklog = int.MaxValue;
            bin.ReliableSession.Ordered = true;
            bin.ReaderQuotas = System.Xml.XmlDictionaryReaderQuotas.Max;

            return bin;
        }
    }
}
