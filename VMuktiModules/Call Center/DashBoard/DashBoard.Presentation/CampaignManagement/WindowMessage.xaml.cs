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


namespace DashBoard.Presentation.CampaignManagement
{
    /// <summary>
    /// Interaction logic for WindowMessage.xaml
    /// </summary>
    public partial class WindowMessage : UserControl
    {
        bool flag = false;
        public string _strCamapign = string.Empty;

         

        public WindowMessage()
        {
            InitializeComponent();

            this.AllowDrop = true;
            this.MouseLeave += new MouseEventHandler(WindowMessage_MouseLeave);
            this.DragOver += new DragEventHandler(WindowMessage_DragOver);
            this.Drop += new DragEventHandler(WindowMessage_Drop);
        }        
        void WindowMessage_DragOver(object sender, DragEventArgs e)
        {
            try
            {
                e.Effects = DragDropEffects.All;
            }
            catch
            {
            }          
        }
        void WindowMessage_Drop(object sender, DragEventArgs e)
        {
            try
            {
                string str1 = (string)e.Data.GetData(DataFormats.StringFormat);
                string pFix = str1.Substring(str1.Length - 1, 1);
                string Actual = str1.Substring(0, str1.Length - 1);
                string strCamapign = txtCampaign.Text;
                Container objCon = new Container();
                if (pFix == "u")
                {
                    objCon.lblName.Content = Actual;
                    triUsers.Items.Add(objCon);
                    DashBoard.Business.CampaignManagement.ClsAssignTreatment.InsertCampaignUser(strCamapign, Actual);
                }
                else
                {
                    DataSet ds = DashBoard.Business.CampaignManagement.ClsAssignTreatment.GetCampaignTreatment(strCamapign, Actual);
                    if (ds.Tables[0].Rows.Count == 0)
                    {
                        objCon.lblName.Content = Actual;
                        triTreatment.Items.Add(objCon);
                        DashBoard.Business.CampaignManagement.ClsAssignTreatment.InsertCampaignTreatment(strCamapign, Actual);
                    }
                }
            }
            catch
            {
            }
        }

        public void WindowMessage_MouseLeave(object sender, MouseEventArgs e)
        {
            try
            {
                if (flag == false)
                {
                this.Visibility = Visibility.Hidden;
            }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void btnPin_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (flag == false)
                {
                    btnPin.Background = Brushes.Red;
                    flag = true;
                  
                }
                else
                {
                    btnPin.Background = Brushes.Green;
                    flag = false;
                }
            }
            catch
            {
            }
        
        }

    }
}
