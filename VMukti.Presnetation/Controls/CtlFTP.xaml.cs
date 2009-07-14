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
using System.Text.RegularExpressions;
using System.Windows;
using VMukti.Business;
using VMuktiAPI;
using System.Text;

namespace VMukti.Presentation.Controls
{
    public partial class CtlFTP
    {
        public static StringBuilder sb1;
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
        public CtlFTP()
        {
            try
            {
                InitializeComponent();
                this.Unloaded += new RoutedEventHandler(CtlFTP_Unloaded);
            }
            catch (Exception ex)
            {
                ex.Data.Add("My Key", "CtlFTP()--:--CtlFTP.xaml.cs--:--" + ex.Message + " :--:--");
                //ClsException.LogError(ex);
                //ClsException.WriteToErrorLogFile(ex);
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

        void CtlFTP_Unloaded(object sender, RoutedEventArgs e)
        {
            try
            {
                ClearData();
            }
            catch (Exception ex)
            {
                ex.Data.Add("My Key", "CtlFTP_Unloaded()--:--CtlFTP.xaml.cs--:--" + ex.Message + " :--:--");
                //ClsException.LogError(ex);
                //ClsException.WriteToErrorLogFile(ex);
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

        private void btnSubmit_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (isModuleFTPValidated() && isRecordingFTPValidated())
                {
                    ClsRecordingModule clsRecording = new ClsRecordingModule();
                    int i = -1;
                    clsRecording.AddNewFTPDetail(ref i, txtServerIP_R.Text.Trim(), txtPortNO_R.Text.Trim(), txtUserName_R.Text.Trim(), txtDirName_R.Text.Trim(), txtPassword_R.Password.Trim(), "Recording");
                    clsRecording.AddNewFTPDetail(ref i, txtServerIP_M.Text.Trim(), txtPortNO_M.Text.Trim(), txtUserName_M.Text.Trim(), txtDirname_M.Text.Trim(), txtPassword_M.Password.Trim(), "Module");
                    ClearData();
                }
            }
            catch (Exception ex)
            {
                ex.Data.Add("My Key", "btnSubmit_Click()--:--CtlFTP.xaml.cs--:--" + ex.Message + " :--:--");
                //ClsException.LogError(ex);
                //ClsException.WriteToErrorLogFile(ex);
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

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                System.Windows.Forms.DialogResult dlr = System.Windows.Forms.MessageBox.Show("Are you want to clear data", "VMukti", System.Windows.Forms.MessageBoxButtons.YesNo, System.Windows.Forms.MessageBoxIcon.Question);
                if (dlr == System.Windows.Forms.DialogResult.Yes)
                {
                    ClearData();
                }
            }
            catch (Exception ex)
            {
                ex.Data.Add("My Key", "btnCancel_Click()--:--CtlFTP.xaml.cs--:--" + ex.Message + " :--:--");
                //ClsException.LogError(ex);
                //ClsException.WriteToErrorLogFile(ex);
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

        void ClearData()
        {
            try
            {
                txtServerIP_M.Text = string.Empty;
                txtPortNO_M.Text = string.Empty;
                txtUserName_M.Text = string.Empty;
                txtDirname_M.Text = string.Empty;
                txtPassword_M.Password = string.Empty;
                txtRePassword_M.Password = string.Empty;


                txtServerIP_R.Text = string.Empty;
                txtPortNO_R.Text = string.Empty;
                txtUserName_R.Text = string.Empty;
                txtDirName_R.Text = string.Empty;
                txtPassword_R.Password = string.Empty;
                txtRePassword_R.Password = string.Empty;
            }
            catch (Exception ex)
            {
                ex.Data.Add("My Key", "ClearData()--:--CtlFTP.xaml.cs--:--" + ex.Message + " :--:--");
                //ClsException.LogError(ex);
                //ClsException.WriteToErrorLogFile(ex);
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
        public bool isModuleFTPValidated()
        {
            try
            {
                if (txtServerIP_M.Text.Trim() == string.Empty)
                {
                    return false;
                }
                else if (!isValidIPAddress(txtServerIP_M.Text.Trim()))
                {
                    return false;
                }
                else if (txtPortNO_M.Text.Trim() == string.Empty)
                {
                    return false;
                }
                else if (txtUserName_M.Text.Trim() == string.Empty)
                {
                    return false;
                }
                else if (txtPortNO_M.Text.Trim() == string.Empty)
                {
                    return false;
                }
                else if (txtDirname_M.Text.Trim() == string.Empty)
                {
                    return false;
                }
                else if (txtPassword_M.Password.Trim() == string.Empty)
                {
                    return false;
                }
                else if (txtPassword_M.Password.Trim() != txtPassword_M.Password.Trim())
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            catch (Exception ex)
            {
                ex.Data.Add("My Key", "isModuleFTPValidated()--:--CtlFTP.xaml.cs--:--" + ex.Message + " :--:--");
                //ClsException.LogError(ex);
                //ClsException.WriteToErrorLogFile(ex);
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
				return false;
            }
        }

        public bool isRecordingFTPValidated()
        {
            try
            {
                if (txtServerIP_R.Text.Trim() == string.Empty)
                {
                    return false;
                }
                else if (!isValidIPAddress(txtServerIP_R.Text.Trim()))
                {
                    return false;
                }
                else if (txtPortNO_R.Text.Trim() == string.Empty)
                {
                    return false;
                }
                else if (txtUserName_R.Text.Trim() == string.Empty)
                {
                    return false;
                }
                else if (txtPortNO_R.Text.Trim() == string.Empty)
                {
                    return false;
                }
                else if (txtDirName_R.Text.Trim() == string.Empty)
                {
                    return false;
                }
                else if (txtPassword_R.Password.Trim() == string.Empty)
                {
                    return false;
                }
                else if (txtPassword_R.Password.Trim() != txtPassword_R.Password.Trim())
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            catch (Exception ex)
            {
                ex.Data.Add("My Key", "isRecordingFTPValidated()--:--CtlFTP.xaml.cs--:--" + ex.Message + " :--:--");
                //ClsException.LogError(ex);
                //ClsException.WriteToErrorLogFile(ex);
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
				return false;
            }
        }

        private bool isValidIPAddress(string strIPAddr)
        {
            try
            {
                if (strIPAddr == string.Empty)
                {
                    return false;
                }
                else
                {
                    string strRegX = @"\b(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\b";
                    Regex reX = new Regex(strRegX);
                    if (reX.IsMatch(strIPAddr))
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            catch (Exception ex)
            {
                ex.Data.Add("My Key", "isValidIPAddress()--:--CtlFTP.xaml.cs--:--" + ex.Message + " :--:--");
                //ClsException.LogError(ex);
                //ClsException.WriteToErrorLogFile(ex);
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
				return false;
            }
        }
    }
}
