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

namespace CRMDesigner.Presentation
{
    /// <summary>
    /// Interaction logic for ctlPOD.xaml
    /// </summary>
    /// 

    public enum ResizeOption
    {
        None = 0,
        LeftEdge = 1,
        RightEdge = 2,
        UpEdge = 3,
        DownEdge = 4,
    }

    public partial class ctlPOD : UserControl
    {
        //public static StringBuilder sb1;
        private Point prevXY = new Point();
        private Point prevWH = new Point();
        private bool isRectVisible;

        private Point TryPoint = new Point();

        private ResizeOption current;

        private Line l = new Line();

        public Point PREVXY
        {
            get { return prevXY; }
            set { prevXY = value; }
        }

        public bool IsRectvisible
        {
            get { return isRectVisible; }
            set
            {
                isRectVisible = value;
                if (isRectVisible)
                {
                    rect.Visibility = Visibility.Visible;
                    rect.Height = this.Height;
                    rect.Width = this.Width;
                    rect.SetValue(Canvas.LeftProperty, 0.0);
                    rect.SetValue(Canvas.TopProperty, 0.0);
                    
                }
                else
                    rect.Visibility = Visibility.Collapsed;
            }
        }

        public Point PREVWH
        {
            get { return prevWH; }
            set { prevWH = value; }
        }

        public ResizeOption Current
        {
            get { return current; }
            set { current = value; }
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

        public ctlPOD()
        {
            try
            {
            InitializeComponent();

            l.Fill = Brushes.White;
            l.StrokeThickness = 2;
            cnvPOD.Children.Add(l);
            this.Focusable = true;
            this.MouseMove += new MouseEventHandler(ctlPOD_MouseMove);
            this.PreviewMouseDown += new MouseButtonEventHandler(ctlPOD_PreviewMouseDown);
            this.MouseDown += new MouseButtonEventHandler(ctlPOD_MouseDown);
            this.SizeChanged += new SizeChangedEventHandler(ctlPOD_SizeChanged);
            //this.MouseLeave += new MouseEventHandler(ctlPOD_MouseLeave);
            }
            catch (Exception exp)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(exp, "ctlPOD()", "ctlPOD.xaml.cs");
            }
        }


        void ctlPOD_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            foreach (object o in this.cnvPOD.Children)
            {
                if (o.GetType() == typeof(Button))
                {
                    try
                    {
                        ((Button)o).Height = this.Height - 4;
                        ((Button)o).Width = this.Width - 4;
                        ((Button)o).SetValue(Canvas.LeftProperty, 2.0);
                        ((Button)o).SetValue(Canvas.TopProperty, 2.0);
                    }
                    catch (Exception exp)
                    {
                        VMuktiAPI.VMuktiHelper.ExceptionHandler(exp, "ctlPOD_SizeChanged()", "ctlPOD.xaml.cs");
                    }
        

                }

                else if (o.GetType() == typeof(TextBox))
                {
                    try
                    {
                        ((TextBox)o).Height = this.Height - 4;
                        ((TextBox)o).Width = this.Width - 4;
                        ((TextBox)o).SetValue(Canvas.LeftProperty, 2.0);
                        ((TextBox)o).SetValue(Canvas.TopProperty, 2.0);
                    }
                    catch (Exception exp)
                    {
                        VMuktiAPI.VMuktiHelper.ExceptionHandler(exp, "ctlPOD_SizeChanged()", "ctlPOD.xaml.cs");
                    }
        
                }

                else if (o.GetType() == typeof(Label))
                {
                    try
                    {
                        ((Label)o).Height = this.Height - 4;
                        ((Label)o).Width = this.Width - 4;
                        ((Label)o).SetValue(Canvas.LeftProperty, 2.0);
                        ((Label)o).SetValue(Canvas.TopProperty, 2.0);
                    }
                    catch (Exception exp)
                    {
                        VMuktiAPI.VMuktiHelper.ExceptionHandler(exp, "ctlPOD_SizeChanged()", "ctlPOD.xaml.cs");
                    }
        
                }

                else if (o.GetType() == typeof(ComboBox))
                {
                    try
                    {
                        ((ComboBox)o).Height = this.Height - 4;
                        ((ComboBox)o).Width = this.Width - 4;
                        ((ComboBox)o).SetValue(Canvas.LeftProperty, 2.0);
                        ((ComboBox)o).SetValue(Canvas.TopProperty, 2.0);
                    }
                    catch (Exception exp)
                    {
                        VMuktiAPI.VMuktiHelper.ExceptionHandler(exp, "ctlPOD_SizeChanged()", "ctlPOD.xaml.cs");
                    }
        
                }

                else if (o.GetType() == typeof(ListBox))
                {
                    try
                    {
                        ((ListBox)o).Height = this.Height - 4;
                        ((ListBox)o).Width = this.Width - 4;
                        ((ListBox)o).SetValue(Canvas.LeftProperty, 2.0);
                        ((ListBox)o).SetValue(Canvas.TopProperty, 2.0);
                    }
                    catch (Exception exp)
                    {
                        VMuktiAPI.VMuktiHelper.ExceptionHandler(exp, "ctlPOD_SizeChanged()", "ctlPOD.xaml.cs");
                    }
        
                }

                else if (o.GetType() == typeof(RadioButton))
                {
                    try
                    {
                        ((RadioButton)o).Height = this.Height - 4;
                        ((RadioButton)o).Width = this.Width - 4;
                        ((RadioButton)o).SetValue(Canvas.LeftProperty, 2.0);
                        ((RadioButton)o).SetValue(Canvas.TopProperty, 2.0);
                    }
                    catch (Exception exp)
                    {
                        VMuktiAPI.VMuktiHelper.ExceptionHandler(exp, "ctlPOD_SizeChanged()", "ctlPOD.xaml.cs");
                    }
        
                }

                else if (o.GetType() == typeof(CheckBox))
                {
                    try
                    {
                        ((CheckBox)o).Height = this.Height - 4;
                        ((CheckBox)o).Width = this.Width - 4;
                        ((CheckBox)o).SetValue(Canvas.LeftProperty, 2.0);
                        ((CheckBox)o).SetValue(Canvas.TopProperty, 2.0);
                    }
                    catch (Exception exp)
                    {
                        VMuktiAPI.VMuktiHelper.ExceptionHandler(exp, "ctlPOD_SizeChanged()", "ctlPOD.xaml.cs");
                    }
        
                }

                else if (o.GetType() == typeof(TextBlock))
                {
                    try
                    {
                        ((TextBlock)o).Height = this.Height - 4;
                        ((TextBlock)o).Width = this.Width - 4;
                        ((TextBlock)o).SetValue(Canvas.LeftProperty, 2.0);
                        ((TextBlock)o).SetValue(Canvas.TopProperty, 2.0);
                    }
                    catch (Exception exp)
                    {
                        VMuktiAPI.VMuktiHelper.ExceptionHandler(exp, "ctlPOD_SizeChanged()", "ctlPOD.xaml.cs");
                    }
        
                }
            }
        }

