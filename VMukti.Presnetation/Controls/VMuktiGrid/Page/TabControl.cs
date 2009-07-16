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
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Media;
using System.Diagnostics;
using System.Collections.Specialized;
using System.Windows.Media.Effects;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using VMuktiGrid.CustomMenu;
using VMukti.Presentation.Controls;
//using VMukti.Presentation.Page_Tab;
using System.Collections.Generic;
using VMukti.Business.CommonDataContracts;
using VMuktiAPI;
using VMukti.Business.VMuktiGrid;
using VMukti.Business;
using System.Xml;
using System.Windows.Markup;
using System.IO;
using System.Text;

namespace VMuktiGrid.ctlPage
{
    [TemplatePart(Name = "PART_DropDown", Type = typeof(ToggleButton))]
    [TemplatePart(Name = "PART_RepeatLeft", Type = typeof(RepeatButton))]
    [TemplatePart(Name = "PART_RepeatRight", Type = typeof(RepeatButton))]
    [TemplatePart(Name = "PART_NewTabButton", Type = typeof(ButtonBase))]
    [TemplatePart(Name = "PART_ScrollViewer", Type = typeof(ScrollViewer))]
    [TemplatePart(Name = "RowDefinition0", Type = typeof(RowDefinition))]

    public class TabControl : System.Windows.Controls.TabControl, IDisposable
    {
        private bool disposed = false;
        // public Events
        public event EventHandler<TabItemEventArgs> TabItemAdded;
        public event EventHandler<TabItemCancelEventArgs> TabItemClosing;
        public event EventHandler<TabItemEventArgs> TabItemClosed;

        public delegate void DelSendPageInfo(VMuktiGrid.ctlPage.TabItem objSelectedPage, string buddyname);
        public DelSendPageInfo objSendPageInfo;

        public clsPageInfo objPage;

        // TemplatePart controls
        private ToggleButton _toggleButton;
        private RowDefinition _RowDefinition0;
        static TabControl()
        {
            try
            {
                DefaultStyleKeyProperty.OverrideMetadata(typeof(TabControl), new FrameworkPropertyMetadata(typeof(TabControl)));

                TabStripPlacementProperty.AddOwner(typeof(TabControl), new FrameworkPropertyMetadata(Dock.Top, new PropertyChangedCallback(OnTabStripPlacementChanged)));
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "TabControl()", "Controls\\VMuktiGrid\\Page\\TabControl.cs");
            }
        }

        /// <summary>
        /// OnTabStripPlacementChanged property callback
        /// </summary>
        /// <remarks>
        ///     We need to supplement the base implementation with this method as the base method does not work when
        ///     we are using virtualization in the tabpanel, it only updates visible items
        /// </remarks>
        private static void OnTabStripPlacementChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            try
            {
                TabControl tc = (TabControl)d;

                foreach (TabItem item in tc.Items)
                    item.CoerceValue(TabItem.TabStripPlacementProperty);
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "OnTabStripPlacementChanged()", "Controls\\VMuktiGrid\\Page\\TabControl.cs");
            }
        }

        #region Dependancy properties

        #region Brushes

        public Brush pgTabItemNormalBackground
        {
            get { return (Brush)GetValue(pgTabItemNormalBackgroundProperty); }
            set { SetValue(pgTabItemNormalBackgroundProperty, value); }
        }
        public static readonly DependencyProperty pgTabItemNormalBackgroundProperty = DependencyProperty.Register("pgTabItemNormalBackground", typeof(Brush), typeof(TabControl), new UIPropertyMetadata(null));

        public Brush pgTabItemMouseOverBackground
        {
            get { return (Brush)GetValue(pgTabItemMouseOverBackgroundProperty); }
            set { SetValue(pgTabItemMouseOverBackgroundProperty, value); }
        }
        public static readonly DependencyProperty pgTabItemMouseOverBackgroundProperty = DependencyProperty.Register("pgTabItemMouseOverBackground", typeof(Brush), typeof(TabControl), new UIPropertyMetadata(null));

        public Brush pgTabItemSelectedBackground
        {
            get { return (Brush)GetValue(pgTabItemSelectedBackgroundProperty); }
            set { SetValue(pgTabItemSelectedBackgroundProperty, value); }
        }
        public static readonly DependencyProperty pgTabItemSelectedBackgroundProperty = DependencyProperty.Register("pgTabItemSelectedBackground", typeof(Brush), typeof(TabControl), new UIPropertyMetadata(null));

        #endregion

        /*
         * Based on the whether the ControlTemplate implements the NewTab button and Close Buttons determines the functionality of the AllowAddNew & AllowDelete properties
         * If they are in the control template, then the visibility of the AddNew & TabItem buttons are bound to these properties
         * 
        */
        /// <summary>
        /// Allow the User to Add New TabItems
        /// </summary>
        public bool AllowAddNew
        {
            get { return (bool)GetValue(AllowAddNewProperty); }
            set { SetValue(AllowAddNewProperty, value); }
        }
        public static readonly DependencyProperty AllowAddNewProperty = DependencyProperty.Register("AllowAddNew", typeof(bool), typeof(TabControl), new UIPropertyMetadata(true));

        /// <summary>
        /// Allow the User to Delete TabItems
        /// </summary>
        public bool AllowDelete
        {
            get { return (bool)GetValue(AllowDeleteProperty); }
            set { SetValue(AllowDeleteProperty, value); }
        }
        public static readonly DependencyProperty AllowDeleteProperty = DependencyProperty.Register("AllowDelete", typeof(bool), typeof(TabControl),
            new FrameworkPropertyMetadata(true, new PropertyChangedCallback(OnAllowDeleteChanged)));

        private static void OnAllowDeleteChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            TabControl tc = (TabControl)d;
            foreach (TabItem item in tc.Items)
                item.AllowDelete = (bool)e.NewValue;
        }

        /// <summary>
        /// Set new TabItem as the current selection
        /// </summary>
        public bool SelectNewTabOnCreate
        {
            get { return (bool)GetValue(SelectNewTabOnCreateProperty); }
            set { SetValue(SelectNewTabOnCreateProperty, value); }
        }
        public static readonly DependencyProperty SelectNewTabOnCreateProperty = DependencyProperty.Register("SelectNewTabOnCreate", typeof(bool), typeof(TabControl), new UIPropertyMetadata(true));


        /// <summary>
        /// Determines where new TabItems are added to the TabControl
        /// </summary>
        /// <remarks>
        ///     Set to true (default) to add all new Tabs to the end of the TabControl
        ///     Set to False to insert new tabs after the current selection
        /// </remarks>
        public bool AddNewTabToEnd
        {
            get { return (bool)GetValue(AddNewTabToEndProperty); }
            set { SetValue(AddNewTabToEndProperty, value); }
        }
        public static readonly DependencyProperty AddNewTabToEndProperty = DependencyProperty.Register("AddNewTabToEnd", typeof(bool), typeof(TabControl), new UIPropertyMetadata(true));

        /// <summary>
        /// defines the Minimum width of a TabItem
        /// </summary>
        [DefaultValue(80)]
        [Category("Layout")]
        [Description("Gets or Sets the minimum Width Constraint shared by all Items in the Control, individual child elements MinWidth property will overide this property")]
        public double TabItemMinWidth
        {
            get { return (double)GetValue(TabItemMinWidthProperty); }
            set { SetValue(TabItemMinWidthProperty, value); }
        }
        public static readonly DependencyProperty TabItemMinWidthProperty = DependencyProperty.Register("TabItemMinWidth", typeof(double), typeof(TabControl),
            new FrameworkPropertyMetadata(80.0, new PropertyChangedCallback(OnMinMaxChanged), new CoerceValueCallback(CoerceMinWidth)));

        private static object CoerceMinWidth(DependencyObject d, object value)
        {
            TabControl tc = (TabControl)d;
            double newValue = (double)value;

            if (newValue > tc.TabItemMaxWidth)
                return tc.TabItemMaxWidth;

            return (newValue > 0 ? newValue : 0);
        }

        /// <summary>
        /// defines the Minimum height of a TabItem
        /// </summary>
        [DefaultValue(20)]
        [Category("Layout")]
        [Description("Gets or Sets the minimum Height Constraint shared by all Items in the Control, individual child elements MinHeight property will override this value")]
        public double TabItemMinHeight
        {
            get { return (double)GetValue(TabItemMinHeightProperty); }
            set { SetValue(TabItemMinHeightProperty, value); }
        }
        public static readonly DependencyProperty TabItemMinHeightProperty = DependencyProperty.Register("TabItemMinHeight", typeof(double), typeof(TabControl),
            new FrameworkPropertyMetadata(30.0, new PropertyChangedCallback(OnMinMaxChanged), new CoerceValueCallback(CoerceMinHeight)));

        private static object CoerceMinHeight(DependencyObject d, object value)
        {
            TabControl tc = (TabControl)d;
            double newValue = (double)value;

            if (newValue > tc.TabItemMaxHeight)
                return tc.TabItemMaxHeight;

            return (newValue > 0 ? newValue : 0);
        }

        /// <summary>
        /// defines the Maximum width of a TabItem
        /// </summary>
        [DefaultValue(double.PositiveInfinity)]
        [Category("Layout")]
        [Description("Gets or Sets the maximum width Constraint shared by all Items in the Control, individual child elements MaxWidth property will override this value")]
        public double TabItemMaxWidth
        {
            get { return (double)GetValue(TabItemMaxWidthProperty); }
            set { SetValue(TabItemMaxWidthProperty, value); }
        }
        public static readonly DependencyProperty TabItemMaxWidthProperty = DependencyProperty.Register("TabItemMaxWidth", typeof(double), typeof(TabControl),
            new FrameworkPropertyMetadata(double.PositiveInfinity, new PropertyChangedCallback(OnMinMaxChanged), new CoerceValueCallback(CoerceMaxWidth)));

        private static object CoerceMaxWidth(DependencyObject d, object value)
        {
            TabControl tc = (TabControl)d;
            double newValue = (double)value;

            if (newValue < tc.TabItemMinWidth)
                return tc.TabItemMinWidth;

            return newValue;
        }

        /// <summary>
        /// defines the Maximum width of a TabItem
        /// </summary>
        [DefaultValue(double.PositiveInfinity)]
        [Category("Layout")]
        [Description("Gets or Sets the maximum height Constraint shared by all Items in the Control, individual child elements MaxHeight property will override this value")]
        public double TabItemMaxHeight
        {
            get { return (double)GetValue(TabItemMaxHeightProperty); }
            set { SetValue(TabItemMaxHeightProperty, value); }
        }
        public static readonly DependencyProperty TabItemMaxHeightProperty = DependencyProperty.Register("TabItemMaxHeight", typeof(double), typeof(TabControl),
            new FrameworkPropertyMetadata(double.PositiveInfinity, new PropertyChangedCallback(OnMinMaxChanged), new CoerceValueCallback(CoerceMaxHeight)));

        private static object CoerceMaxHeight(DependencyObject d, object value)
        {
            TabControl tc = (TabControl)d;
            double newValue = (double)value;

            if (newValue < tc.TabItemMinHeight)
                return tc.TabItemMinHeight;

            return newValue;
        }
        #endregion

        /// <summary>
        /// OnMinMaxChanged callback responds to any of the Min/Max dependancy properties changing
        /// </summary>
        /// <param name="d"></param>
        /// <param name="e"></param>
        private static void OnMinMaxChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            try
            {
                TabControl tc = (TabControl)d;
                if (tc.Template == null) return;

                VirtualizingTabPanel tp = Helper.FindVirtualizingTabPanel(tc);
                if (tp != null)
                    tp.InvalidateMeasure();
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "OnMinMaxChanged()", "Controls\\VMuktiGrid\\Page\\TabControl.cs");
            }
        }

        /*
         * Protected override methods
         * 
        */

        /// <summary>
        /// OnApplyTemplate override
        /// </summary>
        public override void OnApplyTemplate()
        {
            try
            {
                base.OnApplyTemplate();
                this.AllowDrop = true;
                _RowDefinition0 = this.Template.FindName("RowDefinition0", this) as RowDefinition;
                // set up the event handler for the template parts
                _toggleButton = this.Template.FindName("PART_DropDown", this) as ToggleButton;
                if (_toggleButton != null)
                {
                    // create a context menu for the togglebutton
                    ContextMenu cm = new ContextMenu();
                    cm.PlacementTarget = _toggleButton;
                    cm.Placement = PlacementMode.Bottom;

                    // create a binding between the togglebutton's IsChecked Property
                    // and the Context Menu's IsOpen Property
                    Binding b = new Binding();
                    b.Source = _toggleButton;
                    b.Mode = BindingMode.TwoWay;
                    b.Path = new PropertyPath(ToggleButton.IsCheckedProperty);

                    cm.SetBinding(ContextMenu.IsOpenProperty, b);

                    _toggleButton.ContextMenu = cm;
                    _toggleButton.Checked += DropdownButton_Checked;
                }

                ScrollViewer scrollViewer = this.Template.FindName("PART_ScrollViewer", this) as ScrollViewer;

                // set up event handlers for the RepeatButtons Click event
                RepeatButton repeatLeft = this.Template.FindName("PART_RepeatLeft", this) as RepeatButton;
                if (repeatLeft != null)
                {
                    repeatLeft.Click += delegate
                    {
                        if (scrollViewer != null)
                            scrollViewer.LineLeft();
                        GC.Collect();
                    };
                }

                RepeatButton repeatRight = this.Template.FindName("PART_RepeatRight", this) as RepeatButton;
                if (repeatRight != null)
                {
                    repeatRight.Click += delegate
                    {
                        if (scrollViewer != null)
                            scrollViewer.LineRight();
                        GC.Collect();
                    };
                }

                // set up the event handler for the 'New Tab' Button Click event
                ButtonBase button = this.Template.FindName("PART_NewTabButton", this) as ButtonBase;
                if (button != null)
                {
                    button.Click += delegate
                    {
                        VMukti.App.blnIsTwoPanel = false;
                        TabItem item = new TabItem();
                        item.Width = 250;
                        item.MinWidth = 250;
                        item.MaxWidth = 250;

                        item.OwnerID = VMuktiAPI.VMuktiInfo.CurrentPeer.ID;
                        item.OwnerPageIndex = VMukti.App.pageCounter++;

                        ctlPgTabHeader objPgTabHeader = new ctlPgTabHeader();
                        // objPgTabHeader.Title = "New Page - " + this.Items.Count.ToString();
                        //TextBlock txtBlock = new TextBlock();
                        //txtBlock.Text = "New Tab - " + this.Items.Count.ToString();
                        if (VMuktiAPI.VMuktiInfo.CurrentPeer.CurrentMachineIP == VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName || VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName == "")
                        {
                            objPgTabHeader.Title = "New Page";
                        }
                        else
                        {
                            objPgTabHeader.Title = VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName;
                        }
                        Image imgIcon = new Image();
                        imgIcon.Source = new BitmapImage(new Uri(@"\Skins\Images\VMuktiIcon.ico", UriKind.RelativeOrAbsolute));
                        imgIcon.Height = 16;
                        imgIcon.Width = 16;

                        //item.Header = txtBlock;
                        item.Header = objPgTabHeader;
                        item.Icon = imgIcon;

                        item.Content = NewTabControl();
                        if (VMuktiAPI.VMuktiInfo.CurrentPeer.ID > 0)
                        {
                            item.AddBuddy(VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName);
                            item.SetMaxCounter(0, VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName);
                        }
                        //if (i == -1 || i == this.Items.Count - 1 || AddNewTabToEnd)

                        this.Items.Add(item);
                        //else
                        //this.Items.Insert(++i, item);

                        if (SelectNewTabOnCreate)
                        {
                            SelectedItem = item;

                            VirtualizingTabPanel itemsHost = Helper.FindVirtualizingTabPanel(this);
                            if (itemsHost != null)
                                itemsHost.MakeVisible(item, Rect.Empty);

                            item.Focus();
                        }

                        if (TabItemAdded != null)
                            TabItemAdded(this, new TabItemEventArgs(item));
                    };
                    this.Drop += new DragEventHandler(TabControl_Drop);
                }

                LoadDefaultPage();
                objSendPageInfo = new DelSendPageInfo(SendPageInfo);

            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "OnapplyTemplate()", "Controls\\VMuktiGrid\\Page\\TabControl.cs");
            }
        }

        private void LoadDefaultPage()
        {
            this.Items.Clear();
            ((Button)this.Template.FindName("PART_NewTabButton", this)).RaiseEvent(new RoutedEventArgs(Button.ClickEvent));

            ((VMuktiGrid.CustomMenu.ctlPgTabHeader)((VMuktiGrid.ctlPage.TabItem)this.Items[0]).Header).Title = "Default Page";
            VMuktiGrid.ctlTab.TabItem objtabitem = (VMuktiGrid.ctlTab.TabItem)((VMuktiGrid.ctlTab.TabControl)((VMuktiGrid.ctlPage.TabItem)this.Items[0]).Content).Items[0];
            ((VMuktiGrid.CustomMenu.ctlPgTabHeader)(objtabitem).Header).Title = "Default Tab";

            

            VMukti.Business.ClsModuleCollection objCMC = VMukti.Business.ClsModuleCollection.GetNonAuthenticatedMod();
            if (objCMC != null)
            {
                for (int PCnt = 0; PCnt < objCMC.Count; PCnt++)
                {
                    ClsPermissionCollection objCPC = ClsPermissionCollection.Get_PermissionRefModule(objCMC[PCnt].ModuleId, VMuktiAPI.VMuktiInfo.CurrentPeer.RoleID);
                    int[] arrPermissionValue = new int[objCPC.Count];

                    for (int percount = 0; percount < objCPC.Count; percount++)
                    {
                        arrPermissionValue[percount] = objCPC[percount].PermissionValue;
                    }
                    ((VMuktiGrid.CustomGrid.ctlGrid)((VMuktiGrid.ctlTab.TabItem)((VMuktiGrid.ctlTab.TabControl)((VMuktiGrid.ctlPage.TabItem)this.Items[0]).Content).Items[0]).Content).AddControl(objCMC[PCnt].ModuleId, objCMC[PCnt].ModuleTitle, "False", null, arrPermissionValue, false, "fromDatabase");
                }
            }

            this.UnSetUserID();
        }

        void TabControl_Drop(object sender, DragEventArgs e)
        {
            //e.Handled = true;
            //this.AddBuddy(((VMukti.Presentation.Controls.CtlExpanderItem)e.Data.GetData(typeof(VMukti.Presentation.Controls.CtlExpanderItem))).Caption, this.SelectedIndex);
            try
            {
                e.Handled = true;

                bool blnBuddyType = true;

                if (e.Data.GetData("VMukti.Presentation.Controls.CtlExpanderItem") != null && e.Data.GetData("VMukti.Presentation.Controls.CtlExpanderItem").GetType() == typeof(CtlExpanderItem))
                {
                    CtlExpanderItem elt = e.Data.GetData("VMukti.Presentation.Controls.CtlExpanderItem") as CtlExpanderItem;

                    #region Check whether it is module or buddy dropped

                    string[] strTag = elt.Tag.ToString().Split(',');
                    List<string> lstTag = new List<string>();
                    for (int i = 0; i < strTag.Length; i++)
                    {
                        if (strTag[i] == "ModuleType")
                        {
                            blnBuddyType = false;
                            break;
                        }
                    }

                    #endregion

                    if (blnBuddyType && this.AddBuddy(((VMukti.Presentation.Controls.CtlExpanderItem)e.Data.GetData(typeof(VMukti.Presentation.Controls.CtlExpanderItem))).Caption, this.SelectedIndex))
                    {
                        VMuktiGrid.ctlPage.TabItem objSelectedPage = (VMuktiGrid.ctlPage.TabItem)this.SelectedItem;
                        VMuktiGrid.ctlTab.TabItem objSelectedTab = null;

                        clsPageInfo objPageInfo = new clsPageInfo();
                        objPageInfo.intPageID = ((VMuktiGrid.ctlPage.TabItem)this.SelectedItem).ObjectID;
                        objPageInfo.strPageTitle = ((VMuktiGrid.CustomMenu.ctlPgTabHeader)((VMuktiGrid.ctlPage.TabItem)this.SelectedItem).Header).Title;

                        objPageInfo.intOwnerID = VMuktiAPI.VMuktiInfo.CurrentPeer.ID;
                        //objPageInfo.intOwnerPageIndex = this.SelectedIndex;
                        objPageInfo.intOwnerPageIndex = objSelectedPage.OwnerPageIndex;

                        objPageInfo.strDropType = "OnPage";
                        List<string> lstBuddyList = new List<string>();
                        StackPanel stPageBuddyList = ((ctlMenu)((VMuktiGrid.ctlPage.TabItem)this.SelectedItem).Template.FindName("objMenu", (VMuktiGrid.ctlPage.TabItem)this.SelectedItem)).objEMIBuddyList.objBuddyList.stBuddyPanel;
                        for (int i = 0; i < stPageBuddyList.Children.Count; i++)
                        {
                            lstBuddyList.Add(((VMuktiGrid.Buddy.ctlBuddy)stPageBuddyList.Children[i]).Title);
                        }
                        objPageInfo.straPageBuddies = lstBuddyList.ToArray();

                        ((VMuktiGrid.ctlPage.TabItem)this.SelectedItem).NoOfPODs = 0;

                        List<clsTabInfo> lstTabInfos = new List<clsTabInfo>();
                        for (int tCnt = 0; tCnt < ((VMuktiGrid.ctlTab.TabControl)objSelectedPage.Content).Items.Count; tCnt++)
                        {
                            objSelectedTab = ((VMuktiGrid.ctlTab.TabItem)((VMuktiGrid.ctlTab.TabControl)objSelectedPage.Content).Items[tCnt]);
                            objSelectedTab.NoOfPODs = 0;

                            lstTabInfos.Add(new clsTabInfo());

                            lstTabInfos[lstTabInfos.Count - 1].intTabID = objSelectedTab.ObjectID;
                            lstTabInfos[lstTabInfos.Count - 1].strTabTitle = ((VMuktiGrid.CustomMenu.ctlPgTabHeader)objSelectedTab.Header).Title;

                            StackPanel stTabBuddyList = ((ctlMenu)objSelectedTab.Template.FindName("objMenu", objSelectedTab)).objEMIBuddyList.objBuddyList.stBuddyPanel;
                            lstBuddyList.Clear();
                            for (int i = 0; i < stTabBuddyList.Children.Count; i++)
                            {
                                lstBuddyList.Add(((VMuktiGrid.Buddy.ctlBuddy)stTabBuddyList.Children[i]).Title);
                            }
                            lstTabInfos[lstTabInfos.Count - 1].straTabBuddies = lstBuddyList.ToArray();

                            //lstTabInfos[lstTabInfos.Count - 1].intOwnerTabIndex = tCnt;
                            lstTabInfos[lstTabInfos.Count - 1].intOwnerTabIndex = objSelectedTab.OwnerTabIndex;
                            ColumnDefinitionCollection objcols = ((VMuktiGrid.CustomGrid.ctlGrid)objSelectedTab.Content).DocumentRoot.ColumnDefinitions;
                            RowDefinitionCollection objrows = ((VMuktiGrid.CustomGrid.ctlGrid)objSelectedTab.Content).DocumentRoot.RowDefinitions;
                            //lstTabInfos[lstTabInfos.Count - 1].dblC1Width = ((VMuktiGrid.CustomGrid.ctlGrid)objSelectedTab.Content).LeftPanelContainer.ActualWidth;
                            //lstTabInfos[lstTabInfos.Count - 1].dblC2Width = ((VMuktiGrid.CustomGrid.ctlGrid)objSelectedTab.Content).CentralPanelContainer.ActualWidth;
                            //lstTabInfos[lstTabInfos.Count - 1].dblC3Width = ((VMuktiGrid.CustomGrid.ctlGrid)objSelectedTab.Content).RightPanelContainer.ActualWidth;

                            //lstTabInfos[lstTabInfos.Count - 1].dblC4Height = ((VMuktiGrid.CustomGrid.ctlGrid)objSelectedTab.Content).TopPanelContainer.ActualHeight;
                            //lstTabInfos[lstTabInfos.Count - 1].dblC5Height = ((VMuktiGrid.CustomGrid.ctlGrid)objSelectedTab.Content).BottomPanelContainer.ActualHeight;


                            lstTabInfos[lstTabInfos.Count - 1].dblC1Width = objcols[0].Width.Value;
                            lstTabInfos[lstTabInfos.Count - 1].dblC2Width = objcols[1].Width.Value;
                            lstTabInfos[lstTabInfos.Count - 1].dblC3Width = objcols[2].Width.Value;

                            lstTabInfos[lstTabInfos.Count - 1].dblC4Height = objrows[0].Height.Value;
                            lstTabInfos[lstTabInfos.Count - 1].dblC5Height = objrows[2].Height.Value;


                            List<clsPodInfo> lstPodInfo = new List<clsPodInfo>();
                            VMuktiGrid.CustomGrid.ctlGrid objSelectedGrid = (VMuktiGrid.CustomGrid.ctlGrid)objSelectedTab.Content;

                            for (int pCnt = 0; pCnt < objSelectedGrid.LeftPanelContainer.Items.Count; pCnt++)
                            {
                                if (objSelectedGrid.LeftPanelContainer.Items[pCnt].GetType() == typeof(VMuktiGrid.CustomGrid.ctlPOD))
                                {
                                    lstPodInfo.Add(new clsPodInfo());

                                    fncGetPodInfoForDrop(lstPodInfo, lstPodInfo.Count - 1, ((VMuktiGrid.CustomGrid.ctlPOD)objSelectedGrid.LeftPanelContainer.Items[pCnt]));

                                    lstPodInfo[lstPodInfo.Count - 1].intColumnNumber = 1;


                                    lstBuddyList.Clear();
                                    StackPanel stPodBuddyList = ((VMuktiGrid.CustomGrid.ctlPOD)objSelectedGrid.LeftPanelContainer.Items[pCnt]).objBuddyList.stPanel;
                                    for (int i = 0; i < stPodBuddyList.Children.Count; i++)
                                    {
                                        if (stPodBuddyList.Children[i].GetType() == typeof(VMuktiGrid.Buddy.ctlBuddy))
                                        {
                                            lstBuddyList.Add(((VMuktiGrid.Buddy.ctlBuddy)stPodBuddyList.Children[i]).Title);
                                        }
                                    }
                                    lstPodInfo[lstPodInfo.Count - 1].straPodBuddies = lstBuddyList.ToArray();

                                    ((VMuktiGrid.ctlPage.TabItem)this.SelectedItem).NoOfPODs++;
                                    objSelectedTab.NoOfPODs++;
                                }
                            }

                            for (int pCnt = 0; pCnt < objSelectedGrid.CentralPanelContainer.Items.Count; pCnt++)
                            {
                                if (objSelectedGrid.CentralPanelContainer.Items[pCnt].GetType() == typeof(VMuktiGrid.CustomGrid.ctlPOD))
                                {
                                    lstPodInfo.Add(new clsPodInfo());
                                    fncGetPodInfo(lstPodInfo, lstPodInfo.Count - 1, ((VMuktiGrid.CustomGrid.ctlPOD)objSelectedGrid.CentralPanelContainer.Items[pCnt]));


                                    lstPodInfo[lstPodInfo.Count - 1].intColumnNumber = 2;

                                    lstBuddyList.Clear();
                                    StackPanel stPodBuddyList = ((VMuktiGrid.CustomGrid.ctlPOD)objSelectedGrid.CentralPanelContainer.Items[pCnt]).objBuddyList.stPanel;
                                    for (int i = 0; i < stPodBuddyList.Children.Count; i++)
                                    {
                                        if (stPodBuddyList.Children[i].GetType() == typeof(VMuktiGrid.Buddy.ctlBuddy))
                                        {
                                            lstBuddyList.Add(((VMuktiGrid.Buddy.ctlBuddy)stPodBuddyList.Children[i]).Title);
                                        }
                                    }
                                    lstPodInfo[lstPodInfo.Count - 1].straPodBuddies = lstBuddyList.ToArray();

                                    ((VMuktiGrid.ctlPage.TabItem)this.SelectedItem).NoOfPODs++;
                                    objSelectedTab.NoOfPODs++;
                                }
                            }

                            for (int pCnt = 0; pCnt < objSelectedGrid.RightPanelContainer.Items.Count; pCnt++)
                            {
                                if (objSelectedGrid.RightPanelContainer.Items[pCnt].GetType() == typeof(VMuktiGrid.CustomGrid.ctlPOD))
                                {
                                    lstPodInfo.Add(new clsPodInfo());
                                    fncGetPodInfo(lstPodInfo, lstPodInfo.Count - 1, ((VMuktiGrid.CustomGrid.ctlPOD)objSelectedGrid.RightPanelContainer.Items[pCnt]));

                                    lstPodInfo[lstPodInfo.Count - 1].intColumnNumber = 3;

                                    lstBuddyList.Clear();
                                    StackPanel stPodBuddyList = ((VMuktiGrid.CustomGrid.ctlPOD)objSelectedGrid.RightPanelContainer.Items[pCnt]).objBuddyList.stPanel;
                                    for (int i = 0; i < stPodBuddyList.Children.Count; i++)
                                    {
                                        if (stPodBuddyList.Children[i].GetType() == typeof(VMuktiGrid.Buddy.ctlBuddy))
                                        {
                                            lstBuddyList.Add(((VMuktiGrid.Buddy.ctlBuddy)stPodBuddyList.Children[i]).Title);
                                        }
                                    }
                                    lstPodInfo[lstPodInfo.Count - 1].straPodBuddies = lstBuddyList.ToArray();

                                    ((VMuktiGrid.ctlPage.TabItem)this.SelectedItem).NoOfPODs++;
                                    objSelectedTab.NoOfPODs++;
                                }
                            }

                            for (int pCnt = 0; pCnt < objSelectedGrid.TopPanelContainer.Items.Count; pCnt++)
                            {
                                if (objSelectedGrid.TopPanelContainer.Items[pCnt].GetType() == typeof(VMuktiGrid.CustomGrid.ctlPOD))
                                {
                                    lstPodInfo.Add(new clsPodInfo());
                                    fncGetPodInfo(lstPodInfo, lstPodInfo.Count - 1, ((VMuktiGrid.CustomGrid.ctlPOD)objSelectedGrid.TopPanelContainer.Items[pCnt]));
                                    lstPodInfo[lstPodInfo.Count - 1].intColumnNumber = 4;

                                    lstBuddyList.Clear();
                                    StackPanel stPodBuddyList = ((VMuktiGrid.CustomGrid.ctlPOD)objSelectedGrid.TopPanelContainer.Items[pCnt]).objBuddyList.stPanel;
                                    for (int i = 0; i < stPodBuddyList.Children.Count; i++)
                                    {
                                        if (stPodBuddyList.Children[i].GetType() == typeof(VMuktiGrid.Buddy.ctlBuddy))
                                        {
                                            lstBuddyList.Add(((VMuktiGrid.Buddy.ctlBuddy)stPodBuddyList.Children[i]).Title);
                                        }
                                    }
                                    lstPodInfo[lstPodInfo.Count - 1].straPodBuddies = lstBuddyList.ToArray();

                                    ((VMuktiGrid.ctlPage.TabItem)this.SelectedItem).NoOfPODs++;
                                    objSelectedTab.NoOfPODs++;
                                }
                            }

                            for (int pCnt = 0; pCnt < objSelectedGrid.BottomPanelContainer.Items.Count; pCnt++)
                            {
                                if (objSelectedGrid.BottomPanelContainer.Items[pCnt].GetType() == typeof(VMuktiGrid.CustomGrid.ctlPOD))
                                {
                                    lstPodInfo.Add(new clsPodInfo());
                                    fncGetPodInfo(lstPodInfo, lstPodInfo.Count - 1, ((VMuktiGrid.CustomGrid.ctlPOD)objSelectedGrid.BottomPanelContainer.Items[pCnt]));
                                    //VMuktiGrid.CustomGrid.ctlPOD objpod = ((VMuktiGrid.CustomGrid.ctlPOD)objSelectedGrid.BottomPanelContainer.Items[pCnt]);
                                    lstPodInfo[lstPodInfo.Count - 1].intColumnNumber = 5;

                                    lstBuddyList.Clear();
                                    StackPanel stPodBuddyList = ((VMuktiGrid.CustomGrid.ctlPOD)objSelectedGrid.BottomPanelContainer.Items[pCnt]).objBuddyList.stPanel;
                                    for (int i = 0; i < stPodBuddyList.Children.Count; i++)
                                    {
                                        if (stPodBuddyList.Children[i].GetType() == typeof(VMuktiGrid.Buddy.ctlBuddy))
                                        {
                                            lstBuddyList.Add(((VMuktiGrid.Buddy.ctlBuddy)stPodBuddyList.Children[i]).Title);
                                        }
                                    }
                                    lstPodInfo[lstPodInfo.Count - 1].straPodBuddies = lstBuddyList.ToArray();

                                    ((VMuktiGrid.ctlPage.TabItem)this.SelectedItem).NoOfPODs++;
                                    objSelectedTab.NoOfPODs++;
                                }
                            }

                            lstTabInfos[lstTabInfos.Count - 1].objaPods = lstPodInfo.ToArray();
                            objSelectedTab.SetMaxCounter(objSelectedTab.NoOfPODs, ((VMukti.Presentation.Controls.CtlExpanderItem)e.Data.GetData(typeof(VMukti.Presentation.Controls.CtlExpanderItem))).Caption);
                        }
                        objPageInfo.objaTabs = lstTabInfos.ToArray();

                        objPageInfo.strFrom = VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName;
                        objPageInfo.strTo = ((VMukti.Presentation.Controls.CtlExpanderItem)e.Data.GetData(typeof(VMukti.Presentation.Controls.CtlExpanderItem))).Caption;
                        objPageInfo.strMsg = "OPEN_PAGE";

                        this.SetMaxCounter(((VMuktiGrid.ctlPage.TabItem)this.SelectedItem).NoOfPODs, ((VMukti.Presentation.Controls.CtlExpanderItem)e.Data.GetData(typeof(VMukti.Presentation.Controls.CtlExpanderItem))).Caption);

                        if (VMuktiAPI.VMuktiInfo.CurrentPeer.CurrPeerType == VMuktiAPI.PeerType.NodeWithNetP2P || VMuktiAPI.VMuktiInfo.CurrentPeer.CurrPeerType == VMuktiAPI.PeerType.BootStrap || VMuktiAPI.VMuktiInfo.CurrentPeer.CurrPeerType == VMuktiAPI.PeerType.SuperNode)
                        {
                            VMukti.App.chNetP2PSuperNodeChannel.svcSetSpecialMsg(objPageInfo, VMuktiAPI.VMuktiInfo.CurrentPeer.SuperNodeIP);
                        }
                        else
                        {
                            try
                            {
                                VMukti.App.chHttpSuperNodeService.svcSetSpecialMsgs(objPageInfo);
                            }
                            catch (System.ServiceModel.EndpointNotFoundException ex)
                            {
                                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "TabControl_Drop()", "Controls\\VMuktiGrid\\Page\\TabControl.cs");
                                VMuktiAPI.VMuktiHelper.CallEvent("GetSuperNodeIP", null, null);

                                VMukti.App.chHttpSuperNodeService.svcSetSpecialMsgs(objPageInfo);
                            }
                            catch (System.ServiceModel.CommunicationException ex)
                            {
                                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "TabControl_Drop()", "Controls\\VMuktiGrid\\Page\\TabControl.cs");
                                VMuktiAPI.VMuktiHelper.CallEvent("GetSuperNodeIP", null, null);

                                VMukti.App.chHttpSuperNodeService.svcSetSpecialMsgs(objPageInfo);
                            }
                        }

                        //((ctlVMuktiGrid)((Grid)((VMuktiGrid.ctlPage.TabControl)((VMuktiGrid.ctlPage.TabItem)this.Parent).Parent).Parent).Parent).fncChannelSetPageMsg(VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName, ((VMukti.Presentation.Controls.CtlExpanderItem)e.Data.GetData(typeof(VMukti.Presentation.Controls.CtlExpanderItem))).Caption, "OPEN_PAGE", lstModuleInfo);
                        //VMukti.App.chNetP2PSuperNodeChannel.svcAddDraggedBuddy
                    }
                }
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "TabControl_Drop()", "Controls\\VMuktiGrid\\Page\\TabControl.cs");
            }
        }

        public ctlTab.TabControl NewTabControl()
        {
            try
            {
                ctlTab.TabControl objTabControl = new ctlTab.TabControl();
                objTabControl.SetValue(Grid.RowProperty, 1);
                objTabControl.TabItemMinWidth = 150;
                objTabControl.TabItemMaxWidth = 300;
                objTabControl.TabItemMinHeight = 30;
                objTabControl.TabItemMaxHeight = 50;
                objTabControl.VerticalAlignment = VerticalAlignment.Stretch;
                objTabControl.Margin = new Thickness(5);

                ctlTab.TabItem objTabItem = new ctlTab.TabItem();
                objTabItem.Width = 250;
                objTabItem.MinWidth = 250;
                objTabItem.MaxWidth = 250;
                objTabItem.OwnerTabIndex = VMukti.App.tabCounter++;
                ctlPgTabHeader objPgTabHeader = new ctlPgTabHeader();
                objPgTabHeader.Title = "(Untitled)";
                objTabItem.Content = new CustomGrid.ctlGrid();

                //objTabItem.Header = txtTabBlock;
                objTabItem.Header = objPgTabHeader;
                //objTabItem.Icon = imgTabIcon;

                objTabControl.Items.Add(objTabItem);


                return objTabControl;
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "NweTabControl()", "Controls\\VMuktiGrid\\Page\\TabControl.cs");
                return null;
            }
        }

        /// <summary>
        /// IsItemItsOwnContainerOverride
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        protected override bool IsItemItsOwnContainerOverride(object item)
        {
            return item is ctlPage.TabItem;
        }
        /// <summary>
        /// GetContainerForItemOverride
        /// </summary>
        /// <returns></returns>
        protected override DependencyObject GetContainerForItemOverride()
        {
            return new ctlPage.TabItem();
        }

        /// <summary>
        /// Handle the ToggleButton Checked event that displays a context menu of TabItem Headers
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// Look this place for the error, button before new page or tabs
        void DropdownButton_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
                if (_toggleButton == null) return;

                _toggleButton.ContextMenu.Items.Clear();
                if (TabStripPlacement == Dock.Bottom)
                    _toggleButton.ContextMenu.Placement = PlacementMode.Top;
                else
                    _toggleButton.ContextMenu.Placement = PlacementMode.Bottom;

                for (int i = 0; i < Items.Count; i++)
                {
                    TabItem item = this.Items[i] as TabItem;
                    if (item == null)
                        return;

                    //object header = Helper.CloneElement(item.Header);
                    //object icon = Helper.CloneElement(item.Icon);

                    //MenuItem mi = new MenuItem() { Header = header, Icon = icon, Tag = i.ToString() };

                    MenuItem mi = new MenuItem() { Header = ((ctlPgTabHeader)item.Header).Title, Tag = i.ToString() }; mi.Click += ContextMenuItem_Click;

                    _toggleButton.ContextMenu.Items.Add(mi);
                }
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "DropDownButton_Checked()", "Controls\\VMuktiGrid\\Page\\TabControl.cs");
            }
        }

        protected override void OnPreviewKeyDown(KeyEventArgs e)
        {
            try
            {
                if (Items.Count == 0)
                    return;

                TabItem ti = null;

                switch (e.Key)
                {
                    case Key.Home:
                        ti = this.Items[0] as TabItem;
                        break;

                    case Key.End:
                        ti = this.Items[Items.Count - 1] as TabItem;
                        break;

                    case Key.Tab:
                        if (e.KeyboardDevice.Modifiers == ModifierKeys.Control)
                        {
                            int index = SelectedIndex;
                            int direction = 1;
                            if (e.KeyboardDevice.Modifiers == ModifierKeys.Shift)
                                direction = -1;

                            while (true)
                            {
                                index += direction;
                                if (index < 0)
                                    index = Items.Count - 1;
                                else if (index > Items.Count - 1)
                                    index = 0;

                                FrameworkElement ui = Items[index] as FrameworkElement;
                                if (ui.Visibility == Visibility.Visible && ui.IsEnabled)
                                {
                                    ti = Items[index] as TabItem;
                                    break;
                                }
                            }
                        }
                        break;
                }

                VirtualizingTabPanel panel = Helper.FindVirtualizingTabPanel(this);
                if (panel != null && ti != null)
                {
                    panel.MakeVisible(ti, Rect.Empty);
                    SelectedItem = ti;

                    e.Handled = ti.Focus();
                }
                if (!e.Handled)
                    base.OnPreviewKeyDown(e);
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "OnPreviewKeyDown()", "Controls\\VMuktiGrid\\Page\\TabControl.cs");
            }
        }

        /// <summary>
        /// Handle the MenuItem's Click event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void ContextMenuItem_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                MenuItem mi = sender as MenuItem;
                if (mi == null) return;

                int index;
                // get the index of the TabItem from the manuitems Tag property
                bool b = int.TryParse(mi.Tag.ToString(), out index);

                if (b)
                {
                    TabItem tabItem = this.Items[index] as TabItem;
                    if (tabItem != null)
                    {
                        VirtualizingTabPanel itemsHost = Helper.FindVirtualizingTabPanel(this);
                        if (itemsHost != null)
                            itemsHost.MakeVisible(tabItem, Rect.Empty);

                        tabItem.Focus();
                    }
                }
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "ContectMenuItem_Click()", "Controls\\VMuktiGrid\\Page\\TabControl.cs");
            }
        }

        /// <summary>
        /// Called by a child TabItem that wants to remove itself by clicking on the close button
        /// </summary>
        /// <param name="item"></param>
        internal void RemoveItem(TabItem item)
        {
            try
            {
                // gives an opertunity to cancel the removal of the tabitem
                TabItemCancelEventArgs c = new TabItemCancelEventArgs(item);
                if (TabItemClosing != null)
                    TabItemClosing(item, c);

                if (c.Cancel == true)
                    return;

                this.Items.Remove(item);

                if (TabItemClosed != null)
                    TabItemClosed(this, new TabItemEventArgs(item));
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "RemoveItem()", "Controls\\VMuktiGrid\\Page\\TabControl.cs");
            }
        }

        public bool AddBuddy(string strBuddyName, int tabItemIndex)
        {
            try
            {
                return ((ctlPage.TabItem)this.Items[tabItemIndex]).AddBuddy(strBuddyName);
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "AddBuddy()", "Controls\\VMuktiGrid\\Page\\TabControl.cs");
                return false;
            }
        }

        public void SetMaxCounter(int intMaxCounter, string strBuddyName)
        {
            try
            {
                ((ctlPage.TabItem)this.Items[this.SelectedIndex]).SetMaxCounter(intMaxCounter, strBuddyName);
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "SetMaXCounter()", "Controls\\VMuktiGrid\\Page\\TabControl.cs");
            }
        }

        public bool CheckBuddy(string strBuddyName, int tabItemIndex)
        {
            try
            {
                return ((ctlPage.TabItem)this.Items[tabItemIndex]).CheckBuddy(strBuddyName);
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "CheckBuddy()", "Controls\\VMuktiGrid\\Page\\TabControl.cs");
                return false;
            }
        }

        public void LoadPage()
        {
            try
            {
                TabItem item = new TabItem();
                item.Width = 250;
                item.MinWidth = 250;
                item.MaxWidth = 250;

                item.OwnerID = VMuktiAPI.VMuktiInfo.CurrentPeer.ID;
                item.OwnerPageIndex = VMukti.App.pageCounter++;

                ctlPgTabHeader objPgTabHeader = new ctlPgTabHeader();
                if (VMuktiAPI.VMuktiInfo.CurrentPeer.CurrentMachineIP == VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName || VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName == "")
                {
                    objPgTabHeader.Title = "New Page";
                }
                else
                {
                    objPgTabHeader.Title = "Instant Meeting - "+VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName;
                }
                Image imgIcon = new Image();
                imgIcon.Source = new BitmapImage(new Uri(@"\Skins\Images\VMuktiIcon.ico", UriKind.RelativeOrAbsolute));
                imgIcon.Height = 16;
                imgIcon.Width = 16;

                item.Header = objPgTabHeader;
                item.Icon = imgIcon;

                item.Content = NewTabControl();
                if (VMuktiAPI.VMuktiInfo.CurrentPeer.ID > 0)
                {
                    item.AddBuddy(VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName);
                    item.SetMaxCounter(0, VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName);
                }

                this.Items.Add(item);


                if (SelectNewTabOnCreate)
                {
                    SelectedItem = item;

                    VirtualizingTabPanel itemsHost = Helper.FindVirtualizingTabPanel(this);
                    if (itemsHost != null)
                        itemsHost.MakeVisible(item, Rect.Empty);

                    item.Focus();
                }

                if (TabItemAdded != null)
                    TabItemAdded(this, new TabItemEventArgs(item));

                this.Drop += new DragEventHandler(TabControl_Drop);
               item.Loaded+=new RoutedEventHandler(item_Loaded);
            }

            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "LoadPage()", "Controls\\VMuktiGrid\\Page\\TabControl.cs");
            }
        }


        public void LoadPage(int pageID)
        {
            try
            {
                VMukti.Business.VMuktiGrid.ClsPage objPage = VMukti.Business.VMuktiGrid.ClsPage.Get_PageInfo(pageID);
                if (objPage != null)
                {
                    TabItem item = new TabItem();
                    item.ObjectID = pageID;
                    item.IsSaved = true;
                    item.OwnerID = VMuktiAPI.VMuktiInfo.CurrentPeer.ID;
                    item.OwnerPageIndex = VMukti.App.pageCounter++;

                    item.Width = 250;
                    item.MinWidth = 250;
                    item.MaxWidth = 250;

                    ctlPgTabHeader objPgTabHeader = new ctlPgTabHeader();
                    objPgTabHeader.Title = objPage.PageTitle;
                    //TextBlock txtBlock = new TextBlock();
                    //txtBlock.Text = "New Tab - " + this.Items.Count.ToString();

                    //Image imgIcon = new Image();
                    //imgIcon.Source = new BitmapImage(new Uri(@"\Skins\Images\VMuktiIcon.ico", UriKind.RelativeOrAbsolute));
                    //imgIcon.Height = 16;
                    //imgIcon.Width = 16;

                    //item.Header = txtBlock;
                    item.Header = objPgTabHeader;
                    // item.Icon = imgIcon;

                    item.Content = NewTabControl(pageID);

                    //if (i == -1 || i == this.Items.Count - 1 || AddNewTabToEnd)
                    this.Items.Add(item);
                    //else
                    //this.Items.Insert(++i, item);

                    if (SelectNewTabOnCreate)
                    {
                        SelectedItem = item;

                        VirtualizingTabPanel itemsHost = Helper.FindVirtualizingTabPanel(this);
                        if (itemsHost != null)
                            itemsHost.MakeVisible(item, Rect.Empty);

                        item.Focus();
                    }

                    if (TabItemAdded != null)
                        TabItemAdded(this, new TabItemEventArgs(item));

                    #region Permission of rename or delet the page
                    if (pageID == 1 || pageID == 2)
                    {
                        item.CanDelete = false;
                        item.CanRename = false;
                        item.IsSaved = true;
                    }
                    #endregion
                }
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "LoadPage()", "Controls\\VMuktiGrid\\Page\\TabControl.cs");
            }
        }

        public void LoadPage(int pageID, int confID)
        {
            try
            {
                VMukti.Business.VMuktiGrid.ClsPage objPage = VMukti.Business.VMuktiGrid.ClsPage.Get_PageInfo(pageID);

                TabItem item = new TabItem();
                item.ObjectID = pageID;
                item.IsSaved = true;
                item.OwnerID = VMuktiAPI.VMuktiInfo.CurrentPeer.ID;
                item.OwnerPageIndex = VMukti.App.pageCounter++;



                item.ConfID = confID;


                item.Width = 250;
                item.MinWidth = 250;
                item.MaxWidth = 250;

                ctlPgTabHeader objPgTabHeader = new ctlPgTabHeader();
                objPgTabHeader.Title = objPage.PageTitle;
                //TextBlock txtBlock = new TextBlock();
                //txtBlock.Text = "New Tab - " + this.Items.Count.ToString();

                //Image imgIcon = new Image();
                //imgIcon.Source = new BitmapImage(new Uri(@"\Skins\Images\VMuktiIcon.ico", UriKind.RelativeOrAbsolute));
                //imgIcon.Height = 16;
                //imgIcon.Width = 16;

                //item.Header = txtBlock;
                item.Header = objPgTabHeader;
                //  item.Icon = imgIcon;

                item.Content = NewTabControl(pageID);

                //this.ConfID = confID;
                //if (i == -1 || i == this.Items.Count - 1 || AddNewTabToEnd)
                #region Permission of rename or delet the page
                if (pageID == 1 || pageID == 2)
                {
                    item.CanDelete = false;
                    item.CanRename = false;

                }
                #endregion
                this.Items.Add(item);

                //else
                //this.Items.Insert(++i, item);

                if (SelectNewTabOnCreate)
                {
                   // SelectedItem = item;

                    VirtualizingTabPanel itemsHost = Helper.FindVirtualizingTabPanel(this);
                    if (itemsHost != null)
                        itemsHost.MakeVisible(item, Rect.Empty);

                   // item.Focus();
                }

                if (TabItemAdded != null)
                    TabItemAdded(this, new TabItemEventArgs(item));

                item.Focus();
                item.Loaded += new RoutedEventHandler(item_Loaded);
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "LoadPage--2()", "Controls\\VMuktiGrid\\Page\\TabControl.cs");
            }
        }

        public void MovePodToCenter(int pageid, int podid)
        {
            for (int ItemCnt = 0; ItemCnt < this.Items.Count; ItemCnt++)
            {
                
            }
        }

        void item_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                e.Handled = true;
                ((VMuktiGrid.ctlPage.TabItem)e.Source).Focus();
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "item_Loaded()", "Controls\\VMuktiGrid\\Page\\TabControl.cs");               
            }
        }

        public ctlTab.TabControl NewTabControl(int pageID)
        {
            try
            {
                ctlTab.TabControl objTabControl = new ctlTab.TabControl();
                objTabControl.SetValue(Grid.RowProperty, 1);
                objTabControl.TabItemMinWidth = 150;
                objTabControl.TabItemMaxWidth = 300;
                objTabControl.TabItemMinHeight = 30;
                objTabControl.TabItemMaxHeight = 50;
                objTabControl.VerticalAlignment = VerticalAlignment.Stretch;
                objTabControl.Margin = new Thickness(5);

                VMukti.Business.VMuktiGrid.ClsTabCollection objTabs = VMukti.Business.VMuktiGrid.ClsTabCollection.GetAll(pageID);
                foreach (VMukti.Business.VMuktiGrid.ClsTab objTab in objTabs)
                {
                    ctlTab.TabItem objTabItem = new ctlTab.TabItem();
                    objTabItem.Width = 250;
                    objTabItem.MinWidth = 250;
                    objTabItem.MaxWidth = 250;
                    objTabItem.OwnerTabIndex = VMukti.App.tabCounter++;
                    //TextBlock txtTabBlock = new TextBlock();
                    //txtTabBlock.Text = "New Tab - 0";
                    objTabItem.ObjectID = objTab.TabId;
                    objTabItem.IsSaved = true;

                    ctlPgTabHeader objPgTabHeader = new ctlPgTabHeader();
                    objPgTabHeader.Title = objTab.TabTitle;

                    //Image imgTabIcon = new Image();
                    //imgTabIcon.Source = new BitmapImage(new Uri(@"\Skins\Images\VMuktiIcon.ico", UriKind.RelativeOrAbsolute));
                    //imgTabIcon.Height = 16;
                    //imgTabIcon.Width = 16;

                    objTabItem.Content = new CustomGrid.ctlGrid();
                    ((CustomGrid.ctlGrid)objTabItem.Content).LoadGrid(objTab.TabId);

                    //objTabItem.Header = txtTabBlock;
                    objTabItem.Header = objPgTabHeader;
                    //objTabItem.Icon = imgTabIcon;

                    objTabControl.Items.Add(objTabItem);

                }
                return objTabControl;
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "NewTabControl()", "Controls\\VMuktiGrid\\Page\\TabControl.cs");
                return null;
            }
        }

        public void LoadMeetingPage(clsPageInfo objPageInfo, int pageIndex)
        {
            try
            {
                VMuktiGrid.ctlPage.TabItem selectedPage = (VMuktiGrid.ctlPage.TabItem)this.Items[pageIndex];
                ((ctlPgTabHeader)selectedPage.Header).Title = objPageInfo.strPageTitle;
                if (objPageInfo.ConfID != 0)
                {
                    selectedPage.ConfID = objPageInfo.ConfID;
                }

                int i = 0;
                int j = 0;

                for (i = 0; i < objPageInfo.objaTabs.Length; i++)
                {
                    for (j = 0; j < ((VMuktiGrid.ctlTab.TabControl)selectedPage.Content).Items.Count; j++)
                    {
                        if (((VMuktiGrid.ctlTab.TabItem)((VMuktiGrid.ctlTab.TabControl)selectedPage.Content).Items[j]).OwnerTabIndex == objPageInfo.objaTabs[i].intOwnerTabIndex)
                        {
                            ((VMuktiGrid.CustomGrid.ctlGrid)((VMuktiGrid.ctlTab.TabItem)((VMuktiGrid.ctlTab.TabControl)selectedPage.Content).Items[j]).Content).LoadMeetingGrid(objPageInfo, i);
                            ((VMuktiGrid.ctlTab.TabItem)((VMuktiGrid.ctlTab.TabControl)selectedPage.Content).Items[j]).StartBlinking();
                            break;
                        }
                    }
                    if (j == ((VMuktiGrid.ctlTab.TabControl)selectedPage.Content).Items.Count)
                    {
                        ctlTab.TabItem objTabItem = new ctlTab.TabItem();
                        objTabItem.Width = 250;
                        objTabItem.MinWidth = 250;
                        objTabItem.MaxWidth = 250;
                        objTabItem.OwnerTabIndex = objPageInfo.objaTabs[i].intOwnerTabIndex;
                        //TextBlock txtTabBlock = new TextBlock();
                        //txtTabBlock.Text = "New Tab - 0";
                        objTabItem.ObjectID = int.MinValue;
                        objTabItem.IsSaved = true;

                        ctlPgTabHeader objPgTabHeader = new ctlPgTabHeader();
                        objPgTabHeader.Title = objPageInfo.objaTabs[i].strTabTitle;

                        //Image imgTabIcon = new Image();
                        //imgTabIcon.Source = new BitmapImage(new Uri(@"\Skins\Images\VMuktiIcon.ico", UriKind.RelativeOrAbsolute));
                        //imgTabIcon.Height = 16;
                        //imgTabIcon.Width = 16;

                        objTabItem.Content = new CustomGrid.ctlGrid();
                        ((CustomGrid.ctlGrid)objTabItem.Content).LoadNewMeetingGrid(objPageInfo, i);

                        //objTabItem.Header = txtTabBlock;
                        objTabItem.Header = objPgTabHeader;
                        //objTabItem.Icon = imgTabIcon;

                        ((VMuktiGrid.ctlTab.TabControl)selectedPage.Content).Items.Add(objTabItem);
                        objTabItem.StartBlinking();

                    }
                }
                selectedPage.StartBlinking();
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "LoadMeetingPage()", "Controls\\VMuktiGrid\\Page\\TabControl.cs");
            }
        }

        public void LoadNewMeetingPage(clsPageInfo objPageInfo)
        {
            try
            {
                TabItem item = new TabItem();
                item.ObjectID = objPageInfo.intPageID;
                item.IsSaved = false;
                item.OwnerID = objPageInfo.intOwnerID;
                item.OwnerPageIndex = objPageInfo.intOwnerPageIndex;

                item.Width = 250;
                item.MinWidth = 250;
                item.MaxWidth = 250;
                if (objPageInfo.ConfID != 0)
                {
                    item.ConfID = objPageInfo.ConfID;
                    item.Loaded+=new RoutedEventHandler(item_Loaded);
                }

                ctlPgTabHeader objPgTabHeader = new ctlPgTabHeader();
                objPgTabHeader.Title = objPageInfo.strPageTitle;
                //TextBlock txtBlock = new TextBlock();
                //txtBlock.Text = "New Tab - " + this.Items.Count.ToString();

                //Image imgIcon = new Image();
                //imgIcon.Source = new BitmapImage(new Uri(@"\Skins\Images\VMuktiIcon.ico", UriKind.RelativeOrAbsolute));
                //imgIcon.Height = 16;
                //imgIcon.Width = 16;

                //item.Header = txtBlock;
                item.Header = objPgTabHeader;
                //  item.Icon = imgIcon;

                item.Content = NewTabControl(objPageInfo);

                if (objPageInfo.strDropType == "OnPage")
                {
                    int intMaxCounter = 0;

                    foreach (clsTabInfo objTabInfo in objPageInfo.objaTabs)
                    {
                        intMaxCounter += objTabInfo.objaPods.Length;
                    }

                    for (int i = 0; i < objPageInfo.straPageBuddies.Length; i++)
                    {
                        item.AddBuddy(objPageInfo.straPageBuddies[i]);
                        item.SetMaxCounter(intMaxCounter, objPageInfo.straPageBuddies[i]);
                    }
                    item.AddBuddy(objPageInfo.strFrom);
                    item.SetMaxCounter(intMaxCounter, objPageInfo.strFrom);
                }

                //if (i == -1 || i == this.Items.Count - 1 || AddNewTabToEnd)
                this.Items.Add(item);
                //else
                //this.Items.Insert(++i, item);

                if (SelectNewTabOnCreate)
                {
                   // SelectedItem = item;

                    VirtualizingTabPanel itemsHost = Helper.FindVirtualizingTabPanel(this);
                    if (itemsHost != null)
                        itemsHost.MakeVisible(item, Rect.Empty);

                  //  item.Focus();
                }

                if (TabItemAdded != null)
                    TabItemAdded(this, new TabItemEventArgs(item));              

                item.StartBlinking();
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "LoadNewMeeting()", "Controls\\VMuktiGrid\\Page\\TabControl.cs");
            }
        }

        public ctlTab.TabControl NewTabControl(clsPageInfo objPageInfo)
        {
            try
            {
                ctlTab.TabControl objTabControl = new ctlTab.TabControl();

                objTabControl.AllowAddNew = false;

                objTabControl.SetValue(Grid.RowProperty, 1);
                objTabControl.TabItemMinWidth = 150;
                objTabControl.TabItemMaxWidth = 300;
                objTabControl.TabItemMinHeight = 30;
                objTabControl.TabItemMaxHeight = 50;
                objTabControl.VerticalAlignment = VerticalAlignment.Stretch;
                objTabControl.Margin = new Thickness(5);

                for (int i = 0; i < objPageInfo.objaTabs.Length; i++)
                {
                    clsTabInfo objTab = objPageInfo.objaTabs[i];

                    ctlTab.TabItem objTabItem = new ctlTab.TabItem();
                    objTabItem.Width = 250;
                    objTabItem.MinWidth = 250;
                    objTabItem.MaxWidth = 250;
                    objTabItem.OwnerTabIndex = objTab.intOwnerTabIndex;
                    //TextBlock txtTabBlock = new TextBlock();
                    //txtTabBlock.Text = "New Tab - 0";
                    objTabItem.ObjectID = objPageInfo.objaTabs[i].intTabID;
                    objTabItem.IsSaved = true;

                    ctlPgTabHeader objPgTabHeader = new ctlPgTabHeader();
                    objPgTabHeader.Title = objTab.strTabTitle;

                    Image imgTabIcon = new Image();
                    imgTabIcon.Source = new BitmapImage(new Uri(@"\Skins\Images\VMuktiIcon.ico", UriKind.RelativeOrAbsolute));
                    imgTabIcon.Height = 16;
                    imgTabIcon.Width = 16;

                    objTabItem.Content = new CustomGrid.ctlGrid();
                    ((CustomGrid.ctlGrid)objTabItem.Content).LoadNewMeetingGrid(objPageInfo, i);

                    //objTabItem.Header = txtTabBlock;
                    objTabItem.Header = objPgTabHeader;
                    objTabItem.Icon = imgTabIcon;
                    objTabItem.NoOfPODs = objPageInfo.objaTabs[i].objaPods.Length;

                    if (objPageInfo.strDropType == "OnTab")
                    {
                        for (int j = 0; j < objPageInfo.objaTabs[i].straTabBuddies.Length; j++)
                        {
                            objTabItem.AddBuddy(objPageInfo.objaTabs[i].straTabBuddies[j]);
                            objTabItem.SetMaxCounter(objPageInfo.objaTabs[i].objaPods.Length, objPageInfo.objaTabs[i].straTabBuddies[j]);
                        }
                        objTabItem.AddBuddy(objPageInfo.strFrom);
                        objTabItem.SetMaxCounter(objPageInfo.objaTabs[i].objaPods.Length, objPageInfo.strFrom);
                    }

                    objTabControl.Items.Add(objTabItem);

                    //LinearGradientBrush objTabItemNormalBackground = new LinearGradientBrush();
                    //objTabItemNormalBackground.StartPoint = new Point(0, 0);
                    //objTabItemNormalBackground.EndPoint = new Point(0, 1);

                    //objTabItemNormalBackground.GradientStops.Add(new GradientStop(Color.FromArgb(255, 252, 253, 253), 0));
                    //objTabItemNormalBackground.GradientStops.Add(new GradientStop(Color.FromArgb(255, 229, 234, 245), 0.3));
                    //objTabItemNormalBackground.GradientStops.Add(new GradientStop(Color.FromArgb(255, 207, 215, 235), 0.3));
                    //objTabItemNormalBackground.GradientStops.Add(new GradientStop(Color.FromArgb(255, 224, 229, 245), 0.7));
                    //objTabItemNormalBackground.GradientStops.Add(new GradientStop(Color.FromArgb(255, 236, 238, 252), 1));

                    //<GradientStop Color="#FFFBFDFE" Offset="0"/>
                    //<GradientStop Color="#FFEAF6FB" Offset="0.3"/>
                    //<GradientStop Color="#FFCEE7FA" Offset="0.3"/>
                    //<GradientStop Color="#FFB9D1FA" Offset="1"/>

                    //LinearGradientBrush objTabItemSelectedBackground = new LinearGradientBrush();
                    //objTabItemSelectedBackground.StartPoint = new Point(0, 0);
                    //objTabItemSelectedBackground.EndPoint = new Point(0, 1);

                    //objTabItemSelectedBackground.GradientStops.Add(new GradientStop(Color.FromArgb(255, 251, 253, 254), 0));
                    //objTabItemSelectedBackground.GradientStops.Add(new GradientStop(Color.FromArgb(255, 234, 246, 251), 0.3));
                    //objTabItemSelectedBackground.GradientStops.Add(new GradientStop(Color.FromArgb(255, 206, 231, 250), 0.3));
                    //objTabItemSelectedBackground.GradientStops.Add(new GradientStop(Color.FromArgb(255, 185, 209, 250), 1));


                    //<GradientStop Color="#FFFCFDFD" Offset="0"/>
                    //<GradientStop Color="#FFC6DDF7" Offset="0.3"/>
                    //<GradientStop Color="#FF99C6EE" Offset="0.3"/>
                    //<GradientStop Color="#FFB6D6F1" Offset="0.7"/>
                    //<GradientStop Color="#FFD9E9F9" Offset="1"/>


                    //LinearGradientBrush objTabItemMouseOverBackground = new LinearGradientBrush();
                    //objTabItemMouseOverBackground.StartPoint = new Point(0, 0);
                    //objTabItemMouseOverBackground.EndPoint = new Point(0, 1);

                    //objTabItemMouseOverBackground.GradientStops.Add(new GradientStop(Color.FromArgb(255, 252, 253, 253), 0));
                    //objTabItemMouseOverBackground.GradientStops.Add(new GradientStop(Color.FromArgb(255, 198, 221, 247), 0.3));
                    //objTabItemMouseOverBackground.GradientStops.Add(new GradientStop(Color.FromArgb(255, 153, 198, 238), 0.3));
                    //objTabItemMouseOverBackground.GradientStops.Add(new GradientStop(Color.FromArgb(255, 182, 214, 241), 0.7));
                    //objTabItemMouseOverBackground.GradientStops.Add(new GradientStop(Color.FromArgb(255, 217, 233, 249), 1));

                    //objTabControl.SetValue(TabControl.pgTabItemNormalBackgroundProperty, objTabItemNormalBackground);
                    //objTabControl.SetValue(TabControl.pgTabItemSelectedBackgroundProperty, objTabItemSelectedBackground);
                    //objTabControl.SetValue(TabControl.pgTabItemMouseOverBackgroundProperty, objTabItemMouseOverBackground);
                }
                return objTabControl;
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "NewTabControl()", "Controls\\VMuktiGrid\\Page\\TabControl.cs");
                return null;
            }
        }

        public void SetUserID()
        {
            try
            {
                for (int i = 0; i < this.Items.Count; i++)
                {
                    if (((VMuktiGrid.ctlPage.TabItem)this.Items[i]).OwnerID < 0)
                    {
                        ((VMuktiGrid.ctlPage.TabItem)this.Items[i]).OwnerID = VMuktiAPI.VMuktiInfo.CurrentPeer.ID;
                        ((VMuktiGrid.ctlTab.TabControl)((VMuktiGrid.ctlPage.TabItem)this.Items[i]).Content).SetUserID();
                    }
                }
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "SetUserId()", "Controls\\VMuktiGrid\\Page\\TabControl.cs");
            }
        }

        public void UnSetUserID()
        {
            try
            {
                for (int i = 0; i < this.Items.Count; i++)
                {
                    if (((VMuktiGrid.ctlPage.TabItem)this.Items[i]).OwnerID > 0)
                    {
                        ((VMuktiGrid.ctlPage.TabItem)this.Items[i]).OwnerID = int.MinValue;
                        ((VMuktiGrid.ctlTab.TabControl)((VMuktiGrid.ctlPage.TabItem)this.Items[i]).Content).UnSetUserID();
                    }
                }
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "UnSetUserId()", "Controls\\VMuktiGrid\\Page\\TabControl.cs");
            }
        }
        public void SetReturnBuddyStatus(clsBuddyRetPageInfo objBuddyRetPageInfo, int pageIndex)
        {
            try
            {
                VMuktiGrid.ctlPage.TabItem selectedPage = (VMuktiGrid.ctlPage.TabItem)this.Items[pageIndex];

                int i = 0;
                int j = 0;

                for (i = 0; i < objBuddyRetPageInfo.objaTabs.Length; i++)
                {
                    for (j = 0; j < ((VMuktiGrid.ctlTab.TabControl)selectedPage.Content).Items.Count; j++)
                    {
                        if (((VMuktiGrid.ctlTab.TabItem)((VMuktiGrid.ctlTab.TabControl)selectedPage.Content).Items[j]).OwnerTabIndex == objBuddyRetPageInfo.objaTabs[0].intOwnerTabIndex)
                        {
                            if (objBuddyRetPageInfo.strDropType == "OnTab")
                            {
                                ((VMuktiGrid.ctlTab.TabItem)((VMuktiGrid.ctlTab.TabControl)selectedPage.Content).Items[j]).ShowBuddy(objBuddyRetPageInfo.strFrom);
                            }

                            ((VMuktiGrid.CustomGrid.ctlGrid)((VMuktiGrid.ctlTab.TabItem)((VMuktiGrid.ctlTab.TabControl)selectedPage.Content).Items[j]).Content).SetReturnBuddyStatus(objBuddyRetPageInfo);
                            break;
                        }
                    }
                    if (j < ((VMuktiGrid.ctlTab.TabControl)selectedPage.Content).Items.Count)
                    {
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "SetReturnBuddyStatus()", "Controls\\VMuktiGrid\\Page\\TabControl.cs");
            }
        }
        #region IDisposable Members

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    try
                    {
                        this.Items.Clear();
                    }
                    catch (Exception ex)
                    {
                        VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "Dispose()", "Controls\\VMuktiGrid\\Page\\TabControl.cs");
                    }
                }
                disposed = true;
            }
            GC.SuppressFinalize(this);
        }

        ~TabControl()
        {
            Dispose(false);
        }

        #endregion

        #region Multiple Buddy Selection

        public void LoadMultipleBuddyPage(Dictionary<CtlExpanderItem, string> buddiesname, int modid)
        {
            try
            {
                TabItem tbiPage = new TabItem();
                tbiPage.ObjectID = -1;
                tbiPage.IsSaved = false;
                tbiPage.OwnerID = VMuktiAPI.VMuktiInfo.CurrentPeer.ID;
                tbiPage.OwnerPageIndex = VMukti.App.pageCounter++;

                tbiPage.Width = 250;
                tbiPage.MinWidth = 250;
                tbiPage.MaxWidth = 250;

                ctlPgTabHeader objPgTabHeader = new ctlPgTabHeader();
                objPgTabHeader.Title = "New Page";
                //TextBlock txtBlock = new TextBlock();
                //txtBlock.Text = "New Tab - " + this.Items.Count.ToString();

                Image imgIcon = new Image();
                imgIcon.Source = new BitmapImage(new Uri(@"\Skins\Images\VMuktiIcon.ico", UriKind.RelativeOrAbsolute));
                imgIcon.Height = 16;
                imgIcon.Width = 16;

                //item.Header = txtBlock;
                tbiPage.Header = objPgTabHeader;
                tbiPage.Icon = imgIcon;

                tbiPage.Content = LoadMultipleBuddyTab(buddiesname, modid);

                //if (i == -1 || i == this.Items.Count - 1 || AddNewTabToEnd)
                this.Items.Add(tbiPage);
                //else
                //this.Items.Insert(++i, item);

                if (SelectNewTabOnCreate)
                {
                   // SelectedItem = tbiPage;

                    VirtualizingTabPanel itemsHost = Helper.FindVirtualizingTabPanel(this);
                    if (itemsHost != null)
                        itemsHost.MakeVisible(tbiPage, Rect.Empty);

                   // tbiPage.Focus();
                }

                if (TabItemAdded != null)
                    TabItemAdded(this, new TabItemEventArgs(tbiPage));
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "LoadMultipleBuddyPage()", "Controls\\VMuktiGrid\\Page\\TabControl.cs");
            }
        }

        public ctlTab.TabControl LoadMultipleBuddyTab(Dictionary<CtlExpanderItem, string> buddiesname, int modid)
        {

            try
            {
                ctlTab.TabControl objTabControl = new ctlTab.TabControl();
                objTabControl.SetValue(Grid.RowProperty, 1);
                objTabControl.TabItemMinWidth = 150;
                objTabControl.TabItemMaxWidth = 300;
                objTabControl.TabItemMinHeight = 30;
                objTabControl.TabItemMaxHeight = 50;
                objTabControl.VerticalAlignment = VerticalAlignment.Top;
                objTabControl.Margin = new Thickness(5);


                ctlTab.TabItem objTabItem = new ctlTab.TabItem();
                objTabItem.Width = 250;
                objTabItem.MinWidth = 250;
                objTabItem.MaxWidth = 250;
                objTabItem.OwnerTabIndex = VMukti.App.tabCounter++;
                //TextBlock txtTabBlock = new TextBlock();
                //txtTabBlock.Text = "New Tab - 0";
                objTabItem.ObjectID = -1;
                objTabItem.IsSaved = false;

                ctlPgTabHeader objPgTabHeader = new ctlPgTabHeader();
                objPgTabHeader.Title = "New Tab";

                Image imgTabIcon = new Image();
                imgTabIcon.Source = new BitmapImage(new Uri(@"\Skins\Images\VMuktiIcon.ico", UriKind.RelativeOrAbsolute));
                imgTabIcon.Height = 16;
                imgTabIcon.Width = 16;

                objTabItem.Content = new CustomGrid.ctlGrid();
                ((CustomGrid.ctlGrid)objTabItem.Content).LoadMultipleBuddyGrid(buddiesname, modid);

                //objTabItem.Header = txtTabBlock;
                objTabItem.Header = objPgTabHeader;
                objTabItem.Icon = imgTabIcon;

                objTabControl.Items.Add(objTabItem);

               
                objTabItem.StartBlinking();
                return objTabControl;
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "LoadMultipleBuddyTab()", "Controls\\VMuktiGrid\\Page\\TabControl.cs");
                return null;
            }



        }


        public void LoadMultipleBuddyPage(clsModuleInfo objModInfo)
        {
            try
            {
                TabItem tbiPage = new TabItem();
                tbiPage.ObjectID = -1;
                tbiPage.IsSaved = false;
                tbiPage.OwnerID = VMuktiAPI.VMuktiInfo.CurrentPeer.ID;
                tbiPage.OwnerPageIndex = VMukti.App.pageCounter++;

                tbiPage.Width = 250;
                tbiPage.MinWidth = 250;
                tbiPage.MaxWidth = 250;

                ctlPgTabHeader objPgTabHeader = new ctlPgTabHeader();
                objPgTabHeader.Title = "New Page";
                //TextBlock txtBlock = new TextBlock();
                //txtBlock.Text = "New Tab - " + this.Items.Count.ToString();

                Image imgIcon = new Image();
                imgIcon.Source = new BitmapImage(new Uri(@"\Skins\Images\VMuktiIcon.ico", UriKind.RelativeOrAbsolute));
                imgIcon.Height = 16;
                imgIcon.Width = 16;

                //item.Header = txtBlock;
                tbiPage.Header = objPgTabHeader;
                tbiPage.Icon = imgIcon;

                tbiPage.Content = LoadMultipleBuddyTab(objModInfo);

                //if (i == -1 || i == this.Items.Count - 1 || AddNewTabToEnd)
                this.Items.Add(tbiPage);
                //else
                //this.Items.Insert(++i, item);

                if (SelectNewTabOnCreate)
                {
                    //SelectedItem = tbiPage;

                    VirtualizingTabPanel itemsHost = Helper.FindVirtualizingTabPanel(this);
                    if (itemsHost != null)
                        itemsHost.MakeVisible(tbiPage, Rect.Empty);

                   // tbiPage.Focus();
                }

                if (TabItemAdded != null)
                    TabItemAdded(this, new TabItemEventArgs(tbiPage));

                tbiPage.StartBlinking();
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "LoadMultipleBuddyPage()--3", "Controls\\VMuktiGrid\\Page\\TabControl.cs");
            }
        }

        public ctlTab.TabControl LoadMultipleBuddyTab(clsModuleInfo objModInfo)
        {

            try
            {
                ctlTab.TabControl objTabControl = new ctlTab.TabControl();
                objTabControl.SetValue(Grid.RowProperty, 1);
                objTabControl.TabItemMinWidth = 150;
                objTabControl.TabItemMaxWidth = 300;
                objTabControl.TabItemMinHeight = 30;
                objTabControl.TabItemMaxHeight = 50;
                objTabControl.VerticalAlignment = VerticalAlignment.Top;
                objTabControl.Margin = new Thickness(5);


                ctlTab.TabItem objTabItem = new ctlTab.TabItem();
                objTabItem.Width = 250;
                objTabItem.MinWidth = 250;
                objTabItem.MaxWidth = 250;
                objTabItem.OwnerTabIndex = VMukti.App.tabCounter++;
                //TextBlock txtTabBlock = new TextBlock();
                //txtTabBlock.Text = "New Tab - 0";
                objTabItem.ObjectID = -1;
                objTabItem.IsSaved = false;

                ctlPgTabHeader objPgTabHeader = new ctlPgTabHeader();
                objPgTabHeader.Title = "New Tab";

                Image imgTabIcon = new Image();
                imgTabIcon.Source = new BitmapImage(new Uri(@"\Skins\Images\VMuktiIcon.ico", UriKind.RelativeOrAbsolute));
                imgTabIcon.Height = 16;
                imgTabIcon.Width = 16;

                objTabItem.Content = new CustomGrid.ctlGrid();
                ((CustomGrid.ctlGrid)objTabItem.Content).LoadMultipleBuddyGrid(objModInfo);

                //objTabItem.Header = txtTabBlock;
                objTabItem.Header = objPgTabHeader;
                objTabItem.Icon = imgTabIcon;

                objTabControl.Items.Add(objTabItem);
                objTabItem.StartBlinking();
                return objTabControl;
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "LoadMultipleBuddyPage()-4", "Controls\\VMuktiGrid\\Page\\TabControl.cs");
                return null;
            }



        }


        #endregion


        public clsPageInfo SendPage(VMuktiGrid.ctlPage.TabItem objSelectedPage, string buddyname)
        {
            try
            {
                objPage = new clsPageInfo();
                this.Dispatcher.Invoke(System.Windows.Threading.DispatcherPriority.Normal, objSendPageInfo, objSelectedPage, buddyname);
                return objPage;
            }
            catch (Exception exp)
            {
                MessageBox.Show("SendPage" + exp.Message);
                return null;
            }
        }
        private void SendPageInfo(VMuktiGrid.ctlPage.TabItem objSelectedPage, string buddyname)
        {

            try
            {
                clsPageInfo objPageInfo = new clsPageInfo();
                //if (this.AddBuddy(buddyname, this.SelectedIndex))
                if (objSelectedPage.AddBuddy(buddyname))
                {
                    VMuktiGrid.ctlTab.TabItem objSelectedTab = null;


                    objPageInfo.intPageID = objSelectedPage.ObjectID;
                    objPageInfo.strPageTitle = ((VMuktiGrid.CustomMenu.ctlPgTabHeader)objSelectedPage.Header).Title;

                    objPageInfo.intOwnerID = VMuktiAPI.VMuktiInfo.CurrentPeer.ID;
                    objPageInfo.intOwnerPageIndex = objSelectedPage.OwnerPageIndex;
                    objPageInfo.ConfID = objSelectedPage.ConfID;

                    objPageInfo.strDropType = "OnPage";
                    List<string> lstBuddyList = new List<string>();
                    StackPanel stPageBuddyList = ((ctlMenu)objSelectedPage.Template.FindName("objMenu", objSelectedPage)).objEMIBuddyList.objBuddyList.stBuddyPanel;
                    for (int i = 0; i < stPageBuddyList.Children.Count; i++)
                    {
                        lstBuddyList.Add(((VMuktiGrid.Buddy.ctlBuddy)stPageBuddyList.Children[i]).Title);
                    }
                    objPageInfo.straPageBuddies = lstBuddyList.ToArray();

                    objSelectedPage.NoOfPODs = 0;

                    List<clsTabInfo> lstTabInfos = new List<clsTabInfo>();
                    for (int tCnt = 0; tCnt < ((VMuktiGrid.ctlTab.TabControl)objSelectedPage.Content).Items.Count; tCnt++)
                    {
                        objSelectedTab = ((VMuktiGrid.ctlTab.TabItem)((VMuktiGrid.ctlTab.TabControl)objSelectedPage.Content).Items[tCnt]);
                        objSelectedTab.NoOfPODs = 0;

                        lstTabInfos.Add(new clsTabInfo());

                        lstTabInfos[lstTabInfos.Count - 1].intTabID = objSelectedTab.ObjectID;
                        lstTabInfos[lstTabInfos.Count - 1].strTabTitle = ((VMuktiGrid.CustomMenu.ctlPgTabHeader)objSelectedTab.Header).Title;

                        StackPanel stTabBuddyList = ((ctlMenu)objSelectedTab.Template.FindName("objMenu", objSelectedTab)).objEMIBuddyList.objBuddyList.stBuddyPanel;
                        lstBuddyList.Clear();
                        for (int i = 0; i < stTabBuddyList.Children.Count; i++)
                        {
                            lstBuddyList.Add(((VMuktiGrid.Buddy.ctlBuddy)stTabBuddyList.Children[i]).Title);
                        }
                        lstTabInfos[lstTabInfos.Count - 1].straTabBuddies = lstBuddyList.ToArray();

                        //lstTabInfos[lstTabInfos.Count - 1].intOwnerTabIndex = tCnt;
                        lstTabInfos[lstTabInfos.Count - 1].intOwnerTabIndex = objSelectedTab.OwnerTabIndex;
                        lstTabInfos[lstTabInfos.Count - 1].dblC1Width = ((VMuktiGrid.CustomGrid.ctlGrid)objSelectedTab.Content).LeftPanelContainer.ActualWidth;
                        lstTabInfos[lstTabInfos.Count - 1].dblC2Width = ((VMuktiGrid.CustomGrid.ctlGrid)objSelectedTab.Content).CentralPanelContainer.ActualWidth;
                        lstTabInfos[lstTabInfos.Count - 1].dblC3Width = ((VMuktiGrid.CustomGrid.ctlGrid)objSelectedTab.Content).RightPanelContainer.ActualWidth;

                        lstTabInfos[lstTabInfos.Count - 1].dblC4Height = ((VMuktiGrid.CustomGrid.ctlGrid)objSelectedTab.Content).TopPanelContainer.ActualHeight;
                        lstTabInfos[lstTabInfos.Count - 1].dblC5Height = ((VMuktiGrid.CustomGrid.ctlGrid)objSelectedTab.Content).BottomPanelContainer.ActualHeight;

                        List<clsPodInfo> lstPodInfo = new List<clsPodInfo>();
                        VMuktiGrid.CustomGrid.ctlGrid objSelectedGrid = (VMuktiGrid.CustomGrid.ctlGrid)objSelectedTab.Content;

                        for (int pCnt = 0; pCnt < objSelectedGrid.LeftPanelContainer.Items.Count; pCnt++)
                        {
                            if (objSelectedGrid.LeftPanelContainer.Items[pCnt].GetType() == typeof(VMuktiGrid.CustomGrid.ctlPOD))
                            {
                                //lstPodInfo.Add(new clsPodInfo());
                                //fncGetPodInfo(lstPodInfo, lstPodInfo.Count - 1, ((VMuktiGrid.CustomGrid.ctlPOD)objSelectedGrid.LeftPanelContainer.Items[pCnt]));

                                //lstPodInfo[lstPodInfo.Count - 1].intColumnNumber = 1;
                                //lstPodInfo[lstPodInfo.Count - 1].intOwnerPodIndex = ((VMuktiGrid.CustomGrid.ctlPOD)objSelectedGrid.LeftPanelContainer.Items[pCnt]).OwnerPodIndex;

                                clsPodInfo objPodInfo = new clsPodInfo();
                                objPodInfo.intPodID = ((VMuktiGrid.CustomGrid.ctlPOD)objSelectedGrid.LeftPanelContainer.Items[pCnt]).ObjectID;
                                objPodInfo.intModuleId = ((VMuktiGrid.CustomGrid.ctlPOD)objSelectedGrid.LeftPanelContainer.Items[pCnt]).ModuleID;
                                objPodInfo.strPodTitle = ((VMuktiGrid.CustomGrid.ctlPOD)objSelectedGrid.LeftPanelContainer.Items[pCnt]).Title;
                                objPodInfo.strUri = ((VMuktiGrid.CustomGrid.ctlPOD)objSelectedGrid.LeftPanelContainer.Items[pCnt]).WCFUri;
                                objPodInfo.intColumnNumber = 1;


                                lstBuddyList.Clear();
                                StackPanel stPodBuddyList = ((VMuktiGrid.CustomGrid.ctlPOD)objSelectedGrid.LeftPanelContainer.Items[pCnt]).objBuddyList.stPanel;
                                for (int i = 0; i < stPodBuddyList.Children.Count; i++)
                                {
                                    if (stPodBuddyList.Children[i].GetType() == typeof(VMuktiGrid.Buddy.ctlBuddy))
                                    {
                                        lstBuddyList.Add(((VMuktiGrid.Buddy.ctlBuddy)stPodBuddyList.Children[i]).Title);
                                    }
                                }
                                //lstPodInfo[lstPodInfo.Count - 1].straPodBuddies = lstBuddyList.ToArray();

                                objPodInfo.straPodBuddies = lstBuddyList.ToArray();

                                lstPodInfo.Add(objPodInfo);

                                ((VMuktiGrid.ctlPage.TabItem)this.SelectedItem).NoOfPODs++;
                                objSelectedTab.NoOfPODs++;
                            }
                        }

                        for (int pCnt = 0; pCnt < objSelectedGrid.CentralPanelContainer.Items.Count; pCnt++)
                        {
                            if (objSelectedGrid.CentralPanelContainer.Items[pCnt].GetType() == typeof(VMuktiGrid.CustomGrid.ctlPOD))
                            {
                               clsPodInfo objPodInfo=new clsPodInfo();
                               objPodInfo.intPodID = ((VMuktiGrid.CustomGrid.ctlPOD)objSelectedGrid.CentralPanelContainer.Items[pCnt]).ObjectID;
                               objPodInfo.intModuleId = ((VMuktiGrid.CustomGrid.ctlPOD)objSelectedGrid.CentralPanelContainer.Items[pCnt]).ModuleID;
                               objPodInfo.strPodTitle = ((VMuktiGrid.CustomGrid.ctlPOD)objSelectedGrid.CentralPanelContainer.Items[pCnt]).Title;
                               objPodInfo.strUri = ((VMuktiGrid.CustomGrid.ctlPOD)objSelectedGrid.CentralPanelContainer.Items[pCnt]).WCFUri;
                               objPodInfo.intColumnNumber = 2;
                                //fncGetPodInfo(lstPodInfo, lstPodInfo.Count - 1, ((VMuktiGrid.CustomGrid.ctlPOD)objSelectedGrid.CentralPanelContainer.Items[pCnt]));

                                //lstPodInfo[lstPodInfo.Count - 1].intColumnNumber = 2;

                                lstBuddyList.Clear();
                                StackPanel stPodBuddyList = ((VMuktiGrid.CustomGrid.ctlPOD)objSelectedGrid.CentralPanelContainer.Items[pCnt]).objBuddyList.stPanel;
                                for (int i = 0; i < stPodBuddyList.Children.Count; i++)
                                {
                                    if (stPodBuddyList.Children[i].GetType() == typeof(VMuktiGrid.Buddy.ctlBuddy))
                                    {
                                        lstBuddyList.Add(((VMuktiGrid.Buddy.ctlBuddy)stPodBuddyList.Children[i]).Title);
                                    }
                                }
                                objPodInfo.straPodBuddies = lstBuddyList.ToArray();

                                lstPodInfo.Add(objPodInfo);

                                ((VMuktiGrid.ctlPage.TabItem)this.SelectedItem).NoOfPODs++;
                                objSelectedTab.NoOfPODs++;

                            }
                        }

                        for (int pCnt = 0; pCnt < objSelectedGrid.RightPanelContainer.Items.Count; pCnt++)
                        {
                            if (objSelectedGrid.RightPanelContainer.Items[pCnt].GetType() == typeof(VMuktiGrid.CustomGrid.ctlPOD))
                            {
                                //lstPodInfo.Add(new clsPodInfo());
                                //fncGetPodInfo(lstPodInfo, lstPodInfo.Count - 1, ((VMuktiGrid.CustomGrid.ctlPOD)objSelectedGrid.RightPanelContainer.Items[pCnt]));
                                //lstPodInfo[lstPodInfo.Count - 1].intColumnNumber = 3;

                                clsPodInfo objPodInfo = new clsPodInfo();
                                objPodInfo.intPodID = ((VMuktiGrid.CustomGrid.ctlPOD)objSelectedGrid.RightPanelContainer.Items[pCnt]).ObjectID;
                                objPodInfo.intModuleId = ((VMuktiGrid.CustomGrid.ctlPOD)objSelectedGrid.RightPanelContainer.Items[pCnt]).ModuleID;
                                objPodInfo.strPodTitle = ((VMuktiGrid.CustomGrid.ctlPOD)objSelectedGrid.RightPanelContainer.Items[pCnt]).Title;
                                objPodInfo.strUri = ((VMuktiGrid.CustomGrid.ctlPOD)objSelectedGrid.RightPanelContainer.Items[pCnt]).WCFUri;
                                objPodInfo.intColumnNumber = 3;

                                lstBuddyList.Clear();
                                StackPanel stPodBuddyList = ((VMuktiGrid.CustomGrid.ctlPOD)objSelectedGrid.RightPanelContainer.Items[pCnt]).objBuddyList.stPanel;
                                for (int i = 0; i < stPodBuddyList.Children.Count; i++)
                                {
                                    if (stPodBuddyList.Children[i].GetType() == typeof(VMuktiGrid.Buddy.ctlBuddy))
                                    {
                                        lstBuddyList.Add(((VMuktiGrid.Buddy.ctlBuddy)stPodBuddyList.Children[i]).Title);
                                    }
                                }
                                //lstPodInfo[lstPodInfo.Count - 1].straPodBuddies = lstBuddyList.ToArray();
                                objPodInfo.straPodBuddies = lstBuddyList.ToArray();
                                lstPodInfo.Add(objPodInfo);
                                objSelectedPage.NoOfPODs++;
                                objSelectedTab.NoOfPODs++;
                            }
                        }

                        for (int pCnt = 0; pCnt < objSelectedGrid.TopPanelContainer.Items.Count; pCnt++)
                        {
                            if (objSelectedGrid.TopPanelContainer.Items[pCnt].GetType() == typeof(VMuktiGrid.CustomGrid.ctlPOD))
                            {
                                //lstPodInfo.Add(new clsPodInfo());
                                //fncGetPodInfo(lstPodInfo, lstPodInfo.Count - 1, ((VMuktiGrid.CustomGrid.ctlPOD)objSelectedGrid.TopPanelContainer.Items[pCnt]));

                                //lstPodInfo[lstPodInfo.Count - 1].intColumnNumber = 4;

                                clsPodInfo objPodInfo = new clsPodInfo();
                                objPodInfo.intPodID = ((VMuktiGrid.CustomGrid.ctlPOD)objSelectedGrid.TopPanelContainer.Items[pCnt]).ObjectID;
                                objPodInfo.intModuleId = ((VMuktiGrid.CustomGrid.ctlPOD)objSelectedGrid.TopPanelContainer.Items[pCnt]).ModuleID;
                                objPodInfo.strPodTitle = ((VMuktiGrid.CustomGrid.ctlPOD)objSelectedGrid.TopPanelContainer.Items[pCnt]).Title;
                                objPodInfo.strUri = ((VMuktiGrid.CustomGrid.ctlPOD)objSelectedGrid.TopPanelContainer.Items[pCnt]).WCFUri;
                                objPodInfo.intColumnNumber = 4;




                                lstBuddyList.Clear();
                                StackPanel stPodBuddyList = ((VMuktiGrid.CustomGrid.ctlPOD)objSelectedGrid.TopPanelContainer.Items[pCnt]).objBuddyList.stPanel;
                                for (int i = 0; i < stPodBuddyList.Children.Count; i++)
                                {
                                    if (stPodBuddyList.Children[i].GetType() == typeof(VMuktiGrid.Buddy.ctlBuddy))
                                    {
                                        lstBuddyList.Add(((VMuktiGrid.Buddy.ctlBuddy)stPodBuddyList.Children[i]).Title);
                                    }
                                }
                                //lstPodInfo[lstPodInfo.Count - 1].straPodBuddies = lstBuddyList.ToArray();
                                objPodInfo.straPodBuddies = lstBuddyList.ToArray();
                                lstPodInfo.Add(objPodInfo);

                                ((VMuktiGrid.ctlPage.TabItem)this.SelectedItem).NoOfPODs++;
                                objSelectedTab.NoOfPODs++;
                            }
                        }

                        for (int pCnt = 0; pCnt < objSelectedGrid.BottomPanelContainer.Items.Count; pCnt++)
                        {
                            if (objSelectedGrid.BottomPanelContainer.Items[pCnt].GetType() == typeof(VMuktiGrid.CustomGrid.ctlPOD))
                            {
                                //lstPodInfo.Add(new clsPodInfo());

                                //fncGetPodInfo(lstPodInfo, lstPodInfo.Count - 1, ((VMuktiGrid.CustomGrid.ctlPOD)objSelectedGrid.BottomPanelContainer.Items[pCnt]));
                                //lstPodInfo[lstPodInfo.Count - 1].intColumnNumber = 5;


                                clsPodInfo objPodInfo = new clsPodInfo();
                                objPodInfo.intPodID = ((VMuktiGrid.CustomGrid.ctlPOD)objSelectedGrid.BottomPanelContainer.Items[pCnt]).ObjectID;
                                objPodInfo.intModuleId = ((VMuktiGrid.CustomGrid.ctlPOD)objSelectedGrid.BottomPanelContainer.Items[pCnt]).ModuleID;
                                objPodInfo.strPodTitle = ((VMuktiGrid.CustomGrid.ctlPOD)objSelectedGrid.BottomPanelContainer.Items[pCnt]).Title;
                                objPodInfo.strUri = ((VMuktiGrid.CustomGrid.ctlPOD)objSelectedGrid.BottomPanelContainer.Items[pCnt]).WCFUri;
                                objPodInfo.intColumnNumber = 5;



                                lstBuddyList.Clear();
                                StackPanel stPodBuddyList = ((VMuktiGrid.CustomGrid.ctlPOD)objSelectedGrid.BottomPanelContainer.Items[pCnt]).objBuddyList.stPanel;
                                for (int i = 0; i < stPodBuddyList.Children.Count; i++)
                                {
                                    if (stPodBuddyList.Children[i].GetType() == typeof(VMuktiGrid.Buddy.ctlBuddy))
                                    {
                                        lstBuddyList.Add(((VMuktiGrid.Buddy.ctlBuddy)stPodBuddyList.Children[i]).Title);
                                    }
                                }
                                //lstPodInfo[lstPodInfo.Count - 1].straPodBuddies = lstBuddyList.ToArray();

                                //((VMuktiGrid.ctlPage.TabItem)this.SelectedItem).NoOfPODs++;

                                objPodInfo.straPodBuddies = lstBuddyList.ToArray();
                                lstPodInfo.Add(objPodInfo);

                                objSelectedPage.NoOfPODs++;
                                objSelectedTab.NoOfPODs++;
                            }
                        }

                        lstTabInfos[lstTabInfos.Count - 1].objaPods = lstPodInfo.ToArray();
                        objSelectedTab.SetMaxCounter(objSelectedTab.NoOfPODs, buddyname);
                    }
                    objPageInfo.objaTabs = lstTabInfos.ToArray();

                    objPageInfo.strFrom = VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName;
                    objPageInfo.strTo = buddyname;
                    objPageInfo.strMsg = "OPEN_PAGE";

                    this.SetMaxCounter(objSelectedPage.NoOfPODs, buddyname);

                }
                objPage = objPageInfo;
            }
            catch (Exception exp)
            {
                MessageBox.Show("exp" + exp.Message);
                //return null;
            }
        }

        #region changes for tab hide
        public void FncOpenPageStrip(bool open)
        {
            try
            {
                if (open)
                {
                    if (_RowDefinition0 != null)
                    {
                        VMukti.Presentation.GridLengthAnimation gla = new VMukti.Presentation.GridLengthAnimation();
                        gla.From = new GridLength(0, GridUnitType.Pixel);
                        gla.To = new GridLength(30, GridUnitType.Pixel);
                        gla.Duration = new Duration(TimeSpan.FromSeconds(0.50));
                        _RowDefinition0.BeginAnimation(RowDefinition.HeightProperty, gla);
                        //_RowDefinition0.Height = new GridLength(30, GridUnitType.Pixel);
                    }
                }
                else
                {
                    if (_RowDefinition0 != null)
                    {

                        VMukti.Presentation.GridLengthAnimation gla = new VMukti.Presentation.GridLengthAnimation();
                        gla.From = new GridLength(30, GridUnitType.Pixel);
                        gla.To = new GridLength(0, GridUnitType.Pixel);
                        gla.Duration = new Duration(TimeSpan.FromSeconds(0.50));
                        _RowDefinition0.BeginAnimation(RowDefinition.HeightProperty, gla);
                        // _RowDefinition0.Height = new GridLength(0, GridUnitType.Pixel);
                    }
                }
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "FncOpenPageStrip", "TabControl.cs");
            }
        }
        #endregion

        void fncGetPodInfo(List<clsPodInfo> lstPodInfo, int index, VMuktiGrid.CustomGrid.ctlPOD objPod)
        {
            lstPodInfo[index].intModuleId = objPod.ModuleID;
            lstPodInfo[index].strPodTitle = objPod.Title;
            lstPodInfo[index].strUri = objPod.WCFUri;


        }

        void fncGetPodInfoForDrop(List<clsPodInfo> lstPodInfo, int index, VMuktiGrid.CustomGrid.ctlPOD objPod)
        {
            lstPodInfo[index].intModuleId = objPod.ModuleID;
            lstPodInfo[index].strPodTitle = objPod.Title;
            lstPodInfo[index].strUri = objPod.WCFUri;
            lstPodInfo[index].intOwnerPodIndex = objPod.OwnerPodIndex;

        }

    }
}
