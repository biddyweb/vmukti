using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interop;
using System.Xml;
using VMuktiAPI;
using System.Text;
using System.Windows.Documents;
using System.Collections.Generic;
using System.Net;
using System.IO;
namespace Weather.Presentation
{
    /// <summary>
    /// Interaction logic for UCWeather.xaml
    /// </summary>
    /// 
    public enum ModulePermissions
    {
        Add = 0,
        Edit = 1,
        Delete = 2,
        View = 3
    }

    public partial class UCWeather : UserControl, IDisposable
    {
        #region Fields

        WeatherService.GlobalWeather objWeatherService = new global::Weather.Presentation.WeatherService.GlobalWeather();

        public delegate void delLoadDock(string country, string city);
        public delLoadDock objdelLoadDock;



        delegate void DelInvokeCmbChange();
        DelInvokeCmbChange objInvokeCmbChange;

        delegate void DelNetStatus(object a);
        DelNetStatus ObjDelNetstatus;

        public delegate void delfncNetStatus(object a);
        public delfncNetStatus objDelFunctionNetStatus;

        string country = "india";
        string city = "Bombay / Santacruz";
        private bool disposed = false;
        static bool NetStatus = false;
        StringBuilder sb1 = CreateTressInfo();

        HttpWebRequest req;
        #endregion

        #region constructor
        public UCWeather(ModulePermissions[] MyPermissions)
        {
            try
            {
                InitializeComponent();
                btnclick.IsEnabled = false;

                #region Delegate Object
                objInvokeCmbChange = new DelInvokeCmbChange(funThreadCmb);
                objdelLoadDock = new delLoadDock(LoadDocK);
                ObjDelNetstatus = new DelNetStatus(FncNetStatus);
                objDelFunctionNetStatus = new delfncNetStatus(FncNetStatus);
                #endregion

                #region cheking netstatus

                object a1 = new object();
                try
                {
                    ObjDelNetstatus.BeginInvoke(a1, new AsyncCallback(finished), null);
                }
                catch (Exception exp)
                {
                    exp.Data.Add("My Key", "UCWeather()--:--UCWeather.xaml.cs--:--" + exp.Message + " :--:--");
                    ClsException.LogError(exp);
                    ClsException.WriteToErrorLogFile(exp);
                }

                #endregion

                #region event
                btnclick.Click += new RoutedEventHandler(btnclick_Click);
                btnCancle.Click += new RoutedEventHandler(btnCancle_Click);
                btnGet.Click += new RoutedEventHandler(btnGet_Click);
                cmbCountry.SelectionChanged += new SelectionChangedEventHandler(cmbCountry_SelectionChanged);
                objWeatherService.GetCitiesByCountryCompleted += new Weather.Presentation.WeatherService.GetCitiesByCountryCompletedEventHandler(objWeatherService_GetCitiesByCountryCompleted);
                objWeatherService.GetWeatherCompleted += new Weather.Presentation.WeatherService.GetWeatherCompletedEventHandler(objWeatherService_GetWeatherCompleted);
                #endregion

                fncCnvMain();

                #region loading
                try
                {
                    string strGetCookies = Application.GetCookie(BrowserInteropHelper.Source);
                    if (strGetCookies != null)
                    {
                        if (strGetCookies.Split(';').Length > 1)
                        {
                            if (strGetCookies.Split(';')[1].Trim().Split('=')[0].ToLower() == "country")
                            {
                                country = strGetCookies.Split(';')[1].Trim().Split('=')[1];
                                city = strGetCookies.Split(';')[2].Trim().Split('=')[1];
                            }
                        }

                    }
                    else
                    {
                        country = "india";
                        city = "Bombay / Santacruz";
                    }
                }
                catch (Exception ex) // if cookies are not set then user ill get exception so defaultly we load weather for bombay
                {
                    country = "india";
                    city = "Bombay / Santacruz";
                }
                #endregion

                #region loging
                StringBuilder sb = new StringBuilder();
                sb.AppendLine("Weather Module:");
                sb.AppendLine("loading Weather module");
                sb.AppendLine(sb1.ToString());
                VMuktiAPI.ClsLogging.WriteToTresslog(sb);
                #endregion
            }
            catch (Exception exp)
            {
                exp.Data.Add("My Key", "UCWeather()--:--UCWeather.xaml.cs--:--" + exp.Message + " :--:--");
                ClsException.LogError(exp);
                ClsException.WriteToErrorLogFile(exp);
            }

        }
        #endregion

