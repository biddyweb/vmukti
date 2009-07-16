using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media.Animation;
using System.Xml;
using System.Windows.Media.Imaging;
using VMuktiAPI;
using System.Text;
using System.Threading;
using System.Net;
using System.IO;

namespace News.Presentation
{
    /// <summary>
    /// Interaction logic for ctlOurNews.xaml
    /// </summary>
    /// 

    public enum ModulePermissions
    {
        Add = 0,
        Edit = 1,
        Delete = 2,
        View = 3
    }

    public partial class ctlOurNews : UserControl
    {



        #region Field
        News.Bussines.GetNews GetNewsTemp = new News.Bussines.GetNews();

        public delegate void delFncFillCanvas(string country);
        public delFncFillCanvas objdelFncFillCanvas;

        public delegate void delFncGetImage(object obj);
        public delFncGetImage objdelFncGetImage;

        public delegate void delfncNetStatus();
        public delfncNetStatus objDelFunctionNetStatus;

        bool disposed = false;
        StringBuilder sb1 = CreateTressInfo();
        bool Netstatus = false;
        int getNetstatus = 0;
        int timercount = 0;
        System.Timers.Timer objTimerNetStatus = new System.Timers.Timer();
        object parameter = new object();
        int newsType = 0;


        #endregion

        public ctlOurNews(ModulePermissions[] MyPermissions)
        {
            try
            {
                InitializeComponent();

                lblNetStatus.Visibility = Visibility.Collapsed;
                border1.Visibility = Visibility.Collapsed;
                lblNetStatus.Content = "Loading...";
                lblNetStatus.Visibility = Visibility.Visible;

                //lblNewsProvider.Content = "IBN-Live";

                #region delegate

                cbxFeeds.SelectionChanged += new SelectionChangedEventHandler(cbxFeeds_SelectionChanged);
                objdelFncFillCanvas = new delFncFillCanvas(FncFillCanvas);
                objdelFncGetImage = new delFncGetImage(FncGetImage);
                objDelFunctionNetStatus = new delfncNetStatus(FncNetStatus);
                #endregion

                objDelFunctionNetStatus.BeginInvoke(new AsyncCallback(finished), null);

                #region logging
                StringBuilder sb = new StringBuilder();
                sb.AppendLine("News module: module is loaded by user ");
                sb.AppendLine(sb1.ToString());
                VMuktiAPI.ClsLogging.WriteToTresslog(sb);
                #endregion
            }
            catch (Exception exp)
            {
                exp.Data.Add("My Key", "ctlOurNews()--:--ctlOurNews.xaml.cs--:--" + exp.Message + " :--:--");
                ClsException.LogError(exp);
                ClsException.WriteToErrorLogFile(exp);
            }

        }

        #region UiRelated Events

        void cbxFeeds_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            try
            {

                System.Windows.Controls.ComboBox temp = (System.Windows.Controls.ComboBox)sender;
                System.Windows.Controls.ComboBoxItem CurrentCatagory = (System.Windows.Controls.ComboBoxItem)temp.SelectedItem;
                if (CurrentCatagory.Tag.ToString().Contains("cnn"))
                {
                    newsType = 1;
                    System.Windows.Controls.Primitives.ToggleButton tb = (cbxFeeds.Template.FindName("ToggleButton", cbxFeeds) as System.Windows.Controls.Primitives.ToggleButton);
                    (tb.Template.FindName("lblNewsProvider", tb) as Label).Content = "CNN.com";
                    //lblNewsProvider.Content = "CNN News";
                }
                else
                {
                    newsType = 0;
                    System.Windows.Controls.Primitives.ToggleButton tb = (cbxFeeds.Template.FindName("ToggleButton", cbxFeeds) as System.Windows.Controls.Primitives.ToggleButton);
                    (tb.Template.FindName("lblNewsProvider", tb) as Label).Content = "IBN-Live.com";
                    //lblNewsProvider.Content = "IBN-Live";
                }

                #region logging
                StringBuilder sb = new StringBuilder();
                sb.AppendLine("News module: user chane the selection catagory ");
                sb.AppendLine("user selected the catagory: " + CurrentCatagory.ToString());
                sb.AppendLine(sb1.ToString());
                VMuktiAPI.ClsLogging.WriteToTresslog(sb);
                #endregion

                #region clearing wrappanel
                wrapPanel.Children.Clear();
                lblNetStatus.Content = "Loading...";
                lblNetStatus.Visibility = Visibility.Visible;
                border1.Visibility = Visibility.Collapsed;
                #endregion

                #region get news on selection chang
                System.Threading.Thread objthreadFncFillCanvas = new System.Threading.Thread(new System.Threading.ParameterizedThreadStart(FncThreadFillCanvas));
                objthreadFncFillCanvas.IsBackground = true;
                objthreadFncFillCanvas.Priority = System.Threading.ThreadPriority.Normal;
                parameter = CurrentCatagory.Tag;
                objthreadFncFillCanvas.Start(parameter);
                #endregion


            }
            catch (Exception exp)
            {
                exp.Data.Add("My Key", "cbxFeeds_SelectionChanged()--:--ctlOurNews.xaml.cs--:--" + exp.Message + " :--:--");
                ClsException.LogError(exp);
                ClsException.WriteToErrorLogFile(exp);

            }

        }

