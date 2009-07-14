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
using System.Collections;
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
using Role.Business;
using System.Data;
using System.Reflection;
using VMuktiAPI;

namespace Role.Presentation
{
    public enum ModulePermissions
    {
        Add = 0,
        Edit = 1,
        Delete = 2,
        View = 3
    }

    public partial class CtlRole : System.Windows.Controls.UserControl
    {
        Int64 varID = -1;
        int varState = 0;
        ClsRoleCollection objRoleCollection = null;
        DataTable dtRoles = new DataTable();
        //        ctlGridPermission c;
        TreeView trvMain = new TreeView();
        ModulePermissions[] _MyPermissions;


        public CtlRole(ModulePermissions[] MyPermissions)
        {
            try
            {
                InitializeComponent();
                _MyPermissions = MyPermissions;
                trvMain.FontSize = 13;
                FncPermissionsReview();
                funSetGrid();

                //funSetCombobox();
                cmbModules.SelectionChanged += new SelectionChangedEventHandler(cmbModules_SelectionChanged);
                btnSavePer.Click += new RoutedEventHandler(btnSavePer_Click);
                btnCancelPer.Click += new RoutedEventHandler(btnCancelPer_Click);
                FncFillPermissions();

                trvMain.SetValue(Canvas.LeftProperty, 30.0);
                trvMain.Height = 200;
                trvMain.Width = 300;

                this.Loaded += new RoutedEventHandler(CtlRole_Loaded);
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "CtlRole", "CtlRole.xaml.cs");
            }

        }