        void ctlPOD_MouseDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
            rect.Height = this.Height;
            rect.Width = this.Width;
            rect.SetValue(Canvas.LeftProperty, 0.0);
            rect.SetValue(Canvas.TopProperty, 0.0);
            rect.Visibility = Visibility.Visible;

            TryPoint.X = e.GetPosition(this).X;
            TryPoint.Y = e.GetPosition(this).Y;
            }
            catch (Exception exp)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(exp, "ctlPOD_MouseDown()", "ctlPOD.xaml.cs");
            }

        }
        void ctlPOD_MouseLeave(object sender, MouseEventArgs e)
        {
            try
            {
            current = ResizeOption.None;
            }
            catch (Exception exp)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(exp, "ctlPOD_MouseLeave()", "ctlPOD.xaml.cs");
            }

        }
        void ctlPOD_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
            if (this.Cursor == Cursors.SizeNS || this.Cursor == Cursors.SizeWE)
            {
                prevXY.X = e.GetPosition((Canvas)this.Parent).X;
                prevXY.Y = e.GetPosition((Canvas)this.Parent).Y;
                prevWH.X = this.Width;
                prevWH.Y = this.Height;
                rect.Visibility = Visibility.Hidden;
                
            }

            }
            catch (Exception exp)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(exp, "ctlPOD_PreviewMouseDown()", "ctlPOD.xaml.cs");
            }

        }
        void ctlPOD_MouseMove(object sender, MouseEventArgs e)
        {
            try
            {

            double x = e.GetPosition(this).X;
            x = Math.Round(x);
            double y = e.GetPosition(this).Y;
            y = Math.Round(y);

            if ((int.Parse(x.ToString()) == int.Parse((this.Width).ToString()) || int.Parse(x.ToString()) == int.Parse((this.Width - 1).ToString()) || int.Parse(x.ToString()) == int.Parse((this.Width - 2).ToString())) && e.LeftButton == MouseButtonState.Released)
            {
                this.Cursor = Cursors.SizeWE;
                current = ResizeOption.RightEdge;
            }

            else if ((int.Parse(x.ToString()) == 0 || int.Parse(x.ToString()) == 1 || int.Parse(x.ToString()) == 2) && e.LeftButton == MouseButtonState.Released)
            {
                this.Cursor = Cursors.SizeWE;
                current = ResizeOption.LeftEdge;
            }

            else if ((int.Parse(y.ToString()) == int.Parse((this.Height).ToString()) || int.Parse(y.ToString()) == int.Parse((this.Height - 1).ToString()) || int.Parse(y.ToString()) == int.Parse((this.Height - 2).ToString())) && e.LeftButton == MouseButtonState.Released)
            {
                this.Cursor = Cursors.SizeNS;
                current = ResizeOption.DownEdge;

            }

            else if ((int.Parse(y.ToString()) == 0 || int.Parse(y.ToString()) == 1 || int.Parse(y.ToString()) == 2) && e.LeftButton == MouseButtonState.Released)
            {
                this.Cursor = Cursors.SizeNS;
                current = ResizeOption.UpEdge;
            }

            else if (e.LeftButton == MouseButtonState.Released)
            {
                this.Cursor = Cursors.Arrow;
                //this.current = ResizeOption.None;

            }

            forward: ;
            }
            catch (Exception exp)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(exp, "ctlPOD_MouseMove()", "ctlPOD.xaml.cs");
            }

        }
    }
}
