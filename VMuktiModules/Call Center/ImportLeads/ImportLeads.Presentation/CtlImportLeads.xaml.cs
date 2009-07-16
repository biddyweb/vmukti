/* VMukti 1.0 -- An Open Source Unified Communications Engine
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
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.IO;
using ImportLeads.Business;
using ImportLeads.DataAccess;
using System.Data;
using System.Data.SqlClient;
using System.Data.OleDb;
using VMuktiAPI;
using System.Resources;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using System.Diagnostics;
using System.Text;
using VMukti.ZipUnzip.Zip;

namespace ImportLeads.Presentation
{
    /// <summary>
    /// Interaction logic for CtlImportLeads.xaml
    /// </summary>
    /// 
    public enum ModulePermissions
    {
        Import = 0,        
    }
    public partial class CtlImportLeads : System.Windows.Controls.UserControl
    {
        
        int varTop = 30;

        int noOfRowsProcessed = 0;
        int badNumbers = 0;
        int repeatNumbers = 0;
        int NoAreacode = 0;

        DataTable dtLead;
        DataTable dtTrendWestLead;
        DataTable dtLeadDetail;
        DataTable dtLocation;
        DataTable dtTimezone;
        DataTable dtCountry;
        DataTable dtTimezone1;
        DataRow[] drCollection;
        DataTable dtPhoneNumber;
        DataTable TempData;
        DataSet dsLeadFormat;
        DataSet dsLeadFields;
        string[] str=null;
        int ListID =0;
        int CountryCode = 0;


        //shilpa code
        public delegate void DelSaveLead();
        public DelSaveLead objDelSaveLead;

        //progress bar
        int provalue=0;

        //timer
        //DispatcherTimer proTimer=null;



       // CheckBox[] chkTimeZone;
       // CheckBox chkAll = new CheckBox();
        OleDbConnection conn = null;

        string sheetName = "";
        string fileName = "";
        string filterType = "";

        int flag = 0;
        ModulePermissions[] _MyPermissions;

        public CtlImportLeads(ModulePermissions[] MyPermissions)
        {
            try
            {
                InitializeComponent();
                if (!System.IO.File.Exists(AppDomain.CurrentDomain.BaseDirectory.ToString() + "ImportLead"))
                {
                    FastZip fz = new FastZip();
                    fz.ExtractZip(AppDomain.CurrentDomain.BaseDirectory.ToString() + "ImportLead.zip", AppDomain.CurrentDomain.BaseDirectory.ToString() + "ImportLead", null);
                }
                btnSave.IsEnabled = false;
                _MyPermissions = MyPermissions;

                FncPermissionsReview();

                // Format Name //
                ClsFileFormatCollection objColl = ClsFileFormatCollection.GetAll();
                for (int i = 0; i < objColl.Count; i++)
                {
                    ComboBoxItem cbiFormat = new ComboBoxItem();
                    cbiFormat.Content = objColl[i].LeadFormatName;
                    cbiFormat.Tag = objColl[i].ID + "," + objColl[i].FormatType;
                    cmbFormat.Items.Add(cbiFormat);

                }

                
                // Format Name //


                // Country Name //

                //dsLeadFormat = (new ImportLeads.DataAccess.ClsImportLeadsDataService().GetLeadFormat());

                //for (int cnt = 0; cnt < dsLeadFormat.Tables[0].Rows.Count; cnt++)
                //{
                //    cmbFormat.Items.Add(dsLeadFormat.Tables[0].Rows[cnt][0].ToString());
                //}

                ClsCountryCollection objCountry = ClsCountryCollection.GetAll();
                for (int i = 0; i < objCountry.Count; i++)
                {
                    ComboBoxItem cbiCountry = new ComboBoxItem();
                    cbiCountry.Content = objCountry[i].Name;
                    cbiCountry.Tag = objCountry[i].CountryCode;

                    cmbCountry.Items.Add(cbiCountry);
                }

                // Country Name //
                cmbCountry.SelectionChanged += new SelectionChangedEventHandler(cmbCountry_SelectionChanged);
                txtDNC.LostFocus += new RoutedEventHandler(txtDNC_LostFocus);
                cmbListType.MouseDown += new MouseButtonEventHandler(cmbListType_MouseDown);
                cmbListType.SelectionChanged += new SelectionChangedEventHandler(cmbListType_SelectionChanged);
                btnBrowse.Click += new RoutedEventHandler(btnBrowse_Click);
                btnSave.Click += new RoutedEventHandler(btnSave_Click);

                //shilpa code
                btnCancel.Click += new RoutedEventHandler(btnCancel_Click);
                // cmbFilterType.SelectionChanged += new SelectionChangedEventHandler(cmbFilterType_SelectionChanged);
                lstLists.SelectionChanged += new SelectionChangedEventHandler(lstLists_SelectionChanged);
                txtFile.LostFocus += new RoutedEventHandler(txtFile_LostFocus);
                cmbFormat.SelectionChanged += new SelectionChangedEventHandler(cmbFormat_SelectionChanged);

                //shilpa code
                objDelSaveLead = new DelSaveLead(import_leads);
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "CtlImportLeads()", "CtlImportLeads.xaml.cs");
            }
           
        }

        //shilpa code
        void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                cmbFormat.Text = "";
                
                lstLists.Items.Clear();
                cmbCountry.Text = "";
                txtFile.Text = "";
                txtDNC.Text = "";
                chkGlobalDNC.IsChecked = false;
                lstFilterType.Items.Clear();
                btnSave.IsEnabled = false;
                cmbListType.Text = "";
            }

            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "btnCancel_Click()", "CtlImportLeads.xaml.cs");
            }
        }

      

        void cmbFormat_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                lstFilterType.Items.Clear();
                dsLeadFields = new DataSet();
                str = ((ListBoxItem)cmbFormat.SelectedItem).Tag.ToString().Split(',');

                dsLeadFields = ClsList.GetLeadFields(Convert.ToInt64(str[0]));
                ClsFilterNameCollection objFilterColl = ClsFilterNameCollection.Filter_GetName(str[0]);
                if (objFilterColl.Count > 0)
                {
                    lstFilterType.Visibility = Visibility.Visible;
                    for (int i = 0; i < objFilterColl.Count; i++)
                    {
                        ListBoxItem cbiFormat = new ListBoxItem();
                        cbiFormat.Content = objFilterColl[i].FilterName;
                        //cbiFormat.Tag = objFilterColl[i].ID + "," + objFilterColl[i].FieldId + "," + objFilterColl[i].FieldValues + "," + objFilterColl[i].Operator;
                        cbiFormat.Tag = objFilterColl[i].ID + "," + objFilterColl[i].FilterName;
                        lstFilterType.Items.Add(cbiFormat);
                    }
                }
                else
                {
                    lstFilterType.Visibility = Visibility.Hidden;
                }
                //MessageBox.Show(dsLeadFields.Tables[0].Rows.Count.ToString());
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "cmbFormat_SelectionChanged()", "CtlImportLeads.xaml.cs");
            }
        }

        void cmbCountry_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "cmbCountry_SelectionChanged()", "CtlImportLeads.xaml.cs");
            }

            //if (cmbCountry.SelectionBoxItem == null)
            //{
            //    btnBrowse.IsEnabled = false;
            //}
            //else
            //{
            //    btnBrowse.IsEnabled = true;
            //}
        }

        void txtDNC_LostFocus(object sender, RoutedEventArgs e)
        {
            try
            {
                if (txtDNC.Text.Length > 0)
                {
                    btnSave.IsEnabled = true;

                }
                else
                {
                    btnSave.IsEnabled = false;
                }
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "txtDNC_LostFocus()", "CtlImportLeads.xaml.cs");
            }

        }
        void FncPermissionsReview()
        {
            try
            {
                this.Visibility = Visibility.Hidden;

                for (int i = 0; i < _MyPermissions.Length; i++)
                {
                    if (_MyPermissions[i] == ModulePermissions.Import)
                    {
                        this.Visibility = Visibility.Visible;
                    }
                }
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "FncPermissionsReview()", "CtlImportLeads.xaml.cs");
            }
        }
        void txtFile_LostFocus(object sender, RoutedEventArgs e)
        {
            try
            {
                if (txtFile.Text.Length > 0)
                {

                    txtDNC.IsEnabled = false;
                    btnSave.IsEnabled = true;
                }
                else
                {
                    txtDNC.IsEnabled = true;
                    btnSave.IsEnabled = false;
                }
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "txtFile_LostFocus()", "CtlImportLeads.xaml.cs");
            }
        }

        void cmbListType_MouseDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
            }

            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "cmbListType_MouseDown()", "CtlImportLeads.xaml.cs");
            }
        }

        void lstLists_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                lblNoOfProcessed.Content = "";
                lblBadLeads.Content = "";
                lblRecordsInserted.Content = "";
                lblDuplicateLeads.Content = "";
                lblNoAreaCode.Content = "";
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "lstLists_SelectionChanged()", "CtlImportLeads.xaml.cs");
            }
        }

        void cmbFilterType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (txtFile.Text.Length > 0)
                {
                    //chkAll.IsChecked = false;
                    lblNoOfProcessed.Content = "";
                    lblBadLeads.Content = "";
                    lblRecordsInserted.Content = "";
                    lblDuplicateLeads.Content = "";
                    lblNoAreaCode.Content = "";

                    cnvCheck.Children.Clear();
                    DataTable WorksheetName = new DataTable();

                    dtTimezone = new DataTable();
                    string[] str = ((ListBoxItem)cmbFormat.SelectedItem).Tag.ToString().Split(',');
                    conn = new OleDbConnection();

                    if (str[1].ToUpper() == "EXCEL" || str[1].ToUpper() == "CSV")
                    {
                        
                        conn.ConnectionString = ExcelConnection();
                        conn.Open();

                        WorksheetName = conn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);

                        sheetName = (string)WorksheetName.Rows[0][WorksheetName.Columns[2].ColumnName];
                    }

                  /*  if (((ListBoxItem)cmbFilterType.SelectedItem).Content.ToString().ToUpper() == "TIMEZONE")
                    {
                        DataSet ds = ClsList.TimeZone_GetAll();
                        dtTimezone = ds.Tables[0];
                        chkAll.Content = "ALL TIMEZONES";
                        filterType = "TIMEZONE";
                    }
                    else if (((ListBoxItem)cmbFilterType.SelectedItem).Content.ToString().ToUpper() == "STATE")
                    {
                        
                        OleDbDataAdapter da1 = new OleDbDataAdapter("select distinct(State) from [" + sheetName + "]", ExcelConnection());
                        da1.Fill(dtTimezone);
                        da1.Dispose();
                        NAR(da1);
                        chkAll.Content = "ALL STATES";
                        filterType = "STATE";
                        
                    }
                    else if (((ListBoxItem)cmbFilterType.SelectedItem).Content.ToString().ToUpper() == "SITE")
                    {
                        
                        OleDbDataAdapter da1 = new OleDbDataAdapter("select distinct(site) from [" + sheetName + "]", ExcelConnection());
                        da1.Fill(dtTimezone);
                        da1.Dispose();
                        NAR(da1);
                        chkAll.Content = "ALL SITES";
                        filterType = "SITE";
                        DataSet ds = ClsList.TimeZone_GetAll();
                        dtTimezone1 = ds.Tables[0];
                        ds.Dispose();

                    }*/
                    conn.Close();
                    // TimeZone CheckBoxes //

                  //  chkTimeZone = new CheckBox[dtTimezone.Rows.Count];

                    //chkAll.FontSize = 15;
                    //Canvas.SetLeft(chkAll, 10);
                    //Canvas.SetTop(chkAll, 0);
                   // cnvCheck.Children.Add(chkAll);
                    //chkAll.Click += new RoutedEventHandler(chkAll_Click);

                    for (int i = 0; i < dtTimezone.Rows.Count; i++)
                    {
                       // chkTimeZone[i] = new CheckBox();
                        //chkTimeZone[i].Content = dtTimezone.Rows[i][0].ToString();
                       // Canvas.SetLeft(chkTimeZone[i], 10);
                       // Canvas.SetTop(chkTimeZone[i], ((i + 1) * varTop));
                       // chkTimeZone[i].Click += new RoutedEventHandler(CtlImportLeads_Click);
                       // cnvCheck.Children.Add(chkTimeZone[i]);
                    }

                    cnvCheck.Height = (dtTimezone.Rows.Count + 1) * varTop;
                }
                else if (flag == 0)
                {
                    MessageBox.Show("Select Import File Name First");
                }
               

            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "cmbFilterType_SelectionChanged()", "CtlImportLeads.xaml.cs");
            }
        }

        private void NAR(object o)
        {
            try
            {
                System.Runtime.InteropServices.Marshal.ReleaseComObject(o);
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "NAR()", "CtlImportLeads.xaml.cs");
            }
            finally
            {
                o = null;
            }
        }
        void CtlImportLeads_Click(object sender, RoutedEventArgs e)
        {
            try
            {
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "CtlImportLeads_Click()", "CtlImportLeads.xaml.cs");
            }
            /*try
            {
                int count = 0;
                for (int i = 0; i < chkTimeZone.Length; i++)
                {
                    if (chkTimeZone[i].IsChecked == true)
                    {
                        count++;
                    }
                }
                if (count == chkTimeZone.Length)
                {
                    chkAll.IsChecked = true;

                }
                else
                {
                    chkAll.IsChecked = false;

                }
                if (count == 0)
                {
                    btnSave.IsEnabled = false;
                }
                else
                {
                    btnSave.IsEnabled = true;
                }
            }
            catch (Exception ex)
            {
                ex.Data.Add("My Key", "VMukti--:--VmuktiModules--:--Call Center--:--ImportLeads--:--ImportLeads.DataAccess--:--ClsImportLeads.xaml.cs--:--CtlImportLeads_Click()--");
                ClsException.LogError(ex);
                ClsException.WriteToErrorLogFile(ex);

            }*/
        }

        /* void chkAll_Click(object sender, RoutedEventArgs e)
         {
             try
             {
                 for (int i = 0; i < chkTimeZone.Length; i++)
                 {
                     if (((CheckBox)sender).IsChecked == true)
                     {
                         chkTimeZone[i].IsChecked = true;
                         btnSave.IsEnabled = true;
                     }
                     else
                     {
                         chkTimeZone[i].IsChecked = false;
                         btnSave.IsEnabled = false;
                     }
                 }
             }
             catch (Exception ex)
             {
                 ex.Data.Add("My Key", "VMukti--:--VmuktiModules--:--Call Center--:--ImportLeads--:--ImportLeads.DataAccess--:--ClsImportLeads.xaml.cs--:--chkAll_Click()--");
                 ClsException.LogError(ex);
                 ClsException.WriteToErrorLogFile(ex);

             }
         }*/


        //Kanhaiya code
        //17-march-2008
        [STAThread]
        void btnSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                System.Threading.Thread t = new System.Threading.Thread(new System.Threading.ThreadStart(StartImportLead));
                t.Priority = System.Threading.ThreadPriority.Normal;
                t.Start();

            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "btnSave_Click()", "CtlImportLeads.xaml.cs");
            }
        }
        void StartImportLead()
        {
            try
            {
                this.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, objDelSaveLead);
            }
            catch(Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "StartImportLead()", "CtlImportLeads.xaml.cs");
            }
        }
        public void import_leads()
        {
            try
            {
                //FileStream Filewriter = new FileStream(AppDomain.CurrentDomain.BaseDirectory+"import_lead.xml", FileMode.OpenOrCreate);
                //System.Xml.XmlDocument mydoc = new System.Xml.XmlDocument();
                //mydoc.lo

                //making xml file
                
                if (lstLists.SelectedIndex == -1)
                {
                    MessageBox.Show("Select from List");
                }
                //else if (lstFilterType.SelectedIndex == -1)
                //{
                //    MessageBox.Show("Select Filter Type");
                //}
                else                 
                {
                    string xmlFileData = "<Root>";
                    //1)cmbFormat array of string 
                    string[] str = ((ListBoxItem)cmbFormat.SelectedItem).Tag.ToString().Split(',');

                    xmlFileData += "<cmbFormat>";
                    //
                    for (int i = 0; i < str.Length; i++)
                    {
                        //xmlFileData += "<item" + i + " value=" + str[i] + "\"></item" + i + ">";
                        xmlFileData += "<Item value='" + str[i] + "'></Item>";
                        //Filewriter.Write("");
                    }
                    xmlFileData += "</cmbFormat>";

                    //2)cmbListType
                    string cmbListValue = cmbListType.SelectionBoxItem.ToString();
                    string cmbListKey = cmbListType.SelectedIndex.ToString();
                    xmlFileData += "<cmbListType>";
                    xmlFileData += "<item1 key=\"" + cmbListKey + "\" value=\"" + cmbListValue + "\"></item1>";
                    xmlFileData += "</cmbListType>";

                    //3)lstLists SelectedItem
                    //(ListBoxItem)lstLists.SelectedItem).Tag
                    //lstLists.SelectedItems (list of all 

                    xmlFileData += "<lstLists>";

                    string lstListkey = ((ListBoxItem)lstLists.SelectedItem).ToString();
                    string lstListvalue = ((ListBoxItem)lstLists.SelectedItem).Tag.ToString();
                    xmlFileData += "<Item1 key=\"" + lstListkey + "\" value=\"" + lstListvalue + "\"></Item1>";
                    xmlFileData += "</lstLists>";

                    //4)lstFilterType
                    //all selected items and tags
                    //selectedIndex
                    //string[] strFilterValues = ((ListBoxItem)lstFilterType.SelectedItems[cntFilter]).Tag.ToString().Split(',');
                    //selectedItems.count
                    //Getting all condition of a filter

                    #region Getting all condition of a filter
                    if (lstFilterType.SelectedItems.Count > 0)
                    {
                        try
                        {
                            string ParValue = null;
                            for (int i = 0; i < lstFilterType.SelectedItems.Count; i++)
                            {
                                string[] strLeadFormatID = ((ListBoxItem)lstFilterType.SelectedItems[i]).Tag.ToString().Split(',');
                                ParValue += strLeadFormatID[0].ToString() + ",";
                                //ParValue=ParValue.Substring(0,ParValue.Length
                            }
                            ParValue = ParValue.Substring(0, ParValue.Length - 1);
                            ClsFilterTypeCollection objFilterColl = ClsFilterTypeCollection.Filter_GetAll(ParValue);
                            ListBox lstFilterTypeTemp = new ListBox();
                            lstFilterTypeTemp.SelectionMode = SelectionMode.Multiple;
                            for (int i = 0; i < objFilterColl.Count; i++)
                            {
                                ListBoxItem cbiFormat = new ListBoxItem();
                                cbiFormat.Content = objFilterColl[i].FilterName;
                                cbiFormat.Tag = objFilterColl[i].ID + "," + objFilterColl[i].FieldId + "," + objFilterColl[i].FieldValues + "," + objFilterColl[i].Operator;
                                //cbiFormat.Tag = objFilterColl[i].ID + "," + objFilterColl[i].FilterName;
                                lstFilterTypeTemp.Items.Add(cbiFormat);
                            }

                            lstFilterTypeTemp.SelectAll();

                            xmlFileData += "<lstFilterType>";
                            for (int i = 0; i < lstFilterTypeTemp.SelectedItems.Count; i++)
                            //for (int i = 0; i < lstFilterType.SelectedItems.Count; i++)
                            {
                                xmlFileData += "<Item value=\"" + ((ListBoxItem)lstFilterTypeTemp.SelectedItems[i]).Tag.ToString() + "\"></Item>";
                                //xmlFileData += "<Item value=\"" + ((ListBoxItem)lstFilterType.SelectedItems[i]).Tag.ToString() + "\"></Item>";
                            }
                            xmlFileData += "</lstFilterType>";
                        }
                        catch (Exception ex)
                        {
                            VMuktiHelper.ExceptionHandler(ex, "condition of a filter for making XML", "CtlImportLeads.xaml.cs");
                        }
                    }
                    #endregion

                    //5)cmbCountry
                    //selecteditem.tag
                    xmlFileData += "<cmbCountry>";
                    xmlFileData += "<Item value='" + int.Parse(((ComboBoxItem)cmbCountry.SelectedItem).Tag.ToString()) + "'></Item>";
                    xmlFileData += "</cmbCountry>";

                    //6)txtFile
                    //filename and path
                    xmlFileData += "<FileName>";
                    xmlFileData += "<Item value=\"" + txtFile.Text + "\"></Item>";
                    xmlFileData += "</FileName>";

                    //7)txtDNC
                    //IsEnabled or not   text
                    xmlFileData += "<txtDNC>";
                    xmlFileData += "<Item key='" + txtDNC.IsEnabled.ToString() + "' value='" + txtDNC.Text + "'></Item>";
                    xmlFileData += "</txtDNC>";

                    //8)chkGlobalDNC
                    //checked or not
                    xmlFileData += "<chkGlobalDNC>";
                    xmlFileData += "<Item key='" + chkGlobalDNC.IsChecked.ToString() + "'></Item>";
                    xmlFileData += "</chkGlobalDNC>";

                    //9)connectionstring
                    string connString = VMuktiAPI.VMuktiInfo.MainConnectionString;
                    xmlFileData += "<connectionString>";
                    xmlFileData += connString;
                    xmlFileData += "</connectionString>";

                    //9)id
                    string Vid = VMuktiAPI.VMuktiInfo.CurrentPeer.ID.ToString();
                    xmlFileData += "<VmuktiID>";
                    xmlFileData += Vid;
                    xmlFileData += "</VmuktiID>";

                    xmlFileData += "</Root>";

                    System.Xml.XmlDocument mydoc = new System.Xml.XmlDocument();
                    //mydoc.CreateComment(xmlFileData);
                    mydoc.LoadXml(xmlFileData);
                    mydoc.Save(AppDomain.CurrentDomain.BaseDirectory + "\\ImportLead\\Config_ImportLeads.xml");

                    
                    
                    Process leadImport = new Process();
                    leadImport.StartInfo.FileName = AppDomain.CurrentDomain.BaseDirectory + "\\ImportLead\\ImportLeads.exe";
                    //leadImport.StartInfo.Arguments = xmlFileData;
                    leadImport.Start();
                    leadImport.PriorityClass = ProcessPriorityClass.BelowNormal;

                    lblFormat.Visibility=Visibility.Hidden ;
                    cmbFormat.Visibility=Visibility.Hidden ;
                    lblListType.Visibility=Visibility.Hidden ;
                    cmbListType.Visibility=Visibility.Hidden ;
                    lblList.Visibility=Visibility.Hidden ;
                    lstLists.Visibility=Visibility.Hidden ;
                    lblCountry.Visibility=Visibility.Hidden ;
                    cmbCountry.Visibility=Visibility.Hidden ;
                    lblFile.Visibility=Visibility.Hidden ;
                    txtFile.Visibility=Visibility.Hidden ;
                    btnBrowse.Visibility=Visibility.Hidden ;
                    lblDNC.Visibility=Visibility.Hidden ;
                    txtDNC.Visibility=Visibility.Hidden ;
                    lblGlobalDNC.Visibility=Visibility.Hidden ;
                    chkGlobalDNC.Visibility=Visibility.Hidden ;
                    btnSave.Visibility=Visibility.Hidden ;
                    btnCancel.Visibility=Visibility.Hidden ;
                    lblSummary.Visibility = Visibility.Hidden;
                    cnvCheck.Visibility = Visibility.Hidden;

                    lblimport.Visibility = Visibility.Visible;


                    //cnvUpper.Visibility = Visibility.Hidden;
                    //cnvUpper.Visibility = Visibility.Visible;
                    //exe parameter
                    //string scmbFormat ;
                    //scmbFormat = ((ListBoxItem)cmbFormat.SelectedItem).Tag.ToString();
                    //string cmbFormat1 = ((ListBoxItem)cmbFormat.SelectedItem).Tag.ToString(); 
                    //string fileType = cmbFormat1;
                    //string scmbListType = ((ComboBoxItem)cmbListType.SelectedItem).Tag.ToString(); //"2";       //cmbListType.  //cmbListType.SelectedIndex ;
                    //string paraListID = ((ListBoxItem)lstLists.SelectedItem).Tag.ToString();
                    //string FileName = txtFile.Text;
                    //string FilterType = lstFilterType.SelectedItems.ToString();
                    //string scmbCountry = ((ComboBoxItem)cmbCountry.SelectedItem).Tag.ToString();
                    //string stxtDNC = txtDNC.Text;
                    //string schkGlobalDNC = chkGlobalDNC.IsChecked.ToString();

                    //Process insert_leads = new Process();
                    //insert_leads.StartInfo.FileName = AppDomain.CurrentDomain.BaseDirectory + "importExe.exe";
                    //insert_leads.StartInfo.Arguments  = scmbFormat + "\\" + scmbListType + "\\" + paraListID + "\\" + FileName + "\\" + FilterType + "\\" + scmbCountry + "\\" + schkGlobalDNC;
                    //insert_leads.Start();

                    //       //         string ListType = ;
                    //       //         
                    //       //         
                }
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "import_leads()", "CtlImportLeads.xaml.cs");
            }
        }

        //void btnSave_Click(object sender, RoutedEventArgs e)
        //{
        //    try
        //       {
        //       this.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, objDelSaveLead);
        //       // MessageBox.Show(AppDomain.CurrentDomain.BaseDirectory);
        //        //shilpa code
        //        //ImageBrush berriesBrush = new ImageBrush();
        //        //berriesBrush.ImageSource = new BitmapImage(new Uri(AppDomain.CurrentDomain.BaseDirectory.ToString() + "\\loading.gif", UriKind.Relative));
        //        //imgwait.Source = (System.Windows.Media.ImageSource)berriesBrush;

        //        //imgwait.Source =AppDomain.CurrentDomain.BaseDirectory.ToString() + "loading.gif";
        //        //BitmapImage objbitmap = new BitmapImage(new Uri(AppDomain.CurrentDomain.BaseDirectory.ToString() + "loading.gif"));
        //        //imgwait.Source = objbitmap;
        //            //new Uri(AppDomain.CurrentDomain.BaseDirectory.ToString() + "loading.gif");
        //        //imgwait.Visibility = Visibility.Visible;
        //       // MessageBox.Show("Image is done");
        //        //shilpa code
                
        //        if(lstLists.SelectedIndex ==-1)
        //        {
        //            MessageBox.Show("Select from List");
        //        }
        //        else if (lstFilterType.SelectedIndex == -1)
        //        {
        //            MessageBox.Show("Select Filter Type");
        //        }
        //        else
        //        {
        //            //progress bar
        //            //proimport.Visibility = Visibility.Visible;
                    
        //            //proimport.Minimum = 0;
        //            //proimport.Value = 0;
        //            //proimport.Maximum = 20;
        //            //proimport.Minimum = 0;
        //            //proTimer = new DispatcherTimer();
        //            //proTimer.Interval = new TimeSpan(1000);
        //            //proTimer.Tick += new EventHandler(proTimer_Tick);
        //            //proTimer.Start();
        //            //proTimer.IsEnabled = true;
                     

        //            //end code
        //            CountryCode = int.Parse(((ComboBoxItem)cmbCountry.SelectedItem).Tag.ToString());

        //            // Getting Fields For A Format //
        //            if (cmbListType.SelectedIndex == 1)
        //            {
        //                conn = new OleDbConnection();
        //                conn.ConnectionString = ExcelConnection();
        //                conn.Open();
        //                DataTable WorksheetName = new DataTable();

        //                WorksheetName = conn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);

        //                sheetName = (string)WorksheetName.Rows[0][WorksheetName.Columns[2].ColumnName];
        //                conn.Close();
        //                Int64 count = ClsList.GetMaxIDLeads();
        //                // Int64 MaxLocationID = ClsList.GetMaxLocationID();
        //                string[] str = ((ListBoxItem)cmbFormat.SelectedItem).Tag.ToString().Split(',');
        //                createDataTable();

        //               DataSet ds1 = ClsList.PhoneNumbers_GetAll(int.Parse(((ListBoxItem)lstLists.SelectedItem).Tag.ToString()));
        //                dtPhoneNumber = ds1.Tables[0];

        //                if (txtDNC.IsEnabled == true && txtDNC.Text.Length > 0)
        //                {
        //                    //SqlConnection conn = new SqlConnection("Data Source=192.168.1.186\\SQLExpress;Initial Catalog=VMukti;User ID=sa;PassWord=mahavir");
        //                    noOfRowsProcessed = 1;
        //                    string str2 = Convert.ToString(txtDNC.Text.Replace("-", ""));

        //                    Int64 phonenum = 0;
        //                    try
        //                    {
        //                        phonenum = String.IsNullOrEmpty(str2) ? 0 : Int64.Parse(str2);
        //                        phonenum = Int64.Parse((Math.Pow(10 * CountryCode, (phonenum.ToString().Length))).ToString()) + phonenum;
        //                    }
        //                    catch (Exception exp)
        //                    {
        //                        badNumbers++;
        //                    }

        //                    DataRow[] drRepeat = dtPhoneNumber.Select("PhoneNo = " + phonenum);
        //                    if (drRepeat.Length > 0)
        //                    {
        //                        repeatNumbers++;
        //                    }
        //                    else
        //                    {
        //                        SqlConnection conn1 = new SqlConnection(VMuktiAPI.VMuktiInfo.MainConnectionString);
        //                        SqlCommand cmd = new SqlCommand("select getdate()", conn1);
        //                        conn1.Open();
        //                        SqlDataReader dr = cmd.ExecuteReader();
        //                        dr.Read();
        //                        dtLead.Rows.Add(count, phonenum, str[0], Convert.ToDateTime(dr[0].ToString()), VMuktiAPI.VMuktiInfo.CurrentPeer.ID, null, null, false, Convert.ToDateTime(dr[0].ToString()), VMuktiAPI.VMuktiInfo.CurrentPeer.ID, true, 1, int.Parse(((ListBoxItem)lstLists.SelectedItem).Tag.ToString()), 1, 0, "Fresh", chkGlobalDNC.IsChecked == true ? true : false);
        //                        conn1.Close();
        //                    }
        //                }
        //                else if (txtFile.Text.Length > 0 && lstLists.SelectedItems.Count > 0)
        //                {

        //                    TempData = new DataTable();

        //                    OleDbDataAdapter da = new OleDbDataAdapter("select * from [" + sheetName + "]", ExcelConnection());
        //                    da.Fill(TempData);
        //                    da.Dispose();
        //                    noOfRowsProcessed = TempData.Rows.Count;


        //                    //progress bar 

        //                    //Process progress = new Process();
        //                    //progress.StartInfo.FileName  = AppDomain.CurrentDomain.BaseDirectory + "\\Progress_Bar.exe";
        //                    //progress.Start();
        //                    for (int i = 0; i < TempData.Rows.Count; i++)
        //                    {
        //                        // SqlConnection conn1 = new SqlConnection("Data Source=192.168.1.186\\SQLExpress;Initial Catalog=VMukti;User ID=sa;PassWord=mahavir");
        //                        //if (proimport.Maximum == proimport.Value)
        //                        //{
        //                        //    proimport.Visibility = Visibility.Hidden;
        //                        //    proimport.Value =0;
        //                        //    proimport.Visibility = Visibility.Visible ;
        //                        //}
        //                        //else
        //                        //{
        //                        //    proimport.Visibility = Visibility.Hidden;
        //                        //    proimport.Value += 1;
        //                        //    proimport.Visibility = Visibility.Visible;
        //                        //}
        //                       // MessageBox.Show(proimport.Value.ToString());
        //                        string str2 = Convert.ToString(TempData.Rows[i][0].ToString().Replace("-", ""));

        //                        Int64 phonenum;
        //                        try
        //                        {
        //                            phonenum = String.IsNullOrEmpty(str2) ? 0 : Int64.Parse(str2);
        //                            phonenum = Int64.Parse((Math.Pow(10 * CountryCode, (phonenum.ToString().Length))).ToString()) + phonenum;
        //                        }
        //                        catch (Exception exp)
        //                        {
        //                            badNumbers++;
        //                            continue;
        //                        }
        //                        DataRow[] drRepeat = dtPhoneNumber.Select("PhoneNo = " + phonenum);
        //                        if (drRepeat.Length > 0)
        //                        {
        //                            repeatNumbers++;
        //                            continue;
        //                        }
        //                        else
        //                        {
        //                            SqlConnection conn1 = new SqlConnection(VMuktiAPI.VMuktiInfo.MainConnectionString);
        //                            SqlCommand cmd = new SqlCommand("select getdate()", conn1);
        //                            conn1.Open();
        //                            SqlDataReader dr = cmd.ExecuteReader();
        //                            dr.Read();
        //                            dtLead.Rows.Add(count, phonenum, str[0], Convert.ToDateTime(dr[0].ToString()), VMuktiAPI.VMuktiInfo.CurrentPeer.ID, null, null, false, Convert.ToDateTime(dr[0].ToString()), VMuktiAPI.VMuktiInfo.CurrentPeer.ID, true, 1, int.Parse(((ListBoxItem)lstLists.SelectedItem).Tag.ToString()), 1, 0, "Fresh", chkGlobalDNC.IsChecked == true ? true : false);
        //                            count++;
        //                            conn1.Close();
        //                        }

        //                        provalue += 1;
        //                    }
        //                    //progress.Kill();
        //                }
        //            }
        //            else
        //            {
        //                string[] str = ((ListBoxItem)cmbFormat.SelectedItem).Tag.ToString().Split(',');

        //                string strTimezone = "";
        //                int TotalTimeZone = 0;

        //                /*   if (chkTimeZone != null)
        //                   {
        //                       for (int j = 0; j < chkTimeZone.Length; j++)
        //                       {
        //                           if (chkTimeZone[j].IsChecked == true)
        //                           {
        //                               TotalTimeZone = TotalTimeZone + 1;
        //                               if (strTimezone == "")
        //                                   strTimezone = "'" + chkTimeZone[j].Content.ToString() + "'";
        //                               else
        //                                   strTimezone = strTimezone + ",'" + chkTimeZone[j].Content.ToString() + "'";
        //                           }
        //                       }
        //                   }
        //                   if (strTimezone.Length <= 0)
        //                   {
        //                       MessageBox.Show("Select Item");
        //                       goto jump2;
        //                   }*/

        //                ListID = int.Parse(((ListBoxItem)lstLists.SelectedItem).Tag.ToString());
        //                //////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //                if (str[1].ToUpper() == "EXCEL" || str[1].ToUpper() == "CSV")
        //                {
        //                    conn = new OleDbConnection();
        //                    conn.ConnectionString = ExcelConnection();
                            
        //                    conn.Open();
        //                    DataTable WorksheetName = new DataTable();
        //                    //conn.Close();
        //                    WorksheetName = conn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);

        //                    sheetName = (string)WorksheetName.Rows[0][WorksheetName.Columns[2].ColumnName];
        //                    conn.Close();

        //                    if (txtFile.Text.Length <= 0 || lstLists.SelectedItems.Count <= 0)
        //                    {
        //                        goto jump2;
        //                    }
        //                    createDataTable();
        //                    OleDbDataAdapter da = new OleDbDataAdapter("select *,Left([phone number],3) as AreaCod from [" + sheetName + "]", ExcelConnection());
        //                    //OleDbDataAdapter da = new OleDbDataAdapter("select * from  Sheet1$", ExcelConnection());
        //                    string dainfo= da.ToString();
        //                    da.Fill(dtTrendWestLead);
        //                    da.Dispose();
        //                    NAR(da);

        //                    if ((dtTrendWestLead.Columns.Count - 1) != dsLeadFields.Tables[0].Rows.Count)
        //                    {
        //                        MessageBox.Show("Total no of ExcelSheet Columns not match with LeadFormat Columns");
        //                        goto jump2;
        //                    }
        //                    else if ((dsLeadFields.Tables[0].Select("CustomFieldID=1")).Count() == 0)
        //                    {
        //                        MessageBox.Show("Lead Format Must Have Phone Number Field Map with Excel Column");
        //                        goto jump2;
        //                    }
        //                    DataSet StateCountry = ClsList.StateCountry_GetAll();

        //                    DataSet ds1 = ClsList.PhoneNumbers_GetAll(int.Parse(((ListBoxItem)lstLists.SelectedItem).Tag.ToString()));
        //                    dtPhoneNumber = ds1.Tables[0];

        //                    DataRow[] drStateCountry = null;

        //                    DataTable dtTemp = new DataTable();

        //                    /* if (filterType == "TIMEZONE" || filterType == "SITE")
        //                     {
        //                        // DataSet ds = ClsList.AreaCode_GetAll(strTimezone);
        //                       //  dtTemp = ds.Tables[0];
        //                         // drCollection = dtTemp.Select("TimezoneName IN (" + strTimezone + ")");


        //                     }*/

        //                    // strTimezone = strTimezone.Trim('\'');

        //                    // string[] strNoTime = strTimezone.ToString().Split(',');
        //                    // for (int i = 0; i < strNoTime.Length; i++)
        //                    // {
        //                    //     strNoTime[i] = strNoTime[i].Trim('\'');
        //                    // }
        //                    Int64 globalCounter = ClsList.GetMaxIDTrendWestID();
        //                    Int64 count = ClsList.GetMaxIDLeads();
        //                    Int64 MaxLocationID = ClsList.GetMaxLocationID();
        //                    Int64 MaxLeadDetail = ClsList.GetMaxLeadDetailID();

        //                    //  OleDbDataAdapter da = new OleDbDataAdapter("select 1 as A1,1 as A2,*,1 As Work1,Left([phone number],3) as AreaCod from [" + sheetName + "]", ExcelConnection());

        //                    int co = dtTrendWestLead.Rows.Count - 1;
        //                    noOfRowsProcessed = dtTrendWestLead.Rows.Count;

        //                    DataRow[] drPhoneColumn = dsLeadFields.Tables[0].Select("CustomFieldID=1");
        //                    int PhoneCol = int.Parse(drPhoneColumn[0][0].ToString());

        //                    DataRow[] drStateColumn = dsLeadFields.Tables[0].Select("CustomFieldID=2");
        //                    int StateCol = int.Parse(drStateColumn[0][0].ToString());

        //                    DataRow[] drZipColumn = dsLeadFields.Tables[0].Select("CustomFieldID=3");
        //                    int ZipCol = int.Parse(drZipColumn[0][0].ToString());
        //                    int counter = 0;
        //                    //proimport.Maximum = co;


        //                    //Process progress = new Process();
        //                    ////progress.PriorityBoostEnabled = false;
        //                    //progress.StartInfo.FileName  = AppDomain.CurrentDomain.BaseDirectory + "Progress_Bar.exe";
        //                    ////progress.StartInfo.Arguments = co.ToString();
        //                    //progress.Start();
                            
        //                    for (int i = co; i >= 0; i--)
        //                    {
                                
                                
        //                        //MessageBox.Show(proimport.Value.ToString());
        //                        int varcount = 0;
        //                        Int64 phonenum = 0;

        //                        string str2 = Convert.ToString(dtTrendWestLead.Rows[i][PhoneCol].ToString().Replace("-", ""));
        //                        string zipcod = Convert.ToString(dtTrendWestLead.Rows[i][ZipCol].ToString());

        //                        try
        //                        {
        //                            Int64 finalzipcode = String.IsNullOrEmpty(zipcod) ? 0 : Int64.Parse(zipcod);
        //                            drStateCountry = StateCountry.Tables[0].Select("zipcode=" + finalzipcode);
        //                        }
        //                        catch (Exception exp)
        //                        {
        //                            //if (exp.Message.ToLower().Contains("input string") == true)
        //                            //{
        //                            //    MessageBox.Show("Phone Column format is not matching");
        //                            //    goto jump2;
        //                            //}
        //                            badNumbers++;
        //                            dtTrendWestLead.Rows.RemoveAt(i);
        //                            continue;
        //                        }



        //                        try
        //                        {
        //                            phonenum = String.IsNullOrEmpty(str2) ? 0 : Int64.Parse(str2);
        //                            phonenum = Int64.Parse((Math.Pow(10 * CountryCode, (phonenum.ToString().Length))).ToString()) + phonenum;
        //                        }
        //                        catch (Exception exp)
        //                        {
        //                            if (exp.Message.ToLower().Contains("input string") == true)
        //                            {
        //                                MessageBox.Show("Phone Column format is not matching");
        //                                goto jump2;
        //                            }
        //                            badNumbers++;
        //                            dtTrendWestLead.Rows.RemoveAt(i);
        //                            continue;
        //                        }

        //                        DataRow[] drRepeat = dtPhoneNumber.Select("PhoneNo = " + phonenum);
        //                        if (drRepeat.Length > 0)
        //                        {
        //                            repeatNumbers++;
        //                            dtTrendWestLead.Rows.RemoveAt(i);
        //                            continue;
        //                        }
        //                        dtPhoneNumber.Rows.Add(phonenum);

        //                        // Checking For Filtering //
        //                        Int64 FieldID = 0;
        //                        int condition = 0;

        //                        string FieldValues = "";
        //                        DataRow drLocation;
        //                        DataRow[] AreaCod = null;

        //                        try
        //                        {
        //                            for (int cntFilter = 0; cntFilter < lstFilterType.SelectedItems.Count; cntFilter++)
        //                            {

        //                                string[] strFilterValues = ((ListBoxItem)lstFilterType.SelectedItems[cntFilter]).Tag.ToString().Split(',');
        //                                FieldValues = strFilterValues[2].Replace('~', ',');
        //                                FieldID = Convert.ToInt64(strFilterValues[1].ToString());
        //                                string Operator = strFilterValues[3].ToString();

        //                                if (FieldID == 4)
        //                                {

        //                                    if (Operator.ToLower() == "in")
        //                                    {
        //                                        DataSet ds = ClsList.AreaCode_GetAll(FieldValues, "in");
        //                                        dtTemp = ds.Tables[0];
        //                                        // drCollection = dtTemp.Select("TimezoneName IN (" + strTimezone + ")");



        //                                        //int loopcount = drCollection.Length;
        //                                        int loopcount = dtTemp.Rows.Count;

        //                                        AreaCod = dtTemp.Select("Areacode = '" + dtTrendWestLead.Rows[i][0].ToString() + "'");

        //                                        if (AreaCod.Count() > 0)
        //                                        {

        //                                            //varcount++;
        //                                            condition = 1;

        //                                        }
        //                                        else
        //                                        {
        //                                            condition = 0;
        //                                            break;
        //                                        }
        //                                    }
        //                                    else
        //                                    {
        //                                        DataSet ds = ClsList.AreaCode_GetAll(FieldValues, "not in");
        //                                        dtTemp = ds.Tables[0];
        //                                        // drCollection = dtTemp.Select("TimezoneName IN (" + strTimezone + ")");

        //                                        //int loopcount = drCollection.Length;
        //                                        int loopcount = dtTemp.Rows.Count;

        //                                        AreaCod = dtTemp.Select("Areacode = '" + dtTrendWestLead.Rows[i][0].ToString() + "'");

        //                                        if (AreaCod.Count() > 0)
        //                                        {

        //                                            //varcount++;
        //                                            condition = 1;

        //                                        }
        //                                        else
        //                                        {
        //                                            condition = 0;
        //                                            break;
        //                                        }


        //                                    }
        //                                }
        //                                else if (FieldID == 6)
        //                                {
        //                                    if (Operator.ToLower() == "in")
        //                                    {
        //                                        if (FieldValues.Contains(dtTrendWestLead.Rows[i][0].ToString()) == true)
        //                                        {
        //                                            DataSet ds = ClsList.AreaCode_GetID(dtTrendWestLead.Rows[i][0].ToString());
        //                                            dtTemp = ds.Tables[0];
        //                                            AreaCod = dtTemp.Select("Areacode = '" + dtTrendWestLead.Rows[i][0].ToString() + "'");

        //                                            if (AreaCod.Count() > 0)
        //                                            {

        //                                                //varcount++;
        //                                                condition = 1;

        //                                            }
        //                                            else
        //                                            {
        //                                                condition = 0;
        //                                            }


        //                                        }
        //                                        else
        //                                        {
        //                                            condition = 0;
        //                                            break;
        //                                        }
        //                                    }
        //                                    else
        //                                    {
        //                                        if (FieldValues.Contains(dtTrendWestLead.Rows[i][0].ToString()) == false)
        //                                        {
        //                                            DataSet ds = ClsList.AreaCode_GetID(dtTrendWestLead.Rows[i][0].ToString());
        //                                            dtTemp = ds.Tables[0];
        //                                            AreaCod = dtTemp.Select("Areacode = '" + dtTrendWestLead.Rows[i][0].ToString() + "'");

        //                                            if (AreaCod.Count() > 0)
        //                                            {

        //                                                //varcount++;
        //                                                condition = 1;

        //                                            }
        //                                            else
        //                                            {
        //                                                condition = 0;
        //                                            }

        //                                        }
        //                                        else
        //                                        {
        //                                            condition = 0;
        //                                            break;
        //                                        }

        //                                    }

        //                                }
        //                                else if (FieldID == 2)
        //                                {
        //                                    if (Operator.ToLower() == "in")
        //                                    {
        //                                        if (FieldValues.Contains(dtTrendWestLead.Rows[i][StateCol].ToString()) == true)
        //                                        {
        //                                            DataSet ds = ClsList.AreaCode_GetID(dtTrendWestLead.Rows[i][0].ToString());
        //                                            dtTemp = ds.Tables[0];
        //                                            AreaCod = dtTemp.Select("Areacode = '" + dtTrendWestLead.Rows[i][0].ToString() + "'");
        //                                            if (AreaCod.Count() > 0)
        //                                            {

        //                                                //varcount++;
        //                                                condition = 1;

        //                                            }
        //                                            else
        //                                            {
        //                                                condition = 0;
        //                                            }


        //                                        }
        //                                        else
        //                                        {
        //                                            condition = 0;
        //                                            break;
        //                                        }
        //                                    }
        //                                    else
        //                                    {
        //                                        if (FieldValues.Contains(dtTrendWestLead.Rows[i][StateCol].ToString()) == false)
        //                                        {
        //                                            DataSet ds = ClsList.AreaCode_GetID(dtTrendWestLead.Rows[i][0].ToString());
        //                                            dtTemp = ds.Tables[0];
        //                                            AreaCod = dtTemp.Select("Areacode = '" + dtTrendWestLead.Rows[i][0].ToString() + "'");
        //                                            if (AreaCod.Count() > 0)
        //                                            {

        //                                                //varcount++;
        //                                                condition = 1;

        //                                            }
        //                                            else
        //                                            {
        //                                                condition = 0;
        //                                            }


        //                                        }
        //                                        else
        //                                        {
        //                                            condition = 0;
        //                                            break;
        //                                        }

        //                                    }

        //                                }
        //                                else
        //                                {
        //                                    DataRow[] drCustColum = dsLeadFields.Tables[0].Select("CustomFieldID=" + FieldID);
        //                                    int CustCol = int.Parse(drCustColum[0][0].ToString());
        //                                    if (Operator.ToLower() == "in")
        //                                    {
        //                                        if (FieldValues.Contains(dtTrendWestLead.Rows[i][CustCol].ToString()) == true)
        //                                        {
        //                                            DataSet ds = ClsList.AreaCode_GetID(dtTrendWestLead.Rows[i][0].ToString());
        //                                            dtTemp = ds.Tables[0];
        //                                            AreaCod = dtTemp.Select("Areacode = '" + dtTrendWestLead.Rows[i][0].ToString() + "'");
        //                                            if (AreaCod.Count() > 0)
        //                                            {

        //                                                //varcount++;
        //                                                condition = 1;

        //                                            }
        //                                            else
        //                                            {
        //                                                condition = 0;
        //                                            }


        //                                        }
        //                                        else
        //                                        {
        //                                            condition = 0;
        //                                            break;
        //                                        }

        //                                    }
        //                                    else
        //                                    {
        //                                        if (FieldValues.Contains(dtTrendWestLead.Rows[i][CustCol].ToString()) == false)
        //                                        {
        //                                            DataSet ds = ClsList.AreaCode_GetID(dtTrendWestLead.Rows[i][0].ToString());
        //                                            dtTemp = ds.Tables[0];
        //                                            AreaCod = dtTemp.Select("Areacode = '" + dtTrendWestLead.Rows[i][0].ToString() + "'");
        //                                            if (AreaCod.Count() > 0)
        //                                            {

        //                                                //varcount++;
        //                                                condition = 1;

        //                                            }
        //                                            else
        //                                            {
        //                                                condition = 0;
        //                                            }


        //                                        }
        //                                        else
        //                                        {
        //                                            condition = 0;
        //                                            break;
        //                                        }

        //                                    }

        //                                }


        //                            }
        //                            if (condition == 1)
        //                            {
        //                                DataRow[] t1;
        //                                DataSet ds = ClsList.TimeZone_GetAll();
        //                                dtTimezone = ds.Tables[0];
        //                                t1 = dtTimezone.Select("TimezoneName = '" + dtTemp.Rows[0][1].ToString() + "'");

        //                                Int64 gotIt = Int64.Parse(t1[0][1].ToString());

        //                                if (drStateCountry.Length > 0)
        //                                {
        //                                    drLocation = dtLocation.NewRow();
        //                                    drLocation["ID"] = MaxLocationID;
        //                                    drLocation["TimeZoneID"] = gotIt;
        //                                    drLocation["CountryID"] = Convert.ToInt64(drStateCountry[0][1].ToString());
        //                                    drLocation["StateID"] = Convert.ToInt64(drStateCountry[0][0].ToString());
        //                                    drLocation["AreaCodeID"] = Convert.ToInt64(AreaCod[0].ItemArray[0].ToString());
        //                                    drLocation["ZipCodeID"] = Convert.ToInt64(drStateCountry[0][2].ToString());
        //                                    dtLocation.Rows.Add(drLocation);
        //                                }
        //                                else
        //                                {
        //                                    drLocation = dtLocation.NewRow();
        //                                    drLocation["ID"] = MaxLocationID;
        //                                    drLocation["TimeZoneID"] = gotIt;
        //                                    drLocation["CountryID"] = 0;
        //                                    drLocation["StateID"] = 0;
        //                                    drLocation["AreaCodeID"] = Convert.ToInt64(dtTemp.Rows[0][0].ToString());
        //                                    drLocation["ZipCodeID"] = 0;
        //                                    dtLocation.Rows.Add(drLocation);
        //                                }
        //                                varcount++;
        //                            }
        //                            if (varcount == 0)
        //                            {
        //                                //MessageBox.Show(phonenum.ToString().Substring(0,3));
        //                                dtTrendWestLead.Rows.RemoveAt(i);
        //                                if (FieldID >= 1)
        //                                {
        //                                    NoAreacode++;
        //                                }
        //                            }
        //                            else
        //                            {
        //                                //SqlConnection conn = new SqlConnection("Data Source=192.168.1.186\\SQLExpress;Initial Catalog=VMukti;User ID=sa;PassWord=mahavir");
        //                                SqlConnection conn1 = new SqlConnection(VMuktiAPI.VMuktiInfo.MainConnectionString);
        //                                SqlCommand cmd = new SqlCommand("select getdate()", conn1);
        //                                conn1.Open();
        //                                SqlDataReader dr = cmd.ExecuteReader();
        //                                dr.Read();
        //                                dtLead.Rows.Add(count, phonenum, str[0], Convert.ToDateTime(dr[0].ToString()), VMuktiAPI.VMuktiInfo.CurrentPeer.ID, null, null, false, Convert.ToDateTime(dr.GetValue(0)), VMuktiAPI.VMuktiInfo.CurrentPeer.ID, false, 0, int.Parse(((ListBoxItem)lstLists.SelectedItem).Tag.ToString()), MaxLocationID, 0, "Fresh", false);

        //                                for (int cnt = 1; cnt <= dsLeadFields.Tables[0].Rows.Count; cnt++)
        //                                {
        //                                    Int64 tempCustomFieldID = Convert.ToInt64(dsLeadFields.Tables[0].Rows[cnt - 1][1].ToString());
        //                                    if (tempCustomFieldID > 3)
        //                                    {
        //                                        dtLeadDetail.Rows.Add(MaxLeadDetail, count, tempCustomFieldID, dtTrendWestLead.Rows[i][cnt].ToString());
        //                                        MaxLeadDetail++;
        //                                    }

        //                                }
        //                                //dtLead.Rows.Add(count, phonenum, str[0], 1, null, null, false, 1, false, 0, int.Parse(((ListBoxItem)lstLists.SelectedItem).Tag.ToString()), MaxLocationID, 0, "Fresh", false);
        //                                globalCounter++;
        //                                count++;
        //                                MaxLocationID++;
        //                                conn1.Close();
        //                            }
        //                        jump1: ;

        //                        }


        //                        catch (Exception exp)
        //                        {
        //                            MessageBox.Show(exp.Message.ToString());

        //                        }

        //                        //if (proimport.Maximum == proimport.Value)
        //                        //{
        //                        //    //proimport.Visibility = Visibility.Hidden;
        //                        //    counter += 1;
        //                        //    proimport.Value = 0;
        //                        //    //proimport.Visibility = Visibility.Visible;
        //                        //}
        //                        //else
        //                        //{
        //                        //    //proimport.Visibility = Visibility.Hidden;
        //                        //    counter += 1;
        //                        //    //proimport.Visibility = Visibility.Visible;
        //                        //}
        //                        //if (counter == 10)
        //                        //{
        //                        //    //bool methDone = false;
        //                        //    //methDone = callMeth();]
        //                        //    proimport.Value += 2;
        //                        //    counter = 0;
        //                        //}

        //                    }
        //                   //progress.Kill();
        //                }

        //                else
        //                {
        //                    string[] strformat = ((ListBoxItem)cmbFormat.SelectedItem).Tag.ToString().Split(',');

        //                    ClsTextFileInsert insText = new ClsTextFileInsert(fileName);
        //                    int[] TotalFields = InsertTextFile();//insText.TotalCount(Convert.ToInt64(strformat[0]), ListID, strTimezone, filterType, TotalTimeZone,CountryCode);

        //                    if (TotalFields.Length == 1)
        //                    {
        //                        MessageBox.Show("Phone Number Column is missing");
        //                    }
        //                    else
        //                    {
        //                        MessageBox.Show("Records Inserted");
        //                        lblNoOfProcessed.Content = TotalFields[0].ToString() + " Records Processed.";
        //                        lblBadLeads.Content = TotalFields[1].ToString() + " Bad Records Found.";
        //                        lblRecordsInserted.Content = TotalFields[2].ToString() + " Records Inserted.";
        //                        lblDuplicateLeads.Content = TotalFields[3].ToString() + " Records Repeated.";
        //                        lblNoAreaCode.Content = TotalFields[4].ToString() + " Records Are Having Wrong AreaCode";
        //                        Array.Clear(TotalFields, 0, TotalFields.Length);
        //                        badNumbers = 0;
        //                        noOfRowsProcessed = 0;
        //                        repeatNumbers = 0;
        //                        NoAreacode = 0;
        //                    }

        //                }


        //                dtLead.DefaultView.Sort = "ID";

        //                /*  SqlBulkCopy sqlBulk = new SqlBulkCopy(VMuktiAPI.VMuktiInfo.MainConnectionString);
        //                //SqlBulkCopy sqlBulk = new SqlBulkCopy("Data Source=192.168.1.186\\SQLExpress;Initial Catalog=VMukti;User ID=sa;PassWord=mahavir", SqlBulkCopyOptions.FireTriggers);
        //                sqlBulk.DestinationTableName = "TrendWestLead";
        //                sqlBulk.ColumnMappings.Add("ID", "ID");
        //                sqlBulk.ColumnMappings.Add("LeadID", "LeadID");
        //                sqlBulk.ColumnMappings.Add("lead", "VendorLeadID");
        //                sqlBulk.ColumnMappings.Add("First Name", "FirstName");
        //                sqlBulk.ColumnMappings.Add("Last Name", "LastName");
        //                sqlBulk.ColumnMappings.Add("Address", "Address");
        //                sqlBulk.ColumnMappings.Add("City", "City");
        //                sqlBulk.ColumnMappings.Add("State", "State");
        //                sqlBulk.ColumnMappings.Add("Zip", "Zip");
        //                sqlBulk.ColumnMappings.Add("Control", "Control");
        //                sqlBulk.ColumnMappings.Add("Event", "Event");
        //                sqlBulk.ColumnMappings.Add("Work1", "[Work]");
        //                sqlBulk.ColumnMappings.Add("Program Code", "ProgramCode");
        //                sqlBulk.ColumnMappings.Add("Site", "Site");
        //                sqlBulk.ColumnMappings.Add("Email", "Email");
        //                sqlBulk.BulkCopyTimeout = 1000;
        //                sqlBulk.WriteToServer(dtTrendWestLead);
        //                sqlBulk.Close();*/

        //                SqlBulkCopy sqlBulkLocation = new SqlBulkCopy(VMuktiAPI.VMuktiInfo.MainConnectionString);
        //                //SqlBulkCopy sqlBulkLocation = new SqlBulkCopy("Data Source=192.168.1.186\\SQLExpress;Initial Catalog=VMukti;User ID=sa;PassWord=mahavir",SqlBulkCopyOptions.FireTriggers);
        //                sqlBulkLocation.DestinationTableName = "Location";
        //                sqlBulkLocation.BulkCopyTimeout = 1000;
        //                sqlBulkLocation.WriteToServer(dtLocation);
        //                sqlBulkLocation.Close();

        //                SqlBulkCopy sqlBulkLeadDetail = new SqlBulkCopy(VMuktiAPI.VMuktiInfo.MainConnectionString);
        //                //SqlBulkCopy sqlBulkLocation = new SqlBulkCopy("Data Source=192.168.1.186\\SQLExpress;Initial Catalog=VMukti;User ID=sa;PassWord=mahavir",SqlBulkCopyOptions.FireTriggers);
        //                sqlBulkLeadDetail.DestinationTableName = "LeadDetail";
        //                sqlBulkLeadDetail.BulkCopyTimeout = 1000;
        //                sqlBulkLeadDetail.WriteToServer(dtLeadDetail);
        //                sqlBulkLeadDetail.Close();


        //                lblNoOfProcessed.Content = noOfRowsProcessed.ToString() + " Records Processed.";
        //                lblBadLeads.Content = badNumbers.ToString() + " Bad Records Found.";
        //                lblRecordsInserted.Content = dtLead.Rows.Count.ToString() + " Records Inserted.";
        //                lblDuplicateLeads.Content = repeatNumbers.ToString() + " Records Repeated.";
        //                lblNoAreaCode.Content = NoAreacode.ToString() + " Records Are Having Wrong AreaCode";
        //                badNumbers = 0;
        //                noOfRowsProcessed = 0;
        //                repeatNumbers = 0;
        //                NoAreacode = 0;

        //            }

        //            SqlBulkCopy sqlBulk1 = new SqlBulkCopy(VMuktiAPI.VMuktiInfo.MainConnectionString);
        //            // SqlBulkCopy sqlBulk1 = new SqlBulkCopy("Data Source=192.168.1.186\\SQLExpress;Initial Catalog=VMukti;User ID=sa;PassWord=mahavir",SqlBulkCopyOptions.FireTriggers);
        //            sqlBulk1.DestinationTableName = "Leads";
        //            sqlBulk1.ColumnMappings.Add("ID", "ID");
        //            sqlBulk1.ColumnMappings.Add("PhoneNo", "PhoneNo");
        //            sqlBulk1.ColumnMappings.Add("LeadFormatID", "LeadFormatID");
        //            sqlBulk1.ColumnMappings.Add("CreatedDate", "CreatedDate");
        //            sqlBulk1.ColumnMappings.Add("CreatedBy", "CreatedBy");
        //            sqlBulk1.ColumnMappings.Add("DeletedDate", "DeletedDate");
        //            sqlBulk1.ColumnMappings.Add("DeletedBy", "DeletedBy");
        //            sqlBulk1.ColumnMappings.Add("IsDeleted", "IsDeleted");
        //            sqlBulk1.ColumnMappings.Add("ModifiedDate", "ModifiedDate");
        //            sqlBulk1.ColumnMappings.Add("ModifiedBy", "ModifiedBy");
        //            sqlBulk1.ColumnMappings.Add("DNCFlag", "DNCFlag");
        //            sqlBulk1.ColumnMappings.Add("DNCBy", "DNCBy");
        //            sqlBulk1.ColumnMappings.Add("ListID", "ListID");
        //            sqlBulk1.ColumnMappings.Add("LocationID", "LocationID");
        //            sqlBulk1.ColumnMappings.Add("RecycleCount", "RecycleCount");
        //            sqlBulk1.ColumnMappings.Add("Status", "Status");
        //            sqlBulk1.ColumnMappings.Add("IsGlobalDNC", "IsGlobalDNC");
        //            sqlBulk1.BulkCopyTimeout = 1000;
        //            sqlBulk1.WriteToServer(dtLead);
        //            sqlBulk1.Close();

        //            if (cmbListType.SelectionBoxItem.ToString().ToLower() == "dnc list")
        //            {
        //                lblNoOfProcessed.Content = noOfRowsProcessed.ToString() + " Records Processed.";
        //                lblBadLeads.Content = badNumbers.ToString() + " Bad Records Found.";
        //                lblRecordsInserted.Content = dtLead.Rows.Count.ToString() + " Records Inserted.";
        //                lblDuplicateLeads.Content = repeatNumbers.ToString() + " Records Repeated.";
        //                badNumbers = 0;
        //                noOfRowsProcessed = 0;
        //                repeatNumbers = 0;
        //            }
        //        jump2: ;
        //        }//else

               
        //    }//end try
        //    catch (Exception ex)
        //    {
        //        //imgwait.Source = "";
        //        provalue = 0;
        //        //proimport.Value = 0;
               
        //        MessageBox.Show("File is not in correct formate");
        //        MessageBox.Show(ex.ToString());
        //        ex.Data.Add("My Key", "VMukti--:--VmuktiModules--:--Call Center--:--ImportLeads--:--ImportLeads.DataAccess--:--ClsImportLeads.xaml.cs--:--btnSave_Click()--");
        //        ClsException.LogError(ex);
        //        ClsException.WriteToErrorLogFile(ex);
        //    }
        //}

        //bool callMeth()
        //{
        //    proimport.Value += 2;
        //    return true;
        //}

       //private int[] InsertTextFile()
       //{
       //    try
       //    {
       //        DataSet dsCustomeFieldDetails = new DataSet();
       //        int varcount = 0;
       //        DataSet StateCountry = ClsList.StateCountry_GetAll();

       //        DataTable dtTemp = new DataTable();
       //        //DataRow[] drCollection = null;

       //        //if (FilterType == "TIMEZONE" || FilterType == "SITE")
       //        //{
       //        //    DataSet ds = ClsList.AreaCode_GetAll(SelectedTimeZone,"in");
       //        //    dtTemp = ds.Tables[0];
       //        //    //drCollection = dtTemp.Select("TimezoneName IN (" + strTimezone + ")");

       //        //}


       //        ClsImportLeadsDataService clsReterive = new ClsImportLeadsDataService();

       //        dsCustomeFieldDetails = clsReterive.ReteriveCustomeFieldDetails(Convert.ToInt64(str[0]));

       //        // dsCustomeFieldDetails = ClsList.GetLeadFields(Convert.ToInt64(str[0]));

       //        DataRow[] drPhoneNumber = dsCustomeFieldDetails.Tables[0].Select("CustomFieldID = 1");
       //        int phoneStartpos = Convert.ToInt32(drPhoneNumber[0]["StartPosition"]);
       //        int phonelen = Convert.ToInt32(drPhoneNumber[0]["Length"]);

       //        DataRow[] drStateID = dsCustomeFieldDetails.Tables[0].Select("CustomFieldID = 2");
       //        int StateStartpos = Convert.ToInt32(drStateID[0]["StartPosition"]);
       //        int Statelen = Convert.ToInt32(drStateID[0]["Length"]);

       //        DataRow[] drZipID = dsCustomeFieldDetails.Tables[0].Select("CustomFieldID = 3");
       //        int ZipStartpos = Convert.ToInt32(drZipID[0]["StartPosition"]);
       //        int Ziplen = Convert.ToInt32(drZipID[0]["Length"]);
       //        int[] ResultArray;


       //        if (drPhoneNumber.Count() > 0)
       //        {

       //            FileStream fs = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.None, 2 << 18, FileOptions.SequentialScan);
       //            DataTable dtTimezone = new DataTable();

       //            DataRow[] drStateCountry = null;

       //            DataSet dsTimeZone = ClsList.TimeZone_GetAll();
       //            dtTimezone = dsTimeZone.Tables[0];

       //            Int64 LeadMaxcount = ClsList.GetMaxIDLeads();
       //            Int64 MaxLeadDetail = ClsList.GetMaxLeadDetailID();
       //            Int64 MaxLocationID = ClsList.GetMaxLocationID();

       //            createDataTable();


       //            int TotalColm = dsCustomeFieldDetails.Tables[0].Rows.Count;

       //            StreamReader sr = new StreamReader(fs);





       //            // int cntFile = sr.ReadToEnd().Split('\n').Count();

       //            // string[] FileValueArray = sr.ReadToEnd().Split('\n') ;

       //            //int[] Fields = new int[TotalColm];
       //            /// int[] StartPosition = new int[TotalColm];
       //            // int[] Length = new int[TotalColm];



       //            //   fs.Close();

       //            //    sr.Close();

       //            /*  for (int Colcnt = 0; Colcnt < TotalColm; Colcnt++)
       //              {

       //                  Fields[Colcnt] = Convert.ToInt32(dsCustomeFieldDetails.Tables[0].Rows[Colcnt]["CustomFieldID"]);
       //                  StartPosition[Colcnt] = Convert.ToInt32(dsCustomeFieldDetails.Tables[0].Rows[Colcnt]["StartPosition"]);
       //                  Length[Colcnt] = Convert.ToInt32(dsCustomeFieldDetails.Tables[0].Rows[Colcnt]["Length"]);

       //              }*/



       //            SqlConnection conn = new SqlConnection(VMuktiAPI.VMuktiInfo.MainConnectionString);
       //            SqlCommand cmd = new SqlCommand("select getdate()", conn);
       //            conn.Open();
       //            SqlDataReader dr1;
       //            int Rowcnt = 0;
       //            //string Row = "";
       //            int badNumbers = 0;
       //            // Byte badNumberFlag = 0;
       //            // Byte RepeatFlag =0;
       //            Byte AreaCodeFlag = 0;

       //            int repeatNumbers = 0;
       //            int NoAreaCode = 0;
       //            DataTable dtPhoneNumber = new DataTable();

       //            DataSet ds1 = ClsList.PhoneNumbers_GetAll(ListID);
       //            dtPhoneNumber = ds1.Tables[0];


       //            try
       //            {
       //                while (sr.EndOfStream != true)
       //                //for (Rowcnt = 0; Rowcnt < FileValueArray.Count(); Rowcnt++)
       //                {
       //                    Rowcnt = Rowcnt + 1;
       //                    string Row = sr.ReadLine();
       //                    //FileValueArray.ElementAt(Rowcnt);

       //                    Int64 phoneNumber;
       //                    try
       //                    {
       //                        phoneNumber = Convert.ToInt64(Row.Substring(phoneStartpos - 1, phonelen).Replace("-", "").Replace(" ", ""));
       //                        phoneNumber = Int64.Parse((Math.Pow(10 * CountryCode, (phoneNumber.ToString().Length))).ToString()) + phoneNumber;
       //                    }
       //                    catch (Exception exp)
       //                    {
       //                        badNumbers++;

       //                        //  badNumberFlag = 1;
       //                        continue;
       //                    }

       //                    DataRow[] drRepeat = dtPhoneNumber.Select("PhoneNo = " + phoneNumber);
       //                    if (drRepeat.Length > 0)
       //                    {
       //                        repeatNumbers++;
       //                        //  RepeatFlag = 1;
       //                        continue;
       //                    }
       //                    dtPhoneNumber.Rows.Add(phoneNumber);

       //                    dr1 = cmd.ExecuteReader();
       //                    dr1.Read();
       //                    //if (badNumberFlag == 0 || RepeatFlag == 0)
       //                    //  {



       //                    string ZipCode = Row.Substring(ZipStartpos - 1, Ziplen).Replace(" ", "");
       //                    //Int64 finalzipcode = String.IsNullOrEmpty(ZipCode) ? null : Int64.Parse(ZipCode);
       //                    drStateCountry = StateCountry.Tables[0].Select("zipcode='" + ZipCode.Substring(0, 5) + "'");

       //                    Int64 FieldID = 0;
       //                    int condition = 0;

       //                    string FieldValues = "";
       //                    DataRow drLocation1 = null;
       //                    DataRow[] AreaCod = null;

       //                    try
       //                    {
       //                        for (int cntFilter = 0; cntFilter < lstFilterType.SelectedItems.Count; cntFilter++)
       //                        {
       //                            string[] strFilterValues = ((ListBoxItem)lstFilterType.SelectedItems[cntFilter]).Tag.ToString().Split(',');
       //                            FieldValues = strFilterValues[2].Replace('~', ',');
       //                            FieldID = Convert.ToInt64(strFilterValues[1].ToString());
       //                            string Operator = strFilterValues[3].ToString();

       //                            if (FieldID == 4)
       //                            {

       //                                if (Operator.ToLower() == "in")
       //                                {
       //                                    DataSet ds = ClsList.AreaCode_GetAll(FieldValues, "in");
       //                                    dtTemp = ds.Tables[0];
       //                                    // drCollection = dtTemp.Select("TimezoneName IN (" + strTimezone + ")");



       //                                    //int loopcount = drCollection.Length;
       //                                    int loopcount = dtTemp.Rows.Count;

       //                                    AreaCod = dtTemp.Select("Areacode = '" + phoneNumber.ToString().Substring(1, 3) + "'");

       //                                    if (AreaCod.Count() > 0)
       //                                    {

       //                                        //varcount++;
       //                                        condition = 1;

       //                                    }
       //                                    else
       //                                    {
       //                                        condition = 0;
       //                                        break;
       //                                    }
       //                                }
       //                                else
       //                                {
       //                                    DataSet ds = ClsList.AreaCode_GetAll(FieldValues, "not in");
       //                                    dtTemp = ds.Tables[0];
       //                                    // drCollection = dtTemp.Select("TimezoneName IN (" + strTimezone + ")");



       //                                    //int loopcount = drCollection.Length;
       //                                    int loopcount = dtTemp.Rows.Count;

       //                                    AreaCod = dtTemp.Select("Areacode = '" + phoneNumber.ToString().Substring(1, 3) + "'");

       //                                    if (AreaCod.Count() > 0)
       //                                    {

       //                                        //varcount++;
       //                                        condition = 1;

       //                                    }
       //                                    else
       //                                    {
       //                                        condition = 0;
       //                                        break;
       //                                    }


       //                                }
       //                            }
       //                            else if (FieldID == 6)
       //                            {
       //                                if (Operator.ToLower() == "in")
       //                                {
       //                                    if (FieldValues.Contains(phoneNumber.ToString().Substring(1, 3)) == true)
       //                                    {
       //                                        DataSet ds = ClsList.AreaCode_GetID(phoneNumber.ToString().Substring(1, 3));
       //                                        dtTemp = ds.Tables[0];
       //                                        AreaCod = dtTemp.Select("Areacode = '" + phoneNumber.ToString().Substring(1, 3) + "'");

       //                                        if (AreaCod.Count() > 0)
       //                                        {

       //                                            //varcount++;
       //                                            condition = 1;

       //                                        }
       //                                        else
       //                                        {
       //                                            condition = 0;
       //                                        }


       //                                    }
       //                                    else
       //                                    {
       //                                        condition = 0;
       //                                        break;
       //                                    }
       //                                }
       //                                else
       //                                {
       //                                    if (FieldValues.Contains(phoneNumber.ToString().Substring(1, 3)) == false)
       //                                    {
       //                                        DataSet ds = ClsList.AreaCode_GetID(phoneNumber.ToString().Substring(1, 3));
       //                                        dtTemp = ds.Tables[0];
       //                                        AreaCod = dtTemp.Select("Areacode = '" + phoneNumber.ToString().Substring(1, 3) + "'");

       //                                        if (AreaCod.Count() > 0)
       //                                        {

       //                                            //varcount++;
       //                                            condition = 1;

       //                                        }
       //                                        else
       //                                        {
       //                                            condition = 0;
       //                                        }


       //                                    }
       //                                    else
       //                                    {
       //                                        condition = 0;
       //                                        break;
       //                                    }

       //                                }

       //                            }
       //                            else if (FieldID == 2)
       //                            {

       //                                if (Operator.ToLower() == "in")
       //                                {
       //                                    if (FieldValues.Contains(Row.Substring(StateStartpos, Statelen)) == true)
       //                                    {
       //                                        DataSet ds = ClsList.AreaCode_GetID(phoneNumber.ToString().Substring(1, 3));
       //                                        dtTemp = ds.Tables[0];
       //                                        AreaCod = dtTemp.Select("Areacode = '" + phoneNumber.ToString().Substring(1, 3) + "'");
       //                                        if (AreaCod.Count() > 0)
       //                                        {

       //                                            condition = 1;

       //                                        }
       //                                        else
       //                                        {
       //                                            condition = 0;
       //                                        }


       //                                    }
       //                                    else
       //                                    {
       //                                        condition = 0;
       //                                        break;
       //                                    }
       //                                }
       //                                else
       //                                {
       //                                    if (FieldValues.Contains(Row.Substring(StateStartpos, Statelen)) == false)
       //                                    {
       //                                        DataSet ds = ClsList.AreaCode_GetID(phoneNumber.ToString().Substring(1, 3));
       //                                        dtTemp = ds.Tables[0];
       //                                        AreaCod = dtTemp.Select("Areacode = '" + phoneNumber.ToString().Substring(1, 3) + "'");
       //                                        if (AreaCod.Count() > 0)
       //                                        {

       //                                            //varcount++;
       //                                            condition = 1;

       //                                        }
       //                                        else
       //                                        {
       //                                            condition = 0;
       //                                        }


       //                                    }
       //                                    else
       //                                    {
       //                                        condition = 0;
       //                                        break;
       //                                    }

       //                                }

       //                            }
       //                            else
       //                            {
       //                                DataRow[] drCustColum = dsCustomeFieldDetails.Tables[0].Select("CustomFieldID=" + FieldID);
       //                                int CustStartpos = Convert.ToInt32(drStateID[0]["StartPosition"]);
       //                                int Custlen = Convert.ToInt32(drStateID[0]["Length"]);

       //                                if (Operator.ToLower() == "in")
       //                                {
       //                                    if (FieldValues.Contains(Row.Substring(CustStartpos, Custlen)) == true)
       //                                    {
       //                                        DataSet ds = ClsList.AreaCode_GetID(phoneNumber.ToString().Substring(1, 3));
       //                                        dtTemp = ds.Tables[0];
       //                                        AreaCod = dtTemp.Select("Areacode = '" + phoneNumber.ToString().Substring(1, 3) + "'");
       //                                        if (AreaCod.Count() > 0)
       //                                        {

       //                                            //varcount++;
       //                                            condition = 1;

       //                                        }
       //                                        else
       //                                        {
       //                                            condition = 0;
       //                                        }


       //                                    }
       //                                    else
       //                                    {
       //                                        condition = 0;
       //                                        break;
       //                                    }

       //                                }
       //                                else
       //                                {
       //                                    if (FieldValues.Contains(Row.Substring(CustStartpos, Custlen)) == false)
       //                                    {
       //                                        DataSet ds = ClsList.AreaCode_GetID(phoneNumber.ToString().Substring(1, 3));
       //                                        dtTemp = ds.Tables[0];
       //                                        AreaCod = dtTemp.Select("Areacode = '" + phoneNumber.ToString().Substring(1, 3) + "'");
       //                                        if (AreaCod.Count() > 0)
       //                                        {

       //                                            //varcount++;
       //                                            condition = 1;

       //                                        }
       //                                        else
       //                                        {
       //                                            condition = 0;
       //                                        }


       //                                    }
       //                                    else
       //                                    {
       //                                        condition = 0;
       //                                        break;
       //                                    }

       //                                }

       //                            }
       //                        }
       //                    }
       //                    catch (Exception exp)
       //                    {
       //                        MessageBox.Show(exp.Message.ToString());
       //                    }

       //                    if (condition == 1)
       //                    {

       //                        DataRow[] t1;
       //                        DataSet ds = ClsList.TimeZone_GetAll();
       //                        dtTimezone = ds.Tables[0];
       //                        t1 = dtTimezone.Select("TimezoneName = '" + dtTemp.Rows[0][1].ToString() + "'");

       //                        Int64 gotIt = Int64.Parse(t1[0][1].ToString());
       //                        if (drStateCountry.Length > 0)
       //                        {
       //                            drLocation1 = dtLocation.NewRow();
       //                            drLocation1["ID"] = MaxLocationID;
       //                            drLocation1["TimeZoneID"] = gotIt;
       //                            drLocation1["CountryID"] = Convert.ToInt64(drStateCountry[0][1].ToString());
       //                            drLocation1["StateID"] = Convert.ToInt64(drStateCountry[0][0].ToString());
       //                            drLocation1["AreaCodeID"] = Convert.ToInt64(AreaCod[0].ItemArray[0].ToString());
       //                            drLocation1["ZipCodeID"] = Convert.ToInt64(drStateCountry[0][2].ToString());
       //                            dtLocation.Rows.Add(drLocation1);
       //                        }
       //                        else
       //                        {
       //                            drLocation1 = dtLocation.NewRow();
       //                            drLocation1["ID"] = MaxLocationID;
       //                            drLocation1["TimeZoneID"] = gotIt;
       //                            drLocation1["CountryID"] = 0;
       //                            drLocation1["StateID"] = 0;
       //                            drLocation1["AreaCodeID"] = Convert.ToInt64(dtTemp.Rows[0][0].ToString());
       //                            drLocation1["ZipCodeID"] = 0;
       //                            dtLocation.Rows.Add(drLocation1);

       //                        }
       //                        dtLead.Rows.Add(LeadMaxcount, phoneNumber, Convert.ToInt64(str[0]), Convert.ToDateTime(dr1[0].ToString()), VMuktiAPI.VMuktiInfo.CurrentPeer.ID, null, null, false, Convert.ToDateTime(dr1.GetValue(0)), VMuktiAPI.VMuktiInfo.CurrentPeer.ID, false, 0, ListID, MaxLocationID, 0, "Fresh", false);
       //                        int Colcnt = 0;


       //                        for (Colcnt = 0; Colcnt < TotalColm; Colcnt++)
       //                        {
       //                            Int64 fieldID = Convert.ToInt32(dsCustomeFieldDetails.Tables[0].Rows[Colcnt]["CustomFieldID"]); ;
       //                            int StartPositionTmp = Convert.ToInt32(dsCustomeFieldDetails.Tables[0].Rows[Colcnt]["StartPosition"]);
       //                            int LengthTmp = Convert.ToInt32(dsCustomeFieldDetails.Tables[0].Rows[Colcnt]["Length"]);

       //                            // string Row = FileValueArray.ElementAt(Rowcnt);
       //                            string Value = Row.Substring(StartPositionTmp - 1, LengthTmp);

       //                            dtLeadDetail.Rows.Add(MaxLeadDetail, LeadMaxcount, fieldID, Value);
       //                            MaxLeadDetail = MaxLeadDetail + 1;

       //                        }

       //                        MaxLocationID = MaxLocationID + 1;
       //                        LeadMaxcount = LeadMaxcount + 1;
       //                        Row = "";

       //                    }
       //                    else
       //                    {
       //                        NoAreaCode++;
       //                    }


       //                    /* if (varcount == 0)
       //                     {
       //                         //MessageBox.Show(phonenum.ToString().Substring(0,3));
                           
       //                     }
       //                     else
       //                     {
       //                         AreaCodeFlag = 0;
       //                         dtLead.Rows.Add(LeadMaxcount, phoneNumber, Convert.ToInt64(str[0]), Convert.ToDateTime(dr1[0].ToString()), VMuktiAPI.VMuktiInfo.CurrentPeer.ID, null, null, false, Convert.ToDateTime(dr1.GetValue(0)), VMuktiAPI.VMuktiInfo.CurrentPeer.ID, false, 0, ListID, MaxLocationID, 0, "Fresh", false);
       //                         int Colcnt = 0;


       //                         for (Colcnt = 0; Colcnt < TotalColm; Colcnt++)
       //                         {
       //                             Int64 fieldID = Convert.ToInt32(dsCustomeFieldDetails.Tables[0].Rows[Colcnt]["CustomFieldID"]); ;
       //                             int StartPositionTmp = Convert.ToInt32(dsCustomeFieldDetails.Tables[0].Rows[Colcnt]["StartPosition"]);
       //                             int LengthTmp = Convert.ToInt32(dsCustomeFieldDetails.Tables[0].Rows[Colcnt]["Length"]);

       //                             // string Row = FileValueArray.ElementAt(Rowcnt);
       //                             string Value = Row.Substring(StartPositionTmp - 1, LengthTmp);

       //                             dtLeadDetail.Rows.Add(MaxLeadDetail, LeadMaxcount, fieldID, Value);
       //                             MaxLeadDetail = MaxLeadDetail + 1;

       //                         }

       //                         MaxLocationID = MaxLocationID + 1;
       //                         LeadMaxcount = LeadMaxcount + 1;
       //                         Row = "";

       //                     }
       //                     */
       //                    dr1.Close();
       //                    // }
       //                    // else
       //                    // {
       //                    //  badNumberFlag = 0;
       //                    //  RepeatFlag = 0;
       //                    //}
       //                } //While End
       //            }
       //            catch (Exception ex)
       //            {
       //                ex.Data.Add("My Key", "VMukti--:--VmuktiModules--:--Call Center--:--ImportLeads--:--ImportLeads.Business--:--ClsTextFileInsert.cs--:--InsertTextFile()--");
       //                ClsException.LogError(ex);
       //                ClsException.WriteToErrorLogFile(ex);

       //            }
       //            finally
       //            {
       //                sr.Close();
       //                fs.Close();
       //                sr = null;
       //                fs = null;
       //                GC.Collect(0, GCCollectionMode.Forced);

       //                //GC.SuppressFinalize(this);
       //            }



       //            SqlBulkCopy sqlBulkLocation = new SqlBulkCopy(VMuktiAPI.VMuktiInfo.MainConnectionString);
       //            //SqlBulkCopy sqlBulkLocation = new SqlBulkCopy("Data Source=192.168.1.186\\SQLExpress;Initial Catalog=VMukti;User ID=sa;PassWord=mahavir",SqlBulkCopyOptions.FireTriggers);
       //            sqlBulkLocation.DestinationTableName = "Location";
       //            sqlBulkLocation.BulkCopyTimeout = 1000;
       //            sqlBulkLocation.WriteToServer(dtLocation);
       //            sqlBulkLocation.Close();


       //            SqlBulkCopy sqlBulkLead = new SqlBulkCopy(VMuktiAPI.VMuktiInfo.MainConnectionString);
       //            //SqlBulkCopy sqlBulkLocation = new SqlBulkCopy("Data Source=192.168.1.186\\SQLExpress;Initial Catalog=VMukti;User ID=sa;PassWord=mahavir",SqlBulkCopyOptions.FireTriggers);
       //            sqlBulkLead.DestinationTableName = "Leads";
       //            sqlBulkLead.BulkCopyTimeout = 1000;
       //            sqlBulkLead.WriteToServer(dtLead);
       //            sqlBulkLead.Close();


       //            SqlBulkCopy sqlBulkLeadDetail = new SqlBulkCopy(VMuktiAPI.VMuktiInfo.MainConnectionString);
       //            //SqlBulkCopy sqlBulkLocation = new SqlBulkCopy("Data Source=192.168.1.186\\SQLExpress;Initial Catalog=VMukti;User ID=sa;PassWord=mahavir",SqlBulkCopyOptions.FireTriggers);
       //            sqlBulkLeadDetail.DestinationTableName = "LeadDetail";
       //            sqlBulkLeadDetail.BulkCopyTimeout = 1000;
       //            sqlBulkLeadDetail.WriteToServer(dtLeadDetail);
       //            sqlBulkLeadDetail.Close();


       //            ResultArray = new int[] { Rowcnt, badNumbers, dtLead.Rows.Count, repeatNumbers, NoAreaCode };

       //            dtLocation = null;
       //            dtLead = null;
       //            dtLeadDetail = null;

       //            return ResultArray;
       //        }
       //        else
       //        {
       //            ResultArray = new int[] { 0 };
       //            return ResultArray;
       //        }
       //    }
       //    catch (Exception ex)
       //    {
       //        ex.Data.Add("My Key", "VMukti--:--VmuktiModules--:--Call Center--:--ImportLeads--:--ImportLeads.DataAccess--:--ClsImportLeads.xaml.cs--:--ExcelConnection()--");
       //        ClsException.LogError(ex);
       //        ClsException.WriteToErrorLogFile(ex);
       //    }
       
       
       
       
       
       //}
        private string ExcelConnection()
        {
            try
            {
                return
                    @"Provider=Microsoft.Jet.OLEDB.4.0;" +
                    @"Data Source=" + fileName + ";" +
                    @"Extended Properties=" + Convert.ToChar(34).ToString() +
                    @"Excel 8.0;" + Convert.ToChar(34).ToString();

                    //string path = "c:\testInput\Data\Combisep\2004\Processed\OM0001122-GOP";
                    //string fileToQuery = "OM0001122-GOP_Combisep_outFile.csv"
                    //string sql = "SELECT * FROM " + fileToQuery;
                    //System.Data.OleDb.OleDbConnection myConnection = new System.Data.OleDb.OleDbConnection( "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + path + ";" + "Extended Properties='text;FMT=Delimited(,);HDR=YES'");
                    //return @"Provider=Microsoft.Jet.OLEDB.4.0;" +
                    //@"Data Source=" + fileName + ";" +
                    //@"Extended Properties=""text;HDR=NO;FMT=Delimited""";
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "ExcelConnection()", "CtlImportLeads.xaml.cs");
                return null;
            }

        }

        public void createDataTable()
        {
            try
            {
                //dtLeads = new DataTable();
                dtTrendWestLead = new DataTable();
                //dtTrendWestLead.Columns.Add("ID");
                //dtTrendWestLead.Columns[0].DataType = typeof(Int64);
                //dtTrendWestLead.Columns.Add("LeadID");
                //dtTrendWestLead.Columns[1].DataType = typeof(Int64);
                //dtTrendWestLead.Columns.Add("lead");
                //dtTrendWestLead.Columns[2].DataType = typeof(Int64);
                //dtTrendWestLead.Columns.Add("First Name");
                //dtTrendWestLead.Columns.Add("Last Name");
                //dtTrendWestLead.Columns.Add("Address");
                //dtTrendWestLead.Columns.Add("City");
                //dtTrendWestLead.Columns.Add("State");
                //dtTrendWestLead.Columns.Add("Zip");
                //dtTrendWestLead.Columns.Add("Control");
                //dtTrendWestLead.Columns.Add("Event");
                //dtTrendWestLead.Columns.Add("Work1");
                //dtTrendWestLead.Columns[11].DataType = typeof(Int64);
                //dtTrendWestLead.Columns.Add("Program Code");
                //dtTrendWestLead.Columns[12].DataType = typeof(Int64);
                //dtTrendWestLead.Columns.Add("Site");
                //dtTrendWestLead.Columns.Add("Email");
                //dtTrendWestLead.Columns.Add("AreaCod");

                dtLeadDetail = new DataTable();
                dtLeadDetail.Columns.Add("ID");
                dtLeadDetail.Columns[0].DataType = typeof(Int64);
                dtLeadDetail.Columns.Add("LeadID");
                dtLeadDetail.Columns[1].DataType = typeof(Int64);
                dtLeadDetail.Columns.Add("LeadFieldID");
                dtLeadDetail.Columns[2].DataType = typeof(Int64);
                dtLeadDetail.Columns.Add("PropertyValue");
                

         
                dtLead = new DataTable();
                dtLead.Columns.Add("ID");
                dtLead.Columns.Add("PhoneNo");
                dtLead.Columns.Add("LeadFormatID");
                dtLead.Columns.Add("CreatedDate");
                dtLead.Columns.Add("CreatedBy");
                dtLead.Columns.Add("DeletedDate");
                dtLead.Columns.Add("DeletedBy");
                dtLead.Columns.Add("IsDeleted");
                dtLead.Columns.Add("ModifiedDate");
                dtLead.Columns.Add("ModifiedBy");
                dtLead.Columns.Add("DNCFlag");
                dtLead.Columns.Add("DNCBy");
                dtLead.Columns.Add("ListID");
                dtLead.Columns.Add("LocationID");
                dtLead.Columns.Add("RecycleCount");
                dtLead.Columns.Add("Status");
                dtLead.Columns.Add("IsGlobalDNC");

                dtLocation = new DataTable();
                dtLocation.Columns.Add("ID");
                dtLocation.Columns[0].DataType = typeof(Int64);
                dtLocation.Columns.Add("TimeZoneID");
                dtLocation.Columns[1].DataType = typeof(Int64);
                dtLocation.Columns.Add("CountryID");
                dtLocation.Columns[2].DataType = typeof(Int64);
                dtLocation.Columns.Add("StateID");
                dtLocation.Columns[3].DataType = typeof(Int64);
                dtLocation.Columns.Add("AreaCodeID");
                dtLocation.Columns[4].DataType = typeof(Int64);
                dtLocation.Columns.Add("ZipCodeID");
                dtLocation.Columns[5].DataType = typeof(Int64);


                dtTrendWestLead.Rows.Clear();
                dtLead.Rows.Clear();
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "createDataTable()", "CtlImportLeads.xaml.cs");
            }
        }

        public void ReadFromFile(string filename)
        {
            try
            {
            }

            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "ReadFromFile()", "CtlImportLeads.xaml.cs");
            }
        }

        void cmbListType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (cmbListType.SelectedIndex == 1)
                {

                   // cmbFilterType.Visibility = Visibility.Collapsed;
                    //lblFilterType.Visibility = Visibility.Collapsed;
                    lblDNC.Visibility = Visibility.Visible;
                    txtDNC.Visibility = Visibility.Visible;
                    lblGlobalDNC.Visibility = Visibility.Visible;
                    chkGlobalDNC.Visibility = Visibility.Visible;
                    cnvCheck.Visibility = Visibility.Collapsed;
                    btnSave.IsEnabled = false;
                    txtFile.Text = "";
                    FncFillList(Convert.ToBoolean(((ListBoxItem)cmbListType.SelectedItem).Tag));

                }
                else if(cmbListType.SelectedIndex==0)
                {
                    //cmbFilterType.Visibility = Visibility.Visible;
                    //lblFilterType.Visibility = Visibility.Visible;
                    lblDNC.Visibility = Visibility.Collapsed;
                    txtDNC.Visibility = Visibility.Collapsed;
                    lblGlobalDNC.Visibility = Visibility.Collapsed;
                    chkGlobalDNC.Visibility = Visibility.Collapsed;
                    cnvCheck.Visibility = Visibility.Visible;
                    txtFile.Text = "";
                    btnSave.IsEnabled = false;
                    FncFillList(Convert.ToBoolean(((ListBoxItem)cmbListType.SelectedItem).Tag));
                  /*  if (chkTimeZone != null)
                    {
                        cnvCheck.Children.RemoveRange(0, chkTimeZone.Length + 1);
                        flag = 1;
                       // cmbFilterType.SelectedIndex = -1;
                        flag = 0;
                    }*/

                }
                
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "cmbListType_SelectionChanged()", "CtlImportLeads.xaml.cs");
            }
        }

        void btnBrowse_Click(object sender, RoutedEventArgs e)
        {
            try
            {
               // cmbFilterType.SelectedItem = null;
                lblNoOfProcessed.Content = "";
                lblBadLeads.Content = "";
                lblRecordsInserted.Content = "";
                lblDuplicateLeads.Content = "";
                lblNoAreaCode.Content = "";
                txtFile.Text = "";
                System.Windows.Forms.OpenFileDialog d = new System.Windows.Forms.OpenFileDialog();
                string[] str = ((ListBoxItem)cmbFormat.SelectedItem).Tag.ToString().Split(',');

                if (str[1].ToLower() == "excel") //|| str[1].ToLower() == "csv"
                {
                    //d.Filter = "Excel Files (*.xls)|*.xls"; //|CommaSeparated Files (*.csv)|*.csv"
                    d.Filter = "Excel Files |*.xls;*.xlsx;";
                }
                else if (str[1].ToLower() == "csv")
                {
                    d.Filter = "CommaSeparated Files (*.csv)|*.csv";
                }
                else
                {
                    d.Filter = "Text Files (*.txt)|*.txt";
                }

                if (d.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    //MessageBox.Show(d.FileName.ToString());
                }
                txtFile.Text = d.FileName.ToString();
                fileName = txtFile.Text;
                if (txtFile.Text.Length > 0)
                {
                    if (txtFile.Text.Length > 0 && cmbListType.SelectionBoxItem.ToString().ToLower() == "dnc list")
                    {
                        txtDNC.IsEnabled = false;
                        txtDNC.Text = "";
                        btnSave.IsEnabled = true;
                    }
                    else
                    {
                        txtDNC.IsEnabled = true;
                        btnSave.IsEnabled = true;
                    }
                }
                else
                {
                    btnSave.IsEnabled = false;
                }
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "btnBrowse_Click()", "CtlImportLeads.xaml.cs");
            }

        }

        void cmbCampaigns_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
            }

            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "cmbCampaigns_SelectionChanged()", "CtlImportLeads.xaml.cs");
            }
        }

        public void FncFillList(bool state)
        {
            try
            {
                lstLists.Items.Clear();
                ClsListCollection objListColl = ClsListCollection.GetAll(state);

                for (int i = 0; i < objListColl.Count; i++)
                {
                    ListBoxItem lbiItem = new ListBoxItem();
                    lbiItem.Content = objListColl[i].ListName;
                    lbiItem.Tag = objListColl[i].ID;
                    if (objListColl[i].IsActive == true)
                    {
                        lbiItem.Foreground = Brushes.Green;
                    }
                    else
                    {
                        lbiItem.Foreground = Brushes.Red;
                    }
                    lstLists.Items.Add(lbiItem);
                }
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "FncFillList()", "CtlImportLeads.xaml.cs");
            }
        }

    }
}
