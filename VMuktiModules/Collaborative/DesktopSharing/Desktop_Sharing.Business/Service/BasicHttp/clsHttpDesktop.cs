using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using System.IO;
using Desktop_Sharing.Business.Service.MessageContract;

namespace Desktop_Sharing.Business.Service.BasicHttp
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    public class clsHttpDesktop : IHttpDesktop
    {
        public delegate void delsvcJoin(clsMessageContract streamUName);
        public delegate void delsvcSendMessage(clsMessageContract streamImage);
        public delegate void delsvcGetUserList(clsMessageContract streamGetUserList);
        public delegate void delsvcSetUserList(clsMessageContract streamSetUserList);
        public delegate void delsvcSelectedDesktop(clsMessageContract streamUName);
        public delegate void delsvcStopControl(clsMessageContract streamUName);
        public delegate void delsvcBtnDown(clsMessageContract streamButtonDown);
        public delegate void delsvcBtnUp(clsMessageContract streamButtonUp);
        public delegate void delsvcSendXY(clsMessageContract streamXY);
        public delegate void delsvcSendKey(clsMessageContract streamKey);
        public delegate void delsvcAllowView(clsMessageContract streamView);
        public delegate void delsvcAllowControl(clsMessageContract streamControl);
        public delegate clsGetMessage delsvcGetMessages(clsMessageContract streamRecipient);
        public delegate void delsvcUnJoin(clsMessageContract streamUName);

        public event delsvcJoin EntsvcJoin;
        public event delsvcSendMessage EntsvcSendMessage;
        public event delsvcGetUserList EntsvcGetUserList;
        public event delsvcSetUserList EntsvcSetUserList;
        public event delsvcSelectedDesktop EntsvcSelectedDesktop;
        public event delsvcStopControl EntsvcStopControl;
        public event delsvcBtnDown EntsvcBtnDown;
        public event delsvcBtnUp EntsvcBtnUp;
        public event delsvcSendXY EntsvcSendXY;
        public event delsvcSendKey EntsvcSendKey;
        public event delsvcGetMessages EntsvcGetMessages;
        public event delsvcAllowView EntsvcAllowView;
        public event delsvcAllowControl EntsvcAllowControl;
        public event delsvcUnJoin EntsvcUnJoin;

        #region IHttpDesktop Members

        public void svcJoin(clsMessageContract streamUName)
        {
            if (EntsvcJoin != null)
            {
                EntsvcJoin(streamUName);
            }
        }

        public void svcSendMessage(clsMessageContract streamImage)
        {
            if (EntsvcSendMessage != null)
            {
                EntsvcSendMessage(streamImage);
            }
        }

        public void svcGetUserList(clsMessageContract streamGetUserList)
        {
            if (EntsvcGetUserList != null)
            {
                EntsvcGetUserList(streamGetUserList);
            }
        }

        public void svcSetUserList(clsMessageContract streamSetUserList)
        {
            if (EntsvcSetUserList != null)
            {
                EntsvcSetUserList(streamSetUserList);
            }
        }

        public void svcSelectedDesktop(clsMessageContract streamUName)
        {
            if (EntsvcSelectedDesktop != null)
            {
                EntsvcSelectedDesktop(streamUName);
            }
        }

        public void svcStopControl(clsMessageContract streamUName)
        {
            if (EntsvcStopControl != null)
            {
                EntsvcStopControl(streamUName);
            }
        }

        public void svcBtnDown(clsMessageContract streamButtonDown)
        {
            if (EntsvcBtnDown != null)
            {
                EntsvcBtnDown(streamButtonDown);
            }
        }

        public void svcBtnUp(clsMessageContract streamButtonUp)
        {
            if (EntsvcBtnUp != null)
            {
                EntsvcBtnUp(streamButtonUp);
            }
        }

        public void svcSendXY(clsMessageContract streamXY)
        {
            if (EntsvcSendXY != null)
            {
                EntsvcSendXY(streamXY);
            }
        }

        public void svcSendKey(clsMessageContract streamKey)
        {
            if (EntsvcSendKey != null)
            {
                EntsvcSendKey(streamKey);
            }

        }

        public clsGetMessage svcGetMessages(clsMessageContract streamRecipient)
        {
            if (EntsvcGetMessages != null)
            {
                return EntsvcGetMessages(streamRecipient);
            }
            else
            {
                return null;
            }
        }

        public void svcAllowView(clsMessageContract streamView)
        {
            if (EntsvcAllowView != null)
            {
                EntsvcAllowView(streamView);
            }
        }

        public void svcAllowControl(clsMessageContract streamControl)
        {
            if (EntsvcAllowControl != null)
            {
                EntsvcAllowControl(streamControl);
            }
        }

        public void svcUnJoin(clsMessageContract streamUName)
        {
            if (EntsvcUnJoin != null)
            {
                EntsvcUnJoin(streamUName);
            }
        }

        #endregion

    }
}