        #region Webservice function

        void objWeatherService_GetWeatherCompleted(object sender, Weather.Presentation.WeatherService.GetWeatherCompletedEventArgs e)
        {
            try
            {
                if (e != null)
                {
                    string ss = e.Result;

                    #region parsing
                    if (ss != "Data Not Found")
                    {
                        XmlDocument doc = new XmlDocument();
                        doc.LoadXml(ss.ToString());
                        XmlNode nd = null;
                        nd = doc.SelectSingleNode("CurrentWeather");
                        if (nd.ChildNodes.Count == 0)
                        {
                            lblValue.Content = "";
                            lblName.Visibility = Visibility.Visible;
                            lblName.Content = "Not Available";
                        }
                        else
                        {
                            bool status = false;
                            foreach (XmlElement el in nd)
                            {
                                XmlNode xn = nd.SelectSingleNode("Temperature");


                                if (el.Name == "Temperature")
                                {
                                    string s = el.InnerText;
                                    lblValue.Content = s;
                                    lblName.Visibility = Visibility.Visible;
                                    lblName.Content = city.Split('/')[0];
                                    expMenu.Items.Add(el.Name + "=" + el.InnerText.ToString());
                                    status = true;
                                }

                                else if (el.Name != "Status" && el.Name != "Time" && el.Name != "Temperature" && el.Name != "Location")
                                {
                                    expMenu.Items.Add(el.Name + "=" + el.InnerText.ToString());
                                }
                            }
                            if (status == false)
                            {
                                lblValue.Content = "";
                                lblName.Visibility = Visibility.Visible;
                                lblName.Content = "Not Available";
                            }
                        }


                    }
                    else
                    {
                        lblName.Visibility = Visibility.Visible;
                        lblName.Content = "Not Found";
                        lblValue.Content = "";
                    }
                    #endregion

                }

                else
                {
                    lblName.Visibility = Visibility.Visible;
                    lblName.Content = "Not Available";
                    lblValue.Content = "";
                }
            }

            catch (Exception exp)
            {
                try
                {
                    if (!disposed)
            {
                cnvlbl.Visibility = Visibility.Collapsed;
                cnvMain.Visibility = Visibility.Visible;
                lblName.Content = "Not Available";

                exp.Data.Add("My Key", "objWeatherService_GetWeatherCompleted()--:--UCWeather.xaml.cs--:--" + exp.Message + " :--:--");
                ClsException.LogError(exp);
                ClsException.WriteToErrorLogFile(exp);
            }
        }
                catch (Exception ex)
                {

                }
            }
        }

