 using System;
using System.ServiceModel;
using System.Text;

namespace VMuktiService
{
    public class BasicHttpClient
    {
        object factory;
        public string Name = "";

        public BasicHttpBinding objBasicHttpBinding = null;
        
        //string MeshURI = "net.p2p://" + MeshID + MyBaseAddress.Split(':')[2].Substring(4);

        public object OpenClient<MyTChannel>(string MyBaseAddress)
        {
            
            factory = new ChannelFactory<MyTChannel>(NewBasicHttpBinding());
            

            //((DuplexChannelFactory<MyTChannel>)factory).Credentials.Windows.ClientCredential.Domain = "192.168.1.107";
            //((DuplexChannelFactory<MyTChannel>)factory).Credentials.Windows.ClientCredential.UserName = "administrator";
            //((DuplexChannelFactory<MyTChannel>)factory).Credentials.Windows.ClientCredential.Password = "m1havir?";
            //((DuplexChannelFactory<MyTChannel>)factory).Credentials.ClientCertificate.SetCertificate(StoreLocation.CurrentUser, StoreName.TrustedPublisher, X509FindType.FindBySubjectName, @"Adiance\Adiance");
            ((ChannelFactory<MyTChannel>)factory).Open();

            //EndpointIdentity ent = EndpointIdentity.CreateDnsIdentity("Pratik");

            return ((ChannelFactory<MyTChannel>)factory).CreateChannel(new EndpointAddress(MyBaseAddress));
        }

        public void CloseClient<MyTChannel>()
        {
            ((ChannelFactory<MyTChannel>)factory).Close();
        }
        //Before Apply Performance rule.
        //private BasicHttpBinding NewBasicHttpBinding()
        //After Applying rule.
        public BasicHttpBinding NewBasicHttpBinding()
        {
            //BasicHttpBinding objBasicHttpBinding = new BasicHttpBinding();

            objBasicHttpBinding = new BasicHttpBinding();
            
            objBasicHttpBinding.SendTimeout = TimeSpan.MaxValue;
            objBasicHttpBinding.CloseTimeout = TimeSpan.FromMinutes(1);
            objBasicHttpBinding.OpenTimeout = TimeSpan.FromMinutes(10);
            objBasicHttpBinding.ReceiveTimeout = TimeSpan.FromMinutes(10);
            objBasicHttpBinding.SendTimeout = TimeSpan.FromMinutes(10);
            objBasicHttpBinding.BypassProxyOnLocal = true;
            objBasicHttpBinding.MaxBufferPoolSize = 524288;
            objBasicHttpBinding.MaxReceivedMessageSize = 2147483647;
           // objBasicHttpBinding.MessageEncoding = WSMessageEncoding.Text;
           // objBasicHttpBinding.TextEncoding = Encoding.UTF8;
            objBasicHttpBinding.UseDefaultWebProxy = true;
            objBasicHttpBinding.ReaderQuotas.MaxArrayLength = 2147483647;
            objBasicHttpBinding.ReaderQuotas.MaxBytesPerRead = 2147483647;
            objBasicHttpBinding.ReaderQuotas = System.Xml.XmlDictionaryReaderQuotas.Max;

            return objBasicHttpBinding;
        }

    }
}
