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
using System.Data;
using CRMDesigner.Business;

namespace CRMDesigner.Presentation
{
    /// <summary>
    /// Interaction logic for ctlGrid.xaml
    /// </summary>
    public partial class ctlGrid : System.Windows.Controls.UserControl
    {
        public static StringBuilder sb1;
        int colCount = 0;

        public delegate void ButtonClicked(int ID);
        public event ButtonClicked btnEditClicked = null;
        public event ButtonClicked btnDeleteClicked = null;

        public static StringBuilder CreateTressInfo()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("User Is : " + VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName);
            sb.AppendLine();
            sb.Append("Peer Type is : " + VMuktiAPI.VMuktiInfo.CurrentPeer.CurrPeerType.ToString());
            sb.AppendLine();
            sb.Append("User's SuperNode is : " + VMuktiAPI.VMuktiInfo.CurrentPeer.SuperNodeIP);
            sb.AppendLine();
            sb.Append("User's Machine Ip Address : " + VMuktiAPI.GetIPAddress.ClsGetIP4Address.GetIP4Address());
            sb.AppendLine();
            sb.AppendLine("----------------------------------------------------------------------------------------");
            return sb;
        }

        public ctlGrid()
        {
            try
            {
            InitializeComponent();
        }
            catch (Exception exp)
            {
                exp.Data.Add("My Key", "ctlGrid()--:--ctlGrid.xaml.cs--:--" + exp.Message + " :--:--");
                //ClsException.LogError(ex);
                //ClsException.WriteToErrorLogFile(ex);
                System.Text.StringBuilder sb = new StringBuilder();
                sb.AppendLine(exp.Message);
                sb.AppendLine();
                sb.AppendLine("StackTrace : " + exp.StackTrace);
                sb.AppendLine();
                sb.AppendLine("Location : " + exp.Data["My Key"].ToString());
                sb.AppendLine();
                sb1 = CreateTressInfo();
                sb.Append(sb1.ToString());
                VMuktiAPI.ClsLogging.WriteToTresslog(sb);
            }
        }

