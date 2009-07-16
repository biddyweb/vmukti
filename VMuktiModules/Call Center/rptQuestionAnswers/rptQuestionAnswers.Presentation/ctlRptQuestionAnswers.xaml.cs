using System;
using System.Collections.Generic;
using System.Linq;
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
using System.Data;
using Microsoft.Reporting.WinForms;
using rptQuestionAnswers.Business;
using System.Reflection;

namespace rptQuestionAnswers.Presentation
{
    public enum ModulePermissions
    {
        Add = 0,
        Edit = 1,
        Delete = 2,
        View = 3
    }

    public partial class ctlRptQuestionAnswers : UserControl
    {
        DataSet dsReport = new DSQnA();
        ReportDataSource rds1 = new ReportDataSource();
        //ReportDataSource rds2 = new ReportDataSource();

        public ctlRptQuestionAnswers(ModulePermissions[] MyPermissions)
        {
            InitializeComponent();

            DataSet dsScript = ClsRptQuestionAnswers.GetAllScript();

            foreach (DataRow dr in dsScript.Tables[0].Rows)
            {
                ComboBoxItem cbi = new ComboBoxItem();
                cbi.Content = dr["ScriptName"];
                cbi.Tag = dr["ID"];
                cmbScript.Items.Add(cbi);
            }
            this.Loaded += new RoutedEventHandler(ctlRptQuestionAnswers_Loaded);
        }

        private void btnGo_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ComboBoxItem cbi = (ComboBoxItem)cmbScript.SelectedItem;
                int scriptID = int.Parse(cbi.Tag.ToString());

                dsReport = Business.ClsRptQuestionAnswers.GetAllQuestionAnswers(scriptID);

                if (dsReport.Tables.Count > 0)
                {
                    dsReport.Tables[0].TableName = "DSQnA_Question";
                    //dsReport.Tables[1].TableName = "DSQnA_QuestionOptions";
                }

                rds1.Name = "DSQnA_Question";
                rds1.Value = dsReport.Tables["DSQnA_Question"];

                //rds2.Name = "DSQnA_QuestionOptions";
                //rds2.Value = dsReport.Tables["DSQnA_QuestionOptions"];
                
                objReportViewer.LocalReport.ReportPath = Assembly.GetAssembly(this.GetType()).Location.Replace("rptQuestionAnswers.Presentation.dll", "rptScriptQueAns.rdlc");
                objReportViewer.LocalReport.ReportEmbeddedResource = Assembly.GetAssembly(this.GetType()).Location.Replace("rptQuestionAnswers.Presentation.dll", "rptScriptQueAns.rdlc");
                objReportViewer.LocalReport.DataSources.Add(rds1);
                //objReportViewer.LocalReport.DataSources.Add(rds2);
                objReportViewer.RefreshReport();
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "btnGo_Click()", "ctlRptQuestionAnswers.xaml.cs");
            }
        }


        #region changes for reportviewer resizing

        void ctlRptQuestionAnswers_Loaded(object sender, RoutedEventArgs e)
        {
            if (this.Parent != null)
            {
                ((Grid)this.Parent).SizeChanged += new SizeChangedEventHandler(ctlRptQuestionAnswers_SizeChanged);

                wfhRptViewer.Width = ((Grid)this.Parent).ActualWidth - 4.0;
            }
        }

        void ctlRptQuestionAnswers_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            try
            {
                if (e.NewSize.Width > 0)
                {
                    wfhRptViewer.Width = e.NewSize.Width - 4.0;
                }
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "ctlRptQuestionAnswers_SizeChanged()", "ctlRptQuestionAnswers.xaml.cs");
            }
        }

        #endregion
    }
}
