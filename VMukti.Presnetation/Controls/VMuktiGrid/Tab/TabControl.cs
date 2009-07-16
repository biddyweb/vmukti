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
using System.Collections.Generic;
using VMukti.Business.CommonDataContracts;
using VMukti;
using VMuktiAPI;
using System.Windows.Markup;
using System.Xml;
using System.IO;

namespace VMuktiGrid.ctlTab
{
    [TemplatePart(Name = "PART_DropDown", Type = typeof(ToggleButton))]
    [TemplatePart(Name = "PART_RepeatLeft", Type = typeof(RepeatButton))]
    [TemplatePart(Name = "PART_RepeatRight", Type = typeof(RepeatButton))]
    [TemplatePart(Name = "PART_NewTabButton", Type = typeof(ButtonBase))]
    [TemplatePart(Name = "PART_ScrollViewer", Type = typeof(ScrollViewer))]
    [TemplatePart(Name = "RowDefinition0", Type = typeof(RowDefinition))]
   
    public class TabControl : System.Windows.Controls.TabControl, IDisposable
    {
        // public Events
        public event EventHandler<TabItemEventArgs> TabItemAdded;
        public event EventHandler<TabItemCancelEventArgs> TabItemClosing;
        public event EventHandler<TabItemEventArgs> TabItemClosed;

        private bool disposed = false;

        // TemplatePart controls
        private ToggleButton _toggleButton;
        private RowDefinition _RowDefinition0;
        static TabControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(TabControl), new FrameworkPropertyMetadata(typeof(TabControl)));
            TabStripPlacementProperty.AddOwner(typeof(TabControl), new FrameworkPropertyMetadata(Dock.Top, new PropertyChangedCallback(OnTabStripPlacementChanged)));
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
            TabControl tc = (TabControl)d;

            foreach (TabItem item in tc.Items)
                item.CoerceValue(TabItem.TabStripPlacementProperty);
        }

        #region Dependancy properties

        #region Brushes

        public Brush tbTabItemNormalBackground
        {
            get { return (Brush)GetValue(tbTabItemNormalBackgroundProperty); }
            set { SetValue(tbTabItemNormalBackgroundProperty, value); }
        }
        public static readonly DependencyProperty tbTabItemNormalBackgroundProperty = DependencyProperty.Register("tbTabItemNormalBackground", typeof(Brush), typeof(TabControl), new UIPropertyMetadata(null));

        public Brush tbTabItemMouseOverBackground
        {
            get { return (Brush)GetValue(tbTabItemMouseOverBackgroundProperty); }
            set { SetValue(tbTabItemMouseOverBackgroundProperty, value); }
        }
        public static readonly DependencyProperty tbTabItemMouseOverBackgroundProperty = DependencyProperty.Register("tbTabItemMouseOverBackground", typeof(Brush), typeof(TabControl), new UIPropertyMetadata(null));

        public Brush tbTabItemSelectedBackground
        {
            get { return (Brush)GetValue(tbTabItemSelectedBackgroundProperty); }
            set { SetValue(tbTabItemSelectedBackgroundProperty, value); }
        }
        public static readonly DependencyProperty tbTabItemSelectedBackgroundProperty = DependencyProperty.Register("tbTabItemSelectedBackground", typeof(Brush), typeof(TabControl), new UIPropertyMetadata(null));




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
            TabControl tc = (TabControl)d;
            if (tc.Template == null) return;

            VirtualizingTabPanel tp = Helper.FindVirtualizingTabPanel(tc);
            if (tp != null)
                tp.InvalidateMeasure();
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

                this.Drop += new DragEventHandler(TabControl_Drop);
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

                        item.OwnerTabIndex = this.Items.Count;

                        //TextBlock txtBlock = new TextBlock();
                        //txtBlock.Text = "New Tab - " + this.Items.Count.ToString();
                        ctlPgTabHeader objPgTabHeader = new ctlPgTabHeader();
                       // objPgTabHeader.Title = "New Tab - " + this.Items.Count.ToString();
                        objPgTabHeader.Title = "Untiteled";
                        Image imgIcon = new Image();
                        imgIcon.Source = new BitmapImage(new Uri(@"\Skins\Images\VMuktiIcon.ico", UriKind.RelativeOrAbsolute));
                        imgIcon.Height = 16;
                        imgIcon.Width = 16;

                        //item.Header = txtBlock;
                        item.Header = objPgTabHeader;
                        item.Icon = imgIcon;
                        item.Content = new CustomGrid.ctlGrid();

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
                }
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "OnApplyTemplate()", "Controls\\VMuktiGrid\\Tab\\TabControl.cs");
            }
        }

        void TabControl_Drop(object sender, DragEventArgs e)
        {
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

                    if (blnBuddyType && this.AddBuddy(((CtlExpanderItem)e.Data.GetData(typeof(CtlExpanderItem))).Caption, this.SelectedIndex))
                    {
                        clsPageInfo objPageInfo = new clsPageInfo();
                        objPageInfo.intPageID = ((VMuktiGrid.ctlPage.TabItem)this.Parent).ObjectID;
                        objPageInfo.strPageTitle = ((VMuktiGrid.CustomMenu.ctlPgTabHeader)((VMuktiGrid.ctlPage.TabItem)this.Parent).Header).Title;

                        objPageInfo.intOwnerID = VMuktiAPI.VMuktiInfo.CurrentPeer.ID;
                        //objPageInfo.intOwnerPageIndex = ((VMuktiGrid.ctlPage.TabControl)((VMuktiGrid.ctlPage.TabItem)this.Parent).Parent).SelectedIndex;
                        objPageInfo.intOwnerPageIndex = ((VMuktiGrid.ctlPage.TabItem)this.Parent).OwnerPageIndex;

                        objPageInfo.strDropType = "OnTab";

                        List<clsTabInfo> lstTabInfos = new List<clsTabInfo>();
                        lstTabInfos.Add(new clsTabInfo());

                        lstTabInfos[lstTabInfos.Count - 1].intTabID = ((VMuktiGrid.ctlTab.TabItem)this.SelectedItem).ObjectID;
                        lstTabInfos[lstTabInfos.Count - 1].strTabTitle = ((VMuktiGrid.CustomMenu.ctlPgTabHeader)((VMuktiGrid.ctlTab.TabItem)this.SelectedItem).Header).Title;

                        List<string> lstBuddyList = new List<string>();
                        StackPanel stTabBuddyList = ((ctlMenu)((VMuktiGrid.ctlTab.TabItem)this.SelectedItem).Template.FindName("objMenu", (VMuktiGrid.ctlTab.TabItem)this.SelectedItem)).objEMIBuddyList.objBuddyList.stBuddyPanel;
                        for (int i = 0; i < stTabBuddyList.Children.Count; i++)
                        {
                            lstBuddyList.Add(((VMuktiGrid.Buddy.ctlBuddy)stTabBuddyList.Children[i]).Title);
                        }
                        lstTabInfos[lstTabInfos.Count - 1].straTabBuddies = lstBuddyList.ToArray();
                        VMuktiGrid.CustomGrid.ctlGrid objSelectedGrid = (VMuktiGrid.CustomGrid.ctlGrid)((VMuktiGrid.ctlTab.TabItem)this.SelectedItem).Content;
                        //lstTabInfos[lstTabInfos.Count - 1].intOwnerTabIndex = this.SelectedIndex;
                        lstTabInfos[lstTabInfos.Count - 1].intOwnerTabIndex = ((VMuktiGrid.ctlTab.TabItem)this.SelectedItem).OwnerTabIndex;
                        lstTabInfos[lstTabInfos.Count - 1].dblC1Width = objSelectedGrid.LeftPanelContainer.ActualWidth;
                        lstTabInfos[lstTabInfos.Count - 1].dblC2Width = objSelectedGrid.CentralPanelContainer.ActualWidth;
                        lstTabInfos[lstTabInfos.Count - 1].dblC3Width = objSelectedGrid.RightPanelContainer.ActualWidth;

                        lstTabInfos[lstTabInfos.Count - 1].dblC4Height = objSelectedGrid.TopPanelContainer.ActualHeight;
                        lstTabInfos[lstTabInfos.Count - 1].dblC5Height = objSelectedGrid.BottomPanelContainer.ActualHeight;

                        List<clsPodInfo> lstPodInfo = new List<clsPodInfo>();

                        ((VMuktiGrid.ctlTab.TabItem)this.SelectedItem).NoOfPODs = 0;
                        for (int pCnt = 0; pCnt < objSelectedGrid.LeftPanelContainer.Items.Count; pCnt++)
                        {
                            if (objSelectedGrid.LeftPanelContainer.Items[pCnt].GetType() == typeof(VMuktiGrid.CustomGrid.ctlPOD))
                            {
                                lstPodInfo.Add(new clsPodInfo());
                                fncGetPodInfo(lstPodInfo, lstPodInfo.Count - 1, ((VMuktiGrid.CustomGrid.ctlPOD)objSelectedGrid.LeftPanelContainer.Items[pCnt]));
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

                                ((VMuktiGrid.ctlTab.TabItem)this.SelectedItem).NoOfPODs++;
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

                                ((VMuktiGrid.ctlTab.TabItem)this.SelectedItem).NoOfPODs++;
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

                                ((VMuktiGrid.ctlTab.TabItem)this.SelectedItem).NoOfPODs++;
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

                                ((VMuktiGrid.ctlTab.TabItem)this.SelectedItem).NoOfPODs++;
                            }
                        }

                        for (int pCnt = 0; pCnt < objSelectedGrid.BottomPanelContainer.Items.Count; pCnt++)
                        {
                            if (objSelectedGrid.TopPanelContainer.Items[pCnt].GetType() == typeof(VMuktiGrid.CustomGrid.ctlPOD))
                            {
                                lstPodInfo.Add(new clsPodInfo());
                                fncGetPodInfo(lstPodInfo, lstPodInfo.Count - 1, ((VMuktiGrid.CustomGrid.ctlPOD)objSelectedGrid.BottomPanelContainer.Items[pCnt]));

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

                                ((VMuktiGrid.ctlTab.TabItem)this.SelectedItem).NoOfPODs++;
                            }
                        }

                        lstTabInfos[lstTabInfos.Count - 1].objaPods = lstPodInfo.ToArray();
                        objPageInfo.objaTabs = lstTabInfos.ToArray();

                        objPageInfo.strFrom = VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName;
                        objPageInfo.strTo = ((CtlExpanderItem)e.Data.GetData(typeof(CtlExpanderItem))).Caption;
                        objPageInfo.strMsg = "OPEN_PAGE";

                        this.SetMaxCounter(((VMuktiGrid.ctlTab.TabItem)this.SelectedItem).NoOfPODs, ((CtlExpanderItem)e.Data.GetData(typeof(CtlExpanderItem))).Caption);

                        if (VMuktiAPI.VMuktiInfo.CurrentPeer.CurrPeerType == VMuktiAPI.PeerType.NodeWithNetP2P || VMuktiAPI.VMuktiInfo.CurrentPeer.CurrPeerType == VMuktiAPI.PeerType.BootStrap || VMuktiAPI.VMuktiInfo.CurrentPeer.CurrPeerType == VMuktiAPI.PeerType.SuperNode)
                        {
                            App.chNetP2PSuperNodeChannel.svcSetSpecialMsg(objPageInfo, VMuktiAPI.VMuktiInfo.CurrentPeer.SuperNodeIP);
                        }
                        else
                        {
                            try
                            {
                                App.chHttpSuperNodeService.svcSetSpecialMsgs(objPageInfo);
                            }
                            catch (System.ServiceModel.EndpointNotFoundException ex)
                            {
                                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "TabControl_Drop()", "Controls\\VMuktiGrid\\Tab\\TabControl.cs");
                                VMuktiAPI.VMuktiHelper.CallEvent("GetSuperNodeIP", null, null);

                                App.chHttpSuperNodeService.svcSetSpecialMsgs(objPageInfo);
                            }
                            catch (System.ServiceModel.CommunicationException ex1)
                            {
                                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex1, "TabControl_Drop()--1", "Controls\\VMuktiGrid\\Tab\\TabControl.cs");
                                VMuktiAPI.VMuktiHelper.CallEvent("GetSuperNodeIP", null, null);

                                App.chHttpSuperNodeService.svcSetSpecialMsgs(objPageInfo);
                            }
                        }

                        //((ctlVMuktiGrid)((Grid)((VMuktiGrid.ctlPage.TabControl)((VMuktiGrid.ctlPage.TabItem)this.Parent).Parent).Parent).Parent).fncChannelSetPageMsg(VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName, ((VMukti.Presentation.Controls.CtlExpanderItem)e.Data.GetData(typeof(VMukti.Presentation.Controls.CtlExpanderItem))).Caption, "OPEN_PAGE", lstModuleInfo);
                        //VMukti.App.chNetP2PSuperNodeChannel.svcAddDraggedBuddy
                    }
                }
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "TabControl_Drop()--2", "Controls\\VMuktiGrid\\Tab\\TabControl.cs");
            }
        }

        /// <summary>
        /// IsItemItsOwnContainerOverride
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        protected override bool IsItemItsOwnContainerOverride(object item)
        {
            return item is ctlTab.TabItem;
        }

        /// <summary>
        /// GetContainerForItemOverride
        /// </summary>
        /// <returns></returns>
        protected override DependencyObject GetContainerForItemOverride()
        {
            return new TabItem();
        }

        /// <summary>
        /// Handle the ToggleButton Checked event that displays a context menu of TabItem Headers
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// Look this place for the error, button before new page or tabs
        void DropdownButton_Checked(object sender, RoutedEventArgs e)
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
                

                MenuItem mi = new MenuItem() { Header = ((ctlPgTabHeader)item.Header).Title,Tag = i.ToString() };
               
                mi.Click += ContextMenuItem_Click;

                _toggleButton.ContextMenu.Items.Add(mi);
            }
        }

        protected override void OnPreviewKeyDown(KeyEventArgs e)
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

        /// <summary>
        /// Handle the MenuItem's Click event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void ContextMenuItem_Click(object sender, RoutedEventArgs e)
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
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "RemoveItem()", "Controls\\VMuktiGrid\\Tab\\TabControl.cs");
            }
        }

        public bool AddBuddy(string strBuddyName, int tabItemIndex)
        {
            try
            {
                return ((ctlTab.TabItem)this.Items[tabItemIndex]).AddBuddy(strBuddyName);
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "Addbuddy()", "Controls\\VMuktiGrid\\Tab\\TabControl.cs");                
                return false;
            }
        }

        public bool RemoveBuddy(string strBuddyName, int tabItemIndex)
        {
            try
            {
                return ((ctlTab.TabItem)this.Items[tabItemIndex]).RemoveBuddy(strBuddyName);
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "RemoveBuddy()", "Controls\\VMuktiGrid\\Tab\\TabControl.cs");
                return false;
            }
        }

        public void SetMaxCounter(int intMaxCounter, string strBuddyName)
        {
            try
            {
                ((ctlTab.TabItem)this.Items[this.SelectedIndex]).SetMaxCounter(intMaxCounter, strBuddyName);
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "SetmaxCounter()", "Controls\\VMuktiGrid\\Tab\\TabControl.cs");
            }
        }

        public void ShowBuddy(string strBuddyName)
        {
            try
            {
                for (int i = 0; i < this.Items.Count; i++)
                {
                    ((ctlTab.TabItem)this.Items[i]).ShowBuddy(strBuddyName);
                }
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "ShowBuddy()", "Controls\\VMuktiGrid\\Tab\\TabControl.cs");
            }
        }

        public bool CheckBuddy(string strBuddyName, int tabItemIndex)
        {
            try
            {
                return ((ctlTab.TabItem)this.Items[tabItemIndex]).CheckBuddy(strBuddyName);
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "CheckBuddy()", "Controls\\VMuktiGrid\\Tab\\TabControl.cs");                
                return false;
            }
        }

        public void LoadTab(int tabID)
        {
        }

        public void SetUserID()
        {
            //for (int i = 0; i < this.Items.Count; i++)
            //{
            //    if (((VMuktiGrid.ctlTab.TabItem)this.Items[i]).OwnerID < 0)
            //    {
            //        ((VMuktiGrid.ctlTab.TabItem)this.Items[i]).OwnerID = VMuktiAPI.VMuktiInfo.CurrentPeer.ID;
            //        ((VMuktiGrid.CustomGrid.ctlGrid)((VMuktiGrid.ctlTab.TabItem)this.Items[i]).Content).SetUserID();
            //    }
            //}
        }

        public void UnSetUserID()
        {
            //for (int i = 0; i < this.Items.Count; i++)
            //{
            //    if (((VMuktiGrid.ctlTab.TabItem)this.Items[i]).OwnerID > 0)
            //    {
            //        ((VMuktiGrid.ctlTab.TabItem)this.Items[i]).OwnerID = int.MinValue;
            //        ((VMuktiGrid.CustomGrid.ctlGrid)((VMuktiGrid.ctlTab.TabItem)this.Items[i]).Content).UnSetUserID();
            //    }
            //}
        }

        public void Save()
        {
            for (int i = 0; i < this.Items.Count; i++)
            {
                //((VMuktiGrid.ctlTab.TabItem)this.Items[i]).Save(
            }
        }

        //public void SetReturnBuddyStatus(clsBuddyRetPageInfo objBuddyRetPageInfo)
        //{
        //    try
        //    {
        //        int pCnt = 0;
        //        for (pCnt = 0; pCnt < this.Items.Count; pCnt++)
        //        {
        //            if (((VMuktiGrid.ctlPage.TabItem)this.Items[pCnt]).OwnerID == objBuddyRetPageInfo.intOwnerID && ((VMuktiGrid.ctlPage.TabItem)pageControl.Items[pCnt]).OwnerPageIndex == objBuddyRetPageInfo.intOwnerPageIndex)
        //            {
        //                pageControl.SetReturnBuddyStatus(objBuddyRetPageInfo, pCnt);
        //                break;
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        ex.Data.Add("My Key", "SetReturnBuddyStatus()--:--cctlVMuktiGrid.xmal.cs--:--" + ex.Message + " :--:--");
        //        ClsException.LogError(ex);
        //        ClsException.WriteToErrorLogFile(ex);
        //    }
        //}

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
                        VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "Dispose(bool Disposing)", "Controls\\VMuktiGrid\\Tab\\TabControl.cs");
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

       
        #region changes for tab hide
        public void FncOpenTabStrip(bool open)
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
                    }
                }
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "FncOpenTabStrip()", "Controls\\VMuktiGrid\\Tab\\TabControl.cs");

            }
        }

        #endregion 


        void fncGetPodInfo(List<clsPodInfo> lstPodInfo, int index,VMuktiGrid.CustomGrid.ctlPOD objPod)
        {
            lstPodInfo[index].intModuleId = objPod.ModuleID;
            lstPodInfo[index].strPodTitle = objPod.Title;
            lstPodInfo[index].strUri = objPod.WCFUri;
            lstPodInfo[index].intOwnerPodIndex = objPod.OwnerPodIndex;

        }
    }
}
