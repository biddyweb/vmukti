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
using CRMDesigner.Business;

namespace CRMDesigner.Presentation
{
    /// <summary>
    /// Interaction logic for ctlQuestion.xaml
    /// </summary>
    /// 
   public partial class ctlQuestion : UserControl
    {


       public static StringBuilder sb1;
       List<TextBox> lsttxt = new List<TextBox>() { };
       List<clsQuestionDynamic> lstQuestions = new List<clsQuestionDynamic>() { };
       List<ComboBox> lstActionQue = new List<ComboBox>() { };
       string StartQuestion = "";
       int currentQuestion = 0;

       public static StringBuilder CreateTressInfo()
       {
           StringBuilder sb = new StringBuilder();
           sb.Append("User Is : " + VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName);
           sb.AppendLine();
           sb.Append("Peer Type is : " + VMuktiAPI.VMuktiInfo.CurrentPeer.CurrPeerType.ToString());
           sb.AppendLine();
           sb.Append("User's SuperNode is : " + VMuktiAPI.VMuktiInfo.CurrentPeer.SuperNodeIP);
           sb.AppendLine();
           sb.Append("User's Machine Ip Address : " + VMuktiAPI.GetIPAddress.ClsGetIP4Address.GetIP4Address());
           sb.AppendLine();
           sb.AppendLine("----------------------------------------------------------------------------------------");
           return sb;
       }

        public ctlQuestion()
        {
            try
            {
            InitializeComponent();
        }
            catch (Exception exp)
            {
                exp.Data.Add("My Key", "ctlQuestion()--:--clsQuestion.xaml.cs--:--" + exp.Message + " :--:--");
                //ClsException.LogError(ex);
                //ClsException.WriteToErrorLogFile(ex);
                System.Text.StringBuilder sb = new StringBuilder();
                sb.AppendLine(exp.Message);
                sb.AppendLine();
                sb.AppendLine("StackTrace : " + exp.StackTrace);
                sb.AppendLine();
                sb.AppendLine("Location : " + exp.Data["My Key"].ToString());
                sb.AppendLine();
                sb1 = CreateTressInfo();
                sb.Append(sb1.ToString());
                VMuktiAPI.ClsLogging.WriteToTresslog(sb);
            }
        }

