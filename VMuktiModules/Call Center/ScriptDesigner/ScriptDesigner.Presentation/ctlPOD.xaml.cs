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

namespace ScriptDesigner.Presentation
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

        public ctlPOD()
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
                        VMuktiAPI.VMuktiHelper.ExceptionHandler(exp, "ctlPOD_SizeChanged", "ctlPOD.xaml.cs");
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
                        VMuktiAPI.VMuktiHelper.ExceptionHandler(exp, "ctlPOD_SizeChanged", "ctlPOD.xaml.cs");
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
                        VMuktiAPI.VMuktiHelper.ExceptionHandler(exp, "ctlPOD_SizeChanged", "ctlPOD.xaml.cs");
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
                        VMuktiAPI.VMuktiHelper.ExceptionHandler(exp, "ctlPOD_SizeChanged", "ctlPOD.xaml.cs");
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
                        VMuktiAPI.VMuktiHelper.ExceptionHandler(exp, "ctlPOD_SizeChanged", "ctlPOD.xaml.cs");
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
                        VMuktiAPI.VMuktiHelper.ExceptionHandler(exp, "ctlPOD_SizeChanged", "ctlPOD.xaml.cs");
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
                        VMuktiAPI.VMuktiHelper.ExceptionHandler(exp, "ctlPOD_SizeChanged", "ctlPOD.xaml.cs");
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
                        VMuktiAPI.VMuktiHelper.ExceptionHandler(exp, "ctlPOD_SizeChanged", "ctlPOD.xaml.cs");
                    }
                }
            }
        }

        void ctlPOD_MouseDown(object sender, MouseButtonEventArgs e)
        {
            rect.Height = this.Height;
            rect.Width = this.Width;
            rect.SetValue(Canvas.LeftProperty, 0.0);
            rect.SetValue(Canvas.TopProperty, 0.0);
            rect.Visibility = Visibility.Visible;

            TryPoint.X = e.GetPosition(this).X;
            TryPoint.Y = e.GetPosition(this).Y;
        }

        void ctlPOD_MouseLeave(object sender, MouseEventArgs e)
        {
            current = ResizeOption.None;
        }

        void ctlPOD_PreviewMouseDown(object sender, MouseButtonEventArgs e)
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

        void ctlPOD_MouseMove(object sender, MouseEventArgs e)
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

       // forward: ;
        }
    }
}
