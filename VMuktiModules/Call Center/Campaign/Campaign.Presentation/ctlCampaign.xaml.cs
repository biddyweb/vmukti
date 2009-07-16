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
using Campaign.Business;
using System.Data;
using System.Reflection;
using VMuktiAPI;

namespace Campaign.Presentation
{
    /// <summary>
    /// Interaction logic for ctlCampaign.xaml
    /// </summary>
    /// 
    //Set permission for campaign module
    public enum ModulePermissions
    {
        Add = 0,
        Edit = 1,
        Delete = 2,
        View = 3
    }

    public partial class CtlCampaign : System.Windows.Controls.UserControl
    {
        //public static StringBuilder sb1;
        int varState = 0;
        Int64 varID = -1;
        DataTable dtCampaign = new DataTable();
        ClsCampaignCollection objCampCollection = null;
        ModulePermissions[] _MyPermissions;

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

        public CtlCampaign(ModulePermissions[] MyPermissions)
        {
            try
            {

                InitializeComponent();
                _MyPermissions = MyPermissions;

                btnSelectGroups.Click += new RoutedEventHandler(btnSelectGroups_Click);
                btnDeselectGroups.Click += new RoutedEventHandler(btnDeselectGroups_Click);
                btnSelectAllAGroups.Click += new RoutedEventHandler(btnSelectAllAGroups_Click);
                btnSelectAllSGroup.Click += new RoutedEventHandler(btnSelectAllSGroup_Click);
                btnSelectNoneAGroups.Click += new RoutedEventHandler(btnSelectNoneAGroups_Click);
                btnSelectNoneSGroup.Click += new RoutedEventHandler(btnSelectNoneSGroup_Click);
                btnGroupUp.Click += new RoutedEventHandler(btnGroupUp_Click);
                btnGroupDown.Click += new RoutedEventHandler(btnGroupDown_Click);

                btnListUp.Click += new RoutedEventHandler(btnListUp_Click);
                btnListDown.Click += new RoutedEventHandler(btnListDown_Click);

                //setting Up All Events For DNC Lists //
                btnSelectDList.Click += new RoutedEventHandler(btnSelectDList_Click);
                btnDeSelectDList.Click += new RoutedEventHandler(btnDeSelectDList_Click);
                btnDListUp.Click += new RoutedEventHandler(btnDListUp_Click);
                btnDListDown.Click += new RoutedEventHandler(btnDListDown_Click);

                btnSelectAllADL.Click += new RoutedEventHandler(btnSelectAllADL_Click);
                btnSelectNoneADL.Click += new RoutedEventHandler(btnSelectNoneADL_Click);
                btnSelectAllSDL.Click += new RoutedEventHandler(btnSelectAllSDL_Click);
                btnSelectNoneSDL.Click += new RoutedEventHandler(btnSelectNoneSDL_Click);
                //txtNoOfChannels.KeyDown += new KeyEventHandler(txtNoOfChannels_KeyDown);
                txtCampaignPrefix.KeyDown += new KeyEventHandler(txtCampaignPrefix_KeyDown);

                txtNoOfChannels.TextChanged += new TextChangedEventHandler(txtNoOfChannels_TextChanged);
                txtCampaignPrefix.TextChanged += new TextChangedEventHandler(txtCampaignPrefix_TextChanged);
                txtCallerId.KeyUp += new KeyEventHandler(txtCallerId_KeyUp);

                //setting Up All Events For DNC Lists //

                varState = 0;
                varID = -1;

                dtpStartDate.Value = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
                dtpEndDate.Value = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
                FncPermissionsReview();
                FncFillAll();
                funSetGrid();

                this.Loaded += new RoutedEventHandler(CtlCampaign_Loaded);

            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "CtlCampaign()", "CtlCampaign.xaml.cs");               
            }
        }

