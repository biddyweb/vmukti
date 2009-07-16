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
using CRM.Business;
using System.Reflection;
using System.Net;
using System.DirectoryServices;
using System.IO;
using System.Data;
using VMuktiAPI;

namespace CRM.Presentation
{
    /// <summary>
    /// Interaction logic for CtlCRM.xaml
    /// </summary>
    /// 
    public enum ModulePermissions
    {
        Add = 0,
        Edit = 1,
        Delete = 2,
        View = 3
    }

    public partial class CtlCRM : UserControl
    {
        //public static StringBuilder sb1;
        int varState = 0;
        int varID = 0;

        ClsCRMCollection objCRMCollection = null;

        ModulePermissions[] _MyPermissions;
        

        string user_name = "adiance"; //ur ftp user name
        string password = "adiance"; // ftp password
        string uri = VMuktiAPI.VMuktiInfo.BootStrapIPs[0]; // ftp uri
        string port = "21";           // keep same dnt change


        DataTable dtScript = new DataTable();

        //Constructor of the CtlCRM.xaml.cs
        public CtlCRM(ModulePermissions[] MyPermissions)
        {

            try
            {
                varState = 0;
                InitializeComponent();

                this.Loaded +=new RoutedEventHandler(CtlCRM_Loaded);

                //this.Loaded += new RoutedEventHandler(CtlCRM_Loaded);

                //Registering the Events of button.
                btnSave.Click+=new RoutedEventHandler(btnSave_Click);
                btnCancel.Click+=new RoutedEventHandler(btnCancel_Click);
                _MyPermissions = MyPermissions;
                FncPermissionsReview();

                funSetGrid();
            

            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "CtlCRM()", "CtlCRM.xaml.cs");
            }
        }

