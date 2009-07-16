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
using System.IO;
using System.Net;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Forms;

using VMukti.Business;
using VMukti.ZipUnzip.Zip;
using VMuktiAPI;
using VMukti.Business.WCFServices.BootStrapServices.BasicHttp;
using System.Text;
using VMuktiService;
using System.Windows.Media.Imaging;
using System.Drawing;
using System.Text.RegularExpressions;



namespace VMukti.Presentation.Controls.UploadModule
{
    public class clsModulePermission
    {
        //public static StringBuilder sb1;
        public int ModuleID = -1;
        public int PermissionID = -1;
        public string PermissionName = "";
        public int PermissionValue = -1;

        public clsModulePermission()
        {
            try
            {
        }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "clsModulePermission()", "Controls\\CtlUploadModule.xaml.cs");
            }
        }

        public clsModulePermission(int pModuleID, int pPermissionID, string pPermissionName, int pPermissionValue)
        {
            try
            {
                this.ModuleID = pModuleID;
                this.PermissionID = pPermissionID;
                this.PermissionName = pPermissionName;
                this.PermissionValue = pPermissionValue;
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "clsModulePermission()", "Controls\\CtlUploadModule.xaml.cs");

            }
        }
    }

    /// <summary>
    /// Interaction logic for CtlUploadModule.xaml
    /// </summary>
    public partial class CtlUploadModule : System.Windows.Controls.UserControl,IDisposable
    {
        //public static StringBuilder sb1;
        List<clsModulePermission> objModulePermissions = new List<clsModulePermission>();

        OpenFileDialog ofd = new OpenFileDialog();
        Assembly ass = null;
        Assembly a = null;
        DirectoryInfo di;
        ArrayList al = new ArrayList();
        Type[] typecontrol;
        List<string> strValues = new List<string>();
        List<Assembly> lstAss = new List<Assembly>();
        public IHTTPFileTransferService clientHttpChannelFileTransfer = null;

        public CtlUploadModule()
        {
            try
            {
                InitializeComponent();
                ass = Assembly.GetAssembly(typeof(CtlUploadModule));
                btnUpload.Click += new RoutedEventHandler(btnUpload_Click);
                btnBrowseMouleImage.Click += new RoutedEventHandler(btnBrowseMouleImage_Click);
                btnSubmit.Click += new RoutedEventHandler(btnSubmit_Click);
                btnCancel.Click += new RoutedEventHandler(btnCancel_Click);
                cmbAssembly.SelectionChanged += new SelectionChangedEventHandler(cmbAssembly_SelectionChanged);
                this.IsVisibleChanged += new DependencyPropertyChangedEventHandler(CtlUploadModule_IsVisibleChanged);
                BasicHttpClient bhcFiletransfer = new BasicHttpClient();
                clientHttpChannelFileTransfer = (IHTTPFileTransferService)bhcFiletransfer.OpenClient<IHTTPFileTransferService>("http://" + VMuktiInfo.BootStrapIPs[0] + ":80/VMukti/HttpFileTransferService");
                clientHttpChannelFileTransfer.svcHTTPFileTransferServiceJoin();

            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "CtlUploadModule()", "Controls\\CtlUploadModule.xaml.cs");
            }
        }

        void btnBrowseMouleImage_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog objFileDialog = new OpenFileDialog();
            objFileDialog.Title = "Open Image File";
            objFileDialog.Filter = "Bitmap Files|*.bmp" + "|Enhanced Windows MetaFile|*.emf" + "|Exchangeable Image File|*.exif" + "|Gif Files|*.gif|Icons|*.ico|JPEG Files|*.jpg" + "|PNG Files|*.png|TIFF Files|*.tif|Windows MetaFile|*.wmf";
            objFileDialog.DefaultExt = "jpg";
            objFileDialog.ShowDialog();
            txtModuleImage.Text = objFileDialog.FileName;
        }

        void CtlUploadModule_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (!((bool)e.NewValue))
            {
                btnCancel_Click(null, null);
            }
        } 

        void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                txtUpload.Clear();
                txtZip.Clear();
                cmbAssembly.Items.Clear();
                cmbClass.Items.Clear();
                txtModule.Clear();
                txtVersion.Clear();
                rtbDescription.SelectAll();
                rtbDescription.Selection.Text = "";
                ckbCollaborate.IsChecked = false;
                ckbAuthentication.IsChecked = false;
                lstAss.Clear();


                if (grid1.Children.Count > 0)
                {
                    grid1.Children.Clear();
                }

            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "btnCancel_click()", "Controls\\CtlUploadModule.xaml.cs");
            }
        }

        void btnSubmit_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                #region cheking

               

                if (ofd.FileName == null || ofd.FileName == "")
                {
                    System.Windows.MessageBox.Show("Browse the File to Upload");
                    txtUpload.Focus();
                    return;

                }
                if (cmbAssembly.SelectedItem == null)
                {
                    System.Windows.MessageBox.Show("Select the Assembly File");
                    cmbAssembly.Focus();
                    return;
                }
                if (cmbClass.SelectedItem == null)
                {
                    System.Windows.MessageBox.Show("Select the Class Name");
                    cmbClass.Focus();
                    return;
                }
                if (txtModule.Text == "" || txtModule.Text == null)
                {
                    System.Windows.MessageBox.Show("Enter Module Name");
                    txtModule.Focus();
                    return;
                }

                if (!new ClsModuleLogic().ModuleExists(txtModule.Text))
                {
                    System.Windows.MessageBox.Show("Specify another module name");
                    txtModule.Focus();
                    return;
                }
                
                if (!IsVersionValid())
                {
                    System.Windows.MessageBox.Show("Enter Valid Version Number");
                    txtVersion.Focus();
                    return;
                }

                #endregion

                try
                {

                    DirectoryInfo diZip = new DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory + "\\Zip Files");

                    File.Copy(ofd.FileName, diZip.FullName + "\\" + ofd.SafeFileName);


                    DirectoryInfo diModule = new DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory + "\\Modules");
                    VMukti.ZipUnzip.Zip.FastZip fz = new VMukti.ZipUnzip.Zip.FastZip();
                    if (!Directory.Exists(diModule.FullName + "\\" + ofd.SafeFileName.Split('.')[0]))
                    {
                        fz.ExtractZip(diZip.FullName + "\\" + ofd.SafeFileName, diModule.FullName, null);
                    }


                    //    Upload the Zip file To bs 

                    System.IO.FileStream stream = new FileStream(ofd.FileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);

                    RemoteFileInfo rfiUpload = new RemoteFileInfo();
                    rfiUpload.FileName = ofd.SafeFileName;
                    rfiUpload.FolderNameToStore = "";
                    rfiUpload.FileByteStream = stream;
                    rfiUpload.Length = stream.Length;
                    clientHttpChannelFileTransfer.svcHTTPFileTransferServiceUploadFile(rfiUpload);
                    stream.Flush();
                    stream.Close();
                    FileStream fs = new FileStream(ofd.FileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                    rfiUpload.FileByteStream = fs;
                    clientHttpChannelFileTransfer.svcHTTPFileTransferServiceUploadFileToInstallationDirectory(rfiUpload);
                    fs.Flush();
                    fs.Close();
                }
                catch (Exception ex)
                {
                    System.Windows.MessageBox.Show("The Zip File is already uploaded please select another File");
                    VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "btnSubmit_Click()", "Controls\\CtlUploadModule.xaml.cs");
                    return;
                }

                ClsModuleLogic cml = new ClsModuleLogic();
                TextRange tr = new TextRange(rtbDescription.Document.ContentStart, rtbDescription.Document.ContentEnd);


                #region Creating byte array of imagefile
                List<byte[]> ImageByte = new List<byte[]>();

                if (!string.IsNullOrEmpty(txtModuleImage.Text))
                {
                    byte[] b = SetImage(txtModuleImage.Text);
                    if (b != null)
                    {
                        ImageByte.Add(b);
                    }
                    else
                    {
                        b = SetImage(AppDomain.CurrentDomain.BaseDirectory + "\\Skins\\Images\\Help.png");
                        if (b != null)
                        {
                            ImageByte.Add(b);
                        }
                    }
                }
                else
                {
                    byte[] b = SetImage(AppDomain.CurrentDomain.BaseDirectory + "\\Skins\\Images\\Help.png");
                    if (b != null)
                    {
                        ImageByte.Add(b);
                    }
                }
                #endregion

                int intModId = -1;

                intModId = cml.AddModule(-1, txtModule.Text, txtVersion.Text, tr.Text, ((ComboBoxItem)cmbAssembly.SelectedItem).Content.ToString(), ((ComboBoxItem)cmbClass.SelectedItem).Content.ToString(), txtZip.Text, 1, ckbCollaborate.IsChecked.Value, ckbAuthentication.IsChecked.Value, ImageByte[0]);

                if (intModId == -1)
                {
                    System.Windows.MessageBox.Show("Report this error to your administrator");
                    return;
                }
                    ClsPermissionLogic cpl = new ClsPermissionLogic();

                    foreach (object chk in grid1.Children)
                    {
                        if (chk.GetType() == typeof(System.Windows.Controls.CheckBox) && ((System.Windows.Controls.CheckBox)chk).Tag != null)
                        {
                            int i = 0;
                            string[] strPermission = (((System.Windows.Controls.CheckBox)chk).Tag).ToString().Split(',');

                            for (i = 0; i < objModulePermissions.Count; i++)
                            {
                                if (objModulePermissions[i].PermissionName == strPermission[0])
                                {
                                    break;
                                }
                            }
                            if (i == objModulePermissions.Count)
                            {
                                objModulePermissions.Add(new clsModulePermission(intModId, cpl.Add_Permission(-1, intModId, strPermission[0], int.Parse(strPermission[1])), strPermission[0], int.Parse(strPermission[1])));
                            }

                            if (((System.Windows.Controls.CheckBox)chk).IsChecked == true)
                            {
                                for (int j = 0; j < objModulePermissions.Count; j++)
                                {
                                    if (objModulePermissions[j].PermissionName == strPermission[0])
                                    {
                                        cpl.Add_ModulePermission(objModulePermissions[j].PermissionID, int.Parse(strPermission[2]));
                                        ((System.Windows.Controls.CheckBox)chk).IsChecked = false;

                                        break;
                                    }
                                }
                            }
                        }
                    }

                    System.Windows.MessageBox.Show("Module Uploaded");

                    txtUpload.Clear();
                    txtZip.Clear();
                    cmbAssembly.Items.Clear();
                    cmbClass.Items.Clear();
                    txtModule.Clear();
                    txtVersion.Clear();
                    ckbCollaborate.IsChecked = false;
                    rtbDescription.SelectAll();
                    rtbDescription.Selection.Text = "";
                    ckbAuthentication.IsChecked = false;
                    txtModuleImage.Text = "";

                    if (grid1.Children.Count > 0)
                    {
                        grid1.Children.Clear();
                    }

                    objModulePermissions.Clear();
                

            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "btnSubmit_Click()--1", "Controls\\CtlUploadModule.xaml.cs");
            }
        }

        void btnUpload_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!Directory.Exists(ass.Location.Replace("VMukti.Presentation.exe", @"Temp")))
                {
                    di = Directory.CreateDirectory(ass.Location.Replace("VMukti.Presentation.exe", @"Temp"));
                }
                else
                {
                    di = new DirectoryInfo(ass.Location.Replace("VMukti.Presentation.exe", @"Temp"));
                }

                DirectoryInfo[] dinew = di.GetDirectories();

                for (int i = 0; i < dinew.Length; i++)
                {
                    dinew[i].Delete(true);
                }

                ofd.Filter = "Zip Files|*.zip";

                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    FastZip fz = new FastZip();
                    fz.ExtractZip(ofd.FileName, di.FullName, null);

                    txtUpload.Text = ofd.FileName;
                    txtZip.Text = ofd.SafeFileName;

                    DirectoryInfo[] diSub = di.GetDirectories()[0].GetDirectories();

                    for (int m = 0; m < diSub.Length; m++)
                    {
                        if (diSub[m].Name.ToLower() == "control")
                        {
                            ShowDirectory(diSub[m]);
                            break;
                        }
                    }

                    for (int j = 0; j < al.Count; j++)
                    {
                        using (FileStream fs = File.Open(al[j].ToString(), FileMode.Open))
                        {
                            using (MemoryStream ms = new MemoryStream())
                            {
                                byte[] buffer = new byte[1024];
                                int read = 0;
                                while ((read = fs.Read(buffer, 0, 1024)) > 0)
                                    ms.Write(buffer, 0, read);
                                try
                                {
                                    a = Assembly.Load(ms.ToArray());
                                }
                                catch (Exception ex)
                                {
                                    VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "btnUpload_Click()", "Controls\\CtlUploadModule.xaml.cs");
                                }
                            }
                        }

                        typecontrol = a.GetTypes();
                        for (int n = 0; n < typecontrol.Length; n++)
                        {
                            if (typecontrol[n].BaseType != null)
                            {
                                if (typecontrol[n].BaseType.Name.ToLower() == "usercontrol")
                                {
                                    int z = 0;
                                    for (z = 0; z < cmbAssembly.Items.Count; z++)
                                    {
                                        if (((ComboBoxItem)cmbAssembly.Items[z]).Content.ToString() == typecontrol[n].Module.ToString())
                                        {
                                            break;
                                        }
                                    }
                                    if (z == cmbAssembly.Items.Count)
                                    {
                                        ComboBoxItem cbiNew = new ComboBoxItem();
                                        lstAss.Add(a);
                                        cbiNew.Content = typecontrol[n].Module.ToString();
                                        cmbAssembly.Items.Add(cbiNew);
                                    }
                                    
                                }
                                if (typecontrol[n].BaseType.Name.ToLower() == "enum")
                                {
                                    
                                    if (typecontrol[n].Name == "ModulePermissions")
                                    {
                                        grid1.IsEnabled = true;
                                        scrollViewer1.IsEnabled = true;

                                        string[] str = Enum.GetNames(typecontrol[n]);


                                        for (int enumValues = 0; enumValues < typecontrol[n].GetFields().Length; enumValues++)
                                        {

                                            if (typecontrol[n].GetFields()[enumValues].FieldType.Name == "ModulePermissions")
                                            {
                                                strValues.Add(typecontrol[n].GetFields()[enumValues].GetRawConstantValue().ToString());
                                            }
                                        }

                                        ClsRoleCollection crc = ClsRoleCollection.GetAll();

                                        for (int i = 0; i < str.Length + 1; i++)
                                        {
                                            ColumnDefinition col = new ColumnDefinition();
                                            grid1.ColumnDefinitions.Add(col);
                                        }

                                        for (int roleCount = 0; roleCount < crc.Count + 1; roleCount++)
                                        {
                                            RowDefinition row = new RowDefinition();
                                            grid1.RowDefinitions.Add(row);
                                        }

                                        for (int k = 0; k < str.Length; k++)
                                        {
                                            System.Windows.Controls.Label l = new System.Windows.Controls.Label();
                                            l.Content = str[k];
                                            l.Background = System.Windows.Media.Brushes.Snow;
                                            grid1.Children.Add(l);
                                            Grid.SetColumn(l, k + 1);
                                            Grid.SetRow(l, 0);
                                        }

                                        for (int r = 0; r < crc.Count; r++)
                                        {
                                            System.Windows.Controls.Label lRow = new System.Windows.Controls.Label();
                                            lRow.Content = crc[r].RoleName;
                                            lRow.Background = System.Windows.Media.Brushes.Beige;
                                            grid1.Children.Add(lRow);
                                            Grid.SetColumn(lRow, 0);
                                            Grid.SetRow(lRow, r + 1);
                                        }

                                        for (int roleCount = 0; roleCount < crc.Count; roleCount++)
                                        {
                                            for (int perCount = 0; perCount < str.Length; perCount++)
                                            {
                                                System.Windows.Controls.CheckBox chk = new System.Windows.Controls.CheckBox();
                                                chk.Margin = new Thickness(5, 5, 0, 0);
                                                chk.Height = 14;
                                                chk.Width = 14;
                                                chk.Tag = str[perCount] + "," + strValues[perCount] + "," + crc[roleCount].ID;
                                                chk.BorderThickness = new Thickness(2, 2, 2, 2);
                                                grid1.Children.Add(chk);
                                                Grid.SetColumn(chk, perCount + 1);
                                                Grid.SetRow(chk, roleCount + 1);
                                            }
                                        }
                                        System.Windows.Controls.Label lblPermissions = new System.Windows.Controls.Label();
                                        lblPermissions.Height = 23;
                                        lblPermissions.Width = 80;
                                        lblPermissions.Content = "Permissions";
                                        grid1.Children.Add(lblPermissions);
                                        Grid.SetColumn(lblPermissions, 0);
                                        Grid.SetRow(lblPermissions, 0);

                                    }
                                }
                            }
                        }
                    }
                }
                al.Clear();
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "btnUpload_Click()--1", "Controls\\CtlUploadModule.xaml.cs");
            }
        }

        void cmbAssembly_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (cmbAssembly.Items.Count > 0)
                {
                    for (int i = 0; i < lstAss.Count; i++)
                    {
                        if (lstAss[i].ManifestModule.ToString() == ((ComboBoxItem)cmbAssembly.SelectedItem).Content.ToString())
                        {
                            Type[] typeArr = lstAss[i].GetTypes();

                            for (int j = 0; j < typeArr.Length; j++)
                            {
                                if (typeArr[j].BaseType != null)
                                {
                                    if (typeArr[j].BaseType.Name.ToLower() == "usercontrol")
                                    {
                                        ComboBoxItem cbiNewClass = new ComboBoxItem();
                                        cbiNewClass.Content = typeArr[j].Name;
                                        cmbClass.Items.Add(cbiNewClass);                                      
                                    }
                                }
                            }
                        }
                    }
                }
                lstAss.Clear();
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "cmbAssembly_SelectionChanged", "Controls\\CtlUploadModule.xaml.cs");
            }
        }

        public void ShowDirectory(DirectoryInfo dir)
        {
            try
            {
                foreach (FileInfo file in dir.GetFiles("*.dll"))
                {
                    int hj = al.Add(file.FullName);
                }
                foreach (DirectoryInfo subDir in dir.GetDirectories())
                {
                    ShowDirectory(subDir);
                }
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "ShowDirectory()", "Controls\\CtlUploadModule.xaml.cs");
            }
        }

        public byte[] SetImage(string ImagePath)
        {
            try
            {
                if (ImagePath != null)
                {
                   
                    FileStream fs = new FileStream(ImagePath, FileMode.Open,FileAccess.ReadWrite,FileShare.ReadWrite);
                    byte[] objbyte = new byte[fs.Length];
                    fs.Read(objbyte, 0 , objbyte.Length);
                    fs.Close();
                    return objbyte;
                   
                }
                else
                { return null; }
            }
            catch
            {
                return null;
            }
        }

        #region version validator

        //private bool IsVersionValid()
        //{
        //    char[] delimiter = { '1', '2', '3', '4', '5', '6', '7', '8', '9', '0' };//only numbers are allowed
        //    if (txtVersion.Text.Split(delimiter).Length != txtVersion.Text.Length + 1)
        //    {
        //        return false;
        //    }
        //    return true;
        //}

        private bool IsVersionValid()
        {
            try
            {
                Regex objValidEx = new Regex("[1-9][0-9]*[.][0-9]*[.][0-9]*[.][0-9]*$");
                if (objValidEx.IsMatch(txtVersion.Text))
                {
                    return true;
                }
                else
                {
                    return false;
                }
                
                //string[] str = txtVersion.Text.Split('.');
                //if (str.Length == 4)
                //{
                //    for (int i = 0; i < 4; i++)
                //    {
                //        if (!IsPositivInteger(str[i]))
                //        {
                //            return false;
                //        }
                //    }
                //    return true;
                //}
                //else
                //{
                //    return false;
                //}
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "IsversionValid()", "Controls\\CtlUploadModule.xaml.cs");                
                return false;
            }
        }

        public static bool IsPositivInteger(String strNumber)
        {
            try
            {
                Regex objNotNaturalPattern = new Regex("[^0-9]");
                Regex objNaturalPattern = new Regex("0*[1-9][0-9]*");

                return !objNotNaturalPattern.IsMatch(strNumber) &&
                        objNaturalPattern.IsMatch(strNumber);
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "IsPositiveInteger()", "Controls\\CtlUploadModule.xaml.cs");                
                return false;
            }
        }

        #endregion
        
        #region IDisposable Members

        public void Dispose()
        {
            try
            {
            if (objModulePermissions != null)
            {
                objModulePermissions = null;
            }
            if (ofd != null)
            {
                ofd = null;
            }
            if (ass != null)
            {
                ass = null;
            }
            if (a != null)
            {
                a = null;
            }
            if (di != null)
            {
                di = null;
            }
            if (al != null)
            {
                al = null;
            }
            if (typecontrol != null)
            {
                typecontrol = null;
            }
            if (strValues != null)
            {
                strValues = null;
            }
            if (lstAss != null)
            {
                lstAss = null;
            }
        }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "Dispose()", "Controls\\CtlUploadModule.xaml.cs");
            }
        }

        #endregion

        ~CtlUploadModule()
        {
            try
            {
            Dispose();
        }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "~ctlUploadModule()", "Controls\\CtlUploadModule.xaml.cs");
            }
        }

    }
}