        void CtlCampaign_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                this.Width = ((Grid)this.Parent).ActualWidth;
                ((Grid)this.Parent).SizeChanged += new SizeChangedEventHandler(CtlCampaign_SizeChanged);
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "CtlCampaign_Loaded", "CtlCampaign.xaml.cs");
            }
        }

        void CtlCampaign_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            try
            {
                this.Width = e.NewSize.Width;
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "CtlCampaign_Loaded", "CtlCampaign.xaml.cs");
            }
        }

        void txtCampaignPrefix_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {

                #region Allowing only numbers and Tab

                if (!((e.Key >= Key.D0 && e.Key <= Key.D9) ||
                    (e.Key == Key.Decimal) ||
                    e.Key == Key.Tab ||
                     (e.Key >= Key.NumPad0 && e.Key <= Key.NumPad9)))
                {
                    e.Handled = true;
                }

                #endregion
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "txtCampaignPrefix_KeyDown", "CtlCampaign.xaml.cs");
              
            }
        }
        void txtCampaignPrefix_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                IsValidValue(ref sender);
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "txtCampaignPrefix_TextChanged", "ctlCampaign.xaml.cs");
            }
        }

        void txtNoOfChannels_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {

                #region Allowing only numbers and Tab

                if (!((e.Key >= Key.D0 && e.Key <= Key.D9) ||
                    (e.Key == Key.Decimal) ||
                    e.Key == Key.Tab ||
                     (e.Key >= Key.NumPad0 && e.Key <= Key.NumPad9)))
                {
                    e.Handled = true;
                }

                #endregion
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "txtNoOfChannels_KeyDown", "ctlCampaign.xaml.cs");
            }
        }
        void txtNoOfChannels_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                        
            IsValidValue(ref sender);

        }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "txtNoOfChannels_TextChanged", "ctlCampaign.xaml.cs");
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
                        ((TextBox)obj).BorderThickness = new Thickness(0, 0, 0, 0);
                        ((TextBox)obj).BorderBrush = Brushes.Transparent;
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
                        ((PasswordBox)obj).BorderThickness = new Thickness(0, 0, 0, 0);
                        ((PasswordBox)obj).BorderBrush = Brushes.Transparent;
                    }
                }
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "IsValidValue()", "ctlCampaign.xaml.cs");
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
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "IsFloat()", "ctlCampaign.xaml.cs");
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
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "IsMixInt()", "ctlCampaign.xaml.cs");
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
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "IsPositiveInt()", "ctlCampaign.xaml.cs");
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
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "IsNegativeInt()", "ctlCampaign.xaml.cs");
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
                tbcCampaign.Visibility = Visibility.Collapsed;
                btnSaveOD.Visibility = Visibility.Collapsed;
                btnCancelOD.Visibility = Visibility.Collapsed;

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
                        tbcCampaign.Visibility = Visibility.Visible;
                        btnSaveOD.Visibility = Visibility.Visible;
                        btnCancelOD.Visibility = Visibility.Visible;
                    }
                }
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "FncPermissionsReview()", "ctlCampaign.xaml.cs");
            } 
        }     

        void btnSelectNoneSDL_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                unselectall(lstSelectedDList);
            }

            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "btnSelectNoneSDL_Click", "ctlCampaign.xaml.cs");
            }
        }
        void btnSelectAllSDL_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                selectall(lstSelectedDList);
            }

            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "btnSelectAllSDL_Click", "ctlCampaign.xaml.cs");

            }
        }
        void btnSelectNoneADL_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                unselectall(lstAvailableDLists);
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "btnSelectNoneADL_Click", "ctlCampaign.xaml.cs");
            }
        }
        void btnSelectAllADL_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                selectall(lstAvailableDLists);
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "btnSelectAllADL_Click", "ctlCampaign.xaml.cs");
            }
        }

        void btnDListDown_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                moveDown(lstSelectedDList);
            }

            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "btnDListDown_Click", "ctlCampaign.xaml.cs");

            }
        }
        void btnDListUp_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                moveUp(lstSelectedDList);
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "btnDListUp_Click", "ctlCampaign.xaml.cs");

            }
        }

        void btnDeSelectDList_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                moveselected(lstSelectedDList, lstAvailableDLists);
            }

            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "btnDeSelectDList_Click", "ctlCampaign.xaml.cs");

            }
        }
        void btnSelectDList_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                moveselected(lstAvailableDLists, lstSelectedDList);
            }

            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "btnSelectDList_Click", "ctlCampaign.xaml.cs");

            }
        }

        void fillGroups()
        {
            try
            {
                lstAvailableGroups.Items.Clear();
                lstSelectedGroups.Items.Clear();

                // -1 is set for campaignID for All Available Groups
                ClsGroupCollection objGroupCollection = ClsGroupCollection.GetAll(-1);
                for (int i = 0; i < objGroupCollection.Count; i++)
                {
                    ListBoxItem itmGroup = new ListBoxItem();
                    itmGroup.Content = objGroupCollection[i].GroupName;
                    itmGroup.Tag = objGroupCollection[i].ID;
                    if (objGroupCollection[i].IsActive == true)
                        itmGroup.Foreground = Brushes.Green;
                    else
                        itmGroup.Foreground = Brushes.Red;
                    lstAvailableGroups.Items.Add(itmGroup);
                }
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "fillGroups()", "ctlCampaign.xaml.cs");  
            }
        }
        void fillScripts()
        {
            try
            {
                cmbScriptName.Items.Clear();
                // Get all data for scripts from ClsScriptCollection 
                ClsScriptCollection objScriptCollection = ClsScriptCollection.GetAll();
                for (int i = 0; i < objScriptCollection.Count; i++)
                {
                    ComboBoxItem itmScript = new ComboBoxItem();
                    itmScript.Content = objScriptCollection[i].ScriptName;
                    itmScript.Tag = objScriptCollection[i].ID;
                    if (objScriptCollection[i].IsActive == true)
                        itmScript.Foreground = Brushes.Green;
                    else
                        itmScript.Foreground = Brushes.Red;
                    cmbScriptName.Items.Add(itmScript);
                }

                //CRM
                cmbCRMName.Items.Clear();
                //Get all data for CRM from ClsCRMCollection 
                ClsCRMCollection objCRMCollection = ClsCRMCollection.GetAll();
                for (int i = 0; i < objCRMCollection.Count; i++)
                {
                    ComboBoxItem itmScript = new ComboBoxItem();
                    itmScript.Content = objCRMCollection[i].CRMName;
                    itmScript.Tag = objCRMCollection[i].ID;
                    if (objCRMCollection[i].IsActive == true)
                        itmScript.Foreground = Brushes.Green;
                    else
                        itmScript.Foreground = Brushes.Red;
                    cmbCRMName.Items.Add(itmScript);
                }
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "fillScripts()", "ctlCampaign.xaml.cs");  
            } 
        }
        void fillCallingLists()
        {
            try
            {
                lstSelectedList.Items.Clear();
                lstAvailableLists.Items.Clear();
                // -1 for All Available CallingLists
                ClsCallingListCollection objCallingListCollection = ClsCallingListCollection.GetAll(false, -1);
                for (int i = 0; i < objCallingListCollection.Count; i++)
                {
                    ListBoxItem itmList = new ListBoxItem();
                    itmList.Content = objCallingListCollection[i].ListName;
                    itmList.Tag = objCallingListCollection[i].ID;
                    if (objCallingListCollection[i].IsActive == true)
                        itmList.Foreground = Brushes.Green;
                    else
                        itmList.Foreground = Brushes.Red;
                    lstAvailableLists.Items.Add(itmList);
                }
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "fillCallingLists()", "ctlCampaign.xaml.cs");  
            } 
        }

        void fillDNCLists()
        {
            try
            {
                lstSelectedDList.Items.Clear();
                lstAvailableDLists.Items.Clear();

                // -1 for All Available DNCLists
                ClsCallingListCollection objDNCListCollection = ClsCallingListCollection.GetAll(true, -1);
                for (int i = 0; i < objDNCListCollection.Count; i++)
                {
                    ListBoxItem itmList = new ListBoxItem();
                    itmList.Content = objDNCListCollection[i].ListName;
                    itmList.Tag = objDNCListCollection[i].ID;
                    if (objDNCListCollection[i].IsActive == true)
                        itmList.Foreground = Brushes.Green;
                    else
                        itmList.Foreground = Brushes.Red;
                    lstAvailableDLists.Items.Add(itmList);
                }
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "fillDNCLists()", "ctlCampaign.xaml.cs");  
            } 
        }

        void fillTreatments()
        {
            try
            {
                lstAvailableTreatments.Items.Clear();
                lstSelectedTreatments.Items.Clear();

                // -1 for All Available Treatments
                ClsTreatmentCollection objTreatmentCollection = ClsTreatmentCollection.GetAll(-1);
                for (int i = 0; i < objTreatmentCollection.Count; i++)
                {
                    ListBoxItem itmTreatment = new ListBoxItem();
                    itmTreatment.Content = objTreatmentCollection[i].TreatmentName;
                    itmTreatment.Tag = objTreatmentCollection[i].ID;
                    lstAvailableTreatments.Items.Add(itmTreatment);
                }
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "fillTreatments()", "ctlCampaign.xaml.cs");  
            } 
        }

        void fillDispositionLists()
        {
            try
            {
                cmbDispositionList.Items.Clear();

                // -1 for All Available DispositionLists
                ClsDispositionListCollection objDispListCollection = ClsDispositionListCollection.GetAll(-1);
                for (int i = 0; i < objDispListCollection.Count; i++)
                {
                    ComboBoxItem itmList = new ComboBoxItem();
                    itmList.Content = objDispListCollection[i].DispListName;
                    itmList.Tag = objDispListCollection[i].ID;
                    if (objDispListCollection[i].IsActive == true)
                        itmList.Foreground = Brushes.Green;
                    else
                        itmList.Foreground = Brushes.Red;
                    cmbDispositionList.Items.Add(itmList);
                }
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "fillDispositionLists()", "ctlCampaign.xaml.cs");
            } 
        }
        //Function for set data in the grid  
        void funSetGrid()
        {
            try
            {
                CtlGrid.Cols = 12;
                //CtlGrid.CanEdit = true;
                //CtlGrid.CanDelete = true;
                //Setting the header of the grid
                CtlGrid.Columns[0].Header = "Campaign ID";
                CtlGrid.Columns[1].Header = "Campaign Name";
                CtlGrid.Columns[2].Header = "Description";
                //CtlGrid.Columns[3].Header = "NoOfChannels";
                CtlGrid.Columns[3].Header = "CampaginPrefix";
                CtlGrid.Columns[4].Header = "Caller ID";
                CtlGrid.Columns[5].Header = "IsActive";
                CtlGrid.Columns[5].IsIcon = true;
                CtlGrid.Columns[6].Header = "Dialer Type";
                CtlGrid.Columns[7].Header = "Assign To";
                CtlGrid.Columns[8].Header = "Script ID";
                //CtlGrid.Columns[9].Header = "Park Extension";
                //CtlGrid.Columns[10].Header = "Park File Name";
                CtlGrid.Columns[9].Header = "Start Date";
                CtlGrid.Columns[10].Header = "End Date";
                //CtlGrid.Columns[13].Header = "CallingTime";
                CtlGrid.Columns[11].Header = "Rec.FileFormat";
                
                CtlGrid.Columns[0].BindTo("ID");
                CtlGrid.Columns[1].BindTo("Name");
                CtlGrid.Columns[2].BindTo("Description");
                //CtlGrid.Columns[3].BindTo("NoOfChannels");
                CtlGrid.Columns[3].BindTo("Prefix");
                CtlGrid.Columns[4].BindTo("CallerID");
                CtlGrid.Columns[5].BindTo("IsActive");
                CtlGrid.Columns[6].BindTo("DType");
                CtlGrid.Columns[7].BindTo("AssignTo");
                CtlGrid.Columns[8].BindTo("ScriptID");
                //CtlGrid.Columns[9].BindTo("ParkExtension");
                //CtlGrid.Columns[10].BindTo("ParkFileName");
                CtlGrid.Columns[9].BindTo("StartDate");
                CtlGrid.Columns[10].BindTo("EndDate");
                //CtlGrid.Columns[13].BindTo("CallingTime");
                CtlGrid.Columns[11].BindTo("RecordingFileFormat");

                //Function for populating all data for campaign
                objCampCollection = Campaign.Business.ClsCampaignCollection.GetAll();
                // Binding data
                CtlGrid.Bind(objCampCollection);
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "funSetGrid()", "ctlCampaign.xaml.cs");
            } 
        }
        // To delete campaign record
        void CtlGrid_btnDeleteClicked(int RowID)
        {
            try
            {
                varID = objCampCollection[RowID].ID;

                if (System.Windows.MessageBox.Show("Do You Really Want To Delete This Record ?", "Delete Campaign", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    //Calling Delete function of ClsCampaign to delete the record for the particular campaignID
                    ClsCampaign.Delete(varID);
                    System.Windows.MessageBox.Show("Record Deleted!!", "Campaign Deleted", MessageBoxButton.OK, MessageBoxImage.Information);

                }
                funSetGrid();
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "CtlGrid_btnDeleteClicked", "ctlCampaign.xaml.cs");
            }
        }
        //To edit the campaign details 
        void CtlGrid_btnEditClicked(int RowID)
        {
            try
            {
            //Function for clearing all data
            FncClearAll();
            //Function for populating all data 
            FncFillAll();

            tbcCampaign.Visibility = Visibility.Visible;
            btnSaveOD.Visibility = Visibility.Visible;
            btnCancelOD.Visibility = Visibility.Visible;

            varID = objCampCollection[RowID].ID;

            ClsCampaign objCamp = ClsCampaign.GetByCampaignID(varID);

            txt_CampaignName.Text = objCamp.Name;
            txtDescription.Text = objCamp.Description;
                txtCampaignPrefix.Text = objCamp.Prefix.ToString();
            txtNoOfChannels.Text = objCamp.NoOfChannels.ToString();
            txtCallerId.Text = objCamp.CallerID.ToString();
            if (objCamp.IsActive == true)
                chkIsActive.IsChecked = true;
            else
                chkIsActive.IsChecked = false;

            txtStartDate.Text = objCamp.StartDate.ToString();
            txtEndDate.Text = objCamp.EndDate.ToString();
            txtParkExtension.Text = objCamp.ParkExtension.ToString();
            txtParkFileName.Text = objCamp.ParkFileName;
            if (objCamp.DType.ToUpper() == "MANUAL")
                rdoManual.IsChecked = true;
            else
                rdoPredictive.IsChecked = true;

            txtAssignedTo.Text = objCamp.AssignTo;
            //txtTotalLeads.Text = objCamp.TotalLeadsProvided.ToString();
            //objCamp.CallingTime = GetInt(row, "CallingTime");
            cmbRecordingFileFormat.Text = objCamp.RecordingFileFormat;
            dtpStartDate.Value = objCamp.StartDate;
            dtpEndDate.Value = objCamp.EndDate;

            for (int i = 0; i < cmbCallingTimes.Items.Count; i++)
            {
                if (((ListBoxItem)cmbCallingTimes.Items[i]).Tag.ToString() == objCamp.CallingTime.ToString())
                    cmbCallingTimes.Text = ((ListBoxItem)cmbCallingTimes.Items[i]).Content.ToString();

            }

            for (int i = 0; i < cmbScriptName.Items.Count; i++)
            {
                if (((ListBoxItem)cmbScriptName.Items[i]).Tag.ToString() == objCamp.ScriptID.ToString())
                    cmbScriptName.Text = ((ListBoxItem)cmbScriptName.Items[i]).Content.ToString();

            }

            for (int i = 0; i < cmbCRMName.Items.Count; i++)
            {
                if (((ListBoxItem)cmbCRMName.Items[i]).Tag.ToString() == objCamp.CRMID.ToString())
                    cmbCRMName.Text = ((ListBoxItem)cmbCRMName.Items[i]).Content.ToString();

            }

            ClsDispositionListCollection objDListCollection = ClsDispositionListCollection.GetAll(varID);

            if (objDListCollection.Count > 0)
            {
                for (int i = 0; i < cmbDispositionList.Items.Count; i++)
                {
                    if (((ListBoxItem)cmbDispositionList.Items[i]).Tag.ToString() == objDListCollection[0].ID.ToString())
                        cmbDispositionList.Text = ((ListBoxItem)cmbDispositionList.Items[i]).Content.ToString();
                }
            }

            #region FillSelected While Editing A Record

            //Filling Selected Group List

            ClsGroupCollection objGCollection = ClsGroupCollection.GetAll(varID);

            for (int i = 0; i < objGCollection.Count; i++)
            {
                ListBoxItem newItem = new ListBoxItem();
                newItem.Content = objGCollection[i].GroupName.ToString();
                newItem.Tag = objGCollection[i].ID.ToString();
                
                if (objGCollection[i].IsActive == true)
                {
                    newItem.Foreground = Brushes.Green;
                }
                else
                {
                    newItem.Foreground = Brushes.Red;
                }
                lstSelectedGroups.Items.Add(newItem);
                for (int j = 0; j < lstAvailableGroups.Items.Count; j++)
                {
                    if (((ListBoxItem)lstAvailableGroups.Items[j]).Tag.ToString() == objGCollection[i].ID.ToString())
                        lstAvailableGroups.Items.Remove(lstAvailableGroups.Items[j]);
                }
            }


            //Filling Selected Treatment List

            ClsTreatmentCollection objTCollection = ClsTreatmentCollection.GetAll(varID);

            for (int i = 0; i < objTCollection.Count; i++)
            {
                ListBoxItem newItem = new ListBoxItem();
                newItem.Content = objTCollection[i].TreatmentName.ToString();
                newItem.Tag = objTCollection[i].ID.ToString();
                
                lstSelectedTreatments.Items.Add(newItem);
                for (int j = 0; j < lstAvailableTreatments.Items.Count; j++)
                {
                    if (((ListBoxItem)lstAvailableTreatments.Items[j]).Tag.ToString() == objTCollection[i].ID.ToString())
                        lstAvailableTreatments.Items.Remove(lstAvailableTreatments.Items[j]);
                }
            }


            //Filling Selected Calling List

            ClsCallingListCollection objCLCollection = ClsCallingListCollection.GetAll(false, varID);

            for (int i = 0; i < objCLCollection.Count; i++)
            {
                ListBoxItem newItem = new ListBoxItem();
                newItem.Content = objCLCollection[i].ListName.ToString();
                newItem.Tag = objCLCollection[i].ID.ToString();
                
                if (objCLCollection[i].IsActive == true)
                {
                    newItem.Foreground = Brushes.Green;
                }
                else
                {
                    newItem.Foreground = Brushes.Red;
                }
                lstSelectedList.Items.Add(newItem);
                for (int j = 0; j < lstAvailableLists.Items.Count; j++)
                {
                    if (((ListBoxItem)lstAvailableLists.Items[j]).Tag.ToString() == objCLCollection[i].ID.ToString())
                        lstAvailableLists.Items.Remove(lstAvailableLists.Items[j]);
                }
            }


            //Filling Selected DNC List

            ClsCallingListCollection objCDCollection = ClsCallingListCollection.GetAll(true, varID);

            for (int i = 0; i < objCDCollection.Count; i++)
            {
                ListBoxItem newItem = new ListBoxItem();
                newItem.Content = objCDCollection[i].ListName.ToString();
                newItem.Tag = objCDCollection[i].ID.ToString();
                

                if (objCDCollection[i].IsActive == true)
                {
                    newItem.Foreground = Brushes.Green;
                }
                else
                {
                    newItem.Foreground = Brushes.Red;
                }
                lstSelectedDList.Items.Add(newItem);
                for (int j = 0; j < lstAvailableDLists.Items.Count; j++)
                {
                    if (((ListBoxItem)lstAvailableDLists.Items[j]).Tag.ToString() == objCDCollection[i].ID.ToString())
                        lstAvailableDLists.Items.Remove(lstAvailableDLists.Items[j]);
                }
            }

            #endregion

            varState = 1;
            CtlGrid.IsEnabled = false;

        }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "CtlGrid_btnEditClicked", "ctlCampaign.xaml.cs");
            }
        }
        //Function for clearing all data
        public void FncClearAll()
        {
            try
            {
                txt_CampaignName.Text = "";
                txtDescription.Text = "";
                txtNoOfChannels.Text = "";
                txtCampaignPrefix.Text = "";
                txtCallerId.Text = "";
                chkIsActive.IsChecked = false;
                txtStartDate.Text = "";
                txtEndDate.Text = "";
                txtParkExtension.Text = "";
                txtParkFileName.Text = "";
                rdoPredictive.IsChecked = true;
                txtAssignedTo.Text = "";

                //objCamp.CallingTime = GetInt(row, "CallingTime");
                cmbRecordingFileFormat.Text = "";

                lstSelectedGroups.Items.Clear();
                lstSelectedList.Items.Clear();
                lstSelectedTreatments.Items.Clear();
                lstSelectedDList.Items.Clear();
                lstAvailableGroups.Items.Clear();
                lstAvailableLists.Items.Clear();
                lstAvailableTreatments.Items.Clear();
                lstAvailableDLists.Items.Clear();
                cmbDispositionList.Items.Clear();
                cmbScriptName.Items.Clear();
                cmbCRMName.Items.Clear();
                dtpEndDate.Value = null;
                dtpStartDate.Value = null;
                cmbCallingTimes.Text = "";
                tbiCampaignDetails.IsSelected = true;
                CtlGrid.IsEnabled = true;
                dtpStartDate.Value = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
                dtpEndDate.Value = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
                FncPermissionsReview();
                varID = 0;
                varState = 0;
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "FncClearAll()", "ctlCampaign.xaml.cs");
            } 
        }
        //Function for populating all data
        public void FncFillAll()
        {
            try
            {
                //Function for populating all the available groups data in the grid
                fillGroups();
                //Function for populating all the available scripts in the grid
                fillScripts();
                //Function for populating all the available callinglists in the grid
                fillCallingLists();
                //Function for populating all the available DNClists in the grid
                fillDNCLists();
                fillTreatments();
                //Function for populating all the available Dispositionlists in the grid
                fillDispositionLists();

                dtpEndDate.Value = null;
                dtpStartDate.Value = null;
                cmbCallingTimes.Text = "";
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "FncFillAll()", "ctlCampaign.xaml.cs");
            } 
        }
        //To set priority of calling list through the movedown button
        void btnListDown_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                moveDown(lstSelectedList);
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "btnListDown_Click", "ctlCampaign.xaml.cs");
            }
        }
        //To set priority of calling list through the moveup button
        void btnListUp_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                moveUp(lstSelectedList);
            }

            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "btnListUp_Click", "ctlCampaign.xaml.cs");

            }
        }
        // Group Buttons//
        //To movedown the groupname
        void btnGroupDown_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                moveDown(lstSelectedGroups);
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "btnGroupDown_Click", "ctlCampaign.xaml.cs");
            }
        }
        //To moveup the groupname
        void btnGroupUp_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                moveUp(lstSelectedGroups);
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "btnGroupUp_Click", "ctlCampaign.xaml.cs");
            }
        }
        //To deselect Selectedgroups listbox items
        void btnSelectNoneSGroup_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                unselectall(lstSelectedGroups);
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "btnSelectNoneSGroup_Click", "ctlCampaign.xaml.cs");
            }
        }
        //To deselect Availablegroups listbox items
        void btnSelectNoneAGroups_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                unselectall(lstAvailableGroups);
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "btnSelectNoneAGroups_Click", "ctlCampaign.xaml.cs");
            }
        }
        //To selectall Selectedgroups listbox items
        void btnSelectAllSGroup_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                selectall(lstSelectedGroups);
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "btnSelectAllSGroup_Click", "ctlCampaign.xaml.cs");
            }
        }
        //To selectall Availablegroups listbox items
        void btnSelectAllAGroups_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                selectall(lstAvailableGroups);
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "btnSelectAllAGroups_Click", "ctlCampaign.xaml.cs");
            }
        }
        //Transfering Groups from selected listbox to available listbox
        void btnDeselectGroups_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                moveselected(lstSelectedGroups, lstAvailableGroups);
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "btnDeselectGroups_Click", "ctlCampaign.xaml.cs");
            }
        }
        //Transfering Groups from available listbox to selected listbox
        void btnSelectGroups_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                moveselected(lstAvailableGroups, lstSelectedGroups);
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "btnSelectGroups_Click", "ctlCampaign.xaml.cs");
            }
        }
        // Group Buttons//
        void moveselected(System.Windows.Controls.ListBox source, System.Windows.Controls.ListBox dest)
        {
            try
            {
                ListBoxItem[] lbi = new ListBoxItem[source.SelectedItems.Count];
                source.SelectedItems.CopyTo(lbi, 0);
                for (int i = 0; i < lbi.Length; i++)
                {
                    source.Items.Remove(lbi[i]);
                    dest.Items.Add(lbi[i]);
                }
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "moveselected()", "ctlCampaign.xaml.cs");
            }
        }
        //Function for selecting all listbox items
        void selectall(System.Windows.Controls.ListBox l)
        {
            try
            {
                l.SelectAll();
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "selectall()", "ctlCampaign.xaml.cs");
            }
        }
        //Function for unselecting listbox items
        void unselectall(System.Windows.Controls.ListBox l)
        {
            try
            {
                l.UnselectAll();
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "unselectall()", "ctlCampaign.xaml.cs");
            }
        }
        //Function for moving up listbox items
        void moveUp(System.Windows.Controls.ListBox l)
        {
            try
            {
                if (l.SelectedItems.Count == 1)
                {
                    ListBoxItem lbiTemp = new ListBoxItem();
                    lbiTemp = (ListBoxItem)l.SelectedItem;

                    int varIndex = l.SelectedIndex;
                    if (l.SelectedIndex != 0)
                    {
                        l.Items.RemoveAt(l.SelectedIndex);
                        l.Items.Insert(varIndex - 1, lbiTemp);
                    }
                    else
                    {
                        l.Items.RemoveAt(l.SelectedIndex);
                        l.Items.Insert(varIndex, lbiTemp);
                    }
                    l.SelectedIndex = varIndex - 1;
                }
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "moveUp()", "ctlCampaign.xaml.cs");
            }
        }
        //Function for moving down listbox items
        void moveDown(System.Windows.Controls.ListBox l)
        {
            try
            {
                if (l.SelectedItems.Count == 1)
                {
                    ListBoxItem lbiTemp = new ListBoxItem();
                    lbiTemp = (ListBoxItem)l.SelectedItem;

                    int varIndex = l.SelectedIndex;

                    if (l.SelectedIndex != l.Items.Count - 1)
                    {
                        l.Items.RemoveAt(l.SelectedIndex);
                        l.Items.Insert(varIndex + 1, lbiTemp);
                    }
                    else
                    {
                        l.Items.RemoveAt(l.SelectedIndex);
                        l.Items.Insert(varIndex, lbiTemp);
                    }
                    l.SelectedIndex = varIndex + 1;
                }
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "moveDown()", "ctlCampaign.xaml.cs");
            }
        }
        //To selectall callinglist items
        private void btnSelectListAll_Click(object sender, System.Windows.RoutedEventArgs e)
        {
           
        }
        //To Deselect all callinglist items
        private void btnDeSelectListAll_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            
        }
        private void btnInsert_Click(object sender, RoutedEventArgs e)
        {
           
        }
        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
           
        }
        private void txtCompanyId_GotFocus(object sender, RoutedEventArgs e)
        {
           
        }
        private void txtCompanyId_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
           
        }
        private void txtCompanyId_LostFocus(object sender, RoutedEventArgs e)
        {
           
        }
        private void txtName_LostFocus(object sender, RoutedEventArgs e)
        {
           
        }
        private void txtName_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
          
        }
        private void txtName_GotFocus(object sender, RoutedEventArgs e)
        {
           
        }
        private void btnAddNewT_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            
        }
        private void btnSelectAllT_Click(object sender, System.Windows.RoutedEventArgs e)
        {
           
        }
        private void btnSelectAT_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            try
            {
                moveselected(lstAvailableTreatments, lstSelectedTreatments);
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "btnSelectAT_Click", "ctlCampaign.xaml.cs");
            }
        }
        private void btnDeselectT_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            try
            {
                moveselected(lstSelectedTreatments, lstAvailableTreatments);
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "btnDeselectT_Click", "ctlCampaign.xaml.cs"); 
            }
        }
        private void btnDeselectAllT_Click(object sender, System.Windows.RoutedEventArgs e)
        {
           
        }
        private void btnSelectAllAT_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            try
            {
                selectall(lstAvailableTreatments);
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "btnSelectAllAT_Click", "ctlCampaign.xaml.cs"); 
            }
        }
        private void btnSelectNoneAT_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            try
            {
                unselectall(lstAvailableTreatments);
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "btnSelectNoneAT_Click","ctlCampaign.xaml.cs"); 
            }
        }
        private void btnSelectAllST_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            try
            {
                selectall(lstSelectedTreatments);
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "btnSelectAllST_Click", "ctlCampaign.xaml.cs"); 
            }
        }
        private void btnSelectNoneST_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            try
            {
                unselectall(lstSelectedTreatments);
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "btnSelectNoneST_Click", "ctlCampaign.xaml.cs"); 
            }
        }
        private void btnCancleT_Click(object sender, System.Windows.RoutedEventArgs e)
        {
          
        }
        private void btnSaveT_Click(object sender, System.Windows.RoutedEventArgs e)
        {
           
        }
        private void btnMoveUpST_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            try
            {
                moveUp(lstSelectedTreatments);
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "btnMoveUpST_Click", "ctlCampaign.xaml.cs"); 
            }
        }

        private void btnMoveDownST_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            try
            {
                moveDown(lstSelectedTreatments);
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "btnMoveDownST_Click", "ctlCampaign.xaml.cs"); 
            }
        }
        private void lstAvailableTreatments_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
           
        }
        private void btnSelectAllAgent_Click(object sender, System.Windows.RoutedEventArgs e)
        {
           
        }
        private void btnSelectAgent_Click(object sender, System.Windows.RoutedEventArgs e)
        {
           
        }
        private void btnDeselectAgent_Click(object sender, System.Windows.RoutedEventArgs e)
        {
           
        }
        private void btnDeselectAllAgent_Click(object sender, System.Windows.RoutedEventArgs e)
        {
           
        }
        private void btnSelectAllAA_Click(object sender, System.Windows.RoutedEventArgs e)
        {
           
        }
        private void btnSelectNoneAA_Click(object sender, System.Windows.RoutedEventArgs e)
        {
           
        }
        private void btnSelectAllSA_Click(object sender, System.Windows.RoutedEventArgs e)
        {
           
        }
        private void btnSelectNoneSA_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            
        }
        //To save Group in Campaign
        private void btnGroupSave_Click(object sender, System.Windows.RoutedEventArgs e)
        {
           
        }
        //To Cancel editing and deleting 
        private void btnGroupCancel_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            try
            {
                FncClearAll();
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "btnGroupCancel_Click", "ctlCampaign.xaml.cs"); 

            }
        }
        private void btnAddNewAA_Click(object sender, System.Windows.RoutedEventArgs e)
        {
           
        }
        private void lstAvailableAgents_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
          
        }
        private void lstSelectedAgents_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
          
        }
        private void tbiCampaignDetails_GotFocus(object sender, RoutedEventArgs e)
        {
           
        }
        private void tbiCampaignDetails_LostFocus(object sender, RoutedEventArgs e)
        {
           
        }
        private void tbiList_LostFocus(object sender, RoutedEventArgs e)
        {
           
        }
        private void tbiList_GotFocus(object sender, RoutedEventArgs e)
        {
          
        }

        private void tbiTreatments_GotFocus(object sender, RoutedEventArgs e)
        {
           
        }
        private void tbiTreatments_LostFocus(object sender, RoutedEventArgs e)
        {
           
        }
        private void tbiAgentGroups_LostFocus(object sender, RoutedEventArgs e)
        {
            
        }
        private void tbiAgentGroups_GotFocus(object sender, RoutedEventArgs e)
        {
           
        }
        private void tbiOtherDetails_GotFocus(object sender, RoutedEventArgs e)
        {
           
        }
        private void tbiOtherDetails_LostFocus(object sender, RoutedEventArgs e)
        {
           
        }
        private void txt_CampaignName_GotFocus(object sender, RoutedEventArgs e)
        {
           
        }
        private void txt_CampaignName_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
           
        }
        private void txt_CampaignName_LostFocus(object sender, RoutedEventArgs e)
        {
           
        }
        private void txtDescription_GotFocus(object sender, RoutedEventArgs e)
        {
           
        }
        private void txtDescription_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
           
        }
        private void txtDescription_LostFocus(object sender, RoutedEventArgs e)
        {
           
        }
        private void cmbNoOfChannels_GotFocus(object sender, RoutedEventArgs e)
        {
           
        }
        private void cmbNoOfChannels_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            
        }
        private void cmbNoOfChannels_LostFocus(object sender, RoutedEventArgs e)
        {
           
        }
        private void cmbNoOfChannels_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            
        }
        private void txtCallerId_GotFocus(object sender, RoutedEventArgs e)
        {
            
        }
        private void txtCallerId_LostFocus(object sender, RoutedEventArgs e)
        {
           
        }
        bool isShiftDown = false;
        private void txtCallerId_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            try
            {
                #region Allowing only numbers and Tab

                if (e.Key == Key.RightShift || e.Key == Key.LeftShift)
                {
                    isShiftDown = true;
                }

                if (!((e.Key >= Key.D0 && e.Key <= Key.D9) ||
                    (e.Key == Key.Decimal) ||
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
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "txtCallerId_KeyDown", "ctlCampaign.xaml.cs");
            }
        }
        void txtCallerId_KeyUp(object sender, KeyEventArgs e)
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
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "txtCallerId_KeyUp", "ctlCampaign.xaml.cs");
            }
        }
        private void rdoyes_GotFocus(object sender, RoutedEventArgs e)
        {
           
        }
        private void rdoyes_Checked(object sender, RoutedEventArgs e)
        {
           
        }

        private void rdoNo_Checked(object sender, RoutedEventArgs e)
        {
           
        }
        private void rdoNo_GotFocus(object sender, RoutedEventArgs e)
        {
           
        }
        private void txtStartDate_GotFocus(object sender, RoutedEventArgs e)
        {
           
        }
        private void txtStartDate_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            
        }
        private void txtStartDate_LostFocus(object sender, RoutedEventArgs e)
        {
          
        }
        private void txtEndDate_GotFocus(object sender, RoutedEventArgs e)
        {
           
        }
        private void txtEndDate_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
           
        }
        private void txtEndDate_LostFocus(object sender, RoutedEventArgs e)
        {
           
        }
        private void txtParkExtension_GotFocus(object sender, RoutedEventArgs e)
        {
            
        }
        private void txtParkExtension_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            try
            {
                #region Allowing only numbers and Tab

                if (!((e.Key >= Key.D0 && e.Key <= Key.D9) ||
                    (e.Key == Key.Decimal) ||
                    e.Key == Key.Tab ||
                     (e.Key >= Key.NumPad0 && e.Key <= Key.NumPad9)))
                {
                    e.Handled = true;
                }

                #endregion
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "txtParkExtension_KeyDown", "ctlCampaign.xaml.cs");               
            }
        }
        private void txtParkExtension_LostFocus(object sender, RoutedEventArgs e)
        {
           
        }
        private void TextBox_GotFocus(object sender, RoutedEventArgs e)
        {
           
        }
        private void TextBox_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
           
        }
        private void TextBox_LostFocus(object sender, RoutedEventArgs e)
        {
           
        }
        private void btnBrowsParkFile_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                System.Windows.Forms.OpenFileDialog d = new System.Windows.Forms.OpenFileDialog();
                //d.Filter = "Excel Files (*.xls)|*.xls|Text Files (*.txt)|*.txt|CommaSeparated Files (*.csv)|*.csv|All files (*.*)|*.*";
                if (d.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    txtParkFileName.Text = d.FileName.ToString();
                }
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "btnBrowsParkFile_Click", "ctlCampaign.xaml.cs");
            }
        }
        private void rdoPredictive_GotFocus(object sender, RoutedEventArgs e)
        {
           
        }
        private void rdoPredictive_Checked(object sender, RoutedEventArgs e)
        {
           
        }
        private void rdoManual_Checked(object sender, RoutedEventArgs e)
        {
           
        }
        private void rdoManual_GotFocus(object sender, RoutedEventArgs e)
        {
           
        }
        private void cmbScriptName_GotFocus(object sender, RoutedEventArgs e)
        {
            
        }
        private void cmbScriptName_LostFocus(object sender, RoutedEventArgs e)
        {
           
        }
        private void cmbScriptName_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
           
        }
        private void btnAddNew_Click(object sender, RoutedEventArgs e)
        {
           
        }
        private void lstAvailableLists_LostFocus(object sender, RoutedEventArgs e)
        {
           
        }
        private void lstAvailableLists_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
          
        }
        private void lstAvailableLists_GotFocus(object sender, RoutedEventArgs e)
        {
           
        }
        private void lstSelectedList_GotFocus(object sender, RoutedEventArgs e)
        {
           
        }
        private void lstSelectedList_LostFocus(object sender, RoutedEventArgs e)
        {
           
        }
        private void lstSelectedList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
           
        }
        //To select all Available callinglists
        private void btnSelectAllAL_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                selectall(lstAvailableLists);
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "btnSelectAllAL_Click", "ctlCampaign.xaml.cs"); 
            }
        }
        private void btnSelectNoneAL_MouseMove(object sender, System.Windows.Input.MouseEventArgs e)
        {
           
        }
        //To deselect available Callinglists
        private void btnSelectNoneAL_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                unselectall(lstAvailableLists);
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "btnSelectNoneAL_Click", "ctlCampaign.xaml.cs"); 
            }
        }
        private void btnSelectAllSL_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                selectall(lstSelectedList);
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "btnSelectAllSL_Click", "ctlCampaign.xaml.cs"); 
            }
        }
        private void btnSelectNoneSL_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                unselectall(lstSelectedList);
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "btnSelectNoneSL_Click", "ctlCampaign.xaml.cs");    
            }
        }
        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
           
        }
        private void btnSelectList_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                moveselected(lstAvailableLists, lstSelectedList);
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "btnSelectList_Click", "ctlCampaign.xaml.cs");    
            }
        }
        private void btnDeSelectList_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                moveselected(lstSelectedList, lstAvailableLists);
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "btnDeSelectList_Click", "ctlCampaign.xaml.cs");    
            }
        }
        private void btnCancle_Click(object sender, RoutedEventArgs e)
        {
         
        }
        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
           
        }
        private void lstAvailableTreatment_GotFocus(object sender, RoutedEventArgs e)
        {
           
        }
        private void lstAvailableTreatment_LostFocus(object sender, RoutedEventArgs e)
        {
          
        }
        private void lstAvailableTreatment_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
           
        }
        private void lstSelectedTreatments_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
          
        }
        private void lstSelectedTreatments_GotFocus(object sender, RoutedEventArgs e)
        {
          
        }
        private void lstSelectedTreatments_LostFocus(object sender, RoutedEventArgs e)
        {
           
        }
        private void btnCancelT_Click(object sender, RoutedEventArgs e)
        {
           
        }
        private void btnAddAT_Click(object sender, RoutedEventArgs e)
        {
         
        }
        private void btnSelectTreatment_Click(object sender, RoutedEventArgs e)
        {
          
        }
        private void btnDeselectTreatments_Click(object sender, RoutedEventArgs e)
        {
           
        }
        private void tbiAgentGroups_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
           
        }
        //To save new campaign record
        private void btnSaveOD_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                #region Validations

                if (txt_CampaignName.Text.Trim() == "")
                {
                    MessageBox.Show("Name of the Campaign Can't Be Blank ", "-> Please Enter Campaign Name", MessageBoxButton.OK, MessageBoxImage.Information);
                    tbiCampaignDetails.Focus();
                    txt_CampaignName.Text = txt_CampaignName.Text.Trim();
                    txt_CampaignName.Focus();
                    return;
                }
                if (txtCallerId.Text.Trim() == "")
                {
                    MessageBox.Show("Caller ID Can't Be Blank", "-> Please Enter Caller ID", MessageBoxButton.OK, MessageBoxImage.Information);
                    tbiCampaignDetails.Focus();
                    txtCallerId.Text = txtCallerId.Text.Trim();
                    txtCallerId.Focus();
                    return;
                }

                if (txtCallerId.Text.Trim() == "0" || Int64.Parse(txtCallerId.Text) < 1)
                {
                    MessageBox.Show("Caller ID Can't Be Zero", "-> Please Enter valid Caller ID", MessageBoxButton.OK, MessageBoxImage.Information);
                    tbiCampaignDetails.Focus();
                    txtCallerId.Text = txtCallerId.Text.Trim();
                    txtCallerId.Focus();
                    return;
                }

                if (dtpStartDate.Value == null)
                {
                    MessageBox.Show("Start Date Can't Be Blank", "-> Please Enter Start Date", MessageBoxButton.OK, MessageBoxImage.Information);
                    tbiCampaignDetails.Focus();
                    dtpStartDate.Focus();
                    return;
                }
                if (dtpStartDate.Value < System.DateTime.Now.Date && varState == 0)
                {
                    MessageBox.Show("Start Date Can't Be Less Then Current Date", "Please Enter Correct Date", MessageBoxButton.OK, MessageBoxImage.Information);
                    tbiCampaignDetails.Focus();
                    dtpStartDate.Focus();
                    return;
                }

                if (dtpEndDate.Value == null)
                {
                    MessageBox.Show("End Date Can't Be Blank", "-> Please Enter End Date", MessageBoxButton.OK, MessageBoxImage.Information);
                    tbiCampaignDetails.Focus();
                    dtpEndDate.Focus();
                    return;
                }

                if (dtpStartDate.Value > dtpEndDate.Value)
                {
                    MessageBox.Show("Start Date Can't Greater Than End Date", "-> Please Enter Date Properly", MessageBoxButton.OK, MessageBoxImage.Information);
                    tbiCampaignDetails.Focus();
                    dtpStartDate.Focus();
                    return;
                }

                if (cmbScriptName.Text.Trim() == "")
                {
                    MessageBox.Show("Script Can't Be Blank ", "-> Please Select Script", MessageBoxButton.OK, MessageBoxImage.Information);
                    tbiCampaignDetails.Focus();
                    cmbScriptName.Focus();
                    return;
                }

                if (cmbCRMName.Text.Trim() == "")
                {
                    MessageBox.Show("CRM Can't Be Blank ", "-> Please Select CRM", MessageBoxButton.OK, MessageBoxImage.Information);
                    tbiCampaignDetails.Focus();
                    cmbCRMName.Focus();
                    return;
                }


                #endregion

                ClsCampaign objCamp = new ClsCampaign();
                if (varState == 0)
                {
                    objCamp.ID = -1;
                }
                else
                {
                    objCamp.ID = varID;
                    ClsCampaign.RemoveJoin(varID);
                }
                objCamp.Name = txt_CampaignName.Text.Trim();
                objCamp.Description = txtDescription.Text.Trim();
                //objCamp.NoOfChannels = int.Parse(txtNoOfChannels.Text.Trim());
                objCamp.NoOfChannels = 0;
                objCamp.Prefix = txtCampaignPrefix.Text.Trim();
                objCamp.CallerID = Int64.Parse(txtCallerId.Text.Trim());
                if (chkIsActive.IsChecked == true)
                    objCamp.IsActive = true;
                else
                    objCamp.IsActive = false;

                if (rdoManual.IsChecked == true)
                    objCamp.DType = "Manual";
                else
                    objCamp.DType = "Predictive";

                objCamp.AssignTo = txtAssignedTo.Text.Trim();
                objCamp.ScriptID = int.Parse(((ListBoxItem)cmbScriptName.SelectedItem).Tag.ToString());
                objCamp.CRMID = int.Parse(((ListBoxItem)cmbCRMName.SelectedItem).Tag.ToString());
                if (txtParkExtension.Text.Trim().Length > 0)
                {
                    objCamp.ParkExtension = int.Parse(txtParkExtension.Text.Trim());
                }
                else
                {
                    objCamp.ParkExtension = 0;
                }
                objCamp.ParkFileName = txtParkFileName.Text.Trim();
                objCamp.StartDate = Convert.ToDateTime(dtpStartDate.Value.ToString());
                objCamp.EndDate = Convert.ToDateTime(dtpEndDate.Value.ToString());

                if (cmbCallingTimes.Text.Trim() != "")
                    objCamp.CallingTime = int.Parse(((ListBoxItem)cmbCallingTimes.SelectedItem).Tag.ToString());
                else
                    objCamp.CallingTime = 0;

                //objCamp.CallingTime = GetInt(row, "CallingTime");
                if (cmbRecordingFileFormat.Text.Trim() != "")
                    objCamp.RecordingFileFormat = cmbRecordingFileFormat.Text;

                objCamp.ByUserID = VMuktiAPI.VMuktiInfo.CurrentPeer.ID;

                //objCamp.ModifiedDate = GetDateTime(row, "ModifiedDate");
                //Calling save function from ClsCampaign
                Int64 SaveId = objCamp.Save();

                if (SaveId == 0)
                {
                    MessageBox.Show("Duplicate Entries For A Campaign Is Not Allowed !!", "-> Campaign", MessageBoxButton.OK, MessageBoxImage.Information);
                }

                else
                {
                    #region SaveJoinEntries

                    // Saving The Calling Lists With Priorities//

                    for (int i = 0; i < lstSelectedList.Items.Count; i++)
                    {
                        ClsCallingList objList = new ClsCallingList();
                        objList.CampID = SaveId;
                        objList.Priority = i + 1;
                        objList.ID = int.Parse(((ListBoxItem)lstSelectedList.Items[i]).Tag.ToString());
                        objList.Save();

                    }

                    // Savin DNC Lists Setting Priorities -1 //

                    for (int i = 0; i < lstSelectedDList.Items.Count; i++)
                    {
                        ClsCallingList objList = new ClsCallingList();
                        objList.CampID = SaveId;
                        objList.Priority = -1;
                        objList.ID = int.Parse(((ListBoxItem)lstSelectedDList.Items[i]).Tag.ToString());
                        objList.Save();

                    }

                    // Saving The Groups //

                    for (int i = 0; i < lstSelectedGroups.Items.Count; i++)
                    {
                        ClsGroup objGroup = new ClsGroup();
                        objGroup.CampID = SaveId;
                        objGroup.ID = int.Parse(((ListBoxItem)lstSelectedGroups.Items[i]).Tag.ToString());
                        objGroup.Save();
                    }

                    // Saving The Disposition //

                    ClsDispositionList objDisp = new ClsDispositionList();
                    objDisp.CampID = SaveId;
                    if (cmbDispositionList.Text.Trim() != "")
                    {
                        objDisp.ID = int.Parse(((ListBoxItem)cmbDispositionList.SelectedItem).Tag.ToString());
                        objDisp.Save();
                    }
                    // Saving The Treatments //

                    for (int i = 0; i < lstSelectedTreatments.Items.Count; i++)
                    {
                        ClsTreatment objTreat = new ClsTreatment();
                        objTreat.CampID = SaveId;
                        objTreat.ID = int.Parse(((ListBoxItem)lstSelectedTreatments.Items[i]).Tag.ToString());
                        objTreat.Save();
                    }
                    #endregion

                    System.Windows.MessageBox.Show("Record Saved Successfully!!");
                    FncClearAll();
                    FncFillAll();
                    funSetGrid();
                }
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "btnSaveOD_Click", "ctlCampaign.xaml.cs");    
            }
        }
        //To cancel editing or deleting 
        private void btnCancelOD_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                FncClearAll();
                FncFillAll();
                funSetGrid();
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "btnCancelOD_Click", "ctlCampaign.xaml.cs");

            }
        }
        private void cmbAssignTo_LostFocus(object sender, RoutedEventArgs e)
        {
          
        }
        private void cmbAssignTo_GotFocus(object sender, RoutedEventArgs e)
        {
           
        }
        private void cmbAssignTo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
         
        }
        private void cmbFrom_GotFocus(object sender, RoutedEventArgs e)
        {
          
        }
        private void cmbFrom_LostFocus(object sender, RoutedEventArgs e)
        {
           
        }
        private void cmbFrom_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
        private void cmbTo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
        }
        private void cmbTo_LostFocus(object sender, RoutedEventArgs e)
        {
        }
        private void cmbTo_GotFocus(object sender, RoutedEventArgs e)
        {
        }
        private void cmbRecordingFileFormat_GotFocus(object sender, RoutedEventArgs e)
        {
        }
        private void cmbRecordingFileFormat_LostFocus(object sender, RoutedEventArgs e)
        {
        }
        private void cmbRecordingFileFormat_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
        }
        private void txtQualificationGoal_GotFocus(object sender, RoutedEventArgs e)
        {
        }
        private void txtQualificationGoal_LostFocus(object sender, RoutedEventArgs e)
        {
        }
        private void txtQualificationGoal_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
        }
        private void txtQualificationGoalDate_GotFocus(object sender, RoutedEventArgs e)
        {
        }
        private void txtQualificationGoalDate_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
        }
        private void txtQualificationGoalDate_LostFocus(object sender, RoutedEventArgs e)
        {
        }
        private void txtQualifyPerDay_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
        }
        private void txtQualifyPerDay_LostFocus(object sender, RoutedEventArgs e)
        {
        }
        private void txtQualifyPerDay_GotFocus(object sender, RoutedEventArgs e)
        {
        }
        private void txtSaleLeads_GotFocus(object sender, RoutedEventArgs e)
        {
        }
        private void txtSaleLeads_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            try
            {
                #region Allowing only numbers and Tab

                if (!((e.Key >= Key.D0 && e.Key <= Key.D9) ||
                    (e.Key == Key.Decimal) ||
                    e.Key == Key.Tab ||
                     (e.Key >= Key.NumPad0 && e.Key <= Key.NumPad9)))
                {
                    e.Handled = true;
                }

                #endregion
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "txtSaleLeads_KeyDown", "ctlCampaign.xaml.cs");
            }
        }
        private void txtSaleLeads_LostFocus(object sender, RoutedEventArgs e)
        {
        }
        private void txtNonSaleLeads_GotFocus(object sender, RoutedEventArgs e)
        {
        }
        private void txtNonSaleLeads_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            try
            {
                #region Allowing only numbers and Tab

                if (!((e.Key >= Key.D0 && e.Key <= Key.D9) ||
                    (e.Key == Key.Decimal) ||
                    e.Key == Key.Tab ||
                     (e.Key >= Key.NumPad0 && e.Key <= Key.NumPad9)))
                {
                    e.Handled = true;
                }

                #endregion
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "txtNonSaleLeads_KeyDown", "ctlCampaign.xaml.cs");
            }
        }
        private void txtNonSaleLeads_LostFocus(object sender, RoutedEventArgs e)
        {
        }
    }
}