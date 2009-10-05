
﻿/* VMukti 2.0 -- An Open Source Video Communications Suite
=======
﻿/* VMukti 2.0 -- An Open Source Unified Communications Engine
>>>>>>> b74131bacb80d82c79711cf70fe93af3fb611b40:VMuktiModules/Call Center/UserInfo/User.Application/CtlUserInfo.xaml.cs
*
* Copyright (C) 2008 - 2009, VMukti Solutions Pvt. Ltd.
*
* Hardik Sanghvi <hardik@vmukti.com>
*
* See http://www.vmukti.com for more information about
* the VMukti project. Please do not directly contact
* any of the maintainers of this project for assistance;

*/
using System;
using System.IO;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Navigation;
using System.Data;
using User.Business;
using System.Reflection;
using VMuktiAPI;
using System.Windows.Input;
using System.Text.RegularExpressions;
using System.Text;


namespace User.Presentation
{
    public enum ModulePermissions
    {
        Add = 0,
        Edit = 1,
        Delete = 2,
        View = 3
    }

    public partial class CtlUserInfo : System.Windows.Controls.UserControl
    {
       
        int varState = 0;
        int varID = -1;
        bool isShiftDown = false;
        DataTable dtUsers = new DataTable();
        UserCollection objUserCollection = null;
        ClsRoleCollection objRoleCollection = null;
        ModulePermissions[] _MyPermissions;

        public CtlUserInfo(ModulePermissions[] MyPermissions)
        {
            try
            {
                this.InitializeComponent();
                _MyPermissions = MyPermissions;
                FncPermissionsReview();
                funSetCombo();
                funSetGrid();
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "CtlUserInfo", "CtlUserInfo.xaml.cs");
            }
        }

