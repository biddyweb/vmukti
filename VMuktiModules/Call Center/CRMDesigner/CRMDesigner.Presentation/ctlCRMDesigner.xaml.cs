
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
using System.IO;
using System.Diagnostics;
using System.Reflection;
using System.Net;
using System.DirectoryServices;
using VMuktiService;
using VMuktiAPI;

namespace CRMDesigner.Presentation
{
    /// <summary>
    /// Interaction logic for UserControl1.xaml
    /// </summary>

    //public enum ResizeOption
    //{
    //    LeftEdge = 0,
    //    RightEdge = 1,
    //    UpEdge = 2,
    //    DownEdge = 3,
    //}

    public enum ModulePermissions
    {
        View = 0
    }
    public partial class ctlCRMDesigner : UserControl
    {
        public static StringBuilder sb1;
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
        int newDrag = 1;
        int pageCount = 0;
        Point PrePoint = new Point();

        //ClsControlInfoCollection objControlInfoCollection = new ClsControlInfoCollection();

        // Raxit Code //

        ctlPOD currentControl = null;
        ctlPropertyGrid MyPropGrid = null;
        Rectangle r1 = new Rectangle();
        List<Canvas> lstCanvas = new List<Canvas>() { };
        /////
        Button btnNext = new Button();
        Button btnPrev = new Button();
        Button btnStartQue = new Button();
        string startQuestion = "";
        Int64 startQuestionID = 0;

        string AddRef1_2 = "";
        string AddRef2_3 = "";

        int CurrentQueCount = 0;
        int GeneratedQuestions = 0;

        double varTop;
        ClsQuestionCollectionR objQueCollection;
        string varType;
        double varHeight;
        ModulePermissions[] _MyPermissions;

        public static IHTTPFileTransferService clientHttpFileTransfer = null;
        Canvas cnvPaint = new Canvas();
        List<TabItem> tbiLst = new List<TabItem>();
    

        public ctlCRMDesigner(ModulePermissions[] MyPermissions)
        {
            try
            {
                InitializeComponent();
                this.Loaded += new RoutedEventHandler(ctlCRMDesigner_Loaded);

                _MyPermissions = MyPermissions;
                FncPermissionsReview();
                currentControl = new ctlPOD();
                MyPropGrid = new ctlPropertyGrid();

                FncFillCombo();
                //cmbScript.SelectionChanged += new SelectionChangedEventHandler(cmbScript_SelectionChanged);


                txtDrag999.PreviewMouseDown += new MouseButtonEventHandler(txtDrag_PreviewMouseDown);
                lblDrag999.PreviewMouseDown += new MouseButtonEventHandler(lblDrag_PreviewMouseDown);

                btnNewTab.Click += new RoutedEventHandler(btnNewTab_Click);
                cnvPaint.Drop += new DragEventHandler(cnvPaint_Drop);
                //tbi.Drop += new DragEventHandler(tbi_Drop);
                cnvPaint.DragOver += new DragEventHandler(cnvPaint_DragOver);
                cnvPaint.PreviewMouseUp += new MouseButtonEventHandler(ctlCRMDesigner_PreviewMouseUp);
                cnvPaint.PreviewMouseDown += new MouseButtonEventHandler(ctlCRMDesigner_PreviewMouseDown);
                cnvPaint.PreviewMouseMove += new MouseEventHandler(ctlCRMDesigner_MouseMove);
                cnvPaint.MouseMove += new MouseEventHandler(cnvPaint_MouseMove);
                currentControl.SizeChanged += new SizeChangedEventHandler(currentControl_SizeChanged);

                cmbLeadFormat.SelectionChanged += new SelectionChangedEventHandler(cmbLeadFormat_SelectionChanged);

                r1.Stroke = Brushes.White;

                r1.StrokeThickness = 3.0;
                r1.Fill = Brushes.Transparent;
                r1.StrokeDashArray = new DoubleCollection(2);
                MyPropGrid.Height = 500;
                MyPropGrid.Width = 200;
                MyPropGrid.SetValue(Canvas.LeftProperty, 650.0);
                MyPropGrid.SetValue(Canvas.TopProperty, 0.0);
                myExpander1.Content = MyPropGrid;
                myExpander1.Expanded += new RoutedEventHandler(myExpander1_Expanded);
                myExpander1.Collapsed += new RoutedEventHandler(myExpander1_Collapsed);
                //cnvPaint.Children.Add(MyPropGrid);
                cnvPaint.Children.Add(r1);

                r1.SizeChanged += new SizeChangedEventHandler(r1_SizeChanged);

                this.PreviewKeyDown += new KeyEventHandler(ctlCRMDesigner_PreviewKeyDown);

                FncCreateTabItems();

                txtHeight.KeyDown += new KeyEventHandler(txtHeight_KeyDown);
                txtWidth.KeyDown += new KeyEventHandler(txtWidth_KeyDown);

                txtHeight.LostFocus += new RoutedEventHandler(txtHeight_LostFocus);
                txtWidth.LostFocus += new RoutedEventHandler(txtWidth_LostFocus);
                txtHeader.GotFocus += new RoutedEventHandler(txtHeader_GotFocus);

                txtHeight.Text = int.Parse(tbcMain.Height.ToString()).ToString();
                txtWidth.Text = int.Parse(tbcMain.Width.ToString()).ToString();

                ClsCRMCollection objClsCRMCollection = ClsCRMCollection.GetAll();
                for (int i = 0; i < objClsCRMCollection.Count; i++)
                {
                    ComboBoxItem cmb1 = new ComboBoxItem();
                    cmb1.Content = objClsCRMCollection[i].CRMName;
                    cmb1.Tag = objClsCRMCollection[i].ID;
                    cmbCRM.Items.Add(cmb1);
                }
                try
                {

                    BasicHttpClient bhcFts = new BasicHttpClient();
                    clientHttpFileTransfer = (IHTTPFileTransferService)bhcFts.OpenClient<IHTTPFileTransferService>("http://" + VMuktiAPI.VMuktiInfo.BootStrapIPs[0] + ":80/VMukti/HttpFileTransferService");
                    clientHttpFileTransfer.svcHTTPFileTransferServiceJoin();
                }
                catch (Exception ex)
                {
                    ex.Data.Add("My Key", "VMukti--:--VmuktiModules--:--Call Center--:--CRMDesigner--:--CRMDesigner.Presentation--:--ctlCRMDesigner.xaml.cs--:--ctlCRMDesigner()--");
                    //ClsException.LogError(ex);
                    //ClsException.WriteToErrorLogFile(ex);
                    System.Text.StringBuilder sb = new StringBuilder();
                    sb.AppendLine(ex.Message);
                    sb.AppendLine();
                    sb.AppendLine("StackTrace : " + ex.StackTrace);
                    sb.AppendLine();
                    sb.AppendLine("Location : " + ex.Data["My Key"].ToString());
                    sb.AppendLine();
                    sb1 = CreateTressInfo();
                    sb.Append(sb1.ToString());
                    VMuktiAPI.ClsLogging.WriteToTresslog(sb);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        void ctlCRMDesigner_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                this.Width = ((Grid)this.Parent).ActualWidth;
                ((Grid)this.Parent).SizeChanged += new SizeChangedEventHandler(ctlCRMDesigner_SizeChanged);
            }
            catch (Exception ex)
            {
                ex.Data.Add("My Key", "VMukti--:--VmuktiModules--:--Call Center--:--CRMDesigner--:--CRMDesigner.Presentation--:--ctlCRMDesigner.xaml.cs--:--ctlCRMDesigner()--");
                //ClsException.LogError(ex);
                //ClsException.WriteToErrorLogFile(ex);
            }
        }

        void ctlCRMDesigner_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            this.Width = e.NewSize.Width;
        }

        void txtHeader_GotFocus(object sender, RoutedEventArgs e)
        {
            try
            {
                txtHeader.Text = ((TabItem)tbcMain.SelectedItem).Header.ToString();
            }
            catch (Exception ex)
            {
                ex.Data.Add("My Key", "VMukti--:--VmuktiModules--:--Call Center--:--CRMDesigner--:--CRMDesigner.Presentation--:--ctlCRMDesigner.xaml.cs--:--txtHeader_GotFocus()--");
                //ClsException.LogError(ex);
                //ClsException.WriteToErrorLogFile(ex);
                System.Text.StringBuilder sb = new StringBuilder();
                sb.AppendLine(ex.Message);
                sb.AppendLine();
                sb.AppendLine("StackTrace : " + ex.StackTrace);
                sb.AppendLine();
                sb.AppendLine("Location : " + ex.Data["My Key"].ToString());
                sb.AppendLine();
                sb1 = CreateTressInfo();
                sb.Append(sb1.ToString());
                VMuktiAPI.ClsLogging.WriteToTresslog(sb);
            }
        }

        void txtWidth_LostFocus(object sender, RoutedEventArgs e)
        {
            try
            {
                if (txtWidth.Text.Trim() == "" || int.Parse(txtWidth.Text) < 50)
                {
                    txtWidth.Text = "50";
                }

                tbcMain.Width = double.Parse(txtWidth.Text.ToString());
                for (int i = 0; i < tbiLst.Count; i++)
                {
                    //((TabItem)tbiLst[i]).Height = double.Parse(txtHeight.Text.ToString());
                    ((Canvas)((TabItem)tbiLst[i]).Content).Width = double.Parse(txtWidth.Text.ToString());
                    //tbcMain.Items.Add(tbiLst[i]);
                }
            }
            catch (Exception ex)
            {
                ex.Data.Add("My Key", "VMukti--:--VmuktiModules--:--Call Center--:--CRMDesigner--:--CRMDesigner.Presentation--:--ctlCRMDesigner.xaml.cs--:--txtWidth_LostFocus()--");
                //ClsException.LogError(ex);
                //ClsException.WriteToErrorLogFile(ex);
                System.Text.StringBuilder sb = new StringBuilder();
                sb.AppendLine(ex.Message);
                sb.AppendLine();
                sb.AppendLine("StackTrace : " + ex.StackTrace);
                sb.AppendLine();
                sb.AppendLine("Location : " + ex.Data["My Key"].ToString());
                sb.AppendLine();
                sb1 = CreateTressInfo();
                sb.Append(sb1.ToString());
                VMuktiAPI.ClsLogging.WriteToTresslog(sb);
            }
        }

        void txtHeight_LostFocus(object sender, RoutedEventArgs e)
        {
            try
            {
                if (txtHeight.Text.Trim() == "" || int.Parse(txtHeight.Text) < 50)
                {
                    txtHeight.Text = "50";
                }

                tbcMain.Height = double.Parse(txtHeight.Text.ToString());
                for (int i = 0; i < tbiLst.Count; i++)
                {
                    //((TabItem)tbiLst[i]).Height = double.Parse(txtHeight.Text.ToString());
                    ((Canvas)((TabItem)tbiLst[i]).Content).Height = double.Parse(txtHeight.Text.ToString());
                    //tbcMain.Items.Add(tbiLst[i]);
                }
            }
            catch (Exception ex)
            {
                ex.Data.Add("My Key", "VMukti--:--VmuktiModules--:--Call Center--:--CRMDesigner--:--CRMDesigner.Presentation--:--ctlCRMDesigner.xaml.cs--:--txtHeight_LostFocus()--");
                //ClsException.LogError(ex);
                //ClsException.WriteToErrorLogFile(ex);
                System.Text.StringBuilder sb = new StringBuilder();
                sb.AppendLine(ex.Message);
                sb.AppendLine();
                sb.AppendLine("StackTrace : " + ex.StackTrace);
                sb.AppendLine();
                sb.AppendLine("Location : " + ex.Data["My Key"].ToString());
                sb.AppendLine();
                sb1 = CreateTressInfo();
                sb.Append(sb1.ToString());
                VMuktiAPI.ClsLogging.WriteToTresslog(sb);
            }
        }

        void txtWidth_KeyDown(object sender, KeyEventArgs e)
        {
           
            
            #region Allowing only numbers and Tab

            try
            {
                if (!((e.Key >= Key.D0 && e.Key <= Key.D9) ||
                    (e.Key == Key.Decimal) ||
                    e.Key == Key.Tab ||
                     (e.Key >= Key.NumPad0 && e.Key <= Key.NumPad9)))
                {
                    e.Handled = true;
                }
            }
            catch (Exception ex)
            {
                ex.Data.Add("My Key", "VMukti--:--VmuktiModules--:--Call Center--:--CRMDesigner--:--CRMDesigner.Presentation--:--ctlCRMDesigner.xaml.cs--:--txtWidth_KeyDown()--");
                //ClsException.LogError(ex);
                //ClsException.WriteToErrorLogFile(ex);
                System.Text.StringBuilder sb = new StringBuilder();
                sb.AppendLine(ex.Message);
                sb.AppendLine();
                sb.AppendLine("StackTrace : " + ex.StackTrace);
                sb.AppendLine();
                sb.AppendLine("Location : " + ex.Data["My Key"].ToString());
                sb.AppendLine();
                sb1 = CreateTressInfo();
                sb.Append(sb1.ToString());
                VMuktiAPI.ClsLogging.WriteToTresslog(sb);
            }

            #endregion
        }

        void txtHeight_KeyDown(object sender, KeyEventArgs e)
        {
            #region Allowing only numbers and Tab
            try
            {
                if (!((e.Key >= Key.D0 && e.Key <= Key.D9) ||
                    (e.Key == Key.Decimal) ||
                    e.Key == Key.Tab ||
                     (e.Key >= Key.NumPad0 && e.Key <= Key.NumPad9)))
                {
                    e.Handled = true;
                }
            }
            catch (Exception ex)
            {
                ex.Data.Add("My Key", "VMukti--:--VmuktiModules--:--Call Center--:--CRMDesigner--:--CRMDesigner.Presentation--:--ctlCRMDesigner.xaml.cs--:--txtHeight_KeyDown()--");
                //ClsException.LogError(ex);
                //ClsException.WriteToErrorLogFile(ex);
                System.Text.StringBuilder sb = new StringBuilder();
                sb.AppendLine(ex.Message);
                sb.AppendLine();
                sb.AppendLine("StackTrace : " + ex.StackTrace);
                sb.AppendLine();
                sb.AppendLine("Location : " + ex.Data["My Key"].ToString());
                sb.AppendLine();
                sb1 = CreateTressInfo();
                sb.Append(sb1.ToString());
                VMuktiAPI.ClsLogging.WriteToTresslog(sb);
            }
            #endregion
        }

        void btnNewTab_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                TabItem tabTi = new TabItem();
                tabTi.Tag = "1";
                tabTi.Header = "New Tab";
                //cnvPaint.Background = Brushes.Red;
                tabTi.GotFocus += new RoutedEventHandler(tabTi_GotFocus);

                Canvas cnv = new Canvas();
                cnv.Height = 500;
                cnv.Width = 500;
                //cnv.Background = Brushes.Purple;
                UIElement[] uiArray = new UIElement[cnvPaint.Children.Count];
                cnvPaint.Children.CopyTo(uiArray, 0);

                cnvPaint.Children.Clear();
                for (int j = 0; j < uiArray.Length; j++)
                {
                    cnv.Children.Add(uiArray[j]);
                }
                cnvPaint.Children.Clear();
                ((TabItem)cnvPaint.Parent).Content = cnv;
                tabTi.Content = cnvPaint;

                tbiLst.Add(tabTi);

