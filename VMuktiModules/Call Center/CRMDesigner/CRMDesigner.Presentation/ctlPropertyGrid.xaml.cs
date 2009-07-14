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
using System.Reflection;

namespace CRMDesigner.Presentation
{
    /// <summary>
    /// Interaction logic for ctlPropertyGrid.xaml
    /// </summary>
    public partial class ctlPropertyGrid : UserControl
    {
        public static StringBuilder sb1;
        private object _ControlToBind;

        private int _EventTrack;

        public object ControlToBind
        {
            get
            {
                return _ControlToBind;

                if (_ControlToBind.GetType() == typeof(ctlPOD))
                {
                    ((ctlPOD)_ControlToBind).Height = double.Parse(txtHeight.Text);
                    ((ctlPOD)_ControlToBind).Width = double.Parse(txtWidth.Text);
                    ((ctlPOD)_ControlToBind).SetValue(Canvas.LeftProperty, float.Parse(txtLeft.Text));
                    ((ctlPOD)_ControlToBind).SetValue(Canvas.TopProperty, float.Parse(txtTop.Text));
                }

            }
            set
            {
                _ControlToBind = value;
                if (_ControlToBind.GetType() == typeof(ctlPOD))
                {
                    txtHeight.Text = ((ctlPOD)_ControlToBind).Height.ToString();
                    txtWidth.Text = ((ctlPOD)_ControlToBind).Width.ToString();
                    txtLeft.Text = ((ctlPOD)_ControlToBind).GetValue(Canvas.LeftProperty).ToString();
                    txtTop.Text = ((ctlPOD)_ControlToBind).GetValue(Canvas.TopProperty).ToString();

                    //VMuktiAPI.VMuktiInfo.CurrentPeer.FName;

                    foreach (object o in ((ctlPOD)_ControlToBind).cnvPOD.Children)
                    {
                        if (o.GetType() == typeof(Button))
                        {
                            txtContent.Text = ((Button)o).Content.ToString();
                            txtBThick.IsEnabled = true;
                            txtBBrush.IsEnabled = true;
                            btnBorderColor.IsEnabled = true;
                        }
                        else if (o.GetType() == typeof(TextBox))
                        {
                            txtContent.Text = ((TextBox)o).Text;
                            txtBThick.IsEnabled = true;
                            txtBBrush.IsEnabled = true;
                            btnBorderColor.IsEnabled = true;
                        }
                        else if (o.GetType() == typeof(TextBlock))
                        {
                            txtContent.Text = ((TextBlock)o).Text;
                            txtBThick.IsEnabled = false;
                            txtBBrush.IsEnabled = false;
                            btnBorderColor.IsEnabled = false;
                        }
                        else if (o.GetType() == typeof(Label))
                        {
                            txtContent.Text = ((Label)o).Content.ToString();
                            txtBThick.IsEnabled = true;
                            txtBBrush.IsEnabled = true;
                            btnBorderColor.IsEnabled = true;
                        }
                        else if (o.GetType() == typeof(CheckBox))
                        {
                            txtContent.Text = ((CheckBox)o).Content.ToString();
                            txtBThick.IsEnabled = false;
                            txtBBrush.IsEnabled = false;
                            btnBorderColor.IsEnabled = false;
                        }
                        else if (o.GetType() == typeof(RadioButton))
                        {
                            txtContent.Text = ((RadioButton)o).Content.ToString();
                            txtBThick.IsEnabled = false;
                            txtBBrush.IsEnabled = false;
                            btnBorderColor.IsEnabled = false;
                        }
                        else
                        {
                            txtBThick.IsEnabled = false;
                            txtBBrush.IsEnabled = false;
                            btnBorderColor.IsEnabled = false;
                        }
                    }

                    //txtContent = "Text";
                }
            }
        }

        public int EventTrack
        {
            get
            {
                return _EventTrack;
            }
            set
            {
                _EventTrack = value;
            }
        }

        string colorType = "";

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