        void CtlCRM_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                this.Width = ((Grid)this.Parent).ActualWidth;
                ((Grid)this.Parent).SizeChanged += new SizeChangedEventHandler(CtlCRM_SizeChanged);
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "CtlCRM_Loaded()", "CtlCRM.xaml.cs");              
            }
             
        }

        void CtlCRM_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            this.Width = e.NewSize.Width;
        }

        //This function review the permission and display Grid according to permission.
        void FncPermissionsReview()
        {
            try
            {
                //Setting property of Grid
                //Grid can not be edited.
                CtlGrid.CanEdit = false;

                //Grid can not be deleted.
                CtlGrid.CanDelete = false;

                //set the visibility of Grid to collapsed.
                CtlGrid.Visibility = Visibility.Collapsed;


                //Setting the property of tab control.
                tbcCRMAddition.Visibility = Visibility.Collapsed;
                btnSave.Visibility = Visibility.Collapsed;
                btnCancel.Visibility = Visibility.Collapsed;


                //Review the permission and set property according to Permission.
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
                        tbcCRMAddition.Visibility = Visibility.Visible;
                        btnSave.Visibility = Visibility.Visible;
                        btnCancel.Visibility = Visibility.Visible;
                    }
                }
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "FncPermissionsReview()", "CtlCRM.xaml.cs");
            }
        }

        //This Method called when user click on the Edit button of the Grid.
        //Record can be Edited from the Grid through this method.
        void CtlGrid_btnEditClicked(int rowID)
        {
            try
            {
                //Get the id of the selected row.
                varID = Convert.ToInt32(objCRMCollection[rowID].ID);

                //Tab will be made visible for editing the record.
                tbcCRMAddition.Visibility = Visibility.Visible;
                btnSave.Visibility = Visibility.Visible;
                btnCancel.Visibility = Visibility.Visible;

                //Initialize the controls of the tab with selected row of the Grid.
                txtName.Text = objCRMCollection[rowID].CRMName;
                txtCRMURL.Text = objCRMCollection[rowID].CRMURL;
                chkIsActive.IsChecked = (objCRMCollection[rowID].IsActive == true);

                //ClsCRMTypeCollection objCRMTypeCollection = new ClsCRMTypeCollection();
                //objScriptTypeCollection = ClsScriptTypeCollection.GetAll();

                //for (int i = 0; i < cmbScriptType.Items.Count; i++)
                //{
                //    if (((ComboBoxItem)cmbScriptType.Items[i]).Tag.ToString() == objScriptCollection[rowID].ScriptTypeID.ToString())
                //        cmbScriptType.Text = ((ComboBoxItem)cmbScriptType.Items[i]).Content.ToString();
                //}

                //Set Flag variable that indicates Add or Edit
                varState = 1;
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "CtlGrid_btnEditClicked()", "CtlCRM.xaml.cs");
            }
        }

        //This Method is called when user click on the delete button of the Grid.
        //Record can be deleted from the Grid through this method.
        void CtlGrid_btnDeleteClicked(int rowID)
        {
            try
            {
                //Get the id of the selected row.
                varID = Convert.ToInt32(objCRMCollection[rowID].ID);

                //Gets the confirmation from the user for deleting record.
                MessageBoxResult r = System.Windows.MessageBox.Show("Do You Really Want To Delete This Record ?", "->Delete CRM", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (r == MessageBoxResult.Yes)
                {
                    //Calls the method of ClsCRM class of CRM.Business to delete the record.
                    ClsCRM.Delete(varID);
                    System.Windows.MessageBox.Show("Record Deleted!!", "->CRM Deleted", MessageBoxButton.OK, MessageBoxImage.Information);

                    //Refersh the Grid.
                    funSetGrid();
                }
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "CtlGrid_btnDeleteClicked()", "CtlCRM.xaml.cs");
            }
        }
        
        

        //This method is called when User click on the cancel button of the Tab control.
        void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //Refresh the Grid.
                funSetGrid();

                //Set flag variable to its Default value.
                varState = 0;

                //Clear Tab control.
                FncClearAll();
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "btnCancel_Click()", "CtlCRM.xaml.cs");
            }
        }

        //This method clears all the controls of the Tab control.
        private void FncClearAll()
        {
            try
            {
                //Clears the control of the tab control.
                txtName.Text = "";
                txtCRMURL.Text = "";
                //cmbScriptType.Text = "";
                chkIsActive.IsChecked = false;

                //Review the Permissions.
                FncPermissionsReview();

                //Set variables to its default value.
                varID = -1;
                varState = 0;
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "FncClearAll()", "CtlCRM.xaml.cs");
            }
        }

        //This method is called when User click on the Save button the tab control.
        void btnSave_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            try
            {
                //Create the object of the ClsCRM Class of CRM.
                ClsCRM d = new ClsCRM();

                //Checks wheather textbox containing CRMname is left blank or not.
                if (txtName.Text.Trim() == "")
                {
                    System.Windows.MessageBox.Show("CRM Name Can't Be Left Blank", "-> CRM Name", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    txtName.Focus();
                    txtName.Text = txtName.Text.Trim();
                    return;
                }

                //Check wheather to edit existing record or add new record.
                if (varState == 0)
                {
                    //Add new Record.
                    d.ID = -1;
                }
                else
                {
                    //Edit existing Record.
                    d.ID = varID;
                }
                string strZipFile = txtCRMURL.Text;
                string[] str = strZipFile.Split('\\');

                //Set the object of the ClsCRM.
                d.CRMName = txtName.Text;
                d.CRMURL = txtCRMURL.Text;
                d.IsActive = (bool)chkIsActive.IsChecked;
                //ComboBoxItem l = new ComboBoxItem();
                //l = (ComboBoxItem)cmbScriptType.SelectedItem;

                //d.ScriptTypeID = Convert.ToInt32(l.Tag);
                d.CreatedBy = VMuktiAPI.VMuktiInfo.CurrentPeer.ID;

                //Call the save method of ClsCRM class of Vmukti.Business.
                int gID = d.Save();

                //Check wheather record is saved successfully or not.
                if (gID == 0)
                {
                    MessageBox.Show("Duplicate Entry For CRM Name Is Not Allowed !!", "-> CRM ", MessageBoxButton.OK, MessageBoxImage.Information);
                }

                else
                {
                    System.Windows.MessageBox.Show("Record Saved Successfully!!");
                    funSetGrid();
                    varState = 0;
                    FncClearAll();
                }

                //FncCreateVirtualDirectory();

                ////FncCreateFolders();

                //#region UploadFile

                //Uri u = new Uri("ftp://" + VMuktiAPI.VMuktiInfo.BootStrapIPs[0] + "/vmukti/CRMs/");
                //WebClient wc = new WebClient();
                //wc.UseDefaultCredentials = false;
                //wc.Credentials = new NetworkCredential("adiance", "adiance");
                //try
                //{
                //    wc.UploadFile("ftp://" + VMuktiAPI.VMuktiInfo.BootStrapIPs[0] + "/vmukti/CRMs/" + str[str.Length - 1], strZipFile);
                //}
                //catch (Exception exp)
                //{
                //    MessageBox.Show(exp.Message);
                //}

                //#endregion
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "btnSave_Click()", "CtlCRM.xaml.cs");
            }
        }

        //This function set the Grid with existing available data.
        void funSetGrid()
        {
            try
            {
                //Set the Grid.
                CtlGrid.Cols = 4;
                CtlGrid.CanEdit = true;
                CtlGrid.CanDelete = true;

                //Set the Header of the column of the grid.
                CtlGrid.Columns[0].Header = "ID";
                CtlGrid.Columns[1].Header = "CRM Name";
                CtlGrid.Columns[2].Header = "CRM URL";

                CtlGrid.Columns[3].Header = "Is Active";
                CtlGrid.Columns[3].IsIcon = true;
                //CtlGrid.Columns[5].Header = "CreatedBy";

                //Bind the column of the Grid.
                CtlGrid.Columns[0].BindTo("ID");
                CtlGrid.Columns[1].BindTo("CRMName");
                CtlGrid.Columns[2].BindTo("CRMURL");

                CtlGrid.Columns[3].BindTo("IsActive");
                //CtlGrid.Columns[5].BindTo("CreatedBy");

                //Calls the method of CRM.Business to retrive existing CRM.
                objCRMCollection = ClsCRMCollection.GetAll();
                CtlGrid.Bind(objCRMCollection);
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "funSetGrid()", "CtlCRM.xaml.cs");
            }
        }

        private void btnBrowse_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                System.Windows.Forms.OpenFileDialog d = new System.Windows.Forms.OpenFileDialog();
                d.Filter = "Excel Files (*.zip)|*.zip";
                if (d.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    txtCRMURL.Text = d.FileName.ToString();
                }
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "btnBrowse_Click()", "CtlCRM.xaml.cs");
            }
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
                    VMuktiHelper.ExceptionHandler(ex, "CtlCRM()", "CtlCRM.xaml.cs");
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
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                VMuktiHelper.ExceptionHandler(ex, "FncCreateVirtualDirectory()", "CtlCRM.xaml.cs");
            }

        }

        void FncCreateFolders()
        {
            try
            {
                CreatHomeDir("Scripts");
                CreatHomeDir("ScriptModules");
                CreatHomeDir("CRMs");
                CreatHomeDir("CRMModules");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                VMuktiHelper.ExceptionHandler(ex, "FncCreateFolders()", "CtlCRM.xaml.cs");
            }

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
                MessageBox.Show("Directory is not exist");
                flg = "false";
                VMuktiHelper.ExceptionHandler(ex, "DirList()", "CtlCRM.xaml.cs");
            }

            return (flg);

        }

        void CreatHomeDir(string strDir)
        {
            try
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
            catch (Exception ex)
            {
                MessageBox.Show("Directory is not exist");

                VMuktiHelper.ExceptionHandler(ex, "CreatHomeDir()", "CtlCRM.xaml.cs");
            }

        }

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



    }
}
