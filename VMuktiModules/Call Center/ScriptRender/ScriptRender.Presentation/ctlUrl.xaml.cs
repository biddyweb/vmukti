using System;
using System.Collections.Generic;
//using System.Linq;
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
using ScriptRender.Business;

namespace ScriptRender.Presentation
{

    

    public partial class ctlUrl : UserControl
    {

   //     ModulePermissions[] _MyPermissions;
        string sPhoneNumber = string.Empty;
        System.Windows.Threading.DispatcherTimer dt = new System.Windows.Threading.DispatcherTimer(System.Windows.Threading.DispatcherPriority.Normal);
        public ctlUrl()
        {
            try
            {
                InitializeComponent();

                //_MyPermissions = MyPermissions;
                //FncPermissionsReview();

                VMuktiAPI.VMuktiHelper.RegisterEvent("SetLeadIDScript").VMuktiEvent += new VMuktiAPI.VMuktiEvents.VMuktiEventHandler(WebScriptRender_VMuktiEvent);
                VMuktiAPI.VMuktiHelper.RegisterEvent("FireNextCallEvent").VMuktiEvent += new VMuktiAPI.VMuktiEvents.VMuktiEventHandler(FireNextCall_VMuktiEvent);
                this.Unloaded += new RoutedEventHandler(CtlWebScriptRender_Unloaded);

                this.btnEnterDispReason.Click += new RoutedEventHandler(btnEnterDispReason_Click);
                this.btnCancelDispReason.Click += new RoutedEventHandler(btnCancelDispReason_Click);

                this.btnEnterCallBackReason.Click += new RoutedEventHandler(btnEnterCallBackReason_Click);
                this.btnCancelCallBackReason.Click += new RoutedEventHandler(btnCancelCallBackReason_Click);

                this.btnCancelOtherDispReason.Click += new RoutedEventHandler(btnCancelOtherDispReason_Click);
                this.btnEnterOtherDispReason.Click += new RoutedEventHandler(btnEnterOtherDispReason_Click);
                
            }
            catch (Exception wx)
            {
            }
        }

        //void ctlUrl_VMuktiEvent(object sender, VMuktiAPI.VMuktiEventArgs e)
        //{
        //    clsStartClass.sCurrentDispositionID=e._args[0].ToString();
        //    SetDispositionCanvas(clsStartClass.sCurrentDispositionID);
        //}

        void btnEnterOtherDispReason_Click(object sender, RoutedEventArgs e)
        {
            cnvDispoButtons.Visibility = Visibility.Visible;
            cnvDispoButtons.IsEnabled = false;
            cnvCallBack.Visibility = Visibility.Hidden;
            cnvOtherDispositon.Visibility = Visibility.Hidden;
            cnvDispositon.Visibility = Visibility.Hidden;
            //blApplicationExit = false;
            
            VMuktiAPI.VMuktiHelper.CallEvent("SetDisposition", this, new VMuktiAPI.VMuktiEventArgs(int.Parse(clsStartClass.sCurrentDispositionID), txtCallNote.Text, false, null, int.Parse(clsStartClass.sCurrentChannelID)));
            VMuktiAPI.VMuktiHelper.CallEvent("SetDialerEnable", this, new VMuktiAPI.VMuktiEventArgs(true));
            txtCallNote.Text = string.Empty;
        }

        void btnCancelOtherDispReason_Click(object sender, RoutedEventArgs e)
        {
            //cnvDispositon.Visibility = Visibility.Hidden;
            //cnvCallBack.Visibility = Visibility.Hidden;
            //cnvOtherDispositon.Visibility = Visibility.Hidden;
            //cnvDispoButtons.Visibility = Visibility.Visible;
            //cnvDispoButtons.IsEnabled = true;
            VMuktiAPI.VMuktiHelper.CallEvent("SetLeadIDScript", this, new VMuktiAPI.VMuktiEventArgs(clsStartClass.LeadID));
        }

        void btnCancelCallBackReason_Click(object sender, RoutedEventArgs e)
        {
            //cnvDispositon.Visibility = Visibility.Hidden;
            //cnvCallBack.Visibility = Visibility.Hidden;
            //cnvOtherDispositon.Visibility = Visibility.Hidden;
            //cnvDispoButtons.Visibility = Visibility.Visible;
            //cnvDispoButtons.IsEnabled = true;
            VMuktiAPI.VMuktiHelper.CallEvent("SetLeadIDScript", this, new VMuktiAPI.VMuktiEventArgs(clsStartClass.LeadID));
        }