        void lblCurrent_MouseLeave(object sender, MouseEventArgs e)
        {
            try
            {
                txtPopup.Text = "";
                DoubleAnimation dblAnimation = new DoubleAnimation();
                dblAnimation.From = 110;
                dblAnimation.To = 0;
                dblAnimation.Duration = new System.Windows.Duration(TimeSpan.FromSeconds(.40));
                popupPanle.BeginAnimation(System.Windows.Controls.WrapPanel.HeightProperty, dblAnimation);

            }
            catch (Exception exp)
            {
                exp.Data.Add("My Key", "lblCurrent_MouseLeave()--:--ctlOurNews.xaml.cs--:--" + exp.Message + " :--:--");
                ClsException.LogError(exp);
                ClsException.WriteToErrorLogFile(exp);
            }

        }

        void lblCurrent_MouseEnter(object sender, MouseEventArgs e)
        {
            try
            {


                border1.Visibility = Visibility.Visible;
                News.Bussines.EvzItem lblTemp = (News.Bussines.EvzItem)(sender);
                string desc = "";
                desc = News.Bussines.GetNews.FuncRetriveInfoFromNode((XmlNode)(lblTemp.Tag), "description");

                if (newsType == 0)//if ibn then u have to parse decription only ibn contain image as well
                {
                    lblImageAv.Visibility = Visibility.Hidden;
                    lblImageNot.Visibility = Visibility.Hidden;
                    txtPopup.Text = News.Bussines.GetNews.FncFilterString(News.Bussines.GetNews.FncGetTitle(desc), newsType);

                    #region image

                    System.Threading.Thread objThread = new System.Threading.Thread(new System.Threading.ParameterizedThreadStart(FncThreadGetImage));
                    objThread.IsBackground = true;
                    objThread.Priority = System.Threading.ThreadPriority.Normal;
                    objThread.Start(News.Bussines.GetNews.FncGetImageSource(desc));

                    #endregion

                }
                else // if cnn no need to parse discription it only contain title
                {
                    lblImageAv.Visibility = Visibility.Visible;
                    lblImageNot.Visibility = Visibility.Visible;
                    imgNews.BeginInit();
                    imgNews.Source = null;
                    imgNews.Visibility = Visibility.Visible;
                    imgNews.EndInit();
                    txtPopup.Text = News.Bussines.GetNews.FncFilterString(desc, newsType);
                }



                #region Animation
                DoubleAnimation dblAnimation = new DoubleAnimation();
                dblAnimation.From = 0;
                dblAnimation.To = 92;
                dblAnimation.Duration = new Duration(TimeSpan.FromSeconds(.40));
                border1.BeginAnimation(System.Windows.Controls.Border.HeightProperty, dblAnimation);
                #endregion

                #region logging
                StringBuilder sb = new StringBuilder();
                sb.AppendLine("News module: user reques the descriptrion for: ");
                sb.Append(desc);
                sb.AppendLine(sb1.ToString());
                VMuktiAPI.ClsLogging.WriteToTresslog(sb);
                #endregion

            }
            catch (Exception exp)
            {
                exp.Data.Add("My Key", "lblCurrent_MouseEnter()--:--ctlOurNews.xaml.cs--:--" + exp.Message + " :--:--");
                ClsException.LogError(exp);
                ClsException.WriteToErrorLogFile(exp);
            }

        }

