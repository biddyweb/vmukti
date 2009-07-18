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
using System.Data;
using SIPUserInfo.Business;
using System.Reflection;
using VMuktiAPI;
namespace SIPUserInfo.Presentation
{
    /// <summary>
    /// Interaction logic for CtlSIPUserInfo.xaml
    /// </summary>
    /// 
    public enum ModulePermissions
    {
        Add = 0,
        Edit = 1,
        Delete = 2,
        View = 3
    }

    public partial class CtlSIPUserInfo : System.Windows.Controls.UserControl
    {
        DataTable dtSIPUserInfo;
        ClsSIPUserInfoCollection objSIPUserInfoCollection;

        ModulePermissions[] _MyPermissions;

        int varID = -1;
        int varState = 0;

        public CtlSIPUserInfo(ModulePermissions[] MyPermissions)
        {
            try
            {
                InitializeComponent();
                this.DataContext = new ClsSIPUserInfo();
                _MyPermissions = MyPermissions;
                FncPermissionsReview();
                funSetGrid();
                btnSave.Click += new RoutedEventHandler(btnSave_Click);
                btnCancel.Click += new RoutedEventHandler(btnCancel_Click);
                txtSIPID.TextChanged += new TextChangedEventHandler(txtSIPID_TextChanged);
                txtSIPPass.PasswordChanged += new RoutedEventHandler(txtSIPPass_PasswordChanged);

                txtSIPID.BorderThickness = new Thickness(1, 1, 1, 1);
                txtSIPID.BorderBrush = Brushes.LightBlue;

                txtSIPPass.BorderThickness = new Thickness(1, 1, 1, 1);
                txtSIPPass.BorderBrush = Brushes.LightBlue;

                this.Loaded += new RoutedEventHandler(CtlSIPUserInfo_Loaded);
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "CtlSIPUserInfo", "CtlSIPUserInfo.xaml.cs");
            }
        }

