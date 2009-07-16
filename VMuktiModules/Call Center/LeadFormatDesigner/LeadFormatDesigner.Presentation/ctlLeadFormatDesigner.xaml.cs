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
using LeadFormatDesigner.Business;
using VMuktiAPI;



namespace LeadFormatDesigner.Presentation
{
    /// <summary>
    /// Interaction logic for ctlLeadFormatDesigner.xaml
    /// </summary>
    /// 
    public enum ModulePermissions
    {
        Import = 0,
        //Add = 4,
        Edit = 1,
        //Delete = 2,
        //View = 3
    }

    public partial class ctlLeadFormatDesigner : System.Windows.Controls.UserControl
    {
        //public static StringBuilder sb1;

        string cmbSelection="";
        int flag = 0;
        string TempString = "";
        string TempString1 = "";
        ClsLeadFormatBusinessCollection objLeadCollection = null;
        ClsLeadFormatBusiness objLeadFormat = null;
        ClsLeadFormatBusinessCollection objLeadFieldsCollection = null;
        Int64 LeadsFieldID = 0;
        string TmpDefaultValue = "";
        string TmpFieldLength = "";
        int TmpStartPosition = 0;
        List<object> templst = null;
        ModulePermissions[] _MyPermissions;
        Int64 parentRow=-1;
        Int64 curID = -1;
        bool isEditing = false;

       
        public ctlLeadFormatDesigner(ModulePermissions[] MyPermissions)
        {
            try
            {
                InitializeComponent();
                this.Loaded += new RoutedEventHandler(ctlLeadFormatDesigner_Loaded);

                _MyPermissions = MyPermissions;
                FncPermissionsReview();

                //added code
                //shilpa
                // objLeadFormat = new ClsLeadFormatBusiness();

                btnUpdateLead.Visibility = Visibility.Hidden;
                btnNext.Visibility = Visibility.Visible;
                btnUpdateLeadDetail.Visibility = Visibility.Hidden;
                btnSave.Visibility = Visibility.Visible;
                btnDone.Visibility = Visibility.Visible;
                btnAddNewField.Click += new RoutedEventHandler(btnAddNewField_Click);
                btnClose.Click += new RoutedEventHandler(btnClose_Click);
                btnNext.Click += new RoutedEventHandler(btnNext_Click);
                btnSav.Click += new RoutedEventHandler(btnSav_Click);
                btnSave.Click += new RoutedEventHandler(btnSave_Click);
                btnUpdateLeadDetail.Click += new RoutedEventHandler(btnUpdateLeadDetail_Click);
                btnUpdateLead.Click += new RoutedEventHandler(btnUpdateLead_Click);
                tbcDispositionList.SelectionChanged += new SelectionChangedEventHandler(tbcDispositionList_SelectionChanged);
                cmbFormatType.SelectionChanged += new SelectionChangedEventHandler(cmbFormatType_SelectionChanged);
                txtFieldName.KeyUp += new KeyEventHandler(txtFieldName_KeyUp);
                btnDone.Click += new RoutedEventHandler(btnDone_Click);

                funSetGrid();

            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "ctlLeadFormatDesigner()", "ctlLeadFormatDesigner.xaml.cs");
            }
        }

        void ctlLeadFormatDesigner_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                this.Width = ((Grid)this.Parent).ActualWidth;
                ((Grid)this.Parent).SizeChanged += new SizeChangedEventHandler(ctlLeadFormatDesigner_SizeChanged);
            }
            catch
            {
            }
        }

        void ctlLeadFormatDesigner_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            try
            {
                this.Width = e.NewSize.Width - 5;
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "ctlLeadFormatDesigner_SizeChanged()", "ctlLeadFormatDesigner.xaml.cs");
            }
        }

        void btnDone_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                CtlGrid.Visibility = Visibility.Visible;
                CtlGridChild.Visibility = Visibility.Hidden;
                btnUpdateLead.Visibility = Visibility.Hidden;
                btnSave.Visibility = Visibility.Visible;
                btnNext.Visibility = Visibility.Visible;
                btnUpdateLeadDetail.Visibility = Visibility.Hidden;
                tbiDispositions.IsEnabled = false ;
                tbiDispositions.IsSelected = false  ;
                tbiDispositionListDetails.IsEnabled = true;
                tbiDispositionListDetails.IsSelected = true;
                cmbFormatType.IsEnabled = true;
                txtLeadFormatName.Text = "";
                txtDescription.Text = "";
                funSetGrid();
                flag = 0;
                isEditing = false;
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "btnDone_Click()", "ctlLeadFormatDesigner.xaml.cs");
            }
        }
        void FncPermissionsReview()
         {
            try
            {
                this.Visibility = Visibility.Hidden;
                
                for (int i = 0; i < _MyPermissions.Length; i++)
                {
                    if (_MyPermissions[i] == ModulePermissions.Import)
                    {
                        this.Visibility = Visibility.Visible;
                    }

                }
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "FncPermissionsReview()", "ctlLeadFormatDesigner.xaml.cs");
            }
        }

        void txtFieldName_KeyUp(object sender, KeyEventArgs e)
        {
            try
            {
                string item = txtFieldName.Text;
                List<object> lstr = new List<object>();


                if (txtFieldName.Text.Length > 0)
                {

                    if (e.Key == Key.Back || e.Key == Key.Delete)
                    {
                        lstFieldNames.Items.Clear();
                        foreach (object name in templst)
                        {
                            lstFieldNames.Items.Add(name);
                        }

                    }

                    for (int i = 0; i < lstFieldNames.Items.Count; i++)
                    {
                        if (((ListBoxItem)lstFieldNames.Items[i]).Content.ToString().StartsWith(txtFieldName.Text, true, null) == false)
                        {
                            lstr.Add(lstFieldNames.Items[i]);
                        }
                    }

                    foreach (object name in lstr)
                    {
                        lstFieldNames.Items.Remove(name);
                    }
                }
                else
                {
                    lstFieldNames.Items.Clear();
                    foreach (object name in templst)
                    {
                        lstFieldNames.Items.Add(name);
                    }
                }
            }
           catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "txtFieldName_KeyUp()", "ctlLeadFormatDesigner.xaml.cs");
            }
        }

       
        void btnUpdateLeadDetail_Click(object sender, RoutedEventArgs e)
        {
            //throw new Exception("The method or operation is not implemented.");
            try
            {
                int flagChk = 1;
                int flagCol = 1;
                string chk = "";

                if (cmbSelection.ToLower() == "text")
                {
                    chk = txtFieldStartPosition.Text;
                }
                else
                {
                    chk = cmbColumn.SelectionBoxItem.ToString();
                }



                foreach (ClsLeadFormatBusiness cl in objLeadFieldsCollection)
                {
                    if (cl.FieldName == cmbFieldName.SelectionBoxItem.ToString())
                    {
                        flagChk = 0;
                        break;
                    }
                }


                foreach (ClsLeadFormatBusiness cl in objLeadFieldsCollection)
                {
                    if (cl.StartPosition.ToString() == chk)
                    {
                        flagCol = 0;
                        break;
                    }
                }


                if (flagChk == 1 && flagCol == 1)
                {
                    objLeadFormat = new ClsLeadFormatBusiness();
                    objLeadFormat.LeadFieldsID = -1;
                    setLeadFormatValue();
                    int cnt = objLeadFormat.LeadFormatDesignerSave();

                    if (cnt > 0)
                    {
                        MessageBox.Show("New Record Inserted");
                        //funSetChildGrid(objLeadFormat.LeadFormatID);

                    }
                    else
                    {
                        MessageBox.Show("can not edit");
                    }
                }
                else if (flagChk == 0 && flagCol == 1)
                {
                    objLeadFormat = new ClsLeadFormatBusiness();
                    objLeadFormat.LeadFieldsID = LeadsFieldID;
                    setLeadFormatValue();
                    int cnt = objLeadFormat.LeadFormatDesignerSave();

                    if (cnt > 0)
                    {
                        MessageBox.Show("One Record Updated");
                        //funSetChildGrid(objLeadFormat.LeadFormatID);

                    }
                    else
                    {
                        MessageBox.Show("can not edit");
                    }
                }
                else if (flagChk == 0 && flagCol == 0 && (TmpDefaultValue.ToLower() != txtDefaultValue.Text.ToLower() || TmpFieldLength.ToLower() != txtFieldLength.Text.Trim(' ').ToLower()))
                {
                    //objLeadFormat.LeadFieldsID = LeadsFieldID;
                    objLeadFormat = new ClsLeadFormatBusiness();
                    setLeadFormatValue();
                    int cnt = objLeadFormat.LeadFormatDesignerSave();

                    if (cnt > 0)
                    {
                        MessageBox.Show("One Record Updated");
                        funSetChildGrid(objLeadFormat.LeadFormatID);
                        TmpDefaultValue = objLeadFormat.DefaultValue;
                        if (objLeadFormat.Length == 0)
                        {
                            TmpFieldLength = "";
                        }
                        else
                        {
                            TmpFieldLength = objLeadFormat.Length.ToString();
                        }
                    }
                    else
                    {
                        MessageBox.Show("can not Updated , try again");
                    }
                }
                else
                {
                    MessageBox.Show("Field Name and Column Name is already assigned");
                }
                //}
                //catch (Exception ex)
                //{
                //    MessageBox.Show(ex.Message);
                //}

                //shilpa code

               

            }
            catch (Exception ex)
            {
                // MessageBox.Show(ex.Message);
                VMuktiHelper.ExceptionHandler(ex, "btnUpdateLeadDetail_Click()", "ctlLeadFormatDesigner.xaml.cs");
            }

            if (parentRow != -1)
            {
                funSetChildGrid(parentRow);
            }
        }

        void setLeadFormatValue()
        {
            try
            
            {
                //objLeadFormat = new ClsLeadFormatBusiness();
                //objLeadFormat.FieldID = Convert.ToInt64((ListBoxItem)cmbFieldName.SelectedValue);
                //MessageBox.Show((((ListBoxItem)cmbFieldName.SelectedItem).Tag).ToString());
                long fieldName =long.Parse((((ListBoxItem)cmbFieldName.SelectedValue).Tag).ToString());
                objLeadFormat.FieldID = fieldName;
                //objLeadFormat.FieldID = Convert.ToInt64(((ListBoxItem)cmbFieldName.SelectedItem).Tag);
                objLeadFormat.LeadFormatID = objLeadFormat.LeadFormatID;
                objLeadFormat.DefaultValue = txtDefaultValue.Text;
                objLeadFormat.IsRequired = true;

                if (cmbSelection == "excel" || cmbSelection == "csv")
                {
                    objLeadFormat.StartPosition = int.Parse(cmbColumn.SelectionBoxItem.ToString());
                    objLeadFormat.Length=0;
                    objLeadFormat.Delimiters = null;
                }
                else
                {
                    objLeadFormat.StartPosition = int.Parse(txtFieldStartPosition.Text);
                    if (txtFieldLength.Text.Length == 0)
                    {
                        objLeadFormat.Length = 0;
                    }
                    else
                    {
                        objLeadFormat.Length = Convert.ToInt32(txtFieldLength.Text);
                    }
                    objLeadFormat.Delimiters = cmbDelimiter.SelectionBoxItem.ToString();

                }
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "setLeadFormatValue()", "ctlLeadFormatDesigner.xaml.cs");
            }

        }

        void btnUpdateLead_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //Int64 varID = parentRow;
                //objLeadFormat = ClsLeadFormatBusiness.GetByLeadFormatID(varID);
                if (objLeadFormat.LeadFormatName.ToLower() != txtLeadFormatName.Text.ToLower() || objLeadFormat.Description.ToLower() != txtDescription.Text.ToLower())
                {
                    objLeadFormat.LeadFormatName = txtLeadFormatName.Text;
                    objLeadFormat.Description = txtDescription.Text;
                    objLeadFormat.FormatType = cmbFormatType.SelectionBoxItem.ToString();

                    int cntRecord = objLeadFormat.LeadFormatSave();
                    if (cntRecord > 0)
                    {
                        MessageBox.Show("Records Successfully Updated");
                    }
                }
                else
                {
                    MessageBox.Show("No Change In Record");
                }
                //shilpa code
                this.CtlGridChild.Visibility = Visibility.Hidden;
                funSetGrid();
                this.CtlGrid.Visibility = Visibility.Visible;
                txtLeadFormatName.Text = "";
                //cmbFieldType.IsEnabled = true;
                cmbFormatType.IsEnabled = true;
                txtDescription.Text = "";
                btnNext.Visibility = Visibility.Visible;
                btnUpdateLead.Visibility = Visibility.Hidden;
                tbiDispositions.IsEnabled = false;
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "btnUpdateLead_Click()", "ctlLeadFormatDesigner.xaml.cs");
            }
        }

        void cmbFormatType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                string cmbSelectionIndex = cmbFormatType.SelectedIndex.ToString();
                if (cmbSelectionIndex == "0" || cmbSelectionIndex == "2")
                {
                    cmbColumn.Items.Clear();
                    ControlHide();
                    for (int cnt = 1; cnt <= 256; cnt++)
                    {
                        ComboBoxItem cbiFormat = new ComboBoxItem();
                        cbiFormat.Content = cnt;
                        cmbColumn.Items.Add(cbiFormat);
                    }
                }
                else
                {
                    ControlVisible();
                }
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "cmbFormatType_SelectionChanged()", "ctlLeadFormatDesigner.xaml.cs");
            }
        }
       
        void funSetGrid()
        {
            try
            {
                CtlGrid.Cols = 4;
                CtlGrid.CanEdit = true;
                CtlGrid.CanDelete = true;

                CtlGrid.Columns[0].Header = "ID";
                CtlGrid.Columns[1].Header = "Format Name";
                CtlGrid.Columns[2].Header = "Format Type";
                CtlGrid.Columns[3].Header = "Description";
                //CtlGrid.Columns[4].Header = "CreatedDate";
                //CtlGrid.Columns[5].Header = "CreatedBy";
                //CtlGrid.Columns[6].Header = "ModifiedDate";
                //CtlGrid.Columns[7].Header = "ModifeiedBy";

                CtlGrid.Columns[0].BindTo("ID");
                CtlGrid.Columns[1].BindTo("LeadFormatName");
                CtlGrid.Columns[2].BindTo("FormatType");
                CtlGrid.Columns[3].BindTo("Description");
                //CtlGrid.Columns[4].BindTo("CreatedDate");
                //CtlGrid.Columns[5].BindTo("CreatedBy");
                //CtlGrid.Columns[6].BindTo("ModifiedDate");
                //CtlGrid.Columns[7].BindTo("ModifiedBy");

                objLeadCollection = ClsLeadFormatBusinessCollection.GetAll();
                CtlGrid.Bind(objLeadCollection);
                //objLeadCollection = null;
                
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "funSetGrid()", "ctlLeadFormatDesigner.xaml.cs");
            }
        }


        void funSetChildGrid(Int64 FormatID)
        {
            try
            {
                parentRow = FormatID;
                CtlGridChild.CanEdit = true;
                CtlGridChild.CanDelete = true;
                if (cmbFormatType.SelectionBoxItem.ToString().ToLower() == "text")
                {
                    CtlGridChild.Width = 560;
                    CtlGridChild.Cols = 6;
                    CtlGridChild.Columns[0].Header = "ID";
                    CtlGridChild.Columns[1].Header = "FieldName";
                    CtlGridChild.Columns[2].Header = "DefaultValue";
                    CtlGridChild.Columns[3].Header = "Start Position";
                    CtlGridChild.Columns[4].Header = "Length";
                    CtlGridChild.Columns[5].Header = "Delimiters";
                    CtlGridChild.Columns[4].BindTo("Length");
                    CtlGridChild.Columns[5].BindTo("Delimiters");
                }
                else
                {
                    CtlGridChild.Width = 380;
                    CtlGridChild.Cols = 4;
                    CtlGridChild.Columns[0].Header = "ID";
                    CtlGridChild.Columns[1].Header = "FieldName";
                    CtlGridChild.Columns[2].Header = "DefaultValue";
                    CtlGridChild.Columns[3].Header = "MapColumn";
                }
                CtlGridChild.Cols = 4;
                
                CtlGridChild.Columns[0].BindTo("LeadFieldsID");
                CtlGridChild.Columns[1].BindTo("FieldName");
                CtlGridChild.Columns[2].BindTo("DefaultValue");
                CtlGridChild.Columns[3].BindTo("StartPosition");

                objLeadFieldsCollection = ClsLeadFormatBusinessCollection.GetAllLeadFields(FormatID);
                CtlGridChild.Bind(objLeadFieldsCollection);

                //Remove binded columns

                //int i;
                //int j;

                //for (i = 0; i < objLeadFieldsCollection.Count; i++)
                //{
                //    for (j = 0;j<cmbColumn.Items.Count; j++)
                //    {
                //        if (cmbColumn.Items[j].ToString() == objLeadFieldsCollection[i].StartPosition.ToString())
                //        {
                //            //Check the binded column
                //        }
                //    }
                //    cmbColumn.Items.RemoveAt(objLeadFieldsCollection[i].StartPosition-1);
                //    MessageBox.Show((objLeadFieldsCollection[i].StartPosition - 1).ToString());
                //}
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "funSetChildGrid()", "ctlLeadFormatDesigner.xaml.cs");
            }
        }


        void CtlGrid_btnEditClicked(int rowID)
        {
            Int64 varID = 0;

            //MessageBox.Show("clicked");
            try
            {
                varID = Convert.ToInt64(objLeadCollection[rowID].ID);
                curID = varID;
                isEditing = true;
                objLeadFormat = new ClsLeadFormatBusiness();
                objLeadFormat = ClsLeadFormatBusiness.GetByLeadFormatID(varID);
                         
                txtLeadFormatName.Text = objLeadFormat.LeadFormatName;
                if (objLeadFormat.FormatType.ToLower() == "excel")
                {
                    cmbFormatType.SelectedIndex = 0;
                }
                else if (objLeadFormat.FormatType.ToLower() == "text")
                {
                    cmbFormatType.SelectedIndex = 1;
                }
                else
                {
                    cmbFormatType.SelectedIndex = 2;
                }
                
                txtDescription.Text = objLeadFormat.Description;
                
                CtlGrid.Visibility = Visibility.Hidden;
                CtlGridChild.Visibility = Visibility.Visible;
                btnNext.Visibility = Visibility.Hidden;
                cmbFormatType.IsEnabled = false;
                tbiDispositions.IsEnabled = true;
                tbcDispositionsCheck();
                funSetChildGrid(varID);
                btnUpdateLead.Visibility = Visibility.Visible;
                btnUpdateLeadDetail.Visibility = Visibility.Hidden;
                btnSave.Visibility = Visibility.Visible;
                TempString1 = txtLeadFormatName.Text;
                
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "CtlGrid_btnEditClicked()", "ctlLeadFormatDesigner.xaml.cs");
            }
        }

	  public void CtlGridChild_btnDeleteClicked(int rowID)
        {
            try
            {
                if (MessageBox.Show("Are You sure You want to remove the Lead Format Field?", "", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {

                    Int64 varID = 0;
                    varID = Convert.ToInt64(objLeadFieldsCollection[rowID].LeadFieldsID);
                    int DelLeadField = ClsLeadFormatBusiness.DeleteFormatField_ByID(varID);
                    //funSetChildGrid();
                    if (parentRow != -1)
                    {
                        funSetChildGrid(parentRow);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        void CtlGrid_btnDeleteClicked(int rowID)
        {
            try
            {
                if (MessageBox.Show("Are You sure You want to remove the Lead Format?", "", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    Int64 varID = 0;
                    //tbcDispositionsCheck();
                    varID = Convert.ToInt64(objLeadCollection[rowID].ID);
                    //int DelFormat = ClsLeadFormatBusiness.LeadFields_GetByID(varID);
                    int DelFormat = ClsLeadFormatBusiness.DeleteLeadField_ByID(varID);
                    if (DelFormat > 0)
                    {
                        MessageBox.Show("Lead Format is Deleted Sucessfully");
                    }
                    else
                    {
                        MessageBox.Show("Lead Format can not be Deleted");
                    }
                    txtLeadFormatName.Text = "";
                    funSetGrid();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        void CtlGridChild_btnEditClicked(int rowID)
        {
            //shilpa code

            //parentRow = rowID;
            //end
            Int64 varID = 0;
            try
            {

               tbcDispositionsCheck();

               varID = Convert.ToInt64(objLeadFieldsCollection[rowID].LeadFormatID);
               LeadsFieldID = objLeadFieldsCollection[rowID].LeadFieldsID;
               TmpDefaultValue = objLeadFieldsCollection[rowID].DefaultValue;

                if (objLeadFieldsCollection[rowID].Length == 0)
                {
                    TmpFieldLength = "";
                }
                else
                {
                    TmpFieldLength = objLeadFieldsCollection[rowID].Length.ToString();
                }
              
                objLeadFormat = new ClsLeadFormatBusiness();
                objLeadFormat = ClsLeadFormatBusiness.LeadFields_GetByID(varID);

                cmbFieldName.Text = objLeadFieldsCollection[rowID].FieldName;
                txtDefaultValue.Text = objLeadFieldsCollection[rowID].DefaultValue;

                if (cmbSelection.ToLower() == "excel" || cmbSelection.ToLower() == "csv")
                {
                    cmbColumn.Text = objLeadFieldsCollection[rowID].StartPosition.ToString();
                    TmpStartPosition = objLeadFieldsCollection[rowID].StartPosition; 
                }
                else
                {
                    txtFieldStartPosition.Text = objLeadFieldsCollection[rowID].StartPosition.ToString();
                    txtFieldLength.Text = objLeadFieldsCollection[rowID].Length.ToString();
                    cmbDelimiter.Text = objLeadFieldsCollection[rowID].Delimiters.ToString();
                }
                btnUpdateLeadDetail.Visibility = Visibility.Visible;
                btnDone.Visibility = Visibility.Visible;
                btnSave.Visibility = Visibility.Hidden;
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "CtlGridChild_btnEditClicked()", "ctlLeadFormatDesigner.xaml.cs");
            }
        }
        

        void btnSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                bool checkValue = false;
                if (((ListBoxItem)cmbFieldName.SelectedItem).Tag.ToString() == "")
                {
                    MessageBox.Show("Select Field Name");
                }
                else
                {
                    int affectedRow;

                    ClsLeadFormatBusiness LeadFormat = new ClsLeadFormatBusiness();
                    
                    if ((TempString1.ToLower() != txtLeadFormatName.Text.ToLower()) && flag == 0)
                    {
                        flag = 1;
                       // LeadFormat.LeadFormatID = -2;
                    }

                    else if ((TempString1.ToLower() == txtLeadFormatName.Text.ToLower()))
                    {
                        flag = 0;
                       // LeadFormat.LeadFormatID = curID;
                    }

                    TempString1 = txtLeadFormatName.Text;

                    LeadFormat.ID = -1;
                    LeadFormat.LeadFormatName = txtLeadFormatName.Text;
                    LeadFormat.FormatType = cmbFormatType.SelectionBoxItem.ToString();
                    LeadFormat.Description = txtDescription.Text;

                    LeadFormat.FieldID = Convert.ToInt64(((ListBoxItem)cmbFieldName.SelectedItem).Tag);

                    LeadFormat.DefaultValue = txtDefaultValue.Text;
                    //LeadFormat.LeadFormatID = -2;
                    LeadFormat.LeadFieldsID = -1;
                    LeadFormat.IsRequired = true;

                    if (isEditing)
                    {
                        LeadFormat.LeadFormatID = curID;
                    }
                    else
                    {
                        LeadFormat.LeadFormatID = -2;
                    }

                    if (cmbSelection == "excel" || cmbSelection == "csv")
                    {
                        if (cmbColumn.SelectionBoxItem.ToString() != "")
                        {
                            LeadFormat.StartPosition = int.Parse(cmbColumn.SelectionBoxItem.ToString());
                            LeadFormat.Length = 0;
                            LeadFormat.Delimiters = null;
                            checkValue = true;
                        }
                        else
                        {
                            checkValue = false;
                            MessageBox.Show("Select map column");
                        }
                    }
                    else
                    {
                        if (txtFieldStartPosition.Text == "" || txtFieldLength.Text == "" || cmbDelimiter.SelectionBoxItem.ToString() == "")
                        {
                            MessageBox.Show("Enter all required information");
                            checkValue = false;
                        }
                        else
                        {
                            LeadFormat.StartPosition = int.Parse(txtFieldStartPosition.Text);
                            LeadFormat.Length = int.Parse(txtFieldLength.Text);
                            LeadFormat.Delimiters = cmbDelimiter.SelectionBoxItem.ToString();
                            checkValue = true;
                        }
                    }
                    if (checkValue == true)
                    {
                        if (flag == 1)
                        {
                            affectedRow = LeadFormat.LeadFormatSave();
                            flag = 0;
                            //funSetGrid();
                        }


                        int cntInsert = 0;

                        cntInsert = LeadFormat.LeadFormatDesignerSave();
                        
                        if (cntInsert > 0)
                        {
                            cmbColumn.Items.Remove(cmbColumn.SelectedItem);
                            BlankFields();
                        }
                    }
                }

                if (parentRow != -1)
                {
                    CtlGrid.Visibility = Visibility.Hidden;
                    CtlGridChild.Visibility = Visibility.Visible;
                    btnUpdateLead.Visibility = Visibility.Visible;
                    btnNext.Visibility = Visibility.Hidden;
                    funSetChildGrid(parentRow);
                }
            }
            catch (NullReferenceException ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "btnSave_Click()", "ctlLeadFormatDesigner.xaml.cs");
            }
        }

        void tbcDispositionList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if ((TempString.ToLower() != txtLeadFormatName.Text.ToLower()) && flag == 0)
                {
                    cmbColumn.Items.Clear();
                    for (int cnt = 1; cnt <= 256; cnt++)
                    {
                        ComboBoxItem cbiFormat = new ComboBoxItem();
                        cbiFormat.Content = cnt;
                        cmbColumn.Items.Add(cbiFormat);
                    }
                }
                TempString = txtLeadFormatName.Text;

                if (tbcDispositionList.SelectedIndex == 1 && txtLeadFormatName.Text.Length <= 0)
                {
                    MessageBox.Show("Lead Format Name should not be blank");
                    tbcDispositionList.SelectedIndex = 0;
                }
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "tbcDispositionList_SelectionChanged()", "ctlLeadFormatDesigner.xaml.cs");
            }
        }

            
        void btnSav_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int affectedRow;
                int flagFieldSet = 1;
                ClsLeadDesignerBusiness LeadDesigner = new ClsLeadDesignerBusiness();

                if (txtFieldName.Text.Length == 0)
                {
                    MessageBox.Show("Field Name Should not be blank");
                }
                else
                {
                    for (int cntField = 0; cntField < lstFieldNames.Items.Count; cntField++)
                    {
                        if (((ListBoxItem)lstFieldNames.Items[cntField]).Content.ToString().ToLower() == txtFieldName.Text.ToLower())
                        {
                            flagFieldSet = 0;
                            break;
                        }

                    }

                    if (flagFieldSet == 1)
                    {
                        LeadDesigner.ID = -1;
                        LeadDesigner.FieldName = txtFieldName.Text;
                        if (txtFieldSize.Text.Length == 0)
                        {
                            LeadDesigner.FieldSize = 0;
                        }
                        else
                        {
                            LeadDesigner.FieldSize = Convert.ToInt64(txtFieldSize.Text);
                        }
                        LeadDesigner.FieldType = cmbFieldType.SelectionBoxItem.ToString();
                        LeadDesigner.IsRequired = true;

                        affectedRow = LeadDesigner.Save();

                        if (affectedRow > 0)
                        {
                            MessageBox.Show("Record Added");
                            lstFieldNames.Items.Clear();
                            templst.Clear();
                            ClsLeadDesignerBusinessCollection objInsertField = ClsLeadDesignerBusinessCollection.GetAll();
                            for (int i = 0; i < objInsertField.Count; i++)
                            {
                                ListBoxItem cbiFormat = new ListBoxItem();
                                cbiFormat.Content = objInsertField[i].FieldName;
                                cbiFormat.Tag = objInsertField[i].ID;
                                lstFieldNames.Items.Add(cbiFormat);
                                templst.Add(lstFieldNames.Items[i]);
                            }
                            txtFieldName.Text = "";
                            txtFieldSize.Text = "";
                        }
                    }
                    else
                    {
                        MessageBox.Show("Duplicate Entry Found");
                    }
                }
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "btnSav_Click()", "ctlLeadFormatDesigner.xaml.cs");
            }
        }

       void tbcDispositionsCheck()
        {
            try
            {
                tbiDispositions.IsEnabled = true;
                tbiDispositions.IsSelected = true;
                //flag = 1;

                cmbSelection = cmbFormatType.SelectionBoxItem.ToString().ToLower();
                if (cmbSelection == "excel" || cmbSelection == "csv")
                {
                    cmbColumn.Items.Clear();
                    ControlHide();
                    for (int cnt = 1; cnt <= 256; cnt++)
                    {
                        ComboBoxItem cbiFormat = new ComboBoxItem();
                        cbiFormat.Content = cnt;
                        cmbColumn.Items.Add(cbiFormat);
                    }
                }
                else
                {
                    ControlVisible();
                }

                cmbFieldName.Items.Clear();
                ClsLeadDesignerBusinessCollection objLeadDesigner = ClsLeadDesignerBusinessCollection.GetAll();
                for (int i = 0; i < objLeadDesigner.Count; i++)
                {
                    ComboBoxItem cbiFormat = new ComboBoxItem();
                    cbiFormat.Content = objLeadDesigner[i].FieldName;
                    cbiFormat.Tag = objLeadDesigner[i].ID;
                    cmbFieldName.Items.Add(cbiFormat);

                }
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "tbcDispositionsCheck()", "ctlLeadFormatDesigner.xaml.cs");
            }
        }
        void btnNext_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                tbcDispositionsCheck();
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "btnNext_Click()", "ctlLeadFormatDesigner.xaml.cs");
            }
         
        }

        void btnClose_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                cmbFieldName.Items.Clear();
                ClsLeadDesignerBusinessCollection objLeadDesigner = ClsLeadDesignerBusinessCollection.GetAll();
                for (int i = 0; i < objLeadDesigner.Count; i++)
                {
                    ComboBoxItem cbiFormat = new ComboBoxItem();
                    cbiFormat.Content = objLeadDesigner[i].FieldName;
                    cbiFormat.Tag = objLeadDesigner[i].ID;
                    cmbFieldName.Items.Add(cbiFormat);

                }
                template.Visibility = Visibility.Hidden;
                recTemp.Visibility = Visibility.Hidden;
                CtlGrid.IsEnabled = true;
                CtlGrid.Opacity = 5;
                CtlGridChild.IsEnabled = true;
                CtlGridChild.Opacity = 5;
                tbcDispositionList.IsEnabled = true;
                tbcDispositionList.Opacity = 5;
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "btnClose_Click()", "ctlLeadFormatDesigner.xaml.cs");
            }
        }

        void btnAddNewField_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                template.Visibility = Visibility.Visible;
                recTemp.Visibility = Visibility.Visible;
                txtFieldName.Text = "";
                txtFieldLength.Text = "";
                txtFieldSize.Text = "";
                cmbFieldType.SelectedIndex = 0;
                CtlGrid.IsEnabled = false;
                CtlGrid.Opacity = 0.2;
                CtlGridChild.IsEnabled = false;
                CtlGridChild.Opacity = 0.2;
                tbcDispositionList.IsEnabled = false;
                tbcDispositionList.Opacity = 0.2;
                Ellipse ep = new Ellipse();
                lstFieldNames.Items.Clear();


                templst = new List<object>();
                templst.Clear();


                ClsLeadDesignerBusinessCollection objInsertField = ClsLeadDesignerBusinessCollection.GetAll();
                for (int i = 0; i < objInsertField.Count; i++)
                {
                    ListBoxItem cbiFormat = new ListBoxItem();
                    cbiFormat.Content = objInsertField[i].FieldName;
                    cbiFormat.Tag = objInsertField[i].ID;
                    lstFieldNames.Items.Add(cbiFormat);
                    templst.Add(lstFieldNames.Items[i]);
                }
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "btnAddNewField_Click()", "ctlLeadFormatDesigner.xaml.cs");
            }
        }

        void ControlHide()
        {
            try
            {
                lblFieldStartPosition.Visibility = Visibility.Hidden;
                txtFieldStartPosition.Visibility = Visibility.Hidden;
                lblFieldLength.Visibility = Visibility.Hidden;
                txtFieldLength.Visibility = Visibility.Hidden;
                lblDelimiter.Visibility = Visibility.Hidden;
                cmbDelimiter.Visibility = Visibility.Hidden;
                lblColumn.Visibility = Visibility.Visible;
                cmbColumn.Visibility = Visibility.Visible;
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "ControlHide()", "ctlLeadFormatDesigner.xaml.cs");
            }
 
        }

        void ControlVisible()
        {
            try
            {
                lblFieldStartPosition.Visibility = Visibility.Visible;
                txtFieldStartPosition.Visibility = Visibility.Visible;
                lblFieldLength.Visibility = Visibility.Visible;
                txtFieldLength.Visibility = Visibility.Visible;
                lblDelimiter.Visibility = Visibility.Visible;
                cmbDelimiter.Visibility = Visibility.Visible;
                lblColumn.Visibility = Visibility.Hidden;
                cmbColumn.Visibility = Visibility.Hidden;
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "ControlVisible()", "ctlLeadFormatDesigner.xaml.cs");
            }
        }
        void BlankFields()
        {
            try
            {
                txtDescription.Text = "";
                cmbFieldName.SelectedIndex = -1;
                txtDefaultValue.Text = "";
                cmbColumn.SelectedIndex = -1;
                txtFieldStartPosition.Text = "";
                txtFieldLength.Text = "";
                cmbDelimiter.SelectedIndex = -1;
                // txtLeadFormatName.Text = "";
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "BlankFields()", "ctlLeadFormatDesigner.xaml.cs");
            }

        }
    }
}
