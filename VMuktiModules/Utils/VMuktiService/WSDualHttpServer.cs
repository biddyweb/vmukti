using System;
using System.ServiceModel;
using System.ServiceModel.PeerResolvers;
using System.Text;

namespace VMuktiService
{
    public class WSDualHttpServer
    {
        //private CustomPeerResolverService cprs;
        private ServiceHost host;
        public string Name = "";

        public WSDualHttpServer(ref object objSingltoneObject, string MyBaseAddress)
        {
            host = new ServiceHost(objSingltoneObject, new Uri(MyBaseAddress));
        }

        public void AddEndPoint<MyTChannel>(string EndPointAddress)
        {
            host.AddServiceEndpoint(typeof(MyTChannel), NewWSDualHttpBinding(), EndPointAddress);
        }

        public void OpenServer()
        {
            host.Open();
        }

        public void CloseServer()
        {
            host.Close();
        }
        //before applying rule.
        //private WSDualHttpBinding NewWSDualHttpBinding()
            //after applying rule.
         static WSDualHttpBinding NewWSDualHttpBinding()
        {
            WSDualHttpBinding objWSDualHttpBinding = new WSDualHttpBinding();
            
            objWSDualHttpBinding.SendTimeout = TimeSpan.MaxValue;
            objWSDualHttpBinding.CloseTimeout = TimeSpan.FromMinutes(1);
            objWSDualHttpBinding.OpenTimeout = TimeSpan.FromMinutes(1);
            objWSDualHttpBinding.ReceiveTimeout = TimeSpan.FromMinutes(2);
            objWSDualHttpBinding.SendTimeout = TimeSpan.FromMinutes(1);
            objWSDualHttpBinding.BypassProxyOnLocal = false;
            objWSDualHttpBinding.TransactionFlow = false;
            objWSDualHttpBinding.MaxBufferPoolSize = 524288;
            objWSDualHttpBinding.MaxReceivedMessageSize = 2147483647;
            objWSDualHttpBinding.MessageEncoding = WSMessageEncoding.Text;
            objWSDualHttpBinding.TextEncoding = Encoding.UTF8;
            objWSDualHttpBinding.UseDefaultWebProxy = true;

            return objWSDualHttpBinding;
        }
    }
}
