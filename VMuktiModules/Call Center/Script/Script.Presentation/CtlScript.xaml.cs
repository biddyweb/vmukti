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
using System.Collections;
using System.Collections.Generic;
//using System.Xml.Linq;
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
using Script.Business;
using System.Reflection;
using System.Net;
using System.DirectoryServices;
using System.IO;
using VMuktiAPI;

namespace Script.Presentation
{   /// <summary>
    /// Interaction logic for CtlScript.xaml
    /// </summary>
    /// 
    public enum ModulePermissions
    {
        Add = 0,
        Edit = 1,
        Delete = 2,
        View = 3
    }
    public partial class CtlScript : System.Windows.Controls.UserControl
    {
        int varState = 0;
        int varID = 0;

        ClsScriptCollection objScriptCollection = null;
        
        ModulePermissions[] _MyPermissions;
        
        
        string user_name = "adiance"; //ur ftp user name
        string password = "adiance"; // ftp password
        string uri = VMuktiAPI.VMuktiInfo.BootStrapIPs[0]; // ftp uri
        string port = "21";           // keep same dnt change


        DataTable dtScript = new DataTable();
        
        public CtlScript(ModulePermissions[] MyPermissions)
        {
            try
            {
                varState = 0;
                InitializeComponent();

                this.Loaded += new RoutedEventHandler(CtlScript_Loaded);

                _MyPermissions = MyPermissions;
                FncPermissionsReview();

                funSetGrid();
                funSetComboBox();
               
           
                btnCancel.Click += new RoutedEventHandler(btnCancel_Click);
                btnSave.Click += new RoutedEventHandler(btnSave_Click);
                grpBoxWebScript.IsEnabled = false;
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "CtlScript", "CtlScript.xaml.cs");
            }
            //CtlGrid.btnDeleteClicked += new VMukti.CtlGrid.Presentation.ctlGrid.ButtonClicked(CtlGrid_btnDeleteClicked);
            //CtlGrid.btnEditClicked += new VMukti.CtlGrid.Presentation.ctlGrid.ButtonClicked(CtlGrid_btnEditClicked);
        }