        void objWeatherService_GetCitiesByCountryCompleted(object sender, Weather.Presentation.WeatherService.GetCitiesByCountryCompletedEventArgs e)
        {
            try
            {
                if (e != null)
                {
                    string ss1 = e.Result;
                    if (ss1 != "Data Not Found")
                    {
                        cmbCity.Items.Clear();

                        #region xmlDoc

                        XmlDocument doc = new XmlDocument();
                        doc.LoadXml(ss1.ToString());
                        XmlNode nd = null;
                        nd = doc.SelectSingleNode("NewDataSet");
                        if (nd.ChildNodes.Count == 0)
                        {
                            ComboBoxItem cbi = new ComboBoxItem();
                            cbi.Content = string.Format("Data Not Available For {0}", ((ComboBoxItem)cmbCountry.SelectedItem).Content.ToString());
                            cmbCity.Items.Add(cbi);
                        }

                        else
                        {
                            foreach (XmlElement el in nd)
                            {
                                if (el.Name == "Table")
                                {
                                    if (el.HasChildNodes == true)
                                    {
                                        foreach (XmlNode atr in el)
                                        {
                                            if (atr.Name == "City")
                                            {
                                                cmbCity.Items.Add(new ComboBoxItem());
                                                ((ComboBoxItem)cmbCity.Items[cmbCity.Items.Count - 1]).Content = atr.InnerText;
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    cmbCity.Items.Add("Data Not Available");
                                }
                            }
                        }

                        #endregion
                    }
                    else
                    {
                        cnvlbl.Visibility = Visibility.Collapsed;
                        cnvMain.Visibility = Visibility.Visible;
                        lblName.Content = "Not Available";
                    }
                }
                else
                {
                    cnvlbl.Visibility = Visibility.Collapsed;
                    cnvMain.Visibility = Visibility.Visible;
                    lblName.Content = "Not Available";
                }

            }
            catch (Exception exp)
            {
                try
                {
                cnvlbl.Visibility = Visibility.Collapsed;
                cnvMain.Visibility = Visibility.Visible;
                lblName.Content = "Not Available";

                exp.Data.Add("My Key", "objWeatherService_GetCitiesByCountryCompleted()--:--UCWeather.xaml.cs--:--" + exp.Message + " :--:--");
                ClsException.LogError(exp);
                ClsException.WriteToErrorLogFile(exp);
            }
                catch (Exception ex)
                {

                }
            }
        }

        #endregion

        #region Loaddock

        public void FncThreadLoadDock()
        {
            try
            {
                this.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Background, objdelLoadDock, country, city);
            }
            catch (Exception exp)
            {
                exp.Data.Add("My Key", "FncThreadLoadDock()--:--UCWeather.xaml.cs--:--" + exp.Message + " :--:--");
                ClsException.LogError(exp);
                ClsException.WriteToErrorLogFile(exp);
            }

        }

        public void LoadDocK(string country, string city)
        {
            try
            {
                if (NetStatus == true)
                {
                    btnclick.IsEnabled = true;
                    objWeatherService.GetWeatherAsync(city, country);
                }
                else
                {
                    lblName.Visibility = Visibility.Visible;
                    lblName.Content = "Not Responding";
                    lblValue.Content = "";
                }
            }
            catch (Exception exp)
            {
                lblName.Visibility = Visibility.Visible;
                lblName.Content = "Not Available";
                lblValue.Content = "";
                exp.Data.Add("My Key", "LoadDocK()--:--UCWeather.xaml.cs--:--" + exp.Message + " :--:--");
                ClsException.LogError(exp);
                ClsException.WriteToErrorLogFile(exp);
            }
        }

        void btnGet_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                lblValue.Content = "";
                expMenu.Items.Clear();
                lblName.Content = "Loading...";

                country = ((ComboBoxItem)cmbCountry.SelectedItem).Content.ToString();
                city = ((ComboBoxItem)cmbCity.SelectedItem).Content.ToString();

                this.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Background, objdelLoadDock, country, city);

                if (chkDefault.IsChecked == true)
                {
                    Application.SetCookie(BrowserInteropHelper.Source, "Country=" + ((ComboBoxItem)cmbCountry.SelectedItem).Content.ToString() + "; expires=Sat, 24-Dec-2999 16:00:00 GMT");
                    Application.SetCookie(BrowserInteropHelper.Source, "City=" + ((ComboBoxItem)cmbCity.SelectedItem).Content.ToString() + "; expires=Sat, 24-Dec-2999 16:00:00 GMT");

                }

                fncCnvMain();

                #region logging
                StringBuilder sb = new StringBuilder();
                sb.AppendLine("weather module: user request to get current weather of ");
                sb.AppendLine("country :" + ((ComboBoxItem)cmbCountry.SelectedItem).Content.ToString() + "state:" + ((ComboBoxItem)cmbCity.SelectedItem).Content.ToString());
                sb.AppendLine(sb1.ToString());
                VMuktiAPI.ClsLogging.WriteToTresslog(sb);
                #endregion

            }
            catch (Exception exp)
            {
                MessageBox.Show("U must have to select city and state");
                exp.Data.Add("My Key", "btnGet_Click()--:--UCWeather.xaml.cs--:--" + exp.Message + " :--:--");
                ClsException.LogError(exp);
                ClsException.WriteToErrorLogFile(exp);
            }

        }

        #endregion

        #region NetStatus

        private void finished(IAsyncResult results)
        {
            try
            {
                if (results != null)
                {
                ObjDelNetstatus.EndInvoke(results);
                    results.AsyncWaitHandle.Close();
                }

            }
            catch (Exception exp)
            {
                exp.Data.Add("My Key", "finished()--:--UCWeather.xaml.cs--:--" + exp.Message + " :--:--");
                ClsException.LogError(exp);
                ClsException.WriteToErrorLogFile(exp);
            }
        }

