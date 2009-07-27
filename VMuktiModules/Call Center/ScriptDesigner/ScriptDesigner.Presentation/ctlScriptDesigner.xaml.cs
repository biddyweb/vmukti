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
using ScriptDesigner.Business;
using System.IO;
using System.Diagnostics;
using System.Reflection;
using System.Net;
using System.DirectoryServices;
using VMuktiAPI;
using VMuktiService;
using System.Collections;
using System.ServiceModel;


namespace ScriptDesigner.Presentation
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


    
    public partial class ctlScriptDesigner : UserControl
    {

        #region Properties

        private int  _ScriptID = 0;
        private string _ScriptName = "";

        public int SctiptID
        {
            get { return _ScriptID; }
            set { _ScriptID = value; }
        }
        public string ScriptName
        {
            get { return _ScriptName; }
            set { _ScriptName = value; }
        }

        #endregion

        int newDrag = 1;
        int pageCount = 0;
        Point PrePoint = new Point();

        //string ftpUsername = "administrator";
        //string ftpPassword = "vmukti";
       
        //ClsControlInfoCollection objControlInfoCollection = new ClsControlInfoCollection();

        // Raxit Code //

        ctlPOD currentControl = new ctlPOD();
        ctlPropertyGrid MyPropGrid = new ctlPropertyGrid();

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
        //double varHeight;

        ComboBox cmbQuest = new ComboBox();
        Button btnBindQuestion = new Button();
        Boolean flag;
        public static IHTTPFileTransferService clientHttpFileTransfer = null;

        /////

        //System.Windows.Forms.PropertyGrid propGrid = new System.Windows.Forms.PropertyGrid();

        // Raxit code //

        public ctlScriptDesigner(string sName)
        {
            InitializeComponent();

            ScriptName = sName;
            this.Loaded += new RoutedEventHandler(ctlScriptDesigner_Loaded);

            FncFillCombo();
            cmbScript.SelectionChanged += new SelectionChangedEventHandler(cmbScript_SelectionChanged);
            flag = false;
            btnDrag.PreviewMouseDown += new MouseButtonEventHandler(btnDrag_PreviewMouseDown);
            txtDrag999.PreviewMouseDown += new MouseButtonEventHandler(txtDrag_PreviewMouseDown);
            lblDrag999.PreviewMouseDown += new MouseButtonEventHandler(lblDrag_PreviewMouseDown);
            cmbDrag999.PreviewMouseDown += new MouseButtonEventHandler(cmbDrag999_PreviewMouseDown);
            lstDrag999.PreviewMouseDown += new MouseButtonEventHandler(lstDrag999_PreviewMouseDown);
            chkDrag999.PreviewMouseDown += new MouseButtonEventHandler(chkDrag999_PreviewMouseDown);
            radDrag999.PreviewMouseDown += new MouseButtonEventHandler(radDrag999_PreviewMouseDown);
            txbDrag999.PreviewMouseDown += new MouseButtonEventHandler(txbDrag999_PreviewMouseDown);

            cnvPaint.Drop += new DragEventHandler(cnvPaint_Drop);
            cnvPaint.DragOver += new DragEventHandler(cnvPaint_DragOver);
            cnvPaint.PreviewMouseUp += new MouseButtonEventHandler(ctlScriptDesigner_PreviewMouseUp);
            cnvPaint.PreviewMouseDown += new MouseButtonEventHandler(ctlScriptDesigner_PreviewMouseDown);
            cnvPaint.PreviewMouseMove += new MouseEventHandler(ctlScriptDesigner_MouseMove);
            cnvPaint.MouseMove += new MouseEventHandler(cnvPaint_MouseMove);
            currentControl.SizeChanged += new SizeChangedEventHandler(currentControl_SizeChanged);

            cmbLeadFormat.SelectionChanged += new SelectionChangedEventHandler(cmbLeadFormat_SelectionChanged);

            btnBack.Click += new RoutedEventHandler(btnBack_Click);

            r1.Stroke = Brushes.White;

            r1.StrokeThickness = 3.0;
            r1.Fill = Brushes.Transparent;
            r1.StrokeDashArray = new DoubleCollection(2);
            MyPropGrid.Height = 550;
            MyPropGrid.Width = 200;
            MyPropGrid.SetValue(Canvas.LeftProperty, 600.0);
            myExpander1.Content = MyPropGrid;
            myExpander1.Expanded += new RoutedEventHandler(myExpander1_Expanded);
            myExpander1.Collapsed += new RoutedEventHandler(myExpander1_Collapsed);
            //cnvPaint.Children.Add(MyPropGrid);
            cnvPaint.Children.Add(r1);

            r1.SizeChanged += new SizeChangedEventHandler(r1_SizeChanged);

            this.PreviewKeyDown += new KeyEventHandler(ctlScriptDesigner_PreviewKeyDown);
            lblScript.Visibility = Visibility.Collapsed;
            cmbScript.Visibility = Visibility.Collapsed;
            btnGetQuestions.Visibility = Visibility.Collapsed;

            // Adding Combobox and a Button to Next Page Bind

            cmbQuest.Height = 25;
            cmbQuest.Width = 150;

            cmbQuest.SetValue(Canvas.LeftProperty, 390.0);
            cmbQuest.SetValue(Canvas.TopProperty, 2.0);

            cnvHead.Children.Add(cmbQuest);



            btnBindQuestion.Height = 25;
            btnBindQuestion.Width = 50;
            btnBindQuestion.Content = "Bind";
            btnBindQuestion.SetValue(Canvas.LeftProperty, 550.0);
            btnBindQuestion.SetValue(Canvas.TopProperty, 2.0);

            btnBindQuestion.Click += new RoutedEventHandler(btnBindQuestion_Click);

            cnvHead.Children.Add(btnBindQuestion);


            /////////////////////////////////////////////////

            btnAPI.Click += new RoutedEventHandler(btnAPI_Click);
            btnDispo.Click += new RoutedEventHandler(btnDispo_Click);
            

            GetQuestions();
            FncFillCampaign();

            BasicHttpClient bhcFts = new BasicHttpClient();
            clientHttpFileTransfer = (IHTTPFileTransferService)bhcFts.OpenClient<IHTTPFileTransferService>("http://" + VMuktiInfo.BootStrapIPs[0] + ":80/VMukti/HttpFileTransferService");
            clientHttpFileTransfer.svcHTTPFileTransferServiceJoin();


            if (clsStartClass.ScriptName != null && clsStartClass.ScriptType != null)
            {
                string script = clsStartClass.ScriptName;
                string scriptType = clsStartClass.ScriptType;

                if (scriptType.Equals("Static"))
                {
                    lblDrag999.SetValue(Canvas.LeftProperty, txtDrag999.GetValue(Canvas.LeftProperty));
                    lblDrag999.SetValue(Canvas.TopProperty, txtDrag999.GetValue(Canvas.TopProperty));

                    cnvControls.Children.Remove(txtDrag999);
                    cnvControls.Children.Remove(cmbDrag999);
                    cnvControls.Children.Remove(lstDrag999);
                    cnvControls.Children.Remove(radDrag999);
                    cnvControls.Children.Remove(chkDrag999);
                    grdControls.Height = grdControls.Height - 160;
                    //cmbDrag999.Visibility = Visibility.Collapsed;
                    //lstDrag999.Visibility = Visibility.Collapsed;
                    //radDrag999.Visibility = Visibility.Collapsed;
                    //chkDrag999.Visibility = Visibility.Collapsed;
                }
                else
                {
                    txtDrag999.Visibility = Visibility.Visible;
                    cmbDrag999.Visibility = Visibility.Visible;
                    lstDrag999.Visibility = Visibility.Visible;
                    radDrag999.Visibility = Visibility.Visible;
                    chkDrag999.Visibility = Visibility.Visible;

                }
            }
        }

        void ctlScriptDesigner_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                this.Width = ((Grid)this.Parent).ActualWidth;
                ((Grid)this.Parent).SizeChanged += new SizeChangedEventHandler(ctlScriptDesigner_SizeChanged);
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "ctlScriptDesigner_Loaded", "ctlscriptDesigner.xaml.cs");
            }
        }

        void ctlScriptDesigner_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            this.Width = e.NewSize.Width;
        }

       

        void btnDispo_Click(object sender, RoutedEventArgs e)
        {
            if ((((ComboBoxItem)cmbCampaign.SelectedItem) != null) && (((ComboBoxItem)cmbDispositions.SelectedItem) != null))
            {
                ComboBoxItem cbicmbCampaign = new ComboBoxItem();
                ComboBoxItem cbicmbDispositions = new ComboBoxItem();

                foreach (object o in cnvPaint.Children)
                {
                    if (o.GetType() == typeof(ctlPOD))
                    {
                        if (((ctlPOD)o).rect.Visibility == Visibility.Visible)
                        {
                            for (int i = 0; i < ((Canvas)((ctlPOD)o).cnvPOD).Children.Count; i++)
                            {
                                if ((((ctlPOD)o).cnvPOD.Children[i]).GetType() == typeof(Button))
                                {
                                    ((Button)((ctlPOD)o).cnvPOD.Children[i]).Tag = "DISPOSITION-" + ((ComboBoxItem)cmbDispositions.SelectedItem).Tag.ToString();
                                    cbicmbCampaign = (ComboBoxItem)cmbCampaign.SelectedItem;
                                    cbicmbDispositions = (ComboBoxItem)cmbDispositions.SelectedItem;
                                    ((Button)((ctlPOD)o).cnvPOD.Children[i]).ToolTip = "Binded with Campaign-\"" + cbicmbCampaign.Content + "\" #Disposition-\"" + cbicmbDispositions.Content + "\"";
                                }
                                else if ((((ctlPOD)o).cnvPOD.Children[i]).GetType() == typeof(TextBox))
                                {
                                    ((TextBox)((ctlPOD)o).cnvPOD.Children[i]).Tag = "DISPOSITION-" + ((ComboBoxItem)cmbDispositions.SelectedItem).Tag.ToString();
                                    cbicmbCampaign = (ComboBoxItem)cmbCampaign.SelectedItem;
                                    cbicmbDispositions = (ComboBoxItem)cmbDispositions.SelectedItem;
                                    ((TextBox)((ctlPOD)o).cnvPOD.Children[i]).ToolTip = "Binded with Campaign-\"" + cbicmbCampaign.Content + "\" #Disposition-\"" + cbicmbDispositions.Content + "\"";
                                }
                                else if ((((ctlPOD)o).cnvPOD.Children[i]).GetType() == typeof(Label))
                                {
                                    ((Label)((ctlPOD)o).cnvPOD.Children[i]).Tag = "DISPOSITION-" + ((ComboBoxItem)cmbDispositions.SelectedItem).Tag.ToString();
                                    cbicmbCampaign = (ComboBoxItem)cmbCampaign.SelectedItem;
                                    cbicmbDispositions = (ComboBoxItem)cmbDispositions.SelectedItem;
                                    ((Label)((ctlPOD)o).cnvPOD.Children[i]).ToolTip = "Binded with Campaign-\"" + cbicmbCampaign.Content + "\" #Disposition-\"" + cbicmbDispositions.Content + "\"";
                                }
                                else if ((((ctlPOD)o).cnvPOD.Children[i]).GetType() == typeof(TextBlock))
                                {
                                    ((TextBlock)((ctlPOD)o).cnvPOD.Children[i]).Tag = "DISPOSITION-" + ((ComboBoxItem)cmbDispositions.SelectedItem).Tag.ToString();
                                    cbicmbCampaign = (ComboBoxItem)cmbCampaign.SelectedItem;
                                    cbicmbDispositions = (ComboBoxItem)cmbDispositions.SelectedItem;
                                    ((TextBlock)((ctlPOD)o).cnvPOD.Children[i]).ToolTip = "Binded with Campaign-\"" + cbicmbCampaign.Content + "\" #Disposition-\"" + cbicmbDispositions.Content + "\"";
                                }
                            }
                        }
                    }
                }
            }
        }


        void FncFillCampaign()
        {
            cmbCampaign.Items.Clear();

            ClsCampaignCollection objCamp = ClsCampaignCollection.GetAll();

            for (int i = 0; i < objCamp.Count; i++)
            {
                ComboBoxItem cbi = new ComboBoxItem();
                cbi.Content = objCamp[i].Name;
                cbi.Tag = objCamp[i].DispositionListID.ToString();
                cbi.Selected += new RoutedEventHandler(cbi_Selected);
                cmbCampaign.Items.Add(cbi);
            }
            
        }

        void cbi_Selected(object sender, RoutedEventArgs e)
        {
            cmbDispositions.Items.Clear();

            Int64 varID = Int64.Parse(((ComboBoxItem)sender).Tag.ToString());

            ClsDispositionCollection objDisp = ClsDispositionCollection.GetAll(varID);

            for (int i = 0; i < objDisp.Count; i++)
            {
                ComboBoxItem cbi = new ComboBoxItem();
                cbi.Content = objDisp[i].DespositionName;
                cbi.Tag = objDisp[i].ID;
                //cbi.Selected += new RoutedEventHandler(cbi_Selected);
                cmbDispositions.Items.Add(cbi);
            }

        }

        void btnAPI_Click(object sender, RoutedEventArgs e)
        {
            ComboBoxItem cbiAPIItem = new ComboBoxItem();

            if (((ComboBoxItem)cmbAPI.SelectedItem) != null)
            {
                foreach (object o in cnvPaint.Children)
                {
                    if (o.GetType() == typeof(ctlPOD))
                    {
                        if (((ctlPOD)o).rect.Visibility == Visibility.Visible)
                        {
                            for (int i = 0; i < ((Canvas)((ctlPOD)o).cnvPOD).Children.Count; i++)
                            {
                                if ((((ctlPOD)o).cnvPOD.Children[i]).GetType() == typeof(Button))
                                {
                                    ((Button)((ctlPOD)o).cnvPOD.Children[i]).Tag = "VMUKTIAPI-" + ((ComboBoxItem)cmbAPI.SelectedItem).Content.ToString().ToUpper();
                                    cbiAPIItem = (ComboBoxItem)cmbAPI.SelectedItem;
                                    ((Button)((ctlPOD)o).cnvPOD.Children[i]).ToolTip = "Binded with API-\"" + cbiAPIItem.Content + "\"";
                                }
                                else if ((((ctlPOD)o).cnvPOD.Children[i]).GetType() == typeof(TextBox))
                                {
                                    ((TextBox)((ctlPOD)o).cnvPOD.Children[i]).Tag = "VMUKTIAPI-" + ((ComboBoxItem)cmbAPI.SelectedItem).Content.ToString().ToUpper();
                                    cbiAPIItem = (ComboBoxItem)cmbAPI.SelectedItem;
                                    ((TextBox)((ctlPOD)o).cnvPOD.Children[i]).ToolTip = "Binded with API-\"" + cbiAPIItem.Content + "\"";
                                }
                                else if ((((ctlPOD)o).cnvPOD.Children[i]).GetType() == typeof(Label))
                                {
                                    ((Label)((ctlPOD)o).cnvPOD.Children[i]).Tag = "VMUKTIAPI-" + ((ComboBoxItem)cmbAPI.SelectedItem).Content.ToString().ToUpper();
                                    cbiAPIItem = (ComboBoxItem)cmbAPI.SelectedItem;
                                    ((Label)((ctlPOD)o).cnvPOD.Children[i]).ToolTip = "Binded with API-\"" + cbiAPIItem.Content + "\"";
                                }
                                else if ((((ctlPOD)o).cnvPOD.Children[i]).GetType() == typeof(TextBlock))
                                {
                                    ((TextBlock)((ctlPOD)o).cnvPOD.Children[i]).Tag = "VMUKTIAPI-" + ((ComboBoxItem)cmbAPI.SelectedItem).Content.ToString().ToUpper();
                                    cbiAPIItem = (ComboBoxItem)cmbAPI.SelectedItem;
                                    ((TextBlock)((ctlPOD)o).cnvPOD.Children[i]).ToolTip = "Binded with API-\"" + cbiAPIItem.Content + "\"";
                                }
                            }
                        }
                    }
                }
            }
        }

        void btnBindQuestion_Click(object sender, RoutedEventArgs e)
        {
            foreach (object o in cnvPaint.Children)
            {
                if (o.GetType() == typeof(ctlPOD))
                {
                    if (((ctlPOD)o).rect.Visibility == Visibility.Visible)
                    {
                        for (int i = 0; i < ((Canvas)((ctlPOD)o).cnvPOD).Children.Count; i++)
                        {
                            if ((((ctlPOD)o).cnvPOD.Children[i]).GetType() == typeof(Button))
                            {
                                Int64 FieldID = Int64.Parse(((ComboBoxItem)cmbQuest.SelectedItem).Tag.ToString());
                                ((Button)((ctlPOD)o).cnvPOD.Children[i]).Tag = "QuestionBind-" + FieldID.ToString();
                                ComboBoxItem cbiNextQue = new ComboBoxItem();
                                cbiNextQue = (ComboBoxItem)cmbQuest.SelectedItem;
                                ((Button)((ctlPOD)o).cnvPOD.Children[i]).ToolTip = "Next Question -" + cbiNextQue.Content;
                            }
                            //else
                            //{
                            //    MessageBox.Show("You Can Bind Next Question To Button Only", "-> Next Question", MessageBoxButton.OK, MessageBoxImage.Information);
                            //}
                        }
                    }
                }
            }
        }

        void btnBack_Click(object sender, RoutedEventArgs e)
        {
            ((Canvas)this.Parent).Visibility = Visibility.Collapsed;
      
            
        }

        void txbDrag999_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (((Canvas)((TextBlock)sender).Parent).Name == "cnvControls")
            {
                newDrag = 0;
            }
            if (currentControl != null)
            {
                PrePoint.X = e.GetPosition(cnvPaint).X - double.Parse(currentControl.GetValue(Canvas.LeftProperty).ToString());
                PrePoint.Y = e.GetPosition(cnvPaint).Y - double.Parse(currentControl.GetValue(Canvas.TopProperty).ToString());
                System.Windows.DragDrop.DoDragDrop((DependencyObject)((TextBlock)sender), ((TextBlock)sender), DragDropEffects.Move);
            }
        }

        void cmbLeadFormat_SelectionChanged(object sender, SelectionChangedEventArgs e)
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
            cmbFileds.SelectedIndex = 0;

        }

        void cmbScript_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        void FncFillCombo()
        {
            cmbScript.Items.Clear();
            ClsScriptCollection objScriptCollection = ClsScriptCollection.GetAll();

            for (int i = 0; i < objScriptCollection.Count; i++)
            {
                ComboBoxItem cbi = new ComboBoxItem();
                cbi.Content = objScriptCollection[i].ScriptName;
                cbi.Tag = objScriptCollection[i].ID;
                cmbScript.Items.Add(cbi);
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

        void radDrag999_PreviewMouseDown(object sender, MouseButtonEventArgs e)
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

        void chkDrag999_PreviewMouseDown(object sender, MouseButtonEventArgs e)
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

        void lstDrag999_PreviewMouseDown(object sender, MouseButtonEventArgs e)
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

        void cmbDrag999_PreviewMouseDown(object sender, MouseButtonEventArgs e)
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

        void myExpander1_Collapsed(object sender, RoutedEventArgs e)
        {
            if (MyPropGrid.EventTrack == 0)
            {
                myExpander1.Width = 23;
                myExpander1.Height = 23;
                myExpander1.SetValue(Canvas.LeftProperty, 635.0);
            }
            MyPropGrid.EventTrack = 0;
        }

        void myExpander1_Expanded(object sender, RoutedEventArgs e)
        {
            myExpander1.Width = 223;
            myExpander1.Height = 550;
            myExpander1.SetValue(Canvas.LeftProperty, 635.0);
            MyPropGrid.EventTrack = 0;
        }

        void ctlScriptDesigner_PreviewKeyDown(object sender, KeyEventArgs e)
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
                    catch (Exception exp)
                    {
                        VMuktiAPI.VMuktiHelper.ExceptionHandler(exp, "ctlScriptDesigner_PreviewKeyDown", "ctlScriptDesigner.xaml.cs");
                    }
                }
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
            if (r1.Parent != null)
            {
                ((Canvas)r1.Parent).Children.Remove((UIElement)r1);
            }
            cnvPaint.Children.Add(r1);
        }

        void cnvPaint_MouseMove(object sender, MouseEventArgs e)
        {
            if (r1.Height != currentControl.Height && r1.Width != currentControl.Width)
            {
                r1.Visibility = Visibility.Collapsed;
            }
        }

        void currentControl_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            MyPropGrid.ControlToBind = currentControl;
        }

        void ctlScriptDesigner_MouseMove(object sender, MouseEventArgs e)
        {
            #region Clear Resize Option For Each Object

            if (e.LeftButton == MouseButtonState.Released)
            {
                foreach (object o in cnvPaint.Children)
                {
                    if (o.GetType() == typeof(ctlPOD))
                    {
                        ((ctlPOD)o).Current = ScriptDesigner.Presentation.ResizeOption.None;
                    }
                }
            }

            #endregion

            try
            {
                #region Main Implementation

                if (currentControl.Current == ScriptDesigner.Presentation.ResizeOption.DownEdge)
                {
                    FncSetProperties();
                    r1.Height = r1.Height - (currentControl.PREVXY.Y - e.GetPosition(cnvPaint).Y);
                }

                else if (currentControl.Current == ScriptDesigner.Presentation.ResizeOption.UpEdge)
                {
                    FncSetProperties();
                    r1.Height = r1.Height + (currentControl.PREVXY.Y - e.GetPosition(cnvPaint).Y);
                    r1.SetValue(Canvas.TopProperty, float.Parse(r1.GetValue(Canvas.TopProperty).ToString()) + e.GetPosition(cnvPaint).Y - currentControl.PREVXY.Y);
                }

                else if (currentControl.Current == ScriptDesigner.Presentation.ResizeOption.LeftEdge)
                {
                    FncSetProperties();
                    r1.Width = r1.Width - (e.GetPosition(cnvPaint).X - currentControl.PREVXY.X);
                    r1.SetValue(Canvas.LeftProperty, float.Parse(r1.GetValue(Canvas.LeftProperty).ToString()) + (e.GetPosition(cnvPaint).X - currentControl.PREVXY.X));
                }

                else if (currentControl.Current == ScriptDesigner.Presentation.ResizeOption.RightEdge)
                {
                    FncSetProperties();
                    r1.Width = r1.Width + e.GetPosition(cnvPaint).X - currentControl.PREVXY.X;
                }

                #endregion
            }
            catch (Exception exp)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(exp, "ctlScriptDesigner_MouseMove", "ctlScriptDesigner.xaml.cs");
            }
        }

        void FncSetProperties()
        {
            r1.Height = currentControl.Height;
            r1.Width = currentControl.Width;
            r1.Visibility = Visibility.Visible;
            r1.SetValue(Canvas.LeftProperty, currentControl.GetValue(Canvas.LeftProperty));
            r1.SetValue(Canvas.TopProperty, currentControl.GetValue(Canvas.TopProperty));
        }

        void ctlScriptDesigner_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {

            foreach (object o in cnvPaint.Children)
            {
                if (o.GetType() == typeof(ctlPOD))
                {
                    ((ctlPOD)o).IsRectvisible = false;
                }
            }


        }

        void ctlScriptDesigner_PreviewMouseUp(object sender, MouseButtonEventArgs e)
        {
            if (PrePoint.X == e.GetPosition(this).X || PrePoint.Y == e.GetPosition(this).Y)
            { }
            else
            {
                #region Main Implementation

                if (currentControl.Cursor != Cursors.Arrow)
                {

                    if (currentControl.Current == ScriptDesigner.Presentation.ResizeOption.DownEdge)
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

                    else if (currentControl.Current == ScriptDesigner.Presentation.ResizeOption.UpEdge)
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

                    else if (currentControl.Current == ScriptDesigner.Presentation.ResizeOption.LeftEdge)
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

                    else if (currentControl.Current == ScriptDesigner.Presentation.ResizeOption.RightEdge)
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
                    currentControl.Current = ScriptDesigner.Presentation.ResizeOption.None;

                    MyPropGrid.ControlToBind = currentControl;
                }
                #endregion

                r1.Visibility = Visibility.Hidden;
            }
        }

        void cnvPaint_DragOver(object sender, DragEventArgs e)
        {
            if (newDrag == 1)
            {
                r1.Visibility = Visibility.Visible;
                r1.Height = currentControl.Height;
                r1.Width = currentControl.Width;
                if (e.GetPosition(cnvPaint).X - PrePoint.X > 0)
                {
                    r1.SetValue(Canvas.LeftProperty, e.GetPosition(cnvPaint).X - PrePoint.X);
                }
                if (e.GetPosition(cnvPaint).Y - PrePoint.Y > 0)
                {
                r1.SetValue(Canvas.TopProperty, e.GetPosition(cnvPaint).Y - PrePoint.Y);
                }
            }
        }

        void lblDrag_PreviewMouseDown(object sender, MouseButtonEventArgs e)
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

        void txtDrag_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
           
            //objPOD_PreviewMouseDown(((TextBox)sender).Parent, null);

            //MessageBox.Show(((Canvas)((TextBox)sender).Parent).Name);

            if (((Canvas)((TextBox)sender).Parent).Name == "cnvControls")
            {
                //objPOD_PreviewMouseDown(((TextBox)sender).Parent, null);
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

        void btnDrag_PreviewMouseDown(object sender, MouseButtonEventArgs e)
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

        void cnvPaint_Drop(object sender, DragEventArgs e)
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
                    btn.Background = Brushes.Transparent;
                    btn.PreviewMouseDown += new MouseButtonEventHandler(btnDrag_PreviewMouseDown);
                    btn.SetValue(Canvas.LeftProperty, 10.0);
                    btn.SetValue(Canvas.TopProperty, 10.0);

                    ctlPOD objPOD = new ctlPOD();
                    objPOD.AllowDrop = true;
                    objPOD.Height = 25;
                    objPOD.Width = 100;
                    
                    objPOD.SetValue(Canvas.LeftProperty, p.X);
                    objPOD.SetValue(Canvas.TopProperty, p.Y);
                    MyPropGrid.ControlToBind = objPOD;
                    objPOD.PreviewMouseDown += new MouseButtonEventHandler(objPOD_PreviewMouseDown);

                    objPOD.cnvPOD.Children.Add(btn);
                    currentControl = objPOD;
                    cnvPaint.Children.Add(objPOD);
                }
                else if ((((Canvas)((Button)e.Data.GetData(typeof(Button))).Parent).Parent).GetType() == typeof(ctlPOD))
                {
                    //v
                   // if (currentControl.rect.Visibility == Visibility.Visible)
                    {
                        Point p = e.GetPosition((IInputElement)cnvPaint);
                        ((Canvas)((Button)e.Data.GetData(typeof(Button))).Parent).Parent.SetValue(Canvas.LeftProperty, p.X - PrePoint.X);
                        ((Canvas)((Button)e.Data.GetData(typeof(Button))).Parent).Parent.SetValue(Canvas.TopProperty, p.Y - PrePoint.Y);
                    }
                }
            }

            else if (e.Data.GetData(typeof(Label)) != null)
            {
                if (((Canvas)((Label)e.Data.GetData(typeof(Label))).Parent).Name.ToString() == "cnvControls")
                {
                    Point p = e.GetPosition((IInputElement)cnvPaint);
                    Label lbl = new Label();
                    lbl.Content = "Label";
                    lbl.Background = Brushes.Transparent;
                    lbl.Foreground = Brushes.Black ;
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
                    txt.Cursor = Cursors.Arrow;
                    txt.Background = Brushes.Transparent;
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

            else if (e.Data.GetData(typeof(TextBlock)) != null)
            {
                if (((Canvas)((TextBlock)e.Data.GetData(typeof(TextBlock))).Parent).Name.ToString() == "cnvControls")
                {
                    Point p = e.GetPosition((IInputElement)cnvPaint);
                    TextBlock txb = new TextBlock();

                    txb.Cursor = Cursors.Arrow;
                    txb.Height = 25;
                    txb.Width = 100;
                    txb.Background = Brushes.Transparent;
                    txb.TextWrapping = TextWrapping.Wrap;
                    txb.Text = "TextBlock";
                    txb.PreviewMouseDown += new MouseButtonEventHandler(txbDrag999_PreviewMouseDown);
                    txb.SetValue(Canvas.LeftProperty, 10.0);
                    txb.SetValue(Canvas.TopProperty, 10.0);
                    txb.Foreground = Brushes.Black;

                    ctlPOD objPOD = new ctlPOD();
                    objPOD.AllowDrop = true;
                    objPOD.Height = 25;
                    objPOD.Width = 100;
                    objPOD.PreviewMouseDown += new MouseButtonEventHandler(objPOD_PreviewMouseDown);
                    objPOD.SetValue(Canvas.LeftProperty, p.X);
                    objPOD.SetValue(Canvas.TopProperty, p.Y);
                    MyPropGrid.ControlToBind = objPOD;
                    objPOD.cnvPOD.Children.Add(txb);
                    currentControl = objPOD;
                    cnvPaint.Children.Add(objPOD);
                }
                else if ((((Canvas)((TextBlock)e.Data.GetData(typeof(TextBlock))).Parent).Parent).GetType() == typeof(ctlPOD))
                {
                    Point p = e.GetPosition((IInputElement)cnvPaint);
                    ((Canvas)((TextBlock)e.Data.GetData(typeof(TextBlock))).Parent).Parent.SetValue(Canvas.LeftProperty, p.X - PrePoint.X);
                    ((Canvas)((TextBlock)e.Data.GetData(typeof(TextBlock))).Parent).Parent.SetValue(Canvas.TopProperty, p.Y - PrePoint.Y);
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
                    cmb.Background = Brushes.Transparent;
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
                    lst.Background = Brushes.Transparent;
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
                    chk.Background = Brushes.Transparent;
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
                    rad.Background = Brushes.Transparent;
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

        void txt_MouseDown(object sender, MouseButtonEventArgs e)
        {
            objPOD_PreviewMouseDown(((TextBox)sender).Parent, null);
        }

        void rad_PreviewMouseDown(object sender, MouseButtonEventArgs e)
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

        void chk_PreviewMouseDown(object sender, MouseButtonEventArgs e)
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

        void lst_PreviewMouseDown(object sender, MouseButtonEventArgs e)
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

        void cmb_PreviewMouseDown(object sender, MouseButtonEventArgs e)
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

        void objPOD_KeyDown(object sender, KeyEventArgs e)
        {
            if (((ctlPOD)sender).IsRectvisible)
            {
                ((ctlPOD)sender).Visibility = Visibility.Collapsed;
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
            catch (Exception exp)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(exp, "objPOD_PreviewMouseDown", "ctlScriptDesigner.xaml.cs");
            }
        }

        private void btnSubmit_Click(object sender, RoutedEventArgs e)
        {
            if ((((ComboBoxItem)cmbLeadFormat.SelectedItem) != null) && (((ComboBoxItem)cmbFileds.SelectedItem) != null))
            {
                ComboBoxItem cbiCmbField = new ComboBoxItem();
                ComboBoxItem cbiLeadFormat = new ComboBoxItem();

                foreach (object o in cnvPaint.Children)
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
                                    cbiCmbField = (ComboBoxItem)cmbFileds.SelectedItem;
                                    cbiLeadFormat = (ComboBoxItem)cmbLeadFormat.SelectedItem;
                                    ((Button)((ctlPOD)o).cnvPOD.Children[i]).ToolTip = "Binded with LeadFormat-\"" + cbiLeadFormat.Content + "\" #Field-\"" + cbiCmbField.Content + "\"";
                                }
                                else if ((((ctlPOD)o).cnvPOD.Children[i]).GetType() == typeof(TextBox))
                                {
                                    Int64 FieldID = Int64.Parse(((ComboBoxItem)cmbFileds.SelectedItem).Tag.ToString());
                                    ((TextBox)((ctlPOD)o).cnvPOD.Children[i]).Tag = "Bind-" + FieldID.ToString();
                                    cbiCmbField = (ComboBoxItem)cmbFileds.SelectedItem;
                                    cbiLeadFormat = (ComboBoxItem)cmbLeadFormat.SelectedItem;
                                    ((TextBox)((ctlPOD)o).cnvPOD.Children[i]).ToolTip = "Binded with LeadFormat-\"" + cbiLeadFormat.Content + "\" #Field-\"" + cbiCmbField.Content + "\"";
                                }
                                else if ((((ctlPOD)o).cnvPOD.Children[i]).GetType() == typeof(Label))
                                {
                                    Int64 FieldID = Int64.Parse(((ComboBoxItem)cmbFileds.SelectedItem).Tag.ToString());
                                    ((Label)((ctlPOD)o).cnvPOD.Children[i]).Tag = "Bind-" + FieldID.ToString();
                                    cbiCmbField = (ComboBoxItem)cmbFileds.SelectedItem;
                                    cbiLeadFormat = (ComboBoxItem)cmbLeadFormat.SelectedItem;
                                    ((Label)((ctlPOD)o).cnvPOD.Children[i]).ToolTip = "Binded with LeadFormat-\"" + cbiLeadFormat.Content + "\" #Field-\"" + cbiCmbField.Content + "\"";
                                }
                                else if ((((ctlPOD)o).cnvPOD.Children[i]).GetType() == typeof(TextBlock))
                                {
                                    Int64 FieldID = Int64.Parse(((ComboBoxItem)cmbFileds.SelectedItem).Tag.ToString());
                                    ((TextBlock)((ctlPOD)o).cnvPOD.Children[i]).Tag = "Bind-" + FieldID.ToString();
                                    cbiCmbField = (ComboBoxItem)cmbFileds.SelectedItem;
                                    cbiLeadFormat = (ComboBoxItem)cmbLeadFormat.SelectedItem;
                                    ((TextBlock)((ctlPOD)o).cnvPOD.Children[i]).ToolTip = "Binded with LeadFormat-\"" + cbiLeadFormat.Content + "\" #Field-\"" + cbiCmbField.Content + "\"";
                                }
                                //else if ((((ctlPOD)o).cnvPOD.Children[i]).GetType() == typeof(ComboBox))
                                //{
                                //    Int64 FieldID = Int64.Parse(((ComboBoxItem)cmbFileds.SelectedItem).Tag.ToString());
                                //    ((ComboBox)((ctlPOD)o).cnvPOD.Children[i]).Tag = "Bind-" + FieldID.ToString();
                                //    cbiCmbField = (ComboBoxItem)cmbFileds.SelectedItem;
                                //    cbiLeadFormat = (ComboBoxItem)cmbLeadFormat.SelectedItem;
                                //    ((ComboBox)((ctlPOD)o).cnvPOD.Children[i]).ToolTip = "Binded with LeadFormat-\"" + cbiLeadFormat.Content + "\" #Field-\"" + cbiCmbField.Content + "\"";
                                //}
                                //else if ((((ctlPOD)o).cnvPOD.Children[i]).GetType() == typeof(ListBox))
                                //{
                                //    Int64 FieldID = Int64.Parse(((ComboBoxItem)cmbFileds.SelectedItem).Tag.ToString());
                                //    ((ListBox)((ctlPOD)o).cnvPOD.Children[i]).Tag = "Bind-" + FieldID.ToString();
                                //    cbiCmbField = (ComboBoxItem)cmbFileds.SelectedItem;
                                //    cbiLeadFormat = (ComboBoxItem)cmbLeadFormat.SelectedItem;
                                //    ((ListBox)((ctlPOD)o).cnvPOD.Children[i]).ToolTip = "Binded with LeadFormat-\"" + cbiLeadFormat.Content + "\" #Field-\"" + cbiCmbField.Content + "\"";
                                //}
                                //else if ((((ctlPOD)o).cnvPOD.Children[i]).GetType() == typeof(RadioButton))
                                //{
                                //    Int64 FieldID = Int64.Parse(((ComboBoxItem)cmbFileds.SelectedItem).Tag.ToString());
                                //    ((RadioButton)((ctlPOD)o).cnvPOD.Children[i]).Tag = "Bind-" + FieldID.ToString();
                                //    cbiCmbField = (ComboBoxItem)cmbFileds.SelectedItem;
                                //    cbiLeadFormat = (ComboBoxItem)cmbLeadFormat.SelectedItem;
                                //    ((RadioButton)((ctlPOD)o).cnvPOD.Children[i]).ToolTip = "Binded with LeadFormat-\"" + cbiLeadFormat.Content + "\" #Field-\"" + cbiCmbField.Content + "\"";
                                //}
                                //else if ((((ctlPOD)o).cnvPOD.Children[i]).GetType() == typeof(CheckBox))
                                //{
                                //    Int64 FieldID = Int64.Parse(((ComboBoxItem)cmbFileds.SelectedItem).Tag.ToString());
                                //    ((CheckBox)((ctlPOD)o).cnvPOD.Children[i]).Tag = "Bind-" + FieldID.ToString();
                                //    cbiCmbField = (ComboBoxItem)cmbFileds.SelectedItem;
                                //    cbiLeadFormat = (ComboBoxItem)cmbLeadFormat.SelectedItem;
                                //    ((CheckBox)((ctlPOD)o).cnvPOD.Children[i]).ToolTip = "Binded with LeadFormat-\"" + cbiLeadFormat.Content + "\" #Field-\"" + cbiCmbField.Content + "\"";
                                //}

                            }
                        }
                    }
                }
            }
        }

        public void SetQuestionsCombo(ClsQuestionCollectionR o)
        {
            for (int i = 0; i < o.Count; i++)
            {
                ComboBoxItem cbi = new ComboBoxItem();
                cbi.Content = o[i].QuestionName.ToString();
                cbi.Tag = o[i].ID.ToString();
                cmbQuest.Items.Add(cbi);
            }
        }

        int load = 0;
        public void GetQuestions()
        {
            btnNext.Content = "Next";

            if (load == 0)
            {
                try
                {
                    FncLoadScript();
                }
                catch (Exception e)
                {
                    VMuktiAPI.VMuktiHelper.ExceptionHandler(e, "GetQuestions", "ctlScriptdesigner.xaml.cs");
                }
                load++;
            }
            CurrentQueCount = 0;
            startQuestion = "";
            startQuestionID = 0;

            objQueCollection = ClsQuestionCollectionR.GetAll(_ScriptID);

            SetQuestionsCombo(objQueCollection);
            try
            {
            if (objQueCollection.Count > 0)
            {
                    //FncSetInnerCanvas();
                    FncSetCanvas();
                if (btnNext.Parent == null)
                {
                    if (objQueCollection.Count > 1)
                    {
                        btnNext.Content = "Next";
                    }
                    else if (objQueCollection.Count == 1)
                    {
                        btnNext.Content = "Finished";
                        btnPrev.IsEnabled = false;
                    }

                    btnNext.Height = 25;
                    btnNext.Width = 50;
                    btnNext.Click += new RoutedEventHandler(btnYes_Click);
                    Canvas.SetLeft(btnNext, 130.0);
                    Canvas.SetTop(btnNext, 2.0);
                    cnvHead.Children.Add(btnNext);

                    btnPrev.Content = "Previous";
                    btnPrev.Height = 25;
                    btnPrev.Width = 70;
                    btnPrev.Click += new RoutedEventHandler(btnPrev_Click);
                    Canvas.SetLeft(btnPrev, 200.0);
                    Canvas.SetTop(btnPrev, 2.0);
                    cnvHead.Children.Add(btnPrev);
                    btnPrev.IsEnabled = false;
                }

                if (btnStartQue.Parent == null)
                {
                    btnStartQue.Content = "StartQuestion";
                    btnStartQue.Height = 25;
                    btnStartQue.Width = 90;
                    btnStartQue.Click += new RoutedEventHandler(btnStartQue_Click);
                    Canvas.SetLeft(btnStartQue, 290.0);
                    Canvas.SetTop(btnStartQue, 2.0);
                    cnvHead.Children.Add(btnStartQue);
                }
                CurrentQueCount = 0;
            }
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "GetQuestions", "ctlScriptdesigner.xaml.cs");
            }

            try
            {
                if (Directory.Exists(Assembly.GetAssembly(this.GetType()).Location.Replace("ScriptDesigner.Presentation.dll", "SS")))
                {
                    Directory.Delete(Assembly.GetAssembly(this.GetType()).Location.Replace("ScriptDesigner.Presentation.dll", @"SS"), true);
                }
                Directory.CreateDirectory(Assembly.GetAssembly(this.GetType()).Location.Replace("ScriptDesigner.Presentation.dll", "SS"));
            }
            catch (Exception exp)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(exp, "GetQuestions", "ctlScriptdesigner.xaml.cs");
            }

            //MessageBox.Show(Assembly.GetAssembly(this.GetType()).Location.Replace("ScriptDesigner.Presentation.dll", "ScriptBase"));
            //MessageBox.Show(Assembly.GetAssembly(this.GetType()).Location.Replace("ScriptDesigner.Presentation.dll", "SS"));
            try
            {
            copyDirectory(Assembly.GetAssembly(this.GetType()).Location.Replace("ScriptDesigner.Presentation.dll", "ScriptBase"), Assembly.GetAssembly(this.GetType()).Location.Replace("ScriptDesigner.Presentation.dll", "SS"));
            if (!Directory.Exists(Assembly.GetAssembly(this.GetType()).Location.Replace("ScriptDesigner.Presentation.dll", @"SS\Script.DataAccess\ReferencedAssemblies")))
            {
                Directory.CreateDirectory(Assembly.GetAssembly(this.GetType()).Location.Replace("ScriptDesigner.Presentation.dll", @"SS\Script.DataAccess\ReferencedAssemblies"));
            }

            if (!Directory.Exists(Assembly.GetAssembly(this.GetType()).Location.Replace("ScriptDesigner.Presentation.dll", @"SS\Script.Presentation\ReferencedAssemblies")))
            {
                Directory.CreateDirectory(Assembly.GetAssembly(this.GetType()).Location.Replace("ScriptDesigner.Presentation.dll", @"SS\Script.Presentation\ReferencedAssemblies"));
                }
            }
            catch (Exception ert)
            {

                VMuktiAPI.VMuktiHelper.ExceptionHandler(ert, "GetQuestions", "ctlScriptdesigner.xaml.cs");
            }
        }

        private void btnGetQuestions_Click(object sender, RoutedEventArgs e)
        {
            
        }

        void btnPrev_Click(object sender, RoutedEventArgs e)
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

        void FncListToCanvas()
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

        void btnStartQue_Click(object sender, RoutedEventArgs e)
        {
            startQuestion = objQueCollection[CurrentQueCount].QuestionName;
            startQuestionID = objQueCollection[CurrentQueCount].ID;
            btnStartQue.ToolTip = "Start Question - " + objQueCollection[CurrentQueCount].QuestionName;
        }

        public static void copyDirectory(string Src, string Dst)
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

        public void ShowDirectory(DirectoryInfo dir)
        {
            foreach (FileInfo file in dir.GetFiles("*.dll"))
            {
                al.Add(file.FullName);
            }
            foreach (DirectoryInfo subDir in dir.GetDirectories())
            {
                ShowDirectory(subDir);
            }
        }

        public ArrayList al = new ArrayList();

        string str;
        string strModPath;
        string filename;
        string destination;
        string filePath;
        //public static IHTTPFileTransferService clientHttpFileTransfer = null;
       
       
        Type[] t1;

        public void FncLoadScript()
        {
         

            try
            {
                Assembly ass3 = Assembly.GetEntryAssembly();
                if (Directory.Exists(ass3.Location.Replace("VMukti.Presentation.exe", @"Scripts")))
                {
                    Directory.Delete(ass3.Location.Replace("VMukti.Presentation.exe", @"Scripts"), true);
                }
            }
            catch (Exception e)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(e, "FncLoadScript", "ctlScriptdesigner.xaml.cs");
            }
            try
            {
                Assembly ass1 = Assembly.GetEntryAssembly();
                if (Directory.Exists(ass1.Location.Replace("VMukti.Presentation.exe", @"ScriptModules")))
                {
                    Directory.Delete(ass1.Location.Replace("VMukti.Presentation.exe", @"ScriptModules"), true);
                }
            }
            catch (Exception e)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(e, "FncLoadScript", "ctlScriptdesigner.xaml.cs");
            }

            #region Loading ReferencedAssemblies
            try
            {
           

                if (DownloadAndExtractZipFile() == -1)
                {
                    return;
                }

                al.Clear();
              

                 DirectoryInfo dirinfomodule = new DirectoryInfo(strModPath + "\\" + str );
                ShowDirectory(dirinfomodule);
              
                

                for (int j = 0; j < al.Count; j++)
                {
                    string[] arraysplit = al[j].ToString().Split('\\');
                  //  if (arraysplit[arraysplit.Length - 1].ToString() == ScriptName + "_Script" + g + ".Presentation.dll")
                    if (arraysplit[arraysplit.Length - 1].ToString().Contains( ".Presentation.dll"))
                    {
                        
                        Assembly a;
                        Assembly assbal;
                        AssemblyName[] anbal;
                        AssemblyName[] an;

                        a = Assembly.LoadFrom(al[j].ToString());
                        an = a.GetReferencedAssemblies();
                        for (int alcount = 0; alcount < al.Count; alcount++)
                        {
                            string strsplit = al[alcount].ToString();
                            string[] strold = strsplit.Split('\\');
                            string strnew = strold[strold.Length - 1].Substring(0, strold[strold.Length - 1].Length - 4);
                      
                            for (int asscount = 0; asscount < an.Length; asscount++)
                            {
                                if (an[asscount].Name == strnew)
                                {
                                    assbal = System.AppDomain.CurrentDomain.Load(System.Reflection.Assembly.LoadFrom(al[alcount].ToString()).GetName());
                                    anbal = assbal.GetReferencedAssemblies();
                                    for (int andal = 0; andal < al.Count; andal++)
                                    {
                                        string strsplitdal = al[andal].ToString();
                                        string[] strolddal = strsplitdal.Split('\\');
                                        string strnewdal = strolddal[strolddal.Length - 1].Substring(0, strolddal[strolddal.Length - 1].Length - 4);

                                        for (int asscountdal = 0; asscountdal < anbal.Length; asscountdal++)
                                        {
                                            if (anbal[asscountdal].Name == strnewdal)
                                            {
                                                System.AppDomain.CurrentDomain.Load(System.Reflection.Assembly.LoadFrom(al[andal].ToString()).GetName());
                                            }
                                        }
                                    }
                                }
                            }
                        }

                        t1 = a.GetTypes();


                    }
                }



            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "FncLoadScript", "ctlScriptdesigner.xaml.cs");
            }
            #endregion

        }


        void FncSetCanvas()
        {
            int flag = 0;
           
            try
            {
                for (int k = 0; k < t1.Length; k++)
                {
                    
                    if (t1[k].Name == "UserControl" + objQueCollection[CurrentQueCount].ID.ToString())
                    {

                        flag = 1;
                        try
                        {
                            UserControl usrCtl = (UserControl)(Activator.CreateInstance(t1[k], null));
                            usrCtl.SetValue(Canvas.LeftProperty, 0.0);
                            MethodInfo mi = t1[k].GetMethod("GetCanvas");//(MemberTypes.Method,BindingFlags.Public
                            Object[] args = new Object[0];
                            Canvas cnvPage = (Canvas)mi.Invoke(usrCtl, args);
                            usrCtl.SetValue(Canvas.TopProperty, 0.0);

                            cnvPaint.Background = cnvPage.Background;


                            int cnt = cnvPage.Children.Count;
                            // need a for loop get all ctls of page
                            for (int i = cnt-1; i >=0; i--)
                            {

                                if (cnvPage.Children[i].GetType() == typeof(Button))
                                {
                                    Button uiCurr = (Button)cnvPage.Children[i];
                                    ctlPOD ctlCurr = new ctlPOD();

                                    ctlCurr.AllowDrop = true;
                                    ctlCurr.Height = (Double)uiCurr.GetValue(HeightProperty);
                                    ctlCurr.Width = (Double)uiCurr.GetValue(WidthProperty);

                                    ctlCurr.PreviewMouseDown += new MouseButtonEventHandler(objPOD_PreviewMouseDown);
                                    ctlCurr.SetValue(Canvas.LeftProperty, uiCurr.GetValue(Canvas.LeftProperty));
                                    ctlCurr.SetValue(Canvas.TopProperty, uiCurr.GetValue(Canvas.TopProperty));
                                    MyPropGrid.ControlToBind = ctlCurr;

                                    uiCurr.PreviewMouseDown += new MouseButtonEventHandler(btnDrag_PreviewMouseDown);
                                    cnvPage.Children.Remove(uiCurr);
                                    ctlCurr.cnvPOD.Children.Add(uiCurr);

                                    currentControl = ctlCurr;
                                    cnvPaint.Children.Add(ctlCurr);


                                    
                                }

                                else if (cnvPage.Children[i].GetType() == typeof(Label))
                                {
                                    Label uiCurr = (Label)cnvPage.Children[i];
                                    ctlPOD ctlCurr = new ctlPOD();

                                    

                                    ctlCurr.AllowDrop = true;
                                    ctlCurr.Height = (Double)uiCurr.GetValue(HeightProperty);
                                    ctlCurr.Width = (Double)uiCurr.GetValue(WidthProperty);

                                    ctlCurr.PreviewMouseDown += new MouseButtonEventHandler(objPOD_PreviewMouseDown);
                                    ctlCurr.SetValue(Canvas.LeftProperty, uiCurr.GetValue(Canvas.LeftProperty));
                                    ctlCurr.SetValue(Canvas.TopProperty, uiCurr.GetValue(Canvas.TopProperty));
                                    MyPropGrid.ControlToBind = ctlCurr;

                                    uiCurr.PreviewMouseDown += new MouseButtonEventHandler(lblDrag_PreviewMouseDown);
                                    cnvPage.Children.Remove(uiCurr);
                                    ctlCurr.cnvPOD.Children.Add(uiCurr);
                                    currentControl = ctlCurr;
                                    
                                    cnvPaint.Children.Add(ctlCurr);
                                }

                                else if (cnvPage.Children[i].GetType() == typeof(TextBox))
                                {
                                    TextBox uiCurr = (TextBox)cnvPage.Children[i];
                                    ctlPOD ctlCurr = new ctlPOD();



                                    ctlCurr.AllowDrop = true;
                                    ctlCurr.Height = (Double)uiCurr.GetValue(HeightProperty);
                                    ctlCurr.Width = (Double)uiCurr.GetValue(WidthProperty);

                                    ctlCurr.PreviewMouseDown += new MouseButtonEventHandler(objPOD_PreviewMouseDown);
                                    ctlCurr.SetValue(Canvas.LeftProperty, uiCurr.GetValue(Canvas.LeftProperty));
                                    ctlCurr.SetValue(Canvas.TopProperty, uiCurr.GetValue(Canvas.TopProperty));
                                    MyPropGrid.ControlToBind = ctlCurr;

                                    uiCurr.PreviewMouseDown += new MouseButtonEventHandler(txtDrag_PreviewMouseDown);
                                    cnvPage.Children.Remove(uiCurr);
                                    ctlCurr.cnvPOD.Children.Add(uiCurr);
                                    currentControl = ctlCurr;

                                    cnvPaint.Children.Add(ctlCurr);
                                }

                                else if (cnvPage.Children[i].GetType() == typeof(TextBlock))
                                {
                                    TextBlock uiCurr = (TextBlock)cnvPage.Children[i];
                                    ctlPOD ctlCurr = new ctlPOD();



                                    ctlCurr.AllowDrop = true;
                                    ctlCurr.Height = (Double)uiCurr.GetValue(HeightProperty);
                                    ctlCurr.Width = (Double)uiCurr.GetValue(WidthProperty);
                                    string str1 = (string)uiCurr.GetValue(ContentProperty);
                                    ctlCurr.PreviewMouseDown += new MouseButtonEventHandler(objPOD_PreviewMouseDown);
                                    ctlCurr.SetValue(Canvas.LeftProperty, uiCurr.GetValue(Canvas.LeftProperty));
                                    ctlCurr.SetValue(Canvas.TopProperty, uiCurr.GetValue(Canvas.TopProperty));
                                    MyPropGrid.ControlToBind = ctlCurr;
                                  

                                    uiCurr.PreviewMouseDown += new MouseButtonEventHandler(txbDrag999_PreviewMouseDown);//lblDrag_PreviewMouseDown);
                                    cnvPage.Children.Remove(uiCurr);
                                    ctlCurr.cnvPOD.Children.Add(uiCurr);

                                    currentControl = ctlCurr;
                                    cnvPaint.Children.Add(ctlCurr);

                                }
                                else if (cnvPage.Children[i].GetType() == typeof(ComboBox))
                                {
                                    ComboBox uiCurr = (ComboBox)cnvPage.Children[i];
                                    ctlPOD ctlCurr = new ctlPOD();

                                    ctlCurr.AllowDrop = true;
                                    ctlCurr.Height = (Double)uiCurr.GetValue(HeightProperty);
                                    ctlCurr.Width = (Double)uiCurr.GetValue(WidthProperty);
                                    string str1 = (string)uiCurr.GetValue(ContentProperty);
                                    ctlCurr.PreviewMouseDown += new MouseButtonEventHandler(objPOD_PreviewMouseDown);
                                    ctlCurr.SetValue(Canvas.LeftProperty, uiCurr.GetValue(Canvas.LeftProperty));
                                    ctlCurr.SetValue(Canvas.TopProperty, uiCurr.GetValue(Canvas.TopProperty));
                                    MyPropGrid.ControlToBind = ctlCurr;

                                    uiCurr.PreviewMouseDown += new MouseButtonEventHandler(cmbDrag999_PreviewMouseDown);
                                    cnvPage.Children.Remove(uiCurr);
                                    ctlCurr.cnvPOD.Children.Add(uiCurr);

                                    

                                    currentControl = ctlCurr;

                                    cnvPaint.Children.Add(ctlCurr);
                                }
                                else if (cnvPage.Children[i].GetType() == typeof(ListBox))
                                {
                                    ListBox uiCurr = (ListBox)cnvPage.Children[i];
                                    ctlPOD ctlCurr = new ctlPOD();

                                    ctlCurr.AllowDrop = true;
                                    ctlCurr.Height = (Double)uiCurr.GetValue(HeightProperty);
                                    ctlCurr.Width = (Double)uiCurr.GetValue(WidthProperty);
                                    string str1 = (string)uiCurr.GetValue(ContentProperty);
                                    ctlCurr.PreviewMouseDown += new MouseButtonEventHandler(objPOD_PreviewMouseDown);
                                    ctlCurr.SetValue(Canvas.LeftProperty, uiCurr.GetValue(Canvas.LeftProperty));
                                    ctlCurr.SetValue(Canvas.TopProperty, uiCurr.GetValue(Canvas.TopProperty));
                                    MyPropGrid.ControlToBind = ctlCurr;

                                    uiCurr.PreviewMouseDown += new MouseButtonEventHandler(lstDrag999_PreviewMouseDown);
                                    cnvPage.Children.Remove(uiCurr);
                                    ctlCurr.cnvPOD.Children.Add(uiCurr);

                                    

                                    currentControl = ctlCurr;
                                    cnvPaint.Children.Add(ctlCurr);
                                }
                                else if (cnvPage.Children[i].GetType() == typeof(RadioButton))
                                {
                                    RadioButton uiCurr = (RadioButton)cnvPage.Children[i];
                                    
                                    ctlPOD ctlCurr = new ctlPOD();

                                    ctlCurr.AllowDrop = true;
                                    ctlCurr.Height = (Double)uiCurr.GetValue(HeightProperty);
                                    ctlCurr.Width = (Double)uiCurr.GetValue(WidthProperty);
                                    string str1 = (string)uiCurr.GetValue(ContentProperty);
                                    ctlCurr.PreviewMouseDown += new MouseButtonEventHandler(objPOD_PreviewMouseDown);
                                    ctlCurr.SetValue(Canvas.LeftProperty, uiCurr.GetValue(Canvas.LeftProperty));
                                    ctlCurr.SetValue(Canvas.TopProperty, uiCurr.GetValue(Canvas.TopProperty));
                                    MyPropGrid.ControlToBind = ctlCurr;

                                    uiCurr.PreviewMouseDown += new MouseButtonEventHandler(radDrag999_PreviewMouseDown);
                                    cnvPage.Children.Remove(uiCurr);
                                    ctlCurr.cnvPOD.Children.Add(uiCurr);

                                    
                                    currentControl = ctlCurr;

                                    cnvPaint.Children.Add(ctlCurr);
                                }

                                else if (cnvPage.Children[i].GetType() == typeof(CheckBox))
                                {
                                    CheckBox uiCurr = (CheckBox)cnvPage.Children[i];
                                    ctlPOD ctlCurr = new ctlPOD();

                                    ctlCurr.AllowDrop = true;
                                    ctlCurr.Height = (Double)uiCurr.GetValue(HeightProperty);
                                    ctlCurr.Width = (Double)uiCurr.GetValue(WidthProperty);
                                    string str1 = (string)uiCurr.GetValue(ContentProperty);
                                    ctlCurr.PreviewMouseDown += new MouseButtonEventHandler(objPOD_PreviewMouseDown);
                                    ctlCurr.SetValue(Canvas.LeftProperty, uiCurr.GetValue(Canvas.LeftProperty));
                                    ctlCurr.SetValue(Canvas.TopProperty, uiCurr.GetValue(Canvas.TopProperty));
                                    MyPropGrid.ControlToBind = ctlCurr;
                                    uiCurr.PreviewMouseDown += new MouseButtonEventHandler(chkDrag999_PreviewMouseDown);

                                    cnvPage.Children.Remove(uiCurr);
                                    ctlCurr.cnvPOD.Children.Add(uiCurr);

                                    

                                    currentControl = ctlCurr;
                                    cnvPaint.Children.Add(ctlCurr);
                                }

                            }
                        }
                        catch (Exception exp)
                        {
                            VMuktiAPI.VMuktiHelper.ExceptionHandler(exp, "FncsetCanvas", "ctlScriptdesigner.xaml.cs");
                        }
                    }

                }
            }
            catch(Exception er)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(er, "FncSetCanvas", "ctlScriptdesigner.xaml.cs");
            }
           
            //int cntCount = 0;
            if (flag == 0)
            {
                //for (int k = 0; k < t1.Length; k++)
                //{
                //    if (t1[k].Name.Contains("UserControl"))
                //    {
                //        cntCount++;
                //    }
                //}
                //if (cntCount <= objQueCollection.Count)
                FncSetInnerCanvas();
                //int flag2=0;
                //for (int m = 0; m < objQueCollection.Count; m++)
                //{
                //    for (int k = 0; k < t1.Length; k++)
                //    {
                //        if ("UserControl" + objQueCollection[m].ID.ToString() == t1[k].Name )
                //        {
                //            break;
                //        }
                //        else
                //        {
                //            flag2 = 1;
                //        }
                //    }
                //}

            }
            else
            {
                //do nothing
            }

        }

        
        void FncSetInnerCanvas()
        {
            //cnvPaint.Height = 280;
            //cnvPaint.Width = 700;

            TextBlock lblHeader = new TextBlock();
            lblHeader.Background = Brushes.Transparent;
            lblHeader.TextWrapping = TextWrapping.Wrap;
            lblHeader.Text = objQueCollection[CurrentQueCount].QuestionName;
            lblHeader.PreviewMouseDown += new MouseButtonEventHandler(txbDrag999_PreviewMouseDown);
            lblHeader.FontSize = 19;
            lblHeader.Foreground = Brushes.Black;

            ctlPOD objPOD1 = new ctlPOD();
            objPOD1.AllowDrop = true;
            objPOD1.Height = 45;
            objPOD1.Width = 600;
            objPOD1.PreviewMouseDown += new MouseButtonEventHandler(objPOD_PreviewMouseDown);
            objPOD1.SetValue(Canvas.LeftProperty, 25.0);
            objPOD1.SetValue(Canvas.TopProperty, 0.0);
            MyPropGrid.ControlToBind = objPOD1;
            objPOD1.cnvPOD.Children.Add(lblHeader);
            currentControl = objPOD1;
            cnvPaint.Children.Add(objPOD1);

            Label lblQuestion = new Label();
            lblQuestion.Background = Brushes.Transparent;
            lblQuestion.Content = objQueCollection[CurrentQueCount].QuestionText;
            lblQuestion.PreviewMouseDown += new MouseButtonEventHandler(lblDrag_PreviewMouseDown);
            lblQuestion.FontSize = 17;

            lblQuestion.Foreground = Brushes.Black;

            ctlPOD objPOD2 = new ctlPOD();
            objPOD2.AllowDrop = true;
            objPOD2.Height = 45;
            objPOD2.Width = 600;
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
                    chk.Background = Brushes.Transparent;
                    chk.Content = objOptCollection[i].Options;
                    chk.Tag = objOptCollection[i].ActionQueueID.ToString() + "," + objOptCollection[i].ID.ToString();
                    chk.PreviewMouseDown += new MouseButtonEventHandler(chkDrag999_PreviewMouseDown);
                    chk.Height = 18;
                    chk.FontSize = 14;
                    varTop = 120 + (30 * i);

                    ctlPOD objPOD = new ctlPOD();
                    objPOD.AllowDrop = true;
                    objPOD.Height = 25;
                    objPOD.Width = 600;
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
                    chk.Background = Brushes.Transparent;
                    chk.Tag = objOptCollection[i].ActionQueueID.ToString() + "," + objOptCollection[i].ID.ToString();
                    chk.PreviewMouseDown += new MouseButtonEventHandler(radDrag999_PreviewMouseDown);
                    chk.Height = 18;
                    chk.FontSize = 14;
                    varTop = 120 + (30 * i);

                    ctlPOD objPOD = new ctlPOD();
                    objPOD.AllowDrop = true;
                    objPOD.Height = 25;
                    objPOD.Width = 600;
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
                lst.Background = Brushes.Transparent;
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
                objPOD.Width = 600;
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
                cmb.Background = Brushes.Transparent;
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
                objPOD.Width = 600;
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
            btnNext.Background = Brushes.Transparent;
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

            if (objOptCollection.Count == 0 ||varType.ToLower()=="none")
            {
                objPOD3.SetValue(Canvas.TopProperty, 200.0);
            }

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
        }

        void btnYes_Click(object sender, RoutedEventArgs e)
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
                //MessageBox.Show("Script Generated");
                btnPrev.IsEnabled = false;
                FncSaveFiles();
                //MessageBox.Show("Files Saved");
                goto rax;
            }

            if (CurrentQueCount == lstCanvas.Count)
            {
                //FncSetInnerCanvas();
                FncSetCanvas();
                //MessageBox.Show("Inner Canvas Set");
            }
            else
            {
                FncListToCanvas();
                //MessageBox.Show("List To Canvas Set");
            }

            if (CurrentQueCount + 1 == objQueCollection.Count)
            {
                btnNext.Content = "Finished";
            }

            //cnvPaint.Children.Clear();
        rax: ;
        }
        string server;
        void FncSaveFiles()
        {
            g = 0;
            try
            {
                Assembly ass2 = Assembly.GetEntryAssembly();

                strModPath = ass2.Location.Replace("VMukti.Presentation.exe", @"ScriptModules");

                DirectoryInfo d = new DirectoryInfo(strModPath);


                //  MessageBox.Show(strModPath);
                foreach (DirectoryInfo subDir in d.GetDirectories())
                {
                    // ShowDirectory(subDir);
                    // MessageBox.Show(subDir.Name);
                    foreach (DirectoryInfo subDir2 in subDir.GetDirectories())
                    {
                        string[] del = { str };
                        if (subDir2.Name.Contains(str))
                        {
                            string[] name = null;
                            name = subDir2.Name.Split(del, StringSplitOptions.None);

                            //  MessageBox.Show(name[1]+"   b");
                            if (name[1].Trim().Equals(""))
                            {
                                //do nothing

                            }
                            else
                            {
                                if (Int32.Parse(name[1]) > g)
                                    g = Int32.Parse(name[1]);


                            }

                        }
                    }
                }
                g++;
               // MessageBox.Show(g + "");
                if (g != 0)
                {
                    str += g.ToString();
                   // MessageBox.Show(g.ToString());
                    //if (!Directory.Exists(strModPath + "\\" + str, true))
                    //{
                    //    Directory.CreateDirectory(strModPath+"\\"+str , true);
                    //}
                }
                else
                {


                }
            }
            catch(Exception e)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(e, "FncSaveFiles", "ctlScriptdesigner.xaml.cs");
                g++;
            }
            if (flag == false)
            {
                _ScriptName += "_Script";  //Extending _Script with the script name so that it doesn't create conflicts with CRM with same name.
               server = ScriptName;
                
                _ScriptName += g.ToString();

                
                flag = true;
            }

           #region Create Base Solution

            try
            {
                if (Directory.Exists(Assembly.GetAssembly(this.GetType()).Location.Replace("ScriptDesigner.Presentation.dll", "SS")))
                {
                    Directory.Delete(Assembly.GetAssembly(this.GetType()).Location.Replace("ScriptDesigner.Presentation.dll", @"SS"), true);
                }
                Directory.CreateDirectory(Assembly.GetAssembly(this.GetType()).Location.Replace("ScriptDesigner.Presentation.dll", "SS"));
            }
            catch (Exception exp)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(exp, "FncSaveFiles", "ctlScriptdesigner.xaml.cs");
            }

            //MessageBox.Show(Assembly.GetAssembly(this.GetType()).Location.Replace("ScriptDesigner.Presentation.dll", "ScriptBase"));
            //MessageBox.Show(Assembly.GetAssembly(this.GetType()).Location.Replace("ScriptDesigner.Presentation.dll", "SS"));

            copyDirectory(Assembly.GetAssembly(this.GetType()).Location.Replace("ScriptDesigner.Presentation.dll", "ScriptBase"), Assembly.GetAssembly(this.GetType()).Location.Replace("ScriptDesigner.Presentation.dll", "SS"));
            if (!Directory.Exists(Assembly.GetAssembly(this.GetType()).Location.Replace("ScriptDesigner.Presentation.dll", @"SS\Script.DataAccess\ReferencedAssemblies")))
            {
                Directory.CreateDirectory(Assembly.GetAssembly(this.GetType()).Location.Replace("ScriptDesigner.Presentation.dll", @"SS\Script.DataAccess\ReferencedAssemblies"));
            }

            if (!Directory.Exists(Assembly.GetAssembly(this.GetType()).Location.Replace("ScriptDesigner.Presentation.dll", @"SS\Script.Presentation\ReferencedAssemblies")))
            {
                Directory.CreateDirectory(Assembly.GetAssembly(this.GetType()).Location.Replace("ScriptDesigner.Presentation.dll", @"SS\Script.Presentation\ReferencedAssemblies"));
            }
            
           #endregion



            if (startQuestion == "")
            {
                startQuestion = objQueCollection[CurrentQueCount - 1].QuestionName;
                startQuestionID = objQueCollection[CurrentQueCount - 1].ID;
            }

            #region Saving Files

            CurrentQueCount = -1;

            //MessageBox.Show(lstCanvas.Count.ToString());

            for (int j = 0; j < lstCanvas.Count; j++)
            {
                CurrentQueCount++;
                int Counter = 0;
                string strCode = "";
                pageCount++;

                #region Creating .XAML File

                string strXML = "";
                strXML = "<UserControl x:Class=\"Script.Presentation.UserControl" + objQueCollection[CurrentQueCount].ID.ToString() + "\" xmlns=\"http://schemas.microsoft.com/winfx/2006/xaml/presentation\" xmlns:x=\"http://schemas.microsoft.com/winfx/2006/xaml\" Height=\"" + lstCanvas[j].Height + "\" Width=\"" + lstCanvas[j].Width + "\">";
                strXML = strXML + char.ConvertFromUtf32(13) + "<Canvas Name=\"cnvPaint" + "\" Height=\"" + lstCanvas[j].Height + "\" Width=\"" + lstCanvas[j].Width + "\" Background=\"" + lstCanvas[j].Background + "\">";

                foreach (object o in lstCanvas[j].Children)
                {
                    if (o.GetType() == typeof(ctlPOD))
                    {
                        foreach (object chl in ((ctlPOD)o).cnvPOD.Children)
                        {
                            Counter++;
                            if (chl.GetType() == typeof(Button))
                            {
                                if (((Button)chl).Tag != null)
                                {
                                   // if (((Button)chl).Tag.ToString().ToLower() != "")
                                    {
                                     //   MessageBox.Show(((Button)chl).Content.ToString() + "     1");
                                        string[] chkStr = ((Button)chl).Tag.ToString().Split('-');

                                        if (chkStr[0] == "DISPOSITION")
                                        {
                                            strCode = strCode + char.ConvertFromUtf32(13) + " void control" + Counter.ToString() + "_Click(object sender, RoutedEventArgs e)" + char.ConvertFromUtf32(13) +
                                                "{" + char.ConvertFromUtf32(13) +
                                                "clsStartClass.sCurrentDispositionID = \"" + chkStr[1] + "\";" +
                                                char.ConvertFromUtf32(13) +
                                                //"VMuktiAPI.VMuktiHelper.CallEvent(\"HangUp\", this, new VMuktiAPI.VMuktiEventArgs(\"ScriptRender\", int.Parse(clsStartClass.sCurrentChannelID) + 1));" + char.ConvertFromUtf32(13) +
                                                "((MainPage)((Canvas)((Canvas)((Canvas)this.Parent).Parent).Parent).Parent).SetDispositionCanvas();" + char.ConvertFromUtf32(13) +
                                                char.ConvertFromUtf32(13) +
                                                "}";
                                        }
                                        else
                                        {

                                            TextReader tr = new StreamReader(Assembly.GetAssembly(this.GetType()).Location.Replace("ScriptDesigner.Presentation.dll", "\\ScriptPages\\FixedCode.txt"));
                                            string abc = tr.ReadToEnd();
                                            tr.Close();

                                            strCode = strCode + char.ConvertFromUtf32(13) + " void control" + Counter.ToString() + "_Click(object sender, RoutedEventArgs e)" + char.ConvertFromUtf32(13) +
                                                "{" + char.ConvertFromUtf32(13) +
                                                "ClsQuestionCollectionR objQueCollection = ClsQuestionCollectionR.GetAll(int.Parse(\"" + _ScriptID.ToString() + "\"));" + char.ConvertFromUtf32(13) +
                                                abc + char.ConvertFromUtf32(13) +
                                                "}";
                                        }
                                        string strContent = ((Button)chl).Content.ToString();
                                        strContent = strContent.Replace("&", "&amp;");
                                        strContent = strContent.Replace("\"", "&quot;");
                                        strContent = strContent.Replace("<", "&lt;");
                                        strContent = strContent.Replace(">", "&gt;");

                                        strXML = strXML + char.ConvertFromUtf32(13) + "<Button Name=\"control" + Counter.ToString() + "\" Height=\"" + ((ctlPOD)o).Height + "\" Width=\"" + ((ctlPOD)o).Width + "\" Canvas.Left=\"" + ((ctlPOD)o).GetValue(Canvas.LeftProperty) + "\" Canvas.Top=\"" + ((ctlPOD)o).GetValue(Canvas.TopProperty) + "\" Background=\"" + ((Button)chl).Background + "\" Foreground=\"" + ((Button)chl).Foreground + "\" Tag=\"" + ((Button)chl).Tag + "\" Content=\"" + strContent + "\" FontFamily=\"" + ((Button)chl).FontFamily + "\" FontSize=\"" + ((Button)chl).FontSize + "\" FontWeight=\"" + ((Button)chl).FontWeight + "\" FontStyle=\"" + ((Button)chl).FontStyle + "\" Click=\"control" + Counter.ToString() + "_Click" + "\"/>";
                                    }
                                    //else
                                    {
                                     //   MessageBox.Show(((Button)chl).Content.ToString() + "     2");
                                    }
                                }
                                else
                                {
                                   // MessageBox.Show(((Button)chl).Content.ToString() + "     3");
                                    string strContent = ((Button)chl).Content.ToString();
                                    strContent = strContent.Replace("&", "&amp;");
                                    strContent = strContent.Replace("\"", "&quot;");
                                    strContent = strContent.Replace("<", "&lt;");
                                    strContent = strContent.Replace(">", "&gt;");

                                    strXML = strXML + char.ConvertFromUtf32(13) + "<Button Name=\"control" + Counter.ToString() + "\" Height=\"" + ((ctlPOD)o).Height + "\" Width=\"" + ((ctlPOD)o).Width + "\" Canvas.Left=\"" + ((ctlPOD)o).GetValue(Canvas.LeftProperty) + "\" Canvas.Top=\"" + ((ctlPOD)o).GetValue(Canvas.TopProperty) + "\" Background=\"" + ((Button)chl).Background + "\" Foreground=\"" + ((Button)chl).Foreground + "\" Tag=\"" + ((Button)chl).Tag + "\" Content=\"" + strContent + "\" FontFamily=\"" + ((Button)chl).FontFamily + "\" FontSize=\"" + ((Button)chl).FontSize + "\" FontWeight=\"" + ((Button)chl).FontWeight + "\" FontStyle=\"" + ((Button)chl).FontStyle + "\"/>";
                                }
                            }

                            else if (chl.GetType() == typeof(TextBox))
                            {
                                string strContent = ((TextBox)chl).Text.ToString();
                                strContent = strContent.Replace("&", "&amp;");
                                strContent = strContent.Replace("\"", "&quot;");
                                strContent = strContent.Replace("<", "&lt;");
                                strContent = strContent.Replace(">", "&gt;");

                                strXML = strXML + char.ConvertFromUtf32(13) + "<TextBox Name=\"control" + Counter.ToString() + "\" Height=\"" + ((ctlPOD)o).Height + "\" Width=\"" + ((ctlPOD)o).Width + "\" Canvas.Left=\"" + ((ctlPOD)o).GetValue(Canvas.LeftProperty) + "\" Canvas.Top=\"" + ((ctlPOD)o).GetValue(Canvas.TopProperty) + "\" Background=\"" + ((TextBox)chl).Background + "\" Foreground=\"" + ((TextBox)chl).Foreground + "\" Tag=\"" + ((TextBox)chl).Tag + "\" Text=\"" + strContent + "\" FontFamily=\"" + ((TextBox)chl).FontFamily + "\" FontSize=\"" + ((TextBox)chl).FontSize + "\" FontWeight=\"" + ((TextBox)chl).FontWeight + "\" FontStyle=\"" + ((TextBox)chl).FontStyle + "\"/>";
                            }

                            else if (chl.GetType() == typeof(TextBlock))
                            {
                                string strContent = ((TextBlock)chl).Text.ToString();
                                strContent = strContent.Replace("&", "&amp;");
                                strContent = strContent.Replace("\"", "&quot;");
                                strContent = strContent.Replace("<", "&lt;");
                                strContent = strContent.Replace(">", "&gt;");

                                strXML = strXML + char.ConvertFromUtf32(13) + "<TextBlock Name=\"control" + Counter.ToString() + "\" Height=\"" + ((ctlPOD)o).Height + "\" Width=\"" + ((ctlPOD)o).Width + "\" Canvas.Left=\"" + ((ctlPOD)o).GetValue(Canvas.LeftProperty) + "\" Canvas.Top=\"" + ((ctlPOD)o).GetValue(Canvas.TopProperty) + "\" Background=\"" + ((TextBlock)chl).Background + "\" Foreground=\"" + ((TextBlock)chl).Foreground + "\" Tag=\"" + ((TextBlock)chl).Tag + "\" Text=\"" + strContent + "\" FontFamily=\"" + ((TextBlock)chl).FontFamily + "\" FontSize=\"" + ((TextBlock)chl).FontSize + "\" FontWeight=\"" + ((TextBlock)chl).FontWeight + "\" FontStyle=\"" + ((TextBlock)chl).FontStyle + "\" TextWrapping=\"Wrap\" />";
                            }

                            else if (chl.GetType() == typeof(Label))
                            {
                                string strContent = ((Label)chl).Content.ToString();
                                strContent = strContent.Replace("&", "&amp;");
                                strContent = strContent.Replace("\"", "&quot;");
                                strContent = strContent.Replace("<", "&lt;");
                                strContent = strContent.Replace(">", "&gt;");

                                strXML = strXML + char.ConvertFromUtf32(13) + "<Label Name=\"control" + Counter.ToString() + "\" Height=\"" + ((ctlPOD)o).Height + "\" Width=\"" + ((ctlPOD)o).Width + "\" Canvas.Left=\"" + ((ctlPOD)o).GetValue(Canvas.LeftProperty) + "\" Canvas.Top=\"" + ((ctlPOD)o).GetValue(Canvas.TopProperty) + "\" Background=\"" + ((Label)chl).Background + "\" Foreground=\"" + ((Label)chl).Foreground + "\" Tag=\"" + ((Label)chl).Tag + "\" Content=\"" + strContent + "\" FontFamily=\"" + ((Label)chl).FontFamily + "\" FontSize=\"" + ((Label)chl).FontSize + "\" FontWeight=\"" + ((Label)chl).FontWeight + "\" FontStyle=\"" + ((Label)chl).FontStyle + "\"/>";
                            }

                            else if (chl.GetType() == typeof(ComboBox))
                            {
                                strXML = strXML + char.ConvertFromUtf32(13) + "<ComboBox Name=\"control" + Counter.ToString() + "\" Height=\"" + ((ctlPOD)o).Height + "\" Width=\"" + ((ctlPOD)o).Width + "\" Canvas.Left=\"" + ((ctlPOD)o).GetValue(Canvas.LeftProperty) + "\" Canvas.Top=\"" + ((ctlPOD)o).GetValue(Canvas.TopProperty) + "\" Background=\"" + ((ComboBox)chl).Background + "\" Foreground=\"" + ((ComboBox)chl).Foreground + "\" Tag=\"" + ((ComboBox)chl).Tag + "\" FontFamily=\"" + ((ComboBox)chl).FontFamily + "\" FontSize=\"" + ((ComboBox)chl).FontSize + "\" FontWeight=\"" + ((ComboBox)chl).FontWeight + "\" FontStyle=\"" + ((ComboBox)chl).FontStyle + "\">";
                                for (int i = 0; i < ((ComboBox)chl).Items.Count; i++)
                                {
                                    Counter++;

                                    string strContent = ((ComboBoxItem)((ComboBox)chl).Items[i]).Content.ToString();
                                    strContent = strContent.Replace("&", "&amp;");
                                    strContent = strContent.Replace("\"", "&quot;");
                                    strContent = strContent.Replace("<", "&lt;");
                                    strContent = strContent.Replace(">", "&gt;");

                                    strXML = strXML + char.ConvertFromUtf32(13) + "<ComboBoxItem Name=\"control" + Counter.ToString() + "\" Background=\"" + ((ComboBoxItem)((ComboBox)chl).Items[i]).Background + "\" Foreground=\"" + ((ComboBoxItem)((ComboBox)chl).Items[i]).Foreground + "\" Tag=\"" + ((ComboBoxItem)((ComboBox)chl).Items[i]).Tag + "\" Content=\"" + strContent + "\" FontFamily=\"" + ((ComboBoxItem)((ComboBox)chl).Items[i]).FontFamily + "\" FontSize=\"" + ((ComboBoxItem)((ComboBox)chl).Items[i]).FontSize + "\" FontWeight=\"" + ((ComboBoxItem)((ComboBox)chl).Items[i]).FontWeight + "\" FontStyle=\"" + ((ComboBoxItem)((ComboBox)chl).Items[i]).FontStyle + "\"/>";
                                }
                                strXML = strXML + char.ConvertFromUtf32(13) + "</ComboBox>";
                            }

                            else if (chl.GetType() == typeof(ListBox))
                            {
                                strXML = strXML + char.ConvertFromUtf32(13) + "<ListBox Name=\"control" + Counter.ToString() + "\" Height=\"" + ((ctlPOD)o).Height + "\" Width=\"" + ((ctlPOD)o).Width + "\" Canvas.Left=\"" + ((ctlPOD)o).GetValue(Canvas.LeftProperty) + "\" Canvas.Top=\"" + ((ctlPOD)o).GetValue(Canvas.TopProperty) + "\" Background=\"" + ((ListBox)chl).Background + "\" Foreground=\"" + ((ListBox)chl).Foreground + "\" Tag=\"" + ((ListBox)chl).Tag + "\" FontFamily=\"" + ((ListBox)chl).FontFamily + "\" FontSize=\"" + ((ListBox)chl).FontSize + "\" FontWeight=\"" + ((ListBox)chl).FontWeight + "\" FontStyle=\"" + ((ListBox)chl).FontStyle + "\">";
                                for (int i = 0; i < ((ListBox)chl).Items.Count; i++)
                                {
                                    Counter++;

                                    string strContent = ((ListBoxItem)((ListBox)chl).Items[i]).Content.ToString();
                                    strContent = strContent.Replace("&", "&amp;");
                                    strContent = strContent.Replace("\"", "&quot;");
                                    strContent = strContent.Replace("<", "&lt;");
                                    strContent = strContent.Replace(">", "&gt;");

                                    strXML = strXML + char.ConvertFromUtf32(13) + "<ListBoxItem Name=\"control" + Counter.ToString() + "\" Background=\"" + ((ListBoxItem)((ListBox)chl).Items[i]).Background + "\" Foreground=\"" + ((ListBoxItem)((ListBox)chl).Items[i]).Foreground + "\" Tag=\"" + ((ListBoxItem)((ListBox)chl).Items[i]).Tag + "\" Content=\"" + strContent + "\" FontFamily=\"" + ((ListBoxItem)((ListBox)chl).Items[i]).FontFamily + "\" FontSize=\"" + ((ListBoxItem)((ListBox)chl).Items[i]).FontSize + "\" FontWeight=\"" + ((ListBoxItem)((ListBox)chl).Items[i]).FontWeight + "\" FontStyle=\"" + ((ListBoxItem)((ListBox)chl).Items[i]).FontStyle + "\"/>";
                                }
                                strXML = strXML + char.ConvertFromUtf32(13) + "</ListBox>";
                            }

                            else if (chl.GetType() == typeof(RadioButton))
                            {
                                string strContent = ((RadioButton)chl).Content.ToString();
                                strContent = strContent.Replace("&", "&amp;");
                                strContent = strContent.Replace("\"", "&quot;");
                                strContent = strContent.Replace("<", "&lt;");
                                strContent = strContent.Replace(">", "&gt;");

                                strXML = strXML + char.ConvertFromUtf32(13) + "<RadioButton Name=\"control" + Counter.ToString() + "\" Height=\"" + ((ctlPOD)o).Height + "\" Width=\"" + ((ctlPOD)o).Width + "\" Canvas.Left=\"" + ((ctlPOD)o).GetValue(Canvas.LeftProperty) + "\" Canvas.Top=\"" + ((ctlPOD)o).GetValue(Canvas.TopProperty) + "\" Background=\"" + ((RadioButton)chl).Background + "\" Foreground=\"" + ((RadioButton)chl).Foreground + "\" Tag=\"" + ((RadioButton)chl).Tag + "\" Content=\"" + strContent + "\" FontFamily=\"" + ((RadioButton)chl).FontFamily + "\" FontSize=\"" + ((RadioButton)chl).FontSize + "\" FontWeight=\"" + ((RadioButton)chl).FontWeight + "\" FontStyle=\"" + ((RadioButton)chl).FontStyle + "\"/>";
                            }

                            else if (chl.GetType() == typeof(CheckBox))
                            {
                                string strContent = ((CheckBox)chl).Content.ToString();
                                strContent = strContent.Replace("&", "&amp;");
                                strContent = strContent.Replace("\"", "&quot;");
                                strContent = strContent.Replace("<", "&lt;");
                                strContent = strContent.Replace(">", "&gt;");

                                strXML = strXML + char.ConvertFromUtf32(13) + "<CheckBox Name=\"control" + Counter.ToString() + "\" Height=\"" + ((ctlPOD)o).Height + "\" Width=\"" + ((ctlPOD)o).Width + "\" Canvas.Left=\"" + ((ctlPOD)o).GetValue(Canvas.LeftProperty) + "\" Canvas.Top=\"" + ((ctlPOD)o).GetValue(Canvas.TopProperty) + "\" Background=\"" + ((CheckBox)chl).Background + "\" Foreground=\"" + ((CheckBox)chl).Foreground + "\" Tag=\"" + ((CheckBox)chl).Tag + "\" Content=\"" + strContent + "\" FontFamily=\"" + ((CheckBox)chl).FontFamily + "\" FontSize=\"" + ((CheckBox)chl).FontSize + "\" FontWeight=\"" + ((CheckBox)chl).FontWeight + "\" FontStyle=\"" + ((CheckBox)chl).FontStyle + "\"/>";
                            }

                        }
                    }
                }
                strXML = strXML + "</Canvas></UserControl>";

                //TextWriter tw = new StreamWriter(Assembly.GetAssembly(this.GetType()).Location.Replace("ScriptDesigner.Presentation.dll", "\\SS\\Script.Presentation\\" + objQueCollection[CurrentQueCount].QuestionName.ToString() + ".Xaml"));
                TextWriter tw = new StreamWriter(Assembly.GetAssembly(this.GetType()).Location.Replace("ScriptDesigner.Presentation.dll", "\\SS\\Script.Presentation\\" + objQueCollection[CurrentQueCount].ID.ToString() + ".Xaml"));
                tw.WriteLine(strXML);
                tw.Close();

                TextReader tr101 = new StreamReader(Assembly.GetAssembly(this.GetType()).Location.Replace("ScriptDesigner.Presentation.dll","\\ScriptPages\\Binded Data.txt"));
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
                         "using Script.Business;" + char.ConvertFromUtf32(13) +
                         char.ConvertFromUtf32(13) +

                         "namespace Script.Presentation" + char.ConvertFromUtf32(13) +
                         "{" + char.ConvertFromUtf32(13) + char.ConvertFromUtf32(13) + "public partial class UserControl" + objQueCollection[CurrentQueCount].ID.ToString() + ": UserControl" + char.ConvertFromUtf32(13) +
                           "{" + char.ConvertFromUtf32(13) +
                               "public UserControl" + objQueCollection[CurrentQueCount].ID.ToString() + "()" + char.ConvertFromUtf32(13) +
                                   "{" + char.ConvertFromUtf32(13) +
                                   "InitializeComponent();" + char.ConvertFromUtf32(13) +
                    //"VMuktiAPI.VMuktiHelper.RegisterEvent(\"SetLeadIDCRM\").VMuktiEvent += new VMuktiAPI.VMuktiEvents.VMuktiEventHandler(SetLeadIDCRM_VMuktiEvent);" + char.ConvertFromUtf32(13) +
                                   "FncRefresh();" + char.ConvertFromUtf32(13) +
                                   "}" + char.ConvertFromUtf32(13) +
                                   "public Canvas GetCanvas()" + char.ConvertFromUtf32(13) +
                                   "{" + char.ConvertFromUtf32(13) +
                                        "return this.cnvPaint;" +
                                    "}" + char.ConvertFromUtf32(13) +
                                     str101 + char.ConvertFromUtf32(13) +
                                     strCode + char.ConvertFromUtf32(13) +
                                     //char.ConvertFromUtf32(13) + "}" + char.ConvertFromUtf32(13) + str101 + char.ConvertFromUtf32(13) + strCode + char.ConvertFromUtf32(13) +
                           
                            "}" + char.ConvertFromUtf32(13) +
                            "}";
                //}
                //else {

                    //strXML = strXML + char.ConvertFromUtf32(13) +
                    //   "public partial class UserControl" + objQueCollection[CurrentQueCount].ID.ToString() + ": UserControl" + char.ConvertFromUtf32(13) +
                    //       "{"  + char.ConvertFromUtf32(13) + "ModulePermissions[] _MyPermissions;" + char.ConvertFromUtf32(13) +
                    //           "public UserControl" + objQueCollection[CurrentQueCount].ID.ToString() + "()" + char.ConvertFromUtf32(13) +
                    //               "{" + char.ConvertFromUtf32(13) +
                    //               "InitializeComponent();" + char.ConvertFromUtf32(13) +
                    //               "VMuktiAPI.VMuktiHelper.RegisterEvent(\"SetLeadIDScript\").VMuktiEvent += new VMuktiAPI.VMuktiEvents.VMuktiEventHandler(Script_VMuktiEvent);" + char.ConvertFromUtf32(13) +
                    //               "}" + char.ConvertFromUtf32(13) + str101 + char.ConvertFromUtf32(13) + strCode + char.ConvertFromUtf32(13) +
                    //       "}" + char.ConvertFromUtf32(13) +
                    //        "}";
                //}
                TextWriter tw1 = new StreamWriter(Assembly.GetAssembly(this.GetType()).Location.Replace("ScriptDesigner.Presentation.dll", "\\SS\\Script.Presentation\\" + objQueCollection[CurrentQueCount].ID.ToString() + ".Xaml.Cs"));
                //TextWriter tw1 = new StreamWriter(Assembly.GetAssembly(this.GetType()).Location.Replace("ScriptDesigner.Presentation.dll", "\\SS\\Script.Presentation\\" + objQueCollection[CurrentQueCount].QuestionName.ToString() + ".Xaml.Cs"));
                tw1.WriteLine(strXML);
                tw1.Close();

                AddRef1_2 = AddRef1_2 + char.ConvertFromUtf32(13) + "<Page Include=\"" + objQueCollection[CurrentQueCount].ID.ToString() + ".xaml\">"
                + char.ConvertFromUtf32(13) + "<Generator>MSBuild:Compile</Generator>"
                + char.ConvertFromUtf32(13) + "<SubType>Designer</SubType>"
                + char.ConvertFromUtf32(13) + "</Page>";

                AddRef2_3 = AddRef2_3 + char.ConvertFromUtf32(13) + "<Compile Include=\"" + objQueCollection[CurrentQueCount].ID.ToString() + ".Xaml.Cs\">"
                + char.ConvertFromUtf32(13) + "<DependentUpon>" + objQueCollection[CurrentQueCount].ID.ToString() + ".Xaml</DependentUpon>"
                + char.ConvertFromUtf32(13) + "<SubType>Code</SubType>"
                + char.ConvertFromUtf32(13) + "</Compile>";

                #endregion
            }

            //MessageBox.Show("Now Creating Project Files");

            #region Create Project File

            TextWriter tw11 = new StreamWriter(Assembly.GetAssembly(this.GetType()).Location.Replace("ScriptDesigner.Presentation.dll","\\SS\\Script.Presentation\\Script.Presentation.csproj"));

            TextReader tr1 = new StreamReader(Assembly.GetAssembly(this.GetType()).Location.Replace("ScriptDesigner.Presentation.dll", "\\ScriptPages\\1.txt"));
            string str1 = tr1.ReadToEnd();
            tr1.Close();

            TextReader tr2 = new StreamReader(Assembly.GetAssembly(this.GetType()).Location.Replace("ScriptDesigner.Presentation.dll",  "\\ScriptPages\\2.txt"));
            string str2 = tr2.ReadToEnd();
            tr2.Close();

            TextReader tr3 = new StreamReader(Assembly.GetAssembly(this.GetType()).Location.Replace("ScriptDesigner.Presentation.dll", "\\ScriptPages\\3.txt"));
            string str3 = tr3.ReadToEnd();
            tr3.Close();

            tw11.WriteLine(str1 + AddRef2_3 + str2 + AddRef1_2 + str3);
            str1 = "";
            str2 = "";
            str3 = "";
            AddRef1_2 = "";
            AddRef2_3 = "";
            tw11.Close();

            TextWriter twStart = new StreamWriter(Assembly.GetAssembly(this.GetType()).Location.Replace("ScriptDesigner.Presentation.dll",  "\\SS\\Script.Presentation\\clsStartClass.cs"));
            TextReader trClassRead = new StreamReader(Assembly.GetAssembly(this.GetType()).Location.Replace("ScriptDesigner.Presentation.dll","\\ScriptPages\\ClassRead.txt"));
            string strClassRead = trClassRead.ReadToEnd();
            trClassRead.Close();

            string strBetween = char.ConvertFromUtf32(13) + "public static string strStartQuesion = \"UserControl" + startQuestionID.ToString() + "\";" + char.ConvertFromUtf32(13) + "public static int LeadID = 0;" + char.ConvertFromUtf32(13) + "public static string sCurrentChannelID  = \"\";" + char.ConvertFromUtf32(13) + "public static string sCurrentDispositionID  = \"\";" + char.ConvertFromUtf32(13) + "public static string sCallingType  = \"\";" + char.ConvertFromUtf32(13) + "public static int CallID;" + char.ConvertFromUtf32(13) + "public static string StateName = null;" + char.ConvertFromUtf32(13) + "public static string ZipCode = null;" + char.ConvertFromUtf32(13) + "public static string sPhoneNumber = null;" + char.ConvertFromUtf32(13) + "}" + char.ConvertFromUtf32(13) + "}";

            twStart.WriteLine(strClassRead + strBetween);
            twStart.Close();

            #endregion


            #region DLL File Naming

            TextReader txtReader = new StreamReader(Assembly.GetAssembly(this.GetType()).Location.Replace("ScriptDesigner.Presentation.dll", "\\SS\\Script.Presentation\\Script.Presentation.csproj"));
            string txtString = txtReader.ReadToEnd();
            txtReader.Close();

            txtString = txtString.Replace("<AssemblyName>Script.Presentation</AssemblyName>", "<AssemblyName>" + _ScriptName + ".Presentation</AssemblyName>");

            TextWriter txtWriter = new StreamWriter(Assembly.GetAssembly(this.GetType()).Location.Replace("ScriptDesigner.Presentation.dll", "\\SS\\Script.Presentation\\Script.Presentation.csproj"));
            txtWriter.WriteLine(txtString);
            txtWriter.Close();



            TextReader txtReader1 = new StreamReader(Assembly.GetAssembly(this.GetType()).Location.Replace("ScriptDesigner.Presentation.dll", "\\SS\\Script.Business\\Script.Business.csproj"));
            string txtString1 = txtReader1.ReadToEnd();
            txtReader1.Close();

            txtString1 = txtString1.Replace("<AssemblyName>Script.Business</AssemblyName>", "<AssemblyName>" + _ScriptName + ".Business</AssemblyName>");

            TextWriter txtWriter1 = new StreamWriter(Assembly.GetAssembly(this.GetType()).Location.Replace("ScriptDesigner.Presentation.dll", "\\SS\\Script.Business\\Script.Business.csproj"));
            txtWriter1.WriteLine(txtString1);
            txtWriter1.Close();



            TextReader txtReader2 = new StreamReader(Assembly.GetAssembly(this.GetType()).Location.Replace("ScriptDesigner.Presentation.dll", "\\SS\\Script.Common\\Script.Common.csproj"));
            string txtString2 = txtReader2.ReadToEnd();
            txtReader2.Close();

            txtString2 = txtString2.Replace("<AssemblyName>Script.Common</AssemblyName>", "<AssemblyName>" + _ScriptName + ".Common</AssemblyName>");

            TextWriter txtWriter2 = new StreamWriter(Assembly.GetAssembly(this.GetType()).Location.Replace("ScriptDesigner.Presentation.dll", "\\SS\\Script.Common\\Script.Common.csproj"));
            txtWriter2.WriteLine(txtString2);
            txtWriter2.Close();

            TextReader txtReader3 = new StreamReader(Assembly.GetAssembly(this.GetType()).Location.Replace("ScriptDesigner.Presentation.dll", "\\SS\\Script.DataAccess\\Script.DataAccess.csproj"));
            string txtString3 = txtReader3.ReadToEnd();
            txtReader3.Close();

            txtString3 = txtString3.Replace("<AssemblyName>Script.DataAccess</AssemblyName>", "<AssemblyName>" + _ScriptName + ".DataAccess</AssemblyName>");

            TextWriter txtWriter3 = new StreamWriter(Assembly.GetAssembly(this.GetType()).Location.Replace("ScriptDesigner.Presentation.dll", "\\SS\\Script.DataAccess\\Script.DataAccess.csproj"));
            txtWriter3.WriteLine(txtString3);
            txtWriter3.Close();

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

            #region Building the solution File

            string winDir = Environment.GetFolderPath(Environment.SpecialFolder.System);
            winDir = winDir.Substring(0, winDir.LastIndexOf(@"\"));

            Process buildProcess = new Process();

            //TextWriter tw1111 = new StreamWriter(AppDomain.CurrentDomain.BaseDirectory + "\\Script\\Build.bat");
            //tw1111.WriteLine("cd\\");
            //tw1111.WriteLine("C:");
            //tw1111.WriteLine("cd \"C:\\WINDOWS\\Microsoft.NET\\Framework\\v3.5\"");
            //tw1111.WriteLine("msbuild \"" + AppDomain.CurrentDomain.BaseDirectory + "\\Script\\Script.sln\" /t:Rebuild /p:Configuration=Debug");
            //tw1111.Close();
            //System.Diagnostics.Process.Start(AppDomain.CurrentDomain.BaseDirectory + "\\Script\\Build.bat");

            if (!File.Exists(Assembly.GetAssembly(this.GetType()).Location.Replace("ScriptDesigner.Presentation.dll", @"\SS\Script.DataAccess\ReferencedAssemblies\VMuktiAPI.dll")))
                 File.Copy(Assembly.GetAssembly(typeof(VMuktiAPI.ClsPeer)).Location, Assembly.GetAssembly(this.GetType()).Location.Replace("ScriptDesigner.Presentation.dll", @"\SS\Script.DataAccess\ReferencedAssemblies\VMuktiAPI.dll"));
            //MessageBox.Show(Assembly.GetAssembly(typeof(VMuktiAPI.ClsPeer)).Location);
            //MessageBox.Show(Assembly.GetAssembly(this.GetType()).Location.Replace("ScriptDesigner.Presentation.dll", @"SS\Script.Presentation\ReferencedAssemblies\VMuktiAPI.dll"));
            if (!File.Exists(Assembly.GetAssembly(this.GetType()).Location.Replace("ScriptDesigner.Presentation.dll", @"SS\Script.Presentation\ReferencedAssemblies\VMuktiAPI.dll")))
            File.Copy(Assembly.GetAssembly(typeof(VMuktiAPI.ClsPeer)).Location, Assembly.GetAssembly(this.GetType()).Location.Replace("ScriptDesigner.Presentation.dll", @"SS\Script.Presentation\ReferencedAssemblies\VMuktiAPI.dll"));
            if (!File.Exists(Assembly.GetAssembly(this.GetType()).Location.Replace("ScriptDesigner.Presentation.dll", @"SS\Script.Presentation\ReferencedAssemblies\VMukti.CtlDatePicker.Presentation.dll")))
                File.Copy(Assembly.GetExecutingAssembly().Location.Replace("ScriptDesigner.Presentation.dll", "VMukti.CtlDatePicker.Presentation.dll"), Assembly.GetAssembly(this.GetType()).Location.Replace("ScriptDesigner.Presentation.dll", @"SS\Script.Presentation\ReferencedAssemblies\VMukti.CtlDatePicker.Presentation.dll"));
                //File.Copy(Assembly.GetAssembly(typeof(VMuktiAPI.ClsPeer)).Location, Assembly.GetAssembly(this.GetType()).Location.Replace("ScriptDesigner.Presentation.dll", @"SS\Script.Presentation\ReferencedAssemblies\VMukti.CtlDatePicker.Presentation.dll"));
            

            if (System.IO.Directory.Exists(winDir + @"\Microsoft.NET\Framework\v3.5\"))
            {
                buildProcess.StartInfo.WorkingDirectory = winDir + @"\Microsoft.NET\Framework\v3.5\";
                buildProcess.StartInfo.FileName = "msbuild";
                buildProcess.StartInfo.Arguments = @" """ + Assembly.GetAssembly(this.GetType()).Location.Replace("ScriptDesigner.Presentation.dll","") + @"\SS\Script.sln"" /t:Rebuild /p:Configuration=Debug";
                buildProcess.Start();
                
                buildProcess.WaitForExit();
            }
            else
            {
                MessageBox.Show("Microsoft .Net framework is not installed at --> \" " + winDir + @"\Microsoft.NET\Framework\v3.5\" + " \"!!");
            }


            #endregion

            try
            {
                if (Directory.Exists(Assembly.GetAssembly(this.GetType()).Location.Replace("ScriptDesigner.Presentation.dll", "") + ScriptName))
                {
                    Directory.Delete(Assembly.GetAssembly(this.GetType()).Location.Replace("ScriptDesigner.Presentation.dll", "") + ScriptName, true);
                }
                Directory.CreateDirectory(Assembly.GetAssembly(this.GetType()).Location.Replace("ScriptDesigner.Presentation.dll", "") + ScriptName);
                Directory.CreateDirectory(Assembly.GetAssembly(this.GetType()).Location.Replace("ScriptDesigner.Presentation.dll", "") + ScriptName + "\\" + ScriptName);
                Directory.CreateDirectory(Assembly.GetAssembly(this.GetType()).Location.Replace("ScriptDesigner.Presentation.dll", "") + ScriptName + "\\" + ScriptName + "\\BAL");
                Directory.CreateDirectory(Assembly.GetAssembly(this.GetType()).Location.Replace("ScriptDesigner.Presentation.dll", "") + ScriptName + "\\" + ScriptName + "\\DAL");
                Directory.CreateDirectory(Assembly.GetAssembly(this.GetType()).Location.Replace("ScriptDesigner.Presentation.dll", "") + ScriptName + "\\" + ScriptName + "\\Control");
                //Directory.CreateDirectory(Assembly.GetAssembly(this.GetType()).Location.Replace("ScriptDesigner.Presentation.dll", "") + @"Script\Common");
            }
            catch (Exception exp)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(exp, "FncSaveFiles", "ctlScriptdesigner.xaml.cs");
            }

            try
            {
                copyDirectory(Assembly.GetAssembly(this.GetType()).Location.Replace("ScriptDesigner.Presentation.dll", "") + @"SS\Script.Presentation\bin\Debug", Assembly.GetAssembly(this.GetType()).Location.Replace("ScriptDesigner.Presentation.dll", "") + ScriptName);
            }
            catch (Exception exp)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(exp, "FncsaveFiles", "ctlScriptdesigner.xaml.cs");
            }

            try
            {
                string[] filesToDelete = Directory.GetFiles(Assembly.GetAssembly(this.GetType()).Location.Replace("ScriptDesigner.Presentation.dll", "") + ScriptName, "*.pdb");
                for (int i = 0; i < filesToDelete.Length; i++)
                {
                    File.Delete(filesToDelete[i]);
                }
            }
            catch (Exception exp)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(exp, "FncSaveFiles", "ctlScriptdesigner.xaml.cs");
            }

            try
            {
                string[] filesToMove = Directory.GetFiles(Assembly.GetAssembly(this.GetType()).Location.Replace("ScriptDesigner.Presentation.dll", "") + ScriptName, "*.dll");
                for (int i = 0; i < filesToMove.Length; i++)
                {
                    if (filesToMove[i].Substring(filesToMove[i].LastIndexOf(@"\")).Contains("Business"))
                    {
                        File.Move(filesToMove[i], Assembly.GetAssembly(this.GetType()).Location.Replace("ScriptDesigner.Presentation.dll", "") + ScriptName + "\\" + ScriptName +  "\\BAL\\" + filesToMove[i].Substring(filesToMove[i].LastIndexOf(@"\")));
                    }
                    else if (filesToMove[i].Substring(filesToMove[i].LastIndexOf(@"\")).Contains("Common"))
                    {
                        File.Move(filesToMove[i], Assembly.GetAssembly(this.GetType()).Location.Replace("ScriptDesigner.Presentation.dll", "") + ScriptName + "\\" + ScriptName + "\\BAL\\" + filesToMove[i].Substring(filesToMove[i].LastIndexOf(@"\")));
                    }
                    else if (filesToMove[i].Substring(filesToMove[i].LastIndexOf(@"\")).Contains("DataAccess"))
                    {
                        File.Move(filesToMove[i], Assembly.GetAssembly(this.GetType()).Location.Replace("ScriptDesigner.Presentation.dll", "") + ScriptName + "\\" + ScriptName + "\\DAL\\" + filesToMove[i].Substring(filesToMove[i].LastIndexOf(@"\")));
                    }
                    else if (filesToMove[i].Substring(filesToMove[i].LastIndexOf(@"\")).Contains("Presentation"))
                    {
                        if (filesToMove[i].Substring(filesToMove[i].LastIndexOf(@"\")).Contains("CtlDatePicker"))
                        {
                            File.Delete(filesToMove[i]);
                        }
                        else
                        {
                        File.Move(filesToMove[i], Assembly.GetAssembly(this.GetType()).Location.Replace("ScriptDesigner.Presentation.dll", "") + ScriptName + "\\" + ScriptName + "\\Control\\" + filesToMove[i].Substring(filesToMove[i].LastIndexOf(@"\")));
                        }
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
                VMuktiAPI.VMuktiHelper.ExceptionHandler(exp, "FncSaveFiles", "ctlScriptdesigner.xaml.cs");
            }

            try
            {
                Directory.Delete(Assembly.GetAssembly(this.GetType()).Location.Replace("ScriptDesigner.Presentation.dll", "") + @"SS\Script.Presentation\bin", true);
                Directory.Delete(Assembly.GetAssembly(this.GetType()).Location.Replace("ScriptDesigner.Presentation.dll", "") + @"SS\Script.Presentation\obj", true);
                Directory.Delete(Assembly.GetAssembly(this.GetType()).Location.Replace("ScriptDesigner.Presentation.dll", "") + @"SS\Script.Business\bin", true);
                Directory.Delete(Assembly.GetAssembly(this.GetType()).Location.Replace("ScriptDesigner.Presentation.dll", "") + @"SS\Script.Business\obj", true);
                Directory.Delete(Assembly.GetAssembly(this.GetType()).Location.Replace("ScriptDesigner.Presentation.dll", "") + @"SS\Script.Common\bin", true);
                Directory.Delete(Assembly.GetAssembly(this.GetType()).Location.Replace("ScriptDesigner.Presentation.dll", "") + @"SS\Script.Common\obj", true);
                Directory.Delete(Assembly.GetAssembly(this.GetType()).Location.Replace("ScriptDesigner.Presentation.dll", "") + @"SS\Script.DataAccess\bin", true);
                Directory.Delete(Assembly.GetAssembly(this.GetType()).Location.Replace("ScriptDesigner.Presentation.dll", "") + @"SS\Script.DataAccess\obj", true);
            }
            catch (Exception exp)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(exp, "FncSaveFiles", "ctlScriptdesigner.xaml.cs");
            }

            ICSharpCode.SharpZipLib.Zip.FastZip fz = new ICSharpCode.SharpZipLib.Zip.FastZip();
            try
            {
                //MessageBox.Show("File Name - " + Assembly.GetAssembly(this.GetType()).Location.Replace("ScriptDesigner.Presentation.dll", "") + @"SS.zip");
                //MessageBox.Show("Source - " + Assembly.GetAssembly(this.GetType()).Location.Replace("ScriptDesigner.Presentation.dll", "") + @"SS");
                fz.CreateZip(Assembly.GetAssembly(this.GetType()).Location.Replace("ScriptDesigner.Presentation.dll", "") + @"SS.zip", Assembly.GetAssembly(this.GetType()).Location.Replace("ScriptDesigner.Presentation.dll", "") + @"SS", true, "");
                
                
            }
            catch (Exception exp)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(exp, "FncSaveFiles", "ctlScriptdesigner.xaml.cs");
            }

            try
            {
                fz.CreateZip(Assembly.GetAssembly(this.GetType()).Location.Replace("ScriptDesigner.Presentation.dll", "") + server + @".zip", Assembly.GetAssembly(this.GetType()).Location.Replace("ScriptDesigner.Presentation.dll", "") + ScriptName, true, "");
            }
            catch (Exception exp)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(exp, "FncSaveFiles", "ctlScriptdesigner.xaml.cs");
            }

            try
            {
                Directory.Delete(Assembly.GetAssembly(this.GetType()).Location.Replace("ScriptDesigner.Presentation.dll", "") + @"SS", true);
            }
            catch (Exception exp)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(exp, "FncSaveFiles", "ctlScriptdesigner.xaml.cs");
            }

            try
            {
                Directory.Delete(Assembly.GetAssembly(this.GetType()).Location.Replace("ScriptDesigner.Presentation.dll", "") + ScriptName, true);
            }
            catch (Exception exp)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(exp, "FncSaveFiles", "ctlScriptdesigner.xaml.cs");
            }
            
            try
            {
                System.Windows.Forms.DialogResult sfdRes;
                System.Windows.Forms.SaveFileDialog sfd = new System.Windows.Forms.SaveFileDialog();
                sfd.Filter = "Zip file(s) (*.zip)|*.zip";
                sfd.Title = "Save the source file of script to edit manualy!!";
                sfd.FileName = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + @"\SS.zip";
                sfdRes = sfd.ShowDialog();
                if (sfdRes == System.Windows.Forms.DialogResult.OK)
                {
                    if (File.Exists(sfd.FileName))
                    {
                        File.Delete(sfd.FileName);
                    }
                    File.Move(Assembly.GetAssembly(this.GetType()).Location.Replace("ScriptDesigner.Presentation.dll", "") + @"SS.zip", sfd.FileName);
                }

                //sfd.Title = "Save the file of script to add as a module in VMukti plateform!!";
                //sfd.FileName = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + @"\ScriptRender.zip";
                //sfdRes = sfd.ShowDialog();
                //if (sfdRes == System.Windows.Forms.DialogResult.OK)
                //{
                //    if (File.Exists(sfd.FileName))
                //    {
                //        File.Delete(sfd.FileName);
                //    }
                //    File.Move(Assembly.GetAssembly(this.GetType()).Location.Replace("ScriptDesigner.Presentation.dll", "") + @"ScriptRender.zip", sfd.FileName);
                //}

                
                
                #region UploadFile
                try
                {
                    System.IO.FileInfo fileInfo = new System.IO.FileInfo(Assembly.GetAssembly(this.GetType()).Location.Replace("ScriptDesigner.Presentation.dll", "") + server + @".zip");
                    System.IO.FileStream stream = new System.IO.FileStream(Assembly.GetAssembly(this.GetType()).Location.Replace("ScriptDesigner.Presentation.dll", "") + server + @".zip", System.IO.FileMode.Open, System.IO.FileAccess.Read);
                RemoteFileInfo rfi = new RemoteFileInfo();
                rfi.FileName = fileInfo.Name;
                rfi.Length = fileInfo.Length;
                rfi.FileByteStream = stream;
                rfi.FolderNameToStore = "Scripts";
                clientHttpFileTransfer.svcHTTPFileTransferServiceUploadFileToInstallationDirectory(rfi);
                stream.Close();
                    if (File.Exists(Assembly.GetAssembly(this.GetType()).Location.Replace("ScriptDesigner.Presentation.dll", "") + server + @".zip"))
                    {
                        File.Delete(Assembly.GetAssembly(this.GetType()).Location.Replace("ScriptDesigner.Presentation.dll", "") + server + @".zip");
                    }
                }
                catch (Exception er)
                {
                    MessageBox.Show(er.Message);
                }
                #endregion
            }
            catch (Exception exp)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(exp, "FncSaveFiles", "ctlScriptdesigner.xaml.cs");
            }
            


            #endregion
        }

        private void btnBack_Unloaded(object sender, RoutedEventArgs e)
        {
            try
            {
                if (Directory.Exists(Assembly.GetAssembly(this.GetType()).Location.Replace("ScriptDesigner.Presentation.dll", "") + @"SS"))
                     Directory.Delete(Assembly.GetAssembly(this.GetType()).Location.Replace("ScriptDesigner.Presentation.dll", "") + @"SS", true);
                if (File.Exists(Assembly.GetAssembly(this.GetType()).Location.Replace("ScriptDesigner.Presentation.dll", "") + server + @".zip"))
                {
                    File.Delete(Assembly.GetAssembly(this.GetType()).Location.Replace("ScriptDesigner.Presentation.dll", "") + server + @".zip");
                }
            }
            catch (Exception exp)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(exp, "btnBack_Unloaded", "ctlScriptdesigner.xaml.cs");
            }
        }

        int g = 0;
        int DownloadAndExtractZipFile()
        {
            #region Download and Extract Zip file
            try
            {





                str = ScriptName;
                str += "_Script";

                Assembly ass = Assembly.GetEntryAssembly();
                // this maybe something like "using Vmuktiapi"
                filename = str + ".zip";

                #region Download Zip File using WCF FileTranserService

                try
                {
                    BasicHttpClient bhcFts = new BasicHttpClient();
                    clientHttpFileTransfer = (IHTTPFileTransferService)bhcFts.OpenClient<IHTTPFileTransferService>("http://" + VMuktiInfo.BootStrapIPs[0] + ":80/VMukti/HttpFileTransferService");
                    clientHttpFileTransfer.svcHTTPFileTransferServiceJoin();
                    DownloadRequest request = new DownloadRequest();
                    RemoteFileInfo rfi = new RemoteFileInfo();

                    request.FileName = filename;
                    request.FolderWhereFileIsStored = "Scripts";
                    rfi = clientHttpFileTransfer.svcHTTPFileTransferServiceDownloadFile(request);

                

                    if (!Directory.Exists(ass.Location.Replace("VMukti.Presentation.exe", @"Scripts")))
                    {
                        Directory.CreateDirectory(ass.Location.Replace("VMukti.Presentation.exe", @"Scripts"));
                    }
                    destination = ass.Location.Replace("VMukti.Presentation.exe", @"Scripts");

                 
                    filePath = destination + "\\" + filename;

                    System.IO.Stream inputStream = rfi.FileByteStream;


                    using (System.IO.FileStream writeStream = new System.IO.FileStream(filePath, System.IO.FileMode.Create, System.IO.FileAccess.Write, FileShare.ReadWrite))
                    {
                        int chunkSize = 2048;
                        byte[] buffer = new byte[chunkSize];

                        do
                        {
                            // read bytes from input stream
                            int bytesRead = inputStream.Read(buffer, 0, chunkSize);
                            if (bytesRead == 0) break;

                            // write bytes to output stream
                            writeStream.Write(buffer, 0, bytesRead);

                        } while (true);
                        writeStream.Close();
                    }
                    //}
                }
                catch (Exception ex)
                {
                   
                    VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "DownloadAndExtractZipFile", "ctlScriptdesigner.xaml.cs");
                    return -1;
                }

                #endregion

                #region Downloading ZipFile Using WebClient  -----Commented


                #endregion

                #region Extracting

                if (!Directory.Exists(ass.Location.Replace("VMukti.Presentation.exe", @"ScriptModules")))
                {
                    Directory.CreateDirectory(ass.Location.Replace("VMukti.Presentation.exe", @"ScriptModules"));
                }



              
                try
                {
                    str = ScriptName;
                    str += "_Script";

                    Assembly ass2 = Assembly.GetEntryAssembly();

                    strModPath = ass2.Location.Replace("VMukti.Presentation.exe", @"ScriptModules");

                
                    
                  
                   
                }
                catch (Exception e)
                {
                    VMuktiAPI.VMuktiHelper.ExceptionHandler(e, "DownloadAndExtractZipFile", "ctlScriptdesigner.xaml.cs");
                }





            
                ICSharpCode.SharpZipLib.Zip.FastZip fz = new ICSharpCode.SharpZipLib.Zip.FastZip();

               // if (!Directory.Exists(strModPath + "\\" + str  ))
                {
                    fz.ExtractZip(destination + "\\" + filename, strModPath+ "\\"+str, null);
                   
                }
                
              

                #endregion
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "DownloadAndExtractZipFile", "ctlScriptdesigner.xaml.cs");
                return -1;
            }
            #endregion
            return 0;
        }
       
    }
}