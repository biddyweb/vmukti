using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using System.IO;
using Desktop_Sharing.Business.Service.MessageContract;

namespace Desktop_Sharing.Business.Service.BasicHttp
{
    [ServiceContract]
    public interface IHttpDesktop
    {
        [OperationContract(IsOneWay = true)]
         void svcJoin(clsMessageContract streamUName);

        [OperationContract(IsOneWay = true)]
        void svcSendMessage(clsMessageContract streamImage);

        [OperationContract(IsOneWay = true)]
        void svcGetUserList(clsMessageContract streamGetUserList);

        [OperationContract(IsOneWay = true)]
        void svcSetUserList(clsMessageContract streamSetUserList);

        [OperationContract(IsOneWay = true)]
        void svcSelectedDesktop(clsMessageContract streamUName);

        [OperationContract(IsOneWay = true)]
        void svcStopControl(clsMessageContract streamUName);
        
        [OperationContract(IsOneWay = true)]
        void svcBtnDown(clsMessageContract streamButtonDown);

        [OperationContract(IsOneWay = true)]
        void svcBtnUp(clsMessageContract streamButtonUp);

        [OperationContract(IsOneWay = true)]
        void svcSendXY(clsMessageContract streamXY);
        
        [OperationContract(IsOneWay = true)]
        void svcSendKey(clsMessageContract streamKey);

        [OperationContract(IsOneWay = true)]
        void svcAllowView(clsMessageContract streamView);

        [OperationContract(IsOneWay = true)]
        void svcAllowControl(clsMessageContract streamControl);
        
        [OperationContract(IsOneWay = false)]
        clsGetMessage svcGetMessages(clsMessageContract streamRecipient);
        
        [OperationContract(IsOneWay = true)]
        void svcUnJoin(clsMessageContract streamUName);

    }

    public interface IHttpDesktopChannel : IHttpDesktop, IClientChannel
    {
    }
}
