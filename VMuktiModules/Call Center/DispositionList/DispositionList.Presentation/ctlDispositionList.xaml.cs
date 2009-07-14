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
using DispositionList.Business;
using System.Reflection;
using VMuktiAPI;



namespace DispositionList.Presentation
{
    public enum ModulePermissions
    {
        Add = 0,
        Edit = 1,
        Delete = 2,
        View = 3
    }

    public partial class CtlDispositionList : System.Windows.Controls.UserControl
    {

        int varState = 0;
        Int64 varID = 0;

        ClsDispositionListCollection objDispListCollection = null;
        ModulePermissions[] _MyPermissions;

        public CtlDispositionList(ModulePermissions[] MyPermissions)
        {
            try
            {
                InitializeComponent();
                this.Loaded += new RoutedEventHandler(CtlDispositionList_Loaded);

                varState = 0;
                _MyPermissions = MyPermissions;
                FncPermissionsReview();
                funSetGrid();
                funSetComboboxes();
                lstAvailableDispositions.SelectionMode = System.Windows.Controls.SelectionMode.Multiple;
                lstSelectedDispositions.SelectionMode = System.Windows.Controls.SelectionMode.Multiple;
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "CtlDispositionList()","CtlDispositionList.xaml.cs");
                
            }
        }

        void CtlDispositionList_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                this.Width = ((Grid)this.Parent).ActualWidth;
                ((Grid)this.Parent).SizeChanged += new SizeChangedEventHandler(CtlDispositionList_SizeChanged);
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "CtlDispositionList_Loaded()", "CtlDispositionList.xaml.cs");
            }            
        }

        void CtlDispositionList_SizeChanged(object sender, SizeChangedEventArgs e)
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
                tbcDispositionList.Visibility = Visibility.Collapsed;
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
                        tbcDispositionList.Visibility = Visibility.Visible;
                        btnSave.Visibility = Visibility.Visible;
                        btnCancel.Visibility = Visibility.Visible;
                    }

                }
            }

            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "FncPermissionsReview()", "CtlDispositionList.xaml.cs");
            }
        }
        
        void CtlGrid_btnDeleteClicked(int rowID)
        {
            try
            {
                varID = Convert.ToInt64(objDispListCollection[rowID].ID);
                MessageBoxResult r = System.Windows.MessageBox.Show("Do You Really Want To Delete This Record ?", "->Delete Disposition List", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (r == MessageBoxResult.Yes)
                {
                    ClsDispositionList.Delete(varID);
                    System.Windows.MessageBox.Show("Record Deleted!!", "DispositionList Delete", MessageBoxButton.OK, MessageBoxImage.Information);
                    funSetGrid();
                }
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "CtlGrid_btnDeleteClicked()", "CtlDispositionList.xaml.cs");            
            }
        }
    

        void CtlGrid_btnEditClicked(int rowID)
        {
            try
            {
                tbcDispositionList.Visibility = Visibility.Visible;
                btnSave.Visibility = Visibility.Visible;
                btnCancel.Visibility = Visibility.Visible;

                tbcDispositionList.Visibility = Visibility.Visible;
                varID = Convert.ToInt64(objDispListCollection[rowID].ID);

                txtName.Text = objDispListCollection[rowID].DispositionListName;
                txtDescription.Text = objDispListCollection[rowID].Description;
                if ((bool)objDispListCollection[rowID].IsActive == true)
                    chkIsActive.IsChecked = true;
                else
                    chkIsActive.IsChecked = false;

                ClsDispositionCollection AllDispositions = new ClsDispositionCollection();
                AllDispositions = ClsDispositionCollection.GetAll(varID);

                for (int i = 0; i < AllDispositions.Count; i++)
                {
                    ListBoxItem newItem = new ListBoxItem();
                    newItem.Content = AllDispositions[i].DispositionName;
                    newItem.Tag = AllDispositions[i].ID;

                    if (AllDispositions[i].IsActive)
                        newItem.Foreground = Brushes.Green;
                    else
                        newItem.Foreground = Brushes.Red;

                    lstSelectedDispositions.Items.Add(newItem);

                    for (int j = 0; j < lstAvailableDispositions.Items.Count; j++)
                    {
                        if (((ListBoxItem)lstAvailableDispositions.Items[j]).Tag.ToString() == AllDispositions[i].ID.ToString())
                            lstAvailableDispositions.Items.Remove(lstAvailableDispositions.Items[j]);
                    }
                }
                varState = 1;
                CtlGrid.IsEnabled = false;
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "CtlGrid_btnEditClicked()", "CtlDispositionList.xaml.cs");
            }
        }
       
        void funSetGrid()
        {
            try
            {
                CtlGrid.Cols = 4;
                //CtlGrid.CanEdit = true;
                //CtlGrid.CanDelete = true;

                CtlGrid.Columns[0].Header = "ID";
                CtlGrid.Columns[1].Header = "DispositionListName";
                CtlGrid.Columns[2].Header = "Is Active";
                CtlGrid.Columns[2].IsIcon = true;
                CtlGrid.Columns[3].Header = "Description";
                //CtlGrid.Columns[4].Header = "CreatedDate";
                //CtlGrid.Columns[5].Header = "CreatedBy";
                //CtlGrid.Columns[6].Header = "ModifiedDate";
                //CtlGrid.Columns[7].Header = "ModifeiedBy";

                CtlGrid.Columns[0].BindTo("ID");
                CtlGrid.Columns[1].BindTo("DispositionListName");
                CtlGrid.Columns[2].BindTo("IsActive");
                CtlGrid.Columns[3].BindTo("Description");
                //CtlGrid.Columns[4].BindTo("CreatedDate");
                //CtlGrid.Columns[5].BindTo("CreatedBy");
                //CtlGrid.Columns[6].BindTo("ModifiedDate");
                //CtlGrid.Columns[7].BindTo("ModifiedBy");

                objDispListCollection = ClsDispositionListCollection.GetAll();
                CtlGrid.Bind(objDispListCollection);
            }

            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "funSetGrid()", "CtlDispositionList.xaml.cs");
            }
        }

        void funSetComboboxes()
        {
            try
            {
            lstAvailableDispositions.Items.Clear();
            lstSelectedDispositions.Items.Clear();

            ClsDispositionCollection AllDispositions = ClsDispositionCollection.GetAll(-1);
            for (int i = 0; i < AllDispositions.Count; i++)
            {
                ListBoxItem newItem = new ListBoxItem();
                newItem.Content = AllDispositions[i].DispositionName;
                newItem.Tag = AllDispositions[i].ID;

                if (AllDispositions[i].IsActive)
                    newItem.Foreground = Brushes.Green;
                else
                    newItem.Foreground = Brushes.Red;
                lstAvailableDispositions.Items.Add(newItem);
            }
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "funSetComboboxes()", "CtlDispositionList.xaml.cs");
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
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "txtName_GotFocus()", "CtlDispositionList.xaml.cs");
            }
        }

        private void txtName_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
        }

        private void txtName_LostFocus(object sender, System.Windows.RoutedEventArgs e)
        {
        }

        private void txtDescription_GotFocus(object sender, System.Windows.RoutedEventArgs e)
        {
            try
            {
                txtDescription.SelectAll();
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "txtDescription_GotFocus()", "CtlDispositionList.xaml.cs");
            }
        }

        private void btnSearch_Click(object sender, System.Windows.RoutedEventArgs e)
        {
        }

        private void txtDescription_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {           
        }

        private void txtDescription_LostFocus(object sender, System.Windows.RoutedEventArgs e)
        {
        }

        private void tbiDispositionListDetails_GotFocus(object sender, System.Windows.RoutedEventArgs e)
        {           
        }

        private void tbiDispositionListDetails_LostFocus(object sender, System.Windows.RoutedEventArgs e)
        {
        }

        private void tbiDispositions_GotFocus(object sender, System.Windows.RoutedEventArgs e)
        {
        }

        private void tbiDispositions_LostFocus(object sender, System.Windows.RoutedEventArgs e)
        {          
        }

        private void lstAvailableDispositions_GotFocus(object sender, System.Windows.RoutedEventArgs e)
        {        
        }

        private void lstAvailableDispositions_LostFocus(object sender, System.Windows.RoutedEventArgs e)
        {           
        }

        private void lstAvailableDispositions_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
        }

        private void btnAddNew_Click(object sender, System.Windows.RoutedEventArgs e)
        {           
        }

        private void lstSelectedDispositions_GotFocus(object sender, System.Windows.RoutedEventArgs e)
        {            
        }

        private void lstSelectedDispositions_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {           
        }

        private void lstSelectedDispositions_LostFocus(object sender, System.Windows.RoutedEventArgs e)
        {          
        }

        private void lstSelectedDispositions_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {          
        }

        private void btnSelectAllAD_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            try
            {
                lstAvailableDispositions.SelectAll();
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "btnSelectAllAD_Click()", "CtlDispositionList.cs");                            
            }
        }

        private void btnSelectNoneAD_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            try
            {
                lstAvailableDispositions.UnselectAll();
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "btnSelectNoneAD_Click()", "CtlDispositionList.cs");
                
            }
        }

        private void btnSelectAllDisposition_Click(object sender, System.Windows.RoutedEventArgs e)
        {          
        }

        private void btnSelectDisposition_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            try
            {
                ListBoxItem[] lbi = new ListBoxItem[lstAvailableDispositions.SelectedItems.Count];
                lstAvailableDispositions.SelectedItems.CopyTo(lbi, 0);
                for (int i = 0; i < lbi.Length; i++)
                {
                    lstAvailableDispositions.Items.Remove(lbi[i]);
                    lstSelectedDispositions.Items.Add(lbi[i]);
                }
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "btnSelectDisposition_Click()", "CtlDispositionList.cs");                            
            }
        }

        private void btnDeselectDisposition_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            try
            {
                ListBoxItem[] lbi = new ListBoxItem[lstSelectedDispositions.SelectedItems.Count];
                lstSelectedDispositions.SelectedItems.CopyTo(lbi, 0);
                for (int i = 0; i < lbi.Length; i++)
                {
                    lstSelectedDispositions.Items.Remove(lbi[i]);
                    lstAvailableDispositions.Items.Add(lbi[i]);
                }
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "btnDeselectDisposition_Click()", "CtlDispositionList.cs");                            
            }
        }

        private void btnDeselectAllDisposition_Click(object sender, System.Windows.RoutedEventArgs e)
        {           
        }

        private void btnMoveDown_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            try
            {
                if (lstSelectedDispositions.SelectedItems.Count == 1)
                {
                    ListBoxItem lbiTemp = new ListBoxItem();
                    lbiTemp = (ListBoxItem)lstSelectedDispositions.SelectedItem;

                    int varIndex = lstSelectedDispositions.SelectedIndex;

                    if (lstSelectedDispositions.SelectedIndex != lstSelectedDispositions.Items.Count - 1)
                    {
                        lstSelectedDispositions.Items.RemoveAt(lstSelectedDispositions.SelectedIndex);
                        lstSelectedDispositions.Items.Insert(varIndex + 1, lbiTemp);
                    }
                    else
                    {
                        lstSelectedDispositions.Items.RemoveAt(lstSelectedDispositions.SelectedIndex);
                        lstSelectedDispositions.Items.Insert(varIndex, lbiTemp);
                    }
                    lstSelectedDispositions.SelectedIndex = varIndex + 1;
                }
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "btnMoveDown_Click()", "CtlDispositionList.cs");                            
            }
        }

        private void btnSelectAllSD_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            try
            {
                lstSelectedDispositions.SelectAll();
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "btnSelectAllSD_Click()", "CtlDispositionList.cs");                                            
            }
        }

        private void btnSelectNoneSD_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            try
            {
                lstSelectedDispositions.UnselectAll();
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "btnSelectNoneSD_Click()", "CtlDispositionList.cs");                            
                
            }
        }

        private void btnSave_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            try
            {
                if (txtName.Text.Trim() == "")
                {
                    System.Windows.MessageBox.Show("DispositionList Name cannot be left Blank", "-> Please Enter a Name", MessageBoxButton.OK, MessageBoxImage.Information);
                    txtName.Focus();
                    txtName.Text = txtName.Text.Trim();
                    return;
                }

                string strDispositions = "";

                for (int i = 0; i < lstSelectedDispositions.Items.Count; i++)
                {
                    if (i == 0)
                        strDispositions = ((ListBoxItem)lstSelectedDispositions.Items[i]).Tag.ToString();
                    else
                        strDispositions = strDispositions + "," + ((ListBoxItem)lstSelectedDispositions.Items[i]).Tag.ToString();
                }
                ClsDispositionList c = new ClsDispositionList();
                if (varState == 0)
                {
                    c.ID = -1;
                }
                else
                {
                    c.ID = varID;

                }
                c.DispositionListName = txtName.Text.Trim();
                c.Description = txtDescription.Text.Trim();
                c.IsActive = (bool)chkIsActive.IsChecked;
                c.CreatedBy = VMuktiAPI.VMuktiInfo.CurrentPeer.ID;
                Int64 gID = c.Save();

                if (gID == 0)
                {
                    System.Windows.MessageBox.Show("Duplicate Entry For DispositionList Is Not Allowed !!", "-> Disposition List", MessageBoxButton.OK, MessageBoxImage.Information);
                }

                else
                {
                    if (varState != 0)
                    {
                        ClsDisposition u1 = new ClsDisposition();
                        u1.ID = gID;
                        u1.Delete();
                    }

                    for (int i = 0; i < lstSelectedDispositions.Items.Count; i++)
                    {
                        ClsDisposition u = new ClsDisposition();
                        u.ID = int.Parse(((ListBoxItem)lstSelectedDispositions.Items[i]).Tag.ToString());
                        u.DispositionListId = gID;
                        u.Save();
                    }

                    System.Windows.MessageBox.Show("Record Saved Successfully!!", "-> Disposition List", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    FnClearAll();

                }
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "btnSave_Click()", "CtlDispositionList.cs");                            
            }
        }

        private void btnCancel_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            try
            {
                FnClearAll();
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "btnCancel_Click()", "CtlDispositionList.cs");                            
                
            }
            
        }

        private void FnClearAll()
        {
            try
            {
                txtDescription.Text = "";
                txtName.Text = "";
                chkIsActive.IsChecked = false;
                funSetComboboxes();
                FncPermissionsReview();
                funSetGrid();
                tbiDispositionListDetails.IsSelected = true;
                CtlGrid.IsEnabled = true;
                varID = 0;
                varState = 0;
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "FnClearAll()", "CtlDispositionList.cs");                            
                
            }

        }

        private void chkIsActive_LostFocus(object sender, System.Windows.RoutedEventArgs e)
        {           
        }

        private void chkIsActive_GotFocus(object sender, System.Windows.RoutedEventArgs e)
        {          
        }

        private void chkIsActive_Checked(object sender, System.Windows.RoutedEventArgs e)
        {           
        }

        private void btnMoveUp_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            try
            {
                if (lstSelectedDispositions.SelectedItems.Count == 1)
                {
                    ListBoxItem lbiTemp = new ListBoxItem();
                    lbiTemp = (ListBoxItem)lstSelectedDispositions.SelectedItem;

                    int varIndex = lstSelectedDispositions.SelectedIndex;
                    if (lstSelectedDispositions.SelectedIndex != 0)
                    {
                        lstSelectedDispositions.Items.RemoveAt(lstSelectedDispositions.SelectedIndex);
                        lstSelectedDispositions.Items.Insert(varIndex - 1, lbiTemp);
                    }
                    else
                    {
                        lstSelectedDispositions.Items.RemoveAt(lstSelectedDispositions.SelectedIndex);
                        lstSelectedDispositions.Items.Insert(varIndex, lbiTemp);
                    }
                    lstSelectedDispositions.SelectedIndex = varIndex - 1;
                }
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "btnMoveUp_Click()", "CtlDispositionList.cs");                            
                
            }
        }

       
       
    }
}
