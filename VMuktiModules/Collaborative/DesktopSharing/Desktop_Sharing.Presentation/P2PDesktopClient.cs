using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Desktop_Sharing.Business.Service.NetP2P;
using Desktop_Sharing.Business.Service.MessageContract;
using VMuktiService;
using VMuktiAPI;
using System.IO;

namespace Desktop_Sharing.Presentation
{
    [Serializable]
    public class P2PDesktopClient
    {
        object objNetTcpDesktop = null;

        public INetTcpDesktopChannel channelNettcpDesktop;

        string UserName;

        int tempcounter = 0;

        public P2PDesktopClient(string uname, string P2PUri)
        {
            try
            {
                this.UserName = uname;
                RegDesktopp2pClient(P2PUri);
            }
            catch (Exception exp)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(exp, "P2PDesktopClient", "P2PDesktopClient.cs");
            }
        }

        void RegDesktopp2pClient(string P2PUri)
        {
            try
            {
                NetPeerClient npcDummyDesktop = new NetPeerClient();
                objNetTcpDesktop = new clsNetTcpDesktop();
                ((clsNetTcpDesktop)objNetTcpDesktop).EntsvcJoin += new clsNetTcpDesktop.delsvcJoin(P2PDesktopClient_EntsvcJoin);
                ((clsNetTcpDesktop)objNetTcpDesktop).EntsvcUnJoin += new clsNetTcpDesktop.delsvcUnJoin(P2PDesktopClient_EntsvcUnJoin);

                channelNettcpDesktop = (INetTcpDesktopChannel)npcDummyDesktop.OpenClient<INetTcpDesktopChannel>(P2PUri, P2PUri.ToString().Split(':')[2].Split('/')[2], ref objNetTcpDesktop);

                while (tempcounter < 20)
                {
                    try
                    {
                        //Stream mmsUName = fncStringToStream(UserName);

                        //channelNettcpDesktop.svcJoin(mmsUName);
                        #region msgContract
                        clsMessageContract objContract = new clsMessageContract();
                        objContract.blControl = false;
                        objContract.blView = false;
                        objContract.key = 0;
                        objContract.mouseButton = 0;
                        objContract.stremImage = new MemoryStream();
                        objContract.strFrom = UserName;
                        objContract.strTo = "";
                        objContract.strType = "";
                        objContract.x = 0.0;
                        objContract.y = 0.0;
                        #endregion msgContract

                        channelNettcpDesktop.svcJoin(objContract);

                        tempcounter = 20;
                    }
                    catch
                    {
                        tempcounter++;
                        System.Threading.Thread.Sleep(1000);
                    }
                }
            }
            catch
            {

            }
        }

        #region nettcp events

        void P2PDesktopClient_EntsvcUnJoin(clsMessageContract objContract)
        {

        }

        void P2PDesktopClient_EntsvcJoin(clsMessageContract objContract)
        {

        }

        #endregion

        #region Supported Functions

        Stream fncStringToStream(string strInput)
        {
            try
            {
                int length = strInput.Length;
                byte[] resultBytes = new byte[length];

                for (int i = 0; i < length; i++)
                {
                    resultBytes[i] = (byte)strInput[i];
                }

                MemoryStream mmsConvert = new MemoryStream(resultBytes);

                return mmsConvert;
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "fncStringToStream", "P2PDesktopClient.cs");
                return null;
            }
        }

        #endregion
    }
}
