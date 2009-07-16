using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Drawing;
using System.IO;
using Microsoft.Win32;
using VMuktiService;
using ImageSharing.Business.Service.NetP2P;
using ImageSharing.Business.Service.DataContracts;
using ImageSharing.Business.Service.BasicHttp;
using VMuktiAPI;
using System.ServiceModel;
using System.ComponentModel;


namespace ImageSharing.Presentation
{
    /// <summary>
    /// Interaction logic for ctlImageSharing.xaml
    /// </summary>
    /// 
    public enum ModulePermissions
    {
        Add = 0, 
        Edit = 1,
        Delete = 2,
        View = 3
    }

    public partial class ctlImageSharing : System.Windows.Controls.UserControl
    {
        object objNetTcpImageShare;
        INetTcpImageShareChannel channelNetTcp;
        IHttpImageSharing channelHttp;

        System.Windows.Threading.DispatcherTimer dispTimer = new System.Windows.Threading.DispatcherTimer(System.Windows.Threading.DispatcherPriority.Normal);
        System.Windows.Threading.DispatcherTimer dispHttpTimer = new System.Windows.Threading.DispatcherTimer(System.Windows.Threading.DispatcherPriority.Normal);


        byte[] tempArry;
        byte[] ImageArry;
        
        List<byte[]> lstImage;
        List<byte[]> lstImgBlock;
        List<ImageTrack> lstImageTrack;

        string FilePath;
        string strRole;
                
        int temp;
        int tempcounter;
        
        int ImgTag;
        int timerTag;
        int prevTag;
        int curImg;

        public string strUri;

        bool blFirstImg;

       // System.Threading.Thread tHostImageSharing = null;

        public delegate void DelSetImage(byte[] imgArry);
        public DelSetImage objSetImage;

        public delegate void DelImageBlock(List<object> lstData);
        public DelImageBlock objImageBlock;

        public delegate void DelGetMessage(Stream myMessages);
        public DelGetMessage objDelGetMsg;

        public delegate void DelAddIamge(List<object> lstData);
        public DelAddIamge objDelAddImage;

        BackgroundWorker bgHostService;
        System.Threading.Thread thGlobalVariable;

        public ctlImageSharing(VMuktiAPI.PeerType PeerType, string uri, ModulePermissions[] MyPermissions, string Role)
        {
            try
            {
                InitializeComponent();

                thGlobalVariable = new System.Threading.Thread(new System.Threading.ThreadStart(GlobalVariable));
                thGlobalVariable.Start();

                bgHostService = new BackgroundWorker();

                lstImageTrack = new List<ImageTrack>();

                VMuktiAPI.VMuktiHelper.RegisterEvent("SignOut").VMuktiEvent += new VMuktiEvents.VMuktiEventHandler(ctlImageSharing_VMuktiEvent);

                Application.Current.Exit += new ExitEventHandler(Current_Exit);

                btnPrev.IsEnabled = false;
                btnPrev.Click += new RoutedEventHandler(btnPrev_Click);
                btnPlay.Click += new RoutedEventHandler(btnPlay_Click);
                btnPlay.Tag = "play";
                btnNext.Click += new RoutedEventHandler(btnNext_Click);
                btnAddImage.Click += new RoutedEventHandler(btnAddImage_Click);
                btnSave.Click += new RoutedEventHandler(btnSave_Click);
                btnDesktop.Click += new RoutedEventHandler(btnDesktop_Click);
                btnDesktop.ToolTip = "Set Image As Your Desktop Background";

                //objSetImage = new DelSetImage(delSetImage);
                //objImageBlock = new DelImageBlock(delImageBlock);
                //objDelGetMsg = new DelGetMessage(delGetMessage);
                //objDelAddImage = new DelAddIamge(delAddImage);

                strRole = Role;

                bgHostService.DoWork += new DoWorkEventHandler(bgHostService_DoWork);

                //tHostImageSharing = new System.Threading.Thread(new System.Threading.ParameterizedThreadStart(HostImageService));
                List<object> lstParams = new List<object>();
                lstParams.Add(PeerType);
                lstParams.Add(uri);
                lstParams.Add(MyPermissions);

                bgHostService.RunWorkerAsync(lstParams);
                //tHostImageSharing.Start(lstParams);

                dispTimer.Interval = TimeSpan.FromSeconds(2);
                dispTimer.Tick += new EventHandler(dispTimer_Tick);
                this.Loaded += new RoutedEventHandler(ctlImageSharing_Loaded);
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "ctlImageSharing", "ctlImageSharing.xaml.cs");
            }
        }

