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

namespace DashBoard.Presentation.CampaignManagement
{
    /// <summary>
    /// Interaction logic for Container.xaml
    /// </summary>
    public partial class Container : UserControl
    {
        public Container()
        {
            InitializeComponent();
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                WindowMessage obj = new WindowMessage();

                string DelStr = Convert.ToString(lblName.Content);
                string FullTag = Convert.ToString(lblName.Tag);

                string CName = FullTag.Substring(0, FullTag.Length - 1);
                string tag = FullTag.Remove(0, FullTag.Length - 1);

                if (tag == "T")
                    DashBoard.Business.CampaignManagement.ClsCampaignManagement.DeleteTreatment(CName, DelStr);

                if (tag == "U")
                    DashBoard.Business.CampaignManagement.ClsCampaignManagement.DeleteUser(CName, DelStr);
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "btnDelete_Click", "DashBoard.Presentation--:--CampaignManagement--:--Container.xaml.cs");
            }
        }        
    }
}
