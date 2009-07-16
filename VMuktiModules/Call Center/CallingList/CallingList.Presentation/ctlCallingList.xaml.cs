
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
using System.Collections.Generic;
using System.Threading;
using System.Windows.Forms;
using System.Reflection;
using CallingList.Business;
using VMuktiAPI;

namespace CallingList.Presentation
{
    public enum ModulePermissions
    {
        Add = 0,
        Edit = 1,
        Delete = 2,
        View = 3
    }

    public partial class CtlCallingList : System.Windows.Controls.UserControl
    {
        //public static StringBuilder sb1;
        int varState = 0;
        Int64 varID = 0;
        DataTable dtCallingList = new DataTable();

        ClsCallingListCollection objCallCollection = null;
        ModulePermissions[] _MyPermissions;

       public  CtlCallingList(ModulePermissions[] MyPermissions)
     
        {
            try
            {
                InitializeComponent();
                this.Loaded += new RoutedEventHandler(CtlCallingList_Loaded);

                varState = 0;
                _MyPermissions = MyPermissions;
                FncPermissionsReview();
                funSetGrid();
                btnCancel.Click += new RoutedEventHandler(btnCancel_Click);

            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "CtlCallingList", "ctlCalllingList.xaml.cs"); 
            }
        }

        void CtlCallingList_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                this.Width = ((Grid)this.Parent).ActualWidth;
                ((Grid)this.Parent).SizeChanged += new SizeChangedEventHandler(CtlCallingList_SizeChanged);
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "CtlCallingList_Loaded", "ctlCalllingList.xaml.cs"); 
            }
            
        }

        void CtlCallingList_SizeChanged(object sender, SizeChangedEventArgs e)
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
                VMuktiHelper.ExceptionHandler(ex, "FncPermissionsReview", "ctlCalllingList.xaml.cs");
            } 
        }

        //void CtlGrid_btnEditClicked(int rowID)
        //{
        //    tbcUserAdditon.Visibility = Visibility.Visible;
        //    btnSave.Visibility = Visibility.Visible;
        //    btnCancel.Visibility = Visibility.Visible;

        //    varID = Convert.ToInt64(objCallCollection[rowID].ID);
        //    txtName.Text = objCallCollection[rowID].ListName;
        //    if ((bool)objCallCollection[rowID].IsActive == true)
        //        chkIsActive.IsChecked = true;
        //    else
        //        chkIsActive.IsChecked = false;
        //    if ((bool)objCallCollection[rowID].IsDNCList == true)
        //        chkIsDNCList.IsChecked = true;
        //    else
        //        chkIsDNCList.IsChecked = false;

        //    varState = 1;
        //    CtlGrid.IsEnabled = false;
        //}

        void CtlGrid_btnEditClicked(int rowID)
        {
            try
            {
                tbcUserAdditon.Visibility = Visibility.Visible;
                btnSave.Visibility = Visibility.Visible;
                btnCancel.Visibility = Visibility.Visible;

                varID = Convert.ToInt64(objCallCollection[rowID].ID);
                txtName.Text = objCallCollection[rowID].ListName;
                if ((bool)objCallCollection[rowID].IsActive == true)
                    chkIsActive.IsChecked = true;
                else
                    chkIsActive.IsChecked = false;
                if ((bool)objCallCollection[rowID].IsDNCList == true)
                    chkIsDNCList.IsChecked = true;
                else
                    chkIsDNCList.IsChecked = false;

                varState = 1;
                CtlGrid.IsEnabled = false;
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "CtlGrid_btnEditClicked", "ctlCalllingList.xaml.cs"); 

            }
        }

        void CtlGrid_btnDeleteClicked(int rowID)
        {
            try
            {
                varID = Convert.ToInt64(objCallCollection[rowID].ID);
                MessageBoxResult r = System.Windows.MessageBox.Show("Do You Really Want To Delete This Record ?", "->Delete Calling List", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (r == MessageBoxResult.Yes)
                {
                    ClsCallingList.Delete(varID);
                    System.Windows.MessageBox.Show("Record Deleted!!", "->Calling List", MessageBoxButton.OK, MessageBoxImage.Information);
                    funSetGrid();
                }

                FncClearAll();
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "CtlGrid_btnDeleteClicked", "ctlCalllingList.xaml.cs"); 

            }
        }

        private void FncClearAll()
        {
            try
            {
                txtName.Text = "";
                chkIsActive.IsChecked = false;
                chkIsDNCList.IsChecked = false;
                varState = 0;
                CtlGrid.IsEnabled = true;
                FncPermissionsReview();
                varID = 0;
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "FncClearAll", "ctlCalllingList.xaml.cs"); 
            } 
        }

        void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            try
            {
            FncClearAll();
            funSetGrid();
        }

            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "btnCancel_Click", "ctlCalllingList.xaml.cs"); 
            }
        }
        void funSetGrid()
        {
            try
            {
                CtlGrid.Cols = 4;

                CtlGrid.Columns[0].Header = "CallingList ID";
                CtlGrid.Columns[1].Header = "CallingList Name";
                CtlGrid.Columns[2].Header = "IsActive";
                CtlGrid.Columns[2].IsIcon = true;
                CtlGrid.Columns[3].Header = "IsDNCList";
                //CtlGrid.Columns[4].Header = "CreatedDate";
                //CtlGrid.Columns[5].Header = "CreatedBy";
                //CtlGrid.Columns[6].Header = "ModifiedDate";
                //CtlGrid.Columns[7].Header = "ModifeiedBy";

                CtlGrid.Columns[0].BindTo("ID");
                CtlGrid.Columns[1].BindTo("ListName");
                CtlGrid.Columns[2].BindTo("IsActive");
                CtlGrid.Columns[3].BindTo("IsDNCList");
                //CtlGrid.Columns[4].BindTo("CreatedDate");
                //CtlGrid.Columns[5].BindTo("CreatedBy");
                //CtlGrid.Columns[6].BindTo("ModifiedDate");
                //CtlGrid.Columns[7].BindTo("ModifiedBy");

                objCallCollection = ClsCallingListCollection.GetAll();
                CtlGrid.Bind(objCallCollection);
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "funSetGrid", "ctlCalllingList.xaml.cs"); 
            } 
        }

        private void btnSave_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            try
            {

            //CallingList Name Validation
            if (txtName.Text.Trim() == "")
            {
                System.Windows.MessageBox.Show("CallingList Name can't be left blank", "-> Please Enter a Name", MessageBoxButton.OK, MessageBoxImage.Information);
                txtName.Focus();
                txtName.Text = txtName.Text.Trim();
                return;
            }

            ClsCallingList c = new ClsCallingList();
            if (varState == 0)
                c.ID = -1;
            else
                c.ID = varID;
            c.ListName = txtName.Text.TrimEnd(' ').TrimStart(' ');
            c.IsActive = (bool)chkIsActive.IsChecked;
            c.IsDNCList = (bool)chkIsDNCList.IsChecked;
            c.CreatedBy = VMuktiAPI.VMuktiInfo.CurrentPeer.ID;
            Int64 gID = c.Save();

            if (gID == 0)
            {
                System.Windows.MessageBox.Show("Duplicate List Name Is Not Allowed !!", "-> Calling List", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                System.Windows.MessageBox.Show("Record Saved Successfully!!");
                funSetGrid();
                FncClearAll();
            }
        }

            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "btnSave_Click", "ctlCalllingList.xaml.cs");
            }
        }
        void txtName_GotFocus(object sender, RoutedEventArgs e)
        {
            try
            {
            txtName.SelectAll();
        }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "txtName_GotFocus", "ctlCalllingList.xaml.cs"); 
            }
        }
    }
}