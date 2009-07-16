using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VMuktiService;
using System.ServiceModel;
using ImageSharing.Business.Service.DataContracts;
using VMuktiAPI;
using System.Windows;
using System.IO;


namespace ImageSharing.Business.Service.BasicHttp
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)] 
    public class clsHttpImageSharing : IHttpImageSharing
    {
        public delegate void delsvcJoin(Stream streamUName);
        public delegate void delsvcSendIamge(Stream streamImage);
        public delegate Stream delsvcGetMessages(Stream streamRecipient);
        public delegate void delsvcUnJoin(Stream streamUName);


        public event delsvcJoin EntsvcJoin;
        public event delsvcSendIamge EntsvcSendIamge;
        public event delsvcGetMessages EntsvcGetMessages;
        public event delsvcUnJoin EntsvcUnJoin;




        #region IHttpImageSharing Members

        public void svcJoin(Stream streamUName)
        {
            try
            {
                if (EntsvcJoin != null)
                {
                    EntsvcJoin(streamUName);
                }
            }
            catch
            {
            }
        }

        public void svcSendIamge(Stream streamImage)
        {
            try
            {
                if (EntsvcSendIamge != null)
                {
                    EntsvcSendIamge(streamImage);
                }
            }
            catch
            {
            }

        }

        public Stream svcGetMessages(Stream streamRecipient)
        {
            try
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
            catch
            {
                return null;
            }
        }

        public void svcUnJoin(Stream streamUName)
        {
            try
            {
                if (EntsvcUnJoin != null)
                {
                    EntsvcUnJoin(streamUName);
                }
            }
            catch
            {
            }
        }

        #endregion
    }
}