        void FncPermissionsReview()
        {
            try
            {
            CtlGrid.CanEdit = false;
            CtlGrid.CanDelete = false;
            CtlGrid.Visibility = Visibility.Collapsed;
            tbcUserAdditon.Visibility = Visibility.Collapsed;
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
                    tbcUserAdditon.Visibility = Visibility.Visible;
                    btnSave.Visibility = Visibility.Visible;
                    btnCancel.Visibility = Visibility.Visible;
                }
            }
             
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "FncPermissionsReview", "CtlUserInfo.xaml.cs");
            }
        }

        
        public void funSetGrid()
        {
            try
            {
                CtlGrid.ClearGrid();
                CtlGrid.Cols = 12;
                //CtlGrid.CanEdit = true;
                //CtlGrid.CanDelete = true;

                CtlGrid.Columns[0].Header = "User ID";
                CtlGrid.Columns[1].Header = "Display Name";
                CtlGrid.Columns[2].Header = "Role Name";
                CtlGrid.Columns[3].Header = "First Name";
                CtlGrid.Columns[4].Header = "Last Name";
                CtlGrid.Columns[5].Header = "Email";
                CtlGrid.Columns[6].Header = "Password";
                CtlGrid.Columns[7].Header = "Is Active";
                CtlGrid.Columns[7].IsIcon = true;
                //CtlGrid.Columns[8].Header = "Created Date";
                //CtlGrid.Columns[9].Header = "Created By";
                //CtlGrid.Columns[10].Header = "Modified Date";
                //CtlGrid.Columns[11].Header = "Modified By";
                CtlGrid.Columns[8].Header = "RatePerHour";
                CtlGrid.Columns[9].Header = "OverTimeRate";
                CtlGrid.Columns[10].Header = "DoubleRatePerHour";
                CtlGrid.Columns[11].Header = "DoubleOverTimeRate";

                CtlGrid.Columns[0].BindTo("ID");
                CtlGrid.Columns[1].BindTo("DisplayName");
                CtlGrid.Columns[2].BindTo("RoleName");
                CtlGrid.Columns[3].BindTo("FName");
                CtlGrid.Columns[4].BindTo("LName");
                CtlGrid.Columns[5].BindTo("EMail");
                CtlGrid.Columns[6].BindTo("PassWord");
                CtlGrid.Columns[7].BindTo("IsActive");
                //CtlGrid.Columns[8].BindTo("CreatedDate");
                //CtlGrid.Columns[9].BindTo("CreatedBy");
                //CtlGrid.Columns[10].BindTo("ModifiedDate");
                //CtlGrid.Columns[11].BindTo("ModifiedBy");
                CtlGrid.Columns[8].BindTo("RatePerHour");
                CtlGrid.Columns[9].BindTo("OverTimeRate");
                CtlGrid.Columns[10].BindTo("DoubleRatePerHour");
                CtlGrid.Columns[11].BindTo("DoubleOverTimeRate");

                objUserCollection = User.Business.UserCollection.GetAll();
                CtlGrid.Bind(objUserCollection);
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "FncSetGrid", "CtlUserInfo.xaml.cs");
            }
        }

        void CtlGrid_btnDeleteClicked(int RowID)
        {
            try
            {
                varID = Convert.ToInt32(objUserCollection[RowID].ID);
                if (System.Windows.MessageBox.Show("Do You Really Want To Delete This Record ?", "Delete User", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    ClsUser.Delete(varID);
                    System.Windows.MessageBox.Show("Record Deleted!!", "User Deleted", MessageBoxButton.OK, MessageBoxImage.Information);
                    funSetGrid();
                }
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "CtlGrid_btnDeleteClicked", "CtlUserInfo.xaml.cs");
            }
        }

        void CtlGrid_btnEditClicked(int RowID)
        {
            try
            {
                varState = 1;

                tbcUserAdditon.Visibility = Visibility.Visible;
                btnSave.Visibility = Visibility.Visible;
                btnCancel.Visibility = Visibility.Visible;

                varID = Convert.ToInt32(objUserCollection[RowID].ID);
                tbiPersonalInfo.IsSelected = true;

                txtRatePerHour.Text = objUserCollection[RowID].RatePerHour.ToString();
                txtOverTimeRate.Text = objUserCollection[RowID].OverTimeRate.ToString();
                txtDoubleTimeRate.Text = objUserCollection[RowID].DoubleRatePerHour.ToString();
                txtDoubleOverTimeRate.Text = objUserCollection[RowID].DoubleOverTimeRate.ToString();
                txtDisplayName.Text = objUserCollection[RowID].DisplayName;

                for (int i = 0; i < cmbRoleAssigned.Items.Count; i++)
                {
                    if (((ListBoxItem)cmbRoleAssigned.Items[i]).Tag.ToString() == objUserCollection[RowID].RoleID.ToString())
                        cmbRoleAssigned.Text = ((ListBoxItem)cmbRoleAssigned.Items[i]).Content.ToString();
                }

                txtFirstName.Text = objUserCollection[RowID].FName;
                txtLastName.Text = objUserCollection[RowID].LName;
                txtEmail.Text = objUserCollection[RowID].EMail;
                txtPassWord.Password = DeCodeString(objUserCollection[RowID].PassWord);

                if (Convert.ToBoolean(objUserCollection[RowID].IsActive) == true)
                {
                    chkIsActive.IsChecked = true;
                }
                else
                {
                    chkIsActive.IsChecked = false;
                }
                CtlGrid.IsEnabled = false;
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "CtlGrid_btnEditClicked", "CtlUserInfo.xaml.cs");
            }

        }

        public void funSetCombo()
        {
            try
            {
                // No Roles So Initially Displaying //
                cmbRoleAssigned.Items.Clear();
                objRoleCollection = ClsRoleCollection.GetAll();
                for (int i = 0; i < objRoleCollection.Count; i++)
                {
                    ComboBoxItem l = new ComboBoxItem();
                    l.Content = objRoleCollection[i].RoleName;
                    l.Tag = objRoleCollection[i].ID;
                    cmbRoleAssigned.Items.Add(l);
                }
            }

            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "fncSetCombo", "CtlUserInfo.xaml.cs");
            }
        }

        public void funClearTextBoxes()
        {
            try
            {
                varState = 0;
                varID = -1;
                txtRatePerHour.Text = "";
                txtOverTimeRate.Text = "";
                txtDoubleTimeRate.Text = "";
                txtDoubleOverTimeRate.Text = "";
                txtDisplayName.Text = "";
                funSetCombo();
                txtFirstName.Text = "";
                txtLastName.Text = "";
                txtEmail.Text = "";
                txtPassWord.Password = "";
                chkIsActive.IsChecked = false;
                tbiPersonalInfo.IsSelected = true;
                FncPermissionsReview();
                CtlGrid.IsEnabled = true;
                tbiPersonalInfo.IsSelected = true;
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "fncClearTextBoxes", "CtlUserInfo.xaml.cs");
            }
        }

        private void cmbOperators_GotFocus(object sender, System.Windows.RoutedEventArgs e)
        {
           
        }
        private void cmbOperators_LostFocus(object sender, System.Windows.RoutedEventArgs e)
        {
           
        }
        private void cmbOperators_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
        }

        private void cmbFields_GotFocus(object sender, System.Windows.RoutedEventArgs e)
        {
           
        }
        private void cmbFields_LostFocus(object sender, System.Windows.RoutedEventArgs e)
        {
           
        }
        private void cmbFields_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
           
        }


        private void txtValue_LostFocus(object sender, System.Windows.RoutedEventArgs e)
        {
           
        }
        private void txtValue_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            
        }
        private void txtValue_GotFocus(object sender, System.Windows.RoutedEventArgs e)
        {
           
        }
        private void tbiPersonalInfo_GotFocus(object sender, System.Windows.RoutedEventArgs e)
        {
           
        }
        private void tbiPersonalInfo_LostFocus(object sender, System.Windows.RoutedEventArgs e)
        {
           
        }

        private void tbiPayrollInfo_GotFocus(object sender, System.Windows.RoutedEventArgs e)
        {
          
        }
        private void tbiPayrollInfo_LostFocus(object sender, System.Windows.RoutedEventArgs e)
        {
          
        }
        private void txtDisplayName_GotFocus(object sender, System.Windows.RoutedEventArgs e)
        {
            
        }
        private void txtDisplayName_LostFocus(object sender, System.Windows.RoutedEventArgs e)
        {
           
        }
        private void txtDisplayName_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
           
        }
        private void cmbRoleAssigned_LostFocus(object sender, System.Windows.RoutedEventArgs e)
        {
            
        }
        private void cmbRoleAssigned_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
           
        }

        private void cmbRoleAssigned_GotFocus(object sender, System.Windows.RoutedEventArgs e)
        {
          
        }
        private void btnAddRole_Click(object sender, System.Windows.RoutedEventArgs e)
        {
          
        }
      
        private void txtFirstName_LostFocus(object sender, System.Windows.RoutedEventArgs e)
        {
           
        }
        private void txtFirstName_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
           
        }
        private void txtFirstName_GotFocus(object sender, System.Windows.RoutedEventArgs e)
        {
           
        }
        private void txtLastName_GotFocus(object sender, System.Windows.RoutedEventArgs e)
        {
          
        }
        private void txtLastName_LostFocus(object sender, System.Windows.RoutedEventArgs e)
        {
            
        }
        private void txtLastName_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
        }
        private void txtEmail_GotFocus(object sender, System.Windows.RoutedEventArgs e)
        {
           
        }
        private void txtEmail_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
           
        }
        private void txtEmail_LostFocus(object sender, System.Windows.RoutedEventArgs e)
        {
         
        }
        private void txtPassword_LostFocus(object sender, System.Windows.RoutedEventArgs e)
        {
            
        }
        private void txtPassword_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
           
        }
        private void txtPassword_GotFocus(object sender, System.Windows.RoutedEventArgs e)
        {
          
        }
        private void chkIsActive_GotFocus(object sender, System.Windows.RoutedEventArgs e)
        {
            
        }
        private void chkIsActive_Checked(object sender, System.Windows.RoutedEventArgs e)
        {
          
        }
        private void chkIsActive_LostFocus(object sender, System.Windows.RoutedEventArgs e)
        {
           
        }
        private void btnDeleteUser_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            
        }
        private void btnDeleteUser_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            
        }

        
        private void txtRatePerHour_GotFocus(object sender, System.Windows.RoutedEventArgs e)
        {
            
        }
        private void txtRatePerHour_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            try
            {
                if (e.Key == Key.RightShift || e.Key == Key.LeftShift)
            {
                    isShiftDown = true;
            }
            #region Allowing only numbers , '.'(only 1) and Tab
            if (!((e.Key >= Key.D0 && e.Key <= Key.D9) ||
                ((e.Key == Key.OemPeriod || e.Key == Key.Decimal) && ((TextBox)sender).Text.IndexOf(".") == -1) ||
                e.Key == Key.Tab ||
                 (e.Key >= Key.NumPad0 && e.Key <= Key.NumPad9))
                    || (isShiftDown == true))
            {
                e.Handled = true;
            }
            #endregion
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "txtRatePerHour_KeyDown", "CtlUserInfo.xaml.cs");
            }
        }
        private void txtRatePerHour_KeyUp(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.Key == Key.RightShift || e.Key == Key.LeftShift)
                {
                    isShiftDown = false;
                }
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "txtRatePerHour_KeyUp", "CtlUserInfo.xaml.cs");
            }
        }
        private void txtRatePerHour_LostFocus(object sender, System.Windows.RoutedEventArgs e)
        {
            
        }

        private void txtOverTimeRate_GotFocus(object sender, System.Windows.RoutedEventArgs e)
        {
            
        }
        private void txtOverTimeRate_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            try
            {
                if (e.Key == Key.RightShift || e.Key == Key.LeftShift)
            {
                    isShiftDown = true;
            }

            #region Allowing only numbers , '.'(only 1) and Tab
            if (!((e.Key >= Key.D0 && e.Key <= Key.D9) ||
                ((e.Key == Key.OemPeriod || e.Key == Key.Decimal) && ((TextBox)sender).Text.IndexOf(".") == -1) ||
                e.Key == Key.Tab ||
                     (e.Key >= Key.NumPad0 && e.Key <= Key.NumPad9))
                    || (isShiftDown == true))
            {
                e.Handled = true;
            }
            #endregion
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "txtOverTimeRate_KeyDown", "CtlUserInfo.xaml.cs");
            }
            
        }
        private void txtOverTimeRate_KeyUp(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.Key == Key.RightShift || e.Key == Key.LeftShift)
                {
                    isShiftDown = false;
                }
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "txtOverTimeRate_KeyUp", "CtlUserInfo.xaml.cs");
            }
        }
        private void txtOverTimeRate_LostFocus(object sender, System.Windows.RoutedEventArgs e)
        {
            
        }

        private void txtDoubleTimeRate_GotFocus(object sender, System.Windows.RoutedEventArgs e)
        {
           
        }
        private void txtDoubleTimeRate_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            try
            {
                if (e.Key == Key.RightShift || e.Key == Key.LeftShift)
            {
                    isShiftDown = true;
            }

            #region Allowing only numbers , '.'(only 1) and Tab
            if (!((e.Key >= Key.D0 && e.Key <= Key.D9) ||
                ((e.Key == Key.OemPeriod || e.Key == Key.Decimal) && ((TextBox)sender).Text.IndexOf(".") == -1) ||
                e.Key == Key.Tab ||
                     (e.Key >= Key.NumPad0 && e.Key <= Key.NumPad9))
                    || (isShiftDown == true))
            {
                e.Handled = true;
            }
            #endregion
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "txtDoubleTimeRate_KeyDown", "CtlUserInfo.xaml.cs");
            }
        }
        private void txtDoubleTimeRate_KeyUp(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.Key == Key.RightShift || e.Key == Key.LeftShift)
                {
                    isShiftDown = false;
                }
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "txtDoubleTimeRate_KeyUp", "CtlUserInfo.xaml.cs");
            }
        }
        private void txtDoubleTimeRate_LostFocus(object sender, System.Windows.RoutedEventArgs e)
        {
            
        }

        private void txtDoubleOverTimeRate_GotFocus(object sender, System.Windows.RoutedEventArgs e)
        {
          
        }
        private void txtDoubleOverTimeRate_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            try
            {
                if (e.Key == Key.RightShift || e.Key == Key.LeftShift)
            {
                    isShiftDown = true;
            }

            #region Allowing only numbers , '.'(only 1) and Tab
            if (!((e.Key >= Key.D0 && e.Key <= Key.D9) ||
                ((e.Key == Key.OemPeriod || e.Key == Key.Decimal) && ((TextBox)sender).Text.IndexOf(".") == -1) ||
                e.Key == Key.Tab ||
                     (e.Key >= Key.NumPad0 && e.Key <= Key.NumPad9))
                    || (isShiftDown == true))
            {
                e.Handled = true;
            }
            #endregion
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "txtDoubleOverTimeRate_KeyDown", "CtlUserInfo.xaml.cs");
            }
        }
        private void txtDoubleOverTimeRate_KeyUp(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.Key == Key.RightShift || e.Key == Key.LeftShift)
                {
                    isShiftDown = false;
                }
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "txtDoubleOverTimeRate_KeyUp", "CtlUserInfo.xaml.cs");
            }
        }
        private void txtDoubleOverTimeRate_LostFocus(object sender, System.Windows.RoutedEventArgs e)
        {
            try
            {
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "txtDoubleOverTimeRate_LostFocus", "CtlUserInfo.xaml.cs");
            }
        }

        private void btnSave_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            try
            {

                #region DataValidations

                //Display Name Validation
                if (txtDisplayName.Text.Trim() == "")
                {
                    MessageBox.Show("User Name Cann't be left Blank ", "->Please Enter a Display Name", MessageBoxButton.OK, MessageBoxImage.Information);
                    txtDisplayName.Focus();
                    txtDisplayName.Text = txtDisplayName.Text.Trim();
                    return;
                }

                //RoleAssigned Validation
                if (cmbRoleAssigned.Text.Trim() == "")
                {
                    MessageBox.Show("Role Cann't be left Blank", "Select one of the items for RoleAssigned", MessageBoxButton.OK, MessageBoxImage.Information);
                    cmbRoleAssigned.Focus();
                    return;
                }

                //FirstName Validation
                if (txtFirstName.Text.Trim() == "")
                {
                    MessageBox.Show("First Name Cann't be left Blank", "-> Please Enter First Name", MessageBoxButton.OK, MessageBoxImage.Information);
                    txtFirstName.Focus();
                    txtFirstName.Text = txtFirstName.Text.Trim();
                    return;
                }


                //Email Validation

                if (txtEmail.Text.Trim() == "")
                {
                    MessageBox.Show("Email Address cann't be left Blank", "-> Please Enter Email Address", MessageBoxButton.OK, MessageBoxImage.Information);
                    txtEmail.Text = txtEmail.Text.Trim();
                    txtEmail.Focus();
                    return;
                }

                // Email Valid?

                string pattern = @"^[a-z][a-z|0-9|]*([_][a-z|0-9]+)*([.][a-z|" +
                   @"0-9]+([_][a-z|0-9]+)*)?@[a-z][a-z|0-9|]*\.([a-z]" +
                   @"[a-z|0-9]*(\.[a-z][a-z|0-9]*)?)$";

                System.Text.RegularExpressions.Match match =
                    Regex.Match(txtEmail.Text.Trim(), pattern, RegexOptions.IgnoreCase);



                if (match.Success)
                { }
                else
                {
                    MessageBox.Show("Email Address Is Not Valid", "-> Please Enter Valid Email Address", MessageBoxButton.OK, MessageBoxImage.Information);
                    txtEmail.Focus();
                    return;
                }

                //
                //Password Validation


                if (txtPassWord.Password.Trim().Length < 6)
                {
                    MessageBox.Show("Password must be minimum SIX characters", "-> Please Enter Valid Password", MessageBoxButton.OK, MessageBoxImage.Information);
                    txtPassWord.Password = "";
                    txtPassWord.Focus();
                    return;
                }

                //Float Value Validation

                //float a = float.Parse(txtRatePerHour.Text);

                if (txtRatePerHour.Text.Trim() == "") txtRatePerHour.Text = "0.0";
                if (txtDoubleOverTimeRate.Text.Trim() == "") txtDoubleOverTimeRate.Text = "0.0";
                if (txtOverTimeRate.Text.Trim() == "") txtOverTimeRate.Text = "0.0";
                if (txtDoubleTimeRate.Text.Trim() == "") txtDoubleTimeRate.Text = "0.0";

                #endregion

                ClsUser objUser = new ClsUser();
                if (varState == 0)
                    objUser.ID = -1;
                else
                    objUser.ID = varID;

                objUser.DisplayName = txtDisplayName.Text.Trim();
                objUser.RoleID = Convert.ToInt32(((ListBoxItem)cmbRoleAssigned.SelectedItem).Tag);
                objUser.FName = txtFirstName.Text.Trim();
                objUser.LName = txtLastName.Text.Trim();
                objUser.EMail = txtEmail.Text.Trim();
                objUser.PassWord = encodestring(txtPassWord.Password.Trim());
                if (chkIsActive.IsChecked == true)
                    objUser.IsActive = true;
                else
                    objUser.IsActive = false;
                objUser.CreatedBy = VMuktiAPI.VMuktiInfo.CurrentPeer.ID;
                objUser.ModifiedBy = VMuktiAPI.VMuktiInfo.CurrentPeer.ID;

                objUser.RatePerHour = float.Parse(txtRatePerHour.Text.Trim());
                objUser.OverTimeRate = float.Parse(txtOverTimeRate.Text.Trim());
                objUser.DoubleRatePerHour = float.Parse(txtDoubleTimeRate.Text.Trim());
                objUser.DoubleOverTimeRate = float.Parse(txtDoubleOverTimeRate.Text.Trim());
                int retID = objUser.Save();

                if (retID == 0)
                {
                    MessageBox.Show("User With Same Name is Not Allowed", "-> User", MessageBoxButton.OK, MessageBoxImage.Information);
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
                VMuktiHelper.ExceptionHandler(ex, "btnSave_Click", "CtlUserInfo.xaml.cs");
            }
        }
             
        private void btnCancel_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            try
            {
                funClearTextBoxes();
                funSetGrid();
                funSetCombo();
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "btnCancel_Click", "CtlUserInfo.xaml.cs");
            }
            
        }

        string encodestring(string str)
        {
            try
            {
                int j = 0;

                Byte[] encodedBytes = System.Text.ASCIIEncoding.ASCII.GetBytes(str);
                Byte[] ans = new Byte[encodedBytes.Length];
                foreach (Byte b in encodedBytes)
                {

                    int i = Convert.ToInt32(b);

                    if ((i >= 65 && i <= 90) || (i >= 97 && i <= 122))
                    {
                        i += 4;
                        if ((i > 90 && i <= 97) || (i > 122 && i <= 129))
                            i = i - 26;

                    }
                    else if (i >= 48 && i <= 57)
                    {
                        i += 7;
                        if (i > 57)
                            i = i - 10;
                    }
                    else
                    {
                        if (i == 61)
                            i = 44;
                        else if (i == 44)
                            i = 61;
                    }

                    ans[j++] = Convert.ToByte(i);
                }
                return System.Text.ASCIIEncoding.ASCII.GetString(ans);
            }
            catch (Exception ex)
            {

                return null;
            }
        }

        string DeCodeString(string str)
        {
            try
            {
                int j = 0;

                Byte[] encodedBytes = System.Text.ASCIIEncoding.ASCII.GetBytes(str);
                Byte[] ans = new Byte[encodedBytes.Length];
                foreach (Byte b in encodedBytes)
                {

                    int i = Convert.ToInt32(b);

                    if ((i >= 65 && i <= 90) || (i >= 97 && i <= 122))
                    {
                        i -= 4;
                        if ((i < 65 && i >= 61) || (i < 97 && i >= 93))
                            i = i + 26;

                    }
                    else if (i >= 48 && i <= 57)
                    {
                        i -= 7;
                        if (i < 48)
                            i = i + 10;
                    }

                    else
                    {
                        if (i == 61)
                            i = 44;
                        else if (i == 44)
                            i = 61;
                    }

                    ans[j++] = Convert.ToByte(i);
                }
                return System.Text.ASCIIEncoding.ASCII.GetString(ans);
            }
            catch (Exception ex)
            {
                return null;
            }

        }
        

        

    }
}