using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using System.IO;


namespace ImageSharing.Business.Service.NetP2P
{
    [ServiceContract(CallbackContract = typeof(INetTcpImageSharing))]
    public interface INetTcpImageSharing
    {
        [OperationContract(IsOneWay = true)]
        void svcJoin(string uname);

        [OperationContract(IsOneWay = true)]
        void svcSendIamge(Stream streamImage);
        
        [OperationContract(IsOneWay = true)]
        void svcUnJoin(string uname);
    } 

    public interface INetTcpImageShareChannel : INetTcpImageSharing, IClientChannel
    {
    }
}