        void lblCurrent_MouseDown(object sender, MouseButtonEventArgs e)
        {

            try
            {
                XmlNode TempNode = (XmlNode)(((News.Bussines.EvzItem)sender).Tag);
                string link = News.Bussines.GetNews.FuncRetriveInfoFromNode(TempNode, "link").ToString();
                System.Diagnostics.Process.Start(link);

                StringBuilder sb = new StringBuilder();
                sb.AppendLine("News module: user request the detail news for:  ");
                sb.Append(link);
                sb.AppendLine(sb1.ToString());
                VMuktiAPI.ClsLogging.WriteToTresslog(sb);
            }
            catch (Exception exp)
            {
                exp.Data.Add("My Key", "lblCurrent_MouseDown()--:--ctlOurNews.xaml.cs--:--" + exp.Message + " :--:--");
                ClsException.LogError(exp);
                ClsException.WriteToErrorLogFile(exp);
            }

        }

        #endregion

        #region Thread related function

        void FunThreadNetStatus(object temp)
        {
            try
            {
                this.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Background, objDelFunctionNetStatus);
            }

            catch (Exception exp)
            {
                exp.Data.Add("My Key", "FunThreadNetStatus()--:--ctlOurNews.xaml.cs--:--" + exp.Message + " :--:--");
                ClsException.LogError(exp);
                ClsException.WriteToErrorLogFile(exp);

            }
        }

        public void FncNetStatus()
        {
            try
            {
                HttpWebRequest req;

                object data = new object(); //container for our "Stuff"          
                req = (HttpWebRequest)WebRequest.Create("http://www.google.com");
                //RequestState is a custom class to pass info to the callback
                RequestState state = new RequestState(req, data, "http://www.google.com");
                IAsyncResult result = req.BeginGetResponse(new AsyncCallback(finishedNetStatus), state);

            }
            catch (Exception exp)
            {
                exp.Data.Add("My Key", "FncNetStatus()--:--ctlOurNews.xaml.cs--:--" + exp.Message + " :--:--");
                ClsException.LogError(exp);
                ClsException.WriteToErrorLogFile(exp);

            }
        }

        private void finishedNetStatus(IAsyncResult result)
        {
            try
            {
                RequestState state = (RequestState)result.AsyncState;
                WebRequest request = (WebRequest)state.Request;
                HttpWebResponse response = (HttpWebResponse)request.EndGetResponse(result);
                Stream s = (Stream)response.GetResponseStream();
                StreamReader readStream = new StreamReader(s);
                string dataString = readStream.ReadToEnd();

                Netstatus = true;
                getNetstatus = 1;
                parameter = "http://www.ibnlive.com/xml/rss/world.xml";
                this.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Background, objdelFncFillCanvas, parameter);

                response.Close();
                s.Close();
                readStream.Close();

            }
            catch (Exception exp)
            {
                Netstatus = false;
                getNetstatus = 1;
                parameter = "http://www.ibnlive.com/xml/rss/world.xml";
                this.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Background, objdelFncFillCanvas, parameter);
                //this time dispature will tell that net is not availabe
            }
        }

        void FncThreadGetImage(object uri)
        {
            try
            {
                this.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Background, objdelFncGetImage, uri);
            }
            catch (Exception exp)
            {
                exp.Data.Add("My Key", "FncThreadGetImage()--:--ctlOurNews.xaml.cs--:--" + exp.Message + " :--:--");
                ClsException.LogError(exp);
                ClsException.WriteToErrorLogFile(exp);
            }
        }

        void FncGetImage(object uri)
        {

            try
            {
                BitmapImage myBitmapImage = new BitmapImage();
                //myBitmapImage.BeginInit();
                //// myBitmapImage.UriSource = new Uri(News.Bussines.GetNews.FncGetImageSource(desc));
                //myBitmapImage.UriSource = new Uri(@"/image/news_1_48.ico", UriKind.RelativeOrAbsolute);
                //myBitmapImage.EndInit();
                //imgNews.BeginInit();
                //imgNews.Source = myBitmapImage;
                //imgNews.EndInit();

                myBitmapImage.BeginInit();
                myBitmapImage.UriSource = new Uri(uri.ToString());
                myBitmapImage.EndInit();
                imgNews.BeginInit();
                imgNews.Source = myBitmapImage;
                imgNews.EndInit();
            }
            catch (Exception exp)
            {
                exp.Data.Add("My Key", "FncGetImage()--:--ctlOurNews.xaml.cs--:--" + exp.Message + " :--:--");
                ClsException.LogError(exp);
                ClsException.WriteToErrorLogFile(exp);
            }
        }

        void FncThreadFillCanvas(object obj)
        {
            try
            {
                this.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Background, objdelFncFillCanvas, parameter);
            }
            catch (Exception exp)
            {
                exp.Data.Add("My Key", "FncThreadFillCanvas()--:--ctlOurNews.xaml.cs--:--" + exp.Message + " :--:--");
                ClsException.LogError(exp);
                ClsException.WriteToErrorLogFile(exp);

            }
        }

        void FncFillCanvas(string temp)
        {

            try
            {
                if (Netstatus == true)
                {
                    lblNetStatus.Visibility = Visibility.Collapsed;
                    cbxFeeds.IsEnabled = true;

                    wrapPanel.Children.Clear();
                    List<News.Bussines.EvzItem> lblNews = GetNewsTemp.FuncGetNews(temp, newsType);
                    foreach (News.Bussines.EvzItem lblCurrent in lblNews)
                    {

                        wrapPanel.Children.Add(lblCurrent);
                        lblCurrent.HorizontalAlignment = HorizontalAlignment.Stretch;
                        lblCurrent.MouseDoubleClick += new MouseButtonEventHandler(lblCurrent_MouseDown);
                        lblCurrent.MouseDown += new MouseButtonEventHandler(lblCurrent_MouseEnter);

                    }
                }
                else
                {
                    lblNetStatus.Content = "Net Is Not Available";
                    lblNetStatus.Visibility = Visibility.Visible;
                    cbxFeeds.IsEnabled = false;
                }
            }
            catch (Exception exp)
            {
                exp.Data.Add("My Key", "FncFillCanvas()--:--ctlOurNews.xaml.cs--:--" + exp.Message + " :--:--");
                ClsException.LogError(exp);
                ClsException.WriteToErrorLogFile(exp);
            }

        }

        #endregion

        #region destructorfunction

        public void ClosePod()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("News module: module is closed by user ");
            sb.Append(sb1.ToString());
            VMuktiAPI.ClsLogging.WriteToTresslog(sb);
            try
            {
                this.Dispose();
                System.GC.Collect();
                System.GC.WaitForPendingFinalizers();
            }
            catch (Exception exp)
            {
                exp.Data.Add("My Key", "ClosePod()--:--ctlOurNews.xaml.cs--:--" + exp.Message + " :--:--");
                ClsException.LogError(exp);
                ClsException.WriteToErrorLogFile(exp);
            }

        }

        public void Dispose()
        {
            Dispose(true);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    GetNewsTemp.Dispose();
                    border1 = null;
                    objdelFncFillCanvas = null;
                }

            }
            disposed = true;
        }

        ~ctlOurNews()
        {
            this.Dispose();
            System.GC.Collect();

        }
        #endregion

        #region logging function

        public static StringBuilder CreateTressInfo()
        {
            try
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("User Is : " + VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName);
                sb.AppendLine();
                sb.Append("Peer Type is : " + VMuktiAPI.VMuktiInfo.CurrentPeer.CurrPeerType.ToString());
                sb.AppendLine();
                sb.Append("User's SuperNode is : " + VMuktiAPI.VMuktiInfo.CurrentPeer.SuperNodeIP);
                sb.AppendLine();
                sb.Append("User's Machine Ip Address : " + VMuktiAPI.GetIPAddress.ClsGetIP4Address.GetIP4Address());
                sb.AppendLine("-----------------------------------------------------------------------------------------------");
                return sb;
            }
            catch (Exception exp)
            {
                exp.Data.Add("My Key", "CreateTressInfo()--:--ctlOurNews.xaml.cs--:--" + exp.Message + " :--:--");
                ClsException.LogError(exp);
                ClsException.WriteToErrorLogFile(exp);
                return null;
            }
        }

        #endregion

        #region CalBack

        private void finished(IAsyncResult results)
        {
            try
            {
                objDelFunctionNetStatus.EndInvoke(results);
            }
            catch (Exception exp)
            {
                exp.Data.Add("My Key", "finished()--:--ctlOurNews.xaml.cs--:--" + exp.Message + " :--:--");
                ClsException.LogError(exp);
                ClsException.WriteToErrorLogFile(exp);

            }

        }

        #endregion

    }

    class RequestState
    {

        public WebRequest Request; // holds the request

        public object Data; // store any data in this

        public string SiteUrl; // holds the UrlString to match up results (Database lookup, etc).

        public RequestState(WebRequest request, object data, string siteUrl)
        {
            try
            {
                this.Request = request;

                this.Data = data;

                this.SiteUrl = siteUrl;
            }
            catch (Exception exp)
            {
                exp.Data.Add("My Key", "RequestState()--:--ctlOurNews.xaml.cs--:--" + exp.Message + " :--:--");
                ClsException.LogError(exp);
                ClsException.WriteToErrorLogFile(exp);

            }


        }

    }

}