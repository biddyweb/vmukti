using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VMuktiAPI;
using System.Windows;
using System.IO;

namespace ImageSharing.Business.Service.NetP2P
{
    
    public class clsNetTcpImageSharing : INetTcpImageSharing
    {
        public delegate void delsvcJoin(string uName);
        public delegate void delsvcSendIamge(Stream streamImage);
        public delegate void delsvcUnJoin(string uName);

        public event delsvcJoin EntsvcJoin;
        public event delsvcSendIamge EntsvcSendIamge;
        public event delsvcUnJoin EntsvcUnJoin;

        
        #region Net TCP 

        public void svcJoin(string uname)
        {
            try
            {
                if (EntsvcJoin != null)
                {
                    EntsvcJoin(uname);
                }
            }
            catch (Exception exp)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(exp, "svcJoin", "clsNetTcpImageSharing.cs");
            }
        }

        public void svcSendIamge(Stream streamImage)
        {
            if (EntsvcSendIamge != null)
            {
                EntsvcSendIamge(streamImage);
            }
        }

        public void svcUnJoin(string uname)
        {
            try
            {
                if (EntsvcUnJoin != null)
                {
                    EntsvcUnJoin(uname);
                }
            }
            catch (Exception exp)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(exp, "svcUnJoin", "clsNetTcpImageSharing.cs");
            }
        }

        #endregion
    }
}
