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
using System.Collections;
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
    /// Interaction logic for CtlDynamicScript.xaml
    /// </summary>
    public partial class CtlDynamicScript : System.Windows.Controls.UserControl
    {
        public static StringBuilder sb1;
        int CurrentQueCount = 0;
        int varTop;
        ClsQuestionCollectionR objQueCollection;
        string varType;
        double varHeight;

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

        public CtlDynamicScript()
        {
            try
            {
            InitializeComponent();
            btnScript.Click += new RoutedEventHandler(btnScript_Click);

            Button btnYes = new Button();
            btnYes.Content = "Next";
            btnYes.Height = 30;
            btnYes.Width = 50;
            btnYes.Click += new RoutedEventHandler(btnYes_Click);
            Canvas.SetLeft(btnYes, 50);
            Canvas.SetTop(btnYes, 400);
            cnvMain.Children.Add(btnYes);

        }
            catch (Exception exp)
            {
                exp.Data.Add("My Key", "CtlDynamicScript()--:--CtlDynamicScript.xaml.cs--:--" + exp.Message + " :--:--");
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
        void btnScript_Click(object sender, RoutedEventArgs e)
        {
            try
            {
            ClsQuestionCollectionR objQueCollection = ClsQuestionCollectionR.GetAll(int.Parse(txtScript.Text));
            if (objQueCollection.Count > 0)
            {
                FncSetInnerCanvas();
            }
        }
            catch (Exception exp)
            {
                exp.Data.Add("My Key", "btnScript_Click()--:--CtlDynamicScript.xaml.cs--:--" + exp.Message + " :--:--");
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
        void FncSetInnerCanvas()
        {
            try
            {
            //cnvPaint.Height = 280;
            //cnvPaint.Width = 700;

            Label lblHeader = new Label();
            lblHeader.Content = objQueCollection[CurrentQueCount].QuestionName;
            lblHeader.SizeChanged += new SizeChangedEventHandler(lblQuestion_SizeChanged);
            lblHeader.FontSize = 19;
            cnvPaint.Children.Add(lblHeader);

            Label lblQuestion = new Label();
            lblQuestion.Content = objQueCollection[CurrentQueCount].QuestionText;
            lblQuestion.FontSize = 17;
            lblQuestion.SizeChanged += new SizeChangedEventHandler(lblQuestion_SizeChanged);

            Canvas.SetTop(lblQuestion, 60);
            cnvPaint.Children.Add(lblQuestion);

            ClsOptionCollection objOptCollection = ClsOptionCollection.GetAll(objQueCollection[CurrentQueCount].ID);

            varType = objQueCollection[CurrentQueCount].Category;

            if (varType == "CheckBox")
            {
                for (int i = 0; i < objOptCollection.Count; i++)
                {
                    CheckBox chk = new CheckBox();
                    chk.Content = objOptCollection[i].Options;
                    chk.Tag = objOptCollection[i].ActionQueueID.ToString() + "," + objOptCollection[i].ID.ToString();
                    chk.SizeChanged += new SizeChangedEventHandler(lblQuestion_SizeChanged);
                    chk.Height = 18;
                    chk.FontSize = 14;
                    Canvas.SetLeft(chk, 25);
                    varTop = 120 + (30 * i);
                    Canvas.SetTop(chk, varTop);
                    cnvPaint.Children.Add(chk);
                }
            }
            else if (varType == "RadioButton")
            {
                for (int i = 0; i < objOptCollection.Count; i++)
                {
                    RadioButton chk = new RadioButton();
                    chk.Content = objOptCollection[i].Options;
                    chk.Tag = objOptCollection[i].ActionQueueID.ToString() + "," + objOptCollection[i].ID.ToString();
                    chk.SizeChanged += new SizeChangedEventHandler(lblQuestion_SizeChanged);
                    chk.Height = 18;
                    chk.FontSize = 14;
                    Canvas.SetLeft(chk, 25);
                    varTop = 120 + (30 * i);
                    Canvas.SetTop(chk, varTop);
                    cnvPaint.Children.Add(chk);
                }
            }

            cnvPaint.Height = varTop + 50;
            sv.ScrollToTop();
        }
            catch (Exception exp)
            {
                exp.Data.Add("My Key", "FncSetInnerCanvas()--:--CtlDynamicScript.xaml.cs--:--" + exp.Message + " :--:--");
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
        void lblQuestion_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            try
            {
            if (varHeight < 70 + e.NewSize.Width)
            {
                cnvPaint.Width = 70 + e.NewSize.Width;
                varHeight = 70 + e.NewSize.Width;
            }
        }
            catch (Exception exp)
            {
                exp.Data.Add("My Key", "lblQuestion_SizeChanged()--:--CtlDynamicScript.xaml.cs--:--" + exp.Message + " :--:--");
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
        void btnYes_Click(object sender, RoutedEventArgs e)
        {
            try
            {
            ClsQuestionCollectionR objQueCollection = ClsQuestionCollectionR.GetAll(int.Parse(txtScript.Text));

            if (((Button)sender).Tag.ToString().ToLower() == "radiobutton")
            {
                foreach (object r in cnvPaint.Children)
                {
                    if (r.GetType() == typeof(RadioButton))
                    {
                        if (((RadioButton)r).IsChecked == true)
                        {
                            string[] str = ((RadioButton)r).Tag.ToString().Split(',');
                            CurrentQueCount = int.Parse(str[0]);
                            int optID = int.Parse(str[1]);
                            //MessageBox.Show("Option ID = " + optID.ToString());

                            //ClsAnswerR objAns = new ClsAnswerR();
                            //objAns.CallID = int.Parse(txtCall.Text.ToString());
                            //objAns.QusOptionID = int.Parse(optID.ToString());
                            //objAns.Save();

                            for (int k = 0; k < objQueCollection.Count; k++)
                            {
                                if (objQueCollection[k].ID == CurrentQueCount)
                                {
                                    //this.NavigationService.Navigate(new Uri(objQueCollection[k].QuestionName + ".Xaml" , UriKind.RelativeOrAbsolute));
                                    break;
                                }
                            }

                        }
                    }
                }
            }

            else if (varType == "CheckBox")
            {
                foreach (object r in cnvPaint.Children)
                {
                    if (r.GetType() == typeof(CheckBox))
                    {
                        if (((CheckBox)r).IsChecked == true)
                        {
                            string[] str = ((CheckBox)r).Tag.ToString().Split(',');
                            CurrentQueCount = int.Parse(str[0]);
                            int optID = int.Parse(str[1]);
                            //MessageBox.Show("Option ID = " + optID.ToString());

                            //ClsAnswerR objAns = new ClsAnswerR();
                            //objAns.CallID = int.Parse(txtCall.Text.ToString());
                            //objAns.QusOptionID = int.Parse(optID.ToString());
                            //objAns.Save();

                            for (int k = 0; k < objQueCollection.Count; k++)
                            {
                                if (objQueCollection[k].ID == CurrentQueCount)
                                {
                                    //this.NavigationService.Navigate(new Uri(objQueCollection[k].QuestionName + ".Xaml", UriKind.RelativeOrAbsolute));
                                    break;
                                }
                            }
                        }
                    }
                }
            }

            else if (((Button)sender).Tag.ToString().ToLower() == "listbox")
            {
                foreach (object r in cnvPaint.Children)
                {
                    if (r.GetType() == typeof(ListBox))
                    {
                        for (int i = 0; i < ((ListBox)r).Items.Count; i++)
                        {
                            if (((ListBoxItem)((ListBox)r).Items[i]).IsSelected == true)
                            {
                                string[] str = ((ListBoxItem)r).Tag.ToString().Split(',');
                                int CurrentQueCount = int.Parse(str[0]);
                                int optID = int.Parse(str[1]);
                                //MessageBox.Show("Option ID = " + optID.ToString());

                                //ClsAnswerR objAns = new ClsAnswerR();
                                //objAns.CallID = int.Parse(txtCall.Text.ToString());
                                //objAns.QusOptionID = int.Parse(optID.ToString());
                                //objAns.Save();

                                for (int k = 0; k < objQueCollection.Count; k++)
                                {
                                    if (objQueCollection[k].ID == CurrentQueCount)
                                    {
                                        //this.NavigationService.Navigate(new Uri(objQueCollection[k].QuestionName + ".Xaml", UriKind.RelativeOrAbsolute));
                                        break;
                                    }
                                }
                            }
                        }
                    }
                }
            }

            else if (((Button)sender).Tag.ToString().ToLower() == "combobox")
            {
                foreach (object r in cnvPaint.Children)
                {
                    if (r.GetType() == typeof(ComboBox))
                    {
                        for (int i = 0; i < ((ComboBox)r).Items.Count; i++)
                        {
                            if (((ComboBoxItem)((ComboBox)r).Items[i]).IsSelected == true)
                            {
                                string[] str = ((ComboBoxItem)r).Tag.ToString().Split(',');
                                int CurrentQueCount = int.Parse(str[0]);
                                int optID = int.Parse(str[1]);
                                //MessageBox.Show("Option ID = " + optID.ToString());

                                //ClsAnswerR objAns = new ClsAnswerR();
                                //objAns.CallID = int.Parse(txtCall.Text.ToString());
                                //objAns.QusOptionID = int.Parse(optID.ToString());
                                //objAns.Save();

                                for (int k = 0; k < objQueCollection.Count; k++)
                                {
                                    if (objQueCollection[k].ID == CurrentQueCount)
                                    {
                                        //this.NavigationService.Navigate(new Uri(objQueCollection[k].QuestionName + ".Xaml", UriKind.RelativeOrAbsolute));
                                        break;
                                    }
                                }
                            }
                        }
                    }
                }
            }

        }
            catch (Exception exp)
            {
                exp.Data.Add("My Key", "btnYes_Click()--:--CtlDynamicScript.xaml.cs--:--" + exp.Message + " :--:--");
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
