using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ImageSharing.Business.Service.BasicHttp;
using VMuktiService;
using ImageSharing.Business.Service.NetP2P;
using ImageSharing.Business.Service.DataContracts;
using VMuktiAPI;

namespace ImageSharing.Presentation
{
    [Serializable]
    public class P2PImageSharingClient
    {
        object objNetTcpImageSharing = null;

        public INetTcpImageShareChannel channelNettcpImageSharing;

        string UserName;

        int tempcounter = 0;

        public P2PImageSharingClient(string uname, string P2PUri)
        {
            try
            {
                this.UserName = uname;
                RegImageSharingp2pClient(P2PUri);
            }
            catch (Exception exp)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(exp, "P2PImageSharingClient", "P2PImageSharingClient.cs");
            }
        }

        void RegImageSharingp2pClient(string P2PUri)
        {
            try
            {
                NetPeerClient npcDummyImageSharing = new NetPeerClient();
                objNetTcpImageSharing = new clsNetTcpImageSharing();

                ((clsNetTcpImageSharing)objNetTcpImageSharing).EntsvcJoin += new clsNetTcpImageSharing.delsvcJoin(P2PImageSharingClient_EntsvcJoin);
                ((clsNetTcpImageSharing)objNetTcpImageSharing).EntsvcUnJoin += new clsNetTcpImageSharing.delsvcUnJoin(P2PImageSharingClient_EntsvcUnJoin);
                
                channelNettcpImageSharing = (INetTcpImageShareChannel)npcDummyImageSharing.OpenClient<INetTcpImageShareChannel>(P2PUri, P2PUri.ToString().Split(':')[2].Split('/')[2], ref objNetTcpImageSharing);

                while (tempcounter < 20)
                {
                    try
                    {
                        channelNettcpImageSharing.svcJoin(UserName);
                        tempcounter = 20;
                    }
                    catch
                    {
                        tempcounter++;
                        System.Threading.Thread.Sleep(1000);
                    }
                }
            }
            catch (Exception exp)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(exp, "RegImageSharingp2pClient", "P2PImageSharingClient.cs");
            }
        }

        #region P2P Net TCP event handlers
        void P2PImageSharingClient_EntsvcUnJoin(string uName)
        {
            
        }

        void P2PImageSharingClient_EntsvcJoin(string uName)
        {

        }
        #endregion

    }
}
