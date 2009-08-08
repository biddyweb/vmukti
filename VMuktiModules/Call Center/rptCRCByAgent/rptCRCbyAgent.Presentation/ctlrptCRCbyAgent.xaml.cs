/* VMukti 2.0 -- An Open Source Video Communications Suite
*
* Copyright (C) 2008 - 2009, VMukti Solutions Pvt. Ltd.
*
* Hardik Sanghvi <hardik@vmukti.com>
*
* See http://www.vmukti.com for more information about
* the VMukti project. Please do not directly contact
* any of the maintainers of this project for assistance;
* the project provides a web site, forums and mailing lists      
* for your use.

* This program is free software: you can redistribute it and/or modify
* it under the terms of the GNU General Public License as published by
* the Free Software Foundation, either version 3 of the License, or
* (at your option) any later version.

* This program is distributed in the hope that it will be useful,
* but WITHOUT ANY WARRANTY; without even the implied warranty of
* MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
* GNU General Public License for more details.

* You should have received a copy of the GNU General Public License
* along with this program. If not, see <http://www.gnu.org/licenses/>
 
*/
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
using System.IO;
using System.Diagnostics;
using rptCRCbyAgent.Business;
using System.Data;
using Microsoft.Reporting.WinForms;
using System.Reflection;
using VMuktiAPI;
using System.Data.SqlClient;
using rptCRCbyAgent.Presentation;
namespace rptCRCbyAgent.Presentation
{
    //Set permission for Report-CRCByAgent
    public enum ModulePermissions
    {
        Add = 0,
        Edit = 1,
        Delete = 2,
        View = 3
    }

    public partial class ctlRptCRCbyAgent : UserControl
    {
        //public static StringBuilder sb1;

        //DataSet dsReport = new dsCRCbyAgent();
        //ReportDataSource rds = new ReportDataSource();
        private DataSet m_dataSet;
        private MemoryStream m_rdl;
        ReportDataSource rds = new ReportDataSource();

        //public static StringBuilder CreateTressInfo()
        //{
        //    StringBuilder sb = new StringBuilder();
        //    sb.Append("User Is : " + VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName);
        //    sb.AppendLine();
        //    sb.Append("Peer Type is : " + VMuktiAPI.VMuktiInfo.CurrentPeer.CurrPeerType.ToString());
        //    sb.AppendLine();
        //    sb.Append("User's SuperNode is : " + VMuktiAPI.VMuktiInfo.CurrentPeer.SuperNodeIP);
        //    sb.AppendLine();
        //    sb.Append("User's Machine Ip Address : " + VMuktiAPI.GetIPAddress.ClsGetIP4Address.GetIP4Address());
        //    sb.AppendLine();
        //    sb.AppendLine("----------------------------------------------------------------------------------------");
        //    return sb;
        //}

        public ctlRptCRCbyAgent(ModulePermissions[] MyPermissions)
        {
            try
            {
            InitializeComponent();
            this.Loaded += new RoutedEventHandler(ctlRptRemainingLeads_Loaded);
            this.Loaded += new RoutedEventHandler(ctlRptCRCbyAgent_Loaded);
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "ctlRptCRCbyAgent.cs", "ctlRptCRCbyAgent.xaml.cs");
                
            }
        }

        
        
