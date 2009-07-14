/* VMukti 2.0 -- An Open Source Video Communications Suite
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
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
//using System.Drawing;

using System.Windows.Navigation;
using System.Windows.Shapes;
using VMukti.Business;
using System.IO;
using System.Windows.Media.Imaging;
using VMuktiAPI;
using VMukti.Presentation.Controls.VMuktiGrid.DragDropAdviser;
using DnD;
using VMukti.Presentation.Controls.ModuleExplorer;

namespace VMukti.Presentation.Controls
{
    /// <summary>
    /// Interaction logic for ctlModulesAmit.xaml
    /// </summary>
    public partial class CtlModule : UserControl, IDisposable
    {
        #region drag drop
        private Point _startPoint;
        private double _originalLeft;
        private double _originalTop;
        private bool _isDown;
        private bool _isDragging;
        private UIElement _originalElement;
        private DragDropAdorner _overlayElement;
        private string CurrCaption, Tag;
        #endregion 

        public static StringBuilder sb1=new StringBuilder();
        CtlMExpanderItem objExpanderItem = null;
        ClsModuleCollection objCMC=null;
        ClsPageCollection objCPC = null;

        #region pagging
        int mRecordCount = 0,pRecordCount=0, RecordsPerPage = 0,
            mStartIndex = 0,mEndIndex = 0,
            pStartIndex=0, pEndIndex = 0;
        bool blSizeChanged = false, blPageClick = false, blModuleClick = false;
        #endregion

        bool isModule = false;
        public bool isLogin = false;
   

        #region Event declaration

        public delegate void delCloseModule();
        public event delCloseModule EntClosemodule;

        public delegate void DelAutherized();
        public event DelAutherized EntAutherized = null;

        public delegate void DelPageItemSelectionChanged(string strTagText, string strContent);
        public event DelPageItemSelectionChanged EntPageItemSelectionChanged;

        #endregion

        public CtlModule()
        {
            try
            {
                InitializeComponent();
                #region event for add edite the page
                VMuktiAPI.VMuktiHelper.RegisterEvent("PageAdded").VMuktiEvent += new VMuktiEvents.VMuktiEventHandler(CtlModules_VMuktiEvent_PageAdded);
                VMuktiAPI.VMuktiHelper.RegisterEvent("PageEdited").VMuktiEvent += new VMuktiEvents.VMuktiEventHandler(CtlModules_VMuktiEvent_PageEdited);
                VMuktiAPI.VMuktiHelper.RegisterEvent("PageDeleted").VMuktiEvent += new VMuktiEvents.VMuktiEventHandler(CtlModules_VMuktiEvent_PageDeleted);
                #endregion

                //#region property for drag and drop
                //VMuktiDragSourceForModule objVds = new VMuktiDragSourceForModule();
                //VMuktiDropTargetForModule objdt = new VMuktiDropTargetForModule();
                //panelContainer.SetValue(DnD.DragDropManager.DragSourceProperty, objVds);
                //this.SetValue(DnD.DragDropManager.DropTargetProperty, objdt);
                ////grdModule.SetValue(DnD.DragDropManager.DropTargetProperty, objdt);
                ////panelContainer.SetValue(DnD.DragDropManager.DropTargetProperty, objdt);
                ////brdPaging.SetValue(DnD.DragDropManager.DropTargetProperty, objdt);
                //#endregion

                

                #region button event
                // btnClose.Click += new RoutedEventHandler(btnClose_Click);
                btnModule.Click += new RoutedEventHandler(btnModule_Click);
                btnpage.Click += new RoutedEventHandler(btnpage_Click);
                btnNext.Click += new RoutedEventHandler(btnNext_Click);
                btnPrevious.Click += new RoutedEventHandler(btnPrevious_Click);
                objLogin.EntAutherized += new CtlLogin.DelAutherized(objLogin_EntAutherized);
                #endregion

                RecordsPerPage = Convert.ToInt32(panelContainer.ActualWidth / 100);
                panelContainer.SizeChanged += new SizeChangedEventHandler(panelContainer_SizeChanged);
               

                #region drag and drop

                panelContainer.PreviewMouseLeftButtonDown += new MouseButtonEventHandler(panelContainer_PreviewMouseLeftButtonDown);
                panelContainer.PreviewMouseMove += new MouseEventHandler(panelContainer_PreviewMouseMove);
                panelContainer.PreviewMouseLeftButtonUp += new MouseButtonEventHandler(panelContainer_PreviewMouseLeftButtonUp);
                #endregion

            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "CtlModule()", "Controls\\ModuleExplorer\\CtlModule.xaml.cs");
            }
        }

       
        #region drag and drop

        //public event RoutedEventHandler OnModuleDrop
        //{
        //    add
        //    {
        //        base.AddHandler(CtlModule.OnModuleDropEvent, value);
        //    }
        //    remove
        //    {
        //        base.RemoveHandler(CtlModule.OnModuleDropEvent, value);
        //    }
        //}

        //public static readonly RoutedEvent OnModuleDropEvent = EventManager.RegisterRoutedEvent("OnModuleDrop", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(CtlModule));


        void panelContainer_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            try
            {
                if (_isDown)
                {       
                    (((this.Parent as Grid).Parent as ScrollViewer).Parent as pgHome).ForDragdrop(CurrCaption, Tag, e);                    
                 //   base.RaiseEvent(new RoutedEventArgs(CtlModule.OnModuleDropEvent, Tag));     
                    DragFinished(false);
                    e.Handled = true;
                }
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "panelContainer_PreviewMouseLeftButtonUp()", "Controls\\ModuleExplorer\\CtlModule.xaml.cs");
        
            }
           
        }

        void panelContainer_PreviewMouseMove(object sender, MouseEventArgs e)
        {
            try
            {
                if (_isDown && blModuleClick)
                {
                    if ((_isDragging == false) && ((Math.Abs(e.GetPosition(panelContainer).X - _startPoint.X) > SystemParameters.MinimumHorizontalDragDistance) ||
                        (Math.Abs(e.GetPosition(panelContainer).Y - _startPoint.Y) > SystemParameters.MinimumVerticalDragDistance)))
                    {
                        DragStarted();
                    }
                    if (_isDragging)
                    {
                        DragMoved();
                    }
                }
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "panelContainer_PreviewMouseMove()", "Controls\\ModuleExplorer\\CtlModule.xaml.cs");
        
            }
        }

        void panelContainer_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                if (e.Source.GetType() == typeof(CtlMExpanderItem) && blModuleClick)
                {

                    _isDown = true;
                    _startPoint = e.GetPosition(panelContainer);
                    CtlMExpanderItem r = e.Source as CtlMExpanderItem;
                    Tag = r.Tag.ToString();
                    CurrCaption = r.Caption;
                    _originalElement = e.Source as UIElement;

                    panelContainer.CaptureMouse();
                    e.Handled = true;
                }
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "panelContainer_PreviewMouseLeftButtonDown()", "Controls\\ModuleExplorer\\CtlModule.xaml.cs");

            }

        }

        private void DragFinished(bool cancelled)
        {
            try
            {
                System.Windows.Input.Mouse.Capture(null);
                if(_overlayElement != null)
                   AdornerLayer.GetAdornerLayer(_overlayElement.AdornedElement).Remove(_overlayElement);

                _overlayElement = null;
                _originalElement = null;


                _isDragging = false;
                _isDown = false;
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "DragFinished()", "Controls\\ModuleExplorer\\CtlModule.xaml.cs");
            }
        }

        private void DragStarted()
        {
            try
            {
                _isDragging = true;
                _overlayElement = new DragDropAdorner(_originalElement, Tag);
                AdornerLayer layer = AdornerLayer.GetAdornerLayer(_originalElement);
                layer.Add(_overlayElement);
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "DragStarted()", "Controls\\ModuleExplorer\\CtlModule.xaml.cs");
        
            }

        }

        private void DragMoved()
        {
            try
            {
                Point CurrentPosition = System.Windows.Input.Mouse.GetPosition(panelContainer);

                _overlayElement.LeftOffset = CurrentPosition.X - _startPoint.X;
                _overlayElement.TopOffset = CurrentPosition.Y - _startPoint.Y;
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "DragMoved()", "Controls\\ModuleExplorer\\CtlModule.xaml.cs");
        
            }

        }


        #endregion

        #region Add-Remove-Edit-Item

        //public void btnClose_Click(object sender, RoutedEventArgs e)
        //{
        //    try
        //    {
        //        entClosemodule();
        //    }
        //    catch (Exception ex)
        //    {
        //    }
        //}

        public void AddItem(string strCaption, bool ShowImage, ImageType objImageType, string strTag)
        {
            try
            {
                objExpanderItem = new CtlMExpanderItem();
                objExpanderItem.HorizontalAlignment = HorizontalAlignment.Stretch;
                objExpanderItem.VerticalAlignment = VerticalAlignment.Stretch;
                objExpanderItem.Margin = new Thickness(10,10,10,10);

                if (objImageType == ImageType.MaleBuddy || objImageType == ImageType.FemaleBuddy)
                {
                    objExpanderItem.AllowDrop = true;
                }

                objExpanderItem.Tag = strTag;
                objExpanderItem.PreviewMouseDown += new System.Windows.Input.MouseButtonEventHandler(objExpanderItem_PreviewMouseDown);
              
                if (ShowImage)
                {
                    if (objImageType == ImageType.Page)
                    {
                        objExpanderItem.Image = @"\Skins\Images\Page.png";
                    }
                    else if (objImageType == ImageType.Module)
                    {
                       // objExpanderItem.Image = @"\Skins\Images\Modules.png";
                        objExpanderItem.Image = @"\Skins\Images1\Skins.png";
                    }
                    else if (objImageType == ImageType.MaleBuddy)
                    {
                        objExpanderItem.Image = @"\Skins\Images\Buddy.Png";
                    }
                    else if (objImageType == ImageType.FemaleBuddy)
                    {
                        objExpanderItem.Image = @"\Skins\Images\FBuddy.Png";
                    }
                }
                else
                {
                    objExpanderItem.Image = "";
                }

                objExpanderItem.Caption = strCaption;

                if (strTag == "offline")
                {

                    objExpanderItem.Image = @"\Skins\Images\FBuddy.Png";
                    objExpanderItem.ToolTip = strCaption + " is offline";

                   objExpanderItem.PreviewMouseDown -= new System.Windows.Input.MouseButtonEventHandler(objExpanderItem_PreviewMouseDown);
                }
                else if (strTag == "online")
                {

                    objExpanderItem.Image = @"\Skins\Images\Buddy.Png";
                    objExpanderItem.ToolTip = strCaption + " is online";
                }
                panelContainer.Children.Add(objExpanderItem);
               
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "AddItem()", "Controls\\ModuleExplorer\\CtlModule.xaml.cs");
            }
        }

        public void AddItem(string strCaption, string strTag, BitmapImage objimage)
        {
            try
            {
                objExpanderItem = new CtlMExpanderItem();
                objExpanderItem.HorizontalAlignment = HorizontalAlignment.Stretch;
                objExpanderItem.VerticalAlignment = VerticalAlignment.Stretch;
                objExpanderItem.Margin = new Thickness(10, 10, 10, 10);
                if(objimage != null)
                {
                 
                    objExpanderItem.Setimage(objimage);
                }
                else
                {
                    objExpanderItem.Image = @"\Skins\Images1\Skins.png";
                }
               
                objExpanderItem.Tag = strTag;
                objExpanderItem.Caption = strCaption;

                if (strTag == "offline")
                {

                    objExpanderItem.Image = @"\Skins\Images\FBuddy.Png";
                    objExpanderItem.ToolTip = strCaption + " is offline";

                    objExpanderItem.PreviewMouseDown -= new System.Windows.Input.MouseButtonEventHandler(objExpanderItem_PreviewMouseDown);
                }
                else if (strTag == "online")
                {

                    objExpanderItem.Image = @"\Skins\Images\Buddy.Png";
                    objExpanderItem.ToolTip = strCaption + " is online";
                }
                panelContainer.Children.Add(objExpanderItem);

            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "AddItem()", "Controls\\ModuleExplorer\\CtlModule.xaml.cs");
            }
        }

        public void EditPage(string strPageName, string strTag)
        {
            try
            {
                for (int i = 0; i < panelContainer.Children.Count; i++)
                {
                    if (((CtlMExpanderItem)(panelContainer.Children[i])).Tag.ToString() == strTag)
                    {
                        ((CtlMExpanderItem)(panelContainer.Children[i])).Caption = strPageName;
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "EditPage()", "Controls\\ModuleExplorer\\CtlModule.xaml.cs");
            }
        }

        public void RemovePage(string strPageName, string strTag)
        {
            try
            {
                for (int i = 0; i < panelContainer.Children.Count; i++)
                {
                    if (((CtlMExpanderItem)(panelContainer.Children[i])).Caption == strPageName && ((CtlMExpanderItem)(panelContainer.Children[i])).Tag.ToString() == strTag)
                    {
                        panelContainer.Children.RemoveAt(i);
                        pEndIndex = pEndIndex-1;
                        tbStatus.Text = pStartIndex.ToString() + "-" + pEndIndex.ToString() + "out of" + objCPC.Count.ToString();
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "RemovePage()", "Controls\\ModuleExplorer\\CtlModule.xaml.cs");
            }
        }
       
        #endregion
         
        #region pageEvent

        void CtlModules_VMuktiEvent_PageEdited(object sender, VMuktiEventArgs e)
        {
            try
            {
                if (e._args.Count == 2)
                {
                    EditPage(e._args[0].ToString(), e._args[1].ToString());
                }
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "CtlModules_VMuktiEvent_PageEdited()", "Controls\\ModuleExplorer\\CtlModule.xaml.cs");
            }
        }

        void CtlModules_VMuktiEvent_PageDeleted(object sender, VMuktiEventArgs e)
        {
            try
            {
                if (e._args.Count == 2)
                {
                     btnpage_Click(null, null);
                }
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "CtlModules_VMuktiEvent_PageDeleted()", "Controls\\ModuleExplorer\\CtlModule.xaml.cs");
            }
        }

        void CtlModules_VMuktiEvent_PageAdded(object sender, VMuktiEventArgs e)
        {
            try
            {
                if (e._args.Count == 2)
                {
                    btnpage_Click(null, null);
                    
                }
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "CtlModules_VMuktiEvent_PageAdd()", "Controls\\ModuleExplorer\\CtlModule.xaml.cs");
            }
        }

        void objExpanderItem_PreviewMouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            try
            {
                
                    if (EntPageItemSelectionChanged != null)
                    {
                        EntPageItemSelectionChanged(((CtlMExpanderItem)sender).Tag.ToString(), ((CtlMExpanderItem)sender).Caption);
                    }
               
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "objExpanderItem_PreviewMouseDown()", "Controls\\ModuleExplorer\\CtlModule.xaml.cs");
            }
        }

        public void LoadPage(string pageCaption)
        {
            try
            {
                ClsPageCollection objCPC = ClsPageCollection.GetUPageAllocated(VMuktiAPI.VMuktiInfo.CurrentPeer.ID);
                for (int k = 0; k < objCPC.Count; k++)
                {
                    if (objCPC[k].PageTitle == pageCaption)
                    {
                        if (EntPageItemSelectionChanged != null)
                        {
                            EntPageItemSelectionChanged(objCPC[k].PageId.ToString(), pageCaption);
                        }
                    }
                }
                
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "LoadPage()", "Controls\\ModuleExplorer\\CtlModule.xaml.cs");
            }
        }

        #endregion

        #region objloginEvent        
        
        void objLogin_EntAutherized()
        {
            try
            {
                isLogin = true;
                brdCtlLogin.Visibility = Visibility.Hidden;
                brdModuleButtons.Visibility = Visibility.Visible;
                RecordsPerPage = Convert.ToInt32(panelContainer.ActualWidth / 100) - 2;
                btnModule_Click(null, null);

                if (EntAutherized != null)
                {
                    EntAutherized();
                }
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "objLogin_EntAutherized()", "Controls\\ModuleExplorer\\CtlModule.xaml.cs");
            }
        }

        public void LogOut()
        {
            try
            {
                isLogin = false;

                mRecordCount = 0;
                pRecordCount = 0;
                mStartIndex = 0;
                mEndIndex = 0;
                pStartIndex = 0;
                pEndIndex = 0;

                panelCatagory.Children.Clear();
                panelContainer.Children.Clear();

                brdModuleButtons.Visibility = Visibility.Hidden;
                brdCtlLogin.Visibility = Visibility.Visible;
                tbStatus.Text = "Login to Collaborate ";
                objCMC = null;
                objCPC = null;
                btnNext.IsEnabled = false;
                btnPrevious.IsEnabled = false;
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "LogOut()", "Controls\\ModuleExplorer\\CtlModule.xaml.cs");
            }
        }

        public void ShowLogin()
        {
            try
            {
                objLogin.Visibility = Visibility.Visible;
                objLogin.btnLogIn.IsEnabled = true;
                objLogin.btnSignUp.IsEnabled = true;
                objLogin.lblValidate.Content = "";
                objLogin.txtUserNameID.Text = "";
                objLogin.pwdPasssword.Password = "";
                objLogin.txtUserNameID.IsEnabled = true;
                objLogin.pwdPasssword.IsEnabled = true;
                objLogin.txtUserNameID.Focus();
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "ShowLogin()", "Controls\\ModuleExplorer\\CtlModule.xaml.cs");
            }
        }

        #endregion
        
        #region Pagging and Module Button

        void btnPrevious_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (isModule)
                {
                    #region logic for module
                    //intModuleIndex -= 7;
                    //if (intModuleIndex  > 7 || intModuleIndex==0 )
                    //{
                    //    btnPrevious.IsEnabled = true;
                    //    btnNext.IsEnabled = true;
                    //    tbStatus.Text = string.Format("{0}" + " to " + "{1}" + " out of " + "{2}", intModuleIndex, intModuleIndex+7 , objCMC.Count);
                    //    panelContainer.Children.Clear();

                    //    for (int j = intModuleIndex; j < intModuleIndex+7 ; j++)
                    //    {
                    //        if (objCMC[j].IsCollaborative.ToLower() == "true")
                    //        {
                    //            AddItem(objCMC[j].ModuleTitle, objCMC[j].ModuleId + ",ModuleType,True", GetImage(objCMC[j].ImageFile));
                    //        }
                    //        else
                    //        {
                    //            AddItem(objCMC[j].ModuleTitle, objCMC[j].ModuleId + ",ModuleType,True", GetImage(objCMC[j].ImageFile));
                    //        }
                    //    }
                    //}

                    #endregion

                    mStartIndex = mStartIndex - RecordsPerPage;
                    FncFillPanel(mStartIndex,true);

                }
                else
                {
                    pStartIndex = pStartIndex - RecordsPerPage;
                    FncFillPanel(pStartIndex, false);
                }

            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "btnPrevious_Click()", "Controls\\ModuleExplorer\\CtlModule.xaml.cs");
            }
        }

        void btnNext_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (isModule)
                {
                    #region logic for module
                  
                    #endregion
                    mStartIndex = mStartIndex + RecordsPerPage;
                    FncFillPanel(mStartIndex,true);
                }
                else
                {
                    pStartIndex = pStartIndex + RecordsPerPage;
                    FncFillPanel(pStartIndex, false);

                }
            
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "btnNext_Click()", "Controls\\ModuleExplorer\\CtlModule.xaml.cs");
            }
        }

        void btnModule_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                blModuleClick = true; ;
                isModule = true;

                int precount = 0;
                if (objCMC != null)
                {
                    precount = objCMC.Count;
                    ClsModuleCollection objTest = new ClsModuleCollection();
                    objTest = ClsModuleCollection.GetCModCountCheck(VMuktiAPI.VMuktiInfo.CurrentPeer.RoleID, objCMC.Count+1);
                    if (objTest.Count > 0)
                    {
                        objCMC = objTest;
                    }
                }
                else
                {
                    objCMC = ClsModuleCollection.GetCMod(VMuktiAPI.VMuktiInfo.CurrentPeer.RoleID);
                    for (int i = 0; i < objCMC.Count; i++)
                    {
                        if (objCMC[i].AssemblyFile.Equals("MeetingSchedular.Presentation.dll"))
                        {
                            objCMC.RemoveAt(i);
                            break;
                        }
                    }
                    
                }
                //if size of panel container is changed or new module is added then call this function
                if (objCMC.Count != precount || blSizeChanged || blPageClick)
                {
                    blPageClick = false;
                    panelContainer.Children.Clear();
                    blSizeChanged = false;
                    mRecordCount = objCMC.Count;
                    if (mRecordCount != 0)
                    {
                        if (objCMC.Count > RecordsPerPage)
                        {
                            btnPrevious.IsEnabled = false;
                            btnNext.IsEnabled = true;
                            tbStatus.Text = string.Format("1 to " + "{0}" + " out of " + "{1}", RecordsPerPage, objCMC.Count);


                            for (int j = 0; j < RecordsPerPage; j++)
                            {
                                if (objCMC[j].IsCollaborative.ToLower() == "true")
                                {
                                    AddItem(objCMC[j].ModuleTitle, objCMC[j].ModuleId + ",ModuleType,True", GetImage(objCMC[j].ImageFile));
                                }
                                else
                                {
                                    AddItem(objCMC[j].ModuleTitle, objCMC[j].ModuleId + ",ModuleType,False", GetImage(objCMC[j].ImageFile));
                                }
                            }

                        }
                        else
                        {
                            btnPrevious.IsEnabled = false;
                            btnNext.IsEnabled = false;
                            tbStatus.Text = string.Format("1 to " + "{0}" + " out of " + "{1}", objCMC.Count, objCMC.Count);
                            for (int j = 0; j < objCMC.Count; j++)
                            {
                                if (objCMC[j].IsCollaborative.ToLower() == "true")
                                {
                                    AddItem(objCMC[j].ModuleTitle, objCMC[j].ModuleId + ",ModuleType,True", GetImage(objCMC[j].ImageFile));
                                }
                                else
                                {
                                    AddItem(objCMC[j].ModuleTitle, objCMC[j].ModuleId + ",ModuleType,False", GetImage(objCMC[j].ImageFile));
                                }
                            }
                        }
                    }
                    else
                    {
                        btnPrevious.IsEnabled = false;
                        btnNext.IsEnabled = false;
                        tbStatus.Text = string.Format("No records are Available");
                    }
                }
                brdPaging.Visibility = Visibility.Visible;
            }

            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "btnModule_Click()", "Controls\\ModuleExplorer\\CtlModule.xaml.cs");
            }
            

        }

        void btnpage_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                isModule = false;
                blPageClick = true;
                brdPaging.Visibility = Visibility.Visible;
                int precount = 0;
                if (objCPC != null)
                {
                    precount = objCPC.Count;
                }
                objCPC = ClsPageCollection.GetUPageAllocated(VMuktiAPI.VMuktiInfo.CurrentPeer.ID);
                if (precount != objCPC.Count || blSizeChanged || blModuleClick)
                {
                    panelContainer.Children.Clear();
                    blModuleClick = false;
                pRecordCount = objCPC.Count;
                    blSizeChanged = false;
                if (pRecordCount != 0)
                {
                    if (objCPC.Count > RecordsPerPage)
                    {
                        btnPrevious.IsEnabled = false;
                        btnNext.IsEnabled = true;
                        tbStatus.Text = string.Format("1 to" + "{0}" + " out of " + "{1}", RecordsPerPage, objCPC.Count);

                        for (int k = 0; k < RecordsPerPage; k++)
                        {

                            AddItem(objCPC[k].PageTitle, true, ImageType.Page, objCPC[k].PageId.ToString());
                        }
                    }
                    else
                    {
                        btnPrevious.IsEnabled = false;
                        btnNext.IsEnabled = false;
                        tbStatus.Text = string.Format("1 to" + "{0}" + " out of " + "{1}", objCPC.Count, objCPC.Count);

                        for (int k = 0; k < objCPC.Count; k++)
                        {
                            AddItem(objCPC[k].PageTitle, true, ImageType.Page, objCPC[k].PageId.ToString());
                        }
                    }
                    brdPaging.Visibility = Visibility.Visible;
                }
                else
                {
                    btnPrevious.IsEnabled = false;
                    btnNext.IsEnabled = false;
                    tbStatus.Text = string.Format("No records are Available");

                }
            }
            }

            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "btnPage_Click()", "Controls\\ModuleExplorer\\CtlModule.xaml.cs");
            }
        }

        void FncFillPanel(int StartIndex,bool isModule)
        {
            try
            {
                RecordsPerPage = Convert.ToInt32(panelContainer.ActualWidth / 100) - 2;
                if (isModule)
                {
                    #region logic

                    mEndIndex = mStartIndex + RecordsPerPage;
                   
                    #region endindex  > total records
                    if (mEndIndex > mRecordCount)
                    {
                        if (RecordsPerPage >= mRecordCount)
                        {
                            btnPrevious.IsEnabled = false;
                            btnNext.IsEnabled = false;
                        }

                        else
                        {
                            btnNext.IsEnabled = false;
                            btnPrevious.IsEnabled = true;
                        }
                       
                        mEndIndex = mRecordCount % RecordsPerPage;

                        panelContainer.Children.Clear();
                        tbStatus.Text = string.Format("{0}" + " to " + "{1}" + " out of " + "{2}", mStartIndex+1, mEndIndex + mStartIndex,  objCMC.Count);
                      
                        for (int j = mStartIndex; j < mEndIndex + mStartIndex; j++)
                        {
                            if (!objCMC[j].AssemblyFile.Equals("MeetingSchedular.Presentation.dll"))
                            {
                            if (objCMC[j].IsCollaborative.ToLower() == "true")
                            {
                                AddItem(objCMC[j].ModuleTitle, objCMC[j].ModuleId + ",ModuleType,True", GetImage(objCMC[j].ImageFile));
                            }
                            else
                            {
                                AddItem(objCMC[j].ModuleTitle, objCMC[j].ModuleId + ",ModuleType,False", GetImage(objCMC[j].ImageFile));
                                }
                            }
                        }
                    }
                    #endregion

                    #region endindex is same as record count

                    else if (mEndIndex == mRecordCount)
                    {
                        btnPrevious.IsEnabled = true;
                        btnNext.IsEnabled = false;
                        panelContainer.Children.Clear();
                        tbStatus.Text = string.Format("{0}" + " to " + "{1}" + " out of " + "{2}", mStartIndex + 1, mEndIndex, objCMC.Count);

                        for (int j = mStartIndex; j < mEndIndex; j++)
                        {
                            if (!objCMC[j].AssemblyFile.Equals("MeetingSchedular.Presentation.dll"))
                            {
                                if (objCMC[j].IsCollaborative.ToLower() == "true")
                                {
                                    AddItem(objCMC[j].ModuleTitle, objCMC[j].ModuleId + ",ModuleType,True", GetImage(objCMC[j].ImageFile));
                                }
                                else
                                {
                                    AddItem(objCMC[j].ModuleTitle, objCMC[j].ModuleId + ",ModuleType,False", GetImage(objCMC[j].ImageFile));
                                }
                            }
                        }
                    }
                    #endregion

                    #region endindex is same as record count

                    else
                    {
                        if (RecordsPerPage >= mRecordCount)
                        {
                            btnPrevious.IsEnabled = false;
                            btnNext.IsEnabled = false;
                        }

                        else if (mStartIndex.Equals(0))
                        {
                            btnPrevious.IsEnabled = false;
                            btnNext.IsEnabled = true;
                        }

                        else
                        {
                            btnPrevious.IsEnabled = true;
                            btnNext.IsEnabled = true;
                        }
                        panelContainer.Children.Clear();
                        tbStatus.Text = string.Format("{0}" + " to " + "{1}" + " out of " + "{2}", mStartIndex+1, mEndIndex, objCMC.Count);
                      
                        for (int j = mStartIndex; j < mEndIndex; j++)
                        {
                            if (!objCMC[j].AssemblyFile.Equals("MeetingSchedular.Presentation.dll"))
                            {
                            if (objCMC[j].IsCollaborative.ToLower() == "true")
                            {
                                AddItem(objCMC[j].ModuleTitle, objCMC[j].ModuleId + ",ModuleType,True", GetImage(objCMC[j].ImageFile));
                            }
                            else
                            {
                                AddItem(objCMC[j].ModuleTitle, objCMC[j].ModuleId + ",ModuleType,False", GetImage(objCMC[j].ImageFile));
                                }
                            }
                        }
                    }
                    #endregion

                    #endregion
                }

                else
                {
                    #region logic

                    pEndIndex = pStartIndex + RecordsPerPage;


                    if (pEndIndex > pRecordCount)
                    {
                        if (RecordsPerPage >= pRecordCount)
                        {
                            btnPrevious.IsEnabled = false;
                            btnNext.IsEnabled = false;
                        }

                        else
                        {
                            btnNext.IsEnabled = false;
                            btnPrevious.IsEnabled = true;
                        }

                        pEndIndex = pRecordCount % RecordsPerPage;

                        panelContainer.Children.Clear();
                        tbStatus.Text = string.Format("{0}" + " to " + "{1}" + " out of " + "{2}", pStartIndex+1, pEndIndex + pStartIndex, objCPC.Count);

                        for (int j = pStartIndex; j < pEndIndex + pStartIndex; j++)
                        {                                
                                AddItem(objCPC[j].PageTitle, true, ImageType.Page, objCPC[j].PageId.ToString());
                        }
                    }
                    else if (pEndIndex == pRecordCount)
                    {
                        btnPrevious.IsEnabled = true;
                        btnNext.IsEnabled = false;

                        panelContainer.Children.Clear();
                        tbStatus.Text = string.Format("{0}" + " to " + "{1}" + " out of " + "{2}", pStartIndex + 1, pEndIndex, objCPC.Count);

                        for (int j = pStartIndex; j < pEndIndex; j++)
                        {

                            AddItem(objCPC[j].PageTitle, true, ImageType.Page, objCPC[j].PageId.ToString());

                        }
                    }

                    else
                    {
                        if (RecordsPerPage >= pRecordCount)
                        {
                            btnPrevious.IsEnabled = false;
                            btnNext.IsEnabled = false;
                        }

                        else if (pStartIndex.Equals(0))
                        {
                            btnPrevious.IsEnabled = false;
                            btnNext.IsEnabled = true;
                        }

                        else
                        {
                            btnPrevious.IsEnabled = true;
                            btnNext.IsEnabled = true;
                        }
                        panelContainer.Children.Clear();
                        tbStatus.Text = string.Format("{0}" + " to " + "{1}" + " out of " + "{2}", pStartIndex+1, pEndIndex, objCPC.Count);

                        for (int j = pStartIndex; j < pEndIndex; j++)
                        {
                            
                                AddItem(objCPC[j].PageTitle, true, ImageType.Page, objCPC[j].PageId.ToString());
                           
                        }
                    }
                    #endregion
                }
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "FncFillPanel()", "Controls\\ModuleExplorer\\CtlModule.xaml.cs");
            }
        }

        void panelContainer_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            try
            {
                
                 if (isLogin && e.NewSize.Width !=e.PreviousSize.Width)
                {
                    mRecordCount = 0;
                    pRecordCount = 0;
                    mStartIndex = 0;
                    mEndIndex = 0;
                    pStartIndex = 0;
                    pEndIndex = 0;
                    blSizeChanged = true;
                    RecordsPerPage = (Convert.ToInt32(panelContainer.ActualWidth / 100) - 2);

                    if (isModule)
                    {
                        btnModule_Click(null, null);
                    }
                    else
                    {
                        btnpage_Click(null, null);
                    }
                }
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "panelContainer_SizeChanged()", "Controls\\ModuleExplorer\\CtlModule.xaml.cs");
            }
        }

        #endregion

        #region Temp function

        public BitmapImage GetImage(byte[] imgArry)
        {
            try
            {
                if (imgArry != null && imgArry.Length > 0)
                {
                    BitmapImage bmi = new BitmapImage();
                    bmi.BeginInit();
                    bmi.StreamSource = new MemoryStream(imgArry);
                    bmi.EndInit();
                    return bmi;
                }
                else
                {
                    return null;
                }

            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "GetImage()", "Controls\\ModuleExplorer\\CtlModule.xaml.cs");
                return null;
            }
        }

        #endregion

        #region IDisposable Members

        public void Dispose()
        {
            try
            {
                if (EntClosemodule != null)
                    EntClosemodule = null;

                if (EntPageItemSelectionChanged != null)
                    EntPageItemSelectionChanged = null;

                if (EntAutherized != null)
                    EntAutherized = null;
               
                if (objExpanderItem != null)
                    objExpanderItem = null;
                
                if (objCMC != null)
                    objCMC = null;

                if (objCPC != null)
                    objCPC = null;
               
                
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "Dispose()", "Controls\\ModuleExplorer\\CtlModule.xaml.cs");
            }
        }

        #endregion

        ~CtlModule()
        {
            try
            {
                Dispose();
            }

            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "CtlModule()", "Controls\\ModuleExplorer\\CtlModule.xaml.cs");
            }
        }
    }
}