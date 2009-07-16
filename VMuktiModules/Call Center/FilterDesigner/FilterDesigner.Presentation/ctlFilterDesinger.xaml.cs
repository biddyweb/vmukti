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
using FilterDesigner.Business;
using VMuktiAPI;

namespace FilterDesigner.Presentation
{
    /// <summary>
    /// Interaction logic for UserControl1.xaml
    /// </summary>
    public enum ModulePermissions
     {

        Add = 0,
        Edit = 1,
        Delete = 2,
        View = 3
     }     
    public partial class ctlFilterDesinger : UserControl
    {
        //public static StringBuilder sb1;

        
        string selectedValue ="";
        ModulePermissions[] _MyPermissions;

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

        public ctlFilterDesinger(ModulePermissions[] MyPermissions)
        {
            try
            {
                InitializeComponent();
                this.Loaded += new RoutedEventHandler(ctlFilterDesinger_Loaded);    
                _MyPermissions = MyPermissions;

                FncPermissionsReview();
                txtFieldValues.TextWrapping = TextWrapping.Wrap;
                ClsFileFormatCollection objColl = ClsFileFormatCollection.GetAll();
                for (int i = 0; i < objColl.Count; i++)
                {
                    ComboBoxItem cbiFormat = new ComboBoxItem();
                    cbiFormat.Content = objColl[i].LeadFormatName;
                    cbiFormat.Tag = objColl[i].ID;
                    cmbLeadFormat.Items.Add(cbiFormat);

                }
            }
            catch(Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "ctlFilterDesinger()", "ctlFilterDesinger.xaml.cs");
            }
      
        }
        void ctlFilterDesinger_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                this.Width = ((Grid)this.Parent).ActualWidth;
                ((Grid)this.Parent).SizeChanged += new SizeChangedEventHandler(ctlFilterDesinger_SizeChanged);
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "ctlFilterDesinger_Loaded()", "ctlFilterDesinger.xaml.cs");
            }
        }
        void ctlFilterDesinger_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            this.Width = e.NewSize.Width;
        }

        private void btnNext_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                cnvCanavas1.Visibility = Visibility.Hidden;
                cnvCanavas3.Visibility = Visibility.Visible;
                if (lstFill.Items.Count > 0)
                {
                    lstFill.Visibility = Visibility.Visible;
                    txtFieldValues.IsReadOnly = true;
                }
                else
                {
                    lstFill.Visibility = Visibility.Hidden;
                    txtFieldValues.IsReadOnly = false;
                }
            }
            catch(Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "btnNext_Click()", "ctlFilterDesinger.xaml.cs");
            }
        
        }
        void FncPermissionsReview()
        {
            try
            {
                this.Visibility = Visibility.Hidden;

                for (int i = 0; i < _MyPermissions.Length; i++)
                {
                    if (_MyPermissions[i] == ModulePermissions.View || _MyPermissions[i] == ModulePermissions.Edit || _MyPermissions[i] == ModulePermissions.Delete || _MyPermissions[i] == ModulePermissions.Add)
                    {
                        this.Visibility = Visibility.Visible;
                    }
                }
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "FncPermissionsReview()", "ctlFilterDesinger.xaml.cs");
            }
        }
        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                cnvCanavas3.Visibility = Visibility.Hidden;
                cnvCanavas1.Visibility = Visibility.Visible;
                lstFill.Visibility = Visibility.Hidden;
            }
            catch(Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "btnBack_Click()", "ctlFilterDesinger.xaml.cs");
            }
        }

        private void cmbLeadFormat_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                txtFieldValues.IsReadOnly = false;
                lstFill.Items.Clear();
                lstFill.Visibility = Visibility.Hidden;
                cmbFieldName.Items.Clear();
                ClsFormatFieldCollection objFormat = ClsFormatFieldCollection.GetAll(int.Parse(((ComboBoxItem)cmbLeadFormat.SelectedItem).Tag.ToString()));
                for (int i = 0; i < objFormat.Count; i++)
                {
                    ComboBoxItem cbiFormat = new ComboBoxItem();
                    cbiFormat.Content = objFormat[i].LeadFieldName;
                    cbiFormat.Tag = objFormat[i].CustomFieldID;
                    cmbFieldName.Items.Add(cbiFormat);

                }

                ClsFormatFieldCollection objFormatOth = ClsFormatFieldCollection.GetOtherDetail();
                for (int i = 0; i < objFormatOth.Count; i++)
                {
                    ComboBoxItem cbiFormat = new ComboBoxItem();
                    cbiFormat.Content = objFormatOth[i].LeadFieldName;
                    cbiFormat.Tag = objFormatOth[i].CustomFieldID;
                    cmbFieldName.Items.Add(cbiFormat);

                }
            }
            catch(Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "cmbLeadFormat_SelectionChanged()", "ctlFilterDesinger.xaml.cs");
            }
    
        }

        private void cmbFieldName_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                txtFieldValues.Text = "";
                if (cmbFieldName.Items.Count > 0)
                {
                    selectedValue = ((ComboBoxItem)cmbFieldName.SelectedItem).Content.ToString();
                    lstFill.SelectionMode = SelectionMode.Extended;

                    if (selectedValue.ToLower() == "timezone")
                    {
                        txtFieldValues.IsReadOnly = true;
                        lstFill.Visibility = Visibility.Visible;
                        lstFill.Items.Clear();
                        ClsTimeZoneCollection clsTimezone = ClsTimeZoneCollection.Timezone_GetAll();
                        for (int i = 0; i < clsTimezone.Count; i++)
                        {
                            ListBoxItem cbiFormat = new ListBoxItem();
                            cbiFormat.Content = clsTimezone[i].TimezoneName;
                            cbiFormat.Tag = clsTimezone[i].ID;
                            lstFill.Items.Add(cbiFormat);
                        }

                    }
                    else if (selectedValue.ToLower() == "country")
                    {
                        txtFieldValues.IsReadOnly = true;
                        lstFill.Visibility = Visibility.Visible;
                        lstFill.Items.Clear();
                        ClsCountryCollection clsCountry = ClsCountryCollection.Country_GetAll();
                        for (int i = 0; i < clsCountry.Count; i++)
                        {
                            ListBoxItem cbiFormat = new ListBoxItem();
                            cbiFormat.Content = clsCountry[i].CountryName;
                            cbiFormat.Tag = clsCountry[i].ID;
                            lstFill.Items.Add(cbiFormat);
                        }

                    }
                    else if (selectedValue.ToLower() == "areacode")
                    {
                        txtFieldValues.IsReadOnly = true;
                        lstFill.Visibility = Visibility.Visible;
                        lstFill.Items.Clear();
                        ClsAreaCodeCollection clsAreaCode = ClsAreaCodeCollection.AreaCode_GetAll();
                        for (int i = 0; i < clsAreaCode.Count; i++)
                        {
                            ListBoxItem cbiFormat = new ListBoxItem();
                            cbiFormat.Content = clsAreaCode[i].AreaCode;
                            cbiFormat.Tag = clsAreaCode[i].ID;
                            lstFill.Items.Add(cbiFormat);
                        }
                    }
                    else if (selectedValue.ToLower() == "state")
                    {
                        txtFieldValues.IsReadOnly = true;
                        lstFill.Visibility = Visibility.Visible;
                        lstFill.Items.Clear();
                        ClsStateCollection clsState = ClsStateCollection.State_GetAll();
                        for (int i = 0; i < clsState.Count; i++)
                        {
                            ListBoxItem cbiFormat = new ListBoxItem();
                            cbiFormat.Content = clsState[i].StateName;
                            cbiFormat.Tag = clsState[i].ID;
                            lstFill.Items.Add(cbiFormat);
                        }
                    }
                    else
                    {
                        txtFieldValues.IsReadOnly = false;
                        lstFill.Items.Clear();
                        lstFill.Visibility = Visibility.Hidden;
                    }
                }
            }
            catch(Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "cmbFieldName_SelectionChanged()", "ctlFilterDesinger.xaml.cs");
            }
        }

        private void lstFill_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                txtFieldValues.Text = "";


                for (int iCnt = 0; iCnt < lstFill.SelectedItems.Count; iCnt++)
                {
                    txtFieldValues.Text = txtFieldValues.Text + "'" + ((ListBoxItem)lstFill.SelectedItems[iCnt]).Content.ToString() + "',";
                }
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "lstFill_SelectionChanged()", "ctlFilterDesinger.xaml.cs");
            }
          
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ClsFileFormat form = new ClsFileFormat();
                string tempStr = "";
                string Ttype = ((ComboBoxItem)cmbTreatmentType.SelectedItem).Content.ToString();
                Int64 LeadFormat = Convert.ToInt64(((ComboBoxItem)cmbLeadFormat.SelectedItem).Tag.ToString());
                Int64 FieldID = Convert.ToInt64(((ComboBoxItem)cmbFieldName.SelectedItem).Tag.ToString());
                string operator1 = ((ComboBoxItem)cmbOperator.SelectedItem).Content.ToString();
                Int64 userid = 1;//Convert.ToInt64(VMuktiAPI.VMuktiInfo.CurrentPeer.ID.ToString());
                tempStr = txtFieldValues.Text.Replace(",", "~").Substring(0, txtFieldValues.Text.Length - 1);
                string str = form.TreatmentSave(txtTreatmentName.Text, txtDescription.Text, Ttype, LeadFormat, FieldID, operator1, tempStr, userid);

            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "btnSave_Click()", "ctlFilterDesinger.xaml.cs");
            }
         }
    }
}

