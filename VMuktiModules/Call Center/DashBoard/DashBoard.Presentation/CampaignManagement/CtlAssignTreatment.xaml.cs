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
    /// Interaction logic for CtlAssignTreatment.xaml
    /// </summary>
    public partial class CtlAssignTreatment : UserControl
    {
        WindowMessage winMessage = new WindowMessage();
        Thickness abc = new Thickness();
        
        public CtlAssignTreatment()
        {
            try
            {
                InitializeComponent();
                fncGetTreatment();
                fncCampaignList(); 
                fncGetUserList();
                fncGetGroup();
                grdMain.Children.Add(winMessage);
                winMessage.Visibility = Visibility.Hidden;               
                this.lstTreatment.SelectionChanged += new SelectionChangedEventHandler(lstTreatment_SelectionChanged);
                this.lstCampaign.SelectionChanged += new SelectionChangedEventHandler(lstCampaign_SelectionChanged);
                this.lstCampaign.AllowDrop = true;
                this.lstUsers.SelectionChanged+=new SelectionChangedEventHandler(lstUsers_SelectionChanged);
                this.lstGroup.SelectionChanged+=new SelectionChangedEventHandler(lstGroup_SelectionChanged);
                lstCampaign.DragOver+=new DragEventHandler(lstCampaign_DragOver);                      
              
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
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
               
               string str1=(string)e.Data.GetData(DataFormats.StringFormat);
               string pFix = str1.Substring(str1.Length - 1,1);
               string Actual=str1.Substring(0,str1.Length - 1);
               string strCamapign = ((ListBoxItem)(sender)).Content.ToString();
               Container objCon = new Container();
               if (pFix == "u")
               {
                   DataSet ds = DashBoard.Business.CampaignManagement.ClsAssignTreatment.CountGroup(null,strCamapign);
                   if (ds.Tables[0].Rows.Count == 0)
                   {
                       MessageBox.Show("No Group Assigned to this Campaign");
                   }
                   else
                   {
                       objCon.lblName.Content = Actual;                       
                       winMessage.triUsers.Items.Add(objCon);
                       DashBoard.Business.CampaignManagement.ClsAssignTreatment.InsertCampaignUser(strCamapign, Actual);
                   }
               }
               else if (pFix == "g")
               {
                   objCon.lblName.Content = Actual;                  
                   winMessage.triGroup.Items.Add(objCon);
                   DashBoard.Business.CampaignManagement.ClsAssignTreatment.InsertCampaignGroup(strCamapign, Actual);
               }
               else
               {
                   DataSet ds = DashBoard.Business.CampaignManagement.ClsAssignTreatment.GetCampaignTreatment(strCamapign, Actual);
                   if (ds.Tables[0].Rows.Count == 0)
                   {
                   objCon.lblName.Content = Actual;                    
                   winMessage.triTreatment.Items.Add(objCon);
                   DashBoard.Business.CampaignManagement.ClsAssignTreatment.InsertCampaignTreatment(strCamapign, Actual);
                   }
               }

            }
            catch
            {
            }
        }

        private string ListBoxItem(object sender)
        {
            throw new NotImplementedException();
        }

        void lstShow_DragOver(object sender, DragEventArgs e)
        {
            try
            {
                e.Effects=DragDropEffects.All;
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }      
       
        void l_MouseEnter(object sender, MouseEventArgs e)
        {
            try
            {
                
                fncGetUsers(((ListBoxItem)sender).Content.ToString());
                fncGetTreatment(((ListBoxItem)sender).Content.ToString());
                fncGetGroup(((ListBoxItem)sender).Content.ToString());
                if (winMessage.Visibility == Visibility.Hidden)
                {

                    winMessage.grdPopUp.Background = ((ListBoxItem)sender).Background;
                    winMessage.Margin = abc;
                    winMessage.txtCampaign.Text = ((ListBoxItem)sender).Content.ToString();
                    winMessage.HorizontalAlignment = HorizontalAlignment.Left;
                    winMessage.VerticalAlignment = VerticalAlignment.Top;
                    winMessage.Visibility = Visibility.Visible;
                }
                else
                {
                    winMessage.grdPopUp.Background = ((ListBoxItem)sender).Background;
                    winMessage.Margin = abc;
                    winMessage.txtCampaign.Text = ((ListBoxItem)sender).Content.ToString();
                    winMessage.HorizontalAlignment = HorizontalAlignment.Left;
                    winMessage.VerticalAlignment = VerticalAlignment.Top;
                }
            }
            catch(Exception ex)
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
        void lstGroupList_PreviewMouseDown(object sender, MouseEventArgs e)
        {
            try
            {
                if (lstGroup.Items.Count == 0)
                    return;

                string s1 = ((ListBoxItem)sender).Content.ToString().Trim() + "g";
                if (s1 != "" && s1 != null)
                {
                    DragDropEffects drgGroup = DragDrop.DoDragDrop(lstGroup, s1, DragDropEffects.All);
                }
            }
            catch
            {
            }
        }
        void lstGroup_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (lstGroup.Items.Count == 0)
                    return;

                string s1 = ((ListBoxItem)sender).Content.ToString().Trim() + "g";
                if (s1 != "" && s1 != null)
                {
                    DragDropEffects drgGroup = DragDrop.DoDragDrop(lstGroup, s1, DragDropEffects.All);
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
                    string s1 = ((ListBoxItem)lstTreatment.SelectedItem).Content.ToString()+"t";
                    
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
                DataSet ds = DashBoard.Business.CampaignManagement.ClsAssignTreatment.GetCampainNames(UserID);
                DataTable dt = ds.Tables[0];
                abc = lstCampaign.Margin;
                lstCampaign.Items.Clear();
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ListBoxItem l = new ListBoxItem();
                    l.Height = 30;
                    if (i % 2 == 0)
                    {
                        l.Background = Brushes.AntiqueWhite;
                    }
                    else
                    {
                        l.Background = Brushes.LightGray;
                    }
                    l.Drop+=new DragEventHandler(l_Drop);                   
                    l.MouseEnter+=new MouseEventHandler(l_MouseEnter);                    
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
                DataSet ds = DashBoard.Business.CampaignManagement.ClsAssignTreatment.GetTreatment1(CampaignName);
                DataTable dt = ds.Tables[0];
                if (winMessage.triTreatment.Items.Count != 0)
                {
                    winMessage.triTreatment.Items.Clear();
                }
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    Container objCon = new Container();
                    objCon.lblName.Content = dt.Rows[i][0].ToString();
                    objCon.lblName.Tag = "T";
                    winMessage.triTreatment.Items.Add(objCon);                    
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
                DataSet ds = DashBoard.Business.CampaignManagement.ClsAssignTreatment.GetUsers(CampaignName);
                DataTable dt = ds.Tables[0];
                if (winMessage.triUsers.Items.Count != 0)
                {
                    winMessage.triUsers.Items.Clear();
                }
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    Container objCon = new Container();
                    objCon.lblName.Content = dt.Rows[i][0].ToString();
                    objCon.lblName.Tag = "U";
                    winMessage.triUsers.Items.Add(objCon);
                }
                dt.Clear();
                ds.Clear();

            }
            catch
            {
            }
        }
        public void fncGetGroup(string CampaignName)
        {
            try
            {
                DataSet ds = DashBoard.Business.CampaignManagement.ClsAssignTreatment.GetCampaignGroup(CampaignName);
                DataTable dt = ds.Tables[0];
                if (winMessage.triGroup.Items.Count != 0)
                {
                    winMessage.triGroup.Items.Clear();
                }
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    Container objCon = new Container();
                    objCon.lblName.Content = dt.Rows[i][0].ToString();
                    winMessage.triGroup.Items.Add(objCon);
                }
                dt.Clear();
                ds.Clear();

            }
            catch
            {
            }
        }
        public void fncGetTreatment()
        {
            try
            {
                DataSet ds = DashBoard.Business.CampaignManagement.ClsAssignTreatment.GetTreatment();
                DataTable dt = ds.Tables[0];
                lstTreatment.Items.Clear();
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ListBoxItem l = new ListBoxItem();
                    l.Content = dt.Rows[i].ItemArray[0];
                    l.PreviewMouseDown+=new MouseButtonEventHandler(l_PreviewMouseDown);
                    l.Tag = "Treatment";
                    lstTreatment.Items.Add(l);
                }   
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        public void fncGetGroup()
        {
            try
            {
                DataSet ds = DashBoard.Business.CampaignManagement.ClsAssignTreatment.GetGroup();
                DataTable dt = ds.Tables[0];
                lstGroup.Items.Clear();
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ListBoxItem lstGroupList = new ListBoxItem();
                    lstGroupList.Content = dt.Rows[i].ItemArray[0];
                    lstGroupList.PreviewMouseDown+=new MouseButtonEventHandler(lstGroupList_PreviewMouseDown);
                    lstGroupList.Tag = "Group";
                    lstGroup.Items.Add(lstGroupList);
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        public void fncGetUserList()
        {
            try
            {
                DataSet ds = DashBoard.Business.CampaignManagement.ClsAssignTreatment.GetUserList();
                DataTable dt = ds.Tables[0];
                lstUsers.Items.Clear();                
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ListBoxItem lstUser = new ListBoxItem();
                    lstUser.Content = dt.Rows[i].ItemArray[0];
                    lstUser.PreviewMouseDown+=new MouseButtonEventHandler(lstUser_PreviewMouseDown);
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
