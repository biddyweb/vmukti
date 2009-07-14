using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ImageSharing.Business.Service.NetP2P;
using ImageSharing.Business.Service.BasicHttp;
using ImageSharing.Business.Service.DataContracts;
using VMuktiService;
using System.Windows;
using VMuktiAPI;
using System.IO;
using System.Collections;
using System.ServiceModel;

namespace ImageSharing.Presentation
{
    [Serializable]
    public class ImageSharingDummy
    { 
        object objHttpImageSharing = null;
        object objNetTcpImageSharing = null;

        public INetTcpImageShareChannel channelNettcpImageSharing;

        public VMuktiService.BasicHttpServer HttpImageSharingServer = null;

        List<clsMessage> lstMessage = new List<clsMessage>();
        List<string> lstNodes = new List<string>();
        
        Hashtable hashMessages = new Hashtable();

        string UserName;
        int MyId;

        int tempcounter = 0;

        public ImageSharingDummy(string MyName, string UName, int Id, string netP2pUri, string httpUri)
        {
            try
            {
                UserName = MyName;
                MyId = Id;

                RegHttpServer(httpUri);
                RegNetP2PClient(netP2pUri);

            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "ImageSharingDummy", "ImageSharingDummy.xaml.cs");
            }
        }

        void RegHttpServer(object httpUri)
        {
            try
            {
                objHttpImageSharing = new clsHttpImageSharing();

                ((clsHttpImageSharing)objHttpImageSharing).EntsvcJoin += new clsHttpImageSharing.delsvcJoin(objHttp_EntsvcJoin);
                ((clsHttpImageSharing)objHttpImageSharing).EntsvcSendIamge += new clsHttpImageSharing.delsvcSendIamge(objHttp_EntsvcSendIamge);
                ((clsHttpImageSharing)objHttpImageSharing).EntsvcGetMessages += new clsHttpImageSharing.delsvcGetMessages(objHttp_EntsvcGetMessages);
                ((clsHttpImageSharing)objHttpImageSharing).EntsvcUnJoin += new clsHttpImageSharing.delsvcUnJoin(objHttp_EntsvcUnJoin);

                HttpImageSharingServer = new BasicHttpServer(ref objHttpImageSharing, httpUri.ToString());
                
                HttpImageSharingServer.AddEndPoint<IHttpImageSharing>(httpUri.ToString());
                HttpImageSharingServer.objBasicHttpBinding.TransferMode = TransferMode.Streamed;
                HttpImageSharingServer.OpenServer();
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "RegHttpServer", "ImageSharingDummy.xaml.cs");
            }
        }

        void RegNetP2PClient(object netP2pUri)
        {
            try
            {
                NetPeerClient npcDummyImageSharing = new NetPeerClient();
                objNetTcpImageSharing = new clsNetTcpImageSharing();

                ((clsNetTcpImageSharing)objNetTcpImageSharing).EntsvcJoin += new clsNetTcpImageSharing.delsvcJoin(ImageSharingDummy_EntsvcJoin);
                ((clsNetTcpImageSharing)objNetTcpImageSharing).EntsvcSendIamge += new clsNetTcpImageSharing.delsvcSendIamge(ImageSharingDummy_EntsvcSendIamge);
                ((clsNetTcpImageSharing)objNetTcpImageSharing).EntsvcUnJoin += new clsNetTcpImageSharing.delsvcUnJoin(ImageSharingDummy_EntsvcUnJoin);

                channelNettcpImageSharing = (INetTcpImageShareChannel)npcDummyImageSharing.OpenClient<INetTcpImageShareChannel>(netP2pUri.ToString(), netP2pUri.ToString().Split(':')[2].Split('/')[2], ref objNetTcpImageSharing);

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
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "RegNetP2PClient", "ImageSharingDummy.xaml.cs");

            }
        }

        #region Http Events

        void objHttp_EntsvcJoin(Stream streamUName)
        {
            try
            {
                MemoryStream mmsTemp = new MemoryStream();
                int rmvBytes = 0;
                byte[] byteTemp = fncStreamToByteArry(streamUName);
                for (int i = (byteTemp.Length - 1); i > (byteTemp.Length - 4); i--)
                {
                    if ((char)byteTemp[i] == '#')
                    {
                        rmvBytes += 1;
                    }
                }

                mmsTemp.Write(byteTemp, 0, byteTemp.Length - rmvBytes);
                mmsTemp.Position = 0;

                string uName = fncStreamToString(mmsTemp);
                lstNodes.Add(uName);

                hashMessages.Add(uName, new List<Stream>());
            }
            catch(Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "objHttp_EntsvcJoin", "ImageSharingDummy.xaml.cs");
            }
        }

        void objHttp_EntsvcSendIamge(Stream streamImage)
        {
            try
            {
                MemoryStream mmsTemp = new MemoryStream();
                int rmvBytes = 0;

                byte[] byteTemp = fncStreamToByteArry(streamImage);

                for (int i = (byteTemp.Length - 1); i > (byteTemp.Length - 4); i--)
                {
                    if ((char)byteTemp[i] == '#')
                    {
                        rmvBytes += 1;
                    }
                }

                mmsTemp.Write(byteTemp, 0, byteTemp.Length - rmvBytes);
                mmsTemp.Position = 0;

                channelNettcpImageSharing.svcSendIamge(mmsTemp);
            }
            catch(Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "objHttp_EntsvcSendIamge", "ImageSharingDummy.xaml.cs");
            }
        }

        Stream objHttp_EntsvcGetMessages(Stream streamRecipient)
        {
            try
            {
                MemoryStream mmsTemp = new MemoryStream();
                int rmvBytes = 0;

                byte[] byteTemp = fncStreamToByteArry(streamRecipient);

                for (int i = (byteTemp.Length - 1); i > (byteTemp.Length - 4); i--)
                {
                    if ((char)byteTemp[i] == '#')
                    {
                        rmvBytes += 1;
                    }
                }

                mmsTemp.Write(byteTemp, 0, byteTemp.Length - rmvBytes);
                mmsTemp.Position = 0;

                string strRecipient = fncStreamToString(mmsTemp);

                List<Stream> lstTemp = (List<Stream>)hashMessages[strRecipient];


                if (lstTemp != null && lstTemp.Count>0)
                {
                    Stream mmsMessage = new MemoryStream();
                    mmsMessage = lstTemp[0];
                    lstTemp.RemoveAt(0);

                    hashMessages[strRecipient] = lstTemp;

                    return mmsMessage;
                }
                else
                {
                    Stream objTemp = new MemoryStream();
                    return objTemp;
                }
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "objHttp_EntsvcGetMessages", "ImageSharingDummy.xaml.cs");
                Stream objTemp = new MemoryStream();
                return objTemp;
            }
        }

        void objHttp_EntsvcUnJoin(Stream streamUName)
        {
            try
            {
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "objHttp_EntsvcUnJoin", "ImageSharingDummy.xaml.cs");
            }

        }


        #endregion

        #region NetTcp Events

        void ImageSharingDummy_EntsvcJoin(string uName)
        {

        }

        void ImageSharingDummy_EntsvcSendIamge(Stream streamImage)
        {
            try
            {
                if (lstNodes.Count > 0)
                {

                    MemoryStream mmsImgFinal = new MemoryStream();
                    int id = 1;
                    char del = '#';

                    byte[] byteData = fncStreamToByteArry(streamImage);

                    mmsImgFinal.Write(byteData, 0, byteData.Length);
                    mmsImgFinal.WriteByte((byte)id);
                    mmsImgFinal.WriteByte((byte)del);
                    mmsImgFinal.WriteByte((byte)del);
                    mmsImgFinal.WriteByte((byte)del);
                    mmsImgFinal.Position = 0;

                    byte[] byteImage = null;
                    string uNameImg = string.Empty;
                    int pos = 0;
                    bool blDelimeter = true;

                    for (int i = 0; i < byteData.Length; i++)
                    {
                        if ((char)byteData[i] == '!' && (blDelimeter))        //Checking for delimeter in array
                        {
                            char[] bufName = new char[i];
                            for (int j = 0; j < i; j++)         //Converting half of array to username
                            {
                                bufName[j] = (char)byteData[j];
                            }
                            uNameImg = new string(bufName);

                            byteImage = new byte[byteData.Length - (i + 1)];
                            i += 1;
                            blDelimeter = false;
                        }
                        if (uNameImg != null && uNameImg != "")                        //Converting rest of array to imag array
                        {
                            byteImage[pos] = byteData[i];
                            pos += 1;
                        }
                    }

                    for (int i = 0; i < lstNodes.Count; i++)
                    {
                        if (lstNodes[i] != uNameImg)
                        {
                            List<Stream> lstStream = (List<Stream>)hashMessages[lstNodes[i]];
                            lstStream.Add(mmsImgFinal);
                            hashMessages[lstNodes[i]] = lstStream;
                        }
                    }
                }
            }
            catch(Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "ImageSharingDummy_EntsvcSendIamge", "ImageSharingDummy.xaml.cs");
            }
        }

        void ImageSharingDummy_EntsvcUnJoin(string uName)
        {

        }

        #endregion

        #region Supported Functions

        string fncStreamToString(Stream streamInput)
        {
            try
            {

                byte[] byteArry = fncStreamToByteArry(streamInput);

                //convert byte[] to string

                char[] buffer = new char[byteArry.Length];

                for (int j = 0; j < byteArry.Length; j++)
                {
                    buffer[j] = (char)byteArry[j];
                }

                return new string(buffer);

            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "fncStreamToString", "ImageSharingDummy.xaml.cs");
                return null;
            }
        }

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
                VMuktiHelper.ExceptionHandler(ex, "fncStringToStream", "ImageSharingDummy.xaml.cs");
                return null;
            }
        }

        byte[] fncStreamToByteArry(Stream streamInput)
        {
            try
            {
                List<byte> myBytes = new List<byte>();
                int num;
                //streamInput.Position = 0;
                while ((num = streamInput.ReadByte()) != -1)
                {
                    myBytes.Add((byte)num);
                }

                return myBytes.ToArray();
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "fncStreamToByteArry", "ImageSharingDummy.xaml.cs");
                return null;
            }
        }

        #endregion

    }
}
