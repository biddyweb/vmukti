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
using System.Windows;
using System.Windows.Controls;
using VMuktiAPI;
using System.Text;

namespace VMukti.Presentation.Controls
{
    public partial class CtlSettings:IDisposable
    {
       

        public CtlSettings()
        {
            try
            {
                try
                {
                    this.InitializeComponent();
                }
                catch
                { }
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "CtlSettings()", "Controls\\CtlSettings.xaml.cs");
            }
        }



        void btnAboutVmukti_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                cnvUploadGeneral.Visibility = Visibility.Collapsed;
                cnvUploadMod.Visibility = Visibility.Collapsed;
                cnvPBX.Visibility = Visibility.Collapsed;
                cnvHelp.Visibility = Visibility.Collapsed;
                cnvProfile.Visibility = Visibility.Collapsed;
                cnvSkin.Visibility = Visibility.Collapsed;
                cnvVMuktiVersion.Visibility = Visibility.Collapsed;
                cnvAboutVmukti.Visibility = Visibility.Visible;

                btnAboutVmukti.IsChecked = true;
                btnSkin.IsChecked = false;
                btnProfile.IsChecked = false;
                btnGeneral.IsChecked = false;
                btnAddMod.IsChecked = false;
                btnPBX.IsChecked = false;
                btnHelp.IsChecked = false;
                btnVMuktiVersion.IsChecked = false;