        //Generate Report after clicking Go...
        private void btnGo_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                CreateFile();
                wfhRptViewer.Visibility = Visibility.Visible;
                
        }

            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "btnGo_Click.cs", "ctlRptCRCbyAgent.xaml.cs");
            }
        }
        
        private void CreateFile()
        {
            try
            {
                //Krishna Code
               // DateTime dted = dtpEndDate.Value.Value;
                //DateTime dtst = dtpStartDate.Value.Value;
                DateTime temp = dtpStartDate.Value.Value;
                DateTime dtst = new DateTime(temp.Year, temp.Month, temp.Day, cmbStartHour.SelectedIndex, (cmbStartMinute.SelectedIndex == 1) ? 30 : 0, 0);
                temp = dtpEndDate.Value.Value;
                DateTime dted = new DateTime(temp.Year, temp.Month, temp.Day, cmbEndHour.SelectedIndex, (cmbEndMinute.SelectedIndex == 1) ? 30 : 0, 0);

                SqlConnection con = new SqlConnection(VMuktiAPI.VMuktiInfo.MainConnectionString);
                SqlCommand cmd = new SqlCommand("select distinct Campaign.Name,Campaign.ID from Campaign,Call where Campaign.startdate>='" + dtst + "' and enddate<='" + dted + "'", con);
                DataSet ds = new DataSet();
                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = cmd;
                da.Fill(ds);
                string xmlFileData = null;

                DataSet ds1, ds2, ds3, ds4,ds5;
                SqlDataAdapter da1, da2, da3, da4,da5;
                SqlCommand cmd1, cmd2, cmd3, cmd4,cmd5;

                if ((ds.Tables[0].Rows.Count) > 0)
                {
                    xmlFileData = "<DocumentElement>";
                    con.Open();
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {

                        xmlFileData += "<Campaign>";
                        xmlFileData += "<Name>" + ds.Tables[0].Rows[i][0].ToString() + "</Name>";

                        ds5 = new DataSet();
                        cmd5 = new SqlCommand("select DisplayName,ID from UserInfo where RoleID=(select ID from Roles where RoleName='Agent' and CreatedBy=(select CreatedBy from Campaign where Name='" + ds.Tables[0].Rows[i][0].ToString() + "'))", con);
                        da5 = new SqlDataAdapter(cmd5);
                        da5.Fill(ds5);
                        xmlFileData += "<AgentName>";
                        for (int k = 0; k < ds5.Tables[0].Rows.Count; k++)
                        {
                            xmlFileData += '\n'+ds5.Tables[0].Rows[k][0].ToString();
                        }
                        xmlFileData += "</AgentName>";
                        ds1 = new DataSet();
                        cmd1 = new SqlCommand("select distinct DespositionName from Disposition,Campaign where Disposition.ID in (select DispositionId from DispListDisp where DispositionListId=(select DespositionListID from CampaignDespoList where CampaignID=(select ID from Campaign where Name='" + ds.Tables[0].Rows[i][0].ToString() + "')))", con);
                        da1 = new SqlDataAdapter(cmd1);
                        da1.Fill(ds1);
                        if ((ds1.Tables[0].Rows.Count) > 0)
                        {


                            for (int j = 0; j < ds1.Tables[0].Rows.Count; j++)
                            {
                                xmlFileData += "<" + ds1.Tables[0].Rows[j][0].ToString() + ">";

                                ds2 = new DataSet();
                                da2 = new SqlDataAdapter();

                                cmd2 = new SqlCommand("select count(*)countcall from Call where DespositionID=(select ID from Disposition where DespositionName='" + ds1.Tables[0].Rows[j][0].ToString() + "') and CampaignID='" + ds.Tables[0].Rows[i][1].ToString() + "'", con);
                                da2.SelectCommand = cmd2;
                                da2.Fill(ds2);
                                xmlFileData += ds2.Tables[0].Rows[0][0].ToString();
                                for (int k = 0; k < ds5.Tables[0].Rows.Count; k++)
                                {
                                    DataSet dsUserCallCount = new DataSet();
                                    SqlCommand cmdUserCallCount = new SqlCommand("Select Count(*) from Call where GeneratedBy='" + ds5.Tables[0].Rows[k][1].ToString() + "' and CampaignID='" + ds.Tables[0].Rows[i][1].ToString() + "' and DespositionID=(Select ID from Disposition where DespositionName='" + ds1.Tables[0].Rows[j][0].ToString() + "') ", con);
                                    SqlDataAdapter daUserCallcount = new SqlDataAdapter(cmdUserCallCount);
                                    daUserCallcount.Fill(dsUserCallCount);
                                    xmlFileData += '\n' + dsUserCallCount.Tables[0].Rows[0][0].ToString();
                                }

                                xmlFileData += "</" + ds1.Tables[0].Rows[j][0].ToString() + ">";


                            }
                        }
                        ds3 = new DataSet();
                        cmd3 = new SqlCommand("select dbo.fn_Seconds(sum(DurationInSecond)) as TalkTime from Campaign,Call where Campaign.ID=Call.CampaignID and Campaign.Name='" + ds.Tables[0].Rows[i][0] + "'", con);
                        da3 = new SqlDataAdapter(cmd3);
                        da3.Fill(ds3);
                        xmlFileData += "<TalkTime>" + ds3.Tables[0].Rows[0][0].ToString();
                        for (int k = 0; k < ds5.Tables[0].Rows.Count; k++)
                        {
                            DataSet dsUserCallTime = new DataSet();
                            SqlCommand cmdUserCallTime = new SqlCommand("select dbo.fn_Seconds(sum(DurationInSecond)) as TalkTime from Campaign,Call where Campaign.ID=Call.CampaignID and Campaign.Name='" + ds.Tables[0].Rows[i][0] + "' and GeneratedBy='" + ds5.Tables[0].Rows[k][1].ToString() + "'", con);
                            SqlDataAdapter daUserCallTime = new SqlDataAdapter(cmdUserCallTime);
                            daUserCallTime.Fill(dsUserCallTime);
                            xmlFileData += '\n' + dsUserCallTime.Tables[0].Rows[0][0].ToString();
                        }
                        xmlFileData += "</TalkTime>";

                        ds4 = new DataSet();
                        cmd4 = new SqlCommand("select count(*) as TotalCall from Campaign,Call where Campaign.ID=Call.CampaignID and Campaign.Name='" + ds.Tables[0].Rows[i][0] + "'", con);
                        da4 = new SqlDataAdapter(cmd4);
                        da4.Fill(ds4);
                        xmlFileData += "<Totalcall>" + ds4.Tables[0].Rows[0][0].ToString();
                        for (int k = 0; k < ds5.Tables[0].Rows.Count; k++)
                        {
                            DataSet dsUserTotalCallCount = new DataSet();
                            SqlCommand cmdUserTotalCallCount = new SqlCommand("Select Count(*) from Call where GeneratedBy='" + ds5.Tables[0].Rows[k][1].ToString() + "' and CampaignID='" + ds.Tables[0].Rows[i][1].ToString() + "'", con);
                            SqlDataAdapter daUserTotalCallcount = new SqlDataAdapter(cmdUserTotalCallCount);
                            daUserTotalCallcount.Fill(dsUserTotalCallCount);
                            xmlFileData += '\n' + dsUserTotalCallCount.Tables[0].Rows[0][0].ToString();
                        }
                        xmlFileData += "</Totalcall>";
                        xmlFileData += "</Campaign>";


                    }

                    xmlFileData += "</DocumentElement>";
                    System.Xml.XmlDocument mydoc = new System.Xml.XmlDocument();
                    FileInfo fi = new FileInfo(AppDomain.CurrentDomain.BaseDirectory+"my.xml");
                    FileStream fstr = fi.Create();
                    fstr.Close();
                    StreamWriter sw = new StreamWriter(AppDomain.CurrentDomain.BaseDirectory + "my.xml");
                    sw.Write(xmlFileData);
                    sw.Close();
                    OpenDataFile(AppDomain.CurrentDomain.BaseDirectory + "my.xml");

                }
                else
                {
                    this.objReportViewer.Reset();
                    System.Windows.MessageBox.Show("No data found");
                }
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(ex.Message);
            }
        }

        private void OpenDataFile(string filename)
        {
            try
            {
                m_dataSet = new DataSet();
                m_dataSet.ReadXml(filename);
                List<string> selectedFields = GetAvailableFields();

                if (m_rdl != null)
                    m_rdl.Dispose();

                m_rdl = GenerateRdl(selectedFields, selectedFields);
                DumpRdl(m_rdl);

                ShowReport();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);//, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private List<string> GetAvailableFields()
        {
            DataTable dataTable = m_dataSet.Tables[0];
            List<string> availableFields = new List<string>();
            for (int i = 0; i < dataTable.Columns.Count; i++)
            {
                availableFields.Add(dataTable.Columns[i].ColumnName);
            }

            return availableFields;
        }

        private void ShowReport()
        {
            
            this.objReportViewer.Reset();
            this.objReportViewer.LocalReport.LoadReportDefinition(m_rdl);
            this.objReportViewer.LocalReport.DataSources.Add(new ReportDataSource("MyData", m_dataSet.Tables[0]));
            this.objReportViewer.RefreshReport();
        }

        private void DumpRdl(MemoryStream m_rdl)
        {
#if DEBUG_RDLC
            using (FileStream fs = new FileStream(@"c:\test.rdlc", FileMode.Create))
            {
                rdl.WriteTo(fs);
            }
#endif
        }

        private MemoryStream GenerateRdl(List<string> selectedFields, List<string> selectedFields_2)
        {
            MemoryStream ms = new MemoryStream();
            RdlGenerator gen = new RdlGenerator();
            gen.AllFields = selectedFields;
            gen.SelectedFields = selectedFields;
            gen.WriteXml(ms);
            ms.Position = 0;
            return ms;
        }
        
        #region changes for reportviewer resiging

        void ctlRptCRCbyAgent_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                if (this.Parent != null)
                {
                    if (this.Parent.GetType() == typeof(Grid))
                    {
                        this.Width = ((Grid)this.Parent).ActualWidth;
                        this.Height = ((Grid)this.Parent).ActualHeight;
                        wfhRptViewer.Height = ((Grid)this.Parent).ActualHeight - 150.0;
                        ((Grid)this.Parent).SizeChanged += new SizeChangedEventHandler(ctlRptCRCbyAgent_SizeChanged);
                    }
                    else if (this.Parent.GetType() == typeof(TabItem))
                    {

                        ((TabControl)((TabItem)this.Parent).Parent).SizeChanged += new SizeChangedEventHandler(ctlRptRemainingLeads_SizeChanged);
                        this.Width = ((TabControl)((TabItem)this.Parent).Parent).ActualWidth;
                        this.Height = ((TabControl)((TabItem)this.Parent).Parent).ActualHeight;
                        wfhRptViewer.Height = ((TabControl)((TabItem)this.Parent).Parent).ActualHeight - 150.0;
                        wfhRptViewer.Width = ((TabControl)((TabItem)this.Parent).Parent).ActualWidth - 130.0;
                    }
                }
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "ctlRptCRCbyAgent_Loaded()", "ctlRptCRCbyAgent.xaml.cs");
            }
        }

        void ctlRptRemainingLeads_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                if (this.Parent != null)
                {
                    if (this.Parent.GetType() == typeof(Grid))
                    {
                        ((Grid)this.Parent).SizeChanged += new SizeChangedEventHandler(ctlRptRemainingLeads_SizeChanged);
                        wfhRptViewer.Height = ((Grid)this.Parent).ActualHeight - 150.0;
                        wfhRptViewer.Width = ((Grid)this.Parent).ActualWidth - 4.0;
                    }
                    else if (this.Parent.GetType() == typeof(TabItem))
                    {
                        ((TabControl)((TabItem)this.Parent).Parent).SizeChanged += new SizeChangedEventHandler(ctlRptRemainingLeads_SizeChanged);
                        this.Width = ((TabControl)((TabItem)this.Parent).Parent).ActualWidth;
                        this.Height = ((TabControl)((TabItem)this.Parent).Parent).ActualHeight;
                        wfhRptViewer.Height = ((TabControl)((TabItem)this.Parent).Parent).ActualHeight - 150.0;
                        wfhRptViewer.Width = ((TabControl)((TabItem)this.Parent).Parent).ActualWidth - 130.0;
                    }
                }
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "ctlRptRemainingLeads_Loaded()", "ctlRptCRCbyAgent.xaml.cs");
            }
        }

        void ctlRptCRCbyAgent_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            try
            {
                if (this.Parent != null)
                {
                    if (this.Parent.GetType() == typeof(Grid))
                    {
                        this.Width = e.NewSize.Width;
                        this.Height = e.NewSize.Height;
                    }
                }
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "ctlRptCRCbyAgent_SizeChanged()", "ctlRptCRCbyAgent.xaml.cs");
            }
        }

        void ctlRptRemainingLeads_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            try
            {
                if (this.Parent != null)
                {
                    if (this.Parent.GetType() == typeof(Grid))
                    {
                        if (e.NewSize.Width > 0)
                        {
                            this.Width = e.NewSize.Width;
                            wfhRptViewer.Width = e.NewSize.Width - 4.0;
                            wfhRptViewer.Height = e.NewSize.Height - 150.0;
                        }
                    }
                    else if (this.Parent.GetType() == typeof(TabItem))
                    {
                        if (e.NewSize.Width > 0)
                        {
                            this.Width = e.NewSize.Width - 4.0;
                            wfhRptViewer.Width = e.NewSize.Width - 130.0;
                            wfhRptViewer.Height = e.NewSize.Height - 150.0;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "ctlRptRemainingLeads_SizeChanged()", "ctlRptCRCbyAgent.xaml.cs");
            }
        }

        #endregion

        private void dtpStartDate_DropDownOpened(object sender, RoutedEventArgs e)
        {
            wfhRptViewer.Visibility = Visibility.Hidden;
        }

        private void dtpEndDate_DropDownOpened(object sender, RoutedEventArgs e)
        {
            wfhRptViewer.Visibility = Visibility.Hidden;
        }
        private void cmbStartHour_DropDownOpened(object sender, EventArgs e)
        {
            wfhRptViewer.Visibility = Visibility.Hidden;
        }
    }
}