using System;
using System.ServiceModel;

namespace VMuktiService
{
    public class NetPeerClient
    {
        object factory;
        public string Name = "";

        public object OpenClient<MyTChannel>(string MyBaseAddress, string MeshID, ref object MyObject)
        {

            InstanceContext MyInstanctContact = new InstanceContext(MyObject);
            factory = new DuplexChannelFactory<MyTChannel>(MyInstanctContact, NewBinding(MyBaseAddress));

            ((DuplexChannelFactory<MyTChannel>)factory).Credentials.Peer.MeshPassword = "resiprocket";
            ((DuplexChannelFactory<MyTChannel>)factory).Open();

            return ((DuplexChannelFactory<MyTChannel>)factory).CreateChannel(new EndpointAddress("net.p2p://" + MeshID + MyBaseAddress.Split(':')[1].Substring(4)));
        }

        public void CloseClient<MyTChannel>()
        {
            ((DuplexChannelFactory<MyTChannel>)factory).Close();
        }
        //before applyin performance rule.
        //private NetPeerTcpBinding NewBinding(string MyBaseAddress)
        //after applyin rule.
        static NetPeerTcpBinding NewBinding(string MyBaseAddress)
        {
            NetPeerTcpBinding binding = new NetPeerTcpBinding();
            NetTcpBinding bin = new NetTcpBinding();

            bin.Security.Mode = SecurityMode.None;
            bin.MaxBufferPoolSize = long.MaxValue;
            bin.MaxBufferSize = int.MaxValue;
            bin.MaxReceivedMessageSize = int.MaxValue;
            bin.ReceiveTimeout = new TimeSpan(0, 5, 0);
            bin.ListenBacklog = int.MaxValue;
            bin.ReliableSession.Ordered = true;
            bin.ReaderQuotas = System.Xml.XmlDictionaryReaderQuotas.Max;

            binding.Resolver.Custom.Binding = bin;
            binding.Resolver.Custom.Address = new EndpointAddress(MyBaseAddress);
            binding.MaxReceivedMessageSize = int.MaxValue;
            binding.MaxBufferPoolSize = int.MaxValue;

            binding.Port = 0;

            return binding;
        }
    }
}