        void CtlRole_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                this.Width = ((Grid)this.Parent).ActualWidth;
                ((Grid)this.Parent).SizeChanged += new SizeChangedEventHandler(CtlRole_SizeChanged);
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "CtlRole_Loaded", "CtlRole.xaml.cs");
            }
        }

        void CtlRole_SizeChanged(object sender, SizeChangedEventArgs e)
        {
             try
            {
                this.Width = e.NewSize.Width - 5;
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "CtlRole_SizeChanged", "CtlRole.xaml.cs");
            }
        }

        void FncPermissionsReview()
        {
            try
            {
                CtlGrid.CanEdit = false;
                CtlGrid.CanDelete = false;
                CtlGrid.Visibility = Visibility.Collapsed;
                tbcRoleAddition.Visibility = Visibility.Collapsed;
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
                        tbcRoleAddition.Visibility = Visibility.Visible;
                        btnSave.Visibility = Visibility.Visible;
                        btnCancel.Visibility = Visibility.Visible;
                    }
                }
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "FunPermissionsReview", "CtlRole.xaml.cs");
            }
        }


        void CtlGrid_btnEditClicked(int RowID)
        {
            try
            {
                varState = 1;

                tbcRoleAddition.Visibility = Visibility.Visible;
                btnSave.Visibility = Visibility.Visible;
                btnCancel.Visibility = Visibility.Visible;

                varID = Convert.ToInt64(objRoleCollection[RowID].ID);
                FncTickPermissions(varID);

                txtName.Text = objRoleCollection[RowID].RoleName;
                txtDescription.Text = objRoleCollection[RowID].Description;
                if (objRoleCollection[RowID].IsAdmin == true)
                {
                    chkIsAdmin.IsChecked = true;
                }
                else
                {
                    chkIsAdmin.IsChecked = false;
                }

                //funSetCombobox();
                FncFillPermissions();
                FncTickPermissions(varID);
                CtlGrid.IsEnabled = false;
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "CtlGrid_btnEditClicked", "CtlRole.xaml.cs");
            }
        }

        void CtlGrid_btnDeleteClicked(int RowID)
        {
            try
            {
                varID = Convert.ToInt64(objRoleCollection[RowID].ID);
                if (System.Windows.MessageBox.Show("Do You Really Want To Delete This Record ?", "->Delete Role", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    ClsRole.Delete(varID);
                    System.Windows.MessageBox.Show("Record Deleted!!", "->Role Delete", MessageBoxButton.OK, MessageBoxImage.Information);
                    funSetGrid();
                }
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "CtlGrid_btnDeleteClicked", "CtlRole.xaml.cs");
            }
        }

        void FncFillPermissions()
        {
            try
            {
                trvMain.Items.Clear();
                cnvManagement.Children.Clear();

                ClsModulesCollection objMColl = ClsModulesCollection.GetAll();
                TreeViewItem[] tviModules = new TreeViewItem[objMColl.Count];

                for (int i = 0; i < objMColl.Count; i++)
                {
                    tviModules[i] = new TreeViewItem();
                    tviModules[i].FontSize = 13;
                    tviModules[i].Header = objMColl[i].ModuleName.ToString();
                    tviModules[i].Tag = objMColl[i].ID.ToString();

                    ClsPermissionCollection objPColl = ClsPermissionCollection.GetAll(objMColl[i].ID);

                    for (int j = 0; j < objPColl.Count; j++)
                    {
                        CheckBox chkItem = new CheckBox();
                        chkItem.Content = objPColl[j].PermissionName;
                        chkItem.FontSize = 13;
                        chkItem.Tag = objPColl[j].PermissionValue;
                        TreeViewItem tviNew = new TreeViewItem();
                        tviNew.Header = chkItem;
                        tviNew.FontSize = 13;
                        tviNew.Tag = objPColl[j].ID;

                        tviModules[i].Items.Add(tviNew);
                    }
                    trvMain.Items.Add(tviModules[i]);
                }
                cnvManagement.Children.Add(trvMain);
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "FncFillPermissions", "CtlRole.xaml.cs");
            }
        }

        private void FncSavePermissions(Int64 RoleID)
        {
            try
            {
                ClsPermission.Delete(RoleID);

                for (int i = 0; i < trvMain.Items.Count; i++)
                {
                    TreeViewItem tviTemp = ((TreeViewItem)trvMain.Items[i]);
                    for (int j = 0; j < tviTemp.Items.Count; j++)
                    {
                        if (((CheckBox)((TreeViewItem)tviTemp.Items[j]).Header).IsChecked == true)
                        {
                            ClsPermission objper = new ClsPermission();
                            objper.Save(Int64.Parse(((TreeViewItem)tviTemp.Items[j]).Tag.ToString()), RoleID);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "FncSavePermissions", "CtlRole.xaml.cs");
            }
        }

        void FncTickPermissions(Int64 RoleID)
        {
            try
            {
                DataTable dt = ClsPermission.GetPermByRoleID(RoleID);

                for (int i = 0; i < trvMain.Items.Count; i++)
                {
                    TreeViewItem tviTemp = ((TreeViewItem)trvMain.Items[i]);
                    for (int j = 0; j < tviTemp.Items.Count; j++)
                    {
                        for (int r = 0; r < dt.Rows.Count; r++)
                        {
                            if (Int64.Parse(((TreeViewItem)tviTemp.Items[j]).Tag.ToString()) == Int64.Parse(dt.Rows[r][1].ToString()))
                            {
                                ((CheckBox)((TreeViewItem)tviTemp.Items[j]).Header).IsChecked = true;
                            }
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "FncTickPermissions", "CtlRole.xaml.cs");
            }
        }

        void btnCancelPer_Click(object sender, RoutedEventArgs e)
        {
            
            //FncClearAll();
            //tbiManagement.IsEnabled = false;
            //tbiRoles.IsEnabled = true;
            //tbiRoles.IsSelected = true;
        }

        void btnSavePer_Click(object sender, RoutedEventArgs e)
        {
            
            //ClsPermission objP = new ClsPermission();
            //objP.ModuleID = int.Parse(((ListBoxItem)cmbModules.SelectedItem).Tag.ToString());
            //objP.RoleID = varID;
            //objP.Delete();

            //for (int i = 0; i < lstPermissions.SelectedItems.Count; i++)
            //{
            //    ClsPermission objPer = new ClsPermission();
            //    objPer.RoleID = varID;
            //    objPer.ModuleID = int.Parse(((ListBoxItem)cmbModules.SelectedItem).Tag.ToString());
            //    objPer.ID = int.Parse(((ListBoxItem)lstPermissions.SelectedItems[i]).Tag.ToString());
            //    objPer.Save();
            //}
            //MessageBox.Show("Permissions Added Successfully!!","-> Permissions",MessageBoxButton.OK,MessageBoxImage.Exclamation);
            //varState = 0;
            //varID = -1;

        }

        void cmbModules_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            
        }


        public void funSetGrid()
        {
            try
            {
                CtlGrid.Cols = 4;
                //CtlGrid.CanEdit = true;
                //CtlGrid.CanDelete = true;

                CtlGrid.Columns[0].Header = "ID";
                CtlGrid.Columns[1].Header = "RoleName";
                CtlGrid.Columns[2].Header = "Description";
                CtlGrid.Columns[3].Header = "IsAdmin";
                CtlGrid.Columns[3].IsIcon = true;
                //CtlGrid.Columns[4].Header = "CreatedBy";
                //CtlGrid.Columns[5].Header = "ModifiedBy";
                //CtlGrid.Columns[6].Header = "CreatedDate";
                //CtlGrid.Columns[7].Header = "ModifiedDate";

                CtlGrid.Columns[0].BindTo("ID");
                CtlGrid.Columns[1].BindTo("RoleName");
                CtlGrid.Columns[2].BindTo("Description");
                CtlGrid.Columns[3].BindTo("IsAdmin");
                //CtlGrid.Columns[4].BindTo("CreatedBy");
                //CtlGrid.Columns[5].BindTo("ModifiedBy");
                //CtlGrid.Columns[6].BindTo("CreatedDate");
                //CtlGrid.Columns[7].BindTo("ModifiedDate");

                objRoleCollection = ClsRoleCollection.GetAll();
                CtlGrid.Bind(objRoleCollection);
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "FncSetGrid", "CtlRole.xaml.cs");
            }

        }

        public void FncClearAll()
        {
            try
            {
                txtName.Text = "";
                txtDescription.Text = "";
                chkIsAdmin.IsChecked = false;
                varID = -1;
                varState = 0;
                CtlGrid.IsEnabled = true;
                FncFillPermissions();
                FncPermissionsReview();
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "FncClearAll", "CtlRole.xaml.cs");
            }
        }

        private void txtName_GotFocus(object sender, System.Windows.RoutedEventArgs e)
        {
            try
            {
                txtName.SelectAll();
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "txtName_GotFocus", "CtlRole.xaml.cs");
            }
        }

        private void txtDescription_GotFocus(object sender, System.Windows.RoutedEventArgs e)
        {
            try
            {
                txtDescription.SelectAll();
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "txtDescription_GotFocus", "CtlRole.xaml.cs");
            }
        }

        private void txtDescription_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            
        }

        private void txtDescription_LostFocus(object sender, System.Windows.RoutedEventArgs e)
        {
            
        }

        private void chkIsDNC_Checked(object sender, System.Windows.RoutedEventArgs e)
        {
            
        }

        private void btnSave_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            try
            {

                if (txtName.Text.Trim() == "" || txtName.Text.Length > 50)
                {
                    MessageBox.Show("Name of the Role cannot be left Blank or not more than 50 characters. ", "-> Please Enter a Name", MessageBoxButton.OK, MessageBoxImage.Information);
                    txtName.Focus();
                    txtName.Text = txtName.Text.Trim();
                    return;
                }

                ClsRole objRole = new ClsRole();

                if (varState == 0) //whn edit is clicked var state is sate to 1 so that sstored procedure update the table data
                    objRole.ID = -1;
                else
                    objRole.ID = varID;

                objRole.RoleName = txtName.Text.Trim();
                objRole.Description = txtDescription.Text.Trim();

                if (chkIsAdmin.IsChecked == true)
                    objRole.IsAdmin = true;
                else
                    objRole.IsAdmin = false;

                objRole.CreatedBy = VMuktiAPI.VMuktiInfo.CurrentPeer.ID;

                int gID = Convert.ToInt16(objRole.Save());

                if (gID == 0)
                {
                    MessageBox.Show("Duplicate Entry For Role Name Not Allowed !!", "-> Role", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                }

                else
                {
                    lblCurrentRole.Content = "Role : " + txtName.Text;
                    FncSavePermissions(varID);
                    //c.Save(varID);
                    FncClearAll();
                    funSetGrid();
                    tbiRoles.IsSelected = true;
                }
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "btnSave_Click", "CtlRole.xaml.cs");
            }
        }



        private void btnCancel_click(object sender, System.Windows.RoutedEventArgs e)
        {
            try
            {
                FncClearAll();
                funSetGrid();
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "btnCancel_click", "CtlRole.xaml.cs");
            }
        }

        private void tbiManagement_GotFocus(object sender, System.Windows.RoutedEventArgs e)
        {
            
            
        }

        private void tbiManagement_LostFocus(object sender, System.Windows.RoutedEventArgs e)
        {
            
        }

    }
}
