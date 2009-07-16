using System;
using System.ServiceModel;
using System.Text;
 
namespace VMuktiService
{
    public class BasicHttpServer
    {
        private ServiceHost host;
        public string Name = "";

        public BasicHttpBinding objBasicHttpBinding = null;

        public BasicHttpServer(ref object objSingltoneObject, string MyBaseAddress)
        {
            host = new ServiceHost(objSingltoneObject, new Uri(MyBaseAddress));
        }

        public void AddEndPoint<MyTChannel>(string EndPointAddress)
        {
            host.AddServiceEndpoint(typeof(MyTChannel), NewBasicHttpBinding(), EndPointAddress);
        }

        public void OpenServer()
        {
            host.Open();
        }

        public void CloseServer()
        {
            host.Close();
        }
        //Before applyin Rule.
        //private BasicHttpBinding NewBasicHttpBinding()
        //After applying rule.
        public BasicHttpBinding NewBasicHttpBinding()
        {
            //BasicHttpBinding objBasicHttpBinding = new BasicHttpBinding();

            objBasicHttpBinding = new BasicHttpBinding();

            objBasicHttpBinding.SendTimeout = TimeSpan.MaxValue;
            objBasicHttpBinding.CloseTimeout = TimeSpan.FromMinutes(1);
            objBasicHttpBinding.OpenTimeout = TimeSpan.FromMinutes(1);
            objBasicHttpBinding.ReceiveTimeout = TimeSpan.FromMinutes(2);
            objBasicHttpBinding.SendTimeout = TimeSpan.FromMinutes(1);
            objBasicHttpBinding.BypassProxyOnLocal = false;
            objBasicHttpBinding.MaxBufferPoolSize = 524288;
            objBasicHttpBinding.MaxReceivedMessageSize = 2147483647;
            //objBasicHttpBinding.MessageEncoding = WSMessageEncoding.Text;
            //objBasicHttpBinding.TextEncoding = Encoding.UTF8;
            objBasicHttpBinding.UseDefaultWebProxy = true;
            objBasicHttpBinding.ReaderQuotas.MaxArrayLength = 2147483647;
            objBasicHttpBinding.ReaderQuotas.MaxBytesPerRead = 2147483647;
            objBasicHttpBinding.ReaderQuotas = System.Xml.XmlDictionaryReaderQuotas.Max;

            return objBasicHttpBinding;
        }
    }
}
