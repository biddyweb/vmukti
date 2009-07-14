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
using System.Windows.Forms;
using System.IO;
using System.Data;
using PlayFile.Business;
using System.Resources;
using System.Collections;
using System.Data.SqlClient;
using System.Net;
using System.Reflection;
using System.Windows.Forms.Integration;
//using Microsoft.Reporting.WinForms;
using Microsoft.Reporting.WinForms;

namespace PlayFile.Presentation
{
    /// <summary>
    /// Interaction logic for ctlPlayFile.xaml
    /// </summary>
    /// 

    public enum ModulePermissions
    {
        Add = 0,
        Edit = 1,
        Delete = 2,
        View = 3
    }


    public partial class ctlPlayFile : System.Windows.Controls.UserControl
    {
        DataSet ds1;
        ReportDataSource rds = new ReportDataSource();
        DataSet ds2;        
        string selectedPath;
        FolderBrowserDialog sfd;
        string strDate;
        string endDate;
        DateTime dtStart;
        string StartDate;
        int count;
        DateTime dtEnd;
        string EndDate;
        string objPlayFile = "";
        string leadID = "";
        string Campaign;
        string ClientName;
        string searchQry;
        public string[] array;
        ModulePermissions[] _MyPermissions;
       
        #region Cunstructor
        public ctlPlayFile(ModulePermissions[] MyPermissions)
        {
            InitializeComponent();
            try
            {
              
                //btnSearch.IsEnabled = false;
                cbCampaignName.Items.Clear();
                ClsGetCampaignCollection obj = new ClsGetCampaignCollection();
                obj = ClsGetCampaignCollection.GetCampaignCollection();
                
                for (int i = 0; i < obj.Count; i++)
                {
                    try
                    {
                        ComboBoxItem objCBI = new ComboBoxItem();
                        objCBI.Content = obj[i].Name;
                        cbCampaignName.Items.Add(objCBI);
                        //DateTime startdate = Convert.ToDateTime(dpStart.Value);
                    }
                    catch (Exception ex)
                    {
                    }
                   
                }
            }
            catch (Exception ex)
            {
            }
       
        }
        #endregion

        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (cbCampaignName.SelectedItem != null)
                {
                    if (strDate != null)
                    {
                        if (cbHHStart.SelectedItem != null && cbMMStart.SelectedItem != null )
                        {

                            StartDate = strDate;
                            StartDate += " " + cbHHStart.SelectionBoxItem.ToString();
                            StartDate += ":" + cbMMStart.SelectionBoxItem.ToString();
                            StartDate += ":00";                            
                            StartDate += " " + cbAMPMStart.SelectionBoxItem.ToString();
                            //System.Windows.MessageBox.Show(StartDate);


                            if (endDate != null)
                            {
                                if (cbHHEnd.SelectedItem != null && cbMMEnd.SelectedItem != null )
                                {
                                    {
                                        EndDate = endDate;
                                        EndDate += " " + cbHHEnd.SelectionBoxItem.ToString();
                                        EndDate += ":" + cbMMEnd.SelectionBoxItem.ToString();
                                        EndDate += ":00";
                                        EndDate += " " + cbAMPMEnd.SelectionBoxItem.ToString();
                                        //EndDate += " " + cbSSStart.SelectionBoxItem.ToString();
                                        //System.Windows.MessageBox.Show(EndDate);

                                        //searchQry = "select RecordedFileName from call where campaignid in(select ID from campaign where name='" + Campaign + "') and startdate>='" + strDate + "' and modifieddate<='" + endDate + "'";
                                        //searchQry = "select RecordedFileName from call where campaignid in(select ID from campaign where name='" + Campaign + "') and startdate>='" + StartDate + "' and modifieddate<='" + EndDate + "'";
                                        searchQry = "select * from call where campaignid in(select ID from campaign where name='" + Campaign + "') and startdate>='" + StartDate + "' and modifieddate<='" + EndDate + "'";
                                        
                                        

                                        if (tbAgentName.Text != null && tbAgentName.Text != "")
                                        {
                                            try
                                            {
                                                string AgentName = tbAgentName.Text;
                                                DataSet ds = ClsGetCamp4Phno.GetCamp4Agent(Campaign);
                                                if (ds.Tables[0].Rows.Count > 0)
                                                {
                                                    searchQry = searchQry + "and generatedby in (select id from userinfo where firstname='" + AgentName + "')";
                                                }
                                                else
                                                {
                                                    System.Windows.MessageBox.Show("Records Not Found");
                                                }
                                            }
                                            catch (Exception ex)
                                            {
                                            }
                                        }
                                        else
                                        {

                                        }
                                        if (cbClientName.SelectedItem != null && cbClientName.SelectedItem != "")
                                        {
                                            string temp = cbClientName.SelectedItem.ToString();

                                            string[] tempObj = temp.Split(':');
                                            ClientName = tempObj[1].ToString().TrimStart();
                                            searchQry = searchQry + "and leadid in(select leadid from leaddetail where propertyvalue='" + ClientName + "')";

                                        //searchQry= searchQry + "and leadid in(select leadid from leaddetail where propertyvalue='"+ClientName+')";
                                        }
                                        else
                                        {
                                        }
                                        if (lstCRC.SelectedItems.Count > 0)
                                        {
                                            for (int i = 0; i < lstCRC.SelectedItems.Count; i++)
                                            {
                                                if (i == 0)
                                                {
                                                   // objPlayFile = "'" + lstCRC.SelectedItems[i].ToString() + "'";
                                                    objPlayFile = "'" + ((ListBoxItem)lstCRC.SelectedItems[i]).Content.ToString() + "'";
                                                }
                                                else
                                                {
                                                    objPlayFile = objPlayFile + ",'" + lstCRC.SelectedItems[i].ToString() + "'";
                                                }
                                            }
                                        searchQry = searchQry + "and despositionid in(select id from disposition where despositionname in (" + objPlayFile + "))";
                                        }
                                        else
                                        {
                                        }

                                        if (tbPhoneNumber.Text != null && tbPhoneNumber.Text != "")
                                        {
                                            string PhoneNumber = tbPhoneNumber.Text;
                                            DataSet ds = ClsGetCamp4Phno.GetCamp4Phno(PhoneNumber);

                                            if (ds.Tables[0].Rows.Count > 0)
                                            {
                                                searchQry = searchQry + "and leadid in(select id from leads where phoneno='" + PhoneNumber + "')";
                                            }
                                            else
                                            {
                                                System.Windows.MessageBox.Show("Records Not Found");
                                            }
                                            //searchQry = searchQry + "and createddate='" + dpStart + "'";              
                                        }
                                        else
                                        {
                                            //searchQry = searchQry;
                                        }

                                        ds1 = PlayFile.Business.ClsGetSearchResult.GetSearchResult(searchQry);
                                        //PlayRecordedFileItems.Height = ds1.Tables[0].Rows.Count * 25;
                                        for (int j = 0 ; j < ds1.Tables[0].Rows.Count; j++)
                                        {
                                            if (j == 0)
                                            {
                                                leadID = "'" + ds1.Tables[0].Rows[j]["LeadID"].ToString() + "'";

                                            }
                                            else
                                            {
                                                leadID = leadID + ",'" + ds1.Tables[0].Rows[j]["LeadID"].ToString() + "'";
                                            }
                                        }
                                        //string phno = "select call.startdate,call.durationinsecond,call.duration,leads.phoneno from leads,call where leads.id in(" + leadID + ") and call.leadid in("+leadID+")";
                                        string phno = "select call.StartDate,call.DurationInSecond,call.RecordedFileName,leads.PhoneNo from leads,call where leads.id in(" + leadID + ") and call.leadid = leads.id";
                                        //DataSet ds2 = PlayFile.Business.ClsGetSearchResult.GetPhno(phno);

                                        ds2 = PlayFile.Business.ClsGetSearchResult.GetPhoneNo(phno);


                                        //List<PlayRecordedFile> objPlayRecordedFile = new List<PlayRecordedFile>();
                                        //for (int i = 0; i < ds2.Tables[0].Rows.Count; i++)
                                        //{
                                        //    objPlayRecordedFile.Add(PlayRecordedFile.Create(ds2.Tables[0].Rows[i]["StartDate"].ToString(), ds2.Tables[0].Rows[i]["DurationInSecond"].ToString(), ds2.Tables[0].Rows[i]["PhoneNo"].ToString(),ds2.Tables[0].Rows[i]["RecordedFileName"].ToString()));
                                        //}
                                       
                                        windowsFormsHost1.Visibility = Visibility.Visible;
                                       
                                        if (ds2.Tables.Count > 0)
                                        {
                                            ds2.Tables[0].TableName = "dtRecordedFileDetail";
                                        }
                                        rds.Name = "dsRecordedFileDetail_dtRecordedFileDetail";
                                        rds.Value = ds2.Tables["dtRecordedFileDetail"];
                                       

                                        objReportViewer.LocalReport.ReportEmbeddedResource = "PlayFile.Presentation.rptRecordedFileDetail.rdlc";
                                        objReportViewer.LocalReport.DataSources.Clear();
                                        objReportViewer.LocalReport.DataSources.Add(rds);
                                        objReportViewer.RefreshReport();

                                        count = ds2.Tables[0].Rows.Count;

                                        sfd = new FolderBrowserDialog();

                                        if (sfd.ShowDialog() == DialogResult.OK)
                                        {

                                        }
                                        else
                                        {
                                        }
                                        selectedPath = sfd.SelectedPath.ToString();
                                        for (int i = 0; i < ds2.Tables[0].Rows.Count; i++)
                                        {
                                            try
                                            {

                                                new WebClient().DownloadFile(VMuktiAPI.VMuktiInfo.ZipFileDownloadLink + "RecordedFile/" + ds2.Tables[0].Rows[i]["RecordedFileName"].ToString() + ".zip", sfd.SelectedPath.ToString() + "\\" + ds2.Tables[0].Rows[i]["RecordedFileName"].ToString());
                                                
                                            }
                                            catch (Exception ex)
                                            {
                                            }                         

                                        }
                                      
                                    }
                                }
                                else
                                {
                                    System.Windows.MessageBox.Show("Select End Time");
                                }
                            }
                            else
                            {
                                System.Windows.MessageBox.Show("Select End Date");
                            }
                        }
                        else
                        {
                            System.Windows.MessageBox.Show("Select Start Time");
                        }
                    }
                    else
                    {
                        System.Windows.MessageBox.Show("Select Start Date");
                    }
                    
                }
                else
                {
                    System.Windows.MessageBox.Show("Select Campaign Name");
                }
            }
            catch(Exception ex)
            {
            }
   
        }       
        private void cbCampaignName_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                
               
                string temp = cbCampaignName.SelectedItem.ToString();
                string[] tempObj = temp.Split(':');
                Campaign = tempObj[1].ToString().TrimStart();

               
                lstCRC.Items.Clear();
                ClsGetCRCCollection obj = new ClsGetCRCCollection();
                obj = ClsGetCRCCollection.GetCRCCollection(Campaign);

                for (int i = 0; i < obj.Count; i++)
                {
                     
                    ListBoxItem objLBI = new ListBoxItem();
                    objLBI.Content = obj[i].DespositionName;
                    lstCRC.Items.Add(objLBI);
                   
                }

                cbClientName.Items.Clear();
                ClsGetClientCollection obj1 = new ClsGetClientCollection();
                obj1 = ClsGetClientCollection.GetClientCollection(Campaign);

                for (int i = 0; i < obj1.Count; i++)
                {
                    ComboBoxItem objCBI = new ComboBoxItem();
                    objCBI.Content = obj1[i].PropertyValue;
                    cbClientName.Items.Add(objCBI);
                  
                }

            }
            catch(Exception ex)
            {
            }
        }

       public void dpStart_DropDownClosed(object sender, RoutedEventArgs e)
        {           
            dtStart =Convert.ToDateTime(dpStart.Value);
           strDate = dtStart.ToString("MM/dd/yyyy"); 
           
        }

       private void dpEnd_DropDownClosed(object sender, RoutedEventArgs e)
       {
           dtEnd = Convert.ToDateTime(dpEnd.Value);
           endDate = dtEnd.ToString("MM/dd/yyyy");
         
       }

       private void UserControl_Loaded(object sender, RoutedEventArgs e)
       {

       }     

       private void cbHHStart_SelectionChanged(object sender, SelectionChangedEventArgs e)
       {
          
       }

       private void ScrollViewer_ScrollChanged(object sender, ScrollChangedEventArgs e)
       {

       }
               
    }
}