        void btnEnterCallBackReason_Click(object sender, RoutedEventArgs e)
        {
            if (!CallBackInfoAvailable())
            {
                MessageBox.Show("Fill Proper Values For Call Back");
                return;
            }
            else
            {
                //set callback time
                string sCallBackDateTime = monthPicker.SelectedDate.Value.ToShortDateString();
                sCallBackDateTime += " " + cmbHour.SelectionBoxItem.ToString();
                sCallBackDateTime += ":" + cmbMin.SelectionBoxItem.ToString();
                sCallBackDateTime += ":00";
                sCallBackDateTime += " " + cmbAMPM.SelectionBoxItem.ToString();

                //if (sCallingType == "AutoMatic")
                //{
                //    VMuktiHelper.CallEvent("SetDisposition", this, new VMuktiEventArgs(sCurrentDispostion, txtCallBackReason.Text.ToString(), chkIsPublic.IsChecked, sCallBackDateTime, sCurrentChannelID));
                //    VMuktiHelper.CallEvent("SetDialerEnable", this, new VMuktiEventArgs(true));
                //}
                //else if (sCallingType == "Predictive")
                //{
                //    //VMuktiHelper.CallEvent("SetDispositionForPredictive", this, new VMuktiEventArgs(sCurrentDispostion, txtCallBackReason.Text.ToString(), chkIsPublic.IsChecked, sCallBackDateTime, sCurrentChannelID));
                //    VMuktiHelper.CallEvent("SetDispositionForPredictive", this, new VMuktiEventArgs(sCurrentDispostion, txtCallBackReason.Text.ToString(), chkIsPublic.IsChecked, sCallBackDateTime, sCurrentChannelID));
                //    VMuktiHelper.CallEvent("SetPredictiveDialerEnable", this, new VMuktiEventArgs(true, sCurrentChannelID));
                //}

                //VMuktiAPI.VMuktiHelper.CallEvent("CallHangUPFromRender", this, new VMuktiAPI.VMuktiEventArgs(true));
                VMuktiAPI.VMuktiHelper.CallEvent("SetDisposition", this, new VMuktiAPI.VMuktiEventArgs(int.Parse(clsStartClass.sCurrentDispositionID), txtCallBackReason.Text, chkIsPublic.IsChecked, sCallBackDateTime, int.Parse(clsStartClass.sCurrentChannelID)));
                VMuktiAPI.VMuktiHelper.CallEvent("SetDialerEnable", this, new VMuktiAPI.VMuktiEventArgs(true));

                cnvDispoButtons.Visibility = Visibility.Visible;
                cnvDispoButtons.IsEnabled = false;
                cnvDispositon.Visibility = Visibility.Hidden;
                cnvCallBack.Visibility = Visibility.Hidden;
                //blApplicationExit = false;
                txtCallBackReason.Text = string.Empty;
                txtCallBackNo.Text = string.Empty;
                chkIsPublic.IsChecked = false;
                monthPicker.SelectedDate.GetValueOrDefault();
                cmbAMPM.SelectedIndex = 0;
                cmbHour.SelectedIndex = 0;
                cmbMin.SelectedIndex = 0;
            }
        }

        bool CallBackInfoAvailable()
        {
                bool isAllValid = true;
                //if (txtCallBackReason.Text.Trim() == string.Empty)
                //{
                //    isAllValid = false;                
                //}          
                if (cmbMin.SelectedItem == null || cmbHour.SelectedItem == null || cmbAMPM.SelectedItem == null)
                {
                    isAllValid = false;
                }
                else if (object.Equals(monthPicker.SelectedDate, null))
                {
                    isAllValid = false;
                }
                else if (!object.Equals(monthPicker.SelectedDate, null))
                {
                    if (cmbMin.SelectedItem != null && cmbHour.SelectedItem != null && cmbAMPM.SelectedItem != null)
                    {
                        string sCallBackDateTime = monthPicker.SelectedDate.Value.ToShortDateString();
                        sCallBackDateTime += " " + cmbHour.SelectionBoxItem.ToString();
                        sCallBackDateTime += ":" + cmbMin.SelectionBoxItem.ToString();
                        sCallBackDateTime += ":00";
                        sCallBackDateTime += " " + cmbAMPM.SelectionBoxItem.ToString();
                        int i = DateTime.Compare(DateTime.Now, DateTime.Parse(sCallBackDateTime));
                        if (i > 0)
                        {
                            isAllValid = false;
                        }
                    }
                }
                return isAllValid;

        }

