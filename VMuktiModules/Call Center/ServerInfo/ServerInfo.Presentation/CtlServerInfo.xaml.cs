<<<<<<< HEAD:VMuktiModules/Call Center/ServerInfo/ServerInfo.Presentation/CtlServerInfo.xaml.cs
﻿/* VMukti 2.0 -- An Open Source Video Communications Suite
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
=======
﻿using System;
>>>>>>> b74131bacb80d82c79711cf70fe93af3fb611b40:VMuktiModules/Call Center/ServerInfo/ServerInfo.Presentation/CtlServerInfo.xaml.cs
using System.Collections;
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
using ServerInfo.Business;
using System.Data;
using System.Reflection;
using System.Text.RegularExpressions;
using VMuktiAPI;
namespace ServerInfo.Presentation
{
    public enum ModulePermissions
    {
        Add = 0,
        Edit = 1,
        Delete = 2,
        View = 3
    }


    public partial class CtlServerInfo : System.Windows.Controls.UserControl
    {
        DataTable dtServerInfo;
        ClsServerInfoCollection objServerInfoCollection;
        ModulePermissions[] _MyPermissions;
        Int64 varID = -1;
        int varState = 0;

        public CtlServerInfo(ModulePermissions[] MyPermissions)
        {
            try
            {
                InitializeComponent();
                _MyPermissions = MyPermissions;
                FncPermissionsReview();
                funSetGrid();
                funSetCombo();
                varState = 0;
                btnSave.Click += new RoutedEventHandler(btnSave_Click);
                btnCancel.Click += new RoutedEventHandler(btnCancel_Click);
                dtpAddedDate.Value = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
                //txtServerIP.KeyDown += new KeyEventHandler(txtServerIP_KeyDown);
                txtServerIP.TextChanged += new TextChangedEventHandler(txtServerIP_TextChanged);
                txtServerPortNumber.TextChanged += new TextChangedEventHandler(txtServerPortNumber_TextChanged);
                //txtServerPortNumber.KeyDown += new KeyEventHandler(txtServerPortNumber_KeyDown);

                this.Loaded += new RoutedEventHandler(CtlServerInfo_Loaded);
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "CtlServerInfo()", "CtlServerInfo.xaml.cs");
            }
        }

        void CtlServerInfo_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                this.Width = ((Grid)this.Parent).ActualWidth;
                ((Grid)this.Parent).SizeChanged += new SizeChangedEventHandler(CtlServerInfo_SizeChanged);
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "CtlServerInfo_Loaded", "CtlServerInfo.xaml.cs");
            }
        }

        void CtlServerInfo_SizeChanged(object sender, SizeChangedEventArgs e)
        {
              try
            {
                this.Width = e.NewSize.Width - 5;
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "CtlServerInfo_SizeChanged", "CtlServerInfo.xaml.cs");
            }
        }

        void txtServerIP_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                string pattern = @"^[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}$";

                System.Text.RegularExpressions.Match match =
                    Regex.Match(txtServerIP.Text.Trim(), pattern, RegexOptions.IgnoreCase);

                if (match.Success)
                {
                    txtServerIP.BorderThickness = new Thickness(0, 0, 0, 0);
                    txtServerIP.BorderBrush = Brushes.Transparent;
                }
                else
                {
                    txtServerIP.BorderThickness = new Thickness(1, 1, 1, 1);
                    txtServerIP.BorderBrush = Brushes.Red;
                }
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "txtServerIP_TextChanged", "CtlServerInfo.xaml.cs");
            }
        }

        void txtServerPortNumber_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                IsValidValue(ref sender);
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "txtServerPortNumber_TextChanged", "CtlServerInfo.xaml.cs");
            }
        }

        void IsValidValue(ref object obj)
        {
            try
            {
                if (obj.GetType().ToString().ToUpper() == "SYSTEM.WINDOWS.CONTROLS.TEXTBOX")
                {
                    if (((TextBox)obj).Text != "" && !IsPositiveInt(((TextBox)obj).Text))
                    {
                        ((TextBox)obj).BorderThickness = new Thickness(1, 1, 1, 1);
                        ((TextBox)obj).BorderBrush = Brushes.Red;
                    }
                    else
                    {
                        ((TextBox)obj).BorderThickness = new Thickness(0, 0, 0, 0);
                        ((TextBox)obj).BorderBrush = Brushes.Transparent;
                    }
                }

                else if (obj.GetType().ToString().ToUpper() == "SYSTEM.WINDOWS.CONTROLS.PASSWORDBOX")
                {
                    if (((PasswordBox)obj).Password != "" && !IsPositiveInt(((PasswordBox)obj).Password))
                    {
                        ((PasswordBox)obj).BorderThickness = new Thickness(1, 1, 1, 1);
                        ((PasswordBox)obj).BorderBrush = Brushes.Red;
                    }
                    else
                    {
                        ((PasswordBox)obj).BorderThickness = new Thickness(0, 0, 0, 0);
                        ((PasswordBox)obj).BorderBrush = Brushes.Transparent;
                    }
                }
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "IsValidValue()", "CtlServerInfo.xaml.cs");
            }
        }

        internal static bool IsFloat(object ObjectToTest)
        {
            try
            {
                if (ObjectToTest == null)
                {
                    return false;
                }
                else
                {
                    double OutValue;
                    return double.TryParse(ObjectToTest.ToString().Trim(), System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.CurrentCulture, out OutValue);
                }
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "IsFloat()", "CtlServerInfo.xaml.cs");
                return false;
            }
        }

        internal static bool IsMixInt(object ObjectToTest)
        {
            try
            {
                if (ObjectToTest == null)
                {
                    return false;
                }
                else
                {
                    int OutValue;
                    if (ObjectToTest.ToString().IndexOf(".") >= 0 || ObjectToTest.ToString().IndexOf("+") > 0 || ObjectToTest.ToString().IndexOf("-") > 0 || ObjectToTest.ToString().IndexOf("$") >= 0 || ObjectToTest.ToString().IndexOf(" ") >= 0)
                    {
                        return false;
                    }
                    else
                    {
                        return int.TryParse(ObjectToTest.ToString().Trim(), System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.CurrentCulture, out OutValue);
                    }
                }
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "IsMixInt()", "CtlServerInfo.xaml.cs");
                return false;
            }
        }

        internal static bool IsPositiveInt(object ObjectToTest)
        {
            try
            {
                if (ObjectToTest == null)
                {
                    return false;
                }
                else
                {
                    int OutValue;
                    if (ObjectToTest.ToString().IndexOf(".") >= 0 || ObjectToTest.ToString().IndexOf("+") >= 0 || ObjectToTest.ToString().IndexOf("-") >= 0 || ObjectToTest.ToString().IndexOf("$") >= 0 || ObjectToTest.ToString().IndexOf(" ") >= 0)
                    {
                        return false;
                    }
                    else
                    {
                        return int.TryParse(ObjectToTest.ToString().Trim(), System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.CurrentCulture, out OutValue);
                    }
                }
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "IsPositiveInt()", "CtlServerInfo.xaml.cs");
                return false;
            }
        }

        internal static bool IsNegativeInt(object ObjectToTest)
        {
            try
            {
                if (ObjectToTest == null)
                {
                    return false;
                }
                else
                {
                    int OutValue;
                    if (ObjectToTest.ToString().IndexOf(".") >= 0 || ObjectToTest.ToString().IndexOf("+") >= 0 || ObjectToTest.ToString().IndexOf("-") > 0 || ObjectToTest.ToString().IndexOf("$") >= 0 || ObjectToTest.ToString().IndexOf(" ") >= 0)
                    {
                        return false;
                    }
                    else
                    {
                        return int.TryParse(ObjectToTest.ToString().Trim(), System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.CurrentCulture, out OutValue);
                    }
                }
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "IsNegativeInt()", "CtlServerInfo.xaml.cs");
                return false;
            }
        }

        bool FncCheckCanvas(Canvas cnvCheck)
        {
            try
            {
                foreach (object o in cnvCheck.Children)
                {
                    if (o.GetType().ToString().ToUpper() == "SYSTEM.WINDOWS.CONTROLS.TEXTBOX")
                    {
                        if (((TextBox)o).BorderBrush == Brushes.Red)
                        {
                            return false;
                        }
                    }
                    else if (o.GetType().ToString().ToUpper() == "SYSTEM.WINDOWS.CONTROLS.PASSWORDBOX")
                    {
                        if (((PasswordBox)o).BorderBrush == Brushes.Red)
                        {
                            return false;
                        }
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "FncCheckCanvas()", "CtlServerInfo.xaml.cs");
                return false;
            }
        }

        void FncPermissionsReview()
        {
            try
            {
                CtlGrid.CanEdit = false;
                CtlGrid.CanDelete = false;
                CtlGrid.Visibility = Visibility.Collapsed;

                tbcServerInfo.Visibility = Visibility.Collapsed;
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
                        tbcServerInfo.Visibility = Visibility.Visible;
                        btnSave.Visibility = Visibility.Visible;
                        btnCancel.Visibility = Visibility.Visible;
                    }
                }
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "FncPermissionsReview()", "CtlServerInfo.xaml.cs");
               
            }
        }

      

        void txtServerIP_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                #region Allowing only numbers , '.'(only 1) and Tab

                if (!((e.Key >= Key.D0 && e.Key <= Key.D9) ||
                    (e.Key == Key.OemPeriod || e.Key == Key.Decimal) ||
                    e.Key == Key.Tab ||
                     (e.Key >= Key.NumPad0 && e.Key <= Key.NumPad9)))
                {
                    e.Handled = true;
                }

                #endregion
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "txtServerIP_KeyDown", "CtlServerInfo.xaml.cs");
            }
        }


        void CtlGrid_btnEditClicked(int RowID)
        {
            try
            {
                varState = 1;
                varID = Convert.ToInt64(objServerInfoCollection[RowID].ID);

                tbcServerInfo.Visibility = Visibility.Visible;
                btnSave.Visibility = Visibility.Visible;
                btnCancel.Visibility = Visibility.Visible;


                txtServerName.Text = objServerInfoCollection[RowID].ServerName;
                txtServerIP.Text = objServerInfoCollection[RowID].IPAddress;
                txtLocation.Text = objServerInfoCollection[RowID].Location;
                txtAddedDate.Text = objServerInfoCollection[RowID].AddedDate.ToString();
                txtServerPortNumber.Text = objServerInfoCollection[RowID].PortNumber.ToString();
                txtServerUserName.Text = objServerInfoCollection[RowID].ServerUserName;
                txtServerPassWord.Password = objServerInfoCollection[RowID].ServerPassWord;
                dtpAddedDate.Value = objServerInfoCollection[RowID].AddedDate;

                for (int i = 0; i < CmbAddedBy.Items.Count; i++)
                {
                    if (((ListBoxItem)CmbAddedBy.Items[i]).Tag.ToString() == objServerInfoCollection[RowID].AddedBy.ToString())
                        CmbAddedBy.Text = ((ListBoxItem)CmbAddedBy.Items[i]).Content.ToString();
                }
                CtlGrid.IsEnabled = false;
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "CtlGrid_btnEditClicked", "CtlServerInfo.xaml.cs");
            }
        }

        void CtlGrid_btnDeleteClicked(int RowID)
        {
            try
            {
                varID = Convert.ToInt64(objServerInfoCollection[RowID].ID);
                if (System.Windows.MessageBox.Show("Do You Really Want To Delete This Record ?", "->Delete ServerInfo", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    ClsServerInfo.Delete(varID);
                    System.Windows.MessageBox.Show("Record Deleted!!", "->Delete ServerInfo", MessageBoxButton.OK, MessageBoxImage.Information);
                    funSetGrid();
                }
                funClearTextBoxes();
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "CtlGrid_btnDeleteClicked", "CtlServerInfo.xaml.cs");
            }
        }

        void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                funClearTextBoxes();
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "btnCancel_Click", "CtlServerInfo.xaml.cs");
            }
        }

        void btnSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                #region Validations

                if (txtServerName.Text.Trim() == "")
                {
                    MessageBox.Show("Server Name Can't Be Blank", "-> Please Enter Server Name", MessageBoxButton.OK, MessageBoxImage.Information);
                    txtServerName.Focus();
                    txtServerName.Text = txtServerName.Text.Trim();
                    return;
                }

                if (txtServerIP.Text.Trim() == "")
                {
                    MessageBox.Show("Server IP Can't Be Blank", "-> Please Enter Server IP", MessageBoxButton.OK, MessageBoxImage.Information);
                    txtServerIP.Focus();
                    txtServerIP.Text = txtServerIP.Text.Trim();
                    return;
                }

                if (txtServerUserName.Text.Trim() == "")
                {
                    MessageBox.Show("Server User Name Can't Be Blank", "-> Please Enter Server User Name", MessageBoxButton.OK, MessageBoxImage.Information);
                    txtServerUserName.Focus();
                    txtServerUserName.Text = txtServerUserName.Text.Trim();
                    return;
                }

                if (txtServerPassWord.Password.Trim() == "")
                {
                    MessageBox.Show("Server Password Can't Be Blank", "-> Please Enter Server Password", MessageBoxButton.OK, MessageBoxImage.Information);
                    txtServerPassWord.Focus();
                    txtServerPassWord.Password = txtServerPassWord.Password.Trim();
                    return;
                }

                if (CmbAddedBy.Text.Trim() == "")
                {
                    MessageBox.Show("Added By Can't Be Blank", "-> Please Enter Added By", MessageBoxButton.OK, MessageBoxImage.Information);
                    CmbAddedBy.Focus();
                    return;
                }

                if (txtServerPortNumber.Text.Trim() == "")
                {
                    txtServerPortNumber.Text = "0";
                }

                bool b = FncCheckCanvas(cnvMain);

                if (b == false)
                {
                    MessageBox.Show("Please Enter Valid Data In Indicated Fields!!", "-> SIP Server", MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }

                #endregion

                ClsServerInfo objServer = new ClsServerInfo();
                if (varState == 0)
                    objServer.ID = -1;
                else
                    objServer.ID = varID;

                objServer.ServerName = txtServerName.Text.Trim();
                objServer.IPAddress = txtServerIP.Text.Trim();
                objServer.Location = txtLocation.Text.Trim();
                objServer.AddedBy = int.Parse(((ListBoxItem)CmbAddedBy.SelectedItem).Tag.ToString());
                objServer.AddedDate = Convert.ToDateTime(dtpAddedDate.Value.ToString());
                objServer.CreatedBy = VMuktiAPI.VMuktiInfo.CurrentPeer.ID;
                if (txtServerPortNumber.Text.Trim() != "")
                {
                    objServer.PortNumber = int.Parse(txtServerPortNumber.Text.Trim());
                }
                objServer.ServerUserName = txtServerUserName.Text.Trim();
                objServer.ServerPassWord = txtServerPassWord.Password.Trim();
                Int64 gID = objServer.Save();

                if (gID == 0)
                {
                    MessageBox.Show("Duplicate Server Name Is Not Allowed !!", "-> Server Name", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    MessageBox.Show("Record Saved Successfully!!");
                    funClearTextBoxes();
                    funSetGrid();
                    funSetCombo();
                }
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "btnSave_Click", "CtlServerInfo.xaml.cs");
            }
        }

     
        public void funSetGrid()
        {
            try
            {
                CtlGrid.Cols = 8;
                //CtlGrid.CanEdit = true;
                //CtlGrid.CanDelete = true;

                CtlGrid.Columns[0].Header = "ID";
                CtlGrid.Columns[1].Header = "ServerName";
                CtlGrid.Columns[2].Header = "ServerIP";
                CtlGrid.Columns[3].Header = "Location";
                CtlGrid.Columns[4].Header = "ServerUserName";
                CtlGrid.Columns[5].Header = "ServerPassword";
                CtlGrid.Columns[6].Header = "PortNumber";
                CtlGrid.Columns[7].Header = "AddedDate";
                //CtlGrid.Columns[8].Header = "AddedBy";
                //CtlGrid.Columns[9].Header = "CreatedBy";

                CtlGrid.Columns[0].BindTo("ID");
                CtlGrid.Columns[1].BindTo("ServerName");
                CtlGrid.Columns[2].BindTo("IPAddress");
                CtlGrid.Columns[3].BindTo("Location");
                CtlGrid.Columns[4].BindTo("ServerUserName");
                CtlGrid.Columns[5].BindTo("ServerPassWord");
                CtlGrid.Columns[6].BindTo("PortNumber");
                CtlGrid.Columns[7].BindTo("AddedDate");
                //CtlGrid.Columns[8].BindTo("AddedBy");
                //CtlGrid.Columns[9].BindTo("CreatedBy");

                objServerInfoCollection = ClsServerInfoCollection.GetAll();
                CtlGrid.Bind(objServerInfoCollection);
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "funSetGrid()", "CtlServerInfo.xaml.cs");
            }
        }

        public void funSetCombo()
        {
            try
            {
                // No Roles So Initially Displaying //
                CmbAddedBy.Items.Clear();

                ClsUserCollection objUser = ClsUserCollection.GetAll();

                for (int i = 0; i < objUser.Count; i++)
                {
                    ComboBoxItem cbiUser = new ComboBoxItem();
                    cbiUser.Tag = objUser[i].ID;
                    cbiUser.Content = objUser[i].UserName;
                    CmbAddedBy.Items.Add(cbiUser);
                }
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "funSetCombo()", "CtlServerInfo.xaml.cs");
            }
        }

        public void funClearTextBoxes()
        {
            try
            {
                varState = 0;
                varID = -1;
                txtServerName.Text = "";
                txtServerIP.Text = "";
                txtLocation.Text = "";
                CmbAddedBy.Items.Clear();
                txtAddedDate.Text = "";
                txtServerPortNumber.Text = "";
                txtServerUserName.Text = "";
                txtServerPassWord.Password = "";
                dtpAddedDate.Value = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
                CtlGrid.IsEnabled = true;
                FncPermissionsReview();
                funSetCombo();
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "funClearTextBoxes()", "CtlServerInfo.xaml.cs");
            }
        }
    }
}