        void CtlScript_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                this.Width = ((Grid)this.Parent).ActualWidth;
                ((Grid)this.Parent).SizeChanged += new SizeChangedEventHandler(CtlScript_SizeChanged);
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "CtlScript_Loaded", "CtlScript.xaml.cs");
            }
        }

        void CtlScript_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            this.Width = e.NewSize.Width;
        }
        void FncPermissionsReview()
        {
            try
            {
                CtlGrid.CanEdit = false;
                CtlGrid.CanDelete = false;
                CtlGrid.Visibility = Visibility.Collapsed;

                tbcScriptAddition.Visibility = Visibility.Collapsed;
                btnSave.Visibility = Visibility.Collapsed;
                btnCancel.Visibility = Visibility.Collapsed;

                for (int i = 0; i < _MyPermissions.Length; i++)
                {
                    if (_MyPermissions[i] == ModulePermissions.Edit)
                    {
                        CtlGrid.CanEdit = true;
                    }
                    if (_MyPermissions[i] == ModulePermissions.Delete)
                    {
                        CtlGrid.CanDelete = true;
                    }
                    if (_MyPermissions[i] == ModulePermissions.View)
                    {
                        CtlGrid.Visibility = Visibility.Visible;
                    }
                    if (_MyPermissions[i] == ModulePermissions.Add)
                    {
                        tbcScriptAddition.Visibility = Visibility.Visible;
                        btnSave.Visibility = Visibility.Visible;
                        btnCancel.Visibility = Visibility.Visible;
                    }
                }
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "FncPermissionsReview", "CtlScript.xaml.cs");
            }
        }

        void CtlGrid_btnEditClicked(int rowID)
        {
            try
            {
                varID = Convert.ToInt32(objScriptCollection[rowID].ID);

                tbcScriptAddition.Visibility = Visibility.Visible;
                btnSave.Visibility = Visibility.Visible;
                btnCancel.Visibility = Visibility.Visible;

                txtName.Text = objScriptCollection[rowID].ScriptName;
                txtScriptURL.Text = objScriptCollection[rowID].ScriptURL;
                chkIsActive.IsChecked = (objScriptCollection[rowID].IsActive == true);

                ClsScriptTypeCollection objScriptTypeCollection = new ClsScriptTypeCollection();
                objScriptTypeCollection = ClsScriptTypeCollection.GetAll();

                for (int i = 0; i < cmbScriptType.Items.Count; i++)
                {
                    if (((ComboBoxItem)cmbScriptType.Items[i]).Tag.ToString() == objScriptCollection[rowID].ScriptTypeID.ToString())
                        cmbScriptType.Text = ((ComboBoxItem)cmbScriptType.Items[i]).Content.ToString();
                }

                varState = 1;
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "CtlGrid_btnEditClicked", "CtlScript.xaml.cs");
            }

        }


        void CtlGrid_btnDeleteClicked(int rowID)
        {
            try
            {

                varID = Convert.ToInt32(objScriptCollection[rowID].ID);
                MessageBoxResult r = System.Windows.MessageBox.Show("Do You Really Want To Delete This Record ?", "Delete Script", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (r == MessageBoxResult.Yes)
                {
                    ClsScript.Delete(varID);
                    System.Windows.MessageBox.Show("Record Deleted!!", "Group Script", MessageBoxButton.OK, MessageBoxImage.Information);
                    funSetGrid();
                }
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "CtlGrid_btnDeleteClicked", "CtlScript.xaml.cs");
            }
        }


        void funSetComboBox()
        {
            try
            {
                ClsScriptTypeCollection obj = new ClsScriptTypeCollection();
                obj = ClsScriptTypeCollection.GetAll();

                for (int i = 0; i < obj.Count; i++)
                {
                    ComboBoxItem l = new ComboBoxItem();

                    l.Content = obj[i].Scripttype;
                    l.Tag = obj[i].ID;
                    cmbScriptType.Items.Add(l);
                }
                cmbScriptType.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "funSetComboBox", "CtlScript.xaml.cs");
            }
 
        }


        void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                funSetGrid();
                varState = 0;
                FncClearAll();
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "btnCancel_Click", "CtlScript.xaml.cs");
            }
 
        }

        private void FncClearWebScriptInformation()
        {
            try
            {
                txtScriptURL.Text = "";
                cmbLeadFormat.Items.Clear();
                cmbLead.Items.Clear();
                cmbAgentInfo.Items.Clear();
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "CtlScript", "CtlScript.xaml.cs");
            }
        }

        private void FncClearAll() 
        {
            try
            {
                txtName.Text = "";
                txtScriptURL.Text = "";
                cmbScriptType.Text = "";
                chkIsActive.IsChecked = false;
                cmbLeadFormat.Items.Clear();
                cmbLead.Items.Clear();
                cmbAgentInfo.Items.Clear();
                FncPermissionsReview();
                varID = -1;
                varState = 0;
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "FncClearAll", "CtlScript.xaml.cs");
            }

        }

        void btnSave_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            try
            {
                ClsScript d = new ClsScript();

                if (txtName.Text.Trim() == "" || txtName.Text.Length > 100)
                {
                    System.Windows.MessageBox.Show("Script Name Can't Be Left Blank or Script name can not more than 100 characters.", "-> Script Name", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    txtName.Focus();
                    txtName.Text = txtName.Text.Trim();
                    return;
                }

                if (cmbScriptType.Text.Trim() == "")
                {
                    System.Windows.MessageBox.Show("Select A Script Type", "-> Script Type", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    cmbScriptType.Focus();
                    return;
                }

                if (varState == 0)
                {
                    d.ID = -1;
                }
                else
                {
                    d.ID = varID;
                }
                string strZipFile = txtScriptURL.Text;
                string[] str = strZipFile.Split('\\');

                d.ScriptName = txtName.Text;
                d.ScriptURL = txtScriptURL.Text;
                d.IsActive = (bool)chkIsActive.IsChecked;
                ComboBoxItem l = new ComboBoxItem();
                l = (ComboBoxItem)cmbScriptType.SelectedItem;

                d.ScriptTypeID = Convert.ToInt32(l.Tag);
                d.CreatedBy = VMuktiAPI.VMuktiInfo.CurrentPeer.ID;

                int gID = d.Save();

                if (gID == 0)
                {
                    MessageBox.Show("Duplicate Entry For Script Name Is Not Allowed !!", "-> Script ", MessageBoxButton.OK, MessageBoxImage.Information);
                }

                else
                {
                    System.Windows.MessageBox.Show("Record Saved Successfully!!");
                    funSetGrid();
                    varState = 0;
                    FncClearAll();
                }


            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "btnSave_Click", "CtlScript.xaml.cs");
            }

        }


        void funSetGrid()
        {
            try
            {
                CtlGrid.Cols = 5;
                CtlGrid.CanEdit = true;
                CtlGrid.CanDelete = true;

                CtlGrid.Columns[0].Header = "ID";
                CtlGrid.Columns[1].Header = "Script Name";
                CtlGrid.Columns[2].Header = "Script URL";
                CtlGrid.Columns[3].Header = "Script Type";
                CtlGrid.Columns[4].Header = "Is Active";
                CtlGrid.Columns[4].IsIcon = true;
                //CtlGrid.Columns[5].Header = "CreatedBy";

                CtlGrid.Columns[0].BindTo("ID");
                CtlGrid.Columns[1].BindTo("ScriptName");
                CtlGrid.Columns[2].BindTo("ScriptURL");
                //  CtlGrid.Columns[3].BindTo("ScriptTypeID");
                CtlGrid.Columns[3].BindTo("Scripttype");
                CtlGrid.Columns[4].BindTo("IsActive");
                //CtlGrid.Columns[5].BindTo("CreatedBy");

                objScriptCollection = ClsScriptCollection.GetAll();
                CtlGrid.Bind(objScriptCollection);
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "funSetGrid", "CtlScript.xaml.cs");
            }

        }

        private void btnBrowse_Click(object sender, RoutedEventArgs e)
        {
            //System.Windows.Forms.OpenFileDialog d = new System.Windows.Forms.OpenFileDialog();
            //d.Filter = "Excel Files (*.zip)|*.zip";
            //if (d.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            //{
            //    txtScriptURL.Text = d.FileName.ToString();
            //}
        }

        void FncCreateVirtualDirectory()
        {
            System.DirectoryServices.DirectoryEntry oDE;
            System.DirectoryServices.DirectoryEntries oDC;
            System.DirectoryServices.DirectoryEntry oVirDir;
            try
            {
                oDE = new DirectoryEntry("IIS://" + VMuktiAPI.VMuktiInfo.BootStrapIPs[0] + "/MSFTPSVC/1/Root");
                oDC = oDE.Children;
                try
                {
                    oVirDir = oDC.Add("VMukti", oDE.SchemaClassName.ToString());
                }
                catch (Exception ex)
                {
                    oDC.Remove(new DirectoryEntry("IIS://" + VMuktiAPI.VMuktiInfo.BootStrapIPs[0] + "/MSFTPSVC/1/Root/Vmukti"));
                    oVirDir = oDC.Add("VMukti", oDE.SchemaClassName.ToString());
                }
                oVirDir.CommitChanges();

                if (!Directory.Exists("C:\\Program Files\\Default Company Name\\VMukti\\Adiance"))
                {
                    Directory.CreateDirectory("C:\\Program Files\\Default Company Name\\VMukti\\Adiance");
                }
                oVirDir.Properties["Path"].Value = "C:\\Program Files\\Default Company Name\\VMukti\\Adiance";

                oVirDir.Properties["AccessRead"][0] = true;
                oVirDir.Properties["AccessWrite"][0] = true;
                oVirDir.Properties["AccessExecute"][0] = true;
                oVirDir.CommitChanges();
            }
            catch (Exception exc)
            {
                VMuktiHelper.ExceptionHandler(exc, "FncCreateVirtualDirectory", "CtlScript.xaml.cs");
            }

        }

        void FncCreateFolders()
        {
            CreatHomeDir("Scripts");
            CreatHomeDir("ScriptModules");
            CreatHomeDir("CRMs");
            CreatHomeDir("CRMModules");
        }

        string DirList(string dir)
        {
            string flg = "";
            FtpWebRequest request = (FtpWebRequest)WebRequest.Create("ftp://" + user_name + ":" + password + "@" + uri + ":" + port + "/vmukti/" + dir);
            request.KeepAlive = true;
            request.Method = WebRequestMethods.Ftp.ListDirectory;

            FtpWebResponse resp1 = (FtpWebResponse)request.GetResponse();
            StreamReader sr = new StreamReader(resp1.GetResponseStream(), System.Text.Encoding.ASCII);
            try
            {
                while (true)
                {
                    string FDir = sr.ReadLine();

                    if ((FDir.CompareTo(dir)) == 0)
                    {
                        flg = "true";
                        break;
                    }

                    else
                    {
                        continue;
                    }
                }
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "DirList", "CtlScript.xaml.cs");
            }

            return (flg);

        }

        void CreatHomeDir(string strDir)
        {
            string rd = strDir;
            string dName = rd;
            string dstatus = DirList(rd);

            if ((dstatus.CompareTo("false")) == 0)
            {
                FtpWebRequest request1 = (FtpWebRequest)WebRequest.Create("ftp://" + user_name + ":" + password + "@" + uri + ":" + port + "/vmukti/" + rd);
                request1.KeepAlive = true;
                request1.Method = WebRequestMethods.Ftp.MakeDirectory;

                FtpWebResponse resp = (FtpWebResponse)request1.GetResponse();
                MessageBox.Show("directory created");
            }
            else if ((dstatus.CompareTo("true")) == 0)
            {
                MessageBox.Show("dir is done" + rd);
            }
        }

        private void cmbScriptType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (cmbScriptType.SelectedItem != null)
                {
                    if (((ComboBoxItem)cmbScriptType.SelectedItem).Content.ToString().Trim().Equals("Web Script"))
                    {

                        grpBoxWebScript.IsEnabled = true;

                        cmbLeadFormat.Items.Clear();
                        ClsLeadFormatCollection objLeadFormatCollection = ClsLeadFormatCollection.GetAll();

                        for (int i = 0; i < objLeadFormatCollection.Count; i++)
                        {
                            ComboBoxItem cbi = new ComboBoxItem();
                            cbi.Content = objLeadFormatCollection[i].FormatName;
                            cbi.Tag = objLeadFormatCollection[i].ID;
                            cmbLeadFormat.Items.Add(cbi);
                        }
                        cmbAgentInfo.Items.Add("UserID");
                        cmbAgentInfo.Items.Add("DisplayName");
                        cmbAgentInfo.Items.Add("FirstName");
                        cmbAgentInfo.Items.Add("LastName");
                        cmbAgentInfo.Items.Add("RoleID");
                        cmbAgentInfo.Items.Add("Email");
                        //    cmbLeadFormat.SelectedIndex = 0;
                    }
                    else
                    {
                        grpBoxWebScript.IsEnabled = false;
                        FncClearWebScriptInformation();
                    }

                }
                else
                    grpBoxWebScript.IsEnabled = false;
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "cmbScriptType_SelectionChanged", "CtlScript.xaml.cs");
            }

        }

        private void cmbLeadFormat_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (cmbLeadFormat.SelectedItem != null)
                {
                    Int64 FormatID = Int64.Parse(((ComboBoxItem)cmbLeadFormat.SelectedItem).Tag.ToString());

                    cmbLead.Items.Clear();
                    ClsLeadFormatCollection objLeadFormatCollection = ClsLeadFormatCollection.GetAll(FormatID);

                    for (int i = 0; i < objLeadFormatCollection.Count; i++)
                    {
                        ComboBoxItem cbi = new ComboBoxItem();
                        cbi.Content = objLeadFormatCollection[i].FormatName;
                        cbi.Tag = objLeadFormatCollection[i].ID;
                        cmbLead.Items.Add(cbi);
                    }
                    //    cmbLead.SelectedIndex = 0;
                }
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "CtlScript", "CtlScript.xaml.cs");
            }


        }

        private void btnLeadInfo_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                if (cmbLeadFormat.SelectedItem == null || cmbLead.SelectedItem == null)
                {
                    MessageBox.Show("Please select the proper Lead Format and Lead");
                }
                else
                {
                    if (!txtScriptURL.Text.Trim().Equals(""))
                    {
                        try
                        {
                            Uri uri = new Uri(txtScriptURL.Text);

                            if (((ComboBoxItem)cmbLeadFormat.SelectedItem).Content != null && ((ComboBoxItem)cmbLead.SelectedItem).Content != null)
                            {
                                string strLeadFormat = ((ComboBoxItem)cmbLeadFormat.SelectedItem).Content.ToString();
                                string strLead = ((ComboBoxItem)cmbLead.SelectedItem).Content.ToString();

                                string var = "v" + strLeadFormat + "" + strLead;
                                if (txtScriptURL.Text.Trim().Contains(var))
                                {
                                    MessageBox.Show("URL already contains the selected Lead");
                                }
                                else
                                {
                                    if (txtScriptURL.Text.IndexOf('?') == -1)
                                        txtScriptURL.Text += "?v" + strLeadFormat + "" + strLead + "=<<db-" + strLeadFormat + "-" + strLead + ">>";
                                    else
                                        txtScriptURL.Text += "&v" + strLeadFormat + "" + strLead + "=<<db-" + strLeadFormat + "-" + strLead + ">>";
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            VMuktiHelper.ExceptionHandler(ex, "btnLeadInfo_Clicked", "CtlScript.xaml.cs");
                        }

                    }
                    else
                    {
                        MessageBox.Show("Please enter the base Url first");
                    }
                }
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "btnLeadInfo_Click", "CtlScript.xaml.cs");
            }

        }

        private void btnAgentInfo_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (cmbAgentInfo.SelectedItem == null)
                {
                    MessageBox.Show("Please select the proper Agent Information");
                }
                else
                {
                    if (!txtScriptURL.Text.Trim().Equals(""))
                    {
                        try
                        {
                            Uri uri = new Uri(txtScriptURL.Text);

                            if (cmbAgentInfo.SelectedItem != null)
                            {
                                //string strAgentInfo = ((ComboBoxItem)cmbAgentInfo.SelectedItem).Content.ToString();
                                string strAgentInfo = cmbAgentInfo.SelectedItem.ToString();
                                string var = "vAg" + strAgentInfo;
                                if (txtScriptURL.Text.Trim().Contains(var))
                                {
                                    MessageBox.Show("URL already contains the selected Agent Information");
                                }
                                else
                                {
                                    if (txtScriptURL.Text.IndexOf('?') == -1)
                                        txtScriptURL.Text += "?vAg" + strAgentInfo + "=<<Ag-" + strAgentInfo + ">>";
                                    else
                                        txtScriptURL.Text += "&vAg" + strAgentInfo + "=<<Ag-" + strAgentInfo + ">>";
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            VMuktiHelper.ExceptionHandler(ex, "btnAgentInfo_Click", "CtlScript.xaml.cs");
                        }

                    }
                    else
                    {
                        MessageBox.Show("Please enter the base Url first");
                    }
                }
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "btnAgentInfo_Click", "CtlScript.xaml.cs");
            }

        }

    }
}

