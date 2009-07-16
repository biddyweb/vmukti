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
using ScriptRender.Business;
using VMuktiAPI;

namespace ScriptRender.Presentation
{
    public enum ModulePermissions
    {
        View = 1
    }

    public partial class ctlContainer : UserControl
    {
        ModulePermissions[] _MyPermissions;
        ScriptRender.Presentation.ctlUrl objCtlURL = null;
        ScriptRender.Presentation.CtlScriptRender objCtlScriptRender = null;
        public ctlContainer(ModulePermissions[] MyPermissions)
        {
            InitializeComponent();
            try
            {
                _MyPermissions = MyPermissions;
                //FncPermissionsReview();

                long ScriptID = VMuktiAPI.VMuktiInfo.CurrentPeer.ScriptID;
                string scriptType = ClsScriptRender.Script_GetScriptType(ScriptID);

                if (scriptType.Equals("Web Script"))
                {
                    objCtlURL = new ctlUrl();
                    grdMain.Children.Add(objCtlURL);
                    grdMain.Height = objCtlURL.Height;
                    
                }
                else
                {
                    objCtlScriptRender = new CtlScriptRender();
                    grdMain.Children.Add(objCtlScriptRender);
                    grdMain.Height = objCtlScriptRender.Height;
                }

                
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "ctlContainer", "CtlContainer.xaml.cs");
            }
        }

        public void ClosePod()
        {
            try
            {
                //MessageBox.Show("Close pod called for ctlContainer");
                objCtlScriptRender.ClosePod();
                if (objCtlScriptRender != null)
                {
                    objCtlScriptRender = null;
                }
                if (objCtlURL != null)
                {
                    objCtlURL = null;
                }
                grdMain.Children.Clear();
                
            }
            catch (Exception exp)
            {

                VMuktiAPI.VMuktiHelper.ExceptionHandler(exp, "ClosePod", "CtlContainer.xaml.cs");

            }
        }

        void FncPermissionsReview()
        {
            try
            {
                this.Visibility = Visibility.Hidden;

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
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "FncPermissionsReview", "CtlContainer.xaml.cs");
            }
        }

       
       
    }
}