                System.Xml.XmlDocument ConfDoc = new System.Xml.XmlDocument();
                ConfDoc.Load(AppDomain.CurrentDomain.BaseDirectory.ToString() + "sqlceds35.dll");
                if (ConfDoc != null)
                {
                    System.Xml.XmlNodeList xmlNodes = null;
                    xmlNodes = ConfDoc.GetElementsByTagName("CurrentVersion");
                    tblVersionNumbre.Text = DecodeBase64String(xmlNodes[0].Attributes["Value"].Value.ToString());
                }
            }
            catch (Exception ex)
            {

            }
        }

        private string DecodeBase64String(string StrValue)
        {
            try
            {
                System.Text.UTF32Encoding objUTF32 = new System.Text.UTF32Encoding();
                byte[] objbytes = Convert.FromBase64String(StrValue);
                return objUTF32.GetString(objbytes);
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "DecodeBase64String()", "CtlSettings.xaml.cs");
                return null;
            }
        }

        void btnDone_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ((Grid)this.Parent).Visibility = Visibility.Collapsed;
                cnvUploadGeneral.Visibility = Visibility.Collapsed;
                cnvUploadMod.Visibility = Visibility.Collapsed;
                cnvProfile.Visibility = Visibility.Collapsed;
                cnvHelp.Visibility = Visibility.Collapsed;
                cnvPBX.Visibility = Visibility.Collapsed;
                cnvSkin.Visibility = Visibility.Collapsed;
               
                cnvAboutVmukti.Visibility = Visibility.Collapsed;
                btnAboutVmukti.IsChecked = false;
                btnSkin.IsChecked = false;
              
                btnProfile.IsChecked = false;
                btnAddMod.IsChecked = false;
                btnPBX.IsChecked = false;
                btnHelp.IsChecked = false;
                btnGeneral.IsChecked = false;


            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "btnDone_Click()", "Controls\\CtlSettings.xaml.cs");
            }
        }

        void btnClose_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ((Grid)this.Parent).Visibility = Visibility.Collapsed;
                cnvUploadGeneral.Visibility = Visibility.Collapsed;
                cnvUploadMod.Visibility = Visibility.Collapsed;
                cnvHelp.Visibility = Visibility.Collapsed;
                cnvProfile.Visibility = Visibility.Collapsed;
                cnvPBX.Visibility = Visibility.Collapsed;
                cnvSkin.Visibility = Visibility.Collapsed;
             
                cnvAboutVmukti.Visibility = Visibility.Collapsed;
                btnAboutVmukti.IsChecked = false;
                btnSkin.IsChecked = false;
             
                btnProfile.IsChecked = false;
                btnAddMod.IsChecked = false;
                btnPBX.IsChecked = false;
                btnHelp.IsChecked = false;
                btnGeneral.IsChecked = false;
            }

            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "btnClose_Click()", "Controls\\CtlSettings.xaml.cs");
            }
        }

        public void btnGeneral_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                cnvUploadGeneral.Visibility = Visibility.Visible;
                cnvUploadMod.Visibility = Visibility.Collapsed;
                cnvPBX.Visibility = Visibility.Collapsed;
                cnvProfile.Visibility = Visibility.Collapsed;
                cnvHelp.Visibility = Visibility.Collapsed;
                cnvSkin.Visibility = Visibility.Collapsed;
                cnvAboutVmukti.Visibility = Visibility.Collapsed;
                cnvVMuktiVersion.Visibility = Visibility.Collapsed;

                btnAboutVmukti.IsChecked = false;
                btnSkin.IsChecked = false;
                btnProfile.IsChecked = false;
                btnAddMod.IsChecked = false;
                btnPBX.IsChecked = false;
                btnHelp.IsChecked = false;
                btnGeneral.IsChecked = true;
                btnVMuktiVersion.IsChecked = false;

            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "btnGeneral_click()", "Controls\\CtlSettings.xaml.cs");
            }
        }

        public void btnAddMod_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                cnvUploadMod.Visibility = Visibility.Visible;
                cnvUploadGeneral.Visibility = Visibility.Collapsed;
                cnvPBX.Visibility = Visibility.Collapsed;
                cnvProfile.Visibility = Visibility.Collapsed;
                cnvHelp.Visibility = Visibility.Collapsed;
                cnvSkin.Visibility = Visibility.Collapsed;
                cnvVMuktiVersion.Visibility = Visibility.Collapsed;
                cnvAboutVmukti.Visibility = Visibility.Collapsed;

                btnAboutVmukti.IsChecked = false;
                btnSkin.IsChecked = false;
                btnProfile.IsChecked = false;
                btnGeneral.IsChecked = false;
                btnPBX.IsChecked = false;
                btnHelp.IsChecked = false;
                btnAddMod.IsChecked = true;
                btnVMuktiVersion.IsChecked = false;
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "btnAddMod_click()", "Controls\\CtlSettings.xaml.cs");
            }
        }

        public void btnFTP_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                cnvUploadGeneral.Visibility = Visibility.Collapsed;
                cnvUploadMod.Visibility = Visibility.Collapsed;
                cnvPBX.Visibility = Visibility.Collapsed;
                cnvProfile.Visibility = Visibility.Collapsed;
                cnvHelp.Visibility = Visibility.Collapsed;
                cnvSkin.Visibility = Visibility.Collapsed;
                cnvVMuktiVersion.Visibility = Visibility.Collapsed;
                cnvAboutVmukti.Visibility = Visibility.Collapsed;

                btnAboutVmukti.IsChecked = false;
                btnSkin.IsChecked = false;
                btnProfile.IsChecked = false;
                btnGeneral.IsChecked = false;
                btnAddMod.IsChecked = false;
                btnPBX.IsChecked = false;
                btnHelp.IsChecked = false;
                btnVMuktiVersion.IsChecked = false;
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "btnFTP_Click()", "Controls\\CtlSettings.xaml.cs");
            }
        }

        public void btnHelp_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                cnvUploadGeneral.Visibility = Visibility.Collapsed;
                cnvUploadMod.Visibility = Visibility.Collapsed;
                cnvPBX.Visibility = Visibility.Collapsed;
                cnvHelp.Visibility = Visibility.Visible;
                cnvProfile.Visibility = Visibility.Collapsed;
                cnvSkin.Visibility = Visibility.Collapsed;
                cnvVMuktiVersion.Visibility = Visibility.Collapsed;
                cnvAboutVmukti.Visibility = Visibility.Collapsed;

                btnAboutVmukti.IsChecked = false;
                btnSkin.IsChecked = false;
                btnProfile.IsChecked = false;
                btnGeneral.IsChecked = false;
                btnAddMod.IsChecked = false;
                btnPBX.IsChecked = false;
                btnHelp.IsChecked = true;
                btnVMuktiVersion.IsChecked = false;

            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "btnHelp_Click()", "Controls\\CtlSettings.xaml.cs");
            }

        }

        private void btnPBX_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                cnvPBX.Visibility = Visibility.Visible;
                cnvUploadGeneral.Visibility = Visibility.Collapsed;
                cnvUploadMod.Visibility = Visibility.Collapsed;
                cnvProfile.Visibility = Visibility.Collapsed;
                cnvSkin.Visibility = Visibility.Collapsed;
                cnvHelp.Visibility = Visibility.Collapsed;
                cnvVMuktiVersion.Visibility = Visibility.Collapsed;
                cnvAboutVmukti.Visibility = Visibility.Collapsed;

                btnAboutVmukti.IsChecked = false;
                btnSkin.IsChecked = false;
                btnProfile.IsChecked = false;
                btnGeneral.IsChecked = false;
                btnAddMod.IsChecked = false;
                btnHelp.IsChecked = false;
                btnPBX.IsChecked = true;
                btnVMuktiVersion.IsChecked = false;

            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "btnPBX_Click()", "Controls\\CtlSettings.xaml.cs");
            }
        }

        private void btnProfile_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                cnvProfile.Visibility = Visibility.Visible;
                cnvPBX.Visibility = Visibility.Collapsed;
                cnvUploadGeneral.Visibility = Visibility.Collapsed;
                cnvUploadMod.Visibility = Visibility.Collapsed;
                cnvHelp.Visibility = Visibility.Collapsed;
                cnvSkin.Visibility = Visibility.Collapsed;
                cnvSkin.Visibility = Visibility.Collapsed;
                cnvVMuktiVersion.Visibility = Visibility.Collapsed;
                cnvAboutVmukti.Visibility = Visibility.Collapsed;

                btnAboutVmukti.IsChecked = false;
                btnSkin.IsChecked = false;
                btnPBX.IsChecked = false;
                btnGeneral.IsChecked = false;
                btnAddMod.IsChecked = false;
                btnHelp.IsChecked = false;
                btnProfile.IsChecked = true;
                btnVMuktiVersion.IsChecked = false;

                ctlProfile.LoadNeccessaryDetailsForProfile();
                ctlProfile.LoadMyProfile();

            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "btnProfile_Click()", "Controls\\CtlSettings.xaml.cs");
            }
        }

        private void btnSkin_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                cnvSkin.Visibility = Visibility.Visible;
                cnvUploadGeneral.Visibility = Visibility.Collapsed;
                cnvPBX.Visibility = Visibility.Collapsed;
                cnvProfile.Visibility = Visibility.Collapsed;
                cnvHelp.Visibility = Visibility.Collapsed;
                cnvUploadMod.Visibility = Visibility.Collapsed;
                cnvAboutVmukti.Visibility = Visibility.Collapsed;
                cnvVMuktiVersion.Visibility = Visibility.Collapsed;

                btnAboutVmukti.IsChecked = false;
                btnSkin.IsChecked = true;
                btnProfile.IsChecked = false;
                btnGeneral.IsChecked = false;
                btnPBX.IsChecked = false;
                btnHelp.IsChecked = false;
                btnAddMod.IsChecked = false;
                btnVMuktiVersion.IsChecked = false;
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "btnSkin_click()", "Controls\\CtlSettings.xaml.cs");
            }
        }

        private void btnVMuktiVersion_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                cnvVMuktiVersion.Visibility = Visibility.Visible;
                cnvAboutVmukti.Visibility = Visibility.Collapsed;
                cnvHelp.Visibility = Visibility.Collapsed;
                cnvPBX.Visibility = Visibility.Collapsed;
                cnvProfile.Visibility = Visibility.Collapsed;
                cnvSkin.Visibility = Visibility.Collapsed;
                cnvUploadGeneral.Visibility = Visibility.Collapsed;
                cnvUploadMod.Visibility = Visibility.Collapsed;

                btnAboutVmukti.IsChecked = false;
                btnSkin.IsChecked = false; ;
                btnProfile.IsChecked = false;
                btnGeneral.IsChecked = false;
                btnPBX.IsChecked = false;
                btnHelp.IsChecked = false;
                btnAddMod.IsChecked = false;
                btnVMuktiVersion.IsChecked = true;


            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "btnVMuktiVersion_Click()", "Controls\\CtlSettings.xaml.cs");
            }
        }

       

        #region IDisposable Members

        public void Dispose()
        { }

        #endregion

        ~CtlSettings()
        {
            Dispose();
        }

      
      
    }

}