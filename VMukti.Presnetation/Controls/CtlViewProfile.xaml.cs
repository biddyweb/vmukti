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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace VMukti.Presentation.Controls
{
    /// <summary>
    /// Interaction logic for CtlViewProfile.xaml
    /// </summary>
    public partial class CtlViewProfile : UserControl
    {
        public CtlViewProfile()
        {

            InitializeComponent();

        }

        public void showProfile(long userID)
        {
            try
            {
                VMukti.Business.clsProfileCollection objProfile = VMukti.Business.clsProfileCollection.GetUserProfile(int.Parse(userID.ToString()));
                if (objProfile.Count != 0 && objProfile != null)
                {
                    txtUserID.Text = VMukti.Business.clsProfile.GetUserDisplayName(int.Parse(userID.ToString()));
                    txtFullName.Text = objProfile[0].FullName;
                    txtEmail.Text = objProfile[0].Email;

                    txtCountry.Text = objProfile[0].Country;
                    txtState.Text = objProfile[0].State;
                    txtCity.Text = objProfile[0].City;
                    txtTimezone.Text = objProfile[0].Timezone;
                    txtLanguage.Text = objProfile[0].Language;
                    txtGender.Text = objProfile[0].Gender;

                    DateTime birthdate = objProfile[0].BirthDate;
                    if (birthdate.Day.ToString().Equals("1") && birthdate.Month.ToString().Equals("1") && birthdate.Year.ToString().Equals("1753"))
                    {
                        txtBirthdate.Text = "";
                    }
                    else
                    {
                        txtBirthdate.Text = birthdate.Month.ToString() + "/" + birthdate.Day.ToString() + "/" + birthdate.Year.ToString() + "\t (MM/DD/YYYY)";
                    }

                    txtHomePage.Text = objProfile[0].HomePage;
                    txtAboutMe.Text = objProfile[0].AboutMe;
                    txtHomePhone.Text = objProfile[0].HomePhone;
                    txtOfficePhone.Text = objProfile[0].OfficePhone;
                    txtMobilePhone.Text = objProfile[0].MobilePhone;
                }
                else
                {
                    txtUserID.Text = VMukti.Business.clsProfile.GetUserDisplayName(int.Parse(userID.ToString()));
                    txtFullName.Text ="";
                    txtEmail.Text = VMukti.Business.clsProfile.GetUserEmail(int.Parse(userID.ToString()));

                    txtCountry.Text = "";
                    txtState.Text = "";
                    txtCity.Text = "";
                    txtTimezone.Text = "";
                    txtLanguage.Text = "";
                    txtGender.Text = "";
                    txtBirthdate.Text = "";
                    txtHomePage.Text = "";
                    txtAboutMe.Text = "";
                    txtHomePhone.Text = "";
                    txtOfficePhone.Text = "";
                    txtMobilePhone.Text = "";
                }


            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "showProfile()", "Controls\\CtlViewProfile.xaml.cs");
            }

        }

        private void btnDone_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                this.Visibility = Visibility.Collapsed;
                ((Grid)this.Parent).Visibility = Visibility.Collapsed;
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "btnDone_Click()", "Controls\\CtlViewProfile.xaml.cs");
            }
        }

        void btnClose_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                this.Visibility = Visibility.Collapsed;
                ((Grid)this.Parent).Visibility = Visibility.Collapsed;

            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "btnClose_Click()", "Controls\\CtlViewProfile.xaml.cs");
            }
        }

    }
}
