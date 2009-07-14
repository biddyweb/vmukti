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

namespace DispositionRender.Presentation
{
    /// <summary>
    /// Interaction logic for BtnDispositionRender.xaml
    /// </summary>
    public partial class BtnAutoDispositionRender : UserControl
    {
        public static int Entered = 0;
        UserControl DispotionRender = null;
        CtlDispositionRender render = null;
        public BtnAutoDispositionRender(ModulePermissions[] MyPermissions)
        {
            try
            {
                this.DispotionRender = new UserControl();
                InitializeComponent();
                VMuktiAPI.VMuktiHelper.RegisterEvent("SignOut").VMuktiEvent += new VMuktiAPI.VMuktiEvents.VMuktiEventHandler(BtnAutoDispositionRender_VMuktiEvent);
                render = new CtlDispositionRender(MyPermissions);
                this.DispotionRender.Content = render;
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "BtnDispositionRender()", "BtnDispositionRender.xaml.cs");
            }
        }
        void BtnAutoDispositionRender_VMuktiEvent(object sender, VMuktiAPI.VMuktiEventArgs e)
        {
            try
            {
                ClosePod();
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "BtnAutoDispositionRender_VMuktiEvent()", "BtnDispositionRender.xaml.cs");
            }
        }

        private void btnDisposition_MouseEnter(object sender, MouseEventArgs e)
        {
            //try
            //{
            //    if (Entered ==0)
            //    {
            //        Point x = base.PointToScreen(Mouse.GetPosition(this));                    
            //        new Disposition_PopUp().ShowPopUp(this.DispotionRender, x);
            //        Entered = 1;
            //    }

            //}
            try
            {
                Point x = base.PointFromScreen(Mouse.GetPosition((Page)((Grid)((Canvas)this.Parent).Parent).Parent));
                // Point x = new Point(0, 0);
                Rect rect = new Rect(x.X, x.Y, 200, 200);
                PopUpDispo.PlacementRectangle = rect;
                PopUpDispo.IsOpen = true;
                PopUpDispo.Child = this.DispotionRender;
            }
            catch(Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "btnDisposition_MouseEnter()", "BtnDispositionRender.xaml.cs");
            }
        }      

        public void ClosePod()
        {
           try
           {
               if (render != null)
               {
                   render.ClosePod();
                   render = null;
               }
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "PopUpDispo_MouseLeave()", "BtnDispositionRender.xaml.cs");
            }
        }
        private void PopUpDispo_MouseLeave(object sender, MouseEventArgs e)
        {
            try
            {
                PopUpDispo.IsOpen = false;
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "PopUpDispo_MouseLeave()", "BtnDispositionRender.xaml.cs");
            }
        }    
    }
}
