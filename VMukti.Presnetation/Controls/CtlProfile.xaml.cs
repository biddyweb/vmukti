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
using System.Text.RegularExpressions;

namespace VMukti.Presentation.Controls
{
    /// <summary>
    /// Interaction logic for CtlProfile.xaml
    /// </summary>
    public partial class CtlProfile : UserControl
    {
        string errMsg;

        public CtlProfile()
        {
            try
            {
                InitializeComponent();

               

            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "CtlProfile()", "Controls\\CtlProfile.xaml.cs");
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
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "btnDone_Click()", "Controls\\CtlProfile.xaml.cs");
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
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "btnClose_Click()", "Controls\\CtlProfile.xaml.cs");
            }
        }

        private bool validate()
        {
            try
            {
                errMsg = "";
                string pEmail = @"(\w[-._\w]*\w@\w[-._\w]*\w\.\w{2,3})";
                string pURL = @"/^(((ht|f)tp(s?))\:\/\/)?(www.|[a-zA-Z].)[a-zA-Z0-9\-\.]+\.(com|edu|gov|mil|net|org|biz|info|name|museum|us|ca|uk)(\:[0-9]+)*(/($|[a-zA-Z0-9\.\,\;\?\'\\\+&%\$#\=~_\-]+))*$/;";
                
                Regex regExEmail = new Regex(pEmail);
                Regex regExURL = new Regex(pURL);

              
                if (txtEmail.Text.Trim().Equals(""))
                {
                    errMsg += "Please enter Email \n";
                    return false;
                }
               
                if(regExEmail.IsMatch(txtEmail.Text.Trim()))
                {
                }
                else
                {
                    errMsg += "Invalid Email \n";
                    return false;
                }

                if (!(txtHomePage.Text.Trim().Equals("http://") || txtHomePage.Text.Trim().Equals("")))
                {
                    try
                    {
                        Uri homepageURL = new Uri(txtHomePage.Text.Trim());
                    }
                    catch
                    {
                        errMsg += "Invalid URL \n";
                        return false;
                    }

                }

                string day = ((ComboBoxItem)cmbBDay.SelectedItem).Content.ToString();
                string month = ((ComboBoxItem)cmbBMonth.SelectedItem).Content.ToString();
                string year = ((ComboBoxItem)cmbBYear.SelectedItem).Content.ToString();
                if ((day.Equals("-") && month.Equals("-") && year.Equals("-")))
                {
                   
                }
                else if (((day.Equals("-")) || (month.Equals("-")) || (year.Equals("-"))))
                {
                    errMsg += "Please select the complete date \n";
                    return false;
                }
                #region date validation
                switch (month)
                {
                    case "4":
                    case "6":
                    case "9":
                    case "11":
                        if (Int32.Parse(day) > 30)
                        {
                            errMsg += "Entered Date is wrong,Please Enter the correct Date\n";
                            return false;
                        }
                        break;
                    case "2":
                        if (IsLeapYear(Int32.Parse(year)))
                        {
                            if (Int32.Parse(day) > 29)
                            {
                                errMsg += "Entered Date is wrong,Please Enter the correct Date\n";
                                return false;
                            }
                        }
                        else
                        {
                            if (Int32.Parse(day) > 28)
                            {
                                errMsg += "Entered Date is wrong,Please Enter the correct Date\n";
                                return false;
                            }
                        }
                        break;


                }
                #endregion
                
                #region phone validation
                //---- here iam defining a new method "IsPhoneNumber()" to validate a given phone number as  a string-----//
                if (IsPhoneNumber(txtHomePhone.Text))
                {
                }
                else
                {
                    errMsg += "Please enter correct Home phone number";
                    return false;

                }
                if (IsPhoneNumber(txtOfficePhone.Text))
                {
                }
                else
                {
                    errMsg += "Please enter correct Office phone number";
                    return false;

                }

                if (IsPhoneNumber(txtMobilePhone.Text))
                {
                }
                else
                {
                    errMsg += "Please enter correct Mobile phone number";
                    return false;

                }

                #endregion
              
                return true;

            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "validate()", "Controls\\CtlProfile.xaml.cs");
            }
            return false;
        }

        //----------Method to check if passed year is a leap year----//

        private bool IsLeapYear(int yr)
        {
            if (yr % 4 == 0)
            {
                if (yr % 100 == 0)
                {
                    if (yr % 400 == 0)
                        return true;
                    return false;
                }
                return true;
            }
            return false;
        }

        //--------Method to check whether the passed string is a valid phonenumber or not----------//
        //--------Returns true if it is valid and flase if it is invalid---------------------------//
        private bool IsPhoneNumber(string phoneNo)
        {
            phoneNo = phoneNo.Trim();
            char[] delimiter = { '1', '2', '3', '4', '5', '6', '7', '8', '9', '0' };
            if (phoneNo == "")
                return true;// meaning its not mandatory to save phone number
            //------ checking for charecter '+'------//
            if (phoneNo.Contains("+"))
            {

                phoneNo = phoneNo.Substring(1);
            }
            if (phoneNo.Length < 10)
            {
                return false;                
                
            }

            if (phoneNo.Split(delimiter).Length != phoneNo.Length + 1)
                return false;

            return true;// indicating valid phone number

            
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (validate())
                {
                    int userID = VMuktiAPI.VMuktiInfo.CurrentPeer.ID;
                    string fullName = txtFullName.Text.Trim();
                    string email = txtEmail.Text.Trim();
                    string country = ((ComboBoxItem)cmbCountry.SelectedItem).Content.ToString();
                    string state = txtState.Text.Trim();
                    string city = txtCity.Text.Trim();
                    string timezone = ((ComboBoxItem)cmbTimezone.SelectedItem).Content.ToString();
                    string language = ((ComboBoxItem)cmbLanguage.SelectedItem).Content.ToString();
                    string gender = ((ComboBoxItem)cmbGender.SelectedItem).Content.ToString();

                    string day = ((ComboBoxItem)cmbBDay.SelectedItem).Content.ToString();
                    string month = ((ComboBoxItem)cmbBMonth.SelectedItem).Content.ToString();
                    string year = ((ComboBoxItem)cmbBYear.SelectedItem).Content.ToString();
                    DateTime birthdate = DateTime.Now;
                    if ((day.Equals("-") && month.Equals("-") && year.Equals("-")))
                    {
                        birthdate = DateTime.Parse("1/1/1753");
                    }
                    else
                    {

                        // birthdate = DateTime.Parse(((ComboBoxItem)cmbBMonth.SelectedItem).Content.ToString() + "/" + ((ComboBoxItem)cmbBDay.SelectedItem).Content.ToString() + "/" + ((ComboBoxItem)cmbBYear.SelectedItem).Content.ToString());
                        //string strMyDate = ((ComboBoxItem)cmbBDay.SelectedItem).Content.ToString() + "/" + ((ComboBoxItem)cmbBMonth.SelectedItem).Content.ToString() + "/" + ((ComboBoxItem)cmbBYear.SelectedItem).Content.ToString();
                        string strMyDate = ((ComboBoxItem)cmbBMonth.SelectedItem).Content.ToString() + "/" + ((ComboBoxItem)cmbBDay.SelectedItem).Content.ToString() + "/" + ((ComboBoxItem)cmbBYear.SelectedItem).Content.ToString();
                        birthdate = DateTime.Parse(strMyDate);
                        //  birthdate = DateTime.ParseExact(strMyDate, "dd-MM-YYYY", System.Globalization.DateTimeFormatInfo.CurrentInfo);
                        //  DateTime dt = Convert.ToDateTime(strMyDate);
                    }
                    string homePage = txtHomePage.Text.Trim();
                    txtAboutMe.SelectAll();
                    string aboutMe = txtAboutMe.Selection.Text.ToString();
                    string homePhone = txtHomePhone.Text.Trim();
                    string officePhone = txtOfficePhone.Text.Trim();
                    string mobilePhone = txtMobilePhone.Text.Trim();

                    VMukti.Business.clsProfile.UpdateUserProfile(userID, fullName, email, country, state, city, timezone, language, gender, birthdate, homePage, aboutMe, homePhone, officePhone, mobilePhone);
                    MessageBox.Show("Information Save Successfully");

                    txtCity.Text = "";
                    txtEmail.Text = "";
                    txtFullName.Text = "";
                    txtState.Text = "";
                    txtOfficePhone.Text = "";
                    txtMobilePhone.Text = "";
                    txtHomePhone.Text = "";
                    txtHomePage.Text = "http://";
                    //txtAboutMe.AppendText("");
                    cmbBDay.SelectedIndex = 0;
                    cmbBMonth.SelectedIndex = 0;
                    cmbBYear.SelectedIndex = 0;
                    cmbCountry.SelectedIndex = 0;
                    cmbGender.SelectedIndex = 0;
                    cmbLanguage.SelectedIndex = 0;
                    cmbTimezone.SelectedIndex = 0;
                    txtAboutMe.Document.Blocks.Clear();

                }
                else
                {
                    MessageBox.Show(errMsg);
                }
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "btnSave_click()", "Controls\\CtlProfile.xaml.cs");
            }
        }

        public void LoadMyProfile()
        {
            try
            {
                txtAboutMe.SelectAll();
                txtAboutMe.Selection.Text = "";
                VMukti.Business.clsProfileCollection objMyProfile = VMukti.Business.clsProfileCollection.GetUserProfile(VMuktiAPI.VMuktiInfo.CurrentPeer.ID);
                if (objMyProfile.Count != 0 && objMyProfile != null)
                {
                    txtFullName.Text = objMyProfile[0].FullName;
                    txtEmail.Text = objMyProfile[0].Email;
                    ComboBoxItem cbi;
                    cbi = new ComboBoxItem();
                    cbi.Content = objMyProfile[0].Country;
                    cbi.ToolTip = objMyProfile[0].Country;

                    
                    cmbCountry.Text= objMyProfile[0].Country;

                    txtState.Text = objMyProfile[0].State;
                    txtCity.Text = objMyProfile[0].City;

                    cbi.Content = objMyProfile[0].Timezone;
                    cbi.ToolTip = objMyProfile[0].Timezone;
                    cmbTimezone.Text = cbi.Content.ToString();

                    cbi.Content = objMyProfile[0].Language;
                    cbi.ToolTip = objMyProfile[0].Language;
                    cmbLanguage.Text = cbi.Content.ToString();

                    cbi.Content = objMyProfile[0].Gender;
                    cbi.ToolTip = objMyProfile[0].Gender;
                    cmbGender.Text = cbi.Content.ToString();

                    DateTime birthdate = objMyProfile[0].BirthDate;
                    if (birthdate.Day.ToString().Equals("1") && birthdate.Month.ToString().Equals("1") && birthdate.Year.ToString().Equals("1"))
                    {
                        cbi.Content = "-";
                        cbi.ToolTip = "-";
                        cmbBDay.Text = cbi.Content.ToString();
                        cbi.Content = "-";
                        cbi.ToolTip = "-";
                        cmbBMonth.Text = cbi.Content.ToString();
                        cbi.Content = "-";
                        cbi.ToolTip = "-";
                        cmbBYear.Text = cbi.Content.ToString();

                    }
                    else
                    {
                        cbi.Content = birthdate.Day.ToString();
                        cbi.ToolTip = birthdate.Day.ToString();
                        cmbBDay.Text= cbi.Content.ToString();
                        cbi.Content = birthdate.Month.ToString();
                        cbi.ToolTip = birthdate.Month.ToString();
                        cmbBMonth.Text = cbi.Content.ToString();
                        cbi.Content = birthdate.Year.ToString();
                        cbi.ToolTip = birthdate.Year.ToString();
                        cmbBYear.Text = cbi.Content.ToString();
                    }
                    txtHomePage.Text = objMyProfile[0].HomePage;
                    txtAboutMe.AppendText(objMyProfile[0].AboutMe);

                    txtHomePhone.Text = objMyProfile[0].HomePhone;
                    txtOfficePhone.Text = objMyProfile[0].OfficePhone;
                    txtMobilePhone.Text = objMyProfile[0].MobilePhone;

                }
                else
                {
                    txtCity.Text = "";
                    txtFullName.Text = "";
                    txtState.Text = "";
                    txtOfficePhone.Text = "";
                    txtMobilePhone.Text = "";
                    txtHomePhone.Text = "";
                    txtHomePage.Text = "http://";
                    cmbBDay.SelectedIndex = 0;
                    cmbBMonth.SelectedIndex = 0;
                    cmbBYear.SelectedIndex = 0;
                    cmbCountry.SelectedIndex = 0;
                    cmbGender.SelectedIndex = 0;
                    cmbLanguage.SelectedIndex = 0;
                    cmbTimezone.SelectedIndex = 0;
                    txtAboutMe.Document.Blocks.Clear();
                    txtEmail.Text = VMukti.Business.clsProfile.GetUserEmail(VMuktiAPI.VMuktiInfo.CurrentPeer.ID);
                }
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "LoadMyProfile()", "Controls\\CtlProfile.xaml.cs");
            }
        }

        private void cmbCountry_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            cmbCountry.ToolTip = ((ComboBoxItem)cmbCountry.SelectedItem).Content;
        }

        private void cmbTimezone_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            cmbTimezone.ToolTip = ((ComboBoxItem)cmbTimezone.SelectedItem).Content;
        }

        private void cmbLanguage_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            cmbLanguage.ToolTip = ((ComboBoxItem)cmbLanguage.SelectedItem).Content;
        }

        private void cmbGender_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            cmbGender.ToolTip = ((ComboBoxItem)cmbGender.SelectedItem).Content;
        }

        private void cmbBDay_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            cmbBDay.ToolTip = ((ComboBoxItem)cmbBDay.SelectedItem).Content;
        }

        private void cmbBMonth_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            cmbBMonth.ToolTip = ((ComboBoxItem)cmbBMonth.SelectedItem).Content;
        }

        private void cmbBYear_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            cmbBYear.ToolTip = ((ComboBoxItem)cmbBYear.SelectedItem).Content;
        }

        public void LoadNeccessaryDetailsForProfile()
        {
            try
            {
                List<object> lstCountries = VMukti.Business.clsProfile.GetAllCountries();
                foreach (object country in lstCountries)
                {
                    ComboBoxItem cbi = new ComboBoxItem();
                    cbi.Content = country.ToString();
                    cbi.ToolTip = country.ToString();
                    cmbCountry.Items.Add(cbi);
                }

                #region For cmbGender
                //List<string> lstGender = new List<string>();
                //lstGender.Add("NotSpecified");
                //lstGender.Add("Male");
                //lstGender.Add("Female");
                //foreach(object gender in lstGender)
                //{
                //    ComboBoxItem cbi = new ComboBoxItem();
                //    cbi.Content = gender.ToString();
                //    cbi.ToolTip = gender.ToString();
                //    cmbGender.Items.Add(cbi);
                //}
                //cmbGender.SelectedIndex = 0;
               
                #endregion

                List<object> lstLanguages = VMukti.Business.clsProfile.GetAllLanguages();
                foreach (object language in lstLanguages)
                {
                    ComboBoxItem cbi = new ComboBoxItem();
                    cbi.Content = language.ToString();
                    cbi.ToolTip = language.ToString();
                    cmbLanguage.Items.Add(cbi);
                }

                List<object> lstTimezones = VMukti.Business.clsProfile.GetAllTimezones();
                foreach (object timezone in lstTimezones)
                {
                    ComboBoxItem cbi = new ComboBoxItem();
                    cbi.Content = timezone.ToString();
                    cbi.ToolTip = timezone.ToString();
                    cmbTimezone.Items.Add(cbi);
                }

                int currentYear = DateTime.Today.Date.Year;
                for (int i = currentYear; i >= 1900; i--)
                {
                    ComboBoxItem cbi = new ComboBoxItem();
                    cbi.Content = i.ToString();
                    cbi.ToolTip = i.ToString();
                    cmbBYear.Items.Add(cbi);
                }
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "LoadNeccessaryDetailsForProfile()", "Controls\\CtlProfile.xaml.cs");
            }
        }

        private void btnClear_Click(object sender, RoutedEventArgs e)
        {
            txtCity.Text = "";
            txtEmail.Text = "";
            txtFullName.Text = "";
            txtState.Text = "";
            txtOfficePhone.Text = "";
            txtMobilePhone.Text = "";
            txtHomePhone.Text = "";
            txtHomePage.Text = "http://";
            //txtAboutMe.AppendText("");
            cmbBDay.SelectedIndex = 0;
            cmbBMonth.SelectedIndex = 0;
            cmbBYear.SelectedIndex = 0;
            cmbCountry.SelectedIndex = 0;
            cmbGender.SelectedIndex = 0;
            cmbLanguage.SelectedIndex = 0;
            cmbTimezone.SelectedIndex = 0;
            txtAboutMe.Document.Blocks.Clear();

        }
    }
}
