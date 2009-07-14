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
using Recycle.Business;
using System.Data;
using System.Data.SqlClient;
using VMuktiAPI;

namespace Recycle.Presentation
{
    /// <summary>
    /// Interaction logic for CtlRecycle.xaml
    /// </summary>
    /// 
    public enum ModulePermissions
    {
        //Recycle = 0,
        Add = 0,
        Edit = 1,
        Delete = 2,
        View = 3

    }
    public partial class CtlRecycle : System.Windows.Controls.UserControl
    {
        
        int varTop = 30;
        CheckBox[] chkDispositions;
        CheckBox chkAll = new CheckBox();
        ModulePermissions[] _MyPermissions;

        public CtlRecycle(ModulePermissions[] MyPermissions)
        {
           

            try
            {

                InitializeComponent();

                _MyPermissions = MyPermissions;
                FncPermissionsReview();

                FncFillCombo();
                cmbCampaign.SelectionChanged += new SelectionChangedEventHandler(cmbCampaign_SelectionChanged);
                btnRecycle.Click += new RoutedEventHandler(btnRecycle_Click);
                btnCancel.Click += new RoutedEventHandler(btnCancel_Click);
                this.Loaded += new RoutedEventHandler(CtlRecycle_Loaded);
               
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "CtlRecycle()", "CtlRecycle.xaml.xs");
                
            }
        }

