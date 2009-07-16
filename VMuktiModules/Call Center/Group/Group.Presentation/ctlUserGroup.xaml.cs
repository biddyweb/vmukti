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
using Group.Business;
using System.Reflection;
using VMuktiAPI;

namespace Group.Presentation
{
    //Set permission for Group module
    public enum ModulePermissions
    {
        Add = 0,
        Edit = 1,
        Delete = 2,
        View = 3
    }

    public partial class UserControl1 : System.Windows.Controls.UserControl
    {
        
        int varState=0;
        int varID = 0;
        Group.Business.ClsGroupCollection _MyGroups = null;
        ModulePermissions[] _MyPermissions;

        public UserControl1(ModulePermissions[] MyPermissions)
        {
            try
            {
                varState = 0;
                InitializeComponent();
                this.Loaded += new RoutedEventHandler(UserControl1_Loaded);

                _MyPermissions = MyPermissions;
                FncPermissionsReview();

                funSetGrid();
                funSetComboboxes();
                lstAvailableAgents.SelectionMode = System.Windows.Controls.SelectionMode.Multiple;
                lstSelectedAgents.SelectionMode = System.Windows.Controls.SelectionMode.Multiple;

                tbiGroup.IsSelected = true;
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "UserControl1", "ctlUserGroup.xaml.cs");
            }

        }

