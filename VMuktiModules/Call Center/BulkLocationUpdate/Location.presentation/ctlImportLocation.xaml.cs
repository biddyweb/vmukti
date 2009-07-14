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
using System.Data.OleDb;
using System.Data;
using System.Data.Common;
using Location.Business;
using System.Collections;
using System.ComponentModel;

namespace Location.presentation
{
    /// <summary>
    /// Interaction logic for UserControl1.xaml
    /// </summary>
    /// 
    public enum ModulePermissions
    {
        Add = 0,
        Edit = 1,
        Delete = 2,
        View = 3
    }
    public partial class ctlImportLocation : UserControl
    {
        ModulePermissions[] _MyPermissions;
        string strTimeZone = "";
        IList strCountry = null;
        IList strState = null;
        IList strAreaCode = null;
        IList strZipCode = null;

        ClsLocationBusiness clsBusiness = new ClsLocationBusiness();
        DataSet dsCountry = null;
        DataSet dsState = null;
        DataSet dsAreacode = null;
        DataSet dsZipCode = null;

        public ctlImportLocation(ModulePermissions[] MyPermissions)
        {
            try
            {

                InitializeComponent();
                _MyPermissions = MyPermissions;
                DataSet dsTimeZone = clsBusiness.fncGetTimeZone();

                for (int i = 0; i < dsTimeZone.Tables[0].Rows.Count; i++)
                { cmbTimeZone.Items.Add(dsTimeZone.Tables[0].Rows[i][0]); }
                List<string> xList = new List<string>();
                for (int i = 0; i < cmbTimeZone.Items.Count; i++)
                { xList.Add(cmbTimeZone.Items[i].ToString()); }
                xList.Sort();
                cmbTimeZone.Items.Clear();
                for (int j = 0; j < xList.Count; j++)
                { cmbTimeZone.Items.Add(xList[j]); }

                this.Loaded += new RoutedEventHandler(ctlImportLocation_Loaded);                

            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "ctlImportLocation", "ctlImportLocation.xaml.cs");
            }
        }

        