        public ctlGrid(DataTable dt, bool isEdit,bool isDelete)
        {
            try
            {
            InitializeComponent();

            for (int i = 0; i < dt.Columns.Count; i++)
            {
                ColumnDefinition col = new ColumnDefinition();
                grdControl.ColumnDefinitions.Add(col);
            }

            colCount = dt.Columns.Count-1;

            for (int i = 0; i <= dt.Rows.Count; i++)
            {
                RowDefinition row = new RowDefinition();
                grdControl.RowDefinitions.Add(row);
            }

            //Adding Column Headers //

            for (int c = 0; c < dt.Columns.Count; c++)
            {
                Label l = new Label();
                l.Content = dt.Columns[c].Caption.ToString();
                l.BorderBrush = Brushes.Blue;
                l.BorderThickness = new Thickness(0, 0, 0, 2);
                l.Foreground = Brushes.Red;
                l.FontSize = 11;
                
                                
                grdControl.Children.Add(l);
                Grid.SetColumn(l, c);
                Grid.SetRow(l, 0);

            }
            
            ///// Adding Contents To Grid From Tables/////
            for (int c = 0; c < dt.Columns.Count; c++)
            {
                for (int r = 1; r <= dt.Rows.Count; r++)
                {
                    Label l = new Label();
                    l.Content = dt.Rows[r-1][c].ToString();
                    l.BorderBrush = Brushes.Black;
                    l.BorderThickness = new Thickness(0, 0, 0, 1);
                    grdControl.Children.Add(l);
                    Grid.SetColumn(l, c);
                    Grid.SetRow(l, r);
                }
            }
            //////////////////////////
        
            if (isEdit == true)
            {
                AddEditButtons(dt.Rows.Count);
            }

            if (isDelete == true)
            {
                AddDeleteButtons(dt.Rows.Count);
            }
        }
            catch (Exception exp)
            {
                exp.Data.Add("My Key", "ctlGrid()--:--ctlGrid.xaml.cs--:--" + exp.Message + " :--:--");
                //ClsException.LogError(ex);
                //ClsException.WriteToErrorLogFile(ex);
                System.Text.StringBuilder sb = new StringBuilder();
                sb.AppendLine(exp.Message);
                sb.AppendLine();
                sb.AppendLine("StackTrace : " + exp.StackTrace);
                sb.AppendLine();
                sb.AppendLine("Location : " + exp.Data["My Key"].ToString());
                sb.AppendLine();
                sb1 = CreateTressInfo();
                sb.Append(sb1.ToString());
                VMuktiAPI.ClsLogging.WriteToTresslog(sb);
            }
        }
        void AddEditButtons(int MaxRows)
        {
            try
            {
            colCount = colCount + 1;

            //Setting Header //
            Label l = new Label();
            l.Content = "EDIT";
            l.BorderBrush = Brushes.Blue;
            l.BorderThickness = new Thickness(0, 0, 0, 2);
            l.Foreground = Brushes.Red;

            grdControl.Children.Add(l);
            Grid.SetColumn(l, colCount);
            Grid.SetRow(l, 0);
            ///////////////////////

            ColumnDefinition col = new ColumnDefinition();
            grdControl.ColumnDefinitions.Add(col);

            for (int r = 0; r < MaxRows; r++)
            {
                Button btnEdit = new Button();
                btnEdit.Content = "Edit";
                btnEdit.Tag = r;
                btnEdit.Click += new RoutedEventHandler(btnEdit_Click);

                grdControl.Children.Add(btnEdit);
                Grid.SetColumn(btnEdit, colCount);
                Grid.SetRow(btnEdit, r + 1); // Because 0th row is for Column Headers //
            }

            
        }
            catch (Exception exp)
            {
                exp.Data.Add("My Key", "AddEditButtons()--:--ctlGrid.xaml.cs--:--" + exp.Message + " :--:--");
                //ClsException.LogError(ex);
                //ClsException.WriteToErrorLogFile(ex);
                System.Text.StringBuilder sb = new StringBuilder();
                sb.AppendLine(exp.Message);
                sb.AppendLine();
                sb.AppendLine("StackTrace : " + exp.StackTrace);
                sb.AppendLine();
                sb.AppendLine("Location : " + exp.Data["My Key"].ToString());
                sb.AppendLine();
                sb1 = CreateTressInfo();
                sb.Append(sb1.ToString());
                VMuktiAPI.ClsLogging.WriteToTresslog(sb);
            }
        }
        void AddDeleteButtons(int MaxRows)
        {
            try
            {
            colCount = colCount + 1;

            //Setting Header //
            Label l = new Label();
            l.Content = "DELETE";
            l.BorderBrush = Brushes.Blue;
            l.BorderThickness = new Thickness(0, 0, 0, 2);
            l.Foreground = Brushes.Red;

            grdControl.Children.Add(l);
            Grid.SetColumn(l, colCount);
            Grid.SetRow(l, 0);
            ///////////////////////

            ColumnDefinition col = new ColumnDefinition();
            grdControl.ColumnDefinitions.Add(col);

            for (int r = 0; r < MaxRows; r++)
            {
                Button btnDelete = new Button();
                btnDelete.Content = "Delete";
                btnDelete.Tag = r;
                btnDelete.Click += new RoutedEventHandler(btnDelete_Click);

                grdControl.Children.Add(btnDelete);
                Grid.SetColumn(btnDelete, colCount);
                Grid.SetRow(btnDelete, r + 1); // Because 0th row is for Column Headers //
            }
        }
            catch (Exception exp)
            {
                exp.Data.Add("My Key", "AddDeleteButtons()--:--ctlGrid.xaml.cs--:--" + exp.Message + " :--:--");
                //ClsException.LogError(ex);
                //ClsException.WriteToErrorLogFile(ex);
                System.Text.StringBuilder sb = new StringBuilder();
                sb.AppendLine(exp.Message);
                sb.AppendLine();
                sb.AppendLine("StackTrace : " + exp.StackTrace);
                sb.AppendLine();
                sb.AppendLine("Location : " + exp.Data["My Key"].ToString());
                sb.AppendLine();
                sb1 = CreateTressInfo();
                sb.Append(sb1.ToString());
                VMuktiAPI.ClsLogging.WriteToTresslog(sb);
            }
        }
        void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            try
            {
            btnDeleteClicked(int.Parse(((Button)sender).Tag.ToString()));
        }
            catch (Exception exp)
            {
                exp.Data.Add("My Key", "btnDelete_Click()--:--ctlGrid.xaml.cs--:--" + exp.Message + " :--:--");
                //ClsException.LogError(ex);
                //ClsException.WriteToErrorLogFile(ex);
                System.Text.StringBuilder sb = new StringBuilder();
                sb.AppendLine(exp.Message);
                sb.AppendLine();
                sb.AppendLine("StackTrace : " + exp.StackTrace);
                sb.AppendLine();
                sb.AppendLine("Location : " + exp.Data["My Key"].ToString());
                sb.AppendLine();
                sb1 = CreateTressInfo();
                sb.Append(sb1.ToString());
                VMuktiAPI.ClsLogging.WriteToTresslog(sb);
            }
        }
        void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            try
            {
            btnEditClicked(int.Parse(((Button)sender).Tag.ToString()));
        }
            catch (Exception exp)
            {
                exp.Data.Add("My Key", "btnEdit_Click()--:--ctlGrid.xaml.cs--:--" + exp.Message + " :--:--");
                //ClsException.LogError(ex);
                //ClsException.WriteToErrorLogFile(ex);
                System.Text.StringBuilder sb = new StringBuilder();
                sb.AppendLine(exp.Message);
                sb.AppendLine();
                sb.AppendLine("StackTrace : " + exp.StackTrace);
                sb.AppendLine();
                sb.AppendLine("Location : " + exp.Data["My Key"].ToString());
                sb.AppendLine();
                sb1 = CreateTressInfo();
                sb.Append(sb1.ToString());
                VMuktiAPI.ClsLogging.WriteToTresslog(sb);
            }
        }
    }
}
