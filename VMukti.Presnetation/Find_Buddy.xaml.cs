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
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Data.Sql;
using System.Data.SqlClient;
using System.Data.SqlServerCe;
using Microsoft.Synchronization.Data;
using Microsoft.Synchronization.Data.Server;
using Microsoft.Synchronization.Data.SqlServerCe;
using System.IO;
using VMukti.Business;
using VMuktiAPI;
using System.Data;
using VMukti.Presentation.Controls;

namespace VMukti.Presentation
{
    /// <summary>
    /// Interaction logic for Find_Buddy.xaml
    /// </summary>
    public partial class Find_Buddy : Window
    {
        delegate void DelFindBuddy();
        DelFindBuddy delFindBuddy = null;

        public Find_Buddy()
        {
            try
            {
                InitializeComponent();

                btnAddContact.IsEnabled = false;
                btnSearch.Click += new RoutedEventHandler(btnSearch_Click);
                btnAddContact.Click += new RoutedEventHandler(btnAddContact_Click);
                btnClose.Click += new RoutedEventHandler(btnClose_Click);

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        void btnClose_Click(object sender, RoutedEventArgs e)
        {
            wndFindBuddy.Visibility = Visibility.Collapsed;
        }
        //bool isAlreadyClick = false;
       
         void btnAddContact_Click(object sender, RoutedEventArgs e)
        {
           
                string Name = ((System.Data.DataRowView)(lb.SelectedItems[0])).Row.ItemArray[0].ToString();
                VMuktiAPI.VMuktiHelper.CallEvent("FindBuddy", null, new VMuktiAPI.VMuktiEventArgs(new object[] { Name }));
                // txtUserName.Text = "";
                //txtEMailID.Text = "";

            
        }
      


        bool isAlreadyClick = false;
        public void btnSearch_Click(object sender, RoutedEventArgs e)
        {

            try
            {
                if (txtUserName.Text == "" && txtEMailID.Text == "")
                {
                    MessageBox.Show("Please enter UserName or E-Mail ID");
                }


                else
                {
                    if (txtUserName.Text.ToLower() == VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName.ToLower() || txtEMailID.Text.ToLower() == VMuktiAPI.VMuktiInfo.CurrentPeer.EMail.ToLower())
                    {
                        MessageBox.Show("You can not find yourself in your buddylist");
                        txtUserName.Text = string.Empty;
                        return;
                    }

                    else if (!isAlreadyClick)
                    {


                        isAlreadyClick = true;
                        DataSet ds = VMukti.Business.ClsUser.FindBuddy(txtUserName.Text, txtEMailID.Text);
                        int count = ds.Tables[0].Rows.Count;
                        for (int i = 0; i < count; i++)
                        {

                            if (txtUserName.Text != null || txtEMailID.Text != null)
                            {
                                lb.DataContext = ds.Tables[0];
                                btnAddContact.IsEnabled = true;
                            }


                        }
                    }
                }



            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "btnSearch_Click", "Find_Buddy.xaml.cs");
            }
            finally
            {
                
                isAlreadyClick = false;
            }

        }
        


        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void txtUserName_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
    }
}


