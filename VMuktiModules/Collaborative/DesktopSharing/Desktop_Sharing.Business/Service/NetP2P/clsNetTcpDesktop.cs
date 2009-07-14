using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Desktop_Sharing.Business.Service.MessageContract;

namespace Desktop_Sharing.Business.Service.NetP2P
{
    public class clsNetTcpDesktop : INetTcpDesktop
    {
        public delegate void delsvcJoin(clsMessageContract mcJoin);
        public delegate void delsvcSendMessage(clsMessageContract mcSendMessage);
        public delegate void delsvcGetUserList(clsMessageContract mcGetUserList);
        public delegate void delsvcSetUserList(clsMessageContract mcSetUserList);
        public delegate void delsvcSelectedDesktop(clsMessageContract mcSelectedVideo);
        public delegate void delsvcStopControl(clsMessageContract mcStopControl);
        public delegate void delsvcBtnDown(clsMessageContract mcBtnDown);
        public delegate void delsvcBtnUp(clsMessageContract mcBtnUp);
        public delegate void delsvcSendXY(clsMessageContract mcSendXY);
        public delegate void delsvcSendKey(clsMessageContract mcSendKey);
        public delegate void delsvcAllowView(clsMessageContract mcAllowView);
        public delegate void delsvcAllowControl(clsMessageContract mcAllowControl);
        public delegate void delsvcUnJoin(clsMessageContract mcUnJoin);
         
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
        public event delsvcAllowView EntsvcAllowView;
        public event delsvcAllowControl EntsvcAllowControl; 
        public event delsvcUnJoin EntsvcUnJoin;


        #region INetTcpDesktop Members

        public void svcJoin(clsMessageContract mcJoin)
        {
            if (EntsvcJoin != null)
            {
                EntsvcJoin(mcJoin);
            }
        }

        public void svcSendMessage(clsMessageContract mcSendMessage)
        {
            if (EntsvcSendMessage != null)
            {
                EntsvcSendMessage(mcSendMessage);
            }
        }

        public void svcGetUserList(clsMessageContract mcGetUserList)
        {
            if (EntsvcGetUserList != null)
            {
                EntsvcGetUserList(mcGetUserList);
            }
        }

        public void svcSetUserList(clsMessageContract mcSetUserList)
        {
            if (EntsvcSetUserList != null)
            {
                EntsvcSetUserList(mcSetUserList);
            }
        }

        public void svcSelectedDesktop(clsMessageContract mcSelectedDesktop)
        {
            if (EntsvcSelectedDesktop != null)
            {
                EntsvcSelectedDesktop(mcSelectedDesktop);
            }
        }

        public void svcStopControl(clsMessageContract mcStopControl)
        {
            if (EntsvcStopControl != null)
            {
                EntsvcStopControl(mcStopControl);
            }
        }

        public void svcBtnDown(clsMessageContract mcBtnDown)
        {
            if (EntsvcBtnDown != null)
            {
                EntsvcBtnDown(mcBtnDown);
            }

        }

        public void svcBtnUp(clsMessageContract mcBtnUp)
        {
            if (EntsvcBtnUp != null)
            {
                EntsvcBtnUp(mcBtnUp);
            }
        }

        public void svcSendXY(clsMessageContract mcSendXY)
        {
            if (EntsvcSendXY != null)
            {
                EntsvcSendXY(mcSendXY);
            }
        }

        public void svcSendKey(clsMessageContract mcSendKey)
        {
            if (EntsvcSendKey != null)
            {
                EntsvcSendKey(mcSendKey);
            }
        }

        public void svcAllowView(clsMessageContract mcAllowView)
        {
            if (EntsvcAllowView != null)
            {
                EntsvcAllowView(mcAllowView);
            }
        }

        public void svcAllowControl(clsMessageContract mcAllowControl)
        {
            if (EntsvcAllowControl != null)
            {
                EntsvcAllowControl(mcAllowControl);
            }
        }

        public void svcUnJoin(clsMessageContract mcUnJoin)
        {
            if (EntsvcUnJoin != null)
            {
                EntsvcUnJoin(mcUnJoin);
            }
        }

        #endregion
    }
}
