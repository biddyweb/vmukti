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
using System.Data;
using System.Data.SqlClient;
using Microsoft.Reporting.WinForms;
using System.Reflection;
using VMuktiAPI;



namespace rptUserInfo.Presentation
{
    /// <summary>
    /// Interaction logic for ctlRptUserInfo.xaml
    /// </summary>
    /// 
    public enum ModulePermissions
    {
        Add = 0,
        Edit = 1,
        Delete = 2,
        View = 3
    }
    public partial class ctlRptUserInfo : UserControl
    {
        public static StringBuilder sb1;
        DataSet dsReport = new dsUserDetails();
        ReportDataSource rds = new ReportDataSource();

        public ctlRptUserInfo(ModulePermissions[] MyPermissions)
        {
            try
            {
                InitializeComponent();
                this.Loaded += new RoutedEventHandler(ctlRptUserInfo_Loaded);
                comboBox1.Items.Clear();

                System.Data.DataSet ds = rptUserInfo.Business.ClsRptUserInfo.GetAllUserList();
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        ComboBoxItem cbi = new ComboBoxItem();
                        cbi.Content = ds.Tables[0].Rows[i][0].ToString();
                        cbi.Tag = ds.Tables[0].Rows[i][1].ToString();
                        comboBox1.Items.Add(cbi);
                    }
                }
            }
            catch (Exception ex)
            {
                ex.Data.Add("My Key", "VMukti--:--VmuktiModules--:--User Info--:--RptUserInfo--:--RptUserInfo.Presentation--:--ctlRptUserInfo.xaml.cs--:--ctlRptUserInfo(ModulePermissions[] MyPermissions)--");
                ClsException.LogError(ex);
                ClsException.WriteToErrorLogFile(ex);
                System.Text.StringBuilder sb = new StringBuilder();
                sb.AppendLine(ex.Message);
                sb.AppendLine();
                sb.AppendLine("StackTrace : " + ex.StackTrace);
                sb.AppendLine();
                sb.AppendLine("Location : " + ex.Data["My Key"].ToString());
                sb.AppendLine();
                sb1 = CreateTressInfo();
                sb.Append(sb1.ToString());
                VMuktiAPI.ClsLogging.WriteToTresslog(sb);
            }

        }

        void ctlRptUserInfo_Loaded(object sender, RoutedEventArgs e)
        {
            this.Width = ((Grid)this.Parent).ActualWidth;
            ((Grid)this.Parent).SizeChanged += new SizeChangedEventHandler(ctlRptUserInfo_SizeChanged);
        }

        void ctlRptUserInfo_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            this.Width = e.NewSize.Width;
        }


        private void button1_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                windowsFormsHost1.Visibility = Visibility.Visible;

                dsReport = Business.ClsRptUserInfo.GetUserInfo(int.Parse(((ComboBoxItem)comboBox1.SelectedItem).Tag.ToString()));

                if (dsReport.Tables.Count > 0)
                {
                    dsReport.Tables[0].TableName = "dtUserAllDetails";
                }
                rds.Name = "dsUserDetails_dtUserAllDetails";
                rds.Value = dsReport.Tables["dtUserAllDetails"];


                //objReportViewer.LocalReport.ReportPath = Assembly.GetAssembly(this.GetType()).Location.Replace("rptUserInfo.Presentation.dll", "rptUserAllDetails.rdlc");
                //objReportViewer.LocalReport.ReportEmbeddedResource = Assembly.GetAssembly(this.GetType()).Location.Replace("rptUserInfo.Presentation.dll", "rptUserAllDetails.rdlc");

                objReportViewer.LocalReport.ReportEmbeddedResource = "rptUserInfo.Presentation.rptUserAllDetails.rdlc";
                objReportViewer.LocalReport.DataSources.Clear();
                objReportViewer.LocalReport.DataSources.Add(rds);
                objReportViewer.RefreshReport();
            }
            catch (Exception ex)
            {
                ex.Data.Add("My Key", "VMukti--:--VmuktiModules--:--User Info--:--RptUserInfo--:--RptUserInfo.Presentation--:--ctlRptUserInfo.xaml.cs--:--ctlRptUserInfo(ModulePermissions[] MyPermissions)--");
                ClsException.LogError(ex);
                ClsException.WriteToErrorLogFile(ex);
                System.Text.StringBuilder sb = new StringBuilder();
                sb.AppendLine(ex.Message);
                sb.AppendLine();
                sb.AppendLine("StackTrace : " + ex.StackTrace);
                sb.AppendLine();
                sb.AppendLine("Location : " + ex.Data["My Key"].ToString());
                sb.AppendLine();
                sb1 = CreateTressInfo();
                sb.Append(sb1.ToString());
                VMuktiAPI.ClsLogging.WriteToTresslog(sb);

            }
        }

        private void comboBox1_DropDownOpened(object sender, EventArgs e)
        {
            windowsFormsHost1.Visibility = Visibility.Hidden;
        }

        private void button2_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                windowsFormsHost1.Visibility = Visibility.Visible;

                dsReport = Business.ClsRptUserInfo.GetOnlyLoginInfo(int.Parse(((ComboBoxItem)comboBox1.SelectedItem).Tag.ToString()));

                if (dsReport.Tables.Count > 0)
                {
                    dsReport.Tables[0].TableName = "dtUserLoginDetails";
                }
                rds.Name = "dsUserDetails_dtUserLoginDetails";
                rds.Value = dsReport.Tables["dtUserLoginDetails"];


                //objReportViewer.LocalReport.ReportPath = Assembly.GetAssembly(this.GetType()).Location.Replace("rptUserInfo.Presentation.dll", "rptUserAllDetails.rdlc");
                //objReportViewer.LocalReport.ReportEmbeddedResource = Assembly.GetAssembly(this.GetType()).Location.Replace("rptUserInfo.Presentation.dll", "rptUserAllDetails.rdlc");

                objReportViewer.LocalReport.ReportEmbeddedResource = "rptUserInfo.Presentation.rptUserLoginDetails.rdlc";
                objReportViewer.LocalReport.DataSources.Clear();
                objReportViewer.LocalReport.DataSources.Add(rds);
                objReportViewer.RefreshReport();
            }
            catch (Exception ex)
            {
                ex.Data.Add("My Key", "VMukti--:--VmuktiModules--:--User Info--:--RptUserInfo--:--RptUserInfo.Presentation--:--ctlRptUserInfo.xaml.cs--:--ctlRptUserInfo(ModulePermissions[] MyPermissions)--");
                ClsException.LogError(ex);
                ClsException.WriteToErrorLogFile(ex);
                System.Text.StringBuilder sb = new StringBuilder();
                sb.AppendLine(ex.Message);
                sb.AppendLine();
                sb.AppendLine("StackTrace : " + ex.StackTrace);
                sb.AppendLine();
                sb.AppendLine("Location : " + ex.Data["My Key"].ToString());
                sb.AppendLine();
                sb1 = CreateTressInfo();
                sb.Append(sb1.ToString());
                VMuktiAPI.ClsLogging.WriteToTresslog(sb);
            }
        }

        public static StringBuilder CreateTressInfo()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("User Is : " + VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName);
            sb.AppendLine();
            sb.Append("Peer Type is : " + VMuktiAPI.VMuktiInfo.CurrentPeer.CurrPeerType.ToString());
            sb.AppendLine();
            sb.Append("User's SuperNode is : " + VMuktiAPI.VMuktiInfo.CurrentPeer.SuperNodeIP);
            sb.AppendLine();
            sb.Append("User's Machine Ip Address : " + VMuktiAPI.GetIPAddress.ClsGetIP4Address.GetIP4Address());
            sb.AppendLine();
            sb.AppendLine("----------------------------------------------------------------------------------------");
            return sb;
        }
    }
}
