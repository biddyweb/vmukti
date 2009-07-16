using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Controls;
using System.Windows;
using System.Windows.Media;
using System.Windows.Input;

namespace DispositionRender.Presentation
{
    class Disposition_PopUp
    {
        private Grid g;
        private Window popUp;
        
        public Disposition_PopUp()
        {
            this.popUp = null;
            this.g = new Grid();
            
        }
        public void ShowPopUp(UserControl abc, Point x)
        {
            LinearGradientBrush brush = new LinearGradientBrush();
            GradientStop stop = new GradientStop(Colors.Azure, 0.0);
            GradientStop stop2 = new GradientStop(Colors.Black, 1.0);
            brush.GradientStops.Add(stop);
            brush.GradientStops.Add(stop2);
            this.g.Background = brush;
            this.popUp = new Window();
            this.popUp.Title = "Dispositon Render";
            this.popUp.Name = "PopUp";
            this.popUp.AllowsTransparency = true;
            this.popUp.Background = Brushes.Transparent;
            this.popUp.WindowStyle = WindowStyle.None;
            this.popUp.ShowInTaskbar = true;
            //this.popUp.Topmost = true;
            this.popUp.Height = 400.0;
            this.popUp.Width = 230.0;
            
            this.popUp.MouseLeave += new MouseEventHandler(this.popUp_MouseLeave);
            double primaryScreenWidth = SystemParameters.PrimaryScreenWidth;
            double primaryScreenHeight = SystemParameters.PrimaryScreenHeight;
            double num3 = primaryScreenWidth - x.X;
            double num4 = primaryScreenHeight - x.Y;
            if (num3 < this.popUp.Width)
            {
                this.popUp.Left = primaryScreenWidth - this.popUp.Width;
            }
            else
            {
                this.popUp.Left = x.X - 80.0;
                if (this.popUp.Left < 0.0)
                {
                    this.popUp.Left = 0.0;
                }
            }
            if (num4 < this.popUp.Height)
            {
                this.popUp.Top = primaryScreenHeight - this.popUp.Height;
            }
            else
            {
                this.popUp.Top = x.Y - 30.0;
                if (this.popUp.Top < 0.0)
                {
                    this.popUp.Top = 0.0;
                }
            }           
            
            this.g.Children.Add(abc);
           
            this.popUp.Content = this.g;
            this.popUp.Show();
        }
        public void popUp_MouseLeave(object sender, MouseEventArgs e)
        {
            this.g.Children.Clear();
            this.popUp.Close();
            BtnAutoDispositionRender.Entered = 0;
        }
    }
}