        private void txtNoOfOptions_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
        }
            catch (Exception exp)
            {
                exp.Data.Add("My Key", "txtNoOfOptions_TextChanged()--:--clsQuestion.xaml.cs--:--" + exp.Message + " :--:--");
                //ClsException.LogError(ex);
                //ClsException.WriteToErrorLogFile(ex);
                System.Text.StringBuilder sb = new StringBuilder();
                sb.AppendLine(exp.Message);
                sb.AppendLine();
                sb.AppendLine("StackTrace : " + exp.StackTrace);
                sb.AppendLine();
                sb.AppendLine("Location : " + exp.Data["My Key"].ToString());
                sb.AppendLine();
                sb1 = CreateTressInfo();
                sb.Append(sb1.ToString());
                VMuktiAPI.ClsLogging.WriteToTresslog(sb);
            }
        }


        private void btnOK_Click(object sender, RoutedEventArgs e)
        {
            try
            {
            cnvOptions.Children.Clear();
            lsttxt.Clear();
            lstActionQue.Clear();
            int number = 0;
            if(txtNoOfOptions.Text.Trim() !="")
            number = Int16.Parse(txtNoOfOptions.Text.Trim());

            for (int i = 0; i < number; i++)
            {
                Label lbl = new Label();
                lbl.Height = 23;
                lbl.Width = 70;
                lbl.Content = "Option " + (i+1).ToString();
                lbl.SetValue(Canvas.LeftProperty, 20.0);
                lbl.SetValue(Canvas.TopProperty, 10 + (25.0 * i));
                cnvOptions.Children.Add(lbl);

                TextBox txt = new TextBox();
                txt.Height = 23;
                txt.Width = 600;
                txt.SetValue(Canvas.LeftProperty, 100.0);
                txt.SetValue(Canvas.TopProperty, 10 + (25.0 * i));
                lsttxt.Add(txt);
                cnvOptions.Children.Add(txt);

                ComboBox cmb = new ComboBox();
                cmb.Height = 23;
                cmb.Width = 300;

                for (int i1 = 0; i1 < lstQuestions.Count; i1++)
                {
                    ComboBoxItem cbi = new ComboBoxItem();
                    cbi.Content = lstQuestions[i1].Header;
                    cbi.Tag = i1.ToString();
                    cmb.Items.Add(cbi);
                }

                cmb.SetValue(Canvas.LeftProperty, 710.0);
                cmb.SetValue(Canvas.TopProperty, 10 + (25.0 * i));
                lstActionQue.Add(cmb);
                //lsttxt.Add(txt);
                cnvOptions.Children.Add(cmb);
                
            }
        }
            catch (Exception exp)
            {
                exp.Data.Add("My Key", "btnOK_Click()--:--clsQuestion.xaml.cs--:--" + exp.Message + " :--:--");
                //ClsException.LogError(ex);
                //ClsException.WriteToErrorLogFile(ex);
                System.Text.StringBuilder sb = new StringBuilder();
                sb.AppendLine(exp.Message);
                sb.AppendLine();
                sb.AppendLine("StackTrace : " + exp.StackTrace);
                sb.AppendLine();
                sb.AppendLine("Location : " + exp.Data["My Key"].ToString());
                sb.AppendLine();
                sb1 = CreateTressInfo();
                sb.Append(sb1.ToString());
                VMuktiAPI.ClsLogging.WriteToTresslog(sb);
            }
        }
        private void btnNext_Click(object sender, RoutedEventArgs e)
        {
            try
            {
            List<string> strOptions = new List<string>() { };
            int count = 0;
            if (txtNoOfOptions.Text.Trim() != "")
                count = int.Parse(txtNoOfOptions.Text);
            

            for (int i = 0; i < count; i++)
            {
                string str = lsttxt[i].Text;
                if (lstActionQue[i].Items.Count > 0)
                {
                    str = str + "*****" + ((ComboBoxItem)lstActionQue[i].SelectedItem).Content.ToString();
                }
                strOptions.Add(str);
            }

            if(((ComboBoxItem)cmbType.SelectedItem).Content.ToString().ToLower() == "combobox")
            {
                clsQuestionDynamic objQuestion = new clsQuestionDynamic(txtHeader.Text, count, TypeOfOptions.ComboBox, strOptions);
                lstQuestions.Add(objQuestion);
            }
            else if(((ComboBoxItem)cmbType.SelectedItem).Content.ToString().ToLower() == "listbox")
            {
                clsQuestionDynamic objQuestion = new clsQuestionDynamic(txtHeader.Text, count, TypeOfOptions.ListBox, strOptions);
                lstQuestions.Add(objQuestion);
            }
            else if (((ComboBoxItem)cmbType.SelectedItem).Content.ToString().ToLower() == "radiobutton")
            {
                clsQuestionDynamic objQuestion = new clsQuestionDynamic(txtHeader.Text, count, TypeOfOptions.RadioButton, strOptions);
                lstQuestions.Add(objQuestion);
            }
            else if (((ComboBoxItem)cmbType.SelectedItem).Content.ToString().ToLower() == "checkbox")
            {
                clsQuestionDynamic objQuestion = new clsQuestionDynamic(txtHeader.Text, count, TypeOfOptions.CheckBox, strOptions);
                lstQuestions.Add(objQuestion);
            }

            else if (((ComboBoxItem)cmbType.SelectedItem).Content.ToString().ToLower() == "textbox")
            {
                clsQuestionDynamic objQuestion = new clsQuestionDynamic(txtHeader.Text, count, TypeOfOptions.TextBox, strOptions);
                lstQuestions.Add(objQuestion);
            }

            lstQuestion.Items.Add(txtHeader.Text);

            cnvOptions.Children.Clear();

            txtHeader.Text = "";
            txtNoOfOptions.Text = "";

        }
            catch (Exception exp)
            {
                exp.Data.Add("My Key", "btnNext_Click()--:--clsQuestion.xaml.cs--:--" + exp.Message + " :--:--");
                //ClsException.LogError(ex);
                //ClsException.WriteToErrorLogFile(ex);
                System.Text.StringBuilder sb = new StringBuilder();
                sb.AppendLine(exp.Message);
                sb.AppendLine();
                sb.AppendLine("StackTrace : " + exp.StackTrace);
                sb.AppendLine();
                sb.AppendLine("Location : " + exp.Data["My Key"].ToString());
                sb.AppendLine();
                sb1 = CreateTressInfo();
                sb.Append(sb1.ToString());
                VMuktiAPI.ClsLogging.WriteToTresslog(sb);
            }
        }
        private void btnFinish_Click(object sender, RoutedEventArgs e)
        {
            try
            {
            FncNextQuestion();
        }
            catch (Exception exp)
            {
                exp.Data.Add("My Key", "btnFinish_Click()--:--clsQuestion.xaml.cs--:--" + exp.Message + " :--:--");
                //ClsException.LogError(ex);
                //ClsException.WriteToErrorLogFile(ex);
                System.Text.StringBuilder sb = new StringBuilder();
                sb.AppendLine(exp.Message);
                sb.AppendLine();
                sb.AppendLine("StackTrace : " + exp.StackTrace);
                sb.AppendLine();
                sb.AppendLine("Location : " + exp.Data["My Key"].ToString());
                sb.AppendLine();
                sb1 = CreateTressInfo();
                sb.Append(sb1.ToString());
                VMuktiAPI.ClsLogging.WriteToTresslog(sb);
            }
        }
        void FncNextQuestion()
        {
            try
            {
            cnvMain.Children.Clear();
            currentQuestion = -1;

            for (int i = 0; i < lstQuestions.Count; i++)
            {
                if (lstQuestions[i].Header == StartQuestion)
                {
                    currentQuestion = i;
                    break;
                }
            }

            if (currentQuestion == -1)
                goto exit;
            
            Label lblHeader = new Label();
            lblHeader.Content = lstQuestions[currentQuestion].Header;
            lblHeader.Height = 30;
            lblHeader.Width = 600;
            lblHeader.SetValue(Canvas.LeftProperty, 10.0);
            lblHeader.SetValue(Canvas.TopProperty, 10.0);

            if (lstQuestions[currentQuestion].Type == TypeOfOptions.RadioButton)
            {
                RadioButton[] rdo = new RadioButton[lstQuestions[currentQuestion].NoOfOptions];
                List<string> strOpt = lstQuestions[currentQuestion].Options;
                for (int i = 0; i < lstQuestions[currentQuestion].NoOfOptions; i++)
                {
                    rdo[i] = new RadioButton();
                    string[] str = strOpt[i].Split(new string[] {"*****"}, StringSplitOptions.None);
                    rdo[i].Content = str[0];
                    rdo[i].Tag = str[1];
                    rdo[i].Height = 25;
                    rdo[i].Width = 600;
                    rdo[i].SetValue(Canvas.LeftProperty, 80.0);
                    rdo[i].SetValue(Canvas.TopProperty, 10.0 + ((i + 1) * 30));
                    cnvMain.Children.Add(rdo[i]);
                }
            }

            else if (lstQuestions[currentQuestion].Type == TypeOfOptions.CheckBox)
            {

                CheckBox[] rdo = new CheckBox[lstQuestions[currentQuestion].NoOfOptions];
                List<string> strOpt = lstQuestions[currentQuestion].Options;
                for (int i = 0; i < lstQuestions[currentQuestion].NoOfOptions; i++)
                {

                    rdo[i] = new CheckBox();
                    string[] str = strOpt[i].Split(new string[] { "*****" }, StringSplitOptions.None);
                    rdo[i].Content = str[0];
                    rdo[i].Tag = str[1];
                    rdo[i].Height = 25;
                    rdo[i].Width = 600;
                    rdo[i].SetValue(Canvas.LeftProperty, 80.0);
                    rdo[i].SetValue(Canvas.TopProperty, 10.0 + ((i + 1) * 30));
                    cnvMain.Children.Add(rdo[i]);
                }
            }

            else if (lstQuestions[currentQuestion].Type == TypeOfOptions.ListBox)
            {

                ListBox lst = new ListBox();
                lst.Height = 250;
                lst.Width = 250;
                lst.SetValue(Canvas.LeftProperty, 80.0);
                lst.SetValue(Canvas.TopProperty, 80.0);
                cnvMain.Children.Add(lst);
                ListBoxItem[] lbi = new ListBoxItem[lstQuestions[currentQuestion].NoOfOptions];
                List<string> strOpt = lstQuestions[currentQuestion].Options;
                for (int i = 0; i < lstQuestions[currentQuestion].NoOfOptions; i++)
                {
                    lbi[i] = new ListBoxItem();
                    string[] str = strOpt[i].Split(new string[] { "*****" }, StringSplitOptions.None);
                    lbi[i].Content = str[0];
                    lbi[i].Tag = str[1];
                    lst.Items.Add(lbi[i]);
                }
            }

            else if (lstQuestions[currentQuestion].Type == TypeOfOptions.ComboBox)
            {
                ComboBox cmb = new ComboBox();
                cmb.Height = 30;
                cmb.Width = 250;
                cmb.SetValue(Canvas.LeftProperty, 80.0);
                cmb.SetValue(Canvas.TopProperty, 80.0);
                cnvMain.Children.Add(cmb);
                ComboBoxItem[] cbi = new ComboBoxItem[lstQuestions[currentQuestion].NoOfOptions];
                List<string> strOpt = lstQuestions[currentQuestion].Options;
                for (int i = 0; i < lstQuestions[currentQuestion].NoOfOptions; i++)
                {
                    cbi[i] = new ComboBoxItem();
                    string[] str = strOpt[i].Split(new string[] { "*****" }, StringSplitOptions.None);
                    cbi[i].Content = str[0];
                    cbi[i].Tag = str[1];
                    cmb.Items.Add(cbi[i]);
                }
            }

            else if (lstQuestions[currentQuestion].Type == TypeOfOptions.TextBox)
            {
                Label[] lbl = new Label[lstQuestions[currentQuestion].NoOfOptions];
                TextBox[] txt = new TextBox[lstQuestions[currentQuestion].NoOfOptions];
                List<string> strOpt = lstQuestions[currentQuestion].Options;
                for (int i = 0; i < lstQuestions[currentQuestion].NoOfOptions; i++)
                {

                    lbl[i] = new Label();
                    string[] str = strOpt[i].Split(new string[] { "*****" }, StringSplitOptions.None);
                    lbl[i].Content = str[0];
                    lbl[i].Height = 25;
                    lbl[i].Width = 600;
                    lbl[i].SetValue(Canvas.LeftProperty, 80.0);
                    lbl[i].SetValue(Canvas.TopProperty, 10.0 + ((i + 1) * 30));
                    cnvMain.Children.Add(lbl[i]);

                    txt[i] = new TextBox();

                    //string[] str = strOpt[i].Split(new string[] { "*****" }, StringSplitOptions.None);
                    txt[i].Text = str[0];
                    txt[i].Tag = str[1];

                    txt[i].Height = 25;
                    txt[i].Width = 400;
                    txt[i].SetValue(Canvas.LeftProperty, 280.0);
                    txt[i].SetValue(Canvas.TopProperty, 10.0 + ((i + 1) * 30));
                    cnvMain.Children.Add(txt[i]);
                }
            }

            
            cnvMain.Children.Add(lblHeader);
            currentQuestion++;
        exit: ;
        }
            catch (Exception exp)
            {
                exp.Data.Add("My Key", "FncNextQuestion()--:--clsQuestion.xaml.cs--:--" + exp.Message + " :--:--");
                //ClsException.LogError(ex);
                //ClsException.WriteToErrorLogFile(ex);
                System.Text.StringBuilder sb = new StringBuilder();
                sb.AppendLine(exp.Message);
                sb.AppendLine();
                sb.AppendLine("StackTrace : " + exp.StackTrace);
                sb.AppendLine();
                sb.AppendLine("Location : " + exp.Data["My Key"].ToString());
                sb.AppendLine();
                sb1 = CreateTressInfo();
                sb.Append(sb1.ToString());
                VMuktiAPI.ClsLogging.WriteToTresslog(sb);
            }
        }
        private void btnNextQue_Click(object sender, RoutedEventArgs e)
        {
            try
            {
            foreach (object o in cnvMain.Children)
            {
                if (o.GetType() == typeof(CheckBox))
                {
                    if (((CheckBox)o).IsChecked == true)
                    {
                        StartQuestion = ((CheckBox)o).Tag.ToString();
                        break;
                    }
                }
                else if (o.GetType() == typeof(RadioButton))
                {
                    if (((RadioButton)o).IsChecked == true)
                    {
                        StartQuestion = ((RadioButton)o).Tag.ToString();
                        break;
                    }
                }

                else if (o.GetType() == typeof(TextBox))
                {
                    StartQuestion = ((TextBox)o).Tag.ToString();
                    break;
                }

                else if (o.GetType() == typeof(ListBox))
                {
                    for (int i = 0; i < ((ListBox)o).Items.Count; i++)
                    {
                        if (((ListBoxItem)((ListBox)o).Items[i]).IsSelected == true)
                        {
                            StartQuestion = ((ListBoxItem)((ListBox)o).Items[i]).Tag.ToString();
                        }
                    }
                }

                else if (o.GetType() == typeof(ComboBox))
                {
                    for (int i = 0; i < ((ComboBox)o).Items.Count; i++)
                    {
                        if (((ComboBoxItem)((ComboBox)o).Items[i]).IsSelected == true)
                        {
                            StartQuestion = ((ComboBoxItem)((ComboBox)o).Items[i]).Tag.ToString();
                        }
                    }
                }

            }

            FncNextQuestion();
        }
            catch (Exception exp)
            {
                exp.Data.Add("My Key", "btnNextQue_Click()--:--clsQuestion.xaml.cs--:--" + exp.Message + " :--:--");
                //ClsException.LogError(ex);
                //ClsException.WriteToErrorLogFile(ex);
                System.Text.StringBuilder sb = new StringBuilder();
                sb.AppendLine(exp.Message);
                sb.AppendLine();
                sb.AppendLine("StackTrace : " + exp.StackTrace);
                sb.AppendLine();
                sb.AppendLine("Location : " + exp.Data["My Key"].ToString());
                sb.AppendLine();
                sb1 = CreateTressInfo();
                sb.Append(sb1.ToString());
                VMuktiAPI.ClsLogging.WriteToTresslog(sb);
            }
        }
        private void btnStart_Click(object sender, RoutedEventArgs e)
        {
            try
            {
            StartQuestion = txtHeader.Text;
        }
            catch (Exception exp)
            {
                exp.Data.Add("My Key", "btnStart_Click()--:--clsQuestion.xaml.cs--:--" + exp.Message + " :--:--");
                //ClsException.LogError(ex);
                //ClsException.WriteToErrorLogFile(ex);
                System.Text.StringBuilder sb = new StringBuilder();
                sb.AppendLine(exp.Message);
                sb.AppendLine();
                sb.AppendLine("StackTrace : " + exp.StackTrace);
                sb.AppendLine();
                sb.AppendLine("Location : " + exp.Data["My Key"].ToString());
                sb.AppendLine();
                sb1 = CreateTressInfo();
                sb.Append(sb1.ToString());
                VMuktiAPI.ClsLogging.WriteToTresslog(sb);
            }
        }
    }
}
