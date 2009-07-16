using System;
using System.Security.Cryptography.X509Certificates;
using System.ServiceModel;
using System.Text;

namespace VMuktiService
{
    public class WSDualHttpClient
    {
        object factory;
        public string Name = "";
        
     

        public object OpenClient<MyTChannel>(string MyBaseAddress, ref object MyObject)
        {
            InstanceContext MyInstanctContact = new InstanceContext(MyObject);
            factory = new DuplexChannelFactory<MyTChannel>(MyInstanctContact, NewWSDualHttpBinding());
            ((DuplexChannelFactory<MyTChannel>)factory).Credentials.ClientCertificate.SetCertificate(StoreLocation.CurrentUser, StoreName.TrustedPublisher, X509FindType.FindBySubjectName, @"Adiance\Adiance");
            ((DuplexChannelFactory<MyTChannel>)factory).Open();
            return ((DuplexChannelFactory<MyTChannel>)factory).CreateChannel(new EndpointAddress(MyBaseAddress));
        }

        public void CloseClient<MyTChannel>()
        {
            ((DuplexChannelFactory<MyTChannel>)factory).Close();
        }

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
            objWSDualHttpBinding.Security.Message.ClientCredentialType = MessageCredentialType.Certificate;

            return objWSDualHttpBinding;
        }
    }
}
