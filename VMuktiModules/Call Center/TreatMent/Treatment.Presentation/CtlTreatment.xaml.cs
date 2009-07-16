/* VMukti 2.0 -- An Open Source Unified Communications Engine
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
using Treatment.Business;
using VMuktiAPI;
using System.Data;
using System.Reflection;

namespace Treatment.Presentation
{
    /// <summary>
    /// Interaction logic for CtlTreatment.xaml
    /// </summary>
    /// 
    public enum ModulePermissions
    {
        Add = 0,
        Edit = 1,
        Delete = 2,
        View = 3
    }

    public partial class CtlTreatment : UserControl
    {
        ModulePermissions[] _MyPermissions;
        ClsTreatmentCollection objTreatmentCollection = null;
        ClsTreatmentConditionCollection objTreatmentConditionCollection = null;
        string strTreatmentType = "TreatmentOn-Field";
        int VarState = 0; 
        int varID = -1;
        string selectedValue = "";

        public CtlTreatment(ModulePermissions[] MyPermissions)
        {
            try
            {
                InitializeComponent();
                _MyPermissions = MyPermissions;
                btnAdd.Click += new RoutedEventHandler(btnAdd_Click);
                btnDelete.Click += new RoutedEventHandler(btnDelete_Click);
                btnEdit.Click += new RoutedEventHandler(btnEdit_Click);
                cmbField.SelectionChanged += new SelectionChangedEventHandler(cmbField_SelectionChanged);
                cmbOperator.SelectionChanged+=new SelectionChangedEventHandler(cmbOperator_SelectionChanged);
                fncPermissionsReview();
                fncSetGrid();
                fncSetComboboxes();
            }
            catch(Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "CtlTreatment()", "CtlTreatment.xaml.cs");
            }
        }

        private void rdFieldValue_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
                cmbCampaign.SelectedIndex = -1;
                cmbHours.SelectedIndex = -1;
                cmbMins.SelectedIndex = -1;
                cmbValues.Items.Clear();
                strTreatmentType = "TreatmentOn-Field";
                fncSetComboboxes();
                lstFill.Visibility = Visibility.Hidden;
                lblValues.Visibility = Visibility.Visible;
                lblOperator.Visibility = Visibility.Visible;
                cmbOperator.Visibility = Visibility.Visible;
                lblDisposition1.Visibility = Visibility.Collapsed;
                cmbHours.Visibility = Visibility.Collapsed;
                cmbMins.Visibility = Visibility.Collapsed;
                lblHours.Visibility = Visibility.Collapsed;
                lblMinutes.Visibility = Visibility.Collapsed;
                lblLeadFormat.Visibility = Visibility.Visible;
                cmbLeadFormat.Visibility = Visibility.Visible;
                lblCampaign.Visibility = Visibility.Collapsed;
                cmbCampaign.Visibility = Visibility.Collapsed;
                lblField.Visibility = Visibility.Visible;
                cmbField.Visibility = Visibility.Visible;
                lblDisposition.Visibility = Visibility.Collapsed;
            }
            catch(Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "rdFieldValue_Checked()", "CtlTreatment.xaml.cs");
            }
        }

        private void rdDisposition_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
                cmbLeadFormat.SelectedIndex = -1;
                cmbField.Items.Clear();
                lblExample.Visibility = Visibility.Hidden;
                cmbValues.Visibility = Visibility.Visible;
                txtValues.Visibility = Visibility.Hidden;
                lstFill.Visibility = Visibility.Hidden;
                //cmbOperator.Items.Clear();
                //fncSetComboboxes();
                strTreatmentType = "TreatmentOn-Disposition";
                lblLeadFormat.Visibility = Visibility.Collapsed;
                cmbLeadFormat.Visibility = Visibility.Collapsed;
                lblCampaign.Visibility = Visibility.Visible;
                cmbCampaign.Visibility = Visibility.Visible;
                lblField.Visibility = Visibility.Collapsed;
                cmbField.Visibility = Visibility.Collapsed;
                lblDisposition.Visibility = Visibility.Visible;
                cmbMins.Visibility = Visibility.Visible;
                cmbHours.Visibility = Visibility.Visible;
                cmbOperator.Visibility = Visibility.Hidden;
                lblOperator.Visibility = Visibility.Collapsed;
                lblHours.Visibility = Visibility.Visible;
                lblMinutes.Visibility = Visibility.Visible;
                lblValues.Visibility = Visibility.Collapsed;
                lblDisposition1.Visibility = Visibility.Visible;
            }
            catch(Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "rdDisposition_Checked()", "CtlTreatment.xaml.cs");
            }
        }

        private void rdFilter_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
                cmbCampaign.SelectedIndex = -1;
                cmbHours.SelectedIndex = -1;
                cmbMins.SelectedIndex = -1;
                cmbValues.Items.Clear();
                strTreatmentType = "Filter";
                lstFill.Visibility = Visibility.Hidden;
                txtValues.Visibility = Visibility.Visible;
                cmbValues.Visibility = Visibility.Hidden;
                txtValues.Text = "";
                fncSetComboboxes();
                lblValues.Visibility = Visibility.Visible;
                lblOperator.Visibility = Visibility.Visible;
                cmbOperator.Visibility = Visibility.Visible;
                lblDisposition1.Visibility = Visibility.Collapsed;
                cmbHours.Visibility = Visibility.Collapsed;
                cmbMins.Visibility = Visibility.Collapsed;
                lblHours.Visibility = Visibility.Collapsed;
                lblMinutes.Visibility = Visibility.Collapsed;
                lblLeadFormat.Visibility = Visibility.Visible;
                cmbLeadFormat.Visibility = Visibility.Visible;
                lblCampaign.Visibility = Visibility.Collapsed;
                cmbCampaign.Visibility = Visibility.Collapsed;
                lblField.Visibility = Visibility.Visible;
                cmbField.Visibility = Visibility.Visible;
                lblDisposition.Visibility = Visibility.Collapsed;
                //if (lstConditionsCreated.Items.Count > 0)
                //{
                //    btnAdd.IsEnabled = false;
                //}
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "rdFilter_Checked()", "CtlTreatment.xaml.cs");
            }

        }

        private void cmbField_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (cmbField.SelectedItem != null)
                {
                    if (strTreatmentType == "Filter")
                    {
                        txtValues.Text = "";
                        if (cmbField.Items.Count > 0)
                        {
                            selectedValue = ((ComboBoxItem)cmbField.SelectedItem).Content.ToString();
                            lstFill.SelectionMode = SelectionMode.Extended;
                            lblExample.Visibility = Visibility.Hidden;
                            cmbValues.Visibility = Visibility.Hidden;
                            txtValues.Visibility = Visibility.Visible;
                            if ((selectedValue.ToLower() == "timezone") || (selectedValue.ToLower() == "country") || (selectedValue.ToLower() == "areacode") || (selectedValue.ToLower() == "state"))
                            {
                                DataSet ds = new DataSet();
                                if (selectedValue.ToLower() == "timezone")
                                {
                                    ds = ClsTreatment.Timezone_GetAll();
                                }
                                //else if (selectedValue.ToLower() == "country")
                                //{
                                //    ds = ClsTreatment.Country_GetAll();
                                //}
                                else if (selectedValue.ToLower() == "areacode")
                                {
                                    ds = ClsTreatment.AreaCode_GetAll();
                                }
                                else if (selectedValue.ToLower() == "state")
                                {
                                    ds = ClsTreatment.State_GetAll();
                                }
                                txtValues.IsReadOnly = true;
                                lstFill.Visibility = Visibility.Visible;
                                lstFill.Items.Clear();
                                DataTable dt = ds.Tables[0];
                                for (int i = 0; i < dt.Rows.Count; i++)
                                {
                                    ListBoxItem cbiFormat = new ListBoxItem();
                                    cbiFormat.Content = dt.Rows[i].ItemArray[0];
                                    cbiFormat.Tag = dt.Rows[i].ItemArray[1];
                                    lstFill.Items.Add(cbiFormat);
                                }
                            }
                            else
                            {
                                txtValues.IsReadOnly = false;
                                lstFill.Items.Clear();
                                lstFill.Visibility = Visibility.Hidden;
                                lblExample.Visibility = Visibility.Visible;
                            }
                        }
                    }
                    else
                    {
                cmbValues.Items.Clear();
                DataSet ds = ClsTreatment.GetFieldValues(((ComboBoxItem)cmbField.SelectedItem).Content.ToString());
                DataTable dt = ds.Tables[0];
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ComboBoxItem l = new ComboBoxItem();
                    l.Content = dt.Rows[i].ItemArray[0];
                    cmbValues.Items.Add(l);
                }
            }
                }
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "cmbField_SelectionChanged()", "CtlTreatment.xaml.cs");
            }
        }

        private void cmbOperator_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (((ComboBox)sender).SelectedItem != null)
                {
                    if (((ComboBox)sender).SelectedItem.ToString() == "IN" || ((ComboBox)sender).SelectedItem.ToString()=="Not IN")
                    {
                        if (cmbValues.Visibility == Visibility.Visible)
                        {
                            lblExample.Visibility = Visibility.Visible;
                            cmbValues.Visibility = Visibility.Hidden;
                            txtValues.Visibility = Visibility.Visible;
                        }
                    }
                    else
                    {
                        if (cmbValues.Visibility == Visibility.Hidden)
                        {
                            lblExample.Visibility = Visibility.Hidden;
                            cmbValues.Visibility = Visibility.Visible;
                            txtValues.Visibility = Visibility.Hidden;
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "cmbOperator_SelectionChanged()", "CtlTreatment.xaml.cs");

            }
        }

        private void cmbLeadFormat_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                cmbField.Items.Clear();
                if (cmbLeadFormat.SelectedItem != null)
                {
                    DataSet ds = Treatment.Business.ClsTreatment.GetAllFormat(int.Parse(((ComboBoxItem)cmbLeadFormat.SelectedValue).Tag.ToString()));

                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        ComboBoxItem cbiFormat = new ComboBoxItem();
                        cbiFormat.Content = ds.Tables[0].Rows[i][1].ToString();
                        cbiFormat.Tag = ds.Tables[0].Rows[i][0].ToString();
                        cmbField.Items.Add(cbiFormat);
                    }
                    if (strTreatmentType == "Filter")
                    {
                        lstFill.Items.Clear();
                        lstFill.Visibility = Visibility.Hidden;
                        DataSet ds1 = Treatment.Business.ClsTreatment.GetOtherDetail();
                        for (int i = 0; i < ds1.Tables[0].Rows.Count; i++)
                        {
                            ComboBoxItem cbiFormat = new ComboBoxItem();
                            cbiFormat.Content = ds1.Tables[0].Rows[i][1].ToString();
                            cbiFormat.Tag = ds1.Tables[0].Rows[i][0].ToString();
                            cmbField.Items.Add(cbiFormat);

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "cmbLeadFormat_SelectionChanged()", "CtlTreatment.xaml.cs");
            }
        }

        private void cmbCampaign_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                // cmbValues.Items.Clear();
                if (cmbCampaign.SelectedItem != null)
                {
                    DataSet ds = Treatment.Business.ClsTreatment.GetCampaignDisp(null, (((ComboBoxItem)cmbCampaign.SelectedValue).Tag.ToString()));
                    DataTable dt = ds.Tables[0];
                    cmbValues.Items.Clear();
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        ComboBoxItem m = new ComboBoxItem();
                        m.Content = dt.Rows[i][0].ToString();
                        m.Tag = dt.Rows[i][0].ToString();
                        cmbValues.Items.Add(m);
                    }
                }
            }
            catch(Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "cmbCampaign_SelectionChanged()", "CtlTreatment.xaml.cs");
            }
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //cmbCampaign.SelectedIndex = -1;
                btnAdd.IsEnabled = true;
                btnEdit.IsEnabled = true;
                string Str = "";
                ListBoxItem lItem = new ListBoxItem();
                if (rdDisposition.IsChecked != true)
                {
                    rdFilter.IsEnabled = false;
                    rdDisposition.IsEnabled = false;
                    rdFieldValue.IsEnabled = false;
                    if (cmbOperator.Text == "IN" || cmbOperator.Text == "Not IN")
                    {
                        Str = "( " + cmbField.Text + " " + cmbOperator.Text + " " + txtValues.Text + " )";
                    }
                    else
                    {
                        Str = "( " + cmbField.Text + " " + cmbOperator.Text + " " + cmbValues.Text + " )";
                    }
                    if (cmbOperator.Text == "IN" || cmbOperator.Text == "Not IN")
                    {
                        if (cmbField.Text == "" || cmbOperator.Text == "" || txtValues.Text == "")
                        {
                            rdFilter.IsEnabled = true;
                            rdDisposition.IsEnabled = true;
                            rdFieldValue.IsEnabled = true;
                            System.Windows.MessageBox.Show("Please give Valid conditions");
                        }
                        else
                        {
                            lItem.Content = Str;
                            lItem.Tag = cmbField.Text + "~" + cmbOperator.Text + "~" + txtValues.Text;
                            lstConditionsCreated.Items.Add(lItem);
                            txtValues.Text = "";
                            cmbValues.Items.Clear();
                            lstFill.Items.Clear();
                            lstFill.Visibility = Visibility.Hidden;
                            //cmbLeadFormat.SelectedIndex = -1;
                            cmbField.SelectedIndex = -1;
                            cmbOperator.SelectedIndex = -1;
                            //if (strTreatmentType == "Filter")
                            //{
                            //    btnAdd.IsEnabled = false;
                            //}
                        }
                    }
                    else
                    {
                        if (cmbField.Text == "" || cmbOperator.Text == "" || cmbValues.SelectedIndex == -1)
                        {
                            rdFieldValue.IsEnabled = true;
                            rdDisposition.IsEnabled = true;
                            rdFilter.IsEnabled = true;
                            System.Windows.MessageBox.Show("Please give Valid conditions");
                        }
                        else
                        {
                            lItem.Content = Str;
                            lItem.Tag = cmbField.Text + "~" + cmbOperator.Text + "~" + cmbValues.Text;
                            lstConditionsCreated.Items.Add(lItem);
                            txtValues.Text = "";
                            cmbValues.Items.Clear();
                            cmbField.SelectedIndex = -1;
                            cmbOperator.SelectedIndex = -1;
                        }
                    }
                }
                else
                {
                    if (cmbHours.Text == "" || cmbMins.Text == "" || cmbValues.SelectedIndex == -1)
                    {
                        System.Windows.MessageBox.Show("Please give Valid conditions");
                    }
                    else
                    {
                        rdFieldValue.IsEnabled = false;
                        rdDisposition.IsEnabled = false;
                        lItem.Content = "(Call After " + cmbHours.Text + ":" + cmbMins.Text + " hours on " + cmbValues.Text + " )";
                        lItem.Tag = "Call After" + "~" + cmbHours.Text + ":" + cmbMins.Text + "~" + cmbValues.Text;
                        lstConditionsCreated.Items.Add(lItem);
                        txtValues.Text = "";
                        cmbValues.SelectedIndex = -1;
                        cmbHours.SelectedIndex = -1;
                        cmbMins.SelectedIndex = -1;
                        // cmbField.SelectedIndex = -1;
                        //cmbOperator.SelectedIndex = -1;

                    }
                }
                //if(strTreatmentType=="Filter")
                //{
                //    if(lstConditionsCreated.Items.Count>0)
                //    {
                //        btnAdd.IsEnabled=false;
                //    }
                //}
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "btnAdd_Click()", "CtlTreatment.xaml.cs");

            }
        }

        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                btnEdit.IsEnabled = false;
                ListBoxItem lBox = new ListBoxItem();
                if (lstConditionsCreated.SelectedItems.Count == 0)
                {
                    System.Windows.MessageBox.Show("No Items Selected to edit");
                    btnEdit.IsEnabled = true;
                }
                if (lstConditionsCreated.SelectedItems.Count == 1)
                {
                    string[] str = ((ListBoxItem)lstConditionsCreated.SelectedItem).Tag.ToString().Split('~');
                    if (str[0].ToString() == "Call After")
                    {
                        rdDisposition.IsChecked = true;
                        lblDisposition.Visibility = Visibility.Visible;
                        string[] abc = str[1].Split(':');
                        cmbHours.Text = abc[0];
                        cmbMins.Text = abc[1];
                        cmbValues.Text = str[2];
                    }
                    else
                    {
                        if (strTreatmentType == "Filter")
                        {
                            rdFilter.IsChecked = true;
                            btnAdd.IsEnabled = true;
                        }
                        else
                        {
                        rdFieldValue.IsChecked = true;
                        }
                        cmbField.Text = str[0];
                        cmbOperator.Text = str[1];
                        if (cmbOperator.Text == "IN" || cmbOperator.Text == "Not IN")
                        {
                            txtValues.Text = str[2];
                        }
                        else
                        {
                        cmbValues.Text = str[2];
                    }
                    }
                }
                lstConditionsCreated.Items.Remove(((ListBoxItem)lstConditionsCreated.SelectedItem));
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "btnEdit_Click()", "CtlTreatment.xaml.cs");

            }
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                lstConditionsCreated.Items.Remove((ListBoxItem)lstConditionsCreated.SelectedItem);
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "btnDelete_Click()", "CtlTreatment.xaml.cs");

            }
        }

       

        private void fncPermissionsReview()
        {
            try
            {
                CtlGrid.CanEdit = false;
                CtlGrid.CanDelete = false;
                CtlGrid.Visibility = Visibility.Collapsed;
                //tbcCampaign.Visibility = Visibility.Collapsed;
                //btnSaveOD.Visibility = Visibility.Collapsed;
                //btnCancelOD.Visibility = Visibility.Collapsed;

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
                        //tbcCampaign.Visibility = Visibility.Visible;
                        //btnSaveOD.Visibility = Visibility.Visible;
                        //btnCancelOD.Visibility = Visibility.Visible;
                    }
                }
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex,"fncPermissionsReview()", "CtlTratment.xaml.cs");
            }
        }

        #region Grid Display and Events
        private void fncSetGrid()
        {
            try
            {
                CtlGrid.Cols = 5;
                CtlGrid.Columns[0].Header = "Treatment ID";
                CtlGrid.Columns[1].Header = "Name";
                CtlGrid.Columns[2].Header = "Description";
                CtlGrid.Columns[3].Header = "Type";
                CtlGrid.Columns[4].Header = "IsActive";
                CtlGrid.Columns[4].IsIcon = true;

                CtlGrid.Columns[0].BindTo("ID");
                CtlGrid.Columns[1].BindTo("TreatmentName");
                CtlGrid.Columns[2].BindTo("Description");
                CtlGrid.Columns[3].BindTo("Type");
                CtlGrid.Columns[4].BindTo("IsInclude");

                objTreatmentCollection = ClsTreatmentCollection.GetAll();
                CtlGrid.Bind(objTreatmentCollection);
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "fncSetGrid()", "CtlTreatment.xaml.cs");
            }

        }

        private void CtlGrid_btnDeleteClicked(int rowID)
        {
            try
            {
                varID = objTreatmentCollection[rowID].ID;
                if (System.Windows.MessageBox.Show("Do You Really Want To Delete This Record ?", "Delete Treatement", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    ClsTreatment.Delete(varID);
                    System.Windows.MessageBox.Show("Record Deleted!!", "Treatement Delete", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                fncSetGrid();
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "CtlGrid_btnDeleteClicked()", "CtlTreatment.xaml.cs");
            }
        }

        private void CtlGrid_btnEditClicked(int rowID)
        {
            try
            {
                CtlGrid.IsEnabled = false;
                
                VarState = 1;
                varID = Convert.ToInt32(objTreatmentCollection[rowID].ID);
                txtName.Text = objTreatmentCollection[rowID].TreatmentName.ToString();
                txtDescription.Text = objTreatmentCollection[rowID].Description.ToString();
                strTreatmentType = objTreatmentCollection[rowID].Type.ToString();
                cmbValues.Items.Clear();
                objTreatmentConditionCollection = ClsTreatmentConditionCollection.GetAll(varID);
                DataTable dt = ObjectArrayToDataTable1(ClsTreatmentConditionCollection.GetAll(varID), typeof(ClsTreatmentCondition));
                if (dt.Rows.Count != 0)
                {
                    if (strTreatmentType == "Filter")
                    {
                        rdFilter.IsChecked = true;
                        rdFilter.IsEnabled = false;
                        rdDisposition.IsEnabled = false;
                        rdFieldValue.IsEnabled = false;
                    }
                    else
                    {
                    rdFieldValue.IsChecked = true;
                    rdFieldValue.IsEnabled = false;
                    rdDisposition.IsEnabled = false;
                        rdFilter.IsEnabled = false;
                    }
                }
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    cmbLeadFormat.Text = objTreatmentConditionCollection[i].LeadFormatName.ToString();
                    string str = "(" + objTreatmentConditionCollection[i].FieldName.ToString() + " " + objTreatmentConditionCollection[i].Operator.ToString() + " " + objTreatmentConditionCollection[i].FieldValues.ToString() + " )";
                    ListBoxItem newItem = new ListBoxItem();
                    newItem.Content = str;
                    if (strTreatmentType == "Filter")
                    {
                        newItem.Tag = objTreatmentConditionCollection[i].FieldName.ToString() + "~" + objTreatmentConditionCollection[i].Operator.ToString() + "~" + objTreatmentConditionCollection[i].FieldValues.ToString().Replace("~",",");
                    }
                    else
                    {
                    newItem.Tag = objTreatmentConditionCollection[i].FieldName.ToString() + "~" + objTreatmentConditionCollection[i].Operator.ToString() + "~" + objTreatmentConditionCollection[i].FieldValues.ToString();
                    }
                    lstConditionsCreated.Items.Add(newItem);
                }
                Treatment.DataAccess.ClsTreatmentConditionDataService obj = new Treatment.DataAccess.ClsTreatmentConditionDataService();
                DataSet ds = obj.TreatmentDisposition_GetByTreatmentID(varID);
                DataTable dt1 = ds.Tables[0];
                if (dt1.Rows.Count != 0)
                {
                    rdDisposition.IsChecked = true;
                    rdDisposition.IsEnabled = false;
                    rdFieldValue.IsEnabled = false;
                }
                for (int i = 0; i < dt1.Rows.Count; i++)
                {
                    //cmbLeadFormat.Text = objTreatmentConditionCollection[i].LeadFormatName.ToString();
                    string str = "(Call After" + " " + dt1.Rows[i]["Duration"] + " on " + dt1.Rows[i]["Disposition"] + " )";
                    ListBoxItem newItem = new ListBoxItem();
                    newItem.Content = str;
                    newItem.Tag = "Call After ~" + dt1.Rows[i]["Duration"] + "~" + dt1.Rows[i]["Disposition"];
                    lstConditionsCreated.Items.Add(newItem);
                }
                //if (strTreatmentType == "Filter")
                //{
                //    if (lstConditionsCreated.Items.Count > 0)
                //    {
                //        btnAdd.IsEnabled = false;
                //    }
                //}

            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "CtlGrid_btnEditClicked()", "CtlTratment.xaml.cs");

            }
        }

        internal static DataTable ObjectArrayToDataTable1(ClsTreatmentConditionCollection obj, Type type)
        {
            return ObjectArrayToDataTable1(obj, type, null);
        }

        internal static DataTable ObjectArrayToDataTable1(ClsTreatmentConditionCollection obj, Type type, DataColumn[] extra)
        {
            try
            {
                DataTable dt = new DataTable();

                foreach (PropertyInfo pi in type.GetProperties())
                {
                    if (pi.PropertyType.IsPrimitive || pi.PropertyType == typeof(string) || pi.PropertyType == typeof(DateTime))
                    {
                        dt.Columns.Add(pi.Name, pi.PropertyType);
                    }
                }
                if (extra != null)
                {
                    foreach (DataColumn c in extra)
                    {
                        if (dt.Columns.Contains(c.ColumnName))
                            dt.Columns.Remove(c.ColumnName);
                        dt.Columns.Add(c);
                    }
                }

                foreach (object k in obj)
                {
                    DataRow dr = dt.NewRow();
                    foreach (PropertyInfo pi in type.GetProperties())
                    {
                        if (pi.PropertyType.IsPrimitive || pi.PropertyType == typeof(string) || pi.PropertyType == typeof(DateTime))
                        {
                            dr[pi.Name] = pi.GetValue(k, null);
                        }
                    }
                    dt.Rows.Add(dr);
                }

                return dt;
            }
            catch(Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "ObjectArrayToDataTable1()", "CtlTratment.xaml.cs");
                return null;
            }
        }

        #endregion

        private void fncSetComboboxes()
        {
            try
            {
                //Setting Up Hours Comboboxes
                for (int i = 0; i < 23; i++)
                {
                    if (i < 10)
                    {
                        ComboBoxItem m = new ComboBoxItem();
                        m.Content = "0" + i.ToString();
                        m.Tag = "0" + i.ToString();
                        cmbHours.Items.Add(m);
                    }
                    else
                    {
                        ComboBoxItem m = new ComboBoxItem();
                        m.Content = i.ToString();
                        m.Tag = i.ToString();
                        cmbHours.Items.Add(m);

                    }
                }
                //Setting Up Minutes Comboboxes
                for (int i = 0; i < 59; i++)
                {
                    if (i < 10)
                    {
                        ComboBoxItem m = new ComboBoxItem();
                        m.Content = "0" + i.ToString();
                        m.Tag = "0" + i.ToString();
                        cmbMins.Items.Add(m);
                    }
                    else
                    {
                        ComboBoxItem m = new ComboBoxItem();
                        m.Content = i.ToString();
                        m.Tag = i.ToString();
                        cmbMins.Items.Add(m);

                    }
                }
                // Setting Up Fields ComboBoxes.
                cmbLeadFormat.Items.Clear();
                cmbCampaign.Items.Clear();
                DataSet dc = ClsTreatment.GetCampaign(null);
                DataTable dtc = dc.Tables[0];
                for (int i = 0; i < dtc.Rows.Count; i++)
                {
                    ComboBoxItem m = new ComboBoxItem();
                    m.Content = dtc.Rows[i][0].ToString();
                    m.Tag = dtc.Rows[i][0].ToString();
                    cmbCampaign.Items.Add(m);
                }
                

                DataSet df = ClsTreatment.GetLeadFormat();
                DataTable dtt = df.Tables[0];
                
                for (int j = 0; j < dtt.Rows.Count; j++)
                {
                    ComboBoxItem m = new ComboBoxItem();
                    m.Content=dtt.Rows[j].ItemArray[1].ToString();
                    m.Tag = dtt.Rows[j].ItemArray[0].ToString();
                    cmbLeadFormat.Items.Add(m);
                }

                cmbField.Items.Clear();

                //DataSet ds = ClsTreatment.GetFields();
                //DataTable dt = ds.Tables[0];

                //for (int i = 0; i < dt.Columns.Count; i++)
                //{
                    
                //    ComboBoxItem l = new ComboBoxItem();
                //    l.Content = dt.Columns[i].Caption;
                //    cmbField.Items.Add(l);
                //}

                // Setting Up Operator ComboBoxes.

                cmbOperator.Items.Clear();
                if (strTreatmentType == "Filter")
                {
                    cmbOperator.Items.Add("IN");
                    cmbOperator.Items.Add("Not IN");
                }
                else
                {
                cmbOperator.Items.Add("==");
                cmbOperator.Items.Add("<>");
                cmbOperator.Items.Add(">");
                cmbOperator.Items.Add(">=");
                cmbOperator.Items.Add("<");
                cmbOperator.Items.Add("<=");
                cmbOperator.Items.Add("IN");
                cmbOperator.Items.Add("BETWEEN");
            }
            }
            catch(Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "fncSetComboboxes()", "CtlTratment.xaml.cs");

            }
        }

        private void funClearBoxes()
        {
            try
            {
                VarState = 0;
                varID = -1;
                txtDescription.Text = "";
                txtName.Text = "";                
                cmbValues.Items.Clear();               
                txtValues.Text = "";
                cmbHours.SelectedIndex = -1;
                cmbMins.SelectedIndex = -1;
                lstConditionsCreated.Items.Clear();
            }
            catch(Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "funClearBoxes()", "CtlTratment.xaml.cs");

            }
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // We First Have to delete all treatmentconditions related to treatment from DB. 
                CtlGrid.IsEnabled = true;
                ClsTreatment objTreatment = new ClsTreatment();
                if (VarState == 0)
                    objTreatment.ID = -1;
                else
                {
                    if (rdDisposition.IsChecked == true)
                    {
                        ClsTreatment.Delete_Disposition(varID);
                        objTreatment.ID = varID;
                    }
                    else
                    {
                        ClsTreatment.Delete_All(varID);
                        objTreatment.ID = varID;
                    }
                }
                if (txtName.Text != "" && lstConditionsCreated.Items.Count>0)
                {
                    objTreatment.TreatmentName = txtName.Text;
                    objTreatment.Description = txtDescription.Text;
                    objTreatment.IsInclude = true;
                    objTreatment.Type = strTreatmentType;
                    objTreatment.UserID = VMuktiAPI.VMuktiInfo.CurrentPeer.ID;
                    int gID = objTreatment.Save();

                    for (int i = 0; i < lstConditionsCreated.Items.Count; i++)
                    {
                        ClsTreatmentCondition c = new ClsTreatmentCondition();
                        string[] str = ((ListBoxItem)lstConditionsCreated.Items[i]).Tag.ToString().Split('~');
                        if (str[0] != "Call After")
                        {
                        string[] strLeadFormatID = (cmbLeadFormat.SelectedItem.ToString().Split(':'));
                        c.LeadFormatName=strLeadFormatID[1].ToString().TrimStart();
                            if (strTreatmentType == "Filter")
                            {
                                str[2] = str[2].Replace(",", "~");
                                if(str[2].Substring(str[2].Length-1).Contains("~"))
                                {
                                    //str[2] = str[2].Substring(str[2].Length - 1).Replace("", "~");
                                    str[2] = str[2].Substring(0, str[2].Length - 1);
                                }
                            }
                        c.FieldName = str[0];
                        c.Operator = str[1];
                        c.FieldValues = str[2];
                        c.ID = -1;
                        c.TreatmentID = gID;                        
                        c.Save();
                    }
                        else
                        {
                            c.Duration = str[1];
                            c.TreatmentID = gID;
                            c.Disposition = str[2];
                            c.SaveDisposition();
                        }
                    }
                    System.Windows.MessageBox.Show("Record Saved Successfully!!");
                    rdDisposition.IsEnabled = true;
                    rdFieldValue.IsEnabled = true;
                    rdFilter.IsChecked = false;
                    fncSetGrid();
                    fncSetComboboxes();
                    funClearBoxes();
                    btnEdit.IsEnabled = true;
                    btnAdd.IsEnabled = true;
                    rdFilter.IsEnabled = true;
                }
                else
                {
                    System.Windows.MessageBox.Show("Treatment Name and Treatment conditions can't be null");
                }
            }
            catch(Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "btnSave_Click()", "CtlTratment.xaml.cs");

            }  
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                rdFieldValue.IsEnabled = true;
                rdFieldValue.IsChecked = false;
                rdDisposition.IsEnabled = true;
                rdDisposition.IsChecked = false;
                rdFilter.IsEnabled = true;
                rdFilter.IsChecked = false;
                CtlGrid.IsEnabled = true;
                btnEdit.IsEnabled = true;
                btnAdd.IsEnabled = true;
                rdFieldValue.IsChecked = false;
                rdDisposition.IsChecked = false;
                txtValues.IsReadOnly = false;
                lstFill.Items.Clear();
                lstFill.Visibility = Visibility.Hidden;
                funClearBoxes();
                fncSetComboboxes();
            }
            catch(Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "btnCancel_Click()", "CtlTratment.xaml.cs");

            }
        }

        private void lstFill_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            { 
                txtValues.Text = "";
                for (int iCnt = 0; iCnt < lstFill.SelectedItems.Count; iCnt++)
                {
                    txtValues.Text = txtValues.Text + "'" + ((ListBoxItem)lstFill.SelectedItems[iCnt]).Content.ToString() + "',";
                }
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "lstFill_SelectionChanged()", "CtlTratment.xaml.cs");
            }
        }
    }
}
