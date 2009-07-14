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
using ScriptDesigner.Business;
using System.Data;
using System.Reflection;

namespace ScriptDesigner.Presentation
{
    /// <summary>
    /// Interaction logic for CtlQuestionAns.xaml
    /// </summary>
    /// 
    public enum ModulePermissions
    {
        Add = 0,
        Edit = 1,
        Delete = 2,
        View = 3
    }

    public partial class CtlQuestionAns : System.Windows.Controls.UserControl
    {
        int varState = 0;
        int prevScript = 0;
        VMukti.CtlGrid.Presentation.ctlGrid c;
        Canvas cnv;
        ctlScriptDesigner ctlDesigner;
        int PrevOption = 0;
        int varID = -1;
        ClsQuestionCollection objQColl;
        ClsAnswerCollection objAns;
        Label[] lblOption;
        TextBox[] txtOption;
        ComboBox[] cmbOption;
        ModulePermissions[] _MyPermissions;

        bool flag = true;

        public CtlQuestionAns(ModulePermissions[] MyPermissions)
        {
            InitializeComponent();
            _MyPermissions = MyPermissions;
            FncPermissionsReview();
            ClsScriptCollection objScriptCollection = ClsScriptCollection.GetAll();

            for (int i = 0; i < objScriptCollection.Count; i++)
            {
                ListBoxItem lbiItem = new ListBoxItem();
                lbiItem.Content = objScriptCollection[i].ScriptName;
                lbiItem.Tag = objScriptCollection[i].ID;
                cmbScript.Items.Add(lbiItem);
            }

            cnvMain.MouseMove += new MouseEventHandler(cnvMain_MouseMove);
            cmbScript.SelectionChanged += new SelectionChangedEventHandler(cmbScript_SelectionChanged);
            txtNoOfOptions.LostFocus += new RoutedEventHandler(txtNoOfOptions_LostFocus);
            btnSave.Click += new RoutedEventHandler(btnSave_Click);
            btnCancel.Click += new RoutedEventHandler(btnCancel_Click);
		    txtNoOfOptions.IsEnabled = false;
            flag = false;
            txtHeader.IsEnabled = false;
            txtDescription.IsEnabled = false;
            txtName.IsEnabled = false;
            cmbCategory.IsEnabled = false;
            btnDesigner.IsEnabled = false;

            c = new VMukti.CtlGrid.Presentation.ctlGrid();
            c.Height = 200;
            c.Width = 700;
            c.SetValue(Canvas.LeftProperty, 20.0);
            c.SetValue(Canvas.TopProperty, 70.0);

            c.Cols = 5;
            c.CanEdit = true;
            c.CanDelete = true;

            c.Columns[0].Header = "Question ID";
            c.Columns[1].Header = "Question Name";
            c.Columns[2].Header = "Description";
            c.Columns[3].Header = "QuestionText";
            c.Columns[4].Header = "Category";

            c.Columns[0].BindTo("ID");
            c.Columns[1].BindTo("Header");
            c.Columns[2].BindTo("Description");
            c.Columns[3].BindTo("Name");
            c.Columns[4].BindTo("Category");

            c.btnEditClicked += new VMukti.CtlGrid.Presentation.ctlGrid.ButtonClicked(c_btnEditClicked);
            c.btnDeleteClicked += new VMukti.CtlGrid.Presentation.ctlGrid.ButtonClicked(c_btnDeleteClicked);

            cnvMain.Children.Add(c);
        }

        void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            FncFillGrid();
            varState = 0;
            FncClearAll();
        }

        void cnvMain_MouseMove(object sender, MouseEventArgs e)
        {
            if (c != null)
            {
                if (c.Visibility == Visibility.Collapsed)
                {
                    c.Visibility = Visibility.Visible;
                }
            }

            if(cnv != null && cnv.Visibility==Visibility.Visible)
            {
                c.Visibility = Visibility.Collapsed;
            }

        }

        void FncPermissionsReview()
        {
            this.Visibility = Visibility.Visible;
            VMukti.CtlGrid.Presentation.ctlGrid ctlg = new VMukti.CtlGrid.Presentation.ctlGrid();
            for (int i = 0; i < _MyPermissions.Length; i++)
            {
                if (_MyPermissions[i] == ModulePermissions.Edit)
                {
                    ctlg.CanEdit = true;
                }
                if (_MyPermissions[i] == ModulePermissions.Delete)
                {
                    ctlg.CanDelete = true;
                }
                if (_MyPermissions[i] == ModulePermissions.View)
                {
                    ctlg.Visibility = Visibility.Visible;
                }
                if (_MyPermissions[i] == ModulePermissions.Add)
                {
                    this.Visibility = Visibility.Visible;
                }
            }

        }

        void btnSave_Click(object sender, RoutedEventArgs e)
        {
            if (txtHeader.Text.Trim() != "" && txtDescription.Text.Trim() != "" && txtName.Text.Trim() != "")
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
                    if (cmbOption[i].SelectedItem != null)
                    {
                        objAns.ActionQuestionID = int.Parse(((ListBoxItem)cmbOption[i].SelectedItem).Tag.ToString());
                        objAns.Description = "";
                        objAns.Save();
                    }
                    else
                    {
                        objAns.ActionQuestionID = Queueid;
                        MessageBox.Show("Fill Answer tab also");
                        break;
                    }
                    
                }
                FncFillGrid();
                if (objQColl.Count != 0)
                {
                    btnDesigner.IsEnabled = true;
                }
                FncClearAll();
                PrevOption = 0;
            }
            else
            {
                MessageBox.Show("Fill all Information");
            }
        }

        void txtNoOfOptions_LostFocus(object sender, RoutedEventArgs e)
        {
            if (txtNoOfOptions.Text.Trim() != "")
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
                        VMuktiAPI.VMuktiHelper.ExceptionHandler(exp, "txtNoOfOptions_LostFocus", "ctlQuestionAns.xaml.cs");
                    }
                }
            }
        }

        public void FncSetOptions()
        {
            PrevOption = int.Parse(txtNoOfOptions.Text.ToString());
            int NoOfOpt = int.Parse(txtNoOfOptions.Text.ToString());
            if (NoOfOpt > 75)
                MessageBox.Show("It Can't Be More Than 75");

            lblOption = new Label[NoOfOpt];
            txtOption = new TextBox[NoOfOpt];
            cmbOption = new ComboBox[NoOfOpt];

          
            if (NoOfOpt > 0)
            {
                Label lblNextQue = new Label();
                lblNextQue.Content = "Next Question for a particular Answer:";
                lblNextQue.SetValue(Canvas.TopProperty, (0.0));
                lblNextQue.SetValue(Canvas.LeftProperty, 420.0);
                cnvAnswer.Children.Add(lblNextQue);
            }
            for (int i = 0; i < NoOfOpt; i++)
            {
                lblOption[i] = new Label();
                lblOption[i].Content = "Option " + (i + 1).ToString();
                lblOption[i].SetValue(Canvas.TopProperty, (i * 25.0) + 25.0);
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
                        VMuktiAPI.VMuktiHelper.ExceptionHandler(exp, "FncSetOptions", "ctlQuestionAns.xaml.cs");
                    }
                }

                txtOption[i].SetValue(Canvas.TopProperty, (i * 25.0) + 25.0);
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
                            VMuktiAPI.VMuktiHelper.ExceptionHandler(exp, "FncSetOptions", "ctlQuestionAns.xaml.cs");
                        }
                    }
                    cmbOption[i].Items.Add(lstItem);
                }
                cmbOption[i].Width = 240;

                cmbOption[i].SetValue(Canvas.TopProperty, (i * 25.0) + 25.0);
                cmbOption[i].SetValue(Canvas.LeftProperty, 425.0);
                cnvAnswer.Children.Add(cmbOption[i]);
            }
            cnvAnswer.Height = 10.0 + (NoOfOpt * 25.0);
        }

        void cmbScript_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            FncFillGrid();
            ListBoxItem cbiScriptName = (ListBoxItem)cmbScript.SelectedItem;
            string script = cbiScriptName.Content.ToString();
            string scriptType = ClsScriptCollection.GetScriptType(script);
            clsStartClass.ScriptName = script;
            clsStartClass.ScriptType = scriptType;

            if (scriptType.Equals("Web Script"))
            {
                btnDesigner.Visibility = Visibility.Collapsed;
                btnDesigner.IsEnabled = false;
            }
            else
            {
                if (objQColl.Count == 0)
                {
                    MessageBox.Show("Please Fill Question Answers with Script");
                    btnDesigner.IsEnabled = false;
                }
                else
                {
                    btnDesigner.Visibility = Visibility.Visible;
                    btnDesigner.IsEnabled = true;
                }
            }

            cmbCategory.IsEnabled = true;
            if (scriptType.Equals("Static"))
            {
                tbiAnswers.Visibility = Visibility.Collapsed;
                cmbCategory.SelectedIndex = 0;
                cmbCategory.Visibility = Visibility.Collapsed;
                txtNoOfOptions.Text = "0";
                txtNoOfOptions.Visibility = Visibility.Collapsed;
                lblCategory.Visibility = Visibility.Collapsed;
                lblNoOfOption.Visibility = Visibility.Collapsed;
            }
            else
            {
                tbiAnswers.Visibility = Visibility.Visible;
                cmbCategory.Visibility = Visibility.Visible;
                txtNoOfOptions.Visibility = Visibility.Visible;
                lblCategory.Visibility = Visibility.Visible;
                lblNoOfOption.Visibility = Visibility.Visible;
            }


            txtHeader.IsEnabled = true;
            txtDescription.IsEnabled = true;
            txtName.IsEnabled = true;
        }

        public void FncFillGrid()
        {
            //edited
            objQColl = ClsQuestionCollection.GetAll(int.Parse(((ListBoxItem)(cmbScript.SelectedItem)).Tag.ToString()));
            c.Bind(objQColl);
        }

        void c_btnDeleteClicked(int RowID)
        {
            varID = Convert.ToInt32(objQColl[RowID].ID);
            if (System.Windows.MessageBox.Show("Do You Really Want To Delete This Record ?", "Delete Question", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                ClsQuestion.Delete(varID);
                System.Windows.MessageBox.Show("Record Deleted!!", "Question Delete", MessageBoxButton.OK, MessageBoxImage.Information);
                FncFillGrid();
                if (objQColl.Count == 0)
                {
                    btnDesigner.IsEnabled = false;
                }
            }
        }

        void c_btnEditClicked(int ID)
        {
            varState = 1;
            varID = objQColl[ID].ID;
       //     MessageBox.Show("Question ID is - " + objQColl[ID].ID.ToString());
            cnvAnswer.Children.Clear();
            ClsQuestion objQue = ClsQuestion.GetByQueID(objQColl[ID].ID);
            txtHeader.Text = objQue.Header;
            txtName.Text = objQue.Name;
            txtDescription.Text = objQue.Description;
            cmbCategory.Text = objQue.Category;

            objAns = ClsAnswerCollection.GetAll(objQColl[ID].ID);

            txtNoOfOptions.Text = objAns.Count.ToString();

            FncSetOptions();

        }

        public void FncClearAll()
        {
            txtName.Text = "";
            txtHeader.Text = "";
            txtDescription.Text = "";
            txtNoOfOptions.Text = "";
            cmbCategory.SelectedIndex = 0;
            cnvAnswer.Children.Clear();
            tbiQuestions.IsSelected = true;
            varState = 0;
            varID = -1;
        }

        internal static DataTable ObjectArrayToDataTable(ClsQuestionCollection obj, Type type)
        {
            return ObjectArrayToDataTable(obj, type, null);
        }

        internal static DataTable ObjectArrayToDataTable(ClsQuestionCollection obj, Type type, DataColumn[] extra)
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

        private void btnDesigner_Click(object sender, RoutedEventArgs e)
        {
            if (cmbScript.Items.Count > 0)
            {
                if (cmbScript.SelectedItem != null)
                {
                    if (cnv == null)
                    {
                        cnv = new Canvas();
                        cnv.Height = 768;
                        cnv.Width = 1024;
                        cnv.SetValue(Canvas.LeftProperty, 0.0);
                        cnv.SetValue(Canvas.TopProperty, 0.0);

                        //ctlDesigner = new ctlScriptDesigner(((ListBoxItem)(cmbScript.SelectedItem)).Content.ToString());
                        ctlDesigner = new ctlScriptDesigner(((ListBoxItem)(cmbScript.SelectedItem)).Tag.ToString());

                        ((ctlScriptDesigner)ctlDesigner).btnBack.Focus();
                        ctlDesigner.SctiptID = int.Parse(((ListBoxItem)(cmbScript.SelectedItem)).Tag.ToString());
                     //   ctlDesigner.ScriptName = ((ListBoxItem)(cmbScript.SelectedItem)).Content.ToString();
                        prevScript = ctlDesigner.SctiptID;
                        ctlDesigner.GetQuestions();
                        cnv.Children.Add(ctlDesigner);
                        cnvMain.Children.Add(cnv);

                        if (c != null)
                            c.Visibility = Visibility.Collapsed;
                    }
                    else
                    {
                        cnv.Children.Clear();
                        cnv = new Canvas();
                        cnv.Height = 768;
                        cnv.Width = 1024;
                        cnv.SetValue(Canvas.LeftProperty, 0.0);
                        cnv.SetValue(Canvas.TopProperty, 0.0);

                        //ctlDesigner = new ctlScriptDesigner(((ListBoxItem)(cmbScript.SelectedItem)).Content.ToString());
                        ctlDesigner = new ctlScriptDesigner(((ListBoxItem)(cmbScript.SelectedItem)).Tag.ToString());

                        ((ctlScriptDesigner)ctlDesigner).btnBack.Focus();
                        ctlDesigner.SctiptID = int.Parse(((ListBoxItem)(cmbScript.SelectedItem)).Tag.ToString());
                       // ctlDesigner.ScriptName = ((ListBoxItem)(cmbScript.SelectedItem)).Content.ToString();
                            prevScript = ctlDesigner.SctiptID;
                            ctlDesigner.GetQuestions();
                            cnv.Children.Add(ctlDesigner);
                        cnvMain.Children.Add(cnv);

                        if (c != null)
                            c.Visibility = Visibility.Collapsed;
                        //cnv.Visibility = Visibility.Visible;

                        //if (c != null)
                        //    c.Visibility = Visibility.Collapsed;


                        //if (prevScript != int.Parse(((ListBoxItem)(cmbScript.SelectedItem)).Tag.ToString()))
                        //{
                        //    ctlDesigner = new ctlScriptDesigner();
                        //    ctlDesigner.SctiptID = int.Parse(((ListBoxItem)(cmbScript.SelectedItem)).Tag.ToString());
                        //    ctlDesigner.ScriptName = ((ListBoxItem)(cmbScript.SelectedItem)).Content.ToString();
                        //    prevScript = ctlDesigner.SctiptID;
                        //    ctlDesigner.GetQuestions();
                        //    cnv.Children.Add(ctlDesigner);
                        //}
                    }
                }
                else
                {
                    MessageBox.Show("Please Select The Script First", "-> Designer", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
                //v
            else
            {
                MessageBox.Show("currently there are no scripts ");
            }
            //NavigationService.GetNavigationService(this).Navigate(new Uri("pgDesigner.xaml",UriKind.RelativeOrAbsolute));
        }

        private void cmbCategory_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (flag == false)
            {
                if (cmbCategory.SelectedIndex == 0)   // if "None"
                {
                    txtNoOfOptions.IsEnabled = false;
                }
                else
                    txtNoOfOptions.IsEnabled = true;
            }
        }
   

    }
}
