using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using ImageSharing.Business.Service.DataContracts;
using System.IO;

namespace ImageSharing.Business.Service.BasicHttp
{
    [ServiceContract]
    public interface IHttpImageSharing
    {
        [OperationContract(IsOneWay = true)]
        void svcJoin(Stream streamUName);

        [OperationContract(IsOneWay = true)]
        void svcSendIamge(Stream streamImage);

        [OperationContract(IsOneWay = false)]
        Stream svcGetMessages(Stream streamRecipient);

        [OperationContract(IsOneWay = true)]
        void svcUnJoin(Stream streamUName);
    }

    public interface IHttpImageSharingChannel : IHttpImageSharing, IClientChannel
    {
    }
}
