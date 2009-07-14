using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace DashBoard.Presentation.CampaignManagement
{
    public class PopUp
    {
        public static void ShowPopUp(double winHeight, double winWidth, string Campaign, Thickness abc)
        {
            Window wnd = new Window();
            wnd.Name = "PopUp";
            wnd.Height = winHeight;
            wnd.Width = winWidth;
            wnd.AllowsTransparency = true;
            wnd.WindowStyle = WindowStyle.None;
            wnd.Margin = abc;
            Grid gd = new Grid();
            gd.Visibility = Visibility.Visible;
            wnd.Content = gd;
            wnd.Show();

        }
    }
}