        void UserControl1_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                this.Width = ((Grid)this.Parent).ActualWidth;
                ((Grid)this.Parent).SizeChanged += new SizeChangedEventHandler(UserControl1_SizeChanged);
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "UserControl1_Loaded", "ctlUserGroup.xaml.cs");
            }
        }

        void UserControl1_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            this.Width = e.NewSize.Width;
        }
        //Function for setting permissions
        void FncPermissionsReview()
        {
            try
            {
                CtlGrid.CanEdit = false;
                CtlGrid.CanDelete = false;
                CtlGrid.Visibility = Visibility.Collapsed;
                tbcUserGroup.Visibility = Visibility.Collapsed;
                btnGroupSave.Visibility = Visibility.Collapsed;
                btnGroupCancel.Visibility = Visibility.Collapsed;

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
                        tbcUserGroup.Visibility = Visibility.Visible;
                        btnGroupSave.Visibility = Visibility.Visible;
                        btnGroupCancel.Visibility = Visibility.Visible;
                    }
                }
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex,"FncPermissionsReview", "ctlUserGroup.xaml.cs");
            }
        }
        //Function for setting data in grid
        void funSetGrid()
        {
            try
            {
                CtlGrid.Cols = 4;
                //CtlGrid.CanEdit = true;
                //CtlGrid.CanDelete = true;

                //set header of ctlgrid
                CtlGrid.Columns[0].Header = "Group ID";
                CtlGrid.Columns[1].Header = "Group Name";
                CtlGrid.Columns[2].Header = "Is Active";
                CtlGrid.Columns[2].IsIcon = true;
                CtlGrid.Columns[3].Header = "Description";

                CtlGrid.Columns[0].BindTo("ID");
                CtlGrid.Columns[1].BindTo("GroupName");
                CtlGrid.Columns[2].BindTo("IsActive");
                CtlGrid.Columns[3].BindTo("Description");

                //Calling the GetAll() function for getting all data
                _MyGroups = Group.Business.ClsGroupCollection.GetAll();
                //Bind data to CtlGrid
                CtlGrid.Bind(_MyGroups);
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "funSetGrid", "ctlUserGroup.xaml.cs");
               
            }
        }
        //To Edit Group
        void CtlGrid_btnEditClicked(int rowID)
        {
            try
            {
                CtlGrid.IsEnabled = false;

                tbcUserGroup.Visibility = Visibility.Visible;
                btnGroupSave.Visibility = Visibility.Visible;
                btnGroupCancel.Visibility = Visibility.Visible;

                varID = _MyGroups[rowID].ID;

                txtName.Text = _MyGroups[rowID].GroupName;
                txtDescription.Text = _MyGroups[rowID].Description;
                if ((bool)_MyGroups[rowID].IsActive == true)
                    chkIsActive.IsChecked = true;
                else
                    chkIsActive.IsChecked = false;

                ClsUserCollection AllUsers = new ClsUserCollection();
                AllUsers = ClsUserCollection.GetAll(varID);

                for (int i = 0; i < AllUsers.Count; i++)
                {
                    ListBoxItem newItem = new ListBoxItem();
                    newItem.Content = AllUsers[i].AgentName;
                    newItem.Tag = AllUsers[i].ID;
                    lstSelectedAgents.Items.Add(newItem);

                    for (int j = 0; j < lstAvailableAgents.Items.Count; j++)
                    {
                        if (((ListBoxItem)lstAvailableAgents.Items[j]).Tag.ToString() == AllUsers[i].ID.ToString())
                            lstAvailableAgents.Items.Remove(lstAvailableAgents.Items[j]);
                    }
                }
                varState = 1;
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "CtlGrid_btnEditClicked", "ctlUserGroup.xaml.cs");

            }
        }
        //To delete data 
        void CtlGrid_btnDeleteClicked(int ID)
        {
            try
            {
                varID = _MyGroups[ID].ID;
                if (MessageBox.Show("Do You Really Want To Delete This Record ?", "Delete Group", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    //Calling Delete function 
                    ClsGroup.Delete(_MyGroups[ID].ID);
                    MessageBox.Show("Record Deleted!!", "Group Deleted", MessageBoxButton.OK, MessageBoxImage.Information);
                    //Function for setting the grid after deleting record
                    funSetGrid();
                }
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "CtlGrid_btnDeleteClicked", "ctlUserGroup.xaml.cs");

            }
        }
        //set data in AvailableAgents listbox and SelectedAgents listbox
        void funSetComboboxes()
        {
            try
            {
                lstAvailableAgents.Items.Clear();
                lstSelectedAgents.Items.Clear();
                //Calling GetAll function for getting users which are not assigned to group
                ClsUserCollection AllUser = ClsUserCollection.GetAll(-1);

                for (int i = 0; i < AllUser.Count; i++)
                {
                    ListBoxItem newItem = new ListBoxItem();
                    newItem.Content = AllUser[i].AgentName;
                    newItem.Tag = AllUser[i].ID;
                    lstAvailableAgents.Items.Add(newItem);
                }
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "fncSetComboboxes", "ctlUserGroup.xaml.cs");
               
            }
        }

        private void txtName_GotFocus(object sender, System.Windows.RoutedEventArgs e)
        {
          
        }
        private void txtName_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
                       
        }
        private void txtName_LostFocus(object sender, System.Windows.RoutedEventArgs e)
        {
            
        }
        private void txtDescription_GotFocus(object sender, System.Windows.RoutedEventArgs e)
        {
            
        }
        private void txtDescription_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            
        }
        private void txtDescription_LostFocus(object sender, System.Windows.RoutedEventArgs e)
        {
            
        }
        private void tbiGroup_GotFocus(object sender, System.Windows.RoutedEventArgs e)
        {
            
        }
        private void tbiGroup_LostFocus(object sender, System.Windows.RoutedEventArgs e)
        {
            
        }
        private void tbiAgents_GotFocus(object sender, System.Windows.RoutedEventArgs e)
        {
            
        }
        private void tbiAgents_LostFocus(object sender, System.Windows.RoutedEventArgs e)
        {
            
        }
        private void lstAvailableAgents_GotFocus(object sender, System.Windows.RoutedEventArgs e)
        {
            
        }
        private void lstAvailableAgents_LostFocus(object sender, System.Windows.RoutedEventArgs e)
        {
            
        }
        private void lstAvailableAgents_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            
        }
        private void btnAddNew_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            
        }
        private void lstSelectedAgents_GotFocus(object sender, System.Windows.RoutedEventArgs e)
        {
            
        }

        private void lstSelectedAgents_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            
        }

        private void lstSelectedAgents_LostFocus(object sender, System.Windows.RoutedEventArgs e)
        {
            
        }

        private void lstSelectedAgents_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            
        }
        
        private void btnSelectAllAA_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            try
            {
                //selecting all data of lstAvailableAgents
                lstAvailableAgents.SelectAll();
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "btnSelectAllAA_Click", "ctlUserGroup.xaml.cs");
            }
        }

        private void btnSelectNoneAA_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            try
            {
                //Deselecting Data of lstAvailableAgents
                lstAvailableAgents.UnselectAll();
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "btnSelectNoneAA_Click", "ctlUserGroup.xaml.cs");

            }
        }

        private void btnSelectAllAgent_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            
        }

        private void btnSelectAgent_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            try
            {
                ListBoxItem[] lbi = new ListBoxItem[lstAvailableAgents.SelectedItems.Count];
                lstAvailableAgents.SelectedItems.CopyTo(lbi, 0);
                for (int i = 0; i < lbi.Length; i++)
                {
                    //Transfering data from lstAvailableAgents to lstSelectedAgents
                    lstAvailableAgents.Items.Remove(lbi[i]);
                    lstSelectedAgents.Items.Add(lbi[i]);
                }
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "btnSelectAgent_Click", "ctlUserGroup.xaml.cs");
            }
        }

        private void btnDeselectAgent_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            try
            {
                ListBoxItem[] lbi = new ListBoxItem[lstSelectedAgents.SelectedItems.Count];
                lstSelectedAgents.SelectedItems.CopyTo(lbi, 0);
                for (int i = 0; i < lbi.Length; i++)
                {
                    //Transfering Data from lstSelectedAgents to lstAvailableAgents
                    lstSelectedAgents.Items.Remove(lbi[i]);
                    lstAvailableAgents.Items.Add(lbi[i]);
                }
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "btnDeselectAgent_Click", "ctlUserGroup.xaml.cs");
            }
        }

        private void btnDeselectAllAgent_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            
        }
        //Button for Movedown selected items of lstSlectedAgents
        private void btnMoveDown_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            try
            {
                if (lstSelectedAgents.SelectedItems.Count == 1)
                {
                    ListBoxItem lbiTemp = new ListBoxItem();
                    lbiTemp = (ListBoxItem)lstSelectedAgents.SelectedItem;
                    int varIndex = lstSelectedAgents.SelectedIndex;

                    if (lstSelectedAgents.SelectedIndex != lstSelectedAgents.Items.Count - 1)
                    {
                        lstSelectedAgents.Items.RemoveAt(lstSelectedAgents.SelectedIndex);
                        lstSelectedAgents.Items.Insert(varIndex + 1, lbiTemp);
                    }
                    else
                    {
                        lstSelectedAgents.Items.RemoveAt(lstSelectedAgents.SelectedIndex);
                        lstSelectedAgents.Items.Insert(varIndex, lbiTemp);
                    }
                    lstSelectedAgents.SelectedIndex = varIndex + 1;
                }
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "btnMoveDown_Click", "ctlUserGroup.xaml.cs");
            }
        }
        //SelectAll items of lstSelectedAgents
        private void btnSelectAllSA_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            try
            {
                lstSelectedAgents.SelectAll();
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "btnSelectAllSA_Click", "ctlUserGroup.xaml.cs");

            }
        }
        //Deselect items of lstSelectedAgents
        private void btnSelectNoneSA_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            try
            {
                lstSelectedAgents.UnselectAll();
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "btnSelectNoneSA_Click", "ctlUserGroup.xaml.cs");

            }
        }
        //To save new Record
        private void btnGroupSave_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            try
            {
                //Validations for group
                #region GroupName Validation
                if (txtName.Text.Trim() == "")
                {
                    MessageBox.Show("Group Name Can't be left Blank ", "-> Please Enter a Name", MessageBoxButton.OK, MessageBoxImage.Information);
                    txtName.Focus();
                    txtName.Text = txtName.Text.Trim();
                    return;
                }

                #endregion


                string strUsers = "";

                for (int i = 0; i < lstSelectedAgents.Items.Count; i++)
                {
                    if (i == 0)
                        strUsers = ((ListBoxItem)lstSelectedAgents.Items[i]).Tag.ToString();
                    else
                        strUsers = strUsers + "," + ((ListBoxItem)lstSelectedAgents.Items[i]).Tag.ToString();
                }

                int GetId = 0;
                ClsGroup c = new ClsGroup();

                if (varState == 0)
                {
                    c.ID = -1;
                }
                else
                {
                    c.ID = varID;
                }

                c.GroupName = txtName.Text.Trim();
                c.Description = txtDescription.Text.Trim();
                c.IsActive = (bool)chkIsActive.IsChecked;
                c.CreatedBy = VMuktiAPI.VMuktiInfo.CurrentPeer.ID;
                //Function to save new record 
                int gID = c.Save();

                if (gID == 0)
                {
                    MessageBox.Show("User Group With Same Name is Not Allowed", "-> User Group", MessageBoxButton.OK, MessageBoxImage.Information);
                }

                else
                {

                    if (varState != 0)
                    {
                        ClsUser u1 = new ClsUser();
                        u1.ID = gID;
                        u1.Delete();
                    }

                    for (int i = 0; i < lstSelectedAgents.Items.Count; i++)
                    {
                        ClsUser u = new ClsUser();
                        u.ID = int.Parse(((ListBoxItem)lstSelectedAgents.Items[i]).Tag.ToString());
                        //u.AgentName = ((ListBoxItem)lstSelectedAgents.Items[i]).Content.ToString();
                        u.GroupId = gID;
                        //Function to Save record in usergroup 
                        u.Save();
                    }

                    System.Windows.MessageBox.Show("Record Saved Successfully!!");
                    FncClearAll();
                    //set data in grid
                    funSetGrid();
                    //set data in listboxes
                    funSetComboboxes();
                }
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "btnGroupSave_Click", "ctlUserGroup.xaml.cs");

            }

        }
        //function for clearing all data
        void FncClearAll()
        {
            try
            {
                varState = 0;
                varID = 0;
                txtDescription.Text = "";
                txtName.Text = "";
                chkIsActive.IsChecked = false;
                tbiGroup.IsSelected = true;
                CtlGrid.IsEnabled = true;
                FncPermissionsReview();
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "FncClearAll", "ctlUserGroup.xaml.cs");
            }
        }
        //Canceling editing or deleting
        private void btnGroupCancel_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            try
            {
                FncClearAll();
                funSetGrid();
                funSetComboboxes();
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "btnGroupCancel_Click", "ctlUserGroup.xaml.cs");
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

        private void cmbFields_GotFocus(object sender, System.Windows.RoutedEventArgs e)
        {
        }

        private void cmbFields_LostFocus(object sender, System.Windows.RoutedEventArgs e)
        {
        }

        private void cmbFields_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
        }

        private void cmbOperators_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
        }

        private void cmbOperators_LostFocus(object sender, System.Windows.RoutedEventArgs e)
        {
        }

        private void cmbOperators_GotFocus(object sender, System.Windows.RoutedEventArgs e)
        {
        }

        private void txtValue_GotFocus(object sender, System.Windows.RoutedEventArgs e)
        {
        }

        private void txtValue_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
        }

        private void txtValue_LostFocus(object sender, System.Windows.RoutedEventArgs e)
        {
        }

        private void btnSearch_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
        }

        private void btnSearch_Click(object sender, System.Windows.RoutedEventArgs e)
        {
        }
        //Button for moving up lstSelectedAgets items
        private void btnMoveUp_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            try
            {
                if (lstSelectedAgents.SelectedItems.Count == 1)
                {
                    ListBoxItem lbiTemp = new ListBoxItem();
                    lbiTemp = (ListBoxItem)lstSelectedAgents.SelectedItem;
                    int varIndex = lstSelectedAgents.SelectedIndex;
                    if (lstSelectedAgents.SelectedIndex != 0)
                    {
                        lstSelectedAgents.Items.RemoveAt(lstSelectedAgents.SelectedIndex);
                        lstSelectedAgents.Items.Insert(varIndex - 1, lbiTemp);
                    }
                    else
                    {
                        lstSelectedAgents.Items.RemoveAt(lstSelectedAgents.SelectedIndex);
                        lstSelectedAgents.Items.Insert(varIndex, lbiTemp);
                    }
                    lstSelectedAgents.SelectedIndex = varIndex - 1;
                }
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "btnMoveUp_Click", "ctlUserGroup.xaml.cs");
            }
        }
 
    }
}