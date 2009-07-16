using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using System.Windows.Documents;
using System.IO;
using Desktop_Sharing.Business.Service.MessageContract;

namespace Desktop_Sharing.Business.Service.NetP2P
{
    [ServiceContract(CallbackContract = typeof(INetTcpDesktop))]
    public interface INetTcpDesktop
    {
        [OperationContract(IsOneWay = true)]
        void svcJoin(clsMessageContract mcJoin);

        [OperationContract(IsOneWay = true)]
        //void svcSendMessage(Stream streamImage);
        void svcSendMessage(clsMessageContract mcSendMessage);

        [OperationContract(IsOneWay = true)]
        //void svcGetUserList(string streamGetUserList);
        void svcGetUserList(clsMessageContract mcGetUserList);

        [OperationContract(IsOneWay = true)]
        //void svcSetUserList(string streamSetUserList);
        void svcSetUserList(clsMessageContract mcSetUserList);


        [OperationContract(IsOneWay = true)]
       // void svcSelectedDesktop(string uName);
        void svcSelectedDesktop(clsMessageContract mcSelectedVideo);
        
        [OperationContract(IsOneWay = true)]
        //void svcStopControl(string uName);
        void svcStopControl(clsMessageContract mcStopControl);

        [OperationContract(IsOneWay = true)]
        //void svcBtnDown(int mouseButton, string To);
        void svcBtnDown(clsMessageContract mcBtnDown);

        [OperationContract(IsOneWay = true)]
        //void svcBtnUp(int mouseButton, string To);
        void svcBtnUp(clsMessageContract mcBtnUp);

        [OperationContract(IsOneWay = true)]
       // void svcSendXY(double x, double y, string To);
        void svcSendXY(clsMessageContract mcSendXY);

        [OperationContract(IsOneWay = true)]
        //void svcSendKey(int key, string To);
        void svcSendKey(clsMessageContract mcSendKey);

        [OperationContract(IsOneWay = true)]
        //void svcAllowView(string uName, bool blView);
        void svcAllowView(clsMessageContract mcAllowView);



        [OperationContract(IsOneWay = true)]
       // void svcAllowControl(string uName, bool blControl);
        void svcAllowControl(clsMessageContract mcAllowControl);
        
        [OperationContract(IsOneWay = true)]
        //void svcUnJoin(string streamUName);
        void svcUnJoin(clsMessageContract mcUnJoin);

    }

    public interface INetTcpDesktopChannel : INetTcpDesktop, IClientChannel
    { }
}
