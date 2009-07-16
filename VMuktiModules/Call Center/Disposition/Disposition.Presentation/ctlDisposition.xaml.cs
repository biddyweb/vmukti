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
using Disposition.Business;
using VMuktiAPI;

namespace Disposition.Presentation
{
    //Set permission for Disposition Module
    public enum ModulePermissions
    {
        Add = 0,
        Edit = 1,
        Delete = 2,
        View = 3
    }

    public partial class UserControl2 : System.Windows.Controls.UserControl
    {
        int varState = 0;
        Int64 varID = 0;

        DataTable dtDisposition = new DataTable();

        ClsDispositionCollection objDisCollection = null;
        ModulePermissions[] _MyPermissions;

        public UserControl2(ModulePermissions[] MyPermissions)
        {
            try
            {
                InitializeComponent();
                varState = 0;
                _MyPermissions = MyPermissions;
                FncPermissionsReview();
                //set data in Grid
                funSetGrid();
                this.Loaded += new RoutedEventHandler(UserControl2_Loaded);
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "UserControl2()", "clsDisposition.xaml.cs");
                
            }
            btnCancel.Click += new RoutedEventHandler(btnCancel_Click);
        }

        void UserControl2_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                this.Width = ((Grid)this.Parent).ActualWidth;
                ((Grid)this.Parent).SizeChanged += new SizeChangedEventHandler(UserControl2_SizeChanged);
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "UserControl2_Loaded", "CtlDisposition.xaml.cs");
            }
        }

        void UserControl2_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            try
            {
                this.Width = e.NewSize.Width;
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "UserControl2_SizeChanged", "CtlDisposition.xaml.cs");
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
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "FncPermissionsReview()", "clsDisposition.xaml.cs");
               
            }
        }


        //To Delete Existing Disposition
        void CtlGrid_btnDeleteClicked(int rowID)
        {
            try
            {
                varID = Convert.ToInt64(objDisCollection[rowID].ID);
                MessageBoxResult r = System.Windows.MessageBox.Show("Do You Really Want To Delete This Record ?", "->Delete Disposition", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (r == MessageBoxResult.Yes)
                {
                    //call Delete function from Disposition.Business
                    ClsDisposition.Delete(varID);
                    System.Windows.MessageBox.Show("Record Deleted!!", "->Disposition", MessageBoxButton.OK, MessageBoxImage.Information);
                    //funSetGrid function is called to set data in Grid
                    funSetGrid();
                }
                //FncClearAll function is called to clear all data
                FncClearAll();
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "CtlGrid_btnDeleteClicked()", "clsDisposition.xaml.cs");

            }
        }

        //To Edit Existing Disposition
        void CtlGrid_btnEditClicked(int rowID)
        {
            try
            {
                tbcUserAdditon.Visibility = Visibility.Visible;
                btnSave.Visibility = Visibility.Visible;
                btnCancel.Visibility = Visibility.Visible;

                varID = Convert.ToInt64(objDisCollection[rowID].ID);
                ClsDisposition.GetByDispositionID(varID);

                ClsDisposition objDisposition = new ClsDisposition();
                //get edited record from function GetByDispositionID
                objDisposition = ClsDisposition.GetByDispositionID(varID);

                txtName.Text = objDisposition.DespositionName;

                if ((bool)objDisposition.IsActive == true)
                    chkIsActive.IsChecked = true;
                else
                    chkIsActive.IsChecked = false;

                txtDescription.Text = objDisposition.Description;

                varState = 1;
                CtlGrid.IsEnabled = false;
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "CtlGrid_btnEditClicked()", "clsDisposition.xaml.cs");

            }
        }

        //To cancel editing or deleting
        void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                funSetGrid();
                varState = 0;
                FncClearAll();
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "btnCancel_Click()", "clsDisposition.xaml.cs");
            }
        }

        private void FncClearAll()
        {
            try
            {
                txtName.Text = "";
                chkIsActive.IsChecked = false;
                txtDescription.Text = "";
                CtlGrid.IsEnabled = true;
                FncPermissionsReview();
                varID = 0;
                varState = 0;
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "FncClearAll()", "clsDisposition.xaml.cs");

            }
        }
        
        //function for set data in Grid
        void funSetGrid()
        {
            try
            {
                CtlGrid.Cols = 4;
                //CtlGrid.CanEdit = true;
                //CtlGrid.CanDelete = true;

                //set header for Grid
                CtlGrid.Columns[0].Header = "ID";
                CtlGrid.Columns[1].Header = "Desposition Name";
                CtlGrid.Columns[2].Header = "Is Active";
                CtlGrid.Columns[2].IsIcon = true;
                CtlGrid.Columns[3].Header = "Description";
                //CtlGrid.Columns[4].Header = "CreatedDate";
                //CtlGrid.Columns[5].Header = "CreatedBy";
                //CtlGrid.Columns[6].Header = "ModifiedDate";
                //CtlGrid.Columns[7].Header = "ModifeiedBy";

                //Binding data for Grid
                CtlGrid.Columns[0].BindTo("ID");
                CtlGrid.Columns[1].BindTo("DespositionName");
                CtlGrid.Columns[2].BindTo("IsActive");
                CtlGrid.Columns[3].BindTo("Description");
                //CtlGrid.Columns[4].BindTo("CreatedDate");
                //CtlGrid.Columns[5].BindTo("CreatedBy");
                //CtlGrid.Columns[6].BindTo("ModifiedDate");
                //CtlGrid.Columns[7].BindTo("ModifiedBy");

                //calling the function to get all Disposition from Disposition.Business
                objDisCollection = ClsDispositionCollection.GetAll();
                CtlGrid.Bind(objDisCollection);
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "funSetGrid()", "clsDisposition.xaml.cs");

            }
        }

        //To Save New Data
        private void btnSave_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            try
            {
                //Disposition Validations
                if (txtName.Text.Trim() == "")
                {
                    System.Windows.MessageBox.Show("DispositionName cannot be left Blank ", "-> Please Enter a Name", MessageBoxButton.OK, MessageBoxImage.Information);
                    txtName.Focus();
                    txtName.Text = txtName.Text.Trim();
                    return;
                }

                ClsDisposition d = new ClsDisposition();
                d.DespositionName = txtName.Text.Trim();
                d.IsActive = (bool)chkIsActive.IsChecked;
                d.Description = txtDescription.Text.Trim();
                d.CreatedBy = VMuktiAPI.VMuktiInfo.CurrentPeer.ID;
                if (varState == 0)
                    d.ID = -1;
                else
                    d.ID = varID;

                //call Save() function from Disposition.Business
                Int64 gID = d.Save();

                if (gID <= 0)
                {
                    System.Windows.MessageBox.Show("Duplicate Entry Found!!", "-> Disposition", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                }
                else
                {
                    System.Windows.MessageBox.Show("Record Saved Successfully!!", "-> Disposition", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    //funSetGrid function is called to set data in Grid
                    funSetGrid();
                    varState = 0;
                    //FncClearAll function is called to Clear-All data 
                    FncClearAll();
                }
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "btnSave_Click()", "clsDisposition.xaml.cs");
            }
        }

        void txtDescription_GotFocus(object sender, RoutedEventArgs e)
        {
            try
            {
                txtDescription.SelectAll();
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "txtDescription_GotFocus()", "clsDisposition.xaml.cs");
                

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
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "txtName_GotFocus()", "clsDisposition.xaml.cs");
                

            }
        }

     }
}