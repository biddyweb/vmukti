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
using System.Data;
using System.Reflection;

namespace CRMDesigner.Presentation
{
    /// <summary>
    /// Interaction logic for CtlQuestionAns.xaml
    /// </summary>
    /// 
    //public enum ModulePermissions
    //{
    //    Add = 0,
    //    Edit = 1,
    //    Delete = 2,
    //    View = 3
    //}
    public partial class CtlQuestionAns : System.Windows.Controls.UserControl
    {
        //public static StringBuilder sb1;
        int varState = 0;
        int PrevOption=0;
        int varID = -1;
        ClsQuestionCollection objQColl;
        ClsAnswerCollection objAns;
        
        ctlCRMDesigner ctlDesigner;
        Canvas cnv;
        TabControl tbc;
        VMukti.CtlGrid.Presentation.ctlGrid c;
        
        Label[] lblOption;
        TextBox[] txtOption;
        ComboBox[] cmbOption;
        //ModulePermissions[] _MyPermissions;


        public CtlQuestionAns(ModulePermissions[] MyPermissions)
        {
            try
            {
                InitializeComponent();
                //this.Loaded += new RoutedEventHandler(CtlQuestionAns_Loaded);
                //_MyPermissions = MyPermissions;
                //FncPermissionsReview();
                //currentControl = new ctlPOD();
                //MyPropGrid = new ctlPropertyGrid();

                cnvMain.MouseMove += new MouseEventHandler(cnvMain_MouseMove);
                cmbScript.SelectionChanged += new SelectionChangedEventHandler(cmbScript_SelectionChanged);
                txtNoOfOptions.LostFocus += new RoutedEventHandler(txtNoOfOptions_LostFocus);
                btnSave.Click += new RoutedEventHandler(btnSave_Click);
                //btnCancel.Click += new RoutedEventHandler(btnCancel_Click);
                txtNoOfOptions.IsEnabled = false;
                //flag = false;
                txtHeader.IsEnabled = false;
                txtDescription.IsEnabled = false;
                txtName.IsEnabled = false;
                cmbCategory.IsEnabled = false;
                //btnDesigner.IsEnabled = false;

                
                cmbScript.SelectionChanged+=new SelectionChangedEventHandler(cmbScript_SelectionChanged);
                txtNoOfOptions.LostFocus+=new RoutedEventHandler(txtNoOfOptions_LostFocus);
                //btnSave_Click += new RoutedEventHandler(btnSave_Click);

                ClsCRMCollection objClsCRMCollection = ClsCRMCollection.GetAll();
                for (int i = 0; i < objClsCRMCollection.Count; i++)
                {
                    ComboBoxItem cmb1 = new ComboBoxItem();
                    cmb1.Content = objClsCRMCollection[i].CRMName;
                    cmb1.Tag = objClsCRMCollection[i].ID;
                    cmbScript.Items.Add(cmb1);
                }
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "CtlQuestionAns()", "CtlQuestionAns.xaml.cs");
            }
        }

        void cnvMain_MouseMove(object sender, MouseEventArgs e)
        {
            //throw new NotImplementedException();
        }

        //void FncPermissionsReview()
        //{
        //    try
        //    {
        //        this.Visibility = Visibility.Visible;
        //        VMukti.CtlGrid.Presentation.ctlGrid ctlg = new VMukti.CtlGrid.Presentation.ctlGrid();
                
        //        for (int i = 0; i < _MyPermissions.Length; i++)
        //        {
        //            if (_MyPermissions[i] == ModulePermissions.Edit)
        //            {
        //                ctlg.CanEdit = true;
        //            }
        //            if (_MyPermissions[i] == ModulePermissions.Delete)
        //            {
        //                ctlg.CanDelete = true;
        //            }
        //            if (_MyPermissions[i] == ModulePermissions.View)
        //            {
        //                ctlg.Visibility = Visibility.Visible;
        //            }
        //            if (_MyPermissions[i] == ModulePermissions.Add)
        //            {
        //                this.Visibility = Visibility.Visible;
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show(ex.ToString());
        //    }
        //}

        void CtlQuestionAns_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                this.Width = ((Grid)this.Parent).ActualWidth;
                ((Grid)this.Parent).SizeChanged += new SizeChangedEventHandler(CtlQuestionAns_SizeChanged);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        void CtlQuestionAns_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            this.Width = e.NewSize.Width;
        }
        //public static StringBuilder CreateTressInfo()
        //{
        //    StringBuilder sb = new StringBuilder();
        //    sb.Append("User Is : " + VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName);
        //    sb.AppendLine();
        //    sb.Append("Peer Type is : " + VMuktiAPI.VMuktiInfo.CurrentPeer.CurrPeerType.ToString());
        //    sb.AppendLine();
        //    sb.Append("User's SuperNode is : " + VMuktiAPI.VMuktiInfo.CurrentPeer.SuperNodeIP);
        //    sb.AppendLine();
        //    sb.Append("User's Machine Ip Address : " + VMuktiAPI.GetIPAddress.ClsGetIP4Address.GetIP4Address());
        //    sb.AppendLine();
        //    sb.AppendLine("----------------------------------------------------------------------------------------");
        //    return sb;
        //}

        public CtlQuestionAns()
        {
            try
            {
            InitializeComponent();

            ClsScriptCollection objScriptCollection = ClsScriptCollection.GetAll();
            for (int i = 0; i < objScriptCollection.Count; i++)
            {
                ListBoxItem lbiItem = new ListBoxItem();
                lbiItem.Content = objScriptCollection[i].ScriptName;
                lbiItem.Tag = objScriptCollection[i].ID;
                cmbScript.Items.Add(lbiItem);
            }
            
            cmbScript.SelectionChanged +=new SelectionChangedEventHandler(cmbScript_SelectionChanged);
            txtNoOfOptions.LostFocus += new RoutedEventHandler(txtNoOfOptions_LostFocus);
            btnSave.Click += new RoutedEventHandler(btnSave_Click);
        }
            catch (Exception exp)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(exp, "CtlQuestionAns()", "CtlQuestionAns.xaml.cs");
            }
        }
        void btnSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
            ClsQuestion objQue = new ClsQuestion();
            if (varState == 0)
                objQue.ID = -1;
            else
            objQue.ID = varID;
            objQue.Category = cmbCategory.Text;
            objQue.Description = txtDescription.Text;
            objQue.Header = txtHeader.Text;
            objQue.Name = txtName.Text;
            objQue.ScriptID = int.Parse(((ListBoxItem)cmbScript.SelectedItem).Tag.ToString());
            int Queueid = objQue.Save();

            ClsAnswer.Delete(Queueid);

            int count = 0;

            if (txtNoOfOptions.Text.ToString() != "")
            {
                count = int.Parse(txtNoOfOptions.Text.ToString());
            }

            for (int i = 0; i < count; i++)
            {
                ClsAnswer objAns = new ClsAnswer();
                objAns.ID = -1;
                objAns.QuestionID = Queueid;
                objAns.OptionName = txtOption[i].Text.ToString();
                if(cmbOption[i].SelectedItem != null)
                objAns.ActionQuestionID = int.Parse(((ListBoxItem)cmbOption[i].SelectedItem).Tag.ToString());
                else
                    objAns.ActionQuestionID = Queueid;
                objAns.Description = "";
                objAns.Save();
            }

            FncFillGrid();
            FncClearAll();

            PrevOption = 0;
        }
            catch (Exception exp)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(exp, "btnSave_Click()", "CtlQuestionAns.xaml.cs");
            }
        }
        void txtNoOfOptions_LostFocus(object sender, RoutedEventArgs e)
        {
            try
            {
            if(txtNoOfOptions.Text.Trim() != "")
            {
                if (PrevOption != int.Parse(txtNoOfOptions.Text.ToString()))
                {
                    cnvAnswer.Children.Clear();
                    try
                    {
                        if (txtNoOfOptions.Text != "")
                        {
                            FncSetOptions();
                        }
                    }
                    catch (Exception exp)
                    {
                        MessageBox.Show(exp.Message);
                    }
                }
            }
        }
            catch (Exception exp)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(exp, "txtNoOfOptions_LostFocus()", "CtlQuestionAns.xaml.cs");
            }
        }
        public void FncSetOptions()
        {
            try
            {
            PrevOption = int.Parse(txtNoOfOptions.Text.ToString());
            int NoOfOpt = int.Parse(txtNoOfOptions.Text.ToString());
            if (NoOfOpt > 75)
                MessageBox.Show("It Can't Be More Than 75");

            lblOption = new Label[NoOfOpt];
            txtOption = new TextBox[NoOfOpt];
            cmbOption = new ComboBox[NoOfOpt];

            for (int i = 0; i < NoOfOpt; i++)
            {
                lblOption[i] = new Label();
                lblOption[i].Content = "Option " + (i + 1).ToString();
                lblOption[i].SetValue(Canvas.TopProperty, (i * 25.0) + 10.0);
                lblOption[i].SetValue(Canvas.LeftProperty, 5.0);
                cnvAnswer.Children.Add(lblOption[i]);

                txtOption[i] = new TextBox();
                txtOption[i].Width = 360;

                // There is a Known Exception So We Are Ignoring It...

                // The Exception Is When Edit eg We are having 4 options then those Lables,Text,Combo would be
                // generated automatically and now if i want to add 2 more options it
                // throws Exception Because My Answer Collection Object Is having Only
                // 4 Options, so can't find option text for additional textboxes.

                if (varState == 1)
                {
                    try
                    {
                        txtOption[i].Text = objAns[i].OptionName;
                    }
                    catch (Exception exp)
                    {
                    }
                }

                txtOption[i].SetValue(Canvas.TopProperty, (i * 25.0) + 10.0);
                txtOption[i].SetValue(Canvas.LeftProperty, 65.0);
                cnvAnswer.Children.Add(txtOption[i]);

                cmbOption[i] = new ComboBox();
                for (int j = 0; j < objQColl.Count; j++)
                {
                    ListBoxItem lstItem = new ListBoxItem();
                    lstItem.Tag = objQColl[j].ID;
                    lstItem.Content = objQColl[j].Name;
                    if (varState == 1)
                    {
                        try
                        {
                            if (objAns[i].ActionQuestionID == objQColl[j].ID)
                                cmbOption[i].SelectedItem = lstItem;
                        }
                        catch (Exception exp)
                        {
                        }
                    }
                    cmbOption[i].Items.Add(lstItem);
                }
                cmbOption[i].Width = 240;
                
                cmbOption[i].SetValue(Canvas.TopProperty, (i * 25.0) + 10.0);
                cmbOption[i].SetValue(Canvas.LeftProperty, 425.0);
                cnvAnswer.Children.Add(cmbOption[i]);
            }
            cnvAnswer.Height = 10.0 + (NoOfOpt * 25.0);
        }
            catch (Exception exp)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(exp, "FncSetOptions()", "CtlQuestionAns.xaml.cs");
            }
        }
        void cmbScript_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
            FncFillGrid();
            }
            catch (Exception exp)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(exp, "cmbScript_SelectionChanged()", "CtlQuestionAns.xaml.cs");
            }
        }
        public void FncFillGrid()
        {
            try
            {
            objQColl = ClsQuestionCollection.GetAll(int.Parse(((ListBoxItem)(cmbScript.SelectedItem)).Tag.ToString()));
            DataTable dt = ObjectArrayToDataTable(objQColl, typeof(ClsQuestion));
            //MessageBox.Show(objQColl.Count.ToString());

            ctlGrid c = new ctlGrid(dt, true, true);
            c.Height = 200;
            c.Width = 700;
            c.SetValue(Canvas.LeftProperty, 20.0);
            c.SetValue(Canvas.TopProperty, 70.0);

            c.btnEditClicked += new ctlGrid.ButtonClicked(c_btnEditClicked);
            c.btnDeleteClicked += new ctlGrid.ButtonClicked(c_btnDeleteClicked);

            cnvMain.Children.Add(c);

            }
            catch (Exception exp)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(exp, "FncFillGrid()", "CtlQuestionAns.xaml.cs");
            }
        }
        void c_btnDeleteClicked(int RowID)
        {
            try
            {
            varID = Convert.ToInt32(objQColl[RowID].ID);
            if (System.Windows.MessageBox.Show("Do You Really Want To Delete This Record ?", "->Delete Question", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                ClsQuestion.Delete(varID);
                System.Windows.MessageBox.Show("Record Deleted!!", "->Question Delete", MessageBoxButton.OK, MessageBoxImage.Information);
                FncFillGrid();
            }
            }
            catch (Exception exp)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(exp, "c_btnDeleteClicked()", "CtlQuestionAns.xaml.cs");
            }
        }
        void c_btnEditClicked(int ID)
        {
            try
            {
            varState = 1;
            varID = objQColl[ID].ID;
            MessageBox.Show("Question ID is - " + objQColl[ID].ID.ToString());

            ClsQuestion objQue =  ClsQuestion.GetByQueID(objQColl[ID].ID);
            txtHeader.Text = objQue.Header;
            txtName.Text = objQue.Name;
            txtDescription.Text = objQue.Description;
            cmbCategory.Text = objQue.Category;

            objAns = ClsAnswerCollection.GetAll(objQColl[ID].ID);

            txtNoOfOptions.Text = objAns.Count.ToString();

            FncSetOptions();

            }
            catch (Exception exp)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(exp, "c_btnEditClicked()", "CtlQuestionAns.xaml.cs");
            }
        }
        public void FncClearAll()
        {
            try
            {
            txtName.Text = "";
            txtHeader.Text = "";
            txtDescription.Text = "";
            txtNoOfOptions.Text = "";
            cnvAnswer.Children.Clear();
            tbiQuestions.IsSelected = true;
            varState = 0;
            varID = -1;
            }
            catch (Exception exp)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(exp, "FncClearAll()", "CtlQuestionAns.xaml.cs");
            }
        }
        internal static DataTable ObjectArrayToDataTable(ClsQuestionCollection obj, Type type)
        {
            try
            {
            return ObjectArrayToDataTable(obj, type, null);
            }
            catch (Exception exp)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(exp, "ObjectArrayToDataTable()", "CtlQuestionAns.xaml.cs");
                return null;
            }
        }
        internal static DataTable ObjectArrayToDataTable(ClsQuestionCollection obj, Type type, DataColumn[] extra)
        {
            try
            {
            DataTable dt = new DataTable();

            foreach (PropertyInfo pi in type.GetProperties())
            {
                if (pi.PropertyType.IsPrimitive || pi.PropertyType == typeof(string) || pi.PropertyType == typeof(DateTime))
                {
                    dt.Columns.Add(pi.Name, pi.PropertyType);
                }
            }

            if (extra != null)
            {
                foreach (DataColumn c in extra)
                {
                    if (dt.Columns.Contains(c.ColumnName))
                        dt.Columns.Remove(c.ColumnName);
                    dt.Columns.Add(c);
                }
            }

            foreach (object k in obj)
            {
                DataRow dr = dt.NewRow();
                foreach (PropertyInfo pi in type.GetProperties())
                {
                    if (pi.PropertyType.IsPrimitive || pi.PropertyType == typeof(string) || pi.PropertyType == typeof(DateTime))
                    {
                        dr[pi.Name] = pi.GetValue(k, null);
                    }
                }
                dt.Rows.Add(dr);
            }

            return dt;
        }
            catch (Exception exp)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(exp, "ObjectArrayToDataTable()", "CtlQuestionAns.xaml.cs");
                return null;
            }
        }
        private void btnDesigner_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (cmbScript.Items.Count > 0)
                {
                    if (cmbScript.SelectedItem != null)
                    {
                        if (cnv == null)
                        {
                            cnv = new Canvas();

                            tbc = new TabControl();
                            
                            tbc.Height = 768;
                            tbc.Width = 1024;
                            tbc.SetValue(Canvas.LeftProperty, 0.0);
                            tbc.SetValue(Canvas.TopProperty, 0.0);

                            cnv.Height = 768;
                            cnv.Width = 1024;
                            cnv.SetValue(Canvas.LeftProperty, 0.0);
                            cnv.SetValue(Canvas.TopProperty, 0.0);
                            //ctlDesigner = new ctlCRMDesigner(((ListBoxItem)(cmbScript.SelectedItem)).Content.ToString());
                            //ctlDesigner = new ctlCRMDesigner.FunCRMDesigner(((ListBoxItem)(cmbScript.SelectedItem)).Content.ToString());
                            ctlDesigner = new ctlCRMDesigner(((ListBoxItem)(cmbScript.SelectedItem)).Content.ToString());

                            ctlDesigner.SctiptID = int.Parse(((ListBoxItem)(cmbScript.SelectedItem)).Tag.ToString());


                            ctlDesigner.GetQuestions();


                            cnv.Children.Add(ctlDesigner);

                            cnvMain.Children.Add(cnv);
                            
                            if (c != null)
                            {
                                c.Visibility = Visibility.Collapsed;
                            }
                        }
                        else
                        {
                            cnv.Children.Clear();
                            cnv = new Canvas();
                            cnv.Height = 768;
                            cnv.Width = 1024;
                            cnv.SetValue(Canvas.LeftProperty, 0.0);
                            cnv.SetValue(Canvas.TopProperty, 0.0);
                            ctlDesigner = new ctlCRMDesigner(((ListBoxItem)(cmbScript.SelectedItem)).Content.ToString());
                            ctlDesigner.GetQuestions();
                            cnv.Children.Add(ctlDesigner);
                            cnvMain.Children.Add(cnv);

                            if (c != null)
                            {
                                c.Visibility = Visibility.Collapsed;
                            }
                        }
                    }
                    else
                    {
                        MessageBox.Show("Please Select the CRM first", "-> Designer", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }
                else
                {
                    MessageBox.Show("Currently there are no CRMs");
                }
                //NavigationService.GetNavigationService(this).Navigate(new Uri("pgDesigner.xaml", UriKind.RelativeOrAbsolute));
            }
            catch (Exception exp)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(exp, "btnDesigner_Click()", "CtlQuestionAns.xaml.cs");
            }
        }
    }
}
