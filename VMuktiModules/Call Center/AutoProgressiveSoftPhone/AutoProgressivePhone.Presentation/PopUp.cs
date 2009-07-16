
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
using System.Windows.Controls;
using System.Windows;
using System.Windows.Media;
using System.Windows.Input;

namespace AutoProgressivePhone.Presentation
{
    class PopUp
    {
        private Grid g;
        private Window popUp;

        public PopUp()
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
            this.popUp.Title = "SoftPhone";
            this.popUp.Name = "PopUp";
            this.popUp.AllowsTransparency = true;
            this.popUp.Background = Brushes.Transparent;
            this.popUp.WindowStyle = WindowStyle.None;
            this.popUp.ShowInTaskbar = true;           
            this.popUp.Height = 200.0;
            this.popUp.Width = 400.0;
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
            BtnAutoSoftphone.Entered = 0;
        }
    }
}