        void btnCancelDispReason_Click(object sender, RoutedEventArgs e)
        {
            //cnvDispositon.Visibility = Visibility.Hidden;
            //cnvCallBack.Visibility = Visibility.Hidden;
            //cnvOtherDispositon.Visibility = Visibility.Hidden;
            //cnvDispoButtons.Visibility = Visibility.Visible;
            //cnvDispoButtons.IsEnabled = true;
            VMuktiAPI.VMuktiHelper.CallEvent("SetLeadIDScript", this, new VMuktiAPI.VMuktiEventArgs(clsStartClass.LeadID));
        }

        void btnEnterDispReason_Click(object sender, RoutedEventArgs e)
        {
            //VMuktiAPI.VMuktiHelper.CallEvent("CallHangUPFromRender", this, new VMuktiAPI.VMuktiEventArgs(true));
            VMuktiAPI.VMuktiHelper.CallEvent("SetDisposition", this, new VMuktiAPI.VMuktiEventArgs(int.Parse(clsStartClass.sCurrentDispositionID), txtDNCReason.Text, true, null, int.Parse(clsStartClass.sCurrentChannelID)));
            VMuktiAPI.VMuktiHelper.CallEvent("SetDialerEnable", this, new VMuktiAPI.VMuktiEventArgs(true));
            cnvDispoButtons.Visibility = Visibility.Visible;
            cnvDispoButtons.IsEnabled = false;
            cnvDispositon.Visibility = Visibility.Hidden;
            cnvCallBack.Visibility = Visibility.Hidden;
            //blApplicationExit = false;
            txtDNCReason.Text = string.Empty;
            txtPhoneNo.Text = string.Empty;
        }

        //void FncPermissionsReview()
        //{
        //    this.Visibility = Visibility.Hidden;

        //    for (int i = 0; i < _MyPermissions.Length; i++)
        //    {
        //        if (_MyPermissions[i] == ModulePermissions.View)
        //        {
        //            this.Visibility = Visibility.Visible;
        //        }
        //    }
        //}

        void CtlWebScriptRender_Unloaded(object sender, RoutedEventArgs e)
        {
            VMuktiAPI.VMuktiHelper.UnRegisterEvent("SetLeadIDScript");
        }


        void ctlWebScriptRender_Loaded()
        {
            try
            {
               
                string URL = ClsScript.ClsGetScriptURL(int.Parse(VMuktiAPI.VMuktiInfo.CurrentPeer.ScriptID.ToString()));
            //    string URL = ClsScript.ClsGetScriptURL(4);
                string newURL;
                string[] URLSplit = URL.Split('?');
                newURL = URLSplit[0] + "?";

                string[] URLSplit2 = URLSplit[1].Split('&');

                for (int i = 0; i < URLSplit2.Length; i++)
                {
                    string[] varSplit = URLSplit2[i].Split('=');
                    string property = varSplit[1].Trim('<', '>');
                    string[] getProperty = property.Split('-');
                    if (getProperty.Length > 2)
                    {
                        string leadFormat = getProperty[1];
                        string fieldName = getProperty[2];
                        ClsLeadCollection objLeadColl = ClsLeadCollection.GetAll(clsStartClass.LeadID, fieldName, leadFormat);
                        string PropertyValue = objLeadColl[0].PropertyValue;
                        if (fieldName == "PhoneNumber")
                        {
                            sPhoneNumber = PropertyValue;
                        }
                        newURL += varSplit[0] + "=" + PropertyValue + "&";
                    }
                    else if (getProperty.Length == 2)
                    {
                        string PropertyValue="";
                        if(getProperty[1].ToLower() == "userid")
                            PropertyValue = VMuktiAPI.VMuktiInfo.CurrentPeer.ID.ToString();
                        else if (getProperty[1].ToLower() == "displayname")
                            PropertyValue = VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName;
                        else if (getProperty[1].ToLower() == "fname")
                            PropertyValue = VMuktiAPI.VMuktiInfo.CurrentPeer.FName;
                        else if (getProperty[1].ToLower() == "lname")
                            PropertyValue = VMuktiAPI.VMuktiInfo.CurrentPeer.LName;
                        else if (getProperty[1].ToLower() == "roleid")
                            PropertyValue = VMuktiAPI.VMuktiInfo.CurrentPeer.RoleID.ToString();
                        else if (getProperty[1].ToLower() == "email")
                            PropertyValue = VMuktiAPI.VMuktiInfo.CurrentPeer.EMail;

                        newURL += varSplit[0] + "=" + PropertyValue + "&";
                    }
                }

                newURL += "vLeadID=" + clsStartClass.LeadID;

                //if (newURL.LastIndexOf('&') == newURL.Length - 1)
                //{
                //    newURL = newURL.Substring(0, newURL.Length - 1);
                //}

                frmMain.Source = new Uri(newURL);
                frmMain.Visibility = Visibility.Visible;
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "ctlWebScriptRender_Loaded", "CtlUrl.xaml.cs");
            }
        }