        #region Global Variable Initialize

        void GlobalVariable()
        {
            try
            {
                objNetTcpImageShare = new clsNetTcpImageSharing();
                lstImage = new List<byte[]>();
                lstImgBlock = new List<byte[]>();
                blFirstImg = false;

                objSetImage = new DelSetImage(delSetImage);
                objImageBlock = new DelImageBlock(delImageBlock);
                objDelGetMsg = new DelGetMessage(delGetMessage);
                objDelAddImage = new DelAddIamge(delAddImage);
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "GlobalVariable", "ctlImageSharing.xaml.cs");
            }

        }

        #endregion

        #region BWHostService

        void bgHostService_DoWork(object sender, DoWorkEventArgs e)
        {

            List<object> lstTempObj = (List<object>)e.Argument;
            strUri = lstTempObj[1].ToString();

            try
            {
                if ((VMuktiAPI.PeerType)lstTempObj[0] == VMuktiAPI.PeerType.NodeWithNetP2P || (VMuktiAPI.PeerType)lstTempObj[0] == VMuktiAPI.PeerType.BootStrap || (VMuktiAPI.PeerType)lstTempObj[0] == VMuktiAPI.PeerType.SuperNode)
                {
                    NetPeerClient npcImageShare = new NetPeerClient();

                    ((clsNetTcpImageSharing)objNetTcpImageShare).EntsvcJoin += new clsNetTcpImageSharing.delsvcJoin(ctlImageSharing_EntsvcJoin);
                    ((clsNetTcpImageSharing)objNetTcpImageShare).EntsvcSendIamge += new clsNetTcpImageSharing.delsvcSendIamge(ctlImageSharing_EntsvcSendIamge);
                    ((clsNetTcpImageSharing)objNetTcpImageShare).EntsvcUnJoin += new clsNetTcpImageSharing.delsvcUnJoin(ctlImageSharing_EntsvcUnJoin);


                    channelNetTcp = (INetTcpImageShareChannel)npcImageShare.OpenClient<INetTcpImageShareChannel>(strUri, strUri.ToString().Split(':')[2].Split('/')[2], ref objNetTcpImageShare);

                    while (temp < 20)
                    {
                        try
                        {
                            channelNetTcp.svcJoin(VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName);

                            temp = 20;
                        }
                        catch
                        {
                            temp++;
                            System.Threading.Thread.Sleep(1000);
                        }
                    }
                }

                else
                {
                    BasicHttpClient bhcImageSharing = new BasicHttpClient();
                    bhcImageSharing.NewBasicHttpBinding().TransferMode = TransferMode.Streamed;
                    
                    channelHttp = (IHttpImageSharing)bhcImageSharing.OpenClient<IHttpImageSharing>(strUri);

                    while (tempcounter < 20)
                    {
                        try
                        {

                            MemoryStream mmsFinal = new MemoryStream();
                            char del = '#';
                            Stream mmsUName = fncStringToStream(VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName);
                            mmsUName.Position = 0;
                            byte[] byteName = fncStreamToByteArry(mmsUName);

                            mmsFinal.Write(byteName, 0, byteName.Length);
                            mmsFinal.WriteByte((byte)del);
                            mmsFinal.WriteByte((byte)del);
                            mmsFinal.WriteByte((byte)del);
                            mmsFinal.Position = 0;

                            channelHttp.svcJoin(mmsFinal);
                            tempcounter = 20;
                        }
                        catch
                        {
                            tempcounter++;
                            System.Threading.Thread.Sleep(1000);
                        }
                    }
                    
                    dispHttpTimer.Interval = TimeSpan.FromSeconds(2);
                    dispHttpTimer.Tick += new EventHandler(dispHttpTimer_Tick);
                    dispHttpTimer.Start();

                }
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "HostImageService", "ctlImageSharing.xaml.cs");
            }
        }

        #endregion

        #region Host Service

        public void HostImageService(object lstParams)
        {
            //List<object> lstTempObj = (List<object>)lstParams;
            //strUri = lstTempObj[1].ToString();

            //try
            //{
            //    if ((VMuktiAPI.PeerType)lstTempObj[0] == VMuktiAPI.PeerType.NodeWithNetP2P || (VMuktiAPI.PeerType)lstTempObj[0] == VMuktiAPI.PeerType.BootStrap || (VMuktiAPI.PeerType)lstTempObj[0] == VMuktiAPI.PeerType.SuperNode)
            //    {
            //        NetPeerClient npcImageShare = new NetPeerClient();

            //        ((clsNetTcpImageSharing)objNetTcpImageShare).EntsvcJoin += new clsNetTcpImageSharing.delsvcJoin(ctlImageSharing_EntsvcJoin);
            //        ((clsNetTcpImageSharing)objNetTcpImageShare).EntsvcSendIamge += new clsNetTcpImageSharing.delsvcSendIamge(ctlImageSharing_EntsvcSendIamge);
            //        ((clsNetTcpImageSharing)objNetTcpImageShare).EntsvcUnJoin += new clsNetTcpImageSharing.delsvcUnJoin(ctlImageSharing_EntsvcUnJoin);


            //        channelNetTcp = (INetTcpImageShareChannel)npcImageShare.OpenClient<INetTcpImageShareChannel>(strUri, strUri.ToString().Split(':')[2].Split('/')[1], ref objNetTcpImageShare);

            //        while (temp < 20)
            //        {
            //            try
            //            {
            //                channelNetTcp.svcJoin(VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName);

            //                temp = 20;
            //            }
            //            catch
            //            {
            //                temp++;
            //                System.Threading.Thread.Sleep(1000);
            //            }
            //        }
            //    }

            //    else
            //    {
            //        BasicHttpClient bhcImageSharing = new BasicHttpClient();
            //        bhcImageSharing.NewBasicHttpBinding().TransferMode = TransferMode.Streamed;
                    
            //        channelHttp = (IHttpImageSharing)bhcImageSharing.OpenClient<IHttpImageSharing>(strUri);

            //        while (tempcounter < 20)
            //        {
            //            try
            //            {

            //                MemoryStream mmsFinal = new MemoryStream();
            //                char del = '#';
            //                Stream mmsUName = fncStringToStream(VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName);
            //                mmsUName.Position = 0;
            //                byte[] byteName = fncStreamToByteArry(mmsUName);

            //                mmsFinal.Write(byteName, 0, byteName.Length);
            //                mmsFinal.WriteByte((byte)del);
            //                mmsFinal.WriteByte((byte)del);
            //                mmsFinal.WriteByte((byte)del);
            //                mmsFinal.Position = 0;

            //                channelHttp.svcJoin(mmsFinal);
            //                tempcounter = 20;
            //            }
            //            catch
            //            {
            //                tempcounter++;
            //                System.Threading.Thread.Sleep(1000);
            //            }
            //        }
                    
            //        dispHttpTimer.Interval = TimeSpan.FromSeconds(2);
            //        dispHttpTimer.Tick += new EventHandler(dispHttpTimer_Tick);
            //        dispHttpTimer.Start();

            //    }
            //}
            //catch (Exception ex)
            //{
            //    VMuktiHelper.ExceptionHandler(ex, "HostImageService", "ctlImageSharing.xaml.cs");
            //}
        }

        #endregion
        
        #region UI control events

        void ctlImageSharing_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                if (((Grid)(this.Parent)).Width > 0)
                {
                    this.Width = ((Grid)(this.Parent)).Width;
                }
                ((Grid)(this.Parent)).SizeChanged += new SizeChangedEventHandler(ctlImageSharing_SizeChanged);
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "ctlImageSharing_Loaded", "ctlImageSharing.xaml.cs");
            }
        }

        void ctlImageSharing_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            try
            {
                if (e.NewSize.Width > 0)
                {
                    this.Width = ((Grid)(this.Parent)).ActualWidth;
                }
                
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "ctlImageSharing_SizeChanged", "ctlImageSharing.xaml.cs");
            }
        }

        void btnAddImage_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                List<byte[]> lstarry = new List<byte[]>();
                byte[] arr = new byte[5000];

                OpenFileDialog objFileDialog = new OpenFileDialog();
                objFileDialog.Title = "Open Image File";
                objFileDialog.Filter = "Bitmap Files|*.bmp" + "|Enhanced Windows MetaFile|*.emf" + "|Exchangeable Image File|*.exif" + "|Gif Files|*.gif|Icons|*.ico|JPEG Files|*.jpg" + "|PNG Files|*.png|TIFF Files|*.tif|Windows MetaFile|*.wmf";
                objFileDialog.DefaultExt = "jpg";
                objFileDialog.ShowDialog();
                FilePath = objFileDialog.FileName;

                if (FilePath != "")
                {
                    ctlImage objImage = new ctlImage(FilePath, ImgTag);
                    prevTag = ImgTag;
                    ImgTag += 1;

                    objImage.EntSelectedImage += new ctlImage.DelSelectedImage(objImage_EntSelectedImage);

                    tempArry = objImage.SetImage(FilePath);
                    objImage.ShowImage(tempArry);
                    stackMain.Children.Add(objImage);
                    lstImage.Add(tempArry);

                    Stream sname = fncStringToStream(VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName);
                    sname.Position = 0;
                    byte[] NameByte = fncStreamToByteArry(sname);
                    char delimeter = '!';
                    char del = '#';

                    if (VMuktiAPI.VMuktiInfo.CurrentPeer.CurrPeerType == VMuktiAPI.PeerType.NodeWithNetP2P || VMuktiAPI.VMuktiInfo.CurrentPeer.CurrPeerType == VMuktiAPI.PeerType.BootStrap || VMuktiAPI.VMuktiInfo.CurrentPeer.CurrPeerType == VMuktiAPI.PeerType.SuperNode)
                    {

                        MemoryStream FinalStrem = new MemoryStream();
                        FinalStrem.Write(NameByte, 0, NameByte.Length);
                        FinalStrem.WriteByte((byte)delimeter);
                        FinalStrem.Write(tempArry, 0, tempArry.Length);
                        FinalStrem.Position = 0;

                        channelNetTcp.svcSendIamge(FinalStrem);
                    }
                    else
                    {
                        MemoryStream finalImage = new MemoryStream();
                        finalImage.Write(NameByte, 0, NameByte.Length);
                        finalImage.WriteByte((byte)delimeter);
                        finalImage.Write(tempArry, 0, tempArry.Length);
                        finalImage.WriteByte((byte)del);
                        finalImage.WriteByte((byte)del);
                        finalImage.WriteByte((byte)del);
                        finalImage.Position = 0;

                        channelHttp.svcSendIamge(finalImage);
                    }
                }
            }
            catch (Exception ex)
            {

                VMuktiHelper.ExceptionHandler(ex, "btnAddImage_Click", "ctlImageSharing.xaml.cs");
            }
        }

        void btnDesktop_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string strTempPath = System.IO.Path.Combine(System.IO.Path.GetTempPath(), "tempImage.jpg");
                MemoryStream mms = new MemoryStream(lstImage[curImg]);
                
                Bitmap img = (Bitmap)System.Drawing.Image.FromStream(mms);
                img.Save(strTempPath, System.Drawing.Imaging.ImageFormat.Jpeg);
                
                mms.Close();
                img.Dispose();

                string s = "Stretched";
                Wallpaper.Style s2 = (Wallpaper.Style)Enum.Parse(typeof(Wallpaper.Style), s, false);
                Wallpaper.Set(new Uri(strTempPath), s2);

                MessageBox.Show("Image Has Been Set As Your Desktop", "VMukti Says:- ImageSharing");
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "btnDesktop_Click", "ctlImageSharing.xaml.cs");
            }
        }

        void btnSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                SaveFileDialog objSave = new SaveFileDialog();
                objSave.Filter = "Bitmap Files|*.bmp" + "|Enhanced Windows MetaFile|*.emf" + "|Exchangeable Image File|*.exif" + "|Gif Files|*.gif|Icons|*.ico|JPEG Files|*.jpg" + "|PNG Files|*.png|TIFF Files|*.tif|Windows MetaFile|*.wmf";
                objSave.FilterIndex = 1;
                objSave.RestoreDirectory = true;

                if (objSave.ShowDialog() == true)
                {
                    MemoryStream mms = new MemoryStream(lstImage[curImg]);
                    Bitmap img = (Bitmap)System.Drawing.Image.FromStream(mms);
                    img.Save(objSave.FileName, System.Drawing.Imaging.ImageFormat.Jpeg);
                    mms.Close();
                    img.Dispose();
                }
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "btnSave_Click", "ctlImageSharing.xaml.cs");
            }
        }

        void dispTimer_Tick(object sender, EventArgs e)
        {
            try
            {
                if (timerTag < ImgTag)
                {
                    BitmapImage bmi = new BitmapImage();
                    bmi.BeginInit();
                    bmi.StreamSource = new MemoryStream(lstImage[timerTag]);
                    bmi.EndInit();
                    ImgFull.Source = bmi;

                    timerTag += 1;
                }
                else
                {
                    timerTag = 0;
                }
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "dispTimer_Tick", "ctlImageSharing.xaml.cs");
            }
        }

        void btnNext_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                if (!blFirstImg)
                {
                    BitmapImage bmi = new BitmapImage();
                    bmi.BeginInit();
                    bmi.StreamSource = new MemoryStream(lstImage[curImg]);
                    bmi.EndInit();
                    ImgFull.Source = bmi;

                    blFirstImg = true;
                    
                }
                else if (blFirstImg)
                {
                    BitmapImage bmi = new BitmapImage();
                    bmi.BeginInit();
                    bmi.StreamSource = new MemoryStream(lstImage[curImg+1]);
                    bmi.EndInit();
                    ImgFull.Source = bmi;

                    curImg += 1;

                    if (curImg == 1)
                    {
                        btnPrev.IsEnabled = true;
                    }
                    
                    if (curImg == (ImgTag-1))
                    {
                        btnNext.IsEnabled = false;
                    }
                }
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "btnNext_Click", "ctlImageSharing.xaml.cs");
            }
        }

        void btnPlay_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                
                if (btnPlay.Tag.ToString() == "play")
                {
                    timerTag = 0;
                    dispTimer.Start();
                    btnPlay.Tag = "pause";

                    btnPlay.ToolTip = "Pause";
                }
                else if (btnPlay.Tag.ToString() == "pause")
                {
                    dispTimer.Stop();
                    btnPlay.Tag = "play";

                    btnPlay.ToolTip = "Play";
                }
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "btnPlay_Click", "ctlImageSharing.xaml.cs");
            }
        }

        void btnPrev_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (curImg == 0)
                {
                    BitmapImage bmi = new BitmapImage();
                    bmi.BeginInit();
                    bmi.StreamSource = new MemoryStream(lstImage[curImg]);
                    bmi.EndInit();
                    ImgFull.Source = bmi;

                    btnPrev.IsEnabled = false;
                }
                else
                {
                    BitmapImage bmi = new BitmapImage();
                    bmi.BeginInit();
                    bmi.StreamSource = new MemoryStream(lstImage[curImg - 1]);
                    bmi.EndInit();
                    ImgFull.Source = bmi;

                    curImg -= 1;

                    if (curImg != ImgTag)
                    {
                        btnNext.IsEnabled = true;
                    }

                    if (curImg == 0)
                    {
                        btnPrev.IsEnabled = false;
                    }
                }
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "btnPrev_Click", "ctlImageSharing.xaml.cs");
            }
        }


        #endregion

        #region Functions , Delegates

        void objImage_EntSelectedImage(int ImgTag)
        {
            try
            {
                ImageArry = lstImage[ImgTag];
                curImg = ImgTag;
                delSetImage(ImageArry);
                
            }
            catch (Exception ex)
            {

                VMuktiHelper.ExceptionHandler(ex, "objImage_EntSelectedImage", "ctlImageSharing.xaml.cs");
            }
        }

        void singleBlock_EntSelectedImage(int ImgTag)
        {
           try
            {
                ImageArry = lstImage[ImgTag];
                curImg = ImgTag;
                delSetImage(ImageArry);
            }
            catch (Exception ex)
            {

                VMuktiHelper.ExceptionHandler(ex, "singleBlock_EntSelectedImage", "ctlImageSharing.xaml.cs");
            }
        }

        void lastBlock_EntSelectedImage(int ImgTag)
        {
            try
            {
                ImageArry = lstImage[ImgTag];
                curImg = ImgTag;
                delSetImage(ImageArry);
            }
            catch (Exception ex)
            {

                VMuktiHelper.ExceptionHandler(ex, "lastBlock_EntSelectedImage", "ctlImageSharing.xaml.cs");
            }
        }

        void delSetImage(byte[] imgArry)
        {
            try
            {
                BitmapImage bmi = new BitmapImage();
                bmi.BeginInit();
                bmi.StreamSource = new MemoryStream(imgArry);
                bmi.EndInit();
                ImgFull.Source = bmi;
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "delSetImage", "ctlImageSharing.xaml.cs");
            }
        }

        void delAddImage(List<object> lstData)
        {
            try
            {
                ctlImage objImage = new ctlImage("", ImgTag);
                prevTag = ImgTag;
                ImgTag += 1;

                objImage.ShowImage((byte[])lstData[0]);
                stackMain.Children.Add(objImage);
                lstImage.Add((byte[])lstData[0]);
                objImage.EntSelectedImage += new ctlImage.DelSelectedImage(objImage_EntSelectedImage);
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "delAddImage", "ctlImageSharing.xaml.cs");
            }
        }

        void delShareImage(List<byte[]> lstImgArry)
        {
            try
            {
                byte[] fullImg;
                int length = 0;
                int totalByte = 0;
                for (int i = 0; i < lstImgArry.Count; i++)
                {
                    length += lstImgArry[i].Length;
                }
                fullImg = new byte[length];
                for (int i = 0; i < lstImgArry.Count; i++)
                {
                    for (int j = 0; j < lstImgArry[i].Length; j++)
                    {
                        fullImg[totalByte++] = lstImgArry[i][j];
                        
                    }
                }

                ctlImage objImage = new ctlImage("", ImgTag);
                ImgTag += 1;

                objImage.ShowImage(fullImg);
                stackMain.Children.Add(objImage);
                lstImage.Add(fullImg);
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "delShareImage", "ctlImageSharing.xaml.cs");
            }
        }

        void delImageBlock(List<object> lstData)
        {
            try
            {
                int i = 0;

                //------------------------------------------------------
                //Check whether user exist in the list of current downloading img of perticular user

                bool checkUser = false;
                for (i = 0; i < lstImageTrack.Count; i++)
                {
                    if (lstImageTrack[i].uName == lstData[0].ToString())
                    {
                        checkUser = true;
                        break;
                    }
                }

                //------------------------------------------------------

                //------------------------------------------------------

                //Now If user is not present in current downloading list of users

                if (!checkUser)
                {
                    //Check IF sent block of img is not single block

                    if ((bool)lstData[3] == false)
                    {
                     
                        
                        ImageTrack objImgTrack = new ImageTrack();
                        if (objImgTrack.FullImgBlock == null)
                        {
                            objImgTrack.FullImgBlock = new byte[int.Parse(lstData[1].ToString())];
                            objImgTrack.pointer = 0;
                        }

                        objImgTrack.uName = lstData[0].ToString();
                        for (int j = 0; j < ((byte[])lstData[2]).Length; j++)
                        {
                            objImgTrack.FullImgBlock[objImgTrack.pointer++] = ((byte[])lstData[2])[j];
                        }
                        lstImageTrack.Add(objImgTrack);

                    }

                    //Check IF blcok of img is single

                    else if ((bool)lstData[3])
                    {
                        ctlImage objImage = new ctlImage("", ImgTag);
                        prevTag = ImgTag;
                        ImgTag += 1;

                        objImage.ShowImage((byte[])lstData[2]);
                        stackMain.Children.Add(objImage);
                        lstImage.Add((byte[])lstData[2]);

                        objImage.EntSelectedImage +=new ctlImage.DelSelectedImage(singleBlock_EntSelectedImage);
                    }
                }

                //IF user is all ready exist in list of downloading imag of user
                    
                else if (checkUser)
                {
                    try
                    {
                        if ((bool)lstData[3] == false)
                        {
                          
                            for (int j = 0; j < ((byte[])lstData[2]).Length; j++)
                            {
                                lstImageTrack[i].FullImgBlock[lstImageTrack[i].pointer++] = ((byte[])lstData[2])[j];
                            }

                          
                        }
                        else if ((bool)lstData[3])
                        {
                           
                            for (int j = 0; j < ((byte[])lstData[2]).Length; j++)
                            {
                                lstImageTrack[i].FullImgBlock[lstImageTrack[i].pointer++] = ((byte[])lstData[2])[j];
                            }

                            ctlImage objImage = new ctlImage("", ImgTag);
                            prevTag = ImgTag;
                            ImgTag += 1;

                            objImage.ShowImage(lstImageTrack[i].FullImgBlock);
                            stackMain.Children.Add(objImage);
                            lstImage.Add(lstImageTrack[i].FullImgBlock);
                            objImage.EntSelectedImage +=new ctlImage.DelSelectedImage(lastBlock_EntSelectedImage);

                            fncRemoveUser(lstData[0].ToString());
                        }
                    }
                    catch(Exception ex)
                    {

                        VMuktiHelper.ExceptionHandler(ex, "delImageBlock", "ctlImageSharing.xaml.cs");
                    }
                }
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "delImageBlock", "ctlImageSharing.xaml.cs");
            }
        }

        private void fncRemoveUser(string uName)
        {
            try
            {
                int i = 0;
                for (i = 0; i < lstImageTrack.Count; i++)
                {
                    if (lstImageTrack[i].uName == uName)
                    {
                        break;
                    }
                }
                lstImageTrack.RemoveAt(i);

                
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "fncRemoveUser", "ctlImageSharing.xaml.cs");
            }
        }

        void delGetMessage(Stream myMessages)
        {
            try
            {
              

                int rmvBytes = 0;
                byte[] myBytes = fncStreamToByteArry(myMessages);

                for (int i = (myBytes.Length - 1); i > (myBytes.Length - 4); i--)
                {
                    if ((char)myBytes[i] == '#')
                    {
                        rmvBytes += 1;
                    }
                }

                string str = myBytes[myBytes.Length - (rmvBytes + 1)].ToString();

                if (str == "1")
                {
                    MemoryStream mmsTemp = new MemoryStream();
                    mmsTemp.Write(myBytes, 0, myBytes.Length - (rmvBytes + 1));
                    mmsTemp.Position = 0;

                    byte[] byteData = mmsTemp.ToArray();
                    mmsTemp.Dispose();

                    byte[] byteImage = null;
                    string uNameImg = string.Empty;
                    int pos = 0;
                    bool blDelimeter = true;

                    for (int i = 0; i < byteData.Length; i++)
                    {
                        if ((char)byteData[i] == '!' && (blDelimeter))
                        {
                            char[] bufName = new char[i];
                            for (int j = 0; j < i; j++)
                            {
                                bufName[j] = (char)byteData[j];
                            }
                            uNameImg = new string(bufName);
                            byteImage = new byte[byteData.Length - (i + 1)];
                            i += 1;
                            blDelimeter = false;
                        }
                        if (uNameImg != null && uNameImg != "")
                        {
                            byteImage[pos] = byteData[i];
                            pos += 1;
                        }
                    }

                    if (uNameImg != VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName)
                    {

                        List<object> lstData = new List<object>();
                        lstData.Add(byteImage);

                        this.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, objDelAddImage, lstData);

                    }
                }
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "delGetMessage", "ctlImageSharing.xaml.cs");
            }
            finally
            {
                dispHttpTimer.Start();
            }
        }

        #endregion

        #region supported functions

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

                Stream mmsConvert = new MemoryStream(resultBytes);
                mmsConvert.Position = 0;
                return mmsConvert;
            }
            catch (Exception ex)
            {

                VMuktiHelper.ExceptionHandler(ex, "fncStringToStream", "ctlImageSharing.xaml.cs");

                return null;
            }
        }

        byte[] fncStreamToByteArry(Stream streamInput)
        {
            try
            {
                List<byte> myBytes = new List<byte>();
                int num;
                while ((num = streamInput.ReadByte()) != -1)
                {
                    myBytes.Add((byte)num);
                }
                return myBytes.ToArray();
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "fncStreamToByteArry", "ctlImageSharing.xaml.cs");
                return null;
            }
        }

        string fncStreamToString(Stream streamInput)
        {
            try
            {
                byte[] byteArry = fncStreamToByteArry(streamInput);

                char[] buffer = new char[byteArry.Length];

                for (int j = 0; j < byteArry.Length; j++)
                {
                    buffer[j] = (char)byteArry[j];
                }

                return new string(buffer);

            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "fncStreamToString", "ctlImageSharing.xaml.cs");
                return null;
            }
        }

        #endregion

        #region NetTCP WCF Events

        void ctlImageSharing_EntsvcJoin(string uName)
        {
            try { }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "ctlImageSharing_EntsvcJoin", "ctlImageSharing.xaml.cs");
            }
        }

        void ctlImageSharing_EntsvcSendIamge(Stream streamImage)
        {
            try
            {
                byte[] byteData = fncStreamToByteArry(streamImage);
                byte[] byteImage = null;
                string uName = string.Empty;
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
                        uName = new string(bufName);
                        byteImage = new byte[byteData.Length - (i + 1)];
                        i += 1;
                        blDelimeter = false;
                    }
                    if (uName != null && uName != "")                        //Converting rest of array to imag array
                    {
                        byteImage[pos] = byteData[i];
                        pos += 1;
                    }
                }

                if (uName != VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName)
                {

                    List<object> lstData = new List<object>();
                    lstData.Add(byteImage);

                    this.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, objDelAddImage, lstData);

                }
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "ctlImageSharing_EntsvcSendIamge", "ctlImageSharing.xaml.cs");
            }
        }

        void ctlImageSharing_EntsvcUnJoin(string uName)
        {
            try
            {
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "ctlImageSharing_EntsvcUnJoin", "ctlImageSharing.xaml.cs");
            }
        }

        #endregion

        #region Close

        void Current_Exit(object sender, ExitEventArgs e)
        {
            try
            {
                if (channelNetTcp != null)
                {
                    channelNetTcp.Close();
                    channelNetTcp.Dispose();
                    channelNetTcp = null;
                }
                if (channelHttp != null)
                {
                    channelHttp = null;
                }
                if (dispHttpTimer != null)
                {
                    dispHttpTimer.Stop();
                }

                VMuktiAPI.VMuktiHelper.UnRegisterEvent("SignOut");
            }
            catch(Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "Current_Exit", "ctlImageSharing.xaml.cs");
            }
        }

        void ctlImageSharing_VMuktiEvent(object sender, VMuktiEventArgs e)
        {
            try
            {
                ClosePod();
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "ctlImageSharing_VMuktiEvent", "ctlImageSharing.xaml.cs");
            }
        }

        public void ClosePod()
        {
            try
            {
                //call unjoin method

                if (channelNetTcp != null)
                {
                    channelNetTcp.svcUnJoin(VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName);
                }
                else if (channelHttp != null)
                {

                    MemoryStream mmsFinal = new MemoryStream();
                    char del = '#';
                    Stream mmsUName = fncStringToStream(VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName);
                    mmsUName.Position = 0;
                    byte[] byteName = fncStreamToByteArry(mmsUName);

                    mmsFinal.Write(byteName, 0, byteName.Length);
                    
                    mmsFinal.Position = 0;

                    channelHttp.svcUnJoin(mmsFinal);
                }

                if (channelNetTcp != null)
                {
                    channelNetTcp.Close();
                    channelNetTcp.Dispose();
                    channelNetTcp = null;
                }
                if (channelHttp != null)
                {
                    channelHttp = null;
                }
                if (dispTimer != null)
                {
                    dispTimer.Stop();
                }

                Dispose();
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "ClosePod()", "ctlImageSharing.xaml.cs");
            }
        }

        #endregion

        #region Timer

        void dispHttpTimer_Tick(object sender, EventArgs e)
        {
            try
            {

                MemoryStream mmsFinal = new MemoryStream();
                char del = '#';
                Stream mmsUName = fncStringToStream(VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName);
                mmsUName.Position = 0;
                byte[] byteName = fncStreamToByteArry(mmsUName);

                mmsFinal.Write(byteName, 0, byteName.Length);
                mmsFinal.WriteByte((byte)del);
                mmsFinal.WriteByte((byte)del);
                mmsFinal.WriteByte((byte)del);
                mmsFinal.Position = 0;

                Stream objStream = new MemoryStream();
                objStream = channelHttp.svcGetMessages(mmsFinal);

                dispHttpTimer.Stop();

                if (objStream.Length > 0)
                {
                    this.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, objDelGetMsg, objStream);
                }
            }
            catch (Exception ex)
            {
                if (string.Compare(ex.Message, "Specified method is not supported.") == 1)
                {
                    VMuktiHelper.ExceptionHandler(ex, "dispHttpTimer_Tick", "ctlImageSharing.xaml.cs");
                }
            }
        }

        #endregion

        #region IDisposable Members

        public void Dispose()
        {
            try
            {
                if (channelHttp != null)
                {
                    channelHttp = null;
                }
                if (channelNetTcp != null)
                {
                    channelNetTcp.Close();
                    channelNetTcp.Dispose();
                    channelNetTcp = null;
                }
                GC.Collect();
                GC.WaitForPendingFinalizers();
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "Dispose()", "ctlImageSharing.xaml.cs");
            }
        }

        ~ctlImageSharing()
        {
            try
            {
                if (channelHttp != null)
                {
                    channelHttp = null;
                }
                if (channelNetTcp != null)
                {
                    channelNetTcp.Close();
                    channelNetTcp.Dispose();
                    channelNetTcp = null;
                }
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "~ctlImageSharing()", "ctlImageSharing.xaml.cs");
            }
        }

        #endregion
    }
}