        void FunThreadNetStatus(object temp)
        {
            try
            {
                this.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Background, objDelFunctionNetStatus, temp);
            }
            catch (Exception exp)
            {
                exp.Data.Add("My Key", "FunThreadNetStatus()--:--UCWeather.xaml.cs--:--" + exp.Message + " :--:--");
                ClsException.LogError(exp);
                ClsException.WriteToErrorLogFile(exp);
            }
        }

        public void FncNetStatus(object temp)
        {
            try
            {

                object data = new object(); //container for our "Stuff"          
                    req = (HttpWebRequest)WebRequest.Create("http://www.google.com");
                //RequestState is a custom class to pass info to the callback
                RequestState state = new RequestState(req, data, "http://www.google.com");
                IAsyncResult result = req.BeginGetResponse(new AsyncCallback(finishedNetstatus), state);
            }
            catch (Exception exp)
            {
                exp.Data.Add("My Key", "FncNetStatus()--:--UCWeather.xaml.cs--:--" + exp.Message + " :--:--");
                ClsException.LogError(exp);
                ClsException.WriteToErrorLogFile(exp);
            }

        }

        private void finishedNetstatus(IAsyncResult result)
        {
            try
            {
                RequestState state = (RequestState)result.AsyncState;
                WebRequest request = (WebRequest)state.Request;
                HttpWebResponse response = (HttpWebResponse)request.EndGetResponse(result);
                Stream s = (Stream)response.GetResponseStream();
                StreamReader readStream = new StreamReader(s);
                string dataString = readStream.ReadToEnd();
                NetStatus = true;
                this.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Background, objdelLoadDock, country, city);
                response.Close();
                s.Close();
                readStream.Close();

            }
            catch (Exception exp)
            {
                NetStatus = false;
                this.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Background, objdelLoadDock, country, city);
                //this will shoew net is not available ;
            }
        }

        #endregion

        #region combo Selection chang

        void cmbCountry_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (NetStatus == true)
                {
                    this.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Background, objInvokeCmbChange);
                    cmbCity.Items.Clear();
                    cmbCity.Items.Add("Loading...");
                }
                else
                {
                    cnvlbl.Visibility = Visibility.Collapsed;
                    cnvMain.Visibility = Visibility.Visible;
                    lblName.Content = "Not Available";
                }
            }
            catch (Exception exp)
            {
                exp.Data.Add("My Key", "cmbCountry_SelectionChanged()--:--UCWeather.xaml.cs--:--" + exp.Message + " :--:--");
                ClsException.LogError(exp);
                ClsException.WriteToErrorLogFile(exp);
            }
        }

        void fncInvokeCmbChange()
        {
            try
            {
                this.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, objInvokeCmbChange);
            }
            catch (Exception exp)
            {
                exp.Data.Add("My Key", "fncInvokeCmbChange()--:--UCWeather.xaml.cs--:--" + exp.Message + " :--:--");
                ClsException.LogError(exp);
                ClsException.WriteToErrorLogFile(exp);
            }
        }

        void funThreadCmb()
        {
            try
            {
                string strcountry = ((ComboBoxItem)cmbCountry.SelectedItem).Content.ToString();
                cnvlbl.Visibility = Visibility.Visible;
                if (NetStatus == true)
                {
                    objWeatherService.GetCitiesByCountryAsync(strcountry);
                }
                else
                {
                    cnvlbl.Visibility = Visibility.Collapsed;
                    cnvMain.Visibility = Visibility.Visible;
                    lblName.Content = "Not Available";
                }

                StringBuilder sb = new StringBuilder();
                sb.AppendLine("weather module: country is selected by user.");
                sb.AppendLine("Country is :" + strcountry);
                sb.AppendLine(sb1.ToString());
                VMuktiAPI.ClsLogging.WriteToTresslog(sb);

            }
            catch (Exception exp)
            {
                exp.Data.Add("My Key", "funThreadCmb()--:--UCWeather.xaml.cs--:--" + exp.Message + " :--:--");
                ClsException.LogError(exp);
                ClsException.WriteToErrorLogFile(exp);
            }
        }

        #endregion

        #region tempFunction

        void btnCancle_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                fncCnvMain();
                StringBuilder sb = new StringBuilder();
                sb.AppendLine("Weather module: cancel button is clicked by user");
                sb.AppendLine(sb1.ToString());
                VMuktiAPI.ClsLogging.WriteToTresslog(sb);
            }
            catch (Exception exp)
            {
                exp.Data.Add("My Key", "btnCancle_Click()--:--UCWeather.xaml.cs--:--" + exp.Message + " :--:--");
                ClsException.LogError(exp);
                ClsException.WriteToErrorLogFile(exp);
            }
        }

        void btnclick_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                fncCnvlbl();
                StringBuilder sb = new StringBuilder();
                sb.AppendLine("weather module: setting button is clicked to configur city and state.");
                sb.AppendLine(sb1.ToString());
                VMuktiAPI.ClsLogging.WriteToTresslog(sb);
            }
            catch (Exception exp)
            {
                exp.Data.Add("My Key", "btnclick_Click()--:--UCWeather.xaml.cs--:--" + exp.Message + " :--:--");
                ClsException.LogError(exp);
                ClsException.WriteToErrorLogFile(exp);
            }
        }

        void fncCnvlbl()
        {
            try
            {
                cnvMain.Visibility = Visibility.Collapsed;
                cnvlbl.Visibility = Visibility.Visible;
                chkDefault.IsChecked = false;
            }

            catch (Exception exp)
            {
                exp.Data.Add("My Key", "fncCnvlbl()--:--UCWeather.xaml.cs--:--" + exp.Message + " :--:--");
                ClsException.LogError(exp);
                ClsException.WriteToErrorLogFile(exp);
            }
        }

        void fncCnvMain()
        {
            try
            {
                cnvMain.Visibility = Visibility.Visible;
                cnvlbl.Visibility = Visibility.Collapsed;
            }

            catch (Exception exp)
            {
                exp.Data.Add("My Key", "fncCnvMain()--:--UCWeather.xaml.cs--:--" + exp.Message + " :--:--");
                ClsException.LogError(exp);
                ClsException.WriteToErrorLogFile(exp);
            }
        }

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
                sb.AppendLine("--------------------------------------------------------------------------------------");
                return sb;
            }
            catch (Exception exp)
            {
                exp.Data.Add("My Key", "CreateTressInfo()--:--UCWeather.xaml.cs--:--" + exp.Message + " :--:--");
                ClsException.LogError(exp);
                ClsException.WriteToErrorLogFile(exp);
                return null;
            }
        }

        #endregion

        #endregion

        #region destructor

        public void Dispose()
        {
            Dispose(true);
        }

        protected virtual void Dispose(bool disposing)
        {
            try
            {
                if (!this.disposed)
                {
                    if (disposing)
                    {
                        if(objdelLoadDock != null)
                            objdelLoadDock = null;
                        if(LayoutRoot !=null)
                            LayoutRoot = null;
                        if(cnvlbl != null)
                            cnvlbl = null;
                        if (cnvMain != null)
                            cnvMain = null;
                        if (cmbCity != null)
                             cmbCity = null;
                        if (objInvokeCmbChange != null)
                            objInvokeCmbChange = null;
                        if (ObjDelNetstatus != null)
                            ObjDelNetstatus = null;
                        if (objDelFunctionNetStatus != null)
                            objDelFunctionNetStatus = null;


                        country = null;
                        city = null;

                    }
                    this.objWeatherService.Dispose();
                }
                disposed = true;
            }
            catch (Exception exp)
            {
                exp.Data.Add("My Key", "Dispose()--:--UCWeather.xaml.cs--:--" + exp.Message + " :--:--");
                ClsException.LogError(exp);
                ClsException.WriteToErrorLogFile(exp);
            }
        }

        ~UCWeather()
        {
            this.Dispose();
           
        }

        public void ClosePod()
        {
            try
            {

                StringBuilder sb = new StringBuilder();
                sb.AppendLine("weather module: module is closed by user ");
                sb.AppendLine(sb1.ToString());
                VMuktiAPI.ClsLogging.WriteToTresslog(sb);
                try
                {
                    if (req != null)
                    {
                        req.Abort();
                    }
                    this.Dispose();
                   
                  
                }

                catch (Exception exp)
                {
                    exp.Data.Add("My Key", "ClosePod()--:--UCWeather.xaml.cs--:--" + exp.Message + " :--:--");
                    ClsException.LogError(exp);
                    ClsException.WriteToErrorLogFile(exp);
                }
            }
            catch (Exception exp)
            {
                exp.Data.Add("My Key", "ClosePod()--:--UCWeather.xaml.cs--:--" + exp.Message + " :--:--");
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

            this.Request = request;

            this.Data = data;

            this.SiteUrl = siteUrl;

        }

    }

}