        void CtlSIPUserInfo_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                this.Width = ((Grid)this.Parent).ActualWidth;
                ((Grid)this.Parent).SizeChanged += new SizeChangedEventHandler(CtlSIPUserInfo_SizeChanged);
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "CtlSIPUserInfo_Loaded", "CtlSIPUserInfo.xaml.cs");
            }
        }

        void CtlSIPUserInfo_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            try
            {
                this.Width = e.NewSize.Width - 5;
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "CtlSIPUserInfo_Loaded", "CtlSIPUserInfo.xaml.cs");
            }
        }

        void txtSIPPass_PasswordChanged(object sender, RoutedEventArgs e)
        {
            try
            {
                IsValidValue(ref sender);
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "txtSIPPass_PasswordChanged", "CtlSIPUserInfo.xaml.cs");
            }
        }

        void txtSIPID_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                IsValidValue(ref sender);
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "txtSIPID_TextChanged", "CtlSIPUserInfo.xaml.cs");
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
                        ((TextBox)obj).BorderThickness = new Thickness(1, 1, 1, 1);
                        ((TextBox)obj).BorderBrush = Brushes.LightBlue;
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
                        ((PasswordBox)obj).BorderThickness = new Thickness(1, 1, 1, 1);
                        ((PasswordBox)obj).BorderBrush = Brushes.LightBlue;
                    }
                }
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "IsValidValue", "CtlSIPUserInfo.xaml.cs");
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
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "IsFloat()", "CtlSIPUserInfo.xaml.cs");
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
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "IsMixInt()", "CtlSIPUserInfo.xaml.cs");
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
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "IsPositiveInt()", "CtlSIPUserInfo.xaml.cs");
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
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "IsNegativeInt()", "CtlSIPUserInfo.xaml.cs");
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

                tbcSIPUserInfo.Visibility = Visibility.Collapsed;
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
                        tbcSIPUserInfo.Visibility = Visibility.Visible;
                        btnSave.Visibility = Visibility.Visible;
                        btnCancel.Visibility = Visibility.Visible;
                    }
                }
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "FncPermissionsReview()", "CtlSIPUserInfo.xaml.cs");
            }
        }

        void CtlGrid_btnEditClicked(int RowID)
        {
            try
            {
                varState = 1;
                varID = Convert.ToInt32(objSIPUserInfoCollection[RowID].ID);

                tbcSIPUserInfo.Visibility = Visibility.Visible;
                btnSave.Visibility = Visibility.Visible;
                btnCancel.Visibility = Visibility.Visible;


                txtSIPID.Text = objSIPUserInfoCollection[RowID].SIPID.ToString();
                txtSIPPass.Password = objSIPUserInfoCollection[RowID].SIPPass.ToString();
                CtlGrid.IsEnabled = false;
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "CtlGrid_btnEditClicked", "CtlSIPUserInfo.xaml.cs");
            }
        }

        void CtlGrid_btnDeleteClicked(int RowID)
        {
            try
            {
                varID = Convert.ToInt32(objSIPUserInfoCollection[RowID].ID);
                if (System.Windows.MessageBox.Show("Do You Really Want To Delete This Record ?", "->Delete SIP User", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    ClsSIPUserInfo.Delete(varID);
                    System.Windows.MessageBox.Show("Record Deleted!!", "->Delete SIP User", MessageBoxButton.OK, MessageBoxImage.Information);
                    funSetGrid();
                }
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "CtlGrid_btnDeleteClicked", "CtlSIPUserInfo.xaml.cs");
            }
        }

        void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                funSetGrid();
                funClearTextBoxes();
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "btnCancel_Click", "CtlSIPUserInfo.xaml.cs");
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
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "FncCheckCanvas()", "CtlSIPUserInfo.xaml.cs");
                return false;
            }
        }

        void btnSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                #region Validations

                if (txtSIPID.Text.Trim() == "")
                {
                    MessageBox.Show("SIP UserID Can't Be Blank", "-> SIP User", MessageBoxButton.OK, MessageBoxImage.Information);
                    txtSIPID.Focus();
                    txtSIPID.Text = txtSIPID.Text.Trim();
                    return;
                }

                if (txtSIPPass.Password.Trim() == "")
                {
                    MessageBox.Show("SIP User PassWord Can't Be Blank", "-> SIP User", MessageBoxButton.OK, MessageBoxImage.Information);
                    txtSIPPass.Focus();
                    txtSIPPass.Password = txtSIPPass.Password.Trim();
                    return;
                }

                #endregion

                bool b = FncCheckCanvas(cnvMain);

                if (b == false)
                {
                    MessageBox.Show("Please Enter Valid Data In Indicated Fields!!", "-> SIP User", MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }

                ClsSIPUserInfo objServer = new ClsSIPUserInfo();
                if (varState == 0)
                    objServer.ID = -1;
                else
                    objServer.ID = varID;

                objServer.ActiveServerID = 1;
                objServer.CreatedBy = VMuktiAPI.VMuktiInfo.CurrentPeer.ID;
                try
                {
                    objServer.SIPID = int.Parse(txtSIPID.Text.Trim());
                }
                catch (Exception exp)
                {
                    MessageBox.Show("Invalid SIP UserID!!", "-> SIP User", MessageBoxButton.OK, MessageBoxImage.Information);
                    txtSIPID.Focus();
                    return;
                }

                try
                {
                    objServer.SIPPass = int.Parse(txtSIPPass.Password.Trim());
                }
                catch (Exception exp)
                {
                    MessageBox.Show("Invalid SIP Password!!", "-> SIP User", MessageBoxButton.OK, MessageBoxImage.Information);
                    txtSIPPass.Focus();
                    return;
                }

                Int64 gID = objServer.Save();

                if (gID == 0)
                {
                    MessageBox.Show("Duplicate Entry For SIPUsers Is Not Allowed !!", "-> SIP Users", MessageBoxButton.OK, MessageBoxImage.Information);
                }

                else
                {
                    MessageBox.Show("Record Saved Successfully!!");
                    funSetGrid();
                    funClearTextBoxes();
                }
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "btnSave_Click", "CtlSIPUserInfo.xaml.cs");
               
            }
        }

        public void funSetGrid()
        {
            try
            {
                CtlGrid.Cols = 3;
                CtlGrid.CanDelete = true;
                CtlGrid.CanEdit = true;

                CtlGrid.Columns[0].Header = "ID";
                CtlGrid.Columns[1].Header = "SIP ID";
                CtlGrid.Columns[2].Header = "SIP Password";
                //CtlGrid.Columns[3].Header = "Active server ID";
                //CtlGrid.Columns[4].Header = "CreatedBy";

                CtlGrid.Columns[0].BindTo("ID");
                CtlGrid.Columns[1].BindTo("SIPID");
                CtlGrid.Columns[2].BindTo("SIPPass");
                //CtlGrid.Columns[3].BindTo("ActiveServerID");
                //CtlGrid.Columns[4].BindTo("CreatedBy");

                objSIPUserInfoCollection = ClsSIPUserInfoCollection.GetAll();
                CtlGrid.Bind(objSIPUserInfoCollection);
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "funSetGrid()", "CtlSIPUserInfo.xaml.cs");    
            }
        }

        public void funClearTextBoxes()
        {
            try
            {
                varState = 0;
                varID = -1;
                txtSIPID.Text = "";
                txtSIPPass.Password = "";
                CtlGrid.IsEnabled = true;
                FncPermissionsReview();
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "funClearTextBoxes()", "CtlSIPUserInfo.xaml.cs");
            }
        }
    }
}
