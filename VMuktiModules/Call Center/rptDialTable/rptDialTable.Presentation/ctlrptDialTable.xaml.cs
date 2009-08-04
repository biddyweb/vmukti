/* VMukti 2.0 -- An Open Source Video Communications Suite
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

using VMuktiService;

using System.ServiceModel;
using System.Reflection;
using System.Data;
using Microsoft.Reporting.WinForms;
using VMuktiAPI;

namespace rptDialTable.Presentation
{
    /// <summary>
    /// Interaction logic for ctlChat.xaml
    /// </summary>
    /// 

    //set permission for Report-DialTable
    public enum ModulePermissions
    {
        Add = 0,
        Edit = 1,
        Delete = 2,
        View = 3
    }

    public partial class ctlrptDialTable : UserControl
    {
        DataSet dsReport = new dsrptDialTable();
        ReportDataSource rds = new ReportDataSource();
        
        public ctlrptDialTable(ModulePermissions[] MyPermissions)
        {
            try
            {
                InitializeComponent();
                this.Loaded += new RoutedEventHandler(ctlrptDialTable_Loaded);                
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "ctlrptDialTable()", "ctlrptDialTable.xaml.cs");
            }            
        }

        //Generate Report
        private void btnGo_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                wfhRptViewer.Visibility = Visibility.Visible;
                DateTime temp = dtpStartDate.Value.Value;
                DateTime start = new DateTime(temp.Year, temp.Month, temp.Day, cmbStartHour.SelectedIndex, cmbStartMinute.SelectedIndex==1?30:0, 0);
                temp = dtpEndDate.Value.Value;
                DateTime end = new DateTime(temp.Year,temp.Month,temp.Day,cmbEndHour.SelectedIndex,(cmbEndMinute.SelectedIndex==1)?30:0,0);
                //MessageBox.Show(start + "  "+ cmbEndMinute.SelectedIndex);
                //Get data in Dataset of dsReport from rptDialTable.Business
                dsReport = rptDialTable.Business.ClsRptDialTable.GetHistoryDataOfDates(start,end);

                if (dsReport.Tables.Count > 0)
                {
                    dsReport.Tables[0].TableName = "dtrptDialTable";
                }

                rds.Name = "dsrptDialTable_dtrptDialTable";
                //set value of report viewer 
                rds.Value = dsReport.Tables["dtrptDialTable"];
                objReportViewer.LocalReport.ReportPath = Assembly.GetAssembly(this.GetType()).Location.Replace("rptDialTable.Presentation.dll", "rptDialTable.rdlc");
                objReportViewer.LocalReport.ReportEmbeddedResource = Assembly.GetAssembly(this.GetType()).Location.Replace("rptDialTable.Presentation.dll", "rptDialTable.rdlc");
                objReportViewer.LocalReport.DataSources.Add(rds);
                objReportViewer.RefreshReport();
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "btnGo_Click", "ctlrptDialTable.xaml.cs");
            }
        }

        #region changes for reportviewer resiging
        

        void ctlrptDialTable_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                if (this.Parent != null)
                {
                    if (this.Parent.GetType() == typeof(Grid))
                    {
                        this.Width = ((Grid)this.Parent).ActualWidth;
                        this.Height = ((Grid)this.Parent).ActualHeight;
                        wfhRptViewer.Height = ((Grid)this.Parent).ActualHeight - 150.0;
                        ((Grid)this.Parent).SizeChanged += new SizeChangedEventHandler(ctlrptDialTable_SizeChanged);
                    }
                    else if (this.Parent.GetType() == typeof(TabItem))
                    {

                        ((TabControl)((TabItem)this.Parent).Parent).SizeChanged += new SizeChangedEventHandler(ctlRptRemainingLeads_SizeChanged);
                        this.Width = ((TabControl)((TabItem)this.Parent).Parent).ActualWidth;
                        this.Height = ((TabControl)((TabItem)this.Parent).Parent).ActualHeight;
                        wfhRptViewer.Height = ((TabControl)((TabItem)this.Parent).Parent).ActualHeight - 150.0;
                        wfhRptViewer.Width = ((TabControl)((TabItem)this.Parent).Parent).ActualWidth - 130.0;
                    }
                }
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "ctlrptDialTable_Loaded()", "ctlrptDialTable.xaml.cs");
            }
        }

        void ctlRptRemainingLeads_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                if (this.Parent != null)
                {
                    if (this.Parent.GetType() == typeof(Grid))
                    {
                        ((Grid)this.Parent).SizeChanged += new SizeChangedEventHandler(ctlRptRemainingLeads_SizeChanged);
                        wfhRptViewer.Height = ((Grid)this.Parent).ActualHeight - 150.0;
                        wfhRptViewer.Width = ((Grid)this.Parent).ActualWidth - 4.0;
                    }
                    else if (this.Parent.GetType() == typeof(TabItem))
                    {
                        ((TabControl)((TabItem)this.Parent).Parent).SizeChanged += new SizeChangedEventHandler(ctlRptRemainingLeads_SizeChanged);
                        this.Width = ((TabControl)((TabItem)this.Parent).Parent).ActualWidth;
                        this.Height = ((TabControl)((TabItem)this.Parent).Parent).ActualHeight;
                        wfhRptViewer.Height = ((TabControl)((TabItem)this.Parent).Parent).ActualHeight - 150.0;
                        wfhRptViewer.Width = ((TabControl)((TabItem)this.Parent).Parent).ActualWidth - 130.0;
                    }
                }
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "ctlRptRemainingLeads_Loaded()", "ctlrptDialTable.xaml.cs");
            }
        }

        void ctlrptDialTable_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            try
            {
                if (this.Parent != null)
                {
                    if (this.Parent.GetType() == typeof(Grid))
                    {
                        this.Width = e.NewSize.Width;
                        this.Height = e.NewSize.Height;
                        wfhRptViewer.Width = e.NewSize.Width - 4.0;
                        wfhRptViewer.Height = e.NewSize.Height - 150.0;
                    }
                }
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "ctlrptDialTable_SizeChanged()", "ctlrptDialTable.xaml.cs");
            }
        }

        void ctlRptRemainingLeads_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            try
            {
                if (this.Parent != null)
                {
                    if (this.Parent.GetType() == typeof(Grid))
                    {
                        if (e.NewSize.Width > 0)
                        {
                            this.Width = e.NewSize.Width;
                            wfhRptViewer.Width = e.NewSize.Width - 4.0;
                            wfhRptViewer.Height = e.NewSize.Height - 150.0;
                        }
                    }
                    else if (this.Parent.GetType() == typeof(TabItem))
                    {
                        if (e.NewSize.Width > 0)
                        {
                            this.Width = e.NewSize.Width - 4.0;
                            wfhRptViewer.Width = e.NewSize.Width - 130.0;
                            wfhRptViewer.Height = e.NewSize.Height - 150.0;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "ctlRptRemainingLeads_SizeChanged()", "ctlrptDialTable.xaml.cs");
            }
        }



        #endregion

        private void dtpStartDate_DropDownOpened(object sender, RoutedEventArgs e)
        {
            try
            {
                wfhRptViewer.Visibility = Visibility.Hidden;
            }
            catch(Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "dtpStartDate_DropDownOpened()", "ctlrptDialTable.xaml.cs");
            }
        }

        private void cmbStartHour_DropDownOpened(object sender, EventArgs e)
        {
            try
            {
                wfhRptViewer.Visibility = Visibility.Hidden;
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "cmbStartHour_DropDownOpened()", "ctlrptDialTable.xaml.cs");
            }
        }
    }
}