                FncRefreshProperits();
                txtHeader.Text = ((TabItem)tbcMain.SelectedItem).Header.ToString();
            }
            catch (Exception ex)
            {
                ex.Data.Add("My Key", "VMukti--:--VmuktiModules--:--Call Center--:--CRMDesigner--:--CRMDesigner.Presentation--:--ctlCRMDesigner.xaml.cs--:--btnNewTab_Click()--");
                //ClsException.LogError(ex);
                //ClsException.WriteToErrorLogFile(ex);
                System.Text.StringBuilder sb = new StringBuilder();
                sb.AppendLine(ex.Message);
                sb.AppendLine();
                sb.AppendLine("StackTrace : " + ex.StackTrace);
                sb.AppendLine();
                sb.AppendLine("Location : " + ex.Data["My Key"].ToString());
                sb.AppendLine();
                sb1 = CreateTressInfo();
                sb.Append(sb1.ToString());
                VMuktiAPI.ClsLogging.WriteToTresslog(sb);
            }
        }
        
        void FncRefreshProperits()
        {
            try
            {
                tbcMain.Items.Clear();
                for (int i = 0; i < tbiLst.Count; i++)
                {
                    tbcMain.Items.Add(tbiLst[i]);
                }
                ((TabItem)tbcMain.Items[tbcMain.Items.Count - 1]).Focus();
            }
            catch (Exception ex)
            {
                ex.Data.Add("My Key", "VMukti--:--VmuktiModules--:--Call Center--:--CRMDesigner--:--CRMDesigner.Presentation--:--ctlCRMDesigner.xaml.cs--:--FncRefreshProperits()--");
                //ClsException.LogError(ex);
                //ClsException.WriteToErrorLogFile(ex);
                System.Text.StringBuilder sb = new StringBuilder();
                sb.AppendLine(ex.Message);
                sb.AppendLine();
                sb.AppendLine("StackTrace : " + ex.StackTrace);
                sb.AppendLine();
                sb.AppendLine("Location : " + ex.Data["My Key"].ToString());
                sb.AppendLine();
                sb1 = CreateTressInfo();
                sb.Append(sb1.ToString());
                VMuktiAPI.ClsLogging.WriteToTresslog(sb);
            }
            //txtHeader.Text = ((TabItem)tbcMain.Items[tbcMain.Items.Count-1]).Header.ToString();
        }
        
        void FncCreateTabItems()
        {
            try
            {
                TabItem tabTi = new TabItem();
                tabTi.Tag = "1";
                tabTi.Header = "New Tab";
                cnvPaint.Background = Brushes.Transparent;
                tabTi.GotFocus += new RoutedEventHandler(tabTi_GotFocus);
                tabTi.Content = cnvPaint;
                tbiLst.Add(tabTi);

                FncRefreshProperits();
            }
            catch (Exception ex)
            {
                ex.Data.Add("My Key", "VMukti--:--VmuktiModules--:--Call Center--:--CRMDesigner--:--CRMDesigner.Presentation--:--ctlCRMDesigner.xaml.cs--:--FncCreateTabItems()--");
                //ClsException.LogError(ex);
                //ClsException.WriteToErrorLogFile(ex);
                System.Text.StringBuilder sb = new StringBuilder();
                sb.AppendLine(ex.Message);
                sb.AppendLine();
                sb.AppendLine("StackTrace : " + ex.StackTrace);
                sb.AppendLine();
                sb.AppendLine("Location : " + ex.Data["My Key"].ToString());
                sb.AppendLine();
                sb1 = CreateTressInfo();
                sb.Append(sb1.ToString());
                VMuktiAPI.ClsLogging.WriteToTresslog(sb);
            }
        }

       

        void tabTi1_GotFocus(object sender, RoutedEventArgs e)
        {
            try
            {
                FncRefreshProperits();
            }
            catch (Exception ex)
            {
                ex.Data.Add("My Key", "VMukti--:--VmuktiModules--:--Call Center--:--CRMDesigner--:--CRMDesigner.Presentation--:--ctlCRMDesigner.xaml.cs--:--tabTi1_GotFocus()--");
                //ClsException.LogError(ex);
                //ClsException.WriteToErrorLogFile(ex);
                System.Text.StringBuilder sb = new StringBuilder();
                sb.AppendLine(ex.Message);
                sb.AppendLine();
                sb.AppendLine("StackTrace : " + ex.StackTrace);
                sb.AppendLine();
                sb.AppendLine("Location : " + ex.Data["My Key"].ToString());
                sb.AppendLine();
                sb1 = CreateTressInfo();
                sb.Append(sb1.ToString());
                VMuktiAPI.ClsLogging.WriteToTresslog(sb);
            }
            
        }

        void tabTi_GotFocus(object sender, RoutedEventArgs e)
        {
        
        //Saving the Old Tabitem Children 
            Canvas cnv = new Canvas();
            cnv.Height = 500;
            cnv.Width = 500;
            UIElement[] uiArray = new UIElement[cnvPaint.Children.Count];
            cnvPaint.Children.CopyTo(uiArray, 0);

            cnvPaint.Children.Clear();
            for (int j = 0; j < uiArray.Length; j++)
            {
                cnv.Children.Add(uiArray[j]);
            }
            cnvPaint.Children.Clear();
            ((TabItem)cnvPaint.Parent).Content = cnv;
            
            // Assigning New Tabitem Children to cnvPaint

            //Canvas cnv1 = new Canvas();
            //cnv1.Height = 500;
            //cnv1.Width = 500;
            //cnv1.Background = Brushes.Purple;
            UIElement[] uiArray1 = new UIElement[((Canvas)((TabItem)sender).Content).Children.Count];
            ((Canvas)((TabItem)sender).Content).Children.CopyTo(uiArray1, 0);

            ((Canvas)((TabItem)sender).Content).Children.Clear();
            for (int j = 0; j < uiArray1.Length; j++)
            {
                cnvPaint.Children.Add(uiArray1[j]);
            }
            //cnvPaint.Children.Clear();
            
            
            ((TabItem)sender).Content = cnvPaint;
            try{
                txtHeader.Text = ((TabItem)tbcMain.SelectedItem).Header.ToString();
            }
            catch (Exception ex)
            {
                ex.Data.Add("My Key", "VMukti--:--VmuktiModules--:--Call Center--:--CRMDesigner--:--CRMDesigner.Presentation--:--ctlCRMDesigner.xaml.cs--:--tabTi_GotFocus()--");
                //ClsException.LogError(ex);
                //ClsException.WriteToErrorLogFile(ex);
                System.Text.StringBuilder sb = new StringBuilder();
                sb.AppendLine(ex.Message);
                sb.AppendLine();
                sb.AppendLine("StackTrace : " + ex.StackTrace);
                sb.AppendLine();
                sb.AppendLine("Location : " + ex.Data["My Key"].ToString());
                sb.AppendLine();
                sb1 = CreateTressInfo();
                sb.Append(sb1.ToString());
                VMuktiAPI.ClsLogging.WriteToTresslog(sb);
            }
            //cnvPaint = (Canvas)((TabItem)sender).Content;
        }

        void tbi_Drop(object sender, DragEventArgs e)
        {
            try
            {

                if (e.Data.GetData(typeof(Label)) != null)
                {
                    if (((Canvas)((Label)e.Data.GetData(typeof(Label))).Parent).Name.ToString() == "cnvControls")
                    {
                        Point p = e.GetPosition((IInputElement)cnvPaint);
                        Label lbl = new Label();
                        lbl.Content = "Label";
                        lbl.HorizontalContentAlignment = HorizontalAlignment.Center;
                        lbl.Height = 25;
                        lbl.Width = 100;
                        lbl.PreviewMouseDown += new MouseButtonEventHandler(lblDrag_PreviewMouseDown);
                        lbl.SetValue(Canvas.LeftProperty, 10.0);
                        lbl.SetValue(Canvas.TopProperty, 10.0);

                        ctlPOD objPOD = new ctlPOD();
                        objPOD.cnvPOD.Children.Add(lbl);
                        objPOD.AllowDrop = true;
                        objPOD.Height = 25;
                        objPOD.Width = 100;
                        objPOD.SetValue(Canvas.LeftProperty, p.X);
                        objPOD.SetValue(Canvas.TopProperty, p.Y);
                        MyPropGrid.ControlToBind = objPOD;
                        objPOD.KeyDown += new KeyEventHandler(objPOD_KeyDown);
                        objPOD.PreviewMouseDown += new MouseButtonEventHandler(objPOD_PreviewMouseDown);
                        currentControl = objPOD;
                        cnvPaint.Children.Add(objPOD);
                    }

                    else if ((((Canvas)((Label)e.Data.GetData(typeof(Label))).Parent).Parent).GetType() == typeof(ctlPOD))
                    {
                        Point p = e.GetPosition((IInputElement)cnvPaint);
                        ((Canvas)((Label)e.Data.GetData(typeof(Label))).Parent).Parent.SetValue(Canvas.LeftProperty, p.X - PrePoint.X);
                        ((Canvas)((Label)e.Data.GetData(typeof(Label))).Parent).Parent.SetValue(Canvas.TopProperty, p.Y - PrePoint.Y);

                    }
                }

                else if (e.Data.GetData(typeof(TextBox)) != null)
                {
                    if (((Canvas)((TextBox)e.Data.GetData(typeof(TextBox))).Parent).Name.ToString() == "cnvControls")
                    {
                        Point p = e.GetPosition((IInputElement)cnvPaint);
                        TextBox txt = new TextBox();
                        txt.IsReadOnly = true;
                        txt.Cursor = Cursors.Arrow;
                        txt.Height = 25;
                        txt.Width = 100;
                        txt.Text = "TextBox";
                        txt.PreviewMouseDown += new MouseButtonEventHandler(txtDrag_PreviewMouseDown);
                        txt.SetValue(Canvas.LeftProperty, 10.0);
                        txt.SetValue(Canvas.TopProperty, 10.0);

                        ctlPOD objPOD = new ctlPOD();
                        objPOD.AllowDrop = true;
                        objPOD.Height = 25;
                        objPOD.Width = 100;
                        objPOD.PreviewMouseDown += new MouseButtonEventHandler(objPOD_PreviewMouseDown);
                        objPOD.SetValue(Canvas.LeftProperty, p.X);
                        objPOD.SetValue(Canvas.TopProperty, p.Y);
                        MyPropGrid.ControlToBind = objPOD;
                        objPOD.cnvPOD.Children.Add(txt);
                        currentControl = objPOD;
                        cnvPaint.Children.Add(objPOD);
                    }
                    else if ((((Canvas)((TextBox)e.Data.GetData(typeof(TextBox))).Parent).Parent).GetType() == typeof(ctlPOD))
                    {
                        Point p = e.GetPosition((IInputElement)cnvPaint);
                        ((Canvas)((TextBox)e.Data.GetData(typeof(TextBox))).Parent).Parent.SetValue(Canvas.LeftProperty, p.X - PrePoint.X);
                        ((Canvas)((TextBox)e.Data.GetData(typeof(TextBox))).Parent).Parent.SetValue(Canvas.TopProperty, p.Y - PrePoint.Y);
                    }
                }



                newDrag = 1;
                r1.Visibility = Visibility.Collapsed;
                MyPropGrid.ControlToBind = currentControl;
            }

            catch (Exception ex)
            {
                ex.Data.Add("My Key", "VMukti--:--VmuktiModules--:--Call Center--:--CRMDesigner--:--CRMDesigner.Presentation--:--ctlCRMDesigner.xaml.cs--:--tbi_Drop()--");
                //ClsException.LogError(ex);
                //ClsException.WriteToErrorLogFile(ex);
                System.Text.StringBuilder sb = new StringBuilder();
                sb.AppendLine(ex.Message);
                sb.AppendLine();
                sb.AppendLine("StackTrace : " + ex.StackTrace);
                sb.AppendLine();
                sb.AppendLine("Location : " + ex.Data["My Key"].ToString());
                sb.AppendLine();
                sb1 = CreateTressInfo();
                sb.Append(sb1.ToString());
                VMuktiAPI.ClsLogging.WriteToTresslog(sb);
            }
        }

       
        void FncPermissionsReview()
        {
            try
            {
                //this.Visibility = Visibility.Hidden;

                for (int i = 0; i < _MyPermissions.Length; i++)
                {
                    if (_MyPermissions[i] == ModulePermissions.View)
                    {
                        this.Visibility = Visibility.Visible;
                    }
                }
            }
            catch (Exception ex)
            {
                ex.Data.Add("My Key", "VMukti--:--VmuktiModules--:--Call Center--:--CRMDesigner--:--CRMDesigner.Presentation--:--ctlCRMDesigner.xaml.cs--:--FncPermissionsReview()--");
                //ClsException.LogError(ex);
                //ClsException.WriteToErrorLogFile(ex);
                System.Text.StringBuilder sb = new StringBuilder();
                sb.AppendLine(ex.Message);
                sb.AppendLine();
                sb.AppendLine("StackTrace : " + ex.StackTrace);
                sb.AppendLine();
                sb.AppendLine("Location : " + ex.Data["My Key"].ToString());
                sb.AppendLine();
                sb1 = CreateTressInfo();
                sb.Append(sb1.ToString());
                VMuktiAPI.ClsLogging.WriteToTresslog(sb);
            }
        }
        void cmbLeadFormat_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                Int64 FormatID = Int64.Parse(((ComboBoxItem)cmbLeadFormat.SelectedItem).Tag.ToString());

                cmbFileds.Items.Clear();
                ClsLeadFormatCollection objLeadFormatCollection = ClsLeadFormatCollection.GetAll(FormatID);

                for (int i = 0; i < objLeadFormatCollection.Count; i++)
                {
                    ComboBoxItem cbi = new ComboBoxItem();
                    cbi.Content = objLeadFormatCollection[i].FormatName;
                    cbi.Tag = objLeadFormatCollection[i].ID;
                    cmbFileds.Items.Add(cbi);
                }
            }

            catch (Exception ex)
            {
                ex.Data.Add("My Key", "VMukti--:--VmuktiModules--:--Call Center--:--CRMDesigner--:--CRMDesigner.Presentation--:--ctlCRMDesigner.xaml.cs--:--cmbLeadFormat_SelectionChanged()--");
                //ClsException.LogError(ex);
                //ClsException.WriteToErrorLogFile(ex);
                System.Text.StringBuilder sb = new StringBuilder();
                sb.AppendLine(ex.Message);
                sb.AppendLine();
                sb.AppendLine("StackTrace : " + ex.StackTrace);
                sb.AppendLine();
                sb.AppendLine("Location : " + ex.Data["My Key"].ToString());
                sb.AppendLine();
                sb1 = CreateTressInfo();
                sb.Append(sb1.ToString());
                VMuktiAPI.ClsLogging.WriteToTresslog(sb);
            }
        }

        void cmbScript_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
            }

            catch (Exception ex)
            {
                ex.Data.Add("My Key", "VMukti--:--VmuktiModules--:--Call Center--:--CRMDesigner--:--CRMDesigner.Presentation--:--ctlCRMDesigner.xaml.cs--:--cmbScript_SelectionChanged()--");
                //ClsException.LogError(ex);
                //ClsException.WriteToErrorLogFile(ex);
                System.Text.StringBuilder sb = new StringBuilder();
                sb.AppendLine(ex.Message);
                sb.AppendLine();
                sb.AppendLine("StackTrace : " + ex.StackTrace);
                sb.AppendLine();
                sb.AppendLine("Location : " + ex.Data["My Key"].ToString());
                sb.AppendLine();
                sb1 = CreateTressInfo();
                sb.Append(sb1.ToString());
                VMuktiAPI.ClsLogging.WriteToTresslog(sb);
            }
        }

        void FncFillCombo()
        {
            try
            {
                //cmbScript.Items.Clear();
                ClsScriptCollection objScriptCollection = ClsScriptCollection.GetAll();

                for (int i = 0; i < objScriptCollection.Count; i++)
                {
                    ComboBoxItem cbi = new ComboBoxItem();
                    cbi.Content = objScriptCollection[i].ScriptName;
                    cbi.Tag = objScriptCollection[i].ID;
                    //cmbScript.Items.Add(cbi);
                }


                cmbLeadFormat.Items.Clear();
                ClsLeadFormatCollection objLeadFormatCollection = ClsLeadFormatCollection.GetAll();

                for (int i = 0; i < objLeadFormatCollection.Count; i++)
                {
                    ComboBoxItem cbi = new ComboBoxItem();
                    cbi.Content = objLeadFormatCollection[i].FormatName;
                    cbi.Tag = objLeadFormatCollection[i].ID;
                    cmbLeadFormat.Items.Add(cbi);
                }
            }

            catch (Exception ex)
            {
                ex.Data.Add("My Key", "VMukti--:--VmuktiModules--:--Call Center--:--CRMDesigner--:--CRMDesigner.Presentation--:--ctlCRMDesigner.xaml.cs--:--FncFillCombo()--");
                //ClsException.LogError(ex);
                //ClsException.WriteToErrorLogFile(ex);
                System.Text.StringBuilder sb = new StringBuilder();
                sb.AppendLine(ex.Message);
                sb.AppendLine();
                sb.AppendLine("StackTrace : " + ex.StackTrace);
                sb.AppendLine();
                sb.AppendLine("Location : " + ex.Data["My Key"].ToString());
                sb.AppendLine();
                sb1 = CreateTressInfo();
                sb.Append(sb1.ToString());
                VMuktiAPI.ClsLogging.WriteToTresslog(sb);
            }

        }

        void radDrag999_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                if (((Canvas)((RadioButton)sender).Parent).Name == "cnvControls")
                {
                    newDrag = 0;
                }
                if (currentControl != null)
                {
                    PrePoint.X = e.GetPosition(cnvPaint).X - double.Parse(currentControl.GetValue(Canvas.LeftProperty).ToString());
                    PrePoint.Y = e.GetPosition(cnvPaint).Y - double.Parse(currentControl.GetValue(Canvas.TopProperty).ToString());

                    System.Windows.DragDrop.DoDragDrop((DependencyObject)((RadioButton)sender), ((RadioButton)sender), DragDropEffects.Move);
                }
            }
            catch (Exception ex)
            {
                ex.Data.Add("My Key", "VMukti--:--VmuktiModules--:--Call Center--:--CRMDesigner--:--CRMDesigner.Presentation--:--ctlCRMDesigner.xaml.cs--:--radDrag999_PreviewMouseDown()--");
                //ClsException.LogError(ex);
                //ClsException.WriteToErrorLogFile(ex);
                System.Text.StringBuilder sb = new StringBuilder();
                sb.AppendLine(ex.Message);
                sb.AppendLine();
                sb.AppendLine("StackTrace : " + ex.StackTrace);
                sb.AppendLine();
                sb.AppendLine("Location : " + ex.Data["My Key"].ToString());
                sb.AppendLine();
                sb1 = CreateTressInfo();
                sb.Append(sb1.ToString());
                VMuktiAPI.ClsLogging.WriteToTresslog(sb);
            }
        }
         void tabDrag999_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                if (((Canvas)((TabControl)sender).Parent).Name == "cnvControls")
                {
                    newDrag = 0;
                }
                if (currentControl != null)
                {
                    PrePoint.X = e.GetPosition(cnvPaint).X - double.Parse(currentControl.GetValue(Canvas.LeftProperty).ToString());
                    PrePoint.Y = e.GetPosition(cnvPaint).Y - double.Parse(currentControl.GetValue(Canvas.TopProperty).ToString());
                    System.Windows.DragDrop.DoDragDrop((DependencyObject)((TabControl)sender), ((TabControl)sender), DragDropEffects.Move);
                }
            }
            catch (Exception ex)
            {
                ex.Data.Add("My Key", "VMukti--:--VmuktiModules--:--Call Center--:--CRMDesigner--:--CRMDesigner.Presentation--:--ctlCRMDesigner.xaml.cs--:--tabDrag999_PreviewMouseDown()--");
                //ClsException.LogError(ex);
                //ClsException.WriteToErrorLogFile(ex);
                System.Text.StringBuilder sb = new StringBuilder();
                sb.AppendLine(ex.Message);
                sb.AppendLine();
                sb.AppendLine("StackTrace : " + ex.StackTrace);
                sb.AppendLine();
                sb.AppendLine("Location : " + ex.Data["My Key"].ToString());
                sb.AppendLine();
                sb1 = CreateTressInfo();
                sb.Append(sb1.ToString());
                VMuktiAPI.ClsLogging.WriteToTresslog(sb);
            }
        }
        void chkDrag999_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                if (((Canvas)((CheckBox)sender).Parent).Name == "cnvControls")
                {
                    newDrag = 0;
                }
                if (currentControl != null)
                {
                    PrePoint.X = e.GetPosition(cnvPaint).X - double.Parse(currentControl.GetValue(Canvas.LeftProperty).ToString());
                    PrePoint.Y = e.GetPosition(cnvPaint).Y - double.Parse(currentControl.GetValue(Canvas.TopProperty).ToString());

                    System.Windows.DragDrop.DoDragDrop((DependencyObject)((CheckBox)sender), ((CheckBox)sender), DragDropEffects.Move);
                }
            }
            catch (Exception ex)
            {
                ex.Data.Add("My Key", "VMukti--:--VmuktiModules--:--Call Center--:--CRMDesigner--:--CRMDesigner.Presentation--:--ctlCRMDesigner.xaml.cs--:--chkDrag999_PreviewMouseDown()--");
                //ClsException.LogError(ex);
                //ClsException.WriteToErrorLogFile(ex);
                System.Text.StringBuilder sb = new StringBuilder();
                sb.AppendLine(ex.Message);
                sb.AppendLine();
                sb.AppendLine("StackTrace : " + ex.StackTrace);
                sb.AppendLine();
                sb.AppendLine("Location : " + ex.Data["My Key"].ToString());
                sb.AppendLine();
                sb1 = CreateTressInfo();
                sb.Append(sb1.ToString());
                VMuktiAPI.ClsLogging.WriteToTresslog(sb);
            }
        }

        void lstDrag999_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                if (((Canvas)((ListBox)sender).Parent).Name == "cnvControls")
                {
                    newDrag = 0;
                }
                if (currentControl != null)
                {
                    PrePoint.X = e.GetPosition(cnvPaint).X - double.Parse(currentControl.GetValue(Canvas.LeftProperty).ToString());
                    PrePoint.Y = e.GetPosition(cnvPaint).Y - double.Parse(currentControl.GetValue(Canvas.TopProperty).ToString());

                    System.Windows.DragDrop.DoDragDrop((DependencyObject)((ListBox)sender), ((ListBox)sender), DragDropEffects.Move);
                }
            }
            catch (Exception ex)
            {
                ex.Data.Add("My Key", "VMukti--:--VmuktiModules--:--Call Center--:--CRMDesigner--:--CRMDesigner.Presentation--:--ctlCRMDesigner.xaml.cs--:--lstDrag999_PreviewMouseDown()--");
                //ClsException.LogError(ex);
                //ClsException.WriteToErrorLogFile(ex);
                System.Text.StringBuilder sb = new StringBuilder();
                sb.AppendLine(ex.Message);
                sb.AppendLine();
                sb.AppendLine("StackTrace : " + ex.StackTrace);
                sb.AppendLine();
                sb.AppendLine("Location : " + ex.Data["My Key"].ToString());
                sb.AppendLine();
                sb1 = CreateTressInfo();
                sb.Append(sb1.ToString());
                VMuktiAPI.ClsLogging.WriteToTresslog(sb);
            }
        }

        void cmbDrag999_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                if (((Canvas)((ComboBox)sender).Parent).Name == "cnvControls")
                {
                    newDrag = 0;
                }
                if (currentControl != null)
                {
                    PrePoint.X = e.GetPosition(cnvPaint).X - double.Parse(currentControl.GetValue(Canvas.LeftProperty).ToString());
                    PrePoint.Y = e.GetPosition(cnvPaint).Y - double.Parse(currentControl.GetValue(Canvas.TopProperty).ToString());

                    System.Windows.DragDrop.DoDragDrop((DependencyObject)((ComboBox)sender), ((ComboBox)sender), DragDropEffects.Move);
                }
            }
            catch (Exception ex)
            {
                ex.Data.Add("My Key", "VMukti--:--VmuktiModules--:--Call Center--:--CRMDesigner--:--CRMDesigner.Presentation--:--ctlCRMDesigner.xaml.cs--:--cmbDrag999_PreviewMouseDown()--");
                //ClsException.LogError(ex);
                //ClsException.WriteToErrorLogFile(ex);
                System.Text.StringBuilder sb = new StringBuilder();
                sb.AppendLine(ex.Message);
                sb.AppendLine();
                sb.AppendLine("StackTrace : " + ex.StackTrace);
                sb.AppendLine();
                sb.AppendLine("Location : " + ex.Data["My Key"].ToString());
                sb.AppendLine();
                sb1 = CreateTressInfo();
                sb.Append(sb1.ToString());
                VMuktiAPI.ClsLogging.WriteToTresslog(sb);
            }
        }

        void myExpander1_Collapsed(object sender, RoutedEventArgs e)
        {
            try
            {
                if (MyPropGrid.EventTrack == 0)
                {
                    myExpander1.Width = 120;
                    myExpander1.Height = 23;
                    myExpander1.SetValue(Canvas.LeftProperty, 339.0);
                }
                MyPropGrid.EventTrack = 0;
            }
            catch (Exception ex)
            {
                ex.Data.Add("My Key", "VMukti--:--VmuktiModules--:--Call Center--:--CRMDesigner--:--CRMDesigner.Presentation--:--ctlCRMDesigner.xaml.cs--:--myExpander1_Collapsed()--");
                //ClsException.LogError(ex);
                //ClsException.WriteToErrorLogFile(ex);
                System.Text.StringBuilder sb = new StringBuilder();
                sb.AppendLine(ex.Message);
                sb.AppendLine();
                sb.AppendLine("StackTrace : " + ex.StackTrace);
                sb.AppendLine();
                sb.AppendLine("Location : " + ex.Data["My Key"].ToString());
                sb.AppendLine();
                sb1 = CreateTressInfo();
                sb.Append(sb1.ToString());
                VMuktiAPI.ClsLogging.WriteToTresslog(sb);
            }
        }

        void myExpander1_Expanded(object sender, RoutedEventArgs e)
        {
            try
            {
                myExpander1.Width = 223;
                myExpander1.Height = 550;
                myExpander1.SetValue(Canvas.LeftProperty, 339.0);
                MyPropGrid.EventTrack = 0;
                MyPropGrid.Visibility = Visibility.Visible;
            }
            catch (Exception ex)
            {
                ex.Data.Add("My Key", "VMukti--:--VmuktiModules--:--Call Center--:--CRMDesigner--:--CRMDesigner.Presentation--:--ctlCRMDesigner.xaml.cs--:--myExpander1_Expanded()--");
                //ClsException.LogError(ex);
                //ClsException.WriteToErrorLogFile(ex);
                System.Text.StringBuilder sb = new StringBuilder();
                sb.AppendLine(ex.Message);
                sb.AppendLine();
                sb.AppendLine("StackTrace : " + ex.StackTrace);
                sb.AppendLine();
                sb.AppendLine("Location : " + ex.Data["My Key"].ToString());
                sb.AppendLine();
                sb1 = CreateTressInfo();
                sb.Append(sb1.ToString());
                VMuktiAPI.ClsLogging.WriteToTresslog(sb);
            }
        }

        void ctlCRMDesigner_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Delete)
            {
                foreach (object o in cnvPaint.Children)
                {
                    try
                    {
                        if (((ctlPOD)o).rect.IsVisible)
                        {
                            cnvPaint.Children.Remove((ctlPOD)o);
                            break;
                        }
                    }
                    catch (Exception ex)
                    {
                        ex.Data.Add("My Key", "VMukti--:--VmuktiModules--:--Call Center--:--CRMDesigner--:--CRMDesigner.Presentation--:--ctlCRMDesigner.xaml.cs--:--ctlCRMDesigner_PreviewKeyDown()--");
                        //ClsException.LogError(ex);
                        //ClsException.WriteToErrorLogFile(ex);
                        System.Text.StringBuilder sb = new StringBuilder();
                        sb.AppendLine(ex.Message);
                        sb.AppendLine();
                        sb.AppendLine("StackTrace : " + ex.StackTrace);
                        sb.AppendLine();
                        sb.AppendLine("Location : " + ex.Data["My Key"].ToString());
                        sb.AppendLine();
                        sb1 = CreateTressInfo();
                        sb.Append(sb1.ToString());
                        VMuktiAPI.ClsLogging.WriteToTresslog(sb);
                    }
                }
                //MessageBox.Show("Key Down");
            }
            else if (e.Key == Key.Delete)
            {
                if (MyPropGrid.IsVisible)
                    MyPropGrid.Visibility = Visibility.Collapsed;
                else
                    MyPropGrid.Visibility = Visibility.Visible;
            }
        }

        void r1_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            try
            {
                if (r1.Parent != null)
                {
                    ((Canvas)r1.Parent).Children.Remove((UIElement)r1);
                }
                cnvPaint.Children.Add(r1);
            }
            catch (Exception ex)
            {
                ex.Data.Add("My Key", "VMukti--:--VmuktiModules--:--Call Center--:--CRMDesigner--:--CRMDesigner.Presentation--:--ctlCRMDesigner.xaml.cs--:--r1_SizeChanged()--");
                //ClsException.LogError(ex);
                //ClsException.WriteToErrorLogFile(ex);
                System.Text.StringBuilder sb = new StringBuilder();
                sb.AppendLine(ex.Message);
                sb.AppendLine();
                sb.AppendLine("StackTrace : " + ex.StackTrace);
                sb.AppendLine();
                sb.AppendLine("Location : " + ex.Data["My Key"].ToString());
                sb.AppendLine();
                sb1 = CreateTressInfo();
                sb.Append(sb1.ToString());
                VMuktiAPI.ClsLogging.WriteToTresslog(sb);
            }
        }

        void cnvPaint_MouseMove(object sender, MouseEventArgs e)
        {
            try
            {
                if (r1.Height != currentControl.Height && r1.Width != currentControl.Width)
                {
                    r1.Visibility = Visibility.Collapsed;
                }
            }
            catch (Exception ex)
            {
                ex.Data.Add("My Key", "VMukti--:--VmuktiModules--:--Call Center--:--CRMDesigner--:--CRMDesigner.Presentation--:--ctlCRMDesigner.xaml.cs--:--cnvPaint_MouseMove()--");
                //ClsException.LogError(ex);
                //ClsException.WriteToErrorLogFile(ex);
                System.Text.StringBuilder sb = new StringBuilder();
                sb.AppendLine(ex.Message);
                sb.AppendLine();
                sb.AppendLine("StackTrace : " + ex.StackTrace);
                sb.AppendLine();
                sb.AppendLine("Location : " + ex.Data["My Key"].ToString());
                sb.AppendLine();
                sb1 = CreateTressInfo();
                sb.Append(sb1.ToString());
                VMuktiAPI.ClsLogging.WriteToTresslog(sb);
            }
        }

        void currentControl_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            try
            {
                MyPropGrid.ControlToBind = currentControl;
            }
            catch (Exception ex)
            {
                ex.Data.Add("My Key", "VMukti--:--VmuktiModules--:--Call Center--:--CRMDesigner--:--CRMDesigner.Presentation--:--ctlCRMDesigner.xaml.cs--:--currentControl_SizeChanged()--");
                //ClsException.LogError(ex);
                //ClsException.WriteToErrorLogFile(ex);
                System.Text.StringBuilder sb = new StringBuilder();
                sb.AppendLine(ex.Message);
                sb.AppendLine();
                sb.AppendLine("StackTrace : " + ex.StackTrace);
                sb.AppendLine();
                sb.AppendLine("Location : " + ex.Data["My Key"].ToString());
                sb.AppendLine();
                sb1 = CreateTressInfo();
                sb.Append(sb1.ToString());
                VMuktiAPI.ClsLogging.WriteToTresslog(sb);
            }
        }

        void ctlCRMDesigner_MouseMove(object sender, MouseEventArgs e)
        {
            #region Clear Resize Option For Each Object

            if (e.LeftButton == MouseButtonState.Released)
            {
                foreach (object o in cnvPaint.Children)
                {
                    if (o.GetType() == typeof(ctlPOD))
                    {
                        ((ctlPOD)o).Current = CRMDesigner.Presentation.ResizeOption.None;
                    }
                }
            }

            #endregion

            try
            {
                #region Main Implementation

                if (currentControl.Current == CRMDesigner.Presentation.ResizeOption.DownEdge)
                {
                    FncSetProperties();
                    r1.Height = r1.Height - (currentControl.PREVXY.Y - e.GetPosition(cnvPaint).Y);
                }

                else if (currentControl.Current == CRMDesigner.Presentation.ResizeOption.UpEdge)
                {
                    FncSetProperties();
                    r1.Height = r1.Height + (currentControl.PREVXY.Y - e.GetPosition(cnvPaint).Y);
                    r1.SetValue(Canvas.TopProperty, float.Parse(r1.GetValue(Canvas.TopProperty).ToString()) + e.GetPosition(cnvPaint).Y - currentControl.PREVXY.Y);
                }

                else if (currentControl.Current == CRMDesigner.Presentation.ResizeOption.LeftEdge)
                {
                    FncSetProperties();
                    r1.Width = r1.Width - (e.GetPosition(cnvPaint).X - currentControl.PREVXY.X);
                    r1.SetValue(Canvas.LeftProperty, float.Parse(r1.GetValue(Canvas.LeftProperty).ToString()) + (e.GetPosition(cnvPaint).X - currentControl.PREVXY.X));
                }

                else if (currentControl.Current == CRMDesigner.Presentation.ResizeOption.RightEdge)
                {
                    FncSetProperties();
                    r1.Width = r1.Width + e.GetPosition(cnvPaint).X - currentControl.PREVXY.X;
                }

                #endregion
            }
            catch (Exception ex)
            {
                ex.Data.Add("My Key", "VMukti--:--VmuktiModules--:--Call Center--:--CRMDesigner--:--CRMDesigner.Presentation--:--ctlCRMDesigner.xaml.cs--:--ctlCRMDesigner_MouseMove()--");
                //ClsException.LogError(ex);
                //ClsException.WriteToErrorLogFile(ex);
                System.Text.StringBuilder sb = new StringBuilder();
                sb.AppendLine(ex.Message);
                sb.AppendLine();
                sb.AppendLine("StackTrace : " + ex.StackTrace);
                sb.AppendLine();
                sb.AppendLine("Location : " + ex.Data["My Key"].ToString());
                sb.AppendLine();
                sb1 = CreateTressInfo();
                sb.Append(sb1.ToString());
                VMuktiAPI.ClsLogging.WriteToTresslog(sb);
            }
        }

        void FncSetProperties()
        {
            try
            {
                r1.Height = currentControl.Height;
                r1.Width = currentControl.Width;
                r1.Visibility = Visibility.Visible;
                r1.SetValue(Canvas.LeftProperty, currentControl.GetValue(Canvas.LeftProperty));
                r1.SetValue(Canvas.TopProperty, currentControl.GetValue(Canvas.TopProperty));
            }
            catch (Exception ex)
            {
                ex.Data.Add("My Key", "VMukti--:--VmuktiModules--:--Call Center--:--CRMDesigner--:--CRMDesigner.Presentation--:--ctlCRMDesigner.xaml.cs--:--FncSetProperties()--");
                //ClsException.LogError(ex);
                //ClsException.WriteToErrorLogFile(ex);
                System.Text.StringBuilder sb = new StringBuilder();
                sb.AppendLine(ex.Message);
                sb.AppendLine();
                sb.AppendLine("StackTrace : " + ex.StackTrace);
                sb.AppendLine();
                sb.AppendLine("Location : " + ex.Data["My Key"].ToString());
                sb.AppendLine();
                sb1 = CreateTressInfo();
                sb.Append(sb1.ToString());
                VMuktiAPI.ClsLogging.WriteToTresslog(sb);
            }
        }

        void ctlCRMDesigner_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            try
            {

                foreach (object o in cnvPaint.Children)
                {
                    if (o.GetType() == typeof(ctlPOD))
                    {
                        ((ctlPOD)o).IsRectvisible = false;
                    }
                }
            }
            catch (Exception ex)
            {
                ex.Data.Add("My Key", "VMukti--:--VmuktiModules--:--Call Center--:--CRMDesigner--:--CRMDesigner.Presentation--:--ctlCRMDesigner.xaml.cs--:--ctlCRMDesigner_PreviewMouseDown()--");
                //ClsException.LogError(ex);
                //ClsException.WriteToErrorLogFile(ex);
                System.Text.StringBuilder sb = new StringBuilder();
                sb.AppendLine(ex.Message);
                sb.AppendLine();
                sb.AppendLine("StackTrace : " + ex.StackTrace);
                sb.AppendLine();
                sb.AppendLine("Location : " + ex.Data["My Key"].ToString());
                sb.AppendLine();
                sb1 = CreateTressInfo();
                sb.Append(sb1.ToString());
                VMuktiAPI.ClsLogging.WriteToTresslog(sb);
            }


        }

        void ctlCRMDesigner_PreviewMouseUp(object sender, MouseButtonEventArgs e)
        {
            try
            {
                if (PrePoint.X == e.GetPosition(this).X || PrePoint.Y == e.GetPosition(this).Y)
                { }
                else
                {
                    #region Main Implementation

                    if (currentControl.Cursor != Cursors.Arrow)
                    {

                        if (currentControl.Current == CRMDesigner.Presentation.ResizeOption.DownEdge)
                        {
                            if (currentControl.Height + e.GetPosition(cnvPaint).Y - currentControl.PREVXY.Y >= 10)
                            {
                                currentControl.Height = currentControl.Height + e.GetPosition(cnvPaint).Y - currentControl.PREVXY.Y;
                            }
                            else
                            {
                                // Restricting To A Minimum Value //
                                currentControl.Height = 10;
                            }
                        }

                        else if (currentControl.Current == CRMDesigner.Presentation.ResizeOption.UpEdge)
                        {
                            if (currentControl.Height + currentControl.PREVXY.Y - e.GetPosition(cnvPaint).Y >= 10)
                            {
                                currentControl.Height = currentControl.Height + currentControl.PREVXY.Y - e.GetPosition(cnvPaint).Y;
                                currentControl.SetValue(Canvas.TopProperty, e.GetPosition(cnvPaint).Y);
                            }
                            else
                            {
                                // Restricting To A Minimum Value //
                                currentControl.SetValue(Canvas.TopProperty, float.Parse(currentControl.GetValue(Canvas.TopProperty).ToString()) + currentControl.Height - 10.0);
                                currentControl.Height = 10;
                            }
                        }

                        else if (currentControl.Current == CRMDesigner.Presentation.ResizeOption.LeftEdge)
                        {
                            if (currentControl.Width + currentControl.PREVXY.X - e.GetPosition(cnvPaint).X >= 10)
                            {
                                currentControl.Width = currentControl.Width + currentControl.PREVXY.X - e.GetPosition(cnvPaint).X;
                                currentControl.SetValue(Canvas.LeftProperty, e.GetPosition(cnvPaint).X);
                            }
                            else
                            {
                                // Restricting To A Minimum Value //
                                currentControl.SetValue(Canvas.LeftProperty, float.Parse(currentControl.GetValue(Canvas.LeftProperty).ToString()) + currentControl.Width - 10.0);
                                currentControl.Width = 10;
                            }
                        }

                        else if (currentControl.Current == CRMDesigner.Presentation.ResizeOption.RightEdge)
                        {
                            if (currentControl.Width + e.GetPosition(cnvPaint).X - currentControl.PREVXY.X >= 10)
                            {
                                currentControl.Width = currentControl.Width + e.GetPosition(cnvPaint).X - currentControl.PREVXY.X;
                            }
                            else
                            {
                                // Restricting To A Minimum Value //
                                currentControl.Width = 10;

                            }
                        }

                        currentControl.IsRectvisible = true;
                        currentControl.Current = CRMDesigner.Presentation.ResizeOption.None;

                        MyPropGrid.ControlToBind = currentControl;
                    }
                    #endregion

                    r1.Visibility = Visibility.Hidden;
                }
            }
            catch (Exception ex)
            {
                ex.Data.Add("My Key", "VMukti--:--VmuktiModules--:--Call Center--:--CRMDesigner--:--CRMDesigner.Presentation--:--ctlCRMDesigner.xaml.cs--:--ctlCRMDesigner_PreviewMouseUp()--");
                //ClsException.LogError(ex);
                //ClsException.WriteToErrorLogFile(ex);
                System.Text.StringBuilder sb = new StringBuilder();
                sb.AppendLine(ex.Message);
                sb.AppendLine();
                sb.AppendLine("StackTrace : " + ex.StackTrace);
                sb.AppendLine();
                sb.AppendLine("Location : " + ex.Data["My Key"].ToString());
                sb.AppendLine();
                sb1 = CreateTressInfo();
                sb.Append(sb1.ToString());
                VMuktiAPI.ClsLogging.WriteToTresslog(sb);
            }
        }

        void cnvPaint_DragOver(object sender, DragEventArgs e)
        {
            try
            {
                if (newDrag == 1)
                {
                    r1.Visibility = Visibility.Visible;
                    r1.Height = currentControl.Height;
                    r1.Width = currentControl.Width;
                    r1.SetValue(Canvas.LeftProperty, e.GetPosition(cnvPaint).X - PrePoint.X);
                    r1.SetValue(Canvas.TopProperty, e.GetPosition(cnvPaint).Y - PrePoint.Y);
                }
            }
            catch (Exception ex)
            {
                ex.Data.Add("My Key", "VMukti--:--VmuktiModules--:--Call Center--:--CRMDesigner--:--CRMDesigner.Presentation--:--ctlCRMDesigner.xaml.cs--:--cnvPaint_DragOver()--");
                //ClsException.LogError(ex);
                //ClsException.WriteToErrorLogFile(ex);
                System.Text.StringBuilder sb = new StringBuilder();
                sb.AppendLine(ex.Message);
                sb.AppendLine();
                sb.AppendLine("StackTrace : " + ex.StackTrace);
                sb.AppendLine();
                sb.AppendLine("Location : " + ex.Data["My Key"].ToString());
                sb.AppendLine();
                sb1 = CreateTressInfo();
                sb.Append(sb1.ToString());
                VMuktiAPI.ClsLogging.WriteToTresslog(sb);
            }

        }

        void lblDrag_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                if (((Canvas)((Label)sender).Parent).Name == "cnvControls")
                {
                    newDrag = 0;
                }
                if (currentControl != null)
                {
                    PrePoint.X = e.GetPosition(cnvPaint).X - double.Parse(currentControl.GetValue(Canvas.LeftProperty).ToString());
                    PrePoint.Y = e.GetPosition(cnvPaint).Y - double.Parse(currentControl.GetValue(Canvas.TopProperty).ToString());
                    System.Windows.DragDrop.DoDragDrop((DependencyObject)((Label)sender), ((Label)sender), DragDropEffects.Move);
                }
            }
            catch (Exception ex)
            {
                ex.Data.Add("My Key", "VMukti--:--VmuktiModules--:--Call Center--:--CRMDesigner--:--CRMDesigner.Presentation--:--ctlCRMDesigner.xaml.cs--:--lblDrag_PreviewMouseDown()--");
                //ClsException.LogError(ex);
                //ClsException.WriteToErrorLogFile(ex);
                System.Text.StringBuilder sb = new StringBuilder();
                sb.AppendLine(ex.Message);
                sb.AppendLine();
                sb.AppendLine("StackTrace : " + ex.StackTrace);
                sb.AppendLine();
                sb.AppendLine("Location : " + ex.Data["My Key"].ToString());
                sb.AppendLine();
                sb1 = CreateTressInfo();
                sb.Append(sb1.ToString());
                VMuktiAPI.ClsLogging.WriteToTresslog(sb);
            }
        }

        void txtDrag_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                if (((Canvas)((TextBox)sender).Parent).Name == "cnvControls")
                {
                    newDrag = 0;
                }
                if (currentControl != null)
                {
                    objPOD_PreviewMouseDown(((TextBox)sender).Parent, null);
                    PrePoint.X = e.GetPosition(cnvPaint).X - double.Parse(currentControl.GetValue(Canvas.LeftProperty).ToString());
                    PrePoint.Y = e.GetPosition(cnvPaint).Y - double.Parse(currentControl.GetValue(Canvas.TopProperty).ToString());
                    System.Windows.DragDrop.DoDragDrop((DependencyObject)((TextBox)sender), ((TextBox)sender), DragDropEffects.Move);
                }
            }
            catch (Exception ex)
            {
                ex.Data.Add("My Key", "VMukti--:--VmuktiModules--:--Call Center--:--CRMDesigner--:--CRMDesigner.Presentation--:--ctlCRMDesigner.xaml.cs--:--txtDrag_PreviewMouseDown()--");
                //ClsException.LogError(ex);
                //ClsException.WriteToErrorLogFile(ex);
                System.Text.StringBuilder sb = new StringBuilder();
                sb.AppendLine(ex.Message);
                sb.AppendLine();
                sb.AppendLine("StackTrace : " + ex.StackTrace);
                sb.AppendLine();
                sb.AppendLine("Location : " + ex.Data["My Key"].ToString());
                sb.AppendLine();
                sb1 = CreateTressInfo();
                sb.Append(sb1.ToString());
                VMuktiAPI.ClsLogging.WriteToTresslog(sb);
            }
        }

        void btnDrag_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                if (((Canvas)((Button)sender).Parent).Name == "cnvControls")
                {
                    newDrag = 0;
                }

                if (currentControl != null)
                {
                    PrePoint.X = e.GetPosition(cnvPaint).X - double.Parse(currentControl.GetValue(Canvas.LeftProperty).ToString());
                    PrePoint.Y = e.GetPosition(cnvPaint).Y - double.Parse(currentControl.GetValue(Canvas.TopProperty).ToString());
                    System.Windows.DragDrop.DoDragDrop((DependencyObject)((Button)sender), ((Button)sender), DragDropEffects.Move);
                }
            }
            catch (Exception ex)
            {
                ex.Data.Add("My Key", "VMukti--:--VmuktiModules--:--Call Center--:--CRMDesigner--:--CRMDesigner.Presentation--:--ctlCRMDesigner.xaml.cs--:--btnDrag_PreviewMouseDown()--");
                //ClsException.LogError(ex);
                //ClsException.WriteToErrorLogFile(ex);
                System.Text.StringBuilder sb = new StringBuilder();
                sb.AppendLine(ex.Message);
                sb.AppendLine();
                sb.AppendLine("StackTrace : " + ex.StackTrace);
                sb.AppendLine();
                sb.AppendLine("Location : " + ex.Data["My Key"].ToString());
                sb.AppendLine();
                sb1 = CreateTressInfo();
                sb.Append(sb1.ToString());
                VMuktiAPI.ClsLogging.WriteToTresslog(sb);
            }
        }

        void cnvPaint_Drop(object sender, DragEventArgs e)
        {
            try
            {
                if (e.Data.GetData(typeof(Button)) != null)
                {
                    if (((Canvas)((Button)e.Data.GetData(typeof(Button))).Parent).Name.ToString() == "cnvControls")
                    {

                        Point p = e.GetPosition((IInputElement)cnvPaint);
                        Button btn = new Button();
                        btn.Height = 25;
                        btn.Width = 100;
                        btn.Content = "Button";
                        btn.PreviewMouseDown += new MouseButtonEventHandler(btnDrag_PreviewMouseDown);
                        btn.SetValue(Canvas.LeftProperty, 10.0);
                        btn.SetValue(Canvas.TopProperty, 10.0);

                        ctlPOD objPOD = new ctlPOD();
                        objPOD.AllowDrop = true;
                        objPOD.Height = 25;
                        objPOD.Width = 100;
                        objPOD.PreviewMouseDown += new MouseButtonEventHandler(objPOD_PreviewMouseDown);
                        objPOD.SetValue(Canvas.LeftProperty, p.X);
                        objPOD.SetValue(Canvas.TopProperty, p.Y);
                        MyPropGrid.ControlToBind = objPOD;
                        objPOD.cnvPOD.Children.Add(btn);
                        currentControl = objPOD;
                        cnvPaint.Children.Add(objPOD);
                    }
                    else if ((((Canvas)((Button)e.Data.GetData(typeof(Button))).Parent).Parent).GetType() == typeof(ctlPOD))
                    {
                        if (currentControl.rect.Visibility == Visibility.Visible)
                        {
                            Point p = e.GetPosition((IInputElement)cnvPaint);
                            ((Canvas)((Button)e.Data.GetData(typeof(Button))).Parent).Parent.SetValue(Canvas.LeftProperty, p.X - PrePoint.X);
                            ((Canvas)((Button)e.Data.GetData(typeof(Button))).Parent).Parent.SetValue(Canvas.TopProperty, p.Y - PrePoint.Y);
                        }
                    }
                }
                else if (e.Data.GetData(typeof(TabControl)) != null)
                {
                    if (((Canvas)((TabControl)e.Data.GetData(typeof(TabControl))).Parent).Name.ToString() == "cnvControls")
                    {
                        Point p = e.GetPosition((IInputElement)cnvPaint);
                        TabControl lbl = new TabControl();
                        //lbl.Content = "TabControl";
                        lbl.HorizontalContentAlignment = HorizontalAlignment.Center;
                        lbl.Height = 25;
                        lbl.Width = 100;
                        lbl.PreviewMouseDown += new MouseButtonEventHandler(tabDrag999_PreviewMouseDown);
                        lbl.SetValue(Canvas.LeftProperty, 10.0);
                        lbl.SetValue(Canvas.TopProperty, 10.0);

                        ctlPOD objPOD = new ctlPOD();
                        objPOD.cnvPOD.Children.Add(lbl);
                        objPOD.AllowDrop = true;
                        objPOD.Height = 25;
                        objPOD.Width = 100;
                        objPOD.SetValue(Canvas.LeftProperty, p.X);
                        objPOD.SetValue(Canvas.TopProperty, p.Y);
                        MyPropGrid.ControlToBind = objPOD;
                        objPOD.KeyDown += new KeyEventHandler(objPOD_KeyDown);
                        objPOD.PreviewMouseDown += new MouseButtonEventHandler(objPOD_PreviewMouseDown);
                        currentControl = objPOD;
                        cnvPaint.Children.Add(objPOD);
                    }

                    else if ((((Canvas)((TabControl)e.Data.GetData(typeof(TabControl))).Parent).Parent).GetType() == typeof(ctlPOD))
                    {
                        Point p = e.GetPosition((IInputElement)cnvPaint);
                        ((Canvas)((TabControl)e.Data.GetData(typeof(TabControl))).Parent).Parent.SetValue(Canvas.LeftProperty, p.X - PrePoint.X);
                        ((Canvas)((TabControl)e.Data.GetData(typeof(TabControl))).Parent).Parent.SetValue(Canvas.TopProperty, p.Y - PrePoint.Y);

                    }

                }
                else if (e.Data.GetData(typeof(Label)) != null)
                {
                    if (((Canvas)((Label)e.Data.GetData(typeof(Label))).Parent).Name.ToString() == "cnvControls")
                    {
                        Point p = e.GetPosition((IInputElement)cnvPaint);
                        Label lbl = new Label();
                        lbl.Content = "Label";
                        lbl.HorizontalContentAlignment = HorizontalAlignment.Center;
                        lbl.Height = 25;
                        lbl.Width = 100;
                        lbl.PreviewMouseDown += new MouseButtonEventHandler(lblDrag_PreviewMouseDown);
                        lbl.SetValue(Canvas.LeftProperty, 10.0);
                        lbl.SetValue(Canvas.TopProperty, 10.0);

                        ctlPOD objPOD = new ctlPOD();
                        objPOD.cnvPOD.Children.Add(lbl);
                        objPOD.AllowDrop = true;
                        objPOD.Height = 25;
                        objPOD.Width = 100;
                        objPOD.SetValue(Canvas.LeftProperty, p.X);
                        objPOD.SetValue(Canvas.TopProperty, p.Y);
                        MyPropGrid.ControlToBind = objPOD;
                        objPOD.KeyDown += new KeyEventHandler(objPOD_KeyDown);
                        objPOD.PreviewMouseDown += new MouseButtonEventHandler(objPOD_PreviewMouseDown);
                        currentControl = objPOD;
                        cnvPaint.Children.Add(objPOD);
                    }

                    else if ((((Canvas)((Label)e.Data.GetData(typeof(Label))).Parent).Parent).GetType() == typeof(ctlPOD))
                    {
                        Point p = e.GetPosition((IInputElement)cnvPaint);
                        ((Canvas)((Label)e.Data.GetData(typeof(Label))).Parent).Parent.SetValue(Canvas.LeftProperty, p.X - PrePoint.X);
                        ((Canvas)((Label)e.Data.GetData(typeof(Label))).Parent).Parent.SetValue(Canvas.TopProperty, p.Y - PrePoint.Y);

                    }
                }

                else if (e.Data.GetData(typeof(TextBox)) != null)
                {
                    if (((Canvas)((TextBox)e.Data.GetData(typeof(TextBox))).Parent).Name.ToString() == "cnvControls")
                    {
                        Point p = e.GetPosition((IInputElement)cnvPaint);
                        TextBox txt = new TextBox();
                        txt.IsReadOnly = true;
                        txt.Cursor = Cursors.Arrow;
                        txt.Height = 25;
                        txt.Width = 100;
                        txt.Text = "TextBox";
                        txt.MouseDown += new MouseButtonEventHandler(txt_MouseDown);
                        txt.PreviewMouseDown += new MouseButtonEventHandler(txtDrag_PreviewMouseDown);
                        txt.SetValue(Canvas.LeftProperty, 10.0);
                        txt.SetValue(Canvas.TopProperty, 10.0);

                        ctlPOD objPOD = new ctlPOD();
                        objPOD.AllowDrop = true;
                        objPOD.Height = 25;
                        objPOD.Width = 100;
                        objPOD.PreviewMouseDown += new MouseButtonEventHandler(objPOD_PreviewMouseDown);
                        objPOD.SetValue(Canvas.LeftProperty, p.X);
                        objPOD.SetValue(Canvas.TopProperty, p.Y);
                        MyPropGrid.ControlToBind = objPOD;
                        objPOD.cnvPOD.Children.Add(txt);
                        currentControl = objPOD;
                        cnvPaint.Children.Add(objPOD);
                    }
                    else if ((((Canvas)((TextBox)e.Data.GetData(typeof(TextBox))).Parent).Parent).GetType() == typeof(ctlPOD))
                    {
                        Point p = e.GetPosition((IInputElement)cnvPaint);
                        ((Canvas)((TextBox)e.Data.GetData(typeof(TextBox))).Parent).Parent.SetValue(Canvas.LeftProperty, p.X - PrePoint.X);
                        ((Canvas)((TextBox)e.Data.GetData(typeof(TextBox))).Parent).Parent.SetValue(Canvas.TopProperty, p.Y - PrePoint.Y);
                    }
                }


                else if (e.Data.GetData(typeof(ComboBox)) != null)
                {
                    if (((Canvas)((ComboBox)e.Data.GetData(typeof(ComboBox))).Parent).Name.ToString() == "cnvControls")
                    {
                        Point p = e.GetPosition((IInputElement)cnvPaint);
                        ComboBox cmb = new ComboBox();
                        cmb.Cursor = Cursors.Arrow;
                        cmb.Height = 25;
                        cmb.Width = 100;
                        cmb.Text = "ComboBox";
                        cmb.PreviewMouseDown += new MouseButtonEventHandler(cmb_PreviewMouseDown);
                        cmb.SetValue(Canvas.LeftProperty, 10.0);
                        cmb.SetValue(Canvas.TopProperty, 10.0);

                        ctlPOD objPOD = new ctlPOD();
                        objPOD.AllowDrop = true;
                        objPOD.Height = 25;
                        objPOD.Width = 100;
                        objPOD.PreviewMouseDown += new MouseButtonEventHandler(objPOD_PreviewMouseDown);
                        objPOD.SetValue(Canvas.LeftProperty, p.X);
                        objPOD.SetValue(Canvas.TopProperty, p.Y);
                        MyPropGrid.ControlToBind = objPOD;
                        objPOD.cnvPOD.Children.Add(cmb);
                        currentControl = objPOD;
                        cnvPaint.Children.Add(objPOD);
                    }
                    else if ((((Canvas)((ComboBox)e.Data.GetData(typeof(ComboBox))).Parent).Parent).GetType() == typeof(ctlPOD))
                    {
                        Point p = e.GetPosition((IInputElement)cnvPaint);
                        ((Canvas)((ComboBox)e.Data.GetData(typeof(ComboBox))).Parent).Parent.SetValue(Canvas.LeftProperty, p.X - PrePoint.X);
                        ((Canvas)((ComboBox)e.Data.GetData(typeof(ComboBox))).Parent).Parent.SetValue(Canvas.TopProperty, p.Y - PrePoint.Y);
                    }
                }

                else if (e.Data.GetData(typeof(ListBox)) != null)
                {
                    if (((Canvas)((ListBox)e.Data.GetData(typeof(ListBox))).Parent).Name.ToString() == "cnvControls")
                    {
                        Point p = e.GetPosition((IInputElement)cnvPaint);
                        ListBox lst = new ListBox();
                        lst.Cursor = Cursors.Arrow;
                        lst.Height = 25;
                        lst.Width = 100;
                        lst.PreviewMouseDown += new MouseButtonEventHandler(lst_PreviewMouseDown);
                        lst.SetValue(Canvas.LeftProperty, 10.0);
                        lst.SetValue(Canvas.TopProperty, 10.0);

                        ctlPOD objPOD = new ctlPOD();
                        objPOD.AllowDrop = true;
                        objPOD.Height = 25;
                        objPOD.Width = 100;
                        objPOD.PreviewMouseDown += new MouseButtonEventHandler(objPOD_PreviewMouseDown);
                        objPOD.SetValue(Canvas.LeftProperty, p.X);
                        objPOD.SetValue(Canvas.TopProperty, p.Y);
                        MyPropGrid.ControlToBind = objPOD;
                        objPOD.cnvPOD.Children.Add(lst);
                        currentControl = objPOD;
                        cnvPaint.Children.Add(objPOD);
                    }
                    else if ((((Canvas)((ListBox)e.Data.GetData(typeof(ListBox))).Parent).Parent).GetType() == typeof(ctlPOD))
                    {
                        Point p = e.GetPosition((IInputElement)cnvPaint);
                        ((Canvas)((ListBox)e.Data.GetData(typeof(ListBox))).Parent).Parent.SetValue(Canvas.LeftProperty, p.X - PrePoint.X);
                        ((Canvas)((ListBox)e.Data.GetData(typeof(ListBox))).Parent).Parent.SetValue(Canvas.TopProperty, p.Y - PrePoint.Y);
                    }
                }

                else if (e.Data.GetData(typeof(CheckBox)) != null)
                {
                    if (((Canvas)((CheckBox)e.Data.GetData(typeof(CheckBox))).Parent).Name.ToString() == "cnvControls")
                    {
                        Point p = e.GetPosition((IInputElement)cnvPaint);
                        CheckBox chk = new CheckBox();
                        chk.Cursor = Cursors.Arrow;
                        chk.Height = 25;
                        chk.Width = 100;
                        chk.Content = "Check Box";
                        chk.PreviewMouseDown += new MouseButtonEventHandler(chk_PreviewMouseDown);
                        chk.SetValue(Canvas.LeftProperty, 10.0);
                        chk.SetValue(Canvas.TopProperty, 10.0);

                        ctlPOD objPOD = new ctlPOD();
                        objPOD.AllowDrop = true;
                        objPOD.Height = 25;
                        objPOD.Width = 100;
                        objPOD.PreviewMouseDown += new MouseButtonEventHandler(objPOD_PreviewMouseDown);
                        objPOD.SetValue(Canvas.LeftProperty, p.X);
                        objPOD.SetValue(Canvas.TopProperty, p.Y);
                        MyPropGrid.ControlToBind = objPOD;
                        objPOD.cnvPOD.Children.Add(chk);
                        currentControl = objPOD;
                        cnvPaint.Children.Add(objPOD);
                    }
                    else if ((((Canvas)((CheckBox)e.Data.GetData(typeof(CheckBox))).Parent).Parent).GetType() == typeof(ctlPOD))
                    {
                        Point p = e.GetPosition((IInputElement)cnvPaint);
                        ((Canvas)((CheckBox)e.Data.GetData(typeof(CheckBox))).Parent).Parent.SetValue(Canvas.LeftProperty, p.X - PrePoint.X);
                        ((Canvas)((CheckBox)e.Data.GetData(typeof(CheckBox))).Parent).Parent.SetValue(Canvas.TopProperty, p.Y - PrePoint.Y);
                    }
                }

                else if (e.Data.GetData(typeof(RadioButton)) != null)
                {
                    if (((Canvas)((RadioButton)e.Data.GetData(typeof(RadioButton))).Parent).Name.ToString() == "cnvControls")
                    {
                        Point p = e.GetPosition((IInputElement)cnvPaint);
                        RadioButton rad = new RadioButton();
                        rad.Cursor = Cursors.Arrow;
                        rad.Height = 25;
                        rad.Width = 100;
                        rad.Content = "Radio Button";
                        rad.PreviewMouseDown += new MouseButtonEventHandler(rad_PreviewMouseDown);
                        rad.SetValue(Canvas.LeftProperty, 10.0);
                        rad.SetValue(Canvas.TopProperty, 10.0);

                        ctlPOD objPOD = new ctlPOD();
                        objPOD.AllowDrop = true;
                        objPOD.Height = 25;
                        objPOD.Width = 100;
                        objPOD.PreviewMouseDown += new MouseButtonEventHandler(objPOD_PreviewMouseDown);
                        objPOD.SetValue(Canvas.LeftProperty, p.X);
                        objPOD.SetValue(Canvas.TopProperty, p.Y);
                        MyPropGrid.ControlToBind = objPOD;
                        objPOD.cnvPOD.Children.Add(rad);
                        currentControl = objPOD;
                        cnvPaint.Children.Add(objPOD);
                    }
                    else if ((((Canvas)((RadioButton)e.Data.GetData(typeof(RadioButton))).Parent).Parent).GetType() == typeof(ctlPOD))
                    {
                        Point p = e.GetPosition((IInputElement)cnvPaint);
                        ((Canvas)((RadioButton)e.Data.GetData(typeof(RadioButton))).Parent).Parent.SetValue(Canvas.LeftProperty, p.X - PrePoint.X);
                        ((Canvas)((RadioButton)e.Data.GetData(typeof(RadioButton))).Parent).Parent.SetValue(Canvas.TopProperty, p.Y - PrePoint.Y);
                    }
                }

                newDrag = 1;
                r1.Visibility = Visibility.Collapsed;
                MyPropGrid.ControlToBind = currentControl;
            }
            catch (Exception ex)
            {
                ex.Data.Add("My Key", "VMukti--:--VmuktiModules--:--Call Center--:--CRMDesigner--:--CRMDesigner.Presentation--:--ctlCRMDesigner.xaml.cs--:--cnvPaint_Drop()--");
                //ClsException.LogError(ex);
                //ClsException.WriteToErrorLogFile(ex);
                System.Text.StringBuilder sb = new StringBuilder();
                sb.AppendLine(ex.Message);
                sb.AppendLine();
                sb.AppendLine("StackTrace : " + ex.StackTrace);
                sb.AppendLine();
                sb.AppendLine("Location : " + ex.Data["My Key"].ToString());
                sb.AppendLine();
                sb1 = CreateTressInfo();
                sb.Append(sb1.ToString());
                VMuktiAPI.ClsLogging.WriteToTresslog(sb);
            }
        }

        void txt_MouseDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                objPOD_PreviewMouseDown(((TextBox)sender).Parent, null);
            }
            catch (Exception ex)
            {
                ex.Data.Add("My Key", "VMukti--:--VmuktiModules--:--Call Center--:--CRMDesigner--:--CRMDesigner.Presentation--:--ctlCRMDesigner.xaml.cs--:--txt_MouseDown()--");
                //ClsException.LogError(ex);
                //ClsException.WriteToErrorLogFile(ex);
                System.Text.StringBuilder sb = new StringBuilder();
                sb.AppendLine(ex.Message);
                sb.AppendLine();
                sb.AppendLine("StackTrace : " + ex.StackTrace);
                sb.AppendLine();
                sb.AppendLine("Location : " + ex.Data["My Key"].ToString());
                sb.AppendLine();
                sb1 = CreateTressInfo();
                sb.Append(sb1.ToString());
                VMuktiAPI.ClsLogging.WriteToTresslog(sb);
            }
        }
        void rad_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                if (((Canvas)((RadioButton)sender).Parent).Name == "cnvControls")
                {
                    newDrag = 0;
                }
                if (currentControl != null)
                {
                    PrePoint.X = e.GetPosition(cnvPaint).X - double.Parse(currentControl.GetValue(Canvas.LeftProperty).ToString());
                    PrePoint.Y = e.GetPosition(cnvPaint).Y - double.Parse(currentControl.GetValue(Canvas.TopProperty).ToString());

                    System.Windows.DragDrop.DoDragDrop((DependencyObject)((RadioButton)sender), ((RadioButton)sender), DragDropEffects.Move);
                }
            }
            catch (Exception ex)
            {
                ex.Data.Add("My Key", "VMukti--:--VmuktiModules--:--Call Center--:--CRMDesigner--:--CRMDesigner.Presentation--:--ctlCRMDesigner.xaml.cs--:--rad_PreviewMouseDown()--");
                //ClsException.LogError(ex);
                //ClsException.WriteToErrorLogFile(ex);
                System.Text.StringBuilder sb = new StringBuilder();
                sb.AppendLine(ex.Message);
                sb.AppendLine();
                sb.AppendLine("StackTrace : " + ex.StackTrace);
                sb.AppendLine();
                sb.AppendLine("Location : " + ex.Data["My Key"].ToString());
                sb.AppendLine();
                sb1 = CreateTressInfo();
                sb.Append(sb1.ToString());
                VMuktiAPI.ClsLogging.WriteToTresslog(sb);
            }
        }

        void chk_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                if (((Canvas)((CheckBox)sender).Parent).Name == "cnvControls")
                {
                    newDrag = 0;
                }
                if (currentControl != null)
                {
                    PrePoint.X = e.GetPosition(cnvPaint).X - double.Parse(currentControl.GetValue(Canvas.LeftProperty).ToString());
                    PrePoint.Y = e.GetPosition(cnvPaint).Y - double.Parse(currentControl.GetValue(Canvas.TopProperty).ToString());

                    System.Windows.DragDrop.DoDragDrop((DependencyObject)((CheckBox)sender), ((CheckBox)sender), DragDropEffects.Move);
                }
            }
            catch (Exception ex)
            {
                ex.Data.Add("My Key", "VMukti--:--VmuktiModules--:--Call Center--:--CRMDesigner--:--CRMDesigner.Presentation--:--ctlCRMDesigner.xaml.cs--:--chk_PreviewMouseDown()--");
                //ClsException.LogError(ex);
                //ClsException.WriteToErrorLogFile(ex);
                System.Text.StringBuilder sb = new StringBuilder();
                sb.AppendLine(ex.Message);
                sb.AppendLine();
                sb.AppendLine("StackTrace : " + ex.StackTrace);
                sb.AppendLine();
                sb.AppendLine("Location : " + ex.Data["My Key"].ToString());
                sb.AppendLine();
                sb1 = CreateTressInfo();
                sb.Append(sb1.ToString());
                VMuktiAPI.ClsLogging.WriteToTresslog(sb);
            }
        }

        void lst_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                if (((Canvas)((ListBox)sender).Parent).Name == "cnvControls")
                {
                    newDrag = 0;
                }
                if (currentControl != null)
                {
                    PrePoint.X = e.GetPosition(cnvPaint).X - double.Parse(currentControl.GetValue(Canvas.LeftProperty).ToString());
                    PrePoint.Y = e.GetPosition(cnvPaint).Y - double.Parse(currentControl.GetValue(Canvas.TopProperty).ToString());
                    System.Windows.DragDrop.DoDragDrop((DependencyObject)((ListBox)sender), ((ListBox)sender), DragDropEffects.Move);
                }
            }
            catch (Exception ex)
            {
                ex.Data.Add("My Key", "VMukti--:--VmuktiModules--:--Call Center--:--CRMDesigner--:--CRMDesigner.Presentation--:--ctlCRMDesigner.xaml.cs--:--lst_PreviewMouseDown()--");
                //ClsException.LogError(ex);
                //ClsException.WriteToErrorLogFile(ex);
                System.Text.StringBuilder sb = new StringBuilder();
                sb.AppendLine(ex.Message);
                sb.AppendLine();
                sb.AppendLine("StackTrace : " + ex.StackTrace);
                sb.AppendLine();
                sb.AppendLine("Location : " + ex.Data["My Key"].ToString());
                sb.AppendLine();
                sb1 = CreateTressInfo();
                sb.Append(sb1.ToString());
                VMuktiAPI.ClsLogging.WriteToTresslog(sb);
            }
        }

        void cmb_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                if (((Canvas)((ComboBox)sender).Parent).Name == "cnvControls")
                {
                    newDrag = 0;
                }
                if (currentControl != null)
                {
                    PrePoint.X = e.GetPosition(cnvPaint).X - double.Parse(currentControl.GetValue(Canvas.LeftProperty).ToString());
                    PrePoint.Y = e.GetPosition(cnvPaint).Y - double.Parse(currentControl.GetValue(Canvas.TopProperty).ToString());

                    System.Windows.DragDrop.DoDragDrop((DependencyObject)((ComboBox)sender), ((ComboBox)sender), DragDropEffects.Move);
                }
            }
            catch (Exception ex)
            {
                ex.Data.Add("My Key", "VMukti--:--VmuktiModules--:--Call Center--:--CRMDesigner--:--CRMDesigner.Presentation--:--ctlCRMDesigner.xaml.cs--:--cmb_PreviewMouseDown()--");
                //ClsException.LogError(ex);
                //ClsException.WriteToErrorLogFile(ex);
                System.Text.StringBuilder sb = new StringBuilder();
                sb.AppendLine(ex.Message);
                sb.AppendLine();
                sb.AppendLine("StackTrace : " + ex.StackTrace);
                sb.AppendLine();
                sb.AppendLine("Location : " + ex.Data["My Key"].ToString());
                sb.AppendLine();
                sb1 = CreateTressInfo();
                sb.Append(sb1.ToString());
                VMuktiAPI.ClsLogging.WriteToTresslog(sb);
            }
        }

        void objPOD_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (((ctlPOD)sender).IsRectvisible)
                {
                    ((ctlPOD)sender).Visibility = Visibility.Collapsed;
                }
            }
            catch (Exception ex)
            {
                ex.Data.Add("My Key", "VMukti--:--VmuktiModules--:--Call Center--:--CRMDesigner--:--CRMDesigner.Presentation--:--ctlCRMDesigner.xaml.cs--:--objPOD_KeyDown()--");
                //ClsException.LogError(ex);
                //ClsException.WriteToErrorLogFile(ex);
                System.Text.StringBuilder sb = new StringBuilder();
                sb.AppendLine(ex.Message);
                sb.AppendLine();
                sb.AppendLine("StackTrace : " + ex.StackTrace);
                sb.AppendLine();
                sb.AppendLine("Location : " + ex.Data["My Key"].ToString());
                sb.AppendLine();
                sb1 = CreateTressInfo();
                sb.Append(sb1.ToString());
                VMuktiAPI.ClsLogging.WriteToTresslog(sb);
            }
        }

        void objPOD_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                currentControl = ((ctlPOD)sender);
                currentControl.IsRectvisible = true;
                ((ctlPOD)sender).IsRectvisible = true;
            }
            catch (Exception ex)
            {
                ex.Data.Add("My Key", "VMukti--:--VmuktiModules--:--Call Center--:--CRMDesigner--:--CRMDesigner.Presentation--:--ctlCRMDesigner.xaml.cs--:--objPOD_PreviewMouseDown()--");
                //ClsException.LogError(ex);
                //ClsException.WriteToErrorLogFile(ex);
                System.Text.StringBuilder sb = new StringBuilder();
                sb.AppendLine(ex.Message);
                sb.AppendLine();
                sb.AppendLine("StackTrace : " + ex.StackTrace);
                sb.AppendLine();
                sb.AppendLine("Location : " + ex.Data["My Key"].ToString());
                sb.AppendLine();
                sb1 = CreateTressInfo();
                sb.Append(sb1.ToString());
                VMuktiAPI.ClsLogging.WriteToTresslog(sb);
            }
        }

        private void btnSubmit_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                TabItem t = (TabItem)tbcMain.SelectedItem;

                foreach (object o in ((Canvas)t.Content).Children)
                {
                    if (o.GetType() == typeof(ctlPOD))
                    {
                        if (((ctlPOD)o).rect.Visibility == Visibility.Visible)
                        {
                            for (int i = 0; i < ((Canvas)((ctlPOD)o).cnvPOD).Children.Count; i++)
                            {
                                if ((((ctlPOD)o).cnvPOD.Children[i]).GetType() == typeof(Button))
                                {
                                    Int64 FieldID = Int64.Parse(((ComboBoxItem)cmbFileds.SelectedItem).Tag.ToString());
                                    ((Button)((ctlPOD)o).cnvPOD.Children[i]).Tag = "Bind-" + FieldID.ToString();
                                }
                                else if ((((ctlPOD)o).cnvPOD.Children[i]).GetType() == typeof(TextBox))
                                {
                                    Int64 FieldID = Int64.Parse(((ComboBoxItem)cmbFileds.SelectedItem).Tag.ToString());
                                    ((TextBox)((ctlPOD)o).cnvPOD.Children[i]).Tag = "Bind-" + FieldID.ToString();
                                }
                                else if ((((ctlPOD)o).cnvPOD.Children[i]).GetType() == typeof(Label))
                                {
                                    Int64 FieldID = Int64.Parse(((ComboBoxItem)cmbFileds.SelectedItem).Tag.ToString());
                                    ((Label)((ctlPOD)o).cnvPOD.Children[i]).Tag = "Bind-" + FieldID.ToString();
                                }
                            }
                        }
                    }
                }
            }

            catch (Exception ex)
            {
                ex.Data.Add("My Key", "VMukti--:--VmuktiModules--:--Call Center--:--CRMDesigner--:--CRMDesigner.Presentation--:--ctlCRMDesigner.xaml.cs--:--btnSubmit_Click()--");
                //ClsException.LogError(ex);
                //ClsException.WriteToErrorLogFile(ex);
                System.Text.StringBuilder sb = new StringBuilder();
                sb.AppendLine(ex.Message);
                sb.AppendLine();
                sb.AppendLine("StackTrace : " + ex.StackTrace);
                sb.AppendLine();
                sb.AppendLine("Location : " + ex.Data["My Key"].ToString());
                sb.AppendLine();
                sb1 = CreateTressInfo();
                sb.Append(sb1.ToString());
                VMuktiAPI.ClsLogging.WriteToTresslog(sb);
            }
        }

        private void btnGetQuestions_Click(object sender, RoutedEventArgs e)
        {
            /* // Comment By Keyur //
              btnNext.Content = "Next";

              CurrentQueCount = 0;
              startQuestion = "";
              startQuestionID = 0;

              objQueCollection = ClsQuestionCollectionR.GetAll(int.Parse(((ComboBoxItem)cmbScript.SelectedItem).Tag.ToString()));
              if (objQueCollection.Count > 0)
              {
                  FncSetInnerCanvas();
                  if (btnNext.Parent == null)
                  {
                      btnNext.Content = "Next";
                      btnNext.Height = 30;
                      btnNext.Width = 50;
                      btnNext.Click += new RoutedEventHandler(btnYes_Click);
                      Canvas.SetLeft(btnNext, 450.0);
                      Canvas.SetTop(btnNext, 10.0);
                      cnvHead.Children.Add(btnNext);

                      btnPrev.Content = "Previous";
                      btnPrev.Height = 30;
                      btnPrev.Width = 70;
                      btnPrev.Click += new RoutedEventHandler(btnPrev_Click);
                      Canvas.SetLeft(btnPrev, 520.0);
                      Canvas.SetTop(btnPrev, 10.0);
                      cnvHead.Children.Add(btnPrev);
                  }

                  if (btnStartQue.Parent == null)
                  {
                      btnStartQue.Content = "StartQuestion";
                      btnStartQue.Height = 30;
                      btnStartQue.Width = 100;
                      btnStartQue.Click += new RoutedEventHandler(btnStartQue_Click);
                      Canvas.SetLeft(btnStartQue, 650);
                      Canvas.SetTop(btnStartQue, 10);
                      cnvHead.Children.Add(btnStartQue);
                  }
                  CurrentQueCount = 0;
              }*/

            try
            {

                if (Directory.Exists(Assembly.GetAssembly(this.GetType()).Location.Replace("CRMDesigner.Presentation.dll", "CRM_src")))
                {
                    Directory.Delete(Assembly.GetAssembly(this.GetType()).Location.Replace("CRMDesigner.Presentation.dll", @"CRM_src"), true);
                }
                Directory.CreateDirectory(Assembly.GetAssembly(this.GetType()).Location.Replace("CRMDesigner.Presentation.dll", "CRM_src"));



                copyDirectory(Assembly.GetAssembly(this.GetType()).Location.Replace("CRMDesigner.Presentation.dll", "CRMBase"), Assembly.GetAssembly(this.GetType()).Location.Replace("CRMDesigner.Presentation.dll", "CRM_src"));

                //copyDirectory(AppDomain.CurrentDomain.BaseDirectory + "CRMBase", AppDomain.CurrentDomain.BaseDirectory + "CRM_src");


                if (!Directory.Exists(Assembly.GetAssembly(this.GetType()).Location.Replace("CRMDesigner.Presentation.dll", @"CRM_src\CRM.DataAccess\ReferencedAssemblies")))
                {
                    Directory.CreateDirectory(Assembly.GetAssembly(this.GetType()).Location.Replace("CRMDesigner.Presentation.dll", @"CRM_src\CRM.DataAccess\ReferencedAssemblies"));
                }

                if (!Directory.Exists(Assembly.GetAssembly(this.GetType()).Location.Replace("CRMDesigner.Presentation.dll", @"CRM_src\CRM.Presentation\ReferencedAssemblies")))
                {
                    Directory.CreateDirectory(Assembly.GetAssembly(this.GetType()).Location.Replace("CRMDesigner.Presentation.dll", @"CRM_src\CRM.Presentation\ReferencedAssemblies"));
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                ex.Data.Add("My Key", "VMukti--:--VmuktiModules--:--Call Center--:--CRMDesigner--:--CRMDesigner.Presentation--:--ctlCRMDesigner.xaml.cs--:--btnGetQuestions_Click()--");
                //ClsException.LogError(ex);
                //ClsException.WriteToErrorLogFile(ex);
                System.Text.StringBuilder sb = new StringBuilder();
                sb.AppendLine(ex.Message);
                sb.AppendLine();
                sb.AppendLine("StackTrace : " + ex.StackTrace);
                sb.AppendLine();
                sb.AppendLine("Location : " + ex.Data["My Key"].ToString());
                sb.AppendLine();
                sb1 = CreateTressInfo();
                sb.Append(sb1.ToString());
                VMuktiAPI.ClsLogging.WriteToTresslog(sb);
            }
            //MessageBox.Show("CRM Generated");
            FncSaveFiles();
        }

        void btnPrev_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                btnNext.IsEnabled = true;
                btnNext.Content = "Next";

                Canvas c = new Canvas();
                int count = cnvPaint.Children.Count - 1;

                for (int i = count; i >= 0; i--)
                {
                    if (cnvPaint.Children[i].GetType() != typeof(Expander) && cnvPaint.Children[i].GetType() != typeof(Rectangle))
                    {
                        object o = cnvPaint.Children[i];
                        cnvPaint.Children.Remove(cnvPaint.Children[i]);
                        c.Children.Add((UIElement)o);
                    }
                }
                c.Background = cnvPaint.Background;
                c.Height = cnvPaint.Height;
                c.Width = cnvPaint.Width;

                if (GeneratedQuestions > CurrentQueCount)
                {
                    lstCanvas[CurrentQueCount] = c;
                    CurrentQueCount--;
                }
                else if (GeneratedQuestions == CurrentQueCount)
                {
                    GeneratedQuestions++;
                    CurrentQueCount--;
                    lstCanvas.Add(c);
                }

                FncListToCanvas();

                if (CurrentQueCount <= 0)
                {
                    btnPrev.IsEnabled = false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                ex.Data.Add("My Key", "VMukti--:--VmuktiModules--:--Call Center--:--CRMDesigner--:--CRMDesigner.Presentation--:--ctlCRMDesigner.xaml.cs--:--btnPrev_Click()--");
                //ClsException.LogError(ex);
                //ClsException.WriteToErrorLogFile(ex);
                System.Text.StringBuilder sb = new StringBuilder();
                sb.AppendLine(ex.Message);
                sb.AppendLine();
                sb.AppendLine("StackTrace : " + ex.StackTrace);
                sb.AppendLine();
                sb.AppendLine("Location : " + ex.Data["My Key"].ToString());
                sb.AppendLine();
                sb1 = CreateTressInfo();
                sb.Append(sb1.ToString());
                VMuktiAPI.ClsLogging.WriteToTresslog(sb);
            }
        }

        void FncListToCanvas()
        {
            try
            {
                Canvas cnv = new Canvas();
                cnv = lstCanvas[CurrentQueCount];

                int count1 = cnv.Children.Count - 1;

                for (int i = count1; i >= 0; i--)
                {
                    object o = cnv.Children[i];
                    cnv.Children.Remove(cnv.Children[i]);
                    cnvPaint.Children.Add((UIElement)o);
                }

                cnvPaint.Background = cnv.Background;

                r1.Stroke = Brushes.White;
                r1.StrokeThickness = 3.0;
                r1.Fill = Brushes.Transparent;
                r1.StrokeDashArray = new DoubleCollection(2);
                r1.SizeChanged += new SizeChangedEventHandler(r1_SizeChanged);
                if (((Canvas)r1.Parent) != null)
                {
                    ((Canvas)r1.Parent).Children.Remove(r1);
                }
                cnvPaint.Children.Add(r1);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                ex.Data.Add("My Key", "VMukti--:--VmuktiModules--:--Call Center--:--CRMDesigner--:--CRMDesigner.Presentation--:--ctlCRMDesigner.xaml.cs--:--FncListToCanvas()--");
                //ClsException.LogError(ex);
                //ClsException.WriteToErrorLogFile(ex);
                System.Text.StringBuilder sb = new StringBuilder();
                sb.AppendLine(ex.Message);
                sb.AppendLine();
                sb.AppendLine("StackTrace : " + ex.StackTrace);
                sb.AppendLine();
                sb.AppendLine("Location : " + ex.Data["My Key"].ToString());
                sb.AppendLine();
                sb1 = CreateTressInfo();
                sb.Append(sb1.ToString());
                VMuktiAPI.ClsLogging.WriteToTresslog(sb);
            }
        }

        /*void btnStartQue_Click(object sender, RoutedEventArgs e)
        {
           startQuestion = objQueCollection[CurrentQueCount].QuestionName;
            startQuestionID = objQueCollection[CurrentQueCount].ID;
        }*/

        public static void copyDirectory(string Src, string Dst)
        {
            try
            {
                String[] Files;

                if (Dst[Dst.Length - 1] != System.IO.Path.DirectorySeparatorChar)
                    Dst += System.IO.Path.DirectorySeparatorChar;
                if (!Directory.Exists(Dst)) Directory.CreateDirectory(Dst);
                Files = Directory.GetFileSystemEntries(Src);
                foreach (string Element in Files)
                {
                    // Sub directories
                    if (Directory.Exists(Element))
                        copyDirectory(Element, Dst + System.IO.Path.GetFileName(Element));
                    // Files in directory
                    else
                        File.Copy(Element, Dst + System.IO.Path.GetFileName(Element), true);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                ex.Data.Add("My Key", "VMukti--:--VmuktiModules--:--Call Center--:--CRMDesigner--:--CRMDesigner.Presentation--:--ctlCRMDesigner.xaml.cs--:--copyDirectory()--");
                //ClsException.LogError(ex);
                //ClsException.WriteToErrorLogFile(ex);
                System.Text.StringBuilder sb = new StringBuilder();
                sb.AppendLine(ex.Message);
                sb.AppendLine();
                sb.AppendLine("StackTrace : " + ex.StackTrace);
                sb.AppendLine();
                sb.AppendLine("Location : " + ex.Data["My Key"].ToString());
                sb.AppendLine();
                sb1 = CreateTressInfo();
                sb.Append(sb1.ToString());
                VMuktiAPI.ClsLogging.WriteToTresslog(sb);
            }
        }

        #region Commented Code
        /*  void FncSetInnerCanvas()
        {
            //cnvPaint.Height = 280;
            //cnvPaint.Width = 700;

            Label lblHeader = new Label();
            lblHeader.Content = "CRM Designer";
            lblHeader.PreviewMouseDown += new MouseButtonEventHandler(lblDrag_PreviewMouseDown);
            lblHeader.FontSize = 19;

            ctlPOD objPOD1 = new ctlPOD();
            objPOD1.AllowDrop = true;
            objPOD1.Height = 45;
            objPOD1.Width = 700;
            objPOD1.PreviewMouseDown += new MouseButtonEventHandler(objPOD_PreviewMouseDown);
            objPOD1.SetValue(Canvas.LeftProperty, 25.0);
            objPOD1.SetValue(Canvas.TopProperty, 0.0);
            MyPropGrid.ControlToBind = objPOD1;
            objPOD1.cnvPOD.Children.Add(lblHeader);
            currentControl = objPOD1;
            cnvPaint.Children.Add(objPOD1);

            Label lblQuestion = new Label();
            lblQuestion.Content = "CRM Designer2";
            lblQuestion.PreviewMouseDown += new MouseButtonEventHandler(lblDrag_PreviewMouseDown);
            lblQuestion.FontSize = 17;

            ctlPOD objPOD2 = new ctlPOD();
            objPOD2.AllowDrop = true;
            objPOD2.Height = 45;
            objPOD2.Width = 700;
            objPOD2.PreviewMouseDown += new MouseButtonEventHandler(objPOD_PreviewMouseDown);
            objPOD2.SetValue(Canvas.LeftProperty, 0.0);
            objPOD2.SetValue(Canvas.TopProperty, 60.0);
            MyPropGrid.ControlToBind = objPOD2;
            objPOD2.cnvPOD.Children.Add(lblQuestion);
            currentControl = objPOD2;
            cnvPaint.Children.Add(objPOD2);

            ClsOptionCollection objOptCollection = ClsOptionCollection.GetAll(objQueCollection[CurrentQueCount].ID);

            varType = objQueCollection[CurrentQueCount].Category;

            if (varType == "CheckBox")
            {
                for (int i = 0; i < objOptCollection.Count; i++)
                {
                    CheckBox chk = new CheckBox();
                    chk.Content = objOptCollection[i].Options;
                    chk.Tag = objOptCollection[i].ActionQueueID.ToString() + "," + objOptCollection[i].ID.ToString();
                    chk.PreviewMouseDown += new MouseButtonEventHandler(chkDrag999_PreviewMouseDown);
                    chk.Height = 18;
                    chk.FontSize = 14;
                    varTop = 120 + (30 * i);

                    ctlPOD objPOD = new ctlPOD();
                    objPOD.AllowDrop = true;
                    objPOD.Height = 25;
                    objPOD.Width = 700;
                    objPOD.PreviewMouseDown += new MouseButtonEventHandler(objPOD_PreviewMouseDown);
                    objPOD.SetValue(Canvas.LeftProperty, 25.0);
                    objPOD.SetValue(Canvas.TopProperty, varTop);
                    MyPropGrid.ControlToBind = objPOD;
                    objPOD.cnvPOD.Children.Add(chk);
                    currentControl = objPOD;
                    cnvPaint.Children.Add(objPOD);
                }
            }
            else if (varType == "RadioButton")
            {
                for (int i = 0; i < objOptCollection.Count; i++)
                {
                    RadioButton chk = new RadioButton();
                    chk.Content = objOptCollection[i].Options;
                    chk.Tag = objOptCollection[i].ActionQueueID.ToString() + "," + objOptCollection[i].ID.ToString();
                    chk.PreviewMouseDown += new MouseButtonEventHandler(radDrag999_PreviewMouseDown);
                    chk.Height = 18;
                    chk.FontSize = 14;
                    varTop = 120 + (30 * i);

                    ctlPOD objPOD = new ctlPOD();
                    objPOD.AllowDrop = true;
                    objPOD.Height = 25;
                    objPOD.Width = 700;
                    objPOD.PreviewMouseDown += new MouseButtonEventHandler(objPOD_PreviewMouseDown);
                    objPOD.SetValue(Canvas.LeftProperty, 25.0);
                    objPOD.SetValue(Canvas.TopProperty, varTop);
                    MyPropGrid.ControlToBind = objPOD;
                    objPOD.cnvPOD.Children.Add(chk);
                    currentControl = objPOD;
                    cnvPaint.Children.Add(objPOD);
                }
            }
            else if (varType == "ListBox")
            {
                ListBox lst = new ListBox();
                lst.Height = 250;
                lst.Width = 700;
                lst.Tag = "code";
                lst.SetValue(Canvas.LeftProperty, 0.0);
                lst.SetValue(Canvas.TopProperty, 0.0);
                lst.PreviewMouseDown += new MouseButtonEventHandler(lstDrag999_PreviewMouseDown);
                //cnvMain.Children.Add(lst);

                for (int i = 0; i < objOptCollection.Count; i++)
                {
                    ListBoxItem lbi = new ListBoxItem();
                    //lbi.IsEnabled = false;
                    lbi.Content = objOptCollection[i].Options;
                    lbi.Tag = objOptCollection[i].ActionQueueID.ToString() + "," + objOptCollection[i].ID.ToString();
                    lst.Items.Add(lbi);
                }

                ctlPOD objPOD = new ctlPOD();
                objPOD.AllowDrop = true;
                objPOD.Height = 250;
                objPOD.Width = 700;
                objPOD.PreviewMouseDown += new MouseButtonEventHandler(objPOD_PreviewMouseDown);
                objPOD.SetValue(Canvas.LeftProperty, 25.0);
                objPOD.SetValue(Canvas.TopProperty, 120.0);
                MyPropGrid.ControlToBind = objPOD;
                objPOD.cnvPOD.Children.Add(lst);
                currentControl = objPOD;
                cnvPaint.Children.Add(objPOD);
            }
            else if (varType == "ComboBox")
            {
                ComboBox cmb = new ComboBox();
                cmb.Height = 25;
                cmb.Width = 700;
                cmb.Tag = "code";
                cmb.PreviewMouseDown += new MouseButtonEventHandler(cmbDrag999_PreviewMouseDown);
                //lst.PreviewMouseDown += new MouseButtonEventHandler(lstDrag999_PreviewMouseDown);
                //cnvMain.Children.Add(lst);

                for (int i = 0; i < objOptCollection.Count; i++)
                {
                    ComboBoxItem cbi = new ComboBoxItem();
                    //cbi.IsEnabled = false;
                    cbi.Content = objOptCollection[i].Options;
                    cbi.Tag = objOptCollection[i].ActionQueueID.ToString() + "," + objOptCollection[i].ID.ToString();
                    cmb.Items.Add(cbi);
                }

                ctlPOD objPOD = new ctlPOD();
                objPOD.AllowDrop = true;
                objPOD.Height = 25;
                objPOD.Width = 700;
                objPOD.PreviewMouseDown += new MouseButtonEventHandler(objPOD_PreviewMouseDown);
                objPOD.SetValue(Canvas.LeftProperty, 25.0);
                objPOD.SetValue(Canvas.TopProperty, 120.0);
                MyPropGrid.ControlToBind = objPOD;
                objPOD.cnvPOD.Children.Add(cmb);
                currentControl = objPOD;
                cnvPaint.Children.Add(objPOD);
            }

            Button btnNext = new Button();
            btnNext.Content = "Next >> ";
            // To Identify That is has some coding and it's Next Button //
            btnNext.Tag = varType.ToString().ToLower();
            btnNext.PreviewMouseDown += new MouseButtonEventHandler(btnDrag_PreviewMouseDown);
            btnNext.FontSize = 16;

            ctlPOD objPOD3 = new ctlPOD();
            objPOD3.AllowDrop = true;
            objPOD3.Height = 35;
            objPOD3.Width = 100;
            objPOD3.PreviewMouseDown += new MouseButtonEventHandler(objPOD_PreviewMouseDown);
            objPOD3.SetValue(Canvas.LeftProperty, 25.0);

            if (varType.ToLower() != "listbox" && varType.ToLower() != "combobox")
            {
                objPOD3.SetValue(Canvas.TopProperty, varTop + 35.0);
            }
            else if (varType.ToLower() == "listbox")
            {
                objPOD3.SetValue(Canvas.TopProperty, 400.0);
            }
            else if (varType.ToLower() == "combobox")
            {
                objPOD3.SetValue(Canvas.TopProperty, 200.0);
            }

            MyPropGrid.ControlToBind = objPOD3;
            objPOD3.cnvPOD.Children.Add(btnNext);
            currentControl = objPOD3;
            cnvPaint.Children.Add(objPOD3);
            varTop = 0;
        }*/

        /*   void btnYes_Click(object sender, RoutedEventArgs e)
           {
               btnPrev.IsEnabled = true;

               Canvas c = new Canvas();
               int count = cnvPaint.Children.Count - 1;

               for (int i = count; i >= 0; i--)
               {
                   if (cnvPaint.Children[i].GetType() != typeof(Expander) && cnvPaint.Children[i].GetType() != typeof(Rectangle))
                   {
                       object o = cnvPaint.Children[i];
                       cnvPaint.Children.Remove(cnvPaint.Children[i]);
                       c.Children.Add((UIElement)o);
                   }
               }

               c.Background = cnvPaint.Background;
               c.Height = cnvPaint.Height;
               c.Width = cnvPaint.Width;

               if (GeneratedQuestions > CurrentQueCount)
               {
                   lstCanvas[CurrentQueCount] = c;
                   CurrentQueCount++;
               }
               else
               {
                   GeneratedQuestions++;
                   CurrentQueCount++;
                   lstCanvas.Add(c);
               }

               if (btnNext.Content.ToString() == "Finished")
               {
                   MessageBox.Show("Script Generated");
                   FncSaveFiles();
                   goto rax;
               }

               if (CurrentQueCount == lstCanvas.Count)
               {
                   FncSetInnerCanvas();
               }
               else
               {
                   FncListToCanvas();
               }

               if (CurrentQueCount + 1 == objQueCollection.Count)
               {
                   btnNext.Content = "Finished";
               }

               //cnvPaint.Children.Clear();
           rax: ;
           }*/
        #endregion

        void FncSaveFiles()
        {

            if (startQuestion == "")
            {
                startQuestion = "CRMDesigner";
                startQuestionID = 1;
            }

            #region Saving Files

            CurrentQueCount = -1;
            
            for (int j = 0; j < 1; j++)
            {
                CurrentQueCount++;
                int Counter = 0;
                string strCode = "";
                

                #region Creating .XAML File
            
                string strXML = "";
                strXML = "<UserControl x:Class=\"CRM.Presentation.UserControl" +  Convert.ToString(1) + "\" xmlns=\"http://schemas.microsoft.com/winfx/2006/xaml/presentation\" xmlns:x=\"http://schemas.microsoft.com/winfx/2006/xaml\" Height=\"300\" Width=\"300\"  VerticalAlignment=\"Top\">";
                strXML = strXML + char.ConvertFromUtf32(13) + "<Canvas Name=\"cnvPaint" + "\" Height=\"" + (tbcMain.Height + 5).ToString() + "\" Width=\"" + (tbcMain.Width + 5).ToString() + "\" Background=\"Transparent\">";
                strXML = strXML + char.ConvertFromUtf32(13) + "<TabControl Name=\"tbcMain" + "\" Height=\"" + tbcMain.Height + "\" Width=\"" + tbcMain.Width + "\" Canvas.Left=\"0\" Canvas.Top=\"0\">";
                foreach (TabItem t in tbcMain.Items)
                {
                    pageCount++;
                    strXML = strXML + char.ConvertFromUtf32(13) + "<TabItem Header=\"" + t.Header.ToString() +"\">";
                    strXML = strXML + char.ConvertFromUtf32(13) + "<Canvas Name=\"cnvPaint" + pageCount.ToString() + "\" Height=\"" + tbcMain.Height + "\" Width=\"" + tbcMain.Width + "\" Background=\"Transparent\">";
                    
                foreach (object o in ((Canvas)t.Content).Children)
                {
                    if (o.GetType() == typeof(ctlPOD))
                    {
                        foreach (object chl in ((ctlPOD)o).cnvPOD.Children)
                        {
                            Counter++;
                            if (chl.GetType() == typeof(TextBox))
                            {
                               strXML = strXML + char.ConvertFromUtf32(13) + "<TextBox Name=\"control" + Counter.ToString() + "\" Height=\"" + ((ctlPOD)o).Height + "\" Width=\"" + ((ctlPOD)o).Width + "\" Canvas.Left=\"" + ((ctlPOD)o).GetValue(Canvas.LeftProperty) + "\" Canvas.Top=\"" + ((ctlPOD)o).GetValue(Canvas.TopProperty) + "\" Background=\"" + ((TextBox)chl).Background + "\" Foreground=\"" + ((TextBox)chl).Foreground + "\" Tag=\"" + ((TextBox)chl).Tag + "\" Text=\"" + ((TextBox)chl).Text + "\" FontFamily=\"" + ((TextBox)chl).FontFamily + "\" FontSize=\"" + ((TextBox)chl).FontSize + "\" IsEnabled=\"" + ((ctlPOD)o).IsEnabled + "\" FontWeight=\"" + ((TextBox)chl).FontWeight + "\" FontStyle=\"" + ((TextBox)chl).FontStyle + "\"/>";
                            }


                            else if (chl.GetType() == typeof(Label))
                            {
                                strXML = strXML + char.ConvertFromUtf32(13) + "<Label Name=\"control" + Counter.ToString() + "\" Height=\"" + ((ctlPOD)o).Height + "\" Width=\"" + ((ctlPOD)o).Width + "\" Canvas.Left=\"" + ((ctlPOD)o).GetValue(Canvas.LeftProperty) + "\" Canvas.Top=\"" + ((ctlPOD)o).GetValue(Canvas.TopProperty) + "\" Background=\"" + ((Label)chl).Background + "\" Foreground=\"" + ((Label)chl).Foreground + "\" Tag=\"" + ((Label)chl).Tag + "\" Content=\"" + ((Label)chl).Content + "\" FontFamily=\"" + ((Label)chl).FontFamily + "\" FontSize=\"" + ((Label)chl).FontSize + "\" FontWeight=\"" + ((Label)chl).FontWeight + "\" FontStyle=\"" + ((Label)chl).FontStyle + "\"/>";
                            }

                        }
                    }
                }
                strXML = strXML + char.ConvertFromUtf32(13) + "</Canvas></TabItem>";
                }
                strXML = strXML + "</TabControl>";
                strXML = strXML + " <Button Name=\"btnSave\" Height=\"25\" Width=\"70\" Canvas.Top=\"" + int.Parse((int.Parse(tbcMain.Height.ToString()) + 5).ToString()).ToString() + "\" Canvas.Left=\"100\" Content=\"Save\" Click=\"btnSave_Click\" /> <Button Name=\"btnCancel\" Height=\"25\" Width=\"70\" Canvas.Top=\"" + int.Parse((int.Parse(tbcMain.Height.ToString()) + 5).ToString()).ToString() + "\" Canvas.Left=\"200\" Content=\"Cancel\" Click=\"btnCancel_Click\" /> </Canvas></UserControl>";

                TextWriter tw = new StreamWriter(Assembly.GetAssembly(this.GetType()).Location.Replace("CRMDesigner.Presentation.dll", "\\CRM_src\\CRM.Presentation\\" + startQuestion.ToString() + ".Xaml"));
                tw.WriteLine(strXML);
                tw.Close();

                TextReader tr101 = new StreamReader(Assembly.GetAssembly(this.GetType()).Location.Replace("CRMDesigner.Presentation.dll","\\CRMPages\\Binded Data.txt"));
                string str101 = tr101.ReadToEnd();
                tr101.Close();


                strXML = "using System;" + char.ConvertFromUtf32(13) +
                         "using System.Collections.Generic;" + char.ConvertFromUtf32(13) +
                         "using System.Linq;" + char.ConvertFromUtf32(13) +
                         "using System.Text;" + char.ConvertFromUtf32(13) +
                         "using System.Windows;" + char.ConvertFromUtf32(13) +
                         "using System.Windows.Controls;" + char.ConvertFromUtf32(13) +
                         "using System.Windows.Data;" + char.ConvertFromUtf32(13) +
                         "using System.Windows.Documents;" + char.ConvertFromUtf32(13) +
                         "using System.Windows.Input;" + char.ConvertFromUtf32(13) +
                         "using System.Windows.Media;" + char.ConvertFromUtf32(13) +
                         "using System.Windows.Media.Imaging;" + char.ConvertFromUtf32(13) +
                         "using System.Windows.Navigation;" + char.ConvertFromUtf32(13) +
                         "using System.Windows.Shapes;" + char.ConvertFromUtf32(13) +
                         "using CRM.Business;" + char.ConvertFromUtf32(13) +
                         char.ConvertFromUtf32(13) +

                         "namespace CRM.Presentation" + char.ConvertFromUtf32(13) +
                         "{" + char.ConvertFromUtf32(13) + 
                          "" + char.ConvertFromUtf32(13) +
                            "public partial class UserControl" + Convert.ToString(1) + ": UserControl" + char.ConvertFromUtf32(13) +
                                "{" + char.ConvertFromUtf32(13) + "" + char.ConvertFromUtf32(13) +
                                    "public UserControl" + Convert.ToString(1) + "()" + char.ConvertFromUtf32(13) +
                                     "{" + char.ConvertFromUtf32(13) +
                                     "InitializeComponent();" + char.ConvertFromUtf32(13)  + char.ConvertFromUtf32(13) +
                                     "VMuktiAPI.VMuktiHelper.RegisterEvent(\"SetLeadIDCRM\").VMuktiEvent += new VMuktiAPI.VMuktiEvents.VMuktiEventHandler(SetLeadIDCRM_VMuktiEvent);" + char.ConvertFromUtf32(13) +
                                     "}" + str101 + char.ConvertFromUtf32(13) + char.ConvertFromUtf32(13) + strCode + char.ConvertFromUtf32(13) +
                                     "}" + char.ConvertFromUtf32(13) +
                                     "}"; 

                TextWriter tw1 = new StreamWriter(Assembly.GetAssembly(this.GetType()).Location.Replace("CRMDesigner.Presentation.dll", "\\CRM_src\\CRM.Presentation\\" + startQuestion.ToString() + ".Xaml.Cs"));
                tw1.WriteLine(strXML);
                tw1.Close();

                AddRef1_2 = AddRef1_2 + char.ConvertFromUtf32(13) + "<Page Include=\"" + startQuestion.ToString() + ".xaml\">"
                + char.ConvertFromUtf32(13) + "<Generator>MSBuild:Compile</Generator>"
                + char.ConvertFromUtf32(13) + "<SubType>Designer</SubType>"
                + char.ConvertFromUtf32(13) + "</Page>";

                AddRef2_3 = AddRef2_3 + char.ConvertFromUtf32(13) + "<Compile Include=\"" + startQuestion.ToString() + ".Xaml.Cs\">"
                + char.ConvertFromUtf32(13) + "<DependentUpon>" + startQuestion.ToString() + ".Xaml</DependentUpon>"
                + char.ConvertFromUtf32(13) + "<SubType>Code</SubType>"
                + char.ConvertFromUtf32(13) + "</Compile>";


                #endregion
            }

            #region Create Project File


            TextWriter tw11 = new StreamWriter(Assembly.GetAssembly(this.GetType()).Location.Replace("CRMDesigner.Presentation.dll", "\\CRM_src\\CRM.Presentation\\CRM.Presentation.csproj"));

            TextReader tr1 = new StreamReader(Assembly.GetAssembly(this.GetType()).Location.Replace("CRMDesigner.Presentation.dll", "\\CRMPages\\1.txt"));
            string str1 = tr1.ReadToEnd();
            tr1.Close();

            TextReader tr2 = new StreamReader(Assembly.GetAssembly(this.GetType()).Location.Replace("CRMDesigner.Presentation.dll", "\\CRMPages\\2.txt"));
            string str2 = tr2.ReadToEnd();
            tr2.Close();

            TextReader tr3 = new StreamReader(Assembly.GetAssembly(this.GetType()).Location.Replace("CRMDesigner.Presentation.dll", "\\CRMPages\\3.txt"));
            string str3 = tr3.ReadToEnd();
            tr3.Close();

            tw11.WriteLine(str1 + AddRef2_3 + str2 + AddRef1_2 + str3);
            str1 = "";
            str2 = "";
            str3 = "";
            AddRef1_2 = "";
            AddRef2_3 = "";
            tw11.Close();

            TextWriter twStart = new StreamWriter(Assembly.GetAssembly(this.GetType()).Location.Replace("CRMDesigner.Presentation.dll", "\\CRM_src\\CRM.Presentation\\clsStartClass.cs"));
            TextReader trClassRead = new StreamReader(Assembly.GetAssembly(this.GetType()).Location.Replace("CRMDesigner.Presentation.dll", "\\CRMPages\\ClassRead.txt"));
            string strClassRead = trClassRead.ReadToEnd();
            trClassRead.Close();

            string strBetween = char.ConvertFromUtf32(13) + "public static string strStartQuesion = \"UserControl" + startQuestionID.ToString() + "\";" + char.ConvertFromUtf32(13) + "public static int LeadID = 1;" + char.ConvertFromUtf32(13) + "public static int CallID;" + char.ConvertFromUtf32(13) + "public static string StateName = null;" + char.ConvertFromUtf32(13) + "public static string ZipCode = null;" + char.ConvertFromUtf32(13) + "}" + char.ConvertFromUtf32(13) + "}";

            twStart.WriteLine(strClassRead + strBetween);
            twStart.Close();

            #endregion


            #region After Integration
            //if (!Directory.Exists(AppDomain.CurrentDomain.BaseDirectory + "CRM_src\\Script.DataAccess\\ReferencedAssemblies"))
            //{
            //    Directory.CreateDirectory(AppDomain.CurrentDomain.BaseDirectory + "CRM_src\\Script.DataAccess\\ReferencedAssemblies");
            //}

            //File.Copy(Assembly.GetAssembly(typeof(VMuktiAPI.ClsPeer)).Location, Assembly.GetAssembly(this.GetType()).Location.Replace("ScriptDesigner.Presentation.dll", @"\Script_src\Script.DataAccess\ReferencedAssemblies\VMuktiAPI.dll"));
            
            #endregion

            
            //#region Building the solution File

            //TextWriter tw1111 = new StreamWriter(AppDomain.CurrentDomain.BaseDirectory + "\\Script\\Build.bat");
            //tw1111.WriteLine("cd\\");
            //tw1111.WriteLine("C:");
            //tw1111.WriteLine("cd \"C:\\WINDOWS\\Microsoft.NET\\Framework\\v3.5\"");
            //tw1111.WriteLine("msbuild \"" + AppDomain.CurrentDomain.BaseDirectory + "\\Script\\Script.sln\" /t:Rebuild /p:Configuration=Debug");
            //tw1111.Close();

            //System.Diagnostics.Process.Start(AppDomain.CurrentDomain.BaseDirectory + "\\Script\\Build.bat");

            //#endregion

            //MessageBox.Show("Working Till Now");

            #region DLL File Naming

            if (cmbCRM.SelectedIndex < 0)
            {
                MessageBox.Show("Please select a CRM Name");
            }
            else
            {
                string crmName = ((ComboBoxItem)cmbCRM.SelectedItem).Content.ToString();

                TextReader txtReader = new StreamReader(Assembly.GetAssembly(this.GetType()).Location.Replace("CRMDesigner.Presentation.dll", "\\CRM_src\\CRM.Presentation\\CRM.Presentation.csproj"));
                string txtString = txtReader.ReadToEnd();
                txtReader.Close();

                txtString = txtString.Replace("<AssemblyName>CRM.Presentation</AssemblyName>", "<AssemblyName>" + crmName + "_CRM.Presentation</AssemblyName>");

                TextWriter txtWriter = new StreamWriter(Assembly.GetAssembly(this.GetType()).Location.Replace("CRMDesigner.Presentation.dll", "\\CRM_src\\CRM.Presentation\\CRM.Presentation.csproj"));
                txtWriter.WriteLine(txtString);
                txtWriter.Close();



                TextReader txtReader1 = new StreamReader(Assembly.GetAssembly(this.GetType()).Location.Replace("CRMDesigner.Presentation.dll", "\\CRM_src\\CRM.Business\\CRM.Business.csproj"));
                string txtString1 = txtReader1.ReadToEnd();
                txtReader1.Close();

                txtString1 = txtString1.Replace("<AssemblyName>CRM.Business</AssemblyName>", "<AssemblyName>" + crmName + "_CRM.Business</AssemblyName>");

                TextWriter txtWriter1 = new StreamWriter(Assembly.GetAssembly(this.GetType()).Location.Replace("CRMDesigner.Presentation.dll", "\\CRM_src\\CRM.Business\\CRM.Business.csproj"));
                txtWriter1.WriteLine(txtString1);
                txtWriter1.Close();



                TextReader txtReader2 = new StreamReader(Assembly.GetAssembly(this.GetType()).Location.Replace("CRMDesigner.Presentation.dll", "\\CRM_src\\CRM.Common\\CRM.Common.csproj"));
                string txtString2 = txtReader2.ReadToEnd();
                txtReader2.Close();

                txtString2 = txtString2.Replace("<AssemblyName>CRM.Common</AssemblyName>", "<AssemblyName>" + crmName + "_CRM.Common</AssemblyName>");

                TextWriter txtWriter2 = new StreamWriter(Assembly.GetAssembly(this.GetType()).Location.Replace("CRMDesigner.Presentation.dll", "\\CRM_src\\CRM.Common\\CRM.Common.csproj"));
                txtWriter2.WriteLine(txtString2);
                txtWriter2.Close();

                TextReader txtReader3 = new StreamReader(Assembly.GetAssembly(this.GetType()).Location.Replace("CRMDesigner.Presentation.dll", "\\CRM_src\\CRM.DataAccess\\CRM.DataAccess.csproj"));
                string txtString3 = txtReader3.ReadToEnd();
                txtReader3.Close();

                txtString3 = txtString3.Replace("<AssemblyName>CRM.DataAccess</AssemblyName>", "<AssemblyName>" + crmName + "_CRM.DataAccess</AssemblyName>");

                TextWriter txtWriter3 = new StreamWriter(Assembly.GetAssembly(this.GetType()).Location.Replace("CRMDesigner.Presentation.dll", "\\CRM_src\\CRM.DataAccess\\CRM.DataAccess.csproj"));
                txtWriter3.WriteLine(txtString3);
                txtWriter3.Close();

            #endregion

                #region Building the solution File

                string winDir = Environment.GetFolderPath(Environment.SpecialFolder.System);
                winDir = winDir.Substring(0, winDir.LastIndexOf(@"\"));

                Process buildProcess = new Process();

                File.Copy(Assembly.GetAssembly(typeof(VMuktiAPI.ClsPeer)).Location, Assembly.GetAssembly(this.GetType()).Location.Replace("CRMDesigner.Presentation.dll", @"\CRM_src\CRM.DataAccess\ReferencedAssemblies\VMuktiAPI.dll"));
                //MessageBox.Show(Assembly.GetAssembly(typeof(VMuktiAPI.ClsPeer)).Location);
                //MessageBox.Show(Assembly.GetAssembly(this.GetType()).Location.Replace("ScriptDesigner.Presentation.dll", @"Script_src\Script.Presentation\ReferencedAssemblies\VMuktiAPI.dll"));
                File.Copy(Assembly.GetAssembly(typeof(VMuktiAPI.ClsPeer)).Location, Assembly.GetAssembly(this.GetType()).Location.Replace("CRMDesigner.Presentation.dll", @"CRM_src\CRM.Presentation\ReferencedAssemblies\VMuktiAPI.dll"));

                buildProcess.OutputDataReceived += new DataReceivedEventHandler(buildProcess_OutputDataReceived);
                //TextWriter tw1111 = new StreamWriter(AppDomain.CurrentDomain.BaseDirectory + "\\Script\\Build.bat");
                //tw1111.WriteLine("cd\\");
                //tw1111.WriteLine("C:");
                //tw1111.WriteLine("cd \"C:\\WINDOWS\\Microsoft.NET\\Framework\\v3.5\"");
                //tw1111.WriteLine("msbuild \"" + AppDomain.CurrentDomain.BaseDirectory + "\\Script\\Script.sln\" /t:Rebuild /p:Configuration=Debug");
                //tw1111.Close();
                //System.Diagnostics.Process.Start(AppDomain.CurrentDomain.BaseDirectory + "\\Script\\Build.bat");

                try
                {

                    if (System.IO.Directory.Exists(winDir + @"\Microsoft.NET\Framework\v3.5\"))
                    {
                        //buildProcess.StartInfo.UseShellExecute = false;
                        buildProcess.StartInfo.WorkingDirectory = winDir + @"\Microsoft.NET\Framework\v3.5\";
                        buildProcess.StartInfo.FileName = "msbuild";
                        buildProcess.StartInfo.Arguments = @" """ + Assembly.GetAssembly(this.GetType()).Location.Replace("CRMDesigner.Presentation.dll", "") + @"\CRM_src\CRM.sln"" /t:Rebuild /p:Configuration=Debug";
                        buildProcess.Start();
                        buildProcess.WaitForExit();
                    }
                    else
                    {
                        MessageBox.Show("Microsoft .Net framework is not installed at --> \" " + winDir + @"\Microsoft.NET\Framework\v3.5\" + " \"!!");
                    }
                }
                catch (Exception exp)
                {
                    MessageBox.Show(exp.Message.ToString());
                }

                #endregion

                try
                {

                    if (Directory.Exists(Assembly.GetAssembly(this.GetType()).Location.Replace("CRMDesigner.Presentation.dll", "") + crmName + "_CRM"))
                    {
                        Directory.Delete(Assembly.GetAssembly(this.GetType()).Location.Replace("CRMDesigner.Presentation.dll", "") + crmName + "_CRM", true);
                    }
                    Directory.CreateDirectory(Assembly.GetAssembly(this.GetType()).Location.Replace("CRMDesigner.Presentation.dll", "") + crmName + "_CRM");
                    Directory.CreateDirectory(Assembly.GetAssembly(this.GetType()).Location.Replace("CRMDesigner.Presentation.dll", "") + crmName + "_CRM" + "\\" + crmName + "_CRM");
                    Directory.CreateDirectory(Assembly.GetAssembly(this.GetType()).Location.Replace("CRMDesigner.Presentation.dll", "") + crmName + "_CRM" + "\\" + crmName + "_CRM" + @"\BAL");
                    Directory.CreateDirectory(Assembly.GetAssembly(this.GetType()).Location.Replace("CRMDesigner.Presentation.dll", "") + crmName + "_CRM" + "\\" + crmName + "_CRM" + @"\DAL");
                    Directory.CreateDirectory(Assembly.GetAssembly(this.GetType()).Location.Replace("CRMDesigner.Presentation.dll", "") + crmName + "_CRM" + "\\" + crmName + "_CRM" + @"\Control");


                }
                catch (Exception exp)
                {
                    MessageBox.Show("Creating Directories" + exp.Message);
                }

                try
                {
                    copyDirectory(Assembly.GetAssembly(this.GetType()).Location.Replace("CRMDesigner.Presentation.dll", "") + @"CRM_src\CRM.Presentation\bin\Debug", Assembly.GetAssembly(this.GetType()).Location.Replace("CRMDesigner.Presentation.dll", "") + @"CRM");
                }
                catch (Exception exp)
                {
                    MessageBox.Show("Creating Directories" + exp.Message);
                }

                try
                {
                    string[] filesToDelete = Directory.GetFiles(Assembly.GetAssembly(this.GetType()).Location.Replace("CRMDesigner.Presentation.dll", "") + @"CRM", "*.pdb");
                    for (int i = 0; i < filesToDelete.Length; i++)
                    {
                        File.Delete(filesToDelete[i]);
                    }
                }
                catch (Exception exp)
                {
                    MessageBox.Show("Deleting Files" + exp.Message);
                }

                try
                {

                    string[] filesToMove = Directory.GetFiles(Assembly.GetAssembly(this.GetType()).Location.Replace("CRMDesigner.Presentation.dll", "") + @"CRM", "*.dll");
                    for (int i = 0; i < filesToMove.Length; i++)
                    {
                        if (filesToMove[i].Substring(filesToMove[i].LastIndexOf(@"\")).Contains("Business"))
                        {
                            File.Move(filesToMove[i], Assembly.GetAssembly(this.GetType()).Location.Replace("CRMDesigner.Presentation.dll", "") + crmName + "_CRM" + "\\" + crmName + "_CRM" + @"\BAL" + filesToMove[i].Substring(filesToMove[i].LastIndexOf(@"\")));
                        }
                        else if (filesToMove[i].Substring(filesToMove[i].LastIndexOf(@"\")).Contains("Common"))
                        {
                            File.Move(filesToMove[i], Assembly.GetAssembly(this.GetType()).Location.Replace("CRMDesigner.Presentation.dll", "") + crmName + "_CRM" + "\\" + crmName + "_CRM" + @"\BAL" + filesToMove[i].Substring(filesToMove[i].LastIndexOf(@"\")));
                        }
                        else if (filesToMove[i].Substring(filesToMove[i].LastIndexOf(@"\")).Contains("DataAccess"))
                        {
                            File.Move(filesToMove[i], Assembly.GetAssembly(this.GetType()).Location.Replace("CRMDesigner.Presentation.dll", "") + crmName + "_CRM" + "\\" + crmName + "_CRM" + @"\DAL" + filesToMove[i].Substring(filesToMove[i].LastIndexOf(@"\")));
                        }
                        else if (filesToMove[i].Substring(filesToMove[i].LastIndexOf(@"\")).Contains("Presentation"))
                        {
                            File.Move(filesToMove[i], Assembly.GetAssembly(this.GetType()).Location.Replace("CRMDesigner.Presentation.dll", "") + crmName + "_CRM" + "\\" + crmName + "_CRM" + @"\Control" + filesToMove[i].Substring(filesToMove[i].LastIndexOf(@"\")));
                        }
                        else if (filesToMove[i].Substring(filesToMove[i].LastIndexOf(@"\")).Contains("VMuktiAPI"))
                        {

                            File.Delete(filesToMove[i]);

                            //File.Move(filesToMove[i], Assembly.GetAssembly(this.GetType()).Location.Replace("ScriptDesigner.Presentation.dll", "") + @"Script\Control\" + filesToMove[i].Substring(filesToMove[i].LastIndexOf(@"\")));
                        }
                    }

                }
                catch (Exception exp)
                {
                    MessageBox.Show("Move Files" + exp.Message);
                }

                try
                {

                    Directory.Delete(Assembly.GetAssembly(this.GetType()).Location.Replace("CRMDesigner.Presentation.dll", "") + @"CRM_src\CRM.Presentation\bin", true);
                    Directory.Delete(Assembly.GetAssembly(this.GetType()).Location.Replace("CRMDesigner.Presentation.dll", "") + @"CRM_src\CRM.Presentation\obj", true);
                    Directory.Delete(Assembly.GetAssembly(this.GetType()).Location.Replace("CRMDesigner.Presentation.dll", "") + @"CRM_src\CRM.Business\bin", true);
                    Directory.Delete(Assembly.GetAssembly(this.GetType()).Location.Replace("CRMDesigner.Presentation.dll", "") + @"CRM_src\CRM.Business\obj", true);
                    Directory.Delete(Assembly.GetAssembly(this.GetType()).Location.Replace("CRMDesigner.Presentation.dll", "") + @"CRM_src\CRM.Common\bin", true);
                    Directory.Delete(Assembly.GetAssembly(this.GetType()).Location.Replace("CRMDesigner.Presentation.dll", "") + @"CRM_src\CRM.Common\obj", true);
                    Directory.Delete(Assembly.GetAssembly(this.GetType()).Location.Replace("CRMDesigner.Presentation.dll", "") + @"CRM_src\CRM.DataAccess\bin", true);
                    Directory.Delete(Assembly.GetAssembly(this.GetType()).Location.Replace("CRMDesigner.Presentation.dll", "") + @"CRM_src\CRM.DataAccess\obj", true);
                }
                catch (Exception exp)
                {
                    MessageBox.Show("Deleting Files" + exp.Message);
                }

                ICSharpCode.SharpZipLib.Zip.FastZip fz = new ICSharpCode.SharpZipLib.Zip.FastZip();
                try
                {
                    fz.CreateZip(Assembly.GetAssembly(this.GetType()).Location.Replace("CRMDesigner.Presentation.dll", "") + @"CRM_src.zip", Assembly.GetAssembly(this.GetType()).Location.Replace("CRMDesigner.Presentation.dll", "") + @"CRM_src", true, "");
                    //fz.CreateZip(AppDomain.CurrentDomain.BaseDirectory + @"CRM_src.zip", AppDomain.CurrentDomain.BaseDirectory + @"CRM_src", true, "");
                }
                catch (Exception exp)
                {
                    MessageBox.Show("Create Zip" + exp.Message);
                }

                try
                {
                    fz.CreateZip(Assembly.GetAssembly(this.GetType()).Location.Replace("CRMDesigner.Presentation.dll", "") + crmName + "_CRM" + @".zip", Assembly.GetAssembly(this.GetType()).Location.Replace("CRMDesigner.Presentation.dll", "") + crmName + "_CRM", true, "");
                    //fz.CreateZip(AppDomain.CurrentDomain.BaseDirectory + @"CRM.zip", AppDomain.CurrentDomain.BaseDirectory + @"CRM", true, "");
                }
                catch (Exception exp)
                {
                    MessageBox.Show("Create Zip" + exp.Message);
                }

                try
                {
                    Directory.Delete(Assembly.GetAssembly(this.GetType()).Location.Replace("CRMDesigner.Presentation.dll", "") + @"CRM_src", true);
                    //Directory.Delete(AppDomain.CurrentDomain.BaseDirectory + @"CRM_src", true);
                }
                catch (Exception exp)
                {
                    MessageBox.Show("Delete Directory" + exp.Message);
                }

                try
                {
                    Directory.Delete(Assembly.GetAssembly(this.GetType()).Location.Replace("CRMDesigner.Presentation.dll", "") + crmName + "_CRM", true);
                    //Directory.Delete(AppDomain.CurrentDomain.BaseDirectory + @"CRM", true);
                }
                catch (Exception exp)
                {
                    MessageBox.Show("Delete Directory" + exp.Message);
                }

                try
                {
                    System.Windows.Forms.DialogResult sfdRes;
                    System.Windows.Forms.SaveFileDialog sfd = new System.Windows.Forms.SaveFileDialog();
                    sfd.Filter = "Zip file(s) (*.zip)|*.zip";
                    sfd.Title = "Save the source file of script to edit manualy!!";
                    sfd.FileName = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + @"\CRM_src.zip";
                    sfdRes = sfd.ShowDialog();

                    if (sfdRes == System.Windows.Forms.DialogResult.OK)
                    {
                        if (File.Exists(sfd.FileName))
                        {
                            File.Delete(sfd.FileName);
                        }
                        File.Move(Assembly.GetAssembly(this.GetType()).Location.Replace("CRMDesigner.Presentation.dll", "") + @"CRM_src.zip", sfd.FileName);
                    }

                    //sfd.Title = "Save the file of script to add as a module in VMukti plateform!!";
                    //sfd.FileName = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\" + crmName + @".zip";
                    //sfdRes = sfd.ShowDialog();
                    //if (sfdRes == System.Windows.Forms.DialogResult.OK)
                    //{
                    //    if (File.Exists(sfd.FileName))
                    //    {
                    //        File.Delete(sfd.FileName);
                    //    }

                    #region UploadFile

                    System.IO.FileInfo fileInfo = new System.IO.FileInfo(Assembly.GetAssembly(this.GetType()).Location.Replace("CRMDesigner.Presentation.dll", "") + crmName + "_CRM" + @".zip");
                    System.IO.FileStream stream = new System.IO.FileStream(Assembly.GetAssembly(this.GetType()).Location.Replace("CRMDesigner.Presentation.dll", "") + crmName + "_CRM" + @".zip", System.IO.FileMode.Open, System.IO.FileAccess.Read);
                    RemoteFileInfo rfi = new RemoteFileInfo();
                    rfi.FileName = fileInfo.Name;
                    rfi.Length = fileInfo.Length;
                    rfi.FileByteStream = stream;
                    rfi.FolderNameToStore = "CRMs";
                    clientHttpFileTransfer.svcHTTPFileTransferServiceUploadFileToInstallationDirectory(rfi);
                    stream.Close();
                    if (File.Exists(Assembly.GetAssembly(this.GetType()).Location.Replace("CRMDesigner.Presentation.dll", "") + crmName + "_CRM" + @".zip"))
                    {
                        File.Delete(Assembly.GetAssembly(this.GetType()).Location.Replace("CRMDesigner.Presentation.dll", "") + crmName + "_CRM" + @".zip");
                    }


                    #endregion

                }
                catch (Exception exp)
                {
                }

            #endregion
            }
        }

        void buildProcess_OutputDataReceived(object sender, DataReceivedEventArgs e)
        {
            try
            {
                MessageBox.Show(e.Data.ToString());
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                ex.Data.Add("My Key", "VMukti--:--VmuktiModules--:--Call Center--:--CRMDesigner--:--CRMDesigner.Presentation--:--ctlCRMDesigner.xaml.cs--:--buildProcess_OutputDataReceived()--");
                //ClsException.LogError(ex);
                //ClsException.WriteToErrorLogFile(ex);
                System.Text.StringBuilder sb = new StringBuilder();
                sb.AppendLine(ex.Message);
                sb.AppendLine();
                sb.AppendLine("StackTrace : " + ex.StackTrace);
                sb.AppendLine();
                sb.AppendLine("Location : " + ex.Data["My Key"].ToString());
                sb.AppendLine();
                sb1 = CreateTressInfo();
                sb.Append(sb1.ToString());
                VMuktiAPI.ClsLogging.WriteToTresslog(sb);
            }
        }

        private void txtHeader_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                ((TabItem)tbcMain.SelectedItem).Header = txtHeader.Text;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                ex.Data.Add("My Key", "VMukti--:--VmuktiModules--:--Call Center--:--CRMDesigner--:--CRMDesigner.Presentation--:--ctlCRMDesigner.xaml.cs--:--txtHeader_TextChanged()--");
                //ClsException.LogError(ex);
                //ClsException.WriteToErrorLogFile(ex);
                System.Text.StringBuilder sb = new StringBuilder();
                sb.AppendLine(ex.Message);
                sb.AppendLine();
                sb.AppendLine("StackTrace : " + ex.StackTrace);
                sb.AppendLine();
                sb.AppendLine("Location : " + ex.Data["My Key"].ToString());
                sb.AppendLine();
                sb1 = CreateTressInfo();
                sb.Append(sb1.ToString());
                VMuktiAPI.ClsLogging.WriteToTresslog(sb);
                

            }
        }


        void FncCreateVirtualDirectory()
        {
            System.DirectoryServices.DirectoryEntry oDE;
            System.DirectoryServices.DirectoryEntries oDC;
            System.DirectoryServices.DirectoryEntry oVirDir;
            try
            {
                oDE = new DirectoryEntry("IIS://" + VMuktiAPI.VMuktiInfo.BootStrapIPs[0] + "/MSFTPSVC/1/Root");
                oDC = oDE.Children;
                try
                {
                    oVirDir = oDC.Add("VMukti", oDE.SchemaClassName.ToString());
                }
                catch (Exception ex)
                {
                    ex.Data.Add("My Key", "VMukti--:--VmuktiModules--:--Call Center--:--CRMDesigner--:--CRMDesigner.Presentation--:--ctlCRMDesigner.xaml.cs--:--FncCreateVirtualDirectory()--");
                    oDC.Remove(new DirectoryEntry("IIS://" + VMuktiAPI.VMuktiInfo.BootStrapIPs[0] + "/MSFTPSVC/1/Root/Vmukti"));
                    oVirDir = oDC.Add("VMukti", oDE.SchemaClassName.ToString());
                    System.Text.StringBuilder sb = new StringBuilder();
                    sb.AppendLine(ex.Message);
                    sb.AppendLine();
                    sb.AppendLine("StackTrace : " + ex.StackTrace);
                    sb.AppendLine();
                    sb.AppendLine("Location : " + ex.Data["My Key"].ToString());
                    sb.AppendLine();
                    sb1 = CreateTressInfo();
                    sb.Append(sb1.ToString());
                    VMuktiAPI.ClsLogging.WriteToTresslog(sb);
                }
                oVirDir.CommitChanges();

                if (!Directory.Exists("C:\\Program Files\\Default Company Name\\VMukti\\Adiance"))
                {
                    Directory.CreateDirectory("C:\\Program Files\\Default Company Name\\VMukti\\Adiance");
                }
                oVirDir.Properties["Path"].Value = "C:\\Program Files\\Default Company Name\\VMukti\\Adiance";

                oVirDir.Properties["AccessRead"][0] = true;
                oVirDir.Properties["AccessWrite"][0] = true;
                oVirDir.Properties["AccessExecute"][0] = true;
                oVirDir.CommitChanges();
            }
            catch (Exception exc)
            {
                exc.Data.Add("My Key", "VMukti--:--VmuktiModules--:--Call Center--:--CRMDesigner--:--CRMDesigner.Presentation--:--ctlCRMDesigner.xaml.cs--:--FncCreateVirtualDirectory()--");
                MessageBox.Show(exc.Message);
                System.Text.StringBuilder sb = new StringBuilder();
                sb.AppendLine(exc.Message);
                sb.AppendLine();
                sb.AppendLine("StackTrace : " + exc.StackTrace);
                sb.AppendLine();
                sb.AppendLine("Location : " + exc.Data["My Key"].ToString());
                sb.AppendLine();
                sb1 = CreateTressInfo();
                sb.Append(sb1.ToString());
                VMuktiAPI.ClsLogging.WriteToTresslog(sb);
            }

        }
        
    }
}