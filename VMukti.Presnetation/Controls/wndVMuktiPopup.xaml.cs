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
using System.Windows.Shapes;
using System.Windows.Media.Animation;

namespace VMukti.Presentation.Controls
{
    /// <summary>
    /// Interaction logic for wndVMuktiPopup.xaml
    /// </summary>
    public partial class wndVMuktiPopup : Window
    {
        Storyboard objsb = new Storyboard();

        public wndVMuktiPopup()
        {
            System.Timers.Timer objTimre = new System.Timers.Timer();
            try
            {
                InitializeComponent();
                objsb = this.Resources["OnLoaded1"] as Storyboard;
                this.Show();
               
            }
            catch(Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "wndVMuktiPopup()", "wndVMuktiPopup.xaml.cs");

            }
        }

        public void FnvLoadpopup(string host, string modulname, List<string> Participants)
        {
            try
            {
                tblHost.Text = "";
                tblModule.Text = "";
                tblParticipants.Text = "";
                tblHost.Text = "You are invited by " + host;
                tblModule.Text = modulname;
               
                var distpart = Participants.Distinct();
                string[] part = distpart.ToArray<string>();
                tblParticipants.Text = part[0];
                for (int i = 1; i<part.Count(); i++)
                {
                    tblParticipants.Text = tblParticipants.Text + "," + part[i];
                }
                this.Left = Convert.ToInt64(System.Windows.Forms.Screen.PrimaryScreen.Bounds.Right) - 300;
                this.Top = Convert.ToInt64(System.Windows.Forms.Screen.PrimaryScreen.Bounds.Bottom) - 180;
               // ShowWindow();
                objsb.Begin(this);
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "FnvLoadpopup1()", "wndVMuktiPopup.xaml.cs");

            }
        }

        public void FnvLoadpopup(string host, List<string> modules, List<string> Participants)
        {
            try
            {
                tblHost.Text = "";
                tblModule.Text = "";
                tblParticipants.Text = "";
                tblHost.Text = "You are invited by " + host;


                var distmod = modules.Distinct().ToArray<string>();

                if (distmod.Length > 0)
                {
                    tblModule.Text = distmod[0];
                    for (int i = 1; i < distmod.Count(); i++)
                    {
                        tblModule.Text = tblModule.Text + "," + distmod[i];
                    }
                }
                

                var distpart = Participants.Distinct();
                string[] part = distpart.ToArray<string>();
                if (part.Length > 0)
                {
                    tblParticipants.Text = part[0];
                    for (int i = 1; i < part.Count(); i++)
                    {
                        tblParticipants.Text = tblParticipants.Text + "," + part[i];
                    }
                }
                this.Left = Convert.ToInt64(System.Windows.Forms.Screen.PrimaryScreen.Bounds.Right) - 300;
                this.Top = Convert.ToInt64(System.Windows.Forms.Screen.PrimaryScreen.Bounds.Bottom) - 180;
                objsb.Begin(this);
                //ShowWindow();
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "FnvLoadpopup2()", "wndVMuktiPopup.xaml.cs");

            }
        }

      
    }
}