        int callNo=1;
        string DispositionIDFromWeb;
        void WebScriptRender_VMuktiEvent(object sender, VMuktiAPI.VMuktiEventArgs e)
        {
            clsStartClass.LeadID = int.Parse(e._args[0].ToString());
            clsStartClass.sCurrentChannelID = e._args[0].ToString();
            ctlWebScriptRender_Loaded();

            dt.Interval = TimeSpan.FromSeconds(5);
            dt.Tick += new EventHandler(dt_Tick);
            dt.Start();
        }
        
        void dt_Tick(object sender, EventArgs e)
        {
            Object obj;
            obj = new objState(0);

            //ScriptRender.Presentation.WSVMukti.clsWSScript ws = new ScriptRender.Presentation.WSVMukti.clsWSScript();
            //DispositionIDFromWeb = ws.WSReturnDispositionID(callNo).ToString();
            
            if(DispositionIDFromWeb!="0")// OriginalValue==true)
            {
                clsStartClass.sCurrentDispositionID = DispositionIDFromWeb;
                SetDispositionCanvas(DispositionIDFromWeb);
                //VMuktiAPI.VMuktiHelper.CallEvent("CallHangUPFromRender", this, new VMuktiAPI.VMuktiEventArgs(true));
                //VMuktiAPI.VMuktiHelper.CallEvent("SetDisposition", this, new VMuktiAPI.VMuktiEventArgs(int.Parse(clsStartClass.sCurrentDispositionID), "Hello", false, null, int.Parse(clsStartClass.sCurrentChannelID)));
                //VMuktiAPI.VMuktiHelper.CallEvent("SetDialerEnable", this, new VMuktiAPI.VMuktiEventArgs(true));
            }
        }


        void SetDispositionCanvas(string strDispID)
        {
            try
            {
            if (strDispID == "11")
            {
                VMuktiAPI.VMuktiHelper.CallEvent("CallHangUPFromRender", this, new VMuktiAPI.VMuktiEventArgs(true));
                txtPhoneNo.Text = sPhoneNumber;
                cnvDispositon.Visibility = Visibility.Visible;
                frmMain.Visibility = Visibility.Hidden;
                cnvDispoButtons.IsEnabled = false;
                cnvDispoButtons.Visibility = Visibility.Hidden;
            }
            else if (strDispID == "6")
            {
                VMuktiAPI.VMuktiHelper.CallEvent("CallHangUPFromRender", this, new VMuktiAPI.VMuktiEventArgs(true));
                txtCallBackNo.Text = sPhoneNumber;
                cnvDispoButtons.IsEnabled = false;
                cnvCallBack.Visibility = Visibility.Visible;
                frmMain.Visibility = Visibility.Hidden;
                cnvOtherDispositon.Visibility = Visibility.Hidden;
                cnvDispoButtons.Visibility = Visibility.Hidden;
            }
            else
            {
                VMuktiAPI.VMuktiHelper.CallEvent("CallHangUPFromRender", this, new VMuktiAPI.VMuktiEventArgs(true));
                txtOtherPhoneNo.Text = sPhoneNumber;
                cnvDispoButtons.IsEnabled = false;
                cnvCallBack.Visibility = Visibility.Hidden;
                cnvDispositon.Visibility = Visibility.Hidden;
                frmMain.Visibility = Visibility.Hidden;
                cnvOtherDispositon.Visibility = Visibility.Visible;
                cnvDispoButtons.Visibility = Visibility.Hidden;
            }


            }
            catch (Exception ex)
            {
                
            }
        }
    

        void FireNextCall_VMuktiEvent(object sender, VMuktiAPI.VMuktiEventArgs e)
        {
            VMuktiAPI.VMuktiHelper.CallEvent("SetDispositionButtonClickEvent", this, new VMuktiAPI.VMuktiEventArgs(clsStartClass.sCurrentDispositionID));
        }

        private void Frame_Scroll(object sender, System.Windows.Controls.Primitives.ScrollEventArgs e)
        {

        }
    }
}

class objState
{
    private int a;
    public objState(int a)
    {
        this.a = a;
    }
}