        void ctlImportLocation_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                if (this.Parent != null)
                {
                    ((Grid)(this.Parent)).SizeChanged += new SizeChangedEventHandler(ctlImportLocation_SizeChanged);
                    this.Width = ((Grid)(this.Parent)).ActualWidth;
                }
            }
            catch (Exception exp)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(exp, "ctlImportLocation_Loaded", "ctlImportLocation.xaml.cs");

            }
        }

        void ctlImportLocation_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            try
            {
                if (e.NewSize.Height > 0)
                {
                    this.Width = ((Grid)(this.Parent)).ActualWidth;
                }
            }
            catch (Exception exp)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(exp, "ctlImportLocation_SizeChanged", "ctlImportLocation.xaml.cs");

            }
        }

        #region TimeZone Functions
        private void btnAddnewTimeZone_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                TimeZoneStackPanel.Visibility = Visibility.Visible;
                TimeZoneStScroll.Visibility = Visibility.Visible;
                cmbTimeZone.IsEnabled = false;
                btnAddnewTimeZone.IsEnabled = false;
                GrpTimeZone.Height = 120;
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "btnAddnewTimeZone_Click()", "ctlImportLocation.xaml.cs");
            }
        }

        private void btnTimezoneProced_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                GrpTimeZone.IsEnabled = false;
                GrpCountry.IsEnabled = true;
                strTimeZone = cmbTimeZone.SelectedItem.ToString();
                GrpTimeZone.Height = 50;
                cmbCountry.Focus();


                cmbCountry.Items.Clear();
                btnAddNewCountry.IsEnabled = false;
                dsCountry = clsBusiness.fncGetCountry(true, strTimeZone);
                if (dsCountry.Tables.Count > 0 && dsCountry.Tables[0].Rows.Count > 0)
                {
                    for (int j = 0; j < dsCountry.Tables[0].Rows.Count; j++)
                    { cmbCountry.Items.Add(dsCountry.Tables[0].Rows[j][0]); }
                    List<string> xCountryList = new List<string>();
                    for (int i = 0; i < cmbCountry.Items.Count; i++)
                    { xCountryList.Add(cmbCountry.Items[i].ToString()); }
                    xCountryList.Sort();
                    cmbCountry.Items.Clear();
                    for (int j = 0; j < xCountryList.Count; j++)
                    { cmbCountry.Items.Add(xCountryList[j]); }
                }
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "btnTimezoneProced_Click()", "ctlImportLocation.xaml.cs");
            }
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (fncCompare("TimeZone", txtTimeZone.Text))
                {
                    MessageBox.Show("already added");
                }
                else if (txtTimeZone.Text == "")
                {
                    MessageBox.Show("Blank Space not allow");
                }
                else
                {
                    clsBusiness.fncInsertTimeZone(txtTimeZone.Text);
                    cmbTimeZone.Items.Add(txtTimeZone.Text);
                    txtTimeZone.Text = "";

                    List<string> xList = new List<string>();
                    for (int i = 0; i < cmbTimeZone.Items.Count; i++)
                    { xList.Add(cmbTimeZone.Items[i].ToString()); }
                    xList.Sort();
                    cmbTimeZone.Items.Clear();
                    for (int j = 0; j < xList.Count; j++)
                    { cmbTimeZone.Items.Add(xList[j]); }

                    TimeZoneStackPanel.Visibility = Visibility.Collapsed;
                    TimeZoneStScroll.Visibility = Visibility.Collapsed;
                    cmbTimeZone.IsEnabled = true;
                    btnAddnewTimeZone.IsEnabled = true;
                }
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "btnAdd_Click()", "ctlImportLocation.xaml.cs");
            }
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                TimeZoneStackPanel.Visibility = Visibility.Collapsed;
                TimeZoneStScroll.Visibility = Visibility.Collapsed;
                cmbTimeZone.IsEnabled = true;
                btnAddnewTimeZone.IsEnabled = true;
                GrpTimeZone.Height = 50;
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "btnCancel_Click()", "ctlImportLocation.xaml.cs");
            }
        }

        private void cmbTimeZone_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                btnTimezoneProced.IsEnabled = true;
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "cmbTimeZone_SelectionChanged()", "ctlImportLocation.xaml.cs");
            }
        }
        #endregion

        #region Country Functions
        private void btnAddNewCountry_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                GrdCountry.Visibility = Visibility.Visible;
                cmbCountry.IsEnabled = false;
                btnAddNewCountry.IsEnabled = false;
                GrpCountry.Height = 170;
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "btnAddNewCountry_Click()", "ctlImportLocation.xaml.cs");
            }
        }

        private void btnCountryProced_Click(object sender, RoutedEventArgs e)
        {
            try
            {
            GrpCountry.IsEnabled = false;
            GrpState.IsEnabled = true;
            GrpCountry.Height = 100;
            strCountry = new IList[cmbCountry.SelectedItems.Count];
            strCountry = cmbCountry.SelectedItems;
            cmbState.Focus();

            
            cmbState.Items.Clear();
            btnAddNewState.IsEnabled = false;
            dsState = clsBusiness.fncGetState(true, strCountry[0].ToString());
            if (dsState.Tables.Count > 0 && dsState.Tables[0].Rows.Count > 0)
            {
                for (int j = 0; j < dsState.Tables[0].Rows.Count; j++)
                { cmbState.Items.Add(dsState.Tables[0].Rows[j][0]); }
                List<string> xStateList = new List<string>();
                for (int i = 0; i < cmbState.Items.Count; i++)
                { xStateList.Add(cmbState.Items[i].ToString()); }
                xStateList.Sort();
                cmbState.Items.Clear();
                for (int j = 0; j < xStateList.Count; j++)
                { cmbState.Items.Add(xStateList[j]); }
            }
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "btnCountryProced_Click()", "ctlImportLocation.xaml.cs");
            }
        }

        private void btnAddFeildCountry_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (txtCountryName.Text == null || txtCountryName.Text == "")
                {
                    MessageBox.Show("Country name can not be blank");
                }

                else if (fncCompare("Country", txtCountryName.Text))
                {
                    MessageBox.Show(txtCountryName.Text + " Is already present");
                }
                else if (txtCountryCode.Text == null || txtCountryCode.Text == "")
                {
                    MessageBox.Show("Country Code can not be blank");
                }
                else
                {
                    clsBusiness.fncInsertCountry(txtCountryName.Text, txtCountryCode.Text, txtCountryDescription.Text);
                    cmbCountry.Items.Add(txtCountryName.Text);
                    txtCountryName.Text = "";
                    txtCountryCode.Text = "";
                    txtCountryDescription.Text = "";

                    List<string> xCountryList = new List<string>();
                    for (int i = 0; i < cmbCountry.Items.Count; i++)
                    { xCountryList.Add(cmbCountry.Items[i].ToString()); }
                    xCountryList.Sort();
                    cmbCountry.Items.Clear();
                    for (int j = 0; j < xCountryList.Count; j++)
                    { cmbCountry.Items.Add(xCountryList[j]); }
                }
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "btnAddFeildCountry_Click()", "ctlImportLocation.xaml.cs");
            }

        }

        private void btnDoneCountry_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                cmbCountry.IsEnabled = true;
                btnAddNewCountry.IsEnabled = true;
                GrdCountry.Visibility = Visibility.Collapsed;
                GrpCountry.Height = 100;
                if ((txtCountryName.Text != "") && (txtCountryCode.Text != ""))
                {
                    btnAddFeildCountry_Click(null, null);
                }
                txtCountryName.Text = "";
                txtCountryCode.Text = "";
                txtCountryDescription.Text = "";
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "btnDoneCountry_Click()", "ctlImportLocation.xaml.cs");
            }
        }

        private void btnCancelCountry_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                GrdCountry.Visibility = Visibility.Collapsed;
                cmbCountry.IsEnabled = true;
                btnAddNewCountry.IsEnabled = true;
                GrpCountry.Height = 100;
                txtCountryName.Text = "";
                txtCountryCode.Text = "";
                txtCountryDescription.Text = "";
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "btnCancelCountry_Click()", "ctlImportLocation.xaml.cs");
            }
        }

        private void rbCountryShowAll_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                dsCountry.Clear();
                cmbCountry.Items.Clear();
                btnAddNewCountry.IsEnabled = true;
                GrdCountry.IsEnabled = true;
                dsCountry = clsBusiness.fncGetCountry(false, "");
                for (int j = 0; j < dsCountry.Tables[0].Rows.Count; j++)
                { cmbCountry.Items.Add(dsCountry.Tables[0].Rows[j][0]); }
                List<string> xCountryList = new List<string>();
                for (int i = 0; i < cmbCountry.Items.Count; i++)
                { xCountryList.Add(cmbCountry.Items[i].ToString()); }
                xCountryList.Sort();
                cmbCountry.Items.Clear();
                for (int j = 0; j < xCountryList.Count; j++)
                { cmbCountry.Items.Add(xCountryList[j]); }
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "rbCountryShowAll_Click()", "ctlImportLocation.xaml.cs");
            }
        }

        private void rbCountryShowRelated_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                dsCountry.Clear();
                cmbCountry.Items.Clear();
                btnAddNewCountry.IsEnabled = false;
                GrdCountry.IsEnabled = false;
                dsCountry = clsBusiness.fncGetCountry(true, strTimeZone);
                if (dsCountry.Tables.Count > 0 && dsCountry.Tables[0].Rows.Count > 0)
                {
                    for (int j = 0; j < dsCountry.Tables[0].Rows.Count; j++)
                    { cmbCountry.Items.Add(dsCountry.Tables[0].Rows[j][0]); }
                    List<string> xCountryList = new List<string>();
                    for (int i = 0; i < cmbCountry.Items.Count; i++)
                    { xCountryList.Add(cmbCountry.Items[i].ToString()); }
                    xCountryList.Sort();
                    cmbCountry.Items.Clear();
                    for (int j = 0; j < xCountryList.Count; j++)
                    { cmbCountry.Items.Add(xCountryList[j]); }
                }
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "rbCountryShowRelated_Click()", "ctlImportLocation.xaml.cs");
            }
        }

        private void cmbCountry_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (cmbCountry.SelectedItems.Count == 0)
                {
                    btnCountryProced.IsEnabled = false;
                }
                else
                {
                    btnCountryProced.IsEnabled = true;
                }
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "cmbCountry_SelectionChanged()", "ctlImportLocation.xaml.cs");
            }
        }
        #endregion

        #region State Functions
        private void btnAddNewState_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                GrdState.Visibility = Visibility.Visible;
                cmbState.IsEnabled = false;
                btnAddNewState.IsEnabled = false;
                GrpState.Height = 170;
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "btnAddNewState_Click()", "ctlImportLocation.xaml.cs");
            }
        }

        private void btnStateProced_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                GrpState.IsEnabled = false;
                GrpAreacode.IsEnabled = true;
                GrpState.Height = 100;
                strState = new IList[cmbCountry.SelectedItems.Count];
                strState = cmbState.SelectedItems;
                cmbAreaCode.Focus();


                cmbAreaCode.Items.Clear();
                btnAddNewAreaCode.IsEnabled = false;
                dsAreacode = clsBusiness.fncGetAreaCode("State", strState[0].ToString());
                if (dsAreacode.Tables.Count > 0 && dsAreacode.Tables[0].Rows.Count > 0)
                {
                    for (int j = 0; j < dsAreacode.Tables[0].Rows.Count; j++)
                    { cmbAreaCode.Items.Add(dsAreacode.Tables[0].Rows[j][0]); }
                    List<string> xAreaCodeList = new List<string>();
                    for (int i = 0; i < cmbAreaCode.Items.Count; i++)
                    { xAreaCodeList.Add(cmbAreaCode.Items[i].ToString()); }
                    xAreaCodeList.Sort();
                    cmbAreaCode.Items.Clear();
                    for (int j = 0; j < xAreaCodeList.Count; j++)
                    { cmbAreaCode.Items.Add(xAreaCodeList[j]); }
                }


                cmbZipCode.Items.Clear();
                dsZipCode = clsBusiness.fncGetZipCode(strState[0].ToString());
                if (dsZipCode.Tables.Count > 0 && dsZipCode.Tables[0].Rows.Count > 0)
                {
                    for (int j = 0; j < dsZipCode.Tables[0].Rows.Count; j++)
                    { cmbZipCode.Items.Add(dsZipCode.Tables[0].Rows[j][0]); }
                    List<string> xZipCodeList = new List<string>();
                    for (int i = 0; i < cmbZipCode.Items.Count; i++)
                    { xZipCodeList.Add(cmbZipCode.Items[i].ToString()); }
                    xZipCodeList.Sort();
                    cmbZipCode.Items.Clear();
                    for (int j = 0; j < xZipCodeList.Count; j++)
                    { cmbZipCode.Items.Add(xZipCodeList[j]); }
                }
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "btnStateProced_Click()", "ctlImportLocation.xaml.cs");
            }
        }

        private void btnAddState_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (txtStateName.Text == null || txtStateName.Text == "")
                {
                    MessageBox.Show("State name can not be blank");
                }

                else if (fncCompare("State", txtStateName.Text))
                {
                    MessageBox.Show(txtStateName.Text + " Is already present");
                }
                else if (txtStateAbbreviation.Text == null || txtStateAbbreviation.Text == "")
                {
                    MessageBox.Show("State Abbreviation can not be blank");
                }
                else
                {
                    clsBusiness.fncInsertState(txtStateName.Text, txtStateAbbreviation.Text);
                    cmbState.Items.Add(txtStateName.Text);
                    txtStateName.Text = "";
                    txtStateAbbreviation.Text = "";

                    List<string> xStateList = new List<string>();
                    for (int i = 0; i < cmbState.Items.Count; i++)
                    { xStateList.Add(cmbState.Items[i].ToString()); }
                    xStateList.Sort();
                    cmbState.Items.Clear();
                    for (int j = 0; j < xStateList.Count; j++)
                    { cmbState.Items.Add(xStateList[j]); }
                }
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "btnAddState_Click()", "ctlImportLocation.xaml.cs");
            }
        }

        private void btnCancelState_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                GrdState.Visibility = Visibility.Collapsed;
                cmbState.IsEnabled = true;
                btnAddNewState.IsEnabled = true;
                GrpState.Height = 100;
                txtStateName.Text = "";
                txtStateAbbreviation.Text = "";
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "btnCancelState_Click()", "ctlImportLocation.xaml.cs");
            }
        }

        private void btnDoneState_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                cmbState.IsEnabled = true;
                btnAddNewState.IsEnabled = true;
                GrdState.Visibility = Visibility.Collapsed;
                GrpState.Height = 100;
                if ((txtStateName.Text != "") && (txtStateAbbreviation.Text != ""))
                {
                    btnAddState_Click(null, null);
                }
                txtStateName.Text = "";
                txtStateAbbreviation.Text = "";
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "btnDoneState_Click()", "ctlImportLocation.xaml.cs");
            }
        }

        private void rbStateShowAll_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                dsState.Clear();
                cmbState.Items.Clear();
                btnAddNewState.IsEnabled = true;
                GrdState.IsEnabled = true;
                dsState = clsBusiness.fncGetState(false, "");
                for (int j = 0; j < dsState.Tables[0].Rows.Count; j++)
                { cmbState.Items.Add(dsState.Tables[0].Rows[j][0]); }
                List<string> xStateList = new List<string>();
                for (int i = 0; i < cmbState.Items.Count; i++)
                { xStateList.Add(cmbState.Items[i].ToString()); }
                xStateList.Sort();
                cmbState.Items.Clear();
                for (int j = 0; j < xStateList.Count; j++)
                { cmbState.Items.Add(xStateList[j]); }
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "rbStateShowAll_Click()", "ctlImportLocation.xaml.cs");
            }
        }

        private void rbStateShowRelated_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                dsState.Clear();
                cmbState.Items.Clear();
                btnAddNewState.IsEnabled = false;
                GrdState.IsEnabled = false;
                dsState = clsBusiness.fncGetState(true, strCountry[0].ToString());
                if (dsState.Tables.Count > 0 && dsState.Tables[0].Rows.Count > 0)
                {
                    for (int j = 0; j < dsState.Tables[0].Rows.Count; j++)
                    { cmbState.Items.Add(dsState.Tables[0].Rows[j][0]); }
                    List<string> xStateList = new List<string>();
                    for (int i = 0; i < cmbState.Items.Count; i++)
                    { xStateList.Add(cmbState.Items[i].ToString()); }
                    xStateList.Sort();
                    cmbState.Items.Clear();
                    for (int j = 0; j < xStateList.Count; j++)
                    { cmbState.Items.Add(xStateList[j]); }
                }
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "rbStateShowRelated_Click()", "ctlImportLocation.xaml.cs");
            }
        }

        private void cmbState_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (cmbState.SelectedItems.Count == 0)
                {
                    btnStateProced.IsEnabled = false;
                }
                else
                {
                    btnStateProced.IsEnabled = true;
                }
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "cmbState_SelectionChanged()", "ctlImportLocation.xaml.cs");
            }
        }
        #endregion

        #region AreaCode Functions
        private void btnAddNewAreaCode_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                GrdAreaCode.Visibility = Visibility.Visible;
                cmbAreaCode.IsEnabled = false;
                btnAddNewAreaCode.IsEnabled = false;
                GrpAreacode.Height = 170;
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "btnAddNewAreaCode_Click()", "ctlImportLocation.xaml.cs");
            }
        }

        private void btnAreaCodeProced_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                GrpAreacode.IsEnabled = false;
                GrpZipCode.IsEnabled = true;
                GrpAreacode.Height = 100;
                strAreaCode = new IList[cmbAreaCode.SelectedItems.Count];
                strAreaCode = cmbAreaCode.SelectedItems;
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "btnAreaCodeProced_Click()", "ctlImportLocation.xaml.cs");
            }
        }

        private void btnAddAreaCode_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (txtAreacode.Text == null || txtAreacode.Text == "")
                {
                    MessageBox.Show("AreaCode can not be blank");
                }

                else if (fncCompare("AreaCode", txtAreacode.Text))
                {
                    MessageBox.Show(txtAreacode.Text + " Is already present");
                }
                else if (txtAreacode.Text.Length > 3)
                {
                    MessageBox.Show("AreaCode can not be greater than 3 digit");
                }
                else
                {
                    clsBusiness.fncInsertAreaCode(txtAreacode.Text, strTimeZone);
                    cmbAreaCode.Items.Add(txtAreacode.Text);
                    txtAreacode.Text = "";

                    List<string> xAreaCodeList = new List<string>();
                    for (int i = 0; i < cmbAreaCode.Items.Count; i++)
                    { xAreaCodeList.Add(cmbAreaCode.Items[i].ToString()); }
                    xAreaCodeList.Sort();
                    cmbAreaCode.Items.Clear();
                    for (int j = 0; j < xAreaCodeList.Count; j++)
                    { cmbAreaCode.Items.Add(xAreaCodeList[j]); }
                }
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "btnAddAreaCode_Click()", "ctlImportLocation.xaml.cs");
            }
        }

        private void btnCancelAreacode_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                GrdAreaCode.Visibility = Visibility.Collapsed;
                cmbAreaCode.IsEnabled = true;
                btnAddNewAreaCode.IsEnabled = true;
                GrpAreacode.Height = 100;
                txtAreacode.Text = "";
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "btnCancelAreacode_Click()", "ctlImportLocation.xaml.cs");
            }
        }

        private void btnDoneAreaCode_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                cmbAreaCode.IsEnabled = true;
                btnAddNewAreaCode.IsEnabled = true;
                GrdAreaCode.Visibility = Visibility.Collapsed;
                GrpAreacode.Height = 100;
                if (txtAreacode.Text != "")
                {
                    btnAddAreaCode_Click(null, null);
                }
                txtAreacode.Text = "";
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "btnDoneAreaCode_Click()", "ctlImportLocation.xaml.cs");
            }
        }

        private void rbAreaCodeShowRelatedToTimeZone_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                dsAreacode.Clear();
                cmbAreaCode.Items.Clear();
                btnAddNewAreaCode.IsEnabled = true;
                GrdAreaCode.IsEnabled = true;
                dsAreacode = clsBusiness.fncGetAreaCode("TimeZone", strTimeZone);
                if (dsAreacode.Tables.Count > 0 && dsAreacode.Tables[0].Rows.Count > 0)
                {
                    for (int j = 0; j < dsAreacode.Tables[0].Rows.Count; j++)
                    { cmbAreaCode.Items.Add(dsAreacode.Tables[0].Rows[j][0]); }
                    List<string> xAreaCodeList = new List<string>();
                    for (int i = 0; i < cmbAreaCode.Items.Count; i++)
                    { xAreaCodeList.Add(cmbAreaCode.Items[i].ToString()); }
                    xAreaCodeList.Sort();
                    cmbAreaCode.Items.Clear();
                    for (int j = 0; j < xAreaCodeList.Count; j++)
                    { cmbAreaCode.Items.Add(xAreaCodeList[j]); }
                }
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "rbAreaCodeShowRelatedToTimeZone_Click()", "ctlImportLocation.xaml.cs");
            }
        }
        private void rbAreaCodeShowRelatedToState_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                dsAreacode.Clear();
                cmbAreaCode.Items.Clear();
                btnAddNewAreaCode.IsEnabled = false;
                GrdAreaCode.IsEnabled = false;
                dsAreacode = clsBusiness.fncGetAreaCode("State", strState[0].ToString());
                if (dsAreacode.Tables.Count > 0 && dsAreacode.Tables[0].Rows.Count > 0)
                {
                    for (int j = 0; j < dsAreacode.Tables[0].Rows.Count; j++)
                    { cmbAreaCode.Items.Add(dsAreacode.Tables[0].Rows[j][0]); }
                    List<string> xAreaCodeList = new List<string>();
                    for (int i = 0; i < cmbAreaCode.Items.Count; i++)
                    { xAreaCodeList.Add(cmbAreaCode.Items[i].ToString()); }
                    xAreaCodeList.Sort();
                    cmbAreaCode.Items.Clear();
                    for (int j = 0; j < xAreaCodeList.Count; j++)
                    { cmbAreaCode.Items.Add(xAreaCodeList[j]); }
                }
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "rbAreaCodeShowRelatedToState_Click()", "ctlImportLocation.xaml.cs");
            }
        }

        private void cmbAreaCode_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (cmbAreaCode.SelectedItems.Count == 0)
                {
                    btnAreaCodeProced.IsEnabled = false;
                }
                else
                {
                    btnAreaCodeProced.IsEnabled = true;
                }

            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "cmbAreaCode_SelectionChanged()", "ctlImportLocation.xaml.cs");
            }
        }
        #endregion

        #region ZipCode Functions
        private void btnAddNewZipCode_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                GrdZipCode.Visibility = Visibility.Visible;
                cmbZipCode.IsEnabled = false;
                btnAddNewZipCode.IsEnabled = false;
                GrpZipCode.Height = 170;
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "btnAddNewZipCode_Click()", "ctlImportLocation.xaml.cs");
            }
        }

        private void btnZipCodeProced_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                GrpZipCode.IsEnabled = false;
                GrpFinal.IsEnabled = true;
                GrpZipCode.Height = 100;
                strZipCode = new IList[cmbZipCode.SelectedItems.Count];
                strZipCode = cmbZipCode.SelectedItems;
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "btnZipCodeProced_Click()", "ctlImportLocation.xaml.cs");
            }
        }



        private void btnAddZipCode_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (txtZipcode.Text == null || txtZipcode.Text == "")
                {
                    MessageBox.Show("ZipCode can not be blank");
                }

                else if (fncCompare("ZipCode", txtZipcode.Text))
                {
                    MessageBox.Show(txtZipcode.Text + " Is already present");
                }

                else if (clsBusiness.fncCheckZipcode(txtZipcode.Text))
                {
                    MessageBox.Show("Already Present");
                }
                else
                {
                    clsBusiness.fncInsertZipCode(txtZipcode.Text, strState[0].ToString());
                    cmbZipCode.Items.Add(txtZipcode.Text);
                    txtZipcode.Text = "";

                    List<string> xZipCodeList = new List<string>();
                    for (int i = 0; i < cmbZipCode.Items.Count; i++)
                    { xZipCodeList.Add(cmbZipCode.Items[i].ToString()); }
                    xZipCodeList.Sort();
                    cmbZipCode.Items.Clear();
                    for (int j = 0; j < xZipCodeList.Count; j++)
                    { cmbZipCode.Items.Add(xZipCodeList[j]); }
                }
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "btnAddZipCode_Click()", "ctlImportLocation.xaml.cs");
            }
        }

        private void btnDoneZipCode_Click(object sender, RoutedEventArgs e)
        {
            try{
            cmbZipCode.IsEnabled = true;
            btnAddNewZipCode.IsEnabled = true;
            GrdZipCode.Visibility = Visibility.Collapsed;
            GrpZipCode.Height = 100;
            if (txtZipcode.Text != "")
            {
                btnAddZipCode_Click(null, null);
            }
            txtZipcode.Text = "";
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "btnDoneZipCode_Click()", "ctlImportLocation.xaml.cs");
            }
        }

        private void btnCancelZipcode_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                GrdZipCode.Visibility = Visibility.Collapsed;
                cmbZipCode.IsEnabled = true;
                btnAddNewZipCode.IsEnabled = true;
                GrpZipCode.Height = 100;
                txtZipcode.Text = "";
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "btnCancelZipcode_Click()", "ctlImportLocation.xaml.cs");
            }

        }

        private void cmbZipCode_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {

                if (cmbZipCode.SelectedItems.Count == 0)
                {
                    btnZipCodeProced.IsEnabled = false;
                }
                else
                {
                    btnZipCodeProced.IsEnabled = true;
                }
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "cmbZipCode_SelectionChanged()", "ctlImportLocation.xaml.cs");
            }
        }
        #endregion


        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Int64 intTimeZoneId = Int64.Parse(clsBusiness.fncGetTimeZoneId(strTimeZone).ToString());
                for (int a = 0; a < strCountry.Count; a++)
                {
                    //string strCountryName = strCountry[a].ToString();
                    Int64 intCountryId = Int64.Parse(clsBusiness.fncGetCountryId(strCountry[a].ToString()));
                    for (int b = 0; b < strState.Count; b++)
                    {
                        //string strStateName = strState[b].ToString();
                        Int64 intStateId = Int64.Parse(clsBusiness.fncGetStateId(strState[b].ToString()));
                        for (int c = 0; c < strAreaCode.Count; c++)
                        {
                            //string strAreaCodeName = strAreaCode[c].ToString();
                            Int64 intAreaCodeId = Int64.Parse(clsBusiness.fncGetAreaCodeId(strAreaCode[c].ToString()));
                            for (int d = 0; d < strZipCode.Count; d++)
                            {
                                //string strZipCodeName = strZipCode[d].ToString();
                                Int64 intZipCodeId = Int64.Parse(clsBusiness.fncGetZipId(strZipCode[d].ToString()));
                                clsBusiness.fncInsertLocation(intTimeZoneId, intCountryId, intStateId, intAreaCodeId, intZipCodeId);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "btnSave_Click()", "ctlImportLocation.xaml.cs");
            }
        }

        private void btnCanel_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnSaveContinue_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                btnSave_Click(null, null);
                GrpFinal.IsEnabled = false;
                GrpTimeZone.IsEnabled = true;
                dsCountry.Clear();
                dsState.Clear();
                dsAreacode.Clear();
                dsZipCode.Clear();
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "btnSaveContinue_Click()", "ctlImportLocation.xaml.cs");
            }
        }

        private bool fncCompare(string strMethod, string NewValue)
        {
            try
            {
                if (strMethod == "TimeZone")
                {
                    bool blTempTZValue = false; ;
                    for (int i = 0; i < cmbTimeZone.Items.Count; i++)
                    {
                        if (cmbTimeZone.Items[i].ToString().ToLower() == NewValue.ToLower())
                            blTempTZValue = true;
                    }
                    if (blTempTZValue)
                        return true;
                    else
                        return false;
                }

                else if (strMethod == "Country")
                {
                    bool blTempCoValue = false;
                    for (int i = 0; i < cmbCountry.Items.Count; i++)
                    {
                        if (cmbCountry.Items[i].ToString().ToLower() == NewValue.ToLower())
                            blTempCoValue = true;
                    }
                    if (blTempCoValue)
                        return true;
                    else
                        return false;
                }

                else if (strMethod == "State")
                {
                    bool blTempStateValue = false; ;
                    for (int i = 0; i < cmbState.Items.Count; i++)
                    {
                        if (cmbState.Items[i].ToString().ToLower() == NewValue.ToLower())
                            blTempStateValue = true;
                    }
                    if (blTempStateValue)
                        return true;
                    else
                        return false;
                }
                else if (strMethod == "AreaCode")
                {
                    bool blTempAreaCodeValue = false; ;
                    for (int i = 0; i < cmbAreaCode.Items.Count; i++)
                    {
                        if (cmbAreaCode.Items[i].ToString().ToLower() == NewValue.ToLower())
                            blTempAreaCodeValue = true;
                    }
                    if (blTempAreaCodeValue)
                        return true;
                    else
                        return false;
                }
                else if (strMethod == "ZipCode")
                {
                    bool blTempZipCode = false;
                    for (int i = 0; i < cmbZipCode.Items.Count; i++)
                    {
                        if (cmbZipCode.Items[i].ToString().ToLower() == NewValue.ToLower())
                            blTempZipCode = true;
                    }
                    if (blTempZipCode)
                        return true;
                    else
                        return false;
                }
                else
                    return false;
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "fncCompare()", "ctlImportLocation.xaml.cs");
                return false;
            }
        }
    }
}
