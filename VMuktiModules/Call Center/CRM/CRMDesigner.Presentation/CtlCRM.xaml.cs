using System;
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
using CRMDesigner.Business;
using VMuktiAPI;

namespace CRMDesigner.Presentation
{
    /// <summary>
    /// Interaction logic for CtlCRM.xaml
    /// </summary>
    /// 
    public enum ModulePermissions
    {
        Add = 0,
        Edit = 1,
        Delete = 2,
        View = 3
    }
    public partial class CtlCRM : UserControl
    {
        //public static StringBuilder sb1;
        int varState = 0;
        int varID = 0;
        Canvas cnv;
        ctlCRMDesigner objDesginer;
        ModulePermissions[] _MyPermissions;
        ClsCRMsCollection objCRMCollection = null;


        public CtlCRM(ModulePermissions[] MyPermissions)
        {
            try
            {
                InitializeComponent();
                varState = 0;
                _MyPermissions = MyPermissions;
                FncPermissionsReview();
                funSetGrid();
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "CtlCRM()", "CtlCRM.xaml.cs");
            }
        }

        //public static StringBuilder CreateTressInfo()
        //{
        //    StringBuilder sb = new StringBuilder();
        //    sb.Append("User Is : " + VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName);
        //    sb.AppendLine();
        //    sb.Append("Peer Type is : " + VMuktiAPI.VMuktiInfo.CurrentPeer.CurrPeerType.ToString());
        //    sb.AppendLine();
        //    sb.Append("User's SuperNode is : " + VMuktiAPI.VMuktiInfo.CurrentPeer.SuperNodeIP);
        //    sb.AppendLine();
        //    sb.Append("User's Machine Ip Address : " + VMuktiAPI.GetIPAddress.ClsGetIP4Address.GetIP4Address());
        //    sb.AppendLine();
        //    sb.AppendLine("----------------------------------------------------------------------------------------");
        //    return sb;
        //}

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
                ClsCRMs objSaveCRM = new ClsCRMs();

                //Checks wheather textbox containing CRMname is left blank or not.
                if (txtName.Text.Trim() == "")
                {
                    System.Windows.MessageBox.Show("CRM Name Can't Be Left Blank", "-> CRM Name", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    txtName.Focus();
                    txtName.Text = txtName.Text.Trim();
                    return;
                }

                //Check wheather to edit existing record or add new record.
                if (varState == 0)
                {
                    //Add new Record.
                    objSaveCRM.ID = -1;
                }
                else
                {
                    //Edit existing Record.
                    objSaveCRM.ID = varID;
                }

                //Set the object of the ClsCRM.
                objSaveCRM.CRMName = txtName.Text;
                objSaveCRM.IsActive = (bool)chkIsActive.IsChecked;
                objSaveCRM.CreatedBy = VMuktiAPI.VMuktiInfo.CurrentPeer.ID;

                //Call the save method of ClsCRM class of Vmukti.Business.
                int gID = objSaveCRM.Save();

                //Check wheather record is saved successfully or not.
                if (gID == 0)
                {
                    MessageBox.Show("Duplicate Entry For CRM Name Is Not Allowed !!", "-> CRM ", MessageBoxButton.OK, MessageBoxImage.Information);
                }

                else
                {
                    System.Windows.MessageBox.Show("Record Saved Successfully!!");
                    funSetGrid();
                    varState = 0;
                    FncClearAll();
                }
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            FncClearAll();
        }

        private void btnDesignCRM_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (cmbCRM.SelectedIndex!= -1)
                {
                    if (cnv == null)
                    {
                        cnv = new Canvas();
                        cnv.Height = 768;
                        cnv.Width = 1024;
                        cnv.SetValue(Canvas.LeftProperty, 0.0);
                        cnv.SetValue(Canvas.TopProperty, 0.0);
                        cnvView.Children.Clear();
                        //objDesginer = new ctlCRMDesigner(((ListBoxItem)(cmbCRM.SelectedItem)).Content.ToString());
                        objDesginer = new ctlCRMDesigner(((ListBoxItem)(cmbCRM.SelectedItem)).Tag.ToString());
                        //objDesginer = new ctlCRMDesigner(cmbCRM.SelectedItem.ToString());
                        //objDesginer.GetQuestions();
                        cnv.Children.Add(objDesginer);
                        cnvView.Children.Add(cnv);
                    }
                    else
                    {
                        cnv.Children.Clear();
                        cnv.Height = 768;
                        cnv.Width = 1024;
                        cnv.SetValue(Canvas.LeftProperty, 0.0);
                        cnv.SetValue(Canvas.TopProperty, 0.0);
                        //cnvView.Children.Clear();

                        //objDesginer = new ctlCRMDesigner(((ListBoxItem)(cmbCRM.SelectedItem)).Content.ToString());
                        objDesginer = new ctlCRMDesigner(((ListBoxItem)(cmbCRM.SelectedItem)).Tag.ToString());

                        //objDesginer = new ctlCRMDesigner(cmbCRM.SelectedItem.ToString());
                        //objDesginer.GetQuestions();
                        cnv.Children.Add(objDesginer);
                        cnvView.Children.Add(cnv);
                    }
                }
                else
                {
                    MessageBox.Show("Please Select any CRM Name");
                }
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "btnDesignCRM_Click()", "CtlCRM.xaml.cs");
            }
        }

        private void CtlGrid_btnEditClicked(int rowID)
        {
            try
            {
                //Get the id of the selected row.
                varID = Convert.ToInt32(objCRMCollection[rowID].ID);

                //Tab will be made visible for editing the record.
                btnSave.Visibility = Visibility.Visible;
                btnCancel.Visibility = Visibility.Visible;

                //Initialize the controls of the tab with selected row of the Grid.
                txtName.Text = objCRMCollection[rowID].CRMName;
                chkIsActive.IsChecked = (objCRMCollection[rowID].IsActive == true);

                //Set Flag variable that indicates Add or Edit
                varState = 1;
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "CtlGrid_btnEditClicked()", "CtlCRM.xaml.cs");
            }
        }

        private void CtlGrid_btnDeleteClicked(int rowID)
        {
            try
            {
                //Get the id of the selected row.
                varID = Convert.ToInt32(objCRMCollection[rowID].ID);

                //Gets the confirmation from the user for deleting record.
                MessageBoxResult r = System.Windows.MessageBox.Show("Do You Really Want To Delete This Record ?", "->Delete CRM", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (r == MessageBoxResult.Yes)
                {
                    //Calls the method of ClsCRM class of CRM.Business to delete the record.
                    ClsCRMs.Delete(varID);
                    System.Windows.MessageBox.Show("Record Deleted!!", "->CRM Deleted", MessageBoxButton.OK, MessageBoxImage.Information);

                    //Refersh the Grid.
                    funSetGrid();
                }
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "CtlGrid_btnDeleteClicked()", "CtlCRM.xaml.cs");
            }
        }

        void FncPermissionsReview()
        {
            try
            {
                //Setting property of Grid
                //Grid can not be edited.
                CtlGrid.CanEdit = false;

                //Grid can not be deleted.
                CtlGrid.CanDelete = false;

                //set the visibility of Grid to collapsed.
                CtlGrid.Visibility = Visibility.Collapsed;


                //Setting the property of tab control.
                btnSave.IsEnabled = false;
                btnCancel.IsEnabled = false;
                btnDesignCRM.IsEnabled = false;


                //Review the permission and set property according to Permission.
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
                        btnSave.IsEnabled = true;
                        btnCancel.IsEnabled = true;
                        btnDesignCRM.IsEnabled = true;
                    }
                }
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "CtlGrid_btnEditClicked()", "CtlCRM.xaml.cs");
            }
        }

        private void funSetGrid()
        {
            //This function set the Grid with existing available data.
            try
            {
                //Set the Grid.
                CtlGrid.Cols = 3;
                CtlGrid.CanEdit = true;
                CtlGrid.CanDelete = true;

                //Set the Header of the column of the grid.
                CtlGrid.Columns[0].Header = "ID";
                CtlGrid.Columns[1].Header = "CRM Name";
                CtlGrid.Columns[2].Header = "Is Active";
                CtlGrid.Columns[2].IsIcon = true;

                //Bind the column of the Grid.
                CtlGrid.Columns[0].BindTo("ID");
                CtlGrid.Columns[1].BindTo("CRMName");
                CtlGrid.Columns[2].BindTo("IsActive");

                //Calls the method of CRM.Business to retrive existing CRM.
                objCRMCollection = ClsCRMsCollection.GetAll();
                CtlGrid.Bind(objCRMCollection);

                cmbCRM.Items.Clear();
                for (int i = 0; i < objCRMCollection.Count; i++)
                {
                    ComboBoxItem cmb1 = new ComboBoxItem();
                    cmb1.Content = objCRMCollection[i].CRMName;
                    cmb1.Tag = objCRMCollection[i].ID;
                    cmbCRM.Items.Add(cmb1);
                }
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "funSetGrid()", "CtlCRM.xaml.cs");
            }
        }

        private void FncFillcmbCRM()
        {

            try
            {

                cmbCRM.Items.Clear();
                ClsCRMsCollection objClsCRMCollection = ClsCRMsCollection.GetAll();
                for (int i = 0; i < objClsCRMCollection.Count; i++)
                {
                    ComboBoxItem cmb1 = new ComboBoxItem();
                    cmb1.Content = objClsCRMCollection[i].CRMName;
                    cmb1.Tag = objClsCRMCollection[i].ID;
                    cmbCRM.Items.Add(cmb1);
                }
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "FncFillcmbCRM()", "CtlCRM.xaml.cs");
            }
        }

        private void FncClearAll()
        {
            try
            {
                txtName.Text = "";
                chkIsActive.IsChecked = false;

                //FncPermissionsReview();

                //Set variables to its default value.
                varID = -1;
                varState = 0;
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "FncClearAll()", "CtlCRM.xaml.cs");
            }
        }
    }
}