        void CtlRecycle_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                this.Width = ((Grid)this.Parent).ActualWidth;
                ((Grid)this.Parent).SizeChanged += new SizeChangedEventHandler(CtlRecycle_SizeChanged);
            }
            catch(Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "CtlRecycle_Loaded", "CtlRecycle.xaml.cs");
            }
        }

        void CtlRecycle_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            try
            {
                this.Width = e.NewSize.Width - 5;
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "CtlRecycle_SizeChanged", "CtlRecycle.xaml.cs");

            }
        }

       

        void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                rtcSummary.SelectAll();
                rtcSummary.Selection.Text = "";
                for (int i = 0; i < chkDispositions.Length; i++)
                {
                    chkDispositions[i].IsChecked = false;
                }
                chkAll.IsChecked = false;
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "btnCancel_Click()", "CtlRecycle.xaml.xs");
            }
        }

        void FncPermissionsReview()
        {
            try
            {

                this.Visibility = Visibility.Hidden;
                //for (int i = 0; i < _MyPermissions.Length; i++)
                //{
                //    if (_MyPermissions[i] == ModulePermissions.Recycle)
                //    {
                //        this.Visibility = Visibility.Visible;
                //    }

                //}
                for (int i = 0; i < _MyPermissions.Length; i++)
                {
                    if (_MyPermissions[i] == ModulePermissions.Edit)
                    {
                        //CtlGrid.CanEdit = true;
                    }
                    if (_MyPermissions[i] == ModulePermissions.Delete)
                    {
                        //CtlGrid.CanDelete = true;
                    }
                    if (_MyPermissions[i] == ModulePermissions.View)
                    {
                        //CtlGrid.Visibility = Visibility.Visible;
                    }
                    if (_MyPermissions[i] == ModulePermissions.Add)
                    {
                        this.Visibility = Visibility.Visible;
                    }
                }
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "FncPermissionsReview()", "CtlRecycle.xaml.xs");
            }

        }
        void btnRecycle_Click(object sender, RoutedEventArgs e)
        {
            try
            {
            rtcSummary.SelectAll();
            rtcSummary.Selection.Text="";

            string strSummary = "";

            //string strList = "";
            for (int i = 0; i < chkDispositions.Length; i++)
            {
                if (chkDispositions[i].IsChecked == true)
                {
                    DataSet ds = ClsRecycle.Recycle_Leads(Int64.Parse(chkDispositions[i].Tag.ToString()), Int64.Parse(((ListBoxItem)lstLists.SelectedItem).Tag.ToString()));
                    DataTable dt = ds.Tables[0];
                    Int64 MaxID = Int64.Parse(ds.Tables[1].Rows[0][0].ToString());

                    for (int j = 0; j < dt.Rows.Count; j++)
                    {
                        dt.Rows[j][0] = MaxID;
                        dt.Rows[j]["Status"] = "Fresh";
                        MaxID++;
                    }

                    dt.Columns.RemoveAt(dt.Columns.Count - 1);
                    SqlBulkCopy sqlBulk = new SqlBulkCopy(VMuktiAPI.VMuktiInfo.MainConnectionString);
                    sqlBulk.DestinationTableName = "Leads";
                    sqlBulk.BulkCopyTimeout = 3600;
                    sqlBulk.WriteToServer(dt);
                    sqlBulk.Close();
                    strSummary = strSummary + dt.Rows.Count.ToString() + " Leads Recycled For Disposition " + chkDispositions[i].Content.ToString() + "\n";
                }
            }

            rtcSummary.AppendText(strSummary);
            }
            catch(Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "btnRecycle_Click()", "CtlRecycle.xaml.xs");
            }

        }

        void cmbCampaign_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            svrDisposition.Visibility = Visibility.Visible;
            try
            {
                DataSet ds = ClsRecycle.List_GetAll(Int64.Parse(((ListBoxItem)cmbCampaign.SelectedItem).Tag.ToString()));
                DataTable dt = ds.Tables[0];

                lstLists.Items.Clear();

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ListBoxItem l = new ListBoxItem();


                    l.Tag = dt.Rows[i][0];
                    l.Content = dt.Rows[i][1];
                    lstLists.Items.Add(l);
                }
                FncFillCheckBoxes();
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "cmbCampaign_SelectionChanged()", "CtlRecycle.xaml.xs");

            }
        }

        public void FncFillCheckBoxes()
        {
            try
            {
                cnvDispo.Children.Clear();
                DataSet ds = ClsRecycle.Disposition_GetAll(Int64.Parse(((ListBoxItem)cmbCampaign.SelectedItem).Tag.ToString()));
                DataTable dt = ds.Tables[0];

                chkDispositions = new CheckBox[dt.Rows.Count];
                chkAll.Content = "ALL Dispositions";
                chkAll.IsChecked = false;
                chkAll.FontSize = 15;
                Canvas.SetLeft(chkAll, 10);
                Canvas.SetTop(chkAll, 0);
                if (chkAll.Parent == null)
                {
                    cnvDispo.Children.Add(chkAll);
                }
                chkAll.Click += new RoutedEventHandler(chkAll_Click);

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    chkDispositions[i] = new CheckBox();
                    chkDispositions[i].Content = dt.Rows[i][1].ToString();
                    chkDispositions[i].Tag = dt.Rows[i][0].ToString();
                    Canvas.SetLeft(chkDispositions[i], 10);
                    Canvas.SetTop(chkDispositions[i], ((i + 1) * varTop));
                    chkDispositions[i].Click += new RoutedEventHandler(CtlRecycle_Click);
                    cnvDispo.Children.Add(chkDispositions[i]);
                }

                cnvDispo.Height = (dt.Rows.Count + 1) * varTop;
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "FncFillCheckBoxes()", "CtlRecycle.xaml.xs");
            }
        }

        void chkAll_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                for (int i = 0; i < chkDispositions.Length; i++)
                {
                    if (((CheckBox)sender).IsChecked == true)
                        chkDispositions[i].IsChecked = true;
                    else
                        chkDispositions[i].IsChecked = false;
                }
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "chkAll_Click()", "CtlRecycle.xaml.xs");
            }
        }

        void CtlRecycle_Click(object sender, RoutedEventArgs e)
        {
            int count = 0;
            try
            {
                for (int i = 0; i < chkDispositions.Length; i++)
                {
                    if (chkDispositions[i].IsChecked == true)
                    {
                        count++;
                    }
                }
                if (count == chkDispositions.Length)
                {
                    chkAll.IsChecked = true;
                }
                else
                {
                    chkAll.IsChecked = false;
                }
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "CtlRecycle_Click()", "CtlRecycle.xaml.xs");
            }
        }

        public void FncFillCombo()
        {
            DataSet ds = ClsRecycle.Campaign_GetAll();
            DataTable dt = ds.Tables[0];
            try
            {

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ComboBoxItem l = new ComboBoxItem();
                    l.Tag = dt.Rows[i][0];

                    l.Content = dt.Rows[i][1];
                    cmbCampaign.Items.Add(l);
                }
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "FncFillCombo()", "CtlRecycle.xaml.xs");
            }

        }


    }
}
