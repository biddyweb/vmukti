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
using System.Windows;
using System.Windows.Controls;
using VMuktiAPI;
using System.Text;

namespace VMukti.Presentation.Controls
{
    /// <summary>
    /// Interaction logic for ctlGrid.xaml
    /// </summary>
    public partial class CtlBuddyGrid : System.Windows.Controls.UserControl,IDisposable
    {
        
        private Type _ObjType = null;
        public delegate void ButtonClicked(int rowID);
        public event ButtonClicked btnAcceptClicked = null;
        public event ButtonClicked btnRejectClicked = null;

        public CtlBuddyGrid()
        {
            try
            {
                InitializeComponent();
            }

            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "CtlBuddyGrid()", "Controls\\CtlBuddyGrid.xaml.cs");
            }
        }

        public void ClearGrid()
        {
            try
            {
                grdControl.RowDefinitions.Clear();
                grdControl.ColumnDefinitions.Clear();
                grdControl.Children.Clear();
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "ClearGrid()", "Controls\\CtlBuddyGrid.xaml.cs");
            }
        }

        public void Bind(object BindingCollection)
        {
            try
            {
                ClearGrid();

                grdControl.ColumnDefinitions.Add(new ColumnDefinition());
                grdControl.ColumnDefinitions[grdControl.ColumnDefinitions.Count - 1].Width = new GridLength(200);


                System.Reflection.PropertyInfo[] myProps = BindingCollection.GetType().GetProperties();
                for (int j = 0; j < myProps.Length; j++)
                {
                    if (myProps[j].PropertyType.BaseType.ToString().Split('.')[myProps[j].PropertyType.BaseType.ToString().Split('.').Length - 1].ToString() == "ClsBaseObject")
                    {
                        try
                        {
                            int rows = 1;

                            while (true)
                            {
                                object[] indexArgs = { rows - 1 };
                                System.Reflection.PropertyInfo[] mySubProps = myProps[j].GetValue(BindingCollection, indexArgs).GetType().GetProperties();

                                grdControl.RowDefinitions.Add(new RowDefinition());
                                grdControl.RowDefinitions[grdControl.RowDefinitions.Count - 1].Height = new GridLength(50);
                                grdControl.Height += 50;

                                Canvas cnvBuddy = new Canvas();
                                cnvBuddy.Width = 150;
                                cnvBuddy.Height = 50;

                                Label lblHeader = new Label();
                                lblHeader.Height = 25;
                                lblHeader.Width = 150;
                                lblHeader.SetValue(Canvas.TopProperty, 0.0);
                                lblHeader.SetValue(Canvas.LeftProperty, 0.0);
                                lblHeader.Content = mySubProps[4].GetValue(myProps[j].GetValue(BindingCollection, indexArgs), null).ToString() + " Wants to Join You";

                                cnvBuddy.Children.Add(lblHeader);

                                Label lblQuestion = new Label();
                                lblQuestion.Height = 25;
                                lblQuestion.Width = 50;
                                lblQuestion.Content = "Add ?";
                                lblQuestion.SetValue(Canvas.TopProperty, 25.0);
                                lblQuestion.SetValue(Canvas.LeftProperty, 0.0);

                                cnvBuddy.Children.Add(lblQuestion);

                                Button btnAccept = new Button();
                                btnAccept.Content = "Accept";
                                btnAccept.Width = 50;
                                btnAccept.Tag = rows - 1;
                                btnAccept.SetValue(Canvas.TopProperty, 25.0);
                                btnAccept.SetValue(Canvas.LeftProperty, 50.0);
                                btnAccept.Click += new RoutedEventHandler(btnAccept_Click);
                                cnvBuddy.Children.Add(btnAccept);

                                Button btnReject = new Button();
                                btnReject.Content = "Reject";
                                btnReject.Width = 50;
                                btnReject.Tag = rows - 1;
                                btnReject.SetValue(Canvas.TopProperty, 25.0);
                                btnReject.SetValue(Canvas.LeftProperty, 100.0);
                                btnReject.Click += new RoutedEventHandler(btnReject_Click);
                                cnvBuddy.Children.Add(btnReject);

                                grdControl.Children.Add(cnvBuddy);

                                cnvBuddy.SetValue(Grid.ColumnProperty, 0);
                                cnvBuddy.SetValue(Grid.RowProperty, rows - 1);
                                if (grdControl.Visibility == Visibility.Collapsed)
                                {
                                    grdControl.Visibility = Visibility.Visible;
                                }
                                rows++;

                            }
                        }
                        catch (Exception ex)
                        {
                            VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "Bind()", "Controls\\CtlBuddyGrid.xaml.cs");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "Bind()--2", "Controls\\CtlBuddyGrid.xaml.cs");
            }
           
        }


        void btnReject_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (btnRejectClicked != null)
                {
                    int id = int.Parse(((Button)sender).Tag.ToString());
                    btnRejectClicked(id);                  
                }
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "btnreject_Click()", "Controls\\CtlBuddyGrid.xaml.cs");
            }
        }

        void btnAccept_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (btnAcceptClicked != null)
                {
                    int id = int.Parse(((Button)sender).Tag.ToString());
                    btnAcceptClicked(id);                   
                }
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "btnAccept_click()", "Controls\\CtlBuddyGrid.xaml.cs");
            }
        }

        #region IDisposable Members

        public void Dispose()
        {
            try
            {
                if (_ObjType != null)
                {
                    _ObjType = null;
                }
                if (btnAcceptClicked != null)
                {
                    btnAcceptClicked = null;
                }
                if (btnRejectClicked != null)
                {
                    btnRejectClicked = null;

                }
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "Dispose()", "Controls\\CtlBuddyGrid.xaml.cs");
            }
        }

        #endregion

        ~CtlBuddyGrid()
        {
            Dispose();
        }

    }
}
