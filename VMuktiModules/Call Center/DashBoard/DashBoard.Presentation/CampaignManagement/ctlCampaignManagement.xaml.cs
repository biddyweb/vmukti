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
    /// Interaction logic for ctlCampaignManagement.xaml
    /// </summary>
    public partial class ctlCampaignManagement : UserControl
    {
        WindowMessage winMessage = new WindowMessage();
        Thickness abc = new Thickness();
        string cur_Campaign;

        //Button btnCampaign = new Button();

        public ctlCampaignManagement()
        {
            try
            {
                InitializeComponent();

                fncGetTreatment();
                fncCampaignList();
                fncGetUserList();
                
                grdCampaign.Children.Add(winMessage);                
                winMessage.Visibility = Visibility.Hidden;

                this.lstTreatment.SelectionChanged += new SelectionChangedEventHandler(lstTreatment_SelectionChanged);
                this.lstCampaign.SelectionChanged += new SelectionChangedEventHandler(lstCampaign_SelectionChanged);
                this.lstCampaign.AllowDrop = true;
                this.lstUsers.SelectionChanged += new SelectionChangedEventHandler(lstUsers_SelectionChanged);
                
                lstCampaign.DragOver += new DragEventHandler(lstCampaign_DragOver);                        


            }
            catch (Exception ex)
            {
            }

        }

        void lstCampaign_DragOver(object sender, DragEventArgs e)
        {
            try
            {
                e.Effects = DragDropEffects.All;
            }
            catch
            {
            }
        }
        
        void l_Drop(object sender, DragEventArgs e)
        {
            try
            {
                string str1 = (string)e.Data.GetData(DataFormats.StringFormat);
                string pFix = str1.Substring(str1.Length - 1, 1);
                string Actual = str1.Substring(0, str1.Length - 1);
                string strCamapign = ((ListBoxItem)(sender)).Content.ToString();
                Container objCon = new Container();
                if (pFix == "u")
                {
                    objCon.lblName.Content = Actual;
                    winMessage.triUsers.Items.Add(objCon);

                    //DashBoard.Business.CampaignManagement.ClsCampaignManagement.InsertCampaignUser(strCamapign, Actual);
                    DashBoard.Business.CampaignManagement.ClsCampaignManagement.AddCampaignUser(strCamapign, Actual);
                }
                else
                {
                    DataSet ds = DashBoard.Business.CampaignManagement.ClsCampaignManagement.GetCampaignTreatment(strCamapign, Actual);
                    if (ds.Tables[0].Rows.Count == 0)
                    {
                        objCon.lblName.Content = Actual;
                        winMessage.triTreatment.Items.Add(objCon);

                        DashBoard.Business.CampaignManagement.ClsCampaignManagement.InsertCampaignTreatment(strCamapign, Actual);                        
                    }
                }

            }
            catch
            {
            }
        }
      

        //void lstShow_DragOver(object sender, DragEventArgs e)
        //{
        //    try
        //    {
        //        e.Effects = DragDropEffects.All;
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show(ex.Message);
        //    }
        //}


        void l_MouseEnter(object sender, MouseEventArgs e)
        {
            try
            {
                cur_Campaign = ((ListBoxItem)sender).Content.ToString();

                fncGetUsers(((ListBoxItem)sender).Content.ToString());
                fncGetTreatment(((ListBoxItem)sender).Content.ToString());
                if (winMessage.Visibility == Visibility.Hidden)
                {
                    winMessage.Margin = abc;
                    winMessage.txtCampaign.Text = ((ListBoxItem)sender).Content.ToString();
                    winMessage.HorizontalAlignment = HorizontalAlignment.Left;
                    winMessage.VerticalAlignment = VerticalAlignment.Top;
                    winMessage.Visibility = Visibility.Visible;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());

            }
        }
        void lstUser_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                if (lstUsers.Items.Count == 0)
                    return;

                string s1 = ((ListBoxItem)sender).Content.ToString().Trim() + "u";
                if (s1 != "" && s1 != null)
                {
                    DragDropEffects drgUser = DragDrop.DoDragDrop(lstUsers, s1, DragDropEffects.All);
                }

            }

            catch
            {
            }
        }
        void lstUsers_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (lstUsers.Items.Count == 0)
                    return;
                if (lstUsers.SelectedItems.Count > 0)
                {
                    string s1 = ((ListBoxItem)lstUsers.SelectedItem).Content.ToString().Trim() + "u";
                    if (s1 != "" && s1 != null)
                    {
                        DragDropEffects drgUser = DragDrop.DoDragDrop(lstUsers, s1, DragDropEffects.All);
                    }
                }

            }

            catch
            {
            }
        }

        void lstCampaign_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (lstCampaign.Items.Count == 0)
                {
                    return;
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        void lstTreatment_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (lstTreatment.Items.Count == 0)
                    return;
                if (lstTreatment.SelectedItems.Count > 0)
                {
                    string s1 = ((ListBoxItem)lstTreatment.SelectedItem).Content.ToString() + "t";

                    if (s1.Trim() != null)
                    {
                        DragDropEffects ddel = DragDrop.DoDragDrop(lstTreatment, s1, DragDropEffects.All);

                    }
                    else
                    {
                        MessageBox.Show("Select Treatment");
                    }
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("lstTreatment_SelectionChanged  " + ex.Message);
            }
        }
        void l_PreviewMouseDown(object sender, EventArgs e)
        {
            try
            {
                if (lstTreatment.Items.Count == 0)
                    return;

                string s1 = ((ListBoxItem)sender).Content.ToString() + "t";
                if (s1.Trim() != null)
                {
                    DragDropEffects ddel = DragDrop.DoDragDrop(lstTreatment, s1, DragDropEffects.All);
                }
            }
            catch
            {
            }
        }


        public void fncCampaignList()
        {
            try
            {
                Int64 UserID = 2;
                DataSet ds = DashBoard.Business.CampaignManagement.ClsCampaignManagement.GetCampainNames(UserID);

                DataTable dt = ds.Tables[0];
                abc = lstCampaign.Margin;

                lstCampaign.Items.Clear();

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ListBoxItem l = new ListBoxItem();
                    l.Drop += new DragEventHandler(l_Drop);
                    l.MouseEnter += new MouseEventHandler(l_MouseEnter);
                    l.Content = dt.Rows[i].ItemArray[0];
                    l.Tag = "Campaign";
                    lstCampaign.Items.Add(l);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }        
        
        public void fncGetTreatment(string CampaignName)
        {
            try
            {
                DataSet ds = DashBoard.Business.CampaignManagement.ClsCampaignManagement.GetTreatment1(CampaignName);
                DataTable dt = ds.Tables[0];
                if (winMessage.triTreatment.Items.Count != 0)
                {
                    winMessage.triTreatment.Items.Clear();
                }
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    Container objCon = new Container();
                    objCon.lblName.Content = dt.Rows[i][0].ToString();
                    winMessage.triTreatment.Items.Add(objCon);

                    if (cur_Campaign == CampaignName)
                    {
                        objCon.lblName.Tag = cur_Campaign + "T";
                    }
                }
                dt.Clear();
                ds.Clear();
            }
            catch
            {
            }
        }
        public void fncGetUsers(string CampaignName)
        {
            try
            {
                DataSet ds = DashBoard.Business.CampaignManagement.ClsCampaignManagement.GetUsers(CampaignName);
                DataTable dt = ds.Tables[0];
                if (winMessage.triUsers.Items.Count != 0)
                {
                    winMessage.triUsers.Items.Clear();
                }
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    Container objCon = new Container();
                    objCon.lblName.Content = dt.Rows[i][0].ToString();
                    winMessage.triUsers.Items.Add(objCon);

                    if (cur_Campaign == CampaignName)
                    {
                        objCon.lblName.Tag = cur_Campaign + "U";
                    }
                }
                dt.Clear();
                ds.Clear();
            }
            catch
            { }
        }
        public void fncGetTreatment()
        {
            try
            {
                DataSet ds = DashBoard.Business.CampaignManagement.ClsCampaignManagement.GetTreatment();
                DataTable dt = ds.Tables[0];
                lstTreatment.Items.Clear();
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ListBoxItem l = new ListBoxItem();
                    l.Content = dt.Rows[i].ItemArray[0];
                    l.PreviewMouseDown += new MouseButtonEventHandler(l_PreviewMouseDown);
                    l.Tag = "Treatment";
                    lstTreatment.Items.Add(l);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        public void fncGetUserList()
        {
            try
            {
                DataSet ds = DashBoard.Business.CampaignManagement.ClsCampaignManagement.GetUserList();
                DataTable dt = ds.Tables[0];
                lstUsers.Items.Clear();
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ListBoxItem lstUser = new ListBoxItem();
                    lstUser.Content = dt.Rows[i].ItemArray[0];
                    lstUser.PreviewMouseDown += new MouseButtonEventHandler(lstUser_PreviewMouseDown);
                    lstUser.Tag = "User";
                    lstUsers.Items.Add(lstUser);
                }
            }
            catch
            {
            }
        }
    }
}
