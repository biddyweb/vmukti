using System;
using System.Collections.Generic;

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
using VMuktiAPI;

namespace News.Bussines
{
    /// <summary>
    /// Interaction logic for EvzItem.xaml
    /// </summary>
    public partial class EvzItem : UserControl
    {
        #region Public Methods

        public EvzItem()
        {
            try
            {
                this.InitializeComponent();
                txtTitle.MouseDown += new System.Windows.Input.MouseButtonEventHandler(txtTitle_MouseDown);
            }
            catch (Exception exp)
            {
                exp.Data.Add("My Key", "EvzItem()--:--EvzItem.xaml.cs--:--" + exp.Message + " :--:--");
                ClsException.LogError(exp);
                ClsException.WriteToErrorLogFile(exp);
            }

        }

        #endregion

        #region Public Properties

        public string Title
        {
            set
            {
                txtTitle.Text = value;
            }
        }

        #endregion

        #region Private Members

        private void txtTitle_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {

        }

        #endregion
    }
}