        public ctlPropertyGrid()
        {
            try
            {
            InitializeComponent();
            txtHeight.TextChanged += new TextChangedEventHandler(txtHeight_TextChanged);
            txtWidth.TextChanged += new TextChangedEventHandler(txtWidth_TextChanged);
            txtTop.TextChanged += new TextChangedEventHandler(txtTop_TextChanged);
            txtLeft.TextChanged += new TextChangedEventHandler(txtLeft_TextChanged);
            txtContent.TextChanged += new TextChangedEventHandler(txtContent_TextChanged);
            txtBThick.TextChanged += new TextChangedEventHandler(txtBThick_TextChanged);
            txtBThick.KeyDown += new KeyEventHandler(txtBThick_KeyDown);
            txtBThick.LostFocus += new RoutedEventHandler(txtBThick_LostFocus);
            txtForeColor.TextChanged += new TextChangedEventHandler(txtForeColor_TextChanged);
            txtBackColor.TextChanged += new TextChangedEventHandler(txtBackColor_TextChanged);

            cmbAlign.SelectionChanged += new SelectionChangedEventHandler(cmbAlign_SelectionChanged);
            cmbAlign.MouseDown += new MouseButtonEventHandler(cmbAlign_MouseDown);

            ugColor.Visibility = Visibility.Collapsed;
            ugColor.Background = System.Windows.Media.Brushes.White;

            #region Create My Uniform Grid

            this.ugTryColor.Children.Clear();

            PropertyInfo[] props = typeof(System.Windows.Media.Brushes).GetProperties(BindingFlags.Public |
                                                  BindingFlags.Static);
            // Create individual items
            foreach (PropertyInfo p in props)
            {
                Button b = new Button();
                b.Background = (SolidColorBrush)p.GetValue(null, null);
                b.Foreground = System.Windows.Media.Brushes.Transparent;
                b.BorderBrush = System.Windows.Media.Brushes.Black;
                b.BorderThickness = new Thickness(1, 1, 1, 1);
                b.Click += new RoutedEventHandler(b_Click);
                this.ugTryColor.Children.Add(b);
            }

            myExpander.Expanded += new RoutedEventHandler(myExpander_Expanded);
            myExpander.Collapsed += new RoutedEventHandler(myExpander_Collapsed);
            #endregion

            // Disabling some TextBoxes //


        }
            catch (Exception exp)
            {
                exp.Data.Add("My Key", "ctlPropertyGrid()--:--ctlpropertyGrid.xaml.cs--:--" + exp.Message + " :--:--");
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
        void cmbAlign_MouseDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
            if (((ComboBoxItem)cmbAlign.SelectedItem).Content.ToString().ToLower() == "center")
            {
                foreach (object o in ((ctlPOD)_ControlToBind).cnvPOD.Children)
                {
                    if (o.GetType() == typeof(Button))
                    {
                        ((Button)o).HorizontalContentAlignment = HorizontalAlignment.Center;
                    }
                    else if (o.GetType() == typeof(TextBox))
                    {
                        ((TextBox)o).HorizontalContentAlignment = HorizontalAlignment.Center;
                    }
                    else if (o.GetType() == typeof(TextBlock))
                    {
                        ((TextBlock)o).TextAlignment = TextAlignment.Center;
                    }
                    else if (o.GetType() == typeof(Label))
                    {
                        ((Label)o).HorizontalContentAlignment = HorizontalAlignment.Center;
                    }
                    else if (o.GetType() == typeof(ComboBox))
                    {
                        ((ComboBox)o).HorizontalContentAlignment = HorizontalAlignment.Center;
                    }
                    else if (o.GetType() == typeof(ListBox))
                    {
                        ((ListBox)o).HorizontalContentAlignment = HorizontalAlignment.Center;
                    }
                    else if (o.GetType() == typeof(RadioButton))
                    {
                        ((RadioButton)o).HorizontalContentAlignment = HorizontalAlignment.Center;
                    }
                    else if (o.GetType() == typeof(CheckBox))
                    {
                        ((CheckBox)o).HorizontalContentAlignment = HorizontalAlignment.Center;
                    }
                }
            }

            else if (((ComboBoxItem)cmbAlign.SelectedItem).Content.ToString().ToLower() == "left")
            {
                foreach (object o in ((ctlPOD)_ControlToBind).cnvPOD.Children)
                {
                    if (o.GetType() == typeof(Button))
                    {
                        ((Button)o).HorizontalContentAlignment = HorizontalAlignment.Left;
                    }
                    else if (o.GetType() == typeof(TextBox))
                    {
                        ((TextBox)o).HorizontalContentAlignment = HorizontalAlignment.Left;
                    }
                    else if (o.GetType() == typeof(TextBlock))
                    {
                        ((TextBlock)o).TextAlignment = TextAlignment.Left;
                    }
                    else if (o.GetType() == typeof(Label))
                    {
                        ((Label)o).HorizontalContentAlignment = HorizontalAlignment.Left;
                    }
                    else if (o.GetType() == typeof(ComboBox))
                    {
                        ((ComboBox)o).HorizontalContentAlignment = HorizontalAlignment.Left;
                    }
                    else if (o.GetType() == typeof(ListBox))
                    {
                        ((ListBox)o).HorizontalContentAlignment = HorizontalAlignment.Left;
                    }
                    else if (o.GetType() == typeof(RadioButton))
                    {
                        ((RadioButton)o).HorizontalContentAlignment = HorizontalAlignment.Left;
                    }
                    else if (o.GetType() == typeof(CheckBox))
                    {
                        ((CheckBox)o).HorizontalContentAlignment = HorizontalAlignment.Left;
                    }
                }
            }

            else if (((ComboBoxItem)cmbAlign.SelectedItem).Content.ToString().ToLower() == "right")
            {
                foreach (object o in ((ctlPOD)_ControlToBind).cnvPOD.Children)
                {
                    if (o.GetType() == typeof(Button))
                    {
                        ((Button)o).HorizontalContentAlignment = HorizontalAlignment.Right;
                    }
                    else if (o.GetType() == typeof(TextBox))
                    {
                        ((TextBox)o).HorizontalContentAlignment = HorizontalAlignment.Right;
                    }
                    else if (o.GetType() == typeof(TextBlock))
                    {
                        ((TextBlock)o).TextAlignment = TextAlignment.Right;
                    }
                    else if (o.GetType() == typeof(Label))
                    {
                        ((Label)o).HorizontalContentAlignment = HorizontalAlignment.Right;
                    }
                    else if (o.GetType() == typeof(ComboBox))
                    {
                        ((ComboBox)o).HorizontalContentAlignment = HorizontalAlignment.Right;
                    }
                    else if (o.GetType() == typeof(ListBox))
                    {
                        ((ListBox)o).HorizontalContentAlignment = HorizontalAlignment.Right;
                    }
                    else if (o.GetType() == typeof(RadioButton))
                    {
                        ((RadioButton)o).HorizontalContentAlignment = HorizontalAlignment.Right;
                    }
                    else if (o.GetType() == typeof(CheckBox))
                    {
                        ((CheckBox)o).HorizontalContentAlignment = HorizontalAlignment.Right;
                    }
                }
            }
        }
            catch (Exception exp)
            {
                exp.Data.Add("My Key", "cmbAlign_MouseDown()--:--ctlpropertyGrid.xaml.cs--:--" + exp.Message + " :--:--");
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
        void txtBThick_LostFocus(object sender, RoutedEventArgs e)
        {
            
            if (int.Parse(txtBThick.Text) > 5)
            {
                txtBThick.Text = "5";
            }
            try
            {
                foreach (object o in ((ctlPOD)_ControlToBind).cnvPOD.Children)
                {
                    if (o.GetType() == typeof(Button))
                    {
                        ((Button)o).BorderThickness = new Thickness(double.Parse(txtBThick.Text));
                    }
                    else if (o.GetType() == typeof(TextBox))
                    {
                        ((TextBox)o).BorderThickness = new Thickness(double.Parse(txtBThick.Text));
                    }
                    else if (o.GetType() == typeof(Label))
                    {
                        ((Label)o).BorderThickness = new Thickness(double.Parse(txtBThick.Text));
                    }

                }
            }
            catch (Exception exp)
            {
                exp.Data.Add("My Key", "txtBThick_LostFocus()--:--ctlpropertyGrid.xaml.cs--:--" + exp.Message + " :--:--");
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

        void txtBThick_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
            #region Allowing only numbers and Tab

            if (!((e.Key >= Key.D0 && e.Key <= Key.D9) ||
                (e.Key == Key.Decimal) ||
                e.Key == Key.Tab ||
                 (e.Key >= Key.NumPad0 && e.Key <= Key.NumPad9)))
            {
                e.Handled = true;
            }

            #endregion
        }
            catch (Exception exp)
            {
                exp.Data.Add("My Key", "txtBThick_KeyDown()--:--ctlpropertyGrid.xaml.cs--:--" + exp.Message + " :--:--");
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
        void txtBThick_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {

        }
            catch (Exception exp)
            {
                exp.Data.Add("My Key", "txtBThick_TextChanged()--:--ctlpropertyGrid.xaml.cs--:--" + exp.Message + " :--:--");
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
        void myExpander_Collapsed(object sender, RoutedEventArgs e)
        {
            try
            {
            colorType = "";
            EventTrack = 1;
        }
            catch (Exception exp)
            {
                exp.Data.Add("My Key", "myExpander_Collapsed()--:--ctlpropertyGrid.xaml.cs--:--" + exp.Message + " :--:--");
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
        void myExpander_Expanded(object sender, RoutedEventArgs e)
        {
            try
            {
            colorType = "CANVAS";
            EventTrack = 0;
        }
            catch (Exception exp)
            {
                exp.Data.Add("My Key", "myExpander_Expanded()--:--ctlpropertyGrid.xaml.cs--:--" + exp.Message + " :--:--");
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
        void cmbAlign_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
            if (((ComboBoxItem)cmbAlign.SelectedItem).Content.ToString().ToLower() == "center")
            {
                foreach (object o in ((ctlPOD)_ControlToBind).cnvPOD.Children)
                {
                    if (o.GetType() == typeof(Button))
                    {
                        ((Button)o).HorizontalContentAlignment = HorizontalAlignment.Center;
                    }
                    else if (o.GetType() == typeof(TextBox))
                    {
                        ((TextBox)o).HorizontalContentAlignment = HorizontalAlignment.Center;
                    }
                    else if (o.GetType() == typeof(TextBlock))
                    {
                        ((TextBlock)o).TextAlignment = TextAlignment.Center;
                    }
                    else if (o.GetType() == typeof(Label))
                    {
                        ((Label)o).HorizontalContentAlignment = HorizontalAlignment.Center;
                    }
                    else if (o.GetType() == typeof(ComboBox))
                    {
                        ((ComboBox)o).HorizontalContentAlignment = HorizontalAlignment.Center;
                    }
                    else if (o.GetType() == typeof(ListBox))
                    {
                        ((ListBox)o).HorizontalContentAlignment = HorizontalAlignment.Center;
                    }
                    else if (o.GetType() == typeof(RadioButton))
                    {
                        ((RadioButton)o).HorizontalContentAlignment = HorizontalAlignment.Center;
                    }
                    else if (o.GetType() == typeof(CheckBox))
                    {
                        ((CheckBox)o).HorizontalContentAlignment = HorizontalAlignment.Center;
                    }
                }
            }

            else if (((ComboBoxItem)cmbAlign.SelectedItem).Content.ToString().ToLower() == "left")
            {
                foreach (object o in ((ctlPOD)_ControlToBind).cnvPOD.Children)
                {
                    if (o.GetType() == typeof(Button))
                    {
                        ((Button)o).HorizontalContentAlignment = HorizontalAlignment.Left;
                    }
                    else if (o.GetType() == typeof(TextBox))
                    {
                        ((TextBox)o).HorizontalContentAlignment = HorizontalAlignment.Left;
                    }
                    else if (o.GetType() == typeof(TextBlock))
                    {
                        ((TextBlock)o).TextAlignment = TextAlignment.Left;
                    }
                    else if (o.GetType() == typeof(Label))
                    {
                        ((Label)o).HorizontalContentAlignment = HorizontalAlignment.Left;
                    }
                    else if (o.GetType() == typeof(ComboBox))
                    {
                        ((ComboBox)o).HorizontalContentAlignment = HorizontalAlignment.Left;
                    }
                    else if (o.GetType() == typeof(ListBox))
                    {
                        ((ListBox)o).HorizontalContentAlignment = HorizontalAlignment.Left;
                    }
                    else if (o.GetType() == typeof(RadioButton))
                    {
                        ((RadioButton)o).HorizontalContentAlignment = HorizontalAlignment.Left;
                    }
                    else if (o.GetType() == typeof(CheckBox))
                    {
                        ((CheckBox)o).HorizontalContentAlignment = HorizontalAlignment.Left;
                    }
                }
            }

            else if (((ComboBoxItem)cmbAlign.SelectedItem).Content.ToString().ToLower() == "right")
            {
                foreach (object o in ((ctlPOD)_ControlToBind).cnvPOD.Children)
                {
                    if (o.GetType() == typeof(Button))
                    {
                        ((Button)o).HorizontalContentAlignment = HorizontalAlignment.Right;
                    }
                    else if (o.GetType() == typeof(TextBox))
                    {
                        ((TextBox)o).HorizontalContentAlignment = HorizontalAlignment.Right;
                    }
                    else if (o.GetType() == typeof(TextBlock))
                    {
                        ((TextBlock)o).TextAlignment = TextAlignment.Right;
                    }
                    else if (o.GetType() == typeof(Label))
                    {
                        ((Label)o).HorizontalContentAlignment = HorizontalAlignment.Right;
                    }
                    else if (o.GetType() == typeof(ComboBox))
                    {
                        ((ComboBox)o).HorizontalContentAlignment = HorizontalAlignment.Right;
                    }
                    else if (o.GetType() == typeof(ListBox))
                    {
                        ((ListBox)o).HorizontalContentAlignment = HorizontalAlignment.Right;
                    }
                    else if (o.GetType() == typeof(RadioButton))
                    {
                        ((RadioButton)o).HorizontalContentAlignment = HorizontalAlignment.Right;
                    }
                    else if (o.GetType() == typeof(CheckBox))
                    {
                        ((CheckBox)o).HorizontalContentAlignment = HorizontalAlignment.Right;
                    }
                }
            }
        }
            catch (Exception exp)
            {
                exp.Data.Add("My Key", "cmbAlign_SelectionChanged()--:--ctlpropertyGrid.xaml.cs--:--" + exp.Message + " :--:--");
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
        void txtContent_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
            foreach (object o in ((ctlPOD)_ControlToBind).cnvPOD.Children)
            {
                if (o.GetType() == typeof(Button))
                {
                    ((Button)o).Content = txtContent.Text;
                }
                else if (o.GetType() == typeof(TextBox))
                {
                    ((TextBox)o).Text = txtContent.Text;
                }
                else if (o.GetType() == typeof(TextBlock))
                {
                    ((TextBlock)o).Text = txtContent.Text;
                }
                else if (o.GetType() == typeof(Label))
                {
                    ((Label)o).Content = txtContent.Text;
                }
                else if (o.GetType() == typeof(RadioButton))
                {
                    ((RadioButton)o).Content = txtContent.Text;
                }
                else if (o.GetType() == typeof(CheckBox))
                {
                    ((CheckBox)o).Content = txtContent.Text;
                }

            }

            //if (txtLeft.Text.Trim() != "")
            //    ((ctlPOD)_ControlToBind).SetValue(Canvas.LeftProperty, double.Parse(txtLeft.Text));
            //else
            //    ((ctlPOD)_ControlToBind).SetValue(Canvas.LeftProperty, 0.0);
        }
            catch (Exception exp)
            {
                exp.Data.Add("My Key", "txtContent_TextChanged()--:--ctlpropertyGrid.xaml.cs--:--" + exp.Message + " :--:--");
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
        void txtBackColor_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                //b.SetValue(Color.c, txtBackColor.Text);
                //ChangeColor("BACK",);
            }
            catch (Exception exp)
            {
                exp.Data.Add("My Key", "txtBackColor_TextChanged()--:--ctlpropertyGrid.xaml.cs--:--" + exp.Message + " :--:--");
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

        void txtForeColor_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                //ChangeColor("FORE");
            }
            catch (Exception exp)
            {
                exp.Data.Add("My Key", "txtForeColor_TextChanged()--:--ctlpropertyGrid.xaml.cs--:--" + exp.Message + " :--:--");
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

            //((ctlPOD)_ControlToBind).Background = txtForeColor.Text;

        }
        void txtLeft_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                if (txtLeft.Text.Trim() != "")
                    ((ctlPOD)_ControlToBind).SetValue(Canvas.LeftProperty, double.Parse(txtLeft.Text));
                else
                    ((ctlPOD)_ControlToBind).SetValue(Canvas.LeftProperty, 0.0);
            }
            catch (Exception exp)
            {
                exp.Data.Add("My Key", "txtLeft_TextChanged()--:--ctlpropertyGrid.xaml.cs--:--" + exp.Message + " :--:--");
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
        void txtTop_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
            if (txtTop.Text.Trim() != "")
                ((ctlPOD)_ControlToBind).SetValue(Canvas.TopProperty, double.Parse(txtTop.Text));
            else
                ((ctlPOD)_ControlToBind).SetValue(Canvas.TopProperty, 0.0);
        }
            catch (Exception exp)
            {
                exp.Data.Add("My Key", "txtTop_TextChanged()--:--ctlpropertyGrid.xaml.cs--:--" + exp.Message + " :--:--");
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
        void txtWidth_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
            if (txtWidth.Text.Trim() != "")
                ((ctlPOD)_ControlToBind).Width = double.Parse(txtWidth.Text);
            else
                ((ctlPOD)_ControlToBind).Width = 0.0;
        }
            catch (Exception exp)
            {
                exp.Data.Add("My Key", "txtWidth_TextChanged()--:--ctlpropertyGrid.xaml.cs--:--" + exp.Message + " :--:--");
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

        void txtHeight_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
            if (txtHeight.Text.Trim() != "")
                ((ctlPOD)_ControlToBind).Height = double.Parse(txtHeight.Text);
            else
                ((ctlPOD)_ControlToBind).Height = 0.0;
        }
            catch (Exception exp)
            {
                exp.Data.Add("My Key", "txtHeight_TextChanged()--:--ctlpropertyGrid.xaml.cs--:--" + exp.Message + " :--:--");
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

        private void btnForeColor_Click(object sender, RoutedEventArgs e)
        {
            try
            {
            colorType = "FORE";
            ugColor.Visibility = Visibility.Visible;
            createGridOfColor();
        }
            catch (Exception exp)
            {
                exp.Data.Add("My Key", "btnForeColor_Click()--:--ctlpropertyGrid.xaml.cs--:--" + exp.Message + " :--:--");
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
        private void btnBorderColor_Click(object sender, RoutedEventArgs e)
        {
            try
            {
            colorType = "BORDER";
            ugColor.Visibility = Visibility.Visible;
            createGridOfColor();
        }
            catch (Exception exp)
            {
                exp.Data.Add("My Key", "btnBorderColor_Click()--:--ctlpropertyGrid.xaml.cs--:--" + exp.Message + " :--:--");
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
        private void createGridOfColor()
        {
            try
            {
            this.ugColor.Children.Clear();

            PropertyInfo[] props = typeof(System.Windows.Media.Brushes).GetProperties(BindingFlags.Public |
                                                  BindingFlags.Static);
            // Create individual items
            foreach (PropertyInfo p in props)
            {
                Button b = new Button();
                b.Background = (SolidColorBrush)p.GetValue(null, null);
                b.Foreground = System.Windows.Media.Brushes.Transparent;
                b.BorderBrush = System.Windows.Media.Brushes.Black;
                b.BorderThickness = new Thickness(1, 1, 1, 1);
                b.Click += new RoutedEventHandler(b_Click);
                this.ugColor.Children.Add(b);
            }
        }
            catch (Exception exp)
            {
                exp.Data.Add("My Key", "createGridOfColor()--:--ctlpropertyGrid.xaml.cs--:--" + exp.Message + " :--:--");
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
        void b_Click(object sender, RoutedEventArgs e)
        {
            try
            {
            ugColor.Visibility = Visibility.Collapsed;

            if (colorType == "BACK")
            {
                ChangeColor("BACK", sender);
                txtBackColor.Text = ((Button)sender).Background.ToString();
            }
            else if (colorType == "FORE")
            {
                ChangeColor("FORE", sender);
                txtForeColor.Text = ((Button)sender).Background.ToString();
            }

            else if (colorType == "BORDER")
            {
                ChangeColor("BORDER", sender);
                txtBBrush.Text = ((Button)sender).Background.ToString();
            }

            else if (colorType == "CANVAS")
            {
                ChangeColor("CANVAS", sender);
                //txtBackColor.Text = ((Button)sender).Background.ToString();
            }

        }
            catch (Exception exp)
            {
                exp.Data.Add("My Key", "b_Click()--:--ctlpropertyGrid.xaml.cs--:--" + exp.Message + " :--:--");
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
        void ChangeColor(string str, object sender)
        {
            try
            {
            if (str == "CANVAS")
            {
                ((Canvas)((Expander)(this.Parent)).Parent).Background = ((Button)sender).Background;
                myExpander.Background = ((Button)sender).Background;
            }
            else
            {
                foreach (object o in ((ctlPOD)_ControlToBind).cnvPOD.Children)
                {
                    if (o.GetType() == typeof(Button))
                    {
                        if (str == "FORE")
                            ((Button)o).Foreground = ((Button)sender).Background;
                        else if (str == "BORDER")
                            ((Button)o).BorderBrush = ((Button)sender).Background;
                        else
                            ((Button)o).Background = ((Button)sender).Background;
                    }
                    else if (o.GetType() == typeof(TextBox))
                    {
                        if (str == "FORE")
                            ((TextBox)o).Foreground = ((Button)sender).Background;
                        else if (str == "BORDER")
                            ((TextBox)o).BorderBrush = ((Button)sender).Background;
                        else
                            ((TextBox)o).Background = ((Button)sender).Background;
                    }
                    else if (o.GetType() == typeof(TextBlock))
                    {
                        if (str == "FORE")
                            ((TextBlock)o).Foreground = ((Button)sender).Background;
                        else if (str == "BORDER")
                        { }
                        else
                            ((TextBlock)o).Background = ((Button)sender).Background;
                    }
                    else if (o.GetType() == typeof(Label))
                    {
                        if (str == "FORE")
                            ((Label)o).Foreground = ((Button)sender).Background;
                        else if (str == "BORDER")
                            ((Label)o).BorderBrush = ((Button)sender).Background;
                        else
                            ((Label)o).Background = ((Button)sender).Background;
                    }
                    else if (o.GetType() == typeof(ComboBox))
                    {
                        if (str == "FORE")
                            ((ComboBox)o).Foreground = ((Button)sender).Background;
                        else if (str == "BORDER")
                        { }
                        else
                            ((ComboBox)o).Background = ((Button)sender).Background;
                    }
                    else if (o.GetType() == typeof(ListBox))
                    {
                        if (str == "FORE")
                            ((ListBox)o).Foreground = ((Button)sender).Background;
                        else
                            ((ListBox)o).Background = ((Button)sender).Background;
                    }
                    else if (o.GetType() == typeof(RadioButton))
                    {
                        if (str == "FORE")
                            ((RadioButton)o).Foreground = ((Button)sender).Background;
                        else if (str == "BORDER")
                        { }
                        else
                            ((RadioButton)o).Background = ((Button)sender).Background;

                    }
                    else if (o.GetType() == typeof(CheckBox))
                    {
                        if (str == "FORE")
                            ((CheckBox)o).Foreground = ((Button)sender).Background;
                        else if (str == "BORDER")
                        { }
                        else
                            ((CheckBox)o).Background = ((Button)sender).Background;
                    }
                }

            }

        }
            catch (Exception exp)
            {
                exp.Data.Add("My Key", "ChangeColor()--:--ctlpropertyGrid.xaml.cs--:--" + exp.Message + " :--:--");
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
        private void btnBackColor_Click(object sender, RoutedEventArgs e)
        {
            try
            {
            colorType = "BACK";
            ugColor.Visibility = Visibility.Visible;
            createGridOfColor();
        }
            catch (Exception exp)
            {
                exp.Data.Add("My Key", "btnBackColor_Click()--:--ctlpropertyGrid.xaml.cs--:--" + exp.Message + " :--:--");
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
        private void btnFont_Click(object sender, RoutedEventArgs e)
        {
            try
            {
            System.Windows.Forms.FontDialog f = new System.Windows.Forms.FontDialog();
            System.Windows.Forms.DialogResult dr = f.ShowDialog();
            if (dr == System.Windows.Forms.DialogResult.OK)
            {
                //FontConverter F = new FontConverter();
                //F.

                foreach (object o in ((ctlPOD)_ControlToBind).cnvPOD.Children)
                {
                    if (o.GetType() == typeof(Button))
                    {
                        ((Button)o).FontFamily = new FontFamily(f.Font.Name);
                        ((Button)o).FontSize = f.Font.Size;
                        if (f.Font.Bold)
                            ((Button)o).FontWeight = FontWeights.Bold;
                        if (f.Font.Italic)
                            ((Button)o).FontStyle = FontStyles.Italic;

                    }
                    else if (o.GetType() == typeof(TextBox))
                    {
                        ((TextBox)o).FontFamily = new FontFamily(f.Font.Name);
                        ((TextBox)o).FontSize = f.Font.Size;
                        if (f.Font.Bold)
                            ((TextBox)o).FontWeight = FontWeights.Bold;
                        if (f.Font.Italic)
                            ((TextBox)o).FontStyle = FontStyles.Italic;
                    }
                    else if (o.GetType() == typeof(TextBlock))
                    {
                        ((TextBlock)o).FontFamily = new FontFamily(f.Font.Name);
                        ((TextBlock)o).FontSize = f.Font.Size;
                        if (f.Font.Bold)
                            ((TextBlock)o).FontWeight = FontWeights.Bold;
                        if (f.Font.Italic)
                            ((TextBlock)o).FontStyle = FontStyles.Italic;
                    }
                    else if (o.GetType() == typeof(Label))
                    {
                        ((Label)o).FontFamily = new FontFamily(f.Font.Name);
                        ((Label)o).FontSize = f.Font.Size;
                        if (f.Font.Bold)
                            ((Label)o).FontWeight = FontWeights.Bold;
                        if (f.Font.Italic)
                            ((Label)o).FontStyle = FontStyles.Italic;
                    }

                    else if (o.GetType() == typeof(ComboBox))
                    {
                        ((ComboBox)o).FontFamily = new FontFamily(f.Font.Name);
                        ((ComboBox)o).FontSize = f.Font.Size;
                        if (f.Font.Bold)
                            ((ComboBox)o).FontWeight = FontWeights.Bold;
                        if (f.Font.Italic)
                            ((ComboBox)o).FontStyle = FontStyles.Italic;
                    }

                    else if (o.GetType() == typeof(ListBox))
                    {
                        ((ListBox)o).FontFamily = new FontFamily(f.Font.Name);
                        ((ListBox)o).FontSize = f.Font.Size;
                        if (f.Font.Bold)
                            ((ListBox)o).FontWeight = FontWeights.Bold;
                        if (f.Font.Italic)
                            ((ListBox)o).FontStyle = FontStyles.Italic;
                    }

                    else if (o.GetType() == typeof(RadioButton))
                    {
                        ((RadioButton)o).FontFamily = new FontFamily(f.Font.Name);
                        ((RadioButton)o).FontSize = f.Font.Size;
                        if (f.Font.Bold)
                            ((RadioButton)o).FontWeight = FontWeights.Bold;
                        if (f.Font.Italic)
                            ((RadioButton)o).FontStyle = FontStyles.Italic;
                    }

                    else if (o.GetType() == typeof(CheckBox))
                    {
                        ((CheckBox)o).FontFamily = new FontFamily(f.Font.Name);
                        ((CheckBox)o).FontSize = f.Font.Size;
                        if (f.Font.Bold)
                            ((CheckBox)o).FontWeight = FontWeights.Bold;
                        if (f.Font.Italic)
                            ((CheckBox)o).FontStyle = FontStyles.Italic;
                    }
                }

                // btnBackColor.FontFamily = new FontFamily(f.Font.Name);
                //btnBackColor.FontSize = f.Font.Size;
                //btnBackColor.FontFamily = f.Font.Name;
                //btnBackColor.FontFamily = f.Font.Name;

            }
        }
            catch (Exception exp)
            {
                exp.Data.Add("My Key", "btnFont_Click()--:--ctlpropertyGrid.xaml.cs--:--" + exp.Message + " :--:--");
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