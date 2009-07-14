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


//  Summery
// ==========
//    Applicable to VMukti version 1.0.7.4
//=======================================================
//Module Name : VMuktiGrid
//Overview of Module
//    -> VMuktiGrid is combo control of many other sub controls which are listed in "Files with respective group" secsion below. With this control we can create any number of pages

//    In this control two resoucedictionary has been used named:

//    One drag & drop utility is also implemented which includes files as follow:
//        -> DragDropAdviser\VMuktiDragDropAdvisor.cs - (188	edit	pratik	19-04-2008 12:42:56 PM	try-catch removed...)
//        -> DragDropManager\DragDropManager.cs - (193	edit	dhara	19-04-2008 3:03:06 PM	try catch block have been added)
//        -> DragDropManager\DragSourceBase.cs - (205	edit	dhara	19-04-2008 4:25:22 PM	try catch block added)
//        -> DragDropManager\DropPreviewAdorner.cs - (207	edit	dhara	19-04-2008 4:57:28 PM	try catch block have been added)
//        -> DragDropManager\DropTargetBase.cs - (221	edit	dhara	21-04-2008 12:03:53 PM	header have been added)
//        which is used for dragging the module on the grid.
//Purpose of this module

//Files with respective group
//    * VMukti.Presnetation solution folder -> VMukti Project
//        1. Buddy (in "Buddy" folder)
//            -> ctlBuddy.xaml - (22	edit	pratik	12-04-2008 10:34:27 AM	visibility = visible)
//                -> ctlBuddy.xaml.cs - (145	edit	dhara	18-04-2008 4:34:09 PM	added try catch block and header)

//            -> ctlBuddyList.xaml - (21	edit	pratik	11-04-2008 7:55:08 PM	Changes after Grid + Pod/Tab/Page buddies adding after confirmation from dropped buddy.)
//                -> ctlBuddyList.xaml.cs - (150	edit	dhara	18-04-2008 4:48:02 PM	try catch block and header have been added)

//            -> ctlPodBuddyList.xaml - (3	add	nisarg	09-04-2008 5:15:52 PM	New VMukti project has been added with latest code available on 04/04/2008 5:34 PM)
//                -> ctlPodBuddyList.xaml.cs - (153	edit	dhara	18-04-2008 4:55:15 PM	try catch block and header have been added)

//        2. Grid (in "Grid" folder)
//            -> clsConverters.cs - (223	edit	dhara	21-04-2008 12:39:39 PM	header and try catch blocks have been added)

//            -> clsGridPOD.cs - (225	edit	dhara	21-04-2008 12:45:20 PM	header and try catch blocks added)

//            -> ctlGrid.xaml - (129	edit	pratik	17-04-2008 8:12:33 PM	Drag & drop completed!!)
//                -> ctlGrid.xaml.cs - (348	edit	meet	23-04-2008 4:42:06 PM	TopPanelContainer_PreviewDragEnter -> value can not be null error has been resolved.)

//            -> ctlPOD.xaml - (129	edit	pratik	17-04-2008 8:12:33 PM	Drag & drop completed!!)
//                -> ctlPOD.xaml.cs - (346	edit	pratik	23-04-2008 12:21:23 PM	Ctor of ctlPOD has been called . . . . .      exception has been removed....)
//            -> ResPOD.xaml - (3	add	nisarg	09-04-2008 5:15:52 PM	New VMukti project has been added with latest code available on 04/04/2008 5:34 PM)
	

//        3. Menu (in "Menu" folder)
//            -> ctlExpMenuItem.xaml - (3	add	nisarg	09-04-2008 5:15:52 PM	New VMukti project has been added with latest code available on 04/04/2008 5:34 PM)
//                -> ctlExpMenuItem.xaml.cs - (238	edit	dhara	21-04-2008 6:14:30 PM	try catch block and header have been added)

//            -> ctlMenu.xaml - (3	add	nisarg	09-04-2008 5:15:52 PM	New VMukti project has been added with latest code available on 04/04/2008 5:34 PM)
//                -> ctlMenu.xaml.cs - (239	edit	dhara	21-04-2008 6:21:57 PM	try catch blocks and headers have been added)

//            -> ctlMenuItem.xaml - (3	add	nisarg	09-04-2008 5:15:52 PM	New VMukti project has been added with latest code available on 04/04/2008 5:34 PM)
//                -> ctlMenuItem.xaml.cs - (240	edit	dhara	21-04-2008 6:25:30 PM	try catch block and header have been added)

//            -> ctlPgTabHeader.xaml - (21	edit	pratik	11-04-2008 7:55:08 PM	Changes after Grid + Pod/Tab/Page buddies adding after confirmation from dropped buddy.)
//                -> ctlPgTabHeader.xaml.cs - (241	edit	dhara	21-04-2008 6:28:05 PM	try catch block and headers have been added)

//        4. Page (in "Page" folder)
//            -> Helper.cs - (242	edit	dhara	21-04-2008 6:30:49 PM	try catch block and headers hae been added)
//            -> TabControl.cs - (246	edit	dhara	21-04-2008 7:14:29 PM	try catch block and header have been added)
//            -> TabEventArgs.cs - (243	edit	dhara	21-04-2008 6:32:11 PM	header has been added)
//            -> TabItem.cs - (245	edit	dhara	21-04-2008 7:02:58 PM	try catch block and header have been added)
//            -> VirtualizingTabPanel.cs - (342	edit	dhara	23-04-2008 11:11:13 AM	try catch block and header have been added.)

//        5. Tab (in "Tab" folder)
//            -> Helper.cs - (248	edit	dhara	21-04-2008 7:22:52 PM	try catch block and header have been added)
//            -> TabControl.cs - (218	edit	pratik	19-04-2008 8:20:03 PM	svcSendSpecialMsg is made enabled...)
//            -> TabEventArgs.cs - (247	edit	dhara	21-04-2008 7:15:39 PM	header have been added)
//            -> TabItem.cs - (21	edit	pratik	11-04-2008 7:55:08 PM	Changes after Grid + Pod/Tab/Page buddies adding after confirmation from dropped buddy.)
//            -> VirtualizingTabPanel.cs - (3	add	nisarg	09-04-2008 5:15:52 PM	New VMukti project has been added with latest code available on 04/04/2008 5:34 PM)

//        6. Resoucedictionary
//            -> Grid\ResPOD.xaml (which helps for POD UI)  - (3	add	nisarg	09-04-2008 5:15:52 PM	New VMukti project has been added with latest code available on 04/04/2008 5:34 PM)
//            -> Theme\Generic.xaml (which helps for UI of page and tab) - (3	add	nisarg	09-04-2008 5:15:52 PM	New VMukti project has been added with latest code available on 04/04/2008 5:34 PM)

//        7. Drag & drop utility
//            -> DragDropAdviser\VMuktiDragDropAdvisor.cs - (188	edit	pratik	19-04-2008 12:42:56 PM	try-catch removed...)
//            -> DragDropManager\DragDropManager.cs - (193	edit	dhara	19-04-2008 3:03:06 PM	try catch block have been added)
//            -> DragDropManager\DragSourceBase.cs - (205	edit	dhara	19-04-2008 4:25:22 PM	try catch block added)
//            -> DragDropManager\DropPreviewAdorner.cs - (207	edit	dhara	19-04-2008 4:57:28 PM	try catch block have been added)
//            -> DragDropManager\DropTargetBase.cs - (221	edit	dhara	21-04-2008 12:03:53 PM	header have been added)

//        8. Misc files (in "VMuktiGrid" folder)
//            -> clsConverters.cs - (160	edit	dhara	18-04-2008 5:47:15 PM	try catch blck have been added)
//            -> ctlVMuktiGrid.xaml - (21	edit	pratik	11-04-2008 7:55:08 PM	Changes after Grid + Pod/Tab/Page buddies adding after confirmation from dropped buddy.)
//            -> ctlVMuktiGrid.xaml.cs - (167	edit	dhara	18-04-2008 6:28:18 PM	try catch blocks have been added)
//            -> GifAnimation.cs - (165	edit	dhara	18-04-2008 6:23:34 PM	try catch block and header have been added)
//            -> ImageAnim.cs - (159	edit	dhara	18-04-2008 5:06:23 PM	try catch block and header have been added)

//        9. Images
//            -> Buddy.Png - (3	add	nisarg	09-04-2008 5:15:52 PM	New VMukti project has been added with latest code available on 04/04/2008 5:34 PM)
//            -> Delete.png - (3	add	nisarg	09-04-2008 5:15:52 PM	New VMukti project has been added with latest code available on 04/04/2008 5:34 PM)
//            -> ie.ico - (3	add	nisarg	09-04-2008 5:15:52 PM	New VMukti project has been added with latest code available on 04/04/2008 5:34 PM)
//            -> loading.gif - (3	add	nisarg	09-04-2008 5:15:52 PM	New VMukti project has been added with latest code available on 04/04/2008 5:34 PM)
//            -> loading2.gif - (3	add	nisarg	09-04-2008 5:15:52 PM	New VMukti project has been added with latest code available on 04/04/2008 5:34 PM)
//            -> NewTab.ico - (3	add	nisarg	09-04-2008 5:15:52 PM	New VMukti project has been added with latest code available on 04/04/2008 5:34 PM)
//            -> PageDelete.png - (3	add	nisarg	09-04-2008 5:15:52 PM	New VMukti project has been added with latest code available on 04/04/2008 5:34 PM)
//            -> Rename.png - (3	add	nisarg	09-04-2008 5:15:52 PM	New VMukti project has been added with latest code available on 04/04/2008 5:34 PM)
//            -> Save.ico - (3	add	nisarg	09-04-2008 5:15:52 PM	New VMukti project has been added with latest code available on 04/04/2008 5:34 PM)
//            -> VMuktiIcon.ico - (21	edit	pratik	11-04-2008 7:55:08 PM	Changes after Grid + Pod/Tab/Page buddies adding after confirmation from dropped buddy.)

//    * VMukti.Bussiness solution folder -> VMukti.Bussiness Project
//        1. Module (in "VMuktiGrid\Module" folder)
//            -> ClsModule.cs - (191	edit	purvaja	19-04-2008 2:53:29 PM	added try catch and header)
//            -> ClsModuleCollection.cs - (192	edit	purvaja	19-04-2008 3:01:50 PM	added try catch block and header)

//        2. Pages (in "VMuktiGrid\Pages" folder)
//            -> ClsPage.cs - (194	edit	purvaja	19-04-2008 3:11:14 PM	added new header and try catch block)
//            -> ClsPageCollection.cs - (195	edit	purvaja	19-04-2008 3:14:02 PM	added new header and try catch block)

//        3. Pods (in "VMuktiGrid\Pods" folder)
//            -> ClsPod.cs - (197	edit	purvaja	19-04-2008 3:25:00 PM	added new header and try catch block)
//            -> ClsPodCollection.cs - (198	edit	purvaja	19-04-2008 3:27:12 PM	added new header and try catch block)

//        4. Tabs (in "VMuktiGrid\Tabs" folder)
//            -> ClsTab.cs - (250	edit	nisarg	21-04-2008 8:05:06 PM	Page save issue for nodewith http has ben solved and new version V1073 has been published.)
//            -> ClsTabCollection.cs - (200	edit	purvaja	19-04-2008 3:35:46 PM	new header and try catch block)

//    * VMukti.DataAccess solution folder -> VMukti.DataAccess project
//        1. Module (in "VMuktiGrid\Module" folder)
//            -> ClsModuleDataService.cs - (340	edit	purvaja	23-04-2008 10:57:56 AM	added try catch block and header)

//        2. Pages (in "VMuktiGrid\Pages" folder)
//            -> ClsPageDataService.cs - (341	edit	purvaja	23-04-2008 11:10:17 AM	added try catch block and header)

//        3. Pods (in "VMuktiGrid\Pods" folder)
//            -> ClsPodDataService.cs - (343	edit	purvaja	23-04-2008 11:14:57 AM	added try catch block and header)

//        4. Tabs (in "VMuktiGrid\Tabs" folder)
//            -> ClsTabDataService.cs - (344	edit	purvaja	23-04-2008 11:21:37 AM	added try catch block and header)

//Version number(Changeset) for each file
//----------as above--------------------

//Changeset of the main project

//Publish version of main project
//    version 1.0.7.4

//Videos with callouts, comments 
//    <Under process>

//How to compile and run the module
//    -> To Build VMukti project in VMukti.Presentation solution folder
//    -> To run this module include ctlVMuktiGrid.xaml in pgHome.xaml
//        (ie. I have put Page(Tabcontrol->TabItem)->Tab(Tabcontrol->TabItem)->ctlGrid.xaml in ctlVMuktiGrid.xaml file as a default page)

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
using VMukti.Presentation.Controls;
using System.Runtime.InteropServices;
using VMukti.Business;
using VMukti.Business.CommonDataContracts;
using VMuktiAPI;
using System.Xml;
using System.IO;
using System.Windows.Markup;
//using POD;
//using POD.Common;

namespace VMuktiGrid.CustomGrid
{
    public partial class ctlGrid : UserControl, IDisposable
    {
        public static StringBuilder sb1 = new StringBuilder();

       
        private bool disposed = false;
        
        ItemsControl knownParent = null;
        ItemsControl targetItemsControl;
        List<int> lstDeletePods = new List<int>();

        public Rectangle rectSuggetion = new Rectangle();
        public bool IsDragEnter = true;

       
        public ctlGrid()
        {
            try
            {
                InitializeComponent();

                rectSuggetion.Fill = Brushes.Transparent;
                rectSuggetion.Stroke = Brushes.Red;

                DoubleCollection dblCol = new DoubleCollection();
                dblCol.Add(5.0);
                dblCol.Add(5.0);

                rectSuggetion.StrokeDashArray = dblCol;
                rectSuggetion.StrokeDashCap = PenLineCap.Round;
                rectSuggetion.StrokeDashOffset = 50;
                rectSuggetion.StrokeEndLineCap = PenLineCap.Square;
                rectSuggetion.StrokeLineJoin = PenLineJoin.Miter;
                rectSuggetion.StrokeMiterLimit = 50;
                rectSuggetion.RadiusX = 16;
                rectSuggetion.RadiusY = 16;
                rectSuggetion.Height = 100;
                EventManager.RegisterClassHandler(typeof(ctlPOD), ctlPOD.OnPODDragEvent, new RoutedEventHandler(this.OnDragPOD));
                EventManager.RegisterClassHandler(typeof(ctlPOD), ctlPOD.OnPODDropEvent, new RoutedEventHandler(this.OnDropPOD));
                #region drag drop
          //    EventManager.RegisterClassHandler(typeof(CtlModule), CtlModule.OnModuleDropEvent, new RoutedEventHandler(this.OnModuleDrop));

                #endregion
                LeftPanelContainer.MouseLeave += new MouseEventHandler(LeftPanelContainer_MouseLeave);
                CentralPanelContainer.MouseLeave += new MouseEventHandler(CentralPanelContainer_MouseLeave);
                RightPanelContainer.MouseLeave += new MouseEventHandler(RightPanelContainer_MouseLeave);
                TopPanelContainer.MouseLeave += new MouseEventHandler(TopPanelContainer_MouseLeave);
                BottomPanelContainer.MouseLeave += new MouseEventHandler(BottomPanelContainer_MouseLeave);

                LeftPanelContainer.MouseEnter += new MouseEventHandler(LeftPanelContainer_MouseEnter);
                CentralPanelContainer.MouseEnter += new MouseEventHandler(CentralPanelContainer_MouseEnter);
                RightPanelContainer.MouseEnter += new MouseEventHandler(RightPanelContainer_MouseEnter);
                TopPanelContainer.MouseEnter += new MouseEventHandler(TopPanelContainer_MouseEnter);
                BottomPanelContainer.MouseEnter += new MouseEventHandler(BottomPanelContainer_MouseEnter);

                LeftPanelContainer.AllowDrop = true;
                CentralPanelContainer.AllowDrop = true;
                RightPanelContainer.AllowDrop = true;
                TopPanelContainer.AllowDrop = true;
                BottomPanelContainer.AllowDrop = true;
                
                LeftPanelContainer.Tag = 1;
                CentralPanelContainer.Tag = 2;
                RightPanelContainer.Tag = 3;
                TopPanelContainer.Tag = 4;
                BottomPanelContainer.Tag = 5;
}
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "ctlGrid", "Controls\\VmuktiGrid\\Grid\\CtlGrid.xaml.cs");
            }
        }


        #region itemcontrol event
        //when dragging the module from left pan this will be occured and this event will add suggetion to repected itemscontrol
        private void TopPanelContainer_PreviewDragEnter(object sender, DragEventArgs e)
        {
            try
            {
                if (e.Data.GetData("VMuktiDragAndDropModule") != null)
                {
                    string xamlString = e.Data.GetData("VMuktiDragAndDropModule") as string;
                    XmlReader reader = XmlReader.Create(new StringReader(xamlString));
                    CtlMExpanderItem elt = XamlReader.Load(reader) as CtlMExpanderItem;
                    string[] strTag = elt.Tag.ToString().Split(',');

                    if (strTag.Length == 3)
                    {
                        this.SetGridSplliterVisiblity(false);

                        if (rectSuggetion.Parent != null)
                            ((ItemsControl)rectSuggetion.Parent).Items.Remove(rectSuggetion);

                        rectSuggetion.Height = VMuktiHelper.RectSuggestHeight;
                        TopPanelContainer.Items.Add(rectSuggetion);
                    }
                }
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "TopPanelContainer_PreviewDragEnter()", "Controls\\VmuktiGrid\\Grid\\CtlGrid.xaml.cs");
            }
        }

        //when dragging the module from left pan this will be occured and this event will add suggetion to repected itemscontrol
        private void LeftPanelContainer_PreviewDragEnter(object sender, DragEventArgs e)
        {
            try
            {
                if (e.Data.GetData("VMuktiDragAndDropModule") != null)
                {
                    string xamlString = e.Data.GetData("VMuktiDragAndDropModule") as string;
                    XmlReader reader = XmlReader.Create(new StringReader(xamlString));
                    CtlMExpanderItem elt = XamlReader.Load(reader) as CtlMExpanderItem;
                    string[] strTag = elt.Tag.ToString().Split(',');

                    if (strTag.Length == 3)
                    {
                        this.SetGridSplliterVisiblity(false);

                        if (rectSuggetion.Parent != null)
                            ((ItemsControl)rectSuggetion.Parent).Items.Remove(rectSuggetion);

                        rectSuggetion.Height = VMuktiHelper.RectSuggestHeight;
                        LeftPanelContainer.Items.Add(rectSuggetion);
                    }
                }
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "LeftPanelContainer_PreviewDragEnter()", "Controls\\VmuktiGrid\\Grid\\CtlGrid.xaml.cs");
            }
        }

        //when dragging the module from left pan this will be occured and this event will add suggetion to repected itemscontrol
        private void CentralPanelContainer_PreviewDragEnter(object sender, DragEventArgs e)
        {
            try
            {
                if (e.Data.GetData("VMuktiDragAndDropModule") != null)
                {
                    string xamlString = e.Data.GetData("VMuktiDragAndDropModule") as string;
                    XmlReader reader = XmlReader.Create(new StringReader(xamlString));
                    CtlMExpanderItem elt = XamlReader.Load(reader) as CtlMExpanderItem;
                    string[] strTag = elt.Tag.ToString().Split(',');

                    if (strTag.Length == 3)
                    {
                        this.SetGridSplliterVisiblity(false);

                        if (rectSuggetion.Parent != null)
                            ((ItemsControl)rectSuggetion.Parent).Items.Remove(rectSuggetion);

                        rectSuggetion.Height = VMuktiHelper.RectSuggestHeight;
                        CentralPanelContainer.Items.Add(rectSuggetion);
                    }
                }
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "CentalPanelContainer_PreviewDragEnter()", "Controls\\VmuktiGrid\\Grid\\CtlGrid.xaml.cs");
            }
        }

        //when dragging the module from left pan this will be occured and this event will add suggetion to repected itemscontrol
        private void RightPanelContainer_PreviewDragEnter(object sender, DragEventArgs e)
        {
            try
            {
                if (e.Data.GetData("VMuktiDragAndDropModule") != null)
                {
                    string xamlString = e.Data.GetData("VMuktiDragAndDropModule") as string;
                    XmlReader reader = XmlReader.Create(new StringReader(xamlString));
                    CtlMExpanderItem elt = XamlReader.Load(reader) as CtlMExpanderItem;
                    string[] strTag = elt.Tag.ToString().Split(',');

                    if (strTag.Length == 3)
                    {
                        this.SetGridSplliterVisiblity(false);

                        if (rectSuggetion.Parent != null)
                            ((ItemsControl)rectSuggetion.Parent).Items.Remove(rectSuggetion);

                        rectSuggetion.Height = VMuktiHelper.RectSuggestHeight;
                        RightPanelContainer.Items.Add(rectSuggetion);
                    }
                }
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "RightPanleContainer_PreviewDragEnter()", "Controls\\VmuktiGrid\\Grid\\CtlGrid.xaml.cs");
            }
        }

        //when dragging the module from left pan this will be occured and this event will add suggetion to repected itemscontrol
        private void BottomPanelContainer_PreviewDragEnter(object sender, DragEventArgs e)
        {
            try
            {
                if (e.Data.GetData("VMuktiDragAndDropModule") != null)
                {
                    string xamlString = e.Data.GetData("VMuktiDragAndDropModule") as string;
                    XmlReader reader = XmlReader.Create(new StringReader(xamlString));
                    CtlMExpanderItem elt = XamlReader.Load(reader) as CtlMExpanderItem;
                    string[] strTag = elt.Tag.ToString().Split(',');

                    if (strTag.Length == 3)
                    {
                        this.SetGridSplliterVisiblity(false);

                        if (rectSuggetion.Parent != null)
                            ((ItemsControl)rectSuggetion.Parent).Items.Remove(rectSuggetion);

                        rectSuggetion.Height = VMuktiHelper.RectSuggestHeight;
                        BottomPanelContainer.Items.Add(rectSuggetion);
                    }
                }
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "BottomanelContainer_PreviewDragEnter()", "Controls\\VmuktiGrid\\Grid\\CtlGrid.xaml.cs");
            }
        }

        void ctlGrid_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
            this.PreviewMouseLeftButtonDown -= new MouseButtonEventHandler(ctlGrid_PreviewMouseLeftButtonDown);

            TopPanelContainer.SizeChanged += new SizeChangedEventHandler(TopPanelContainer_SizeChanged);
            LeftPanelContainer.SizeChanged += new SizeChangedEventHandler(TopPanelContainer_SizeChanged);
            CentralPanelContainer.SizeChanged += new SizeChangedEventHandler(TopPanelContainer_SizeChanged);
            RightPanelContainer.SizeChanged += new SizeChangedEventHandler(TopPanelContainer_SizeChanged);
            BottomPanelContainer.SizeChanged += new SizeChangedEventHandler(TopPanelContainer_SizeChanged);
        }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "ctlGrid_PreviewMouseLeftButtonDown()", "Controls\\VmuktiGrid\\Grid\\CtlGrid.xaml.cs");
            }
        }

        //when ctlPOD.xaml will be dragged and will be entered into the ctlGrid usercontrol, grid splliters are creating problem when dragging the module so we are making them visible collapsed here
        public void ctlGrid_PreviewDragEnter(object sender, DragEventArgs e)
        {
            try
            {
                this.PreviewDragEnter -= new DragEventHandler(ctlGrid_PreviewDragEnter);
                IsDragEnter = false;
                
                string xamlString = e.Data.GetData("VMuktiDragAndDropModule") as string;
                XmlReader reader = XmlReader.Create(new StringReader(xamlString));
                CtlMExpanderItem elt = XamlReader.Load(reader) as CtlMExpanderItem;
                string[] strTag = elt.Tag.ToString().Split(',');

                if (strTag.Length == 3)
                {
                    VMuktiHelper.IsDraggingPOD = true;
                    VMuktiHelper.IsRectSuggestAdded = false;

                    TopSplitter.Visibility = Visibility.Collapsed;
                    TopSplitter_1.Visibility = Visibility.Collapsed;
                    LeftSplitter.Visibility = Visibility.Collapsed;
                    LeftSplitter_1.Visibility = Visibility.Collapsed;
                    RightSplitter.Visibility = Visibility.Collapsed;
                    RightSplitter_1.Visibility = Visibility.Collapsed;
                    BottomSplitter.Visibility = Visibility.Collapsed;
                    BottomSplitter_1.Visibility = Visibility.Collapsed;

                }
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "ctlGrid_PreviewDragEnter()", "Controls\\VmuktiGrid\\Grid\\CtlGrid.xaml.cs");
            }
        }
        
        //when dragging pod from title bar of pod. .. . this event will occure and will handled suggetion rectangle.
        void TopPanelContainer_MouseEnter(object sender, MouseEventArgs e)
        {
            try
            {
                if (VMuktiGrid.CustomGrid.VMuktiHelper.objPOD != null && VMuktiHelper.IsDraggingPOD && VMuktiHelper.IsRectSuggestAdded == false)
                {
                    VMuktiGrid.CustomGrid.VMuktiHelper.objPOD.rectSuggetion.Height = VMuktiHelper.RectSuggestHeight;
                    if (VMuktiGrid.CustomGrid.VMuktiHelper.objPOD.rectSuggetion.Parent != null)
                    {
                        (VMuktiGrid.CustomGrid.VMuktiHelper.objPOD.rectSuggetion.Parent as ItemsControl).Items.Remove(VMuktiGrid.CustomGrid.VMuktiHelper.objPOD.rectSuggetion);
                    }
                    TopPanelContainer.Items.Add(VMuktiGrid.CustomGrid.VMuktiHelper.objPOD.rectSuggetion);
                    VMuktiHelper.IsRectSuggestAdded = true;
                }
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "TopPanelContainer_MouseEnter()", "Controls\\VmuktiGrid\\Grid\\CtlGrid.xaml.cs");
            }
        }

        //when dragging pod from title bar of pod. .. . this event will occure and will handled suggetion rectangle.
        void TopPanelContainer_MouseLeave(object sender, MouseEventArgs e)
        {
            try
            {
                if (VMuktiGrid.CustomGrid.VMuktiHelper.objPOD != null)
                {
                    TopPanelContainer.Items.Remove(VMuktiGrid.CustomGrid.VMuktiHelper.objPOD.rectSuggetion);
                    VMuktiGrid.CustomGrid.VMuktiHelper.objPOD.rectSuggetion.Height = VMuktiHelper.RectSuggestHeight;
                    VMuktiHelper.IsRectSuggestAdded = false;
                }
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "TopPanleContainer_MouseLeave()", "Controls\\VmuktiGrid\\Grid\\CtlGrid.xaml.cs");
            }
        }
        
        //when dragging pod from title bar of pod. .. . this event will occure and will handled suggetion rectangle.
        void LeftPanelContainer_MouseEnter(object sender, MouseEventArgs e)
        {
            try
            {
                if (VMuktiGrid.CustomGrid.VMuktiHelper.objPOD != null && VMuktiHelper.IsDraggingPOD && VMuktiHelper.IsRectSuggestAdded == false)
                {
                    VMuktiGrid.CustomGrid.VMuktiHelper.objPOD.rectSuggetion.Height = VMuktiHelper.RectSuggestHeight;
                    if (VMuktiGrid.CustomGrid.VMuktiHelper.objPOD.rectSuggetion.Parent != null)
                    {
                        (VMuktiGrid.CustomGrid.VMuktiHelper.objPOD.rectSuggetion.Parent as ItemsControl).Items.Remove(VMuktiGrid.CustomGrid.VMuktiHelper.objPOD.rectSuggetion);
                    }
                    LeftPanelContainer.Items.Add(VMuktiGrid.CustomGrid.VMuktiHelper.objPOD.rectSuggetion);
                    VMuktiHelper.IsRectSuggestAdded = true;
                }
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "LeftPanelContainer_MouseEnter()", "Controls\\VmuktiGrid\\Grid\\CtlGrid.xaml.cs");
            }
        }

        //when dragging pod from title bar of pod. .. . this event will occure and will handled suggetion rectangle.
        void LeftPanelContainer_MouseLeave(object sender, MouseEventArgs e)
        {
            try
            {
                if (VMuktiGrid.CustomGrid.VMuktiHelper.objPOD != null)
                {
                    LeftPanelContainer.Items.Remove(VMuktiGrid.CustomGrid.VMuktiHelper.objPOD.rectSuggetion);
                    VMuktiGrid.CustomGrid.VMuktiHelper.objPOD.rectSuggetion.Height = VMuktiHelper.RectSuggestHeight;
                    VMuktiHelper.IsRectSuggestAdded = false;
                }
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "LefPanelcontainer_MouseLeave()", "Controls\\VmuktiGrid\\Grid\\CtlGrid.xaml.cs");
            }
        }

        //when dragging pod from title bar of pod. .. . this event will occure and will handled suggetion rectangle.
        void CentralPanelContainer_MouseEnter(object sender, MouseEventArgs e)
        {
            try
            {
                if (VMuktiGrid.CustomGrid.VMuktiHelper.objPOD != null && VMuktiHelper.IsDraggingPOD && VMuktiHelper.IsRectSuggestAdded == false)
                {
                    VMuktiGrid.CustomGrid.VMuktiHelper.objPOD.rectSuggetion.Height = VMuktiHelper.RectSuggestHeight;
                    if (VMuktiGrid.CustomGrid.VMuktiHelper.objPOD.rectSuggetion.Parent != null)
                    {
                        (VMuktiGrid.CustomGrid.VMuktiHelper.objPOD.rectSuggetion.Parent as ItemsControl).Items.Remove(VMuktiGrid.CustomGrid.VMuktiHelper.objPOD.rectSuggetion);
                    }
                    CentralPanelContainer.Items.Add(VMuktiGrid.CustomGrid.VMuktiHelper.objPOD.rectSuggetion);
                    VMuktiHelper.IsRectSuggestAdded = true;
                }
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "CentralPanleContainer_MouseEnter()", "Controls\\VmuktiGrid\\Grid\\CtlGrid.xaml.cs");
            }
        }

        //when dragging pod from title bar of pod. .. . this event will occure and will handled suggetion rectangle.
        void CentralPanelContainer_MouseLeave(object sender, MouseEventArgs e)
        {
            try
            {
                if (VMuktiGrid.CustomGrid.VMuktiHelper.objPOD != null)
                {
                    CentralPanelContainer.Items.Remove(VMuktiGrid.CustomGrid.VMuktiHelper.objPOD.rectSuggetion);
                    VMuktiGrid.CustomGrid.VMuktiHelper.objPOD.rectSuggetion.Height = VMuktiHelper.RectSuggestHeight;
                    VMuktiHelper.IsRectSuggestAdded = false;
                }
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "CenterPanelContainer_MouseLeave()", "Controls\\VmuktiGrid\\Grid\\CtlGrid.xaml.cs");
            }
        }

        //when dragging pod from title bar of pod. .. . this event will occure and will handled suggetion rectangle.
        void RightPanelContainer_MouseEnter(object sender, MouseEventArgs e)
        {
            try
            {
                if (VMuktiGrid.CustomGrid.VMuktiHelper.objPOD != null && VMuktiHelper.IsDraggingPOD && VMuktiHelper.IsRectSuggestAdded == false)
                {
                    VMuktiGrid.CustomGrid.VMuktiHelper.objPOD.rectSuggetion.Height = VMuktiHelper.RectSuggestHeight;
                    if (VMuktiGrid.CustomGrid.VMuktiHelper.objPOD.rectSuggetion.Parent != null)
                    {
                        (VMuktiGrid.CustomGrid.VMuktiHelper.objPOD.rectSuggetion.Parent as ItemsControl).Items.Remove(VMuktiGrid.CustomGrid.VMuktiHelper.objPOD.rectSuggetion);
                    }
                    RightPanelContainer.Items.Add(VMuktiGrid.CustomGrid.VMuktiHelper.objPOD.rectSuggetion);
                    VMuktiHelper.IsRectSuggestAdded = true;
                }
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "RightPanelContainer_MouseEnter()", "Controls\\VmuktiGrid\\Grid\\CtlGrid.xaml.cs");
            }
        }

        //when dragging pod from title bar of pod. .. . this event will occure and will handled suggetion rectangle.
        void RightPanelContainer_MouseLeave(object sender, MouseEventArgs e)
        {
            try
            {
                if (VMuktiGrid.CustomGrid.VMuktiHelper.objPOD != null)
                {
                    RightPanelContainer.Items.Remove(VMuktiGrid.CustomGrid.VMuktiHelper.objPOD.rectSuggetion);
                    VMuktiGrid.CustomGrid.VMuktiHelper.objPOD.rectSuggetion.Height = VMuktiHelper.RectSuggestHeight;
                    VMuktiHelper.IsRectSuggestAdded = false;
                }
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "RightPanleContainer_MouseLeave()", "Controls\\VmuktiGrid\\Grid\\CtlGrid.xaml.cs");
            }
        }

        //when dragging pod from title bar of pod. .. . this event will occure and will handled suggetion rectangle.
        void BottomPanelContainer_MouseEnter(object sender, MouseEventArgs e)
        {
            try
            {
                if (VMuktiGrid.CustomGrid.VMuktiHelper.objPOD != null && VMuktiHelper.IsDraggingPOD && VMuktiHelper.IsRectSuggestAdded == false)
                {
                    VMuktiGrid.CustomGrid.VMuktiHelper.objPOD.rectSuggetion.Height = VMuktiHelper.RectSuggestHeight;
                    if (VMuktiGrid.CustomGrid.VMuktiHelper.objPOD.rectSuggetion.Parent != null)
                    {
                        (VMuktiGrid.CustomGrid.VMuktiHelper.objPOD.rectSuggetion.Parent as ItemsControl).Items.Remove(VMuktiGrid.CustomGrid.VMuktiHelper.objPOD.rectSuggetion);
                    }
                    BottomPanelContainer.Items.Add(VMuktiGrid.CustomGrid.VMuktiHelper.objPOD.rectSuggetion);
                    VMuktiHelper.IsRectSuggestAdded = true;
                }
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "BottomPanlecontainer_MouseEnter()", "Controls\\VmuktiGrid\\Grid\\CtlGrid.xaml.cs");
            }
        }

        //when dragging pod from title bar of pod. .. . this event will occure and will handled suggetion rectangle.
        void BottomPanelContainer_MouseLeave(object sender, MouseEventArgs e)
        {
            try
            {
                if (VMuktiGrid.CustomGrid.VMuktiHelper.objPOD != null)
                {
                    BottomPanelContainer.Items.Remove(VMuktiGrid.CustomGrid.VMuktiHelper.objPOD.rectSuggetion);
                    VMuktiGrid.CustomGrid.VMuktiHelper.objPOD.rectSuggetion.Height = VMuktiHelper.RectSuggestHeight;
                    VMuktiHelper.IsRectSuggestAdded = false;
                }
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "BottomPanelContiner_MouseLeave()", "Controls\\VmuktiGrid\\Grid\\CtlGrid.xaml.cs");
            }
        }

        #endregion


        //when we will start the dragging of pod from pod's titlebar at that time this event will occur and it will handle the suggetion rectagle.
        private void OnDragPOD(object o, RoutedEventArgs args)
        {
            try
            {
                VMuktiGrid.CustomGrid.VMuktiHelper.objPOD = o as ctlPOD;
                knownParent = this.GetItemsControlContainingPanel(VMuktiGrid.CustomGrid.VMuktiHelper.objPOD);

                if (knownParent != null)
                {
                    VMuktiGrid.CustomGrid.VMuktiHelper.objPOD.intCurIndex = knownParent.Items.IndexOf(VMuktiGrid.CustomGrid.VMuktiHelper.objPOD) > 0 ? knownParent.Items.IndexOf(VMuktiGrid.CustomGrid.VMuktiHelper.objPOD) : 0;
                    knownParent.Items.Remove(VMuktiGrid.CustomGrid.VMuktiHelper.objPOD);
                    ((VMuktiGrid.ctlTab.TabItem)this.Parent).IsSaved = false;
                    this.DocumentRoot.Children.Add(VMuktiGrid.CustomGrid.VMuktiHelper.objPOD);

                    VMuktiGrid.CustomGrid.VMuktiHelper.objPOD.Width = VMuktiGrid.CustomGrid.VMuktiHelper.objPOD.RenderSize.Width;
                    VMuktiGrid.CustomGrid.VMuktiHelper.objPOD.IsHitTestVisible = false;
                    VMuktiGrid.CustomGrid.VMuktiHelper.objPOD.HorizontalAlignment = HorizontalAlignment.Left;
                    VMuktiGrid.CustomGrid.VMuktiHelper.objPOD.VerticalAlignment = VerticalAlignment.Top;
                    Grid.SetColumnSpan(VMuktiGrid.CustomGrid.VMuktiHelper.objPOD, this.DocumentRoot.ColumnDefinitions.Count);
                }
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "OnDragPOD()", "Controls\\VmuktiGrid\\Grid\\CtlGrid.xaml.cs");
            }
        }

        //when we will complete the drag and drop of pod from pod's titlebar at that time this event will occur and it will put the POD on selected place.
        private void OnDropPOD(object o, RoutedEventArgs args)
        {
            try
            {
                if (args.OriginalSource.GetType()==typeof(ctlPOD))
                {
                    VMuktiGrid.CustomGrid.VMuktiHelper.objPOD.Width = Double.NaN;
                    VMuktiGrid.CustomGrid.VMuktiHelper.objPOD.HorizontalAlignment = HorizontalAlignment.Stretch;
                    VMuktiGrid.CustomGrid.VMuktiHelper.objPOD.IsHitTestVisible = true;
                    this.DocumentRoot.Children.Remove(VMuktiGrid.CustomGrid.VMuktiHelper.objPOD);

                    //Hit-test to find out the ItemsControl under the mouse-pointer.
                    targetItemsControl = null;
                    VisualTreeHelper.HitTest(this.DocumentRoot, new HitTestFilterCallback(this.FilterHitTestResultsCallback), new HitTestResultCallback(this.HitTestResultCallback), new PointHitTestParameters(Mouse.PrimaryDevice.GetPosition(this.DocumentRoot)));
                    if (targetItemsControl != null)
                    {

                        //if (targetItemsControl.Name == "CentralPanelContainer" && VMukti.App.blnIsTwoPanel)
                        //{
                        //    fncChangItemControl();
                        //}

                        ((VMuktiGrid.ctlTab.TabItem)this.Parent).IsSaved = false;
                        VMuktiGrid.CustomGrid.VMuktiHelper.objPOD.ColNo = (int)targetItemsControl.Tag;

                        this.DocumentRoot.Children.Remove(VMuktiGrid.CustomGrid.VMuktiHelper.objPOD);
                        if (targetItemsControl.Items.IndexOf(VMuktiGrid.CustomGrid.VMuktiHelper.objPOD.rectSuggetion) >= 0)
                        {
                            targetItemsControl.Items.Insert(targetItemsControl.Items.IndexOf(VMuktiGrid.CustomGrid.VMuktiHelper.objPOD.rectSuggetion), VMuktiGrid.CustomGrid.VMuktiHelper.objPOD);
                            targetItemsControl.Items.Remove(VMuktiGrid.CustomGrid.VMuktiHelper.objPOD.rectSuggetion);

                            VMuktiGrid.CustomGrid.VMuktiHelper.objPOD.intCurIndex = targetItemsControl.Items.IndexOf(VMuktiGrid.CustomGrid.VMuktiHelper.objPOD) > 0 ? targetItemsControl.Items.IndexOf(VMuktiGrid.CustomGrid.VMuktiHelper.objPOD) : 0;
                        }

                    }
                    else if (knownParent != null)
                    {
                        //if (knownParent.Name == "CentralPanelContainer" && VMukti.App.blnIsTwoPanel)
                        //{
                        //    fncChangItemControl();
                        //}
                        ((VMuktiGrid.ctlTab.TabItem)this.Parent).IsSaved = false;
                        VMuktiGrid.CustomGrid.VMuktiHelper.objPOD.ColNo = (int)knownParent.Tag;

                        this.DocumentRoot.Children.Remove(VMuktiGrid.CustomGrid.VMuktiHelper.objPOD); 
                        if (knownParent.Items.IndexOf(VMuktiGrid.CustomGrid.VMuktiHelper.objPOD.rectSuggetion) >= 0)
                        {
                            knownParent.Items.Insert(knownParent.Items.IndexOf(VMuktiGrid.CustomGrid.VMuktiHelper.objPOD.rectSuggetion), VMuktiGrid.CustomGrid.VMuktiHelper.objPOD);
                            knownParent.Items.Remove(VMuktiGrid.CustomGrid.VMuktiHelper.objPOD.rectSuggetion);
                        }

                    }
                    
                }
                else if (args.OriginalSource.GetType() == typeof(int) && knownParent != null)
                {
                    //if (knownParent.Name == "CentralPanelContainer" && VMukti.App.blnIsTwoPanel)
                    //{
                    //    fncChangItemControl();
                    //}

                    VMuktiGrid.CustomGrid.VMuktiHelper.objPOD.Width = Double.NaN;
                    VMuktiGrid.CustomGrid.VMuktiHelper.objPOD.HorizontalAlignment = HorizontalAlignment.Stretch;
                    VMuktiGrid.CustomGrid.VMuktiHelper.objPOD.IsHitTestVisible = true;
                    this.DocumentRoot.Children.Remove(VMuktiGrid.CustomGrid.VMuktiHelper.objPOD);
                    if (int.Parse(args.OriginalSource.ToString()) <= knownParent.Items.Count)
                    {
                        knownParent.Items.Insert(int.Parse(args.OriginalSource.ToString()), VMuktiGrid.CustomGrid.VMuktiHelper.objPOD);
                    }
                    else
                    {
                        knownParent.Items.Add(VMuktiGrid.CustomGrid.VMuktiHelper.objPOD);
                    }
                }
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "OnDragPOD--2", "Controls\\VmuktiGrid\\Grid\\CtlGrid.xaml.cs");
            }
        }


        #region changeNIS

        //void fncChangItemControl()
        //{
        //    try
        //    {
        //        for (int i = 0; i < CentralPanelContainer.Items.Count; i++)
        //        {
        //            if (CentralPanelContainer.Items[i].GetType() == typeof(ctlPOD))
        //            {
        //                ctlPOD objpod = CentralPanelContainer.Items[i] as ctlPOD;

        //                CentralPanelContainer.Items.RemoveAt(i);

        //                RightPanelContainer.Items.Add(objpod);

        //            }

        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "fncChangItemControl", "Controls\\VmuktiGrid\\Grid\\CtlGrid.xaml.cs");
            
        //    }
        //}

        #endregion
        //it will return the ItemsControl on which current pod is - hit testing
        private ItemsControl GetItemsControlContainingPanel(ctlPOD panel)
        {
            try
            {
                if (this.IsPanelInItemsControl(panel, this.LeftPanelContainer))
                {
                    return this.LeftPanelContainer;
                }
                else if (this.IsPanelInItemsControl(panel, this.CentralPanelContainer))
                {
                    return this.CentralPanelContainer;
                }
                else if (this.IsPanelInItemsControl(panel, this.RightPanelContainer))
                {
                    return this.RightPanelContainer;
                }
                else if (this.IsPanelInItemsControl(panel, this.TopPanelContainer))
                {
                    return this.TopPanelContainer;
                }
                else if (this.IsPanelInItemsControl(panel, this.BottomPanelContainer))
                {
                    return this.BottomPanelContainer;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "GetItemsControlContainingPanel()", "Controls\\VmuktiGrid\\Grid\\CtlGrid.xaml.cs");                
                return null;
            }
        }

        //checking wheather perticular pod is in the itemscontrol or not
        private bool IsPanelInItemsControl(ctlPOD panel, ItemsControl itemsControl)
        {
            try
            {
                for (int i = 0; i < itemsControl.Items.Count; i++)
                {
                    if (itemsControl.Items[i].GetType() == typeof(ctlPOD) && itemsControl.Items[i] == panel)
                    {
                        return true;
                    }
                }
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "IsPanelInItemControl()", "Controls\\VmuktiGrid\\Grid\\CtlGrid.xaml.cs");
            }
            return false;
        }

        //identifying the hittest with perticular filter of current mouse pointer position.
        private HitTestFilterBehavior FilterHitTestResultsCallback(DependencyObject target)
        {
            if (target.GetType().IsAssignableFrom(typeof(ItemsControl)))
            {
                targetItemsControl = target as ItemsControl;
                return HitTestFilterBehavior.Stop;
            }
            else
            {
                return HitTestFilterBehavior.Continue;
            }
        }

        //identifying the hittest with perticular filter of current mouse pointer position.
        private HitTestResultBehavior HitTestResultCallback(HitTestResult result)
        {
            return HitTestResultBehavior.Stop;
        }

        //to add the pod on grid from dtabase 
        public void AddControl(string strTitle, int moduleID, int colNumber)
        {
            try
            {
                #region test data
                if (colNumber == 1)
                {
                    //Button objButton = new Button();
                    //objButton.Content = (11111 * moduleID).ToString();
                    //ctlPOD objPod = new ctlPOD(strTitle, moduleID, (UIElement)objButton);
                    //LeftPanelContainer.Items.Add(objPod);
                }
                else if (colNumber == 2)
                {
                    //Button objButton = new Button();
                    //objButton.Content = (11111 * moduleID).ToString();
                    //ctlPOD objPod = new ctlPOD(strTitle, moduleID, (UIElement)objButton);
                    //CentralPanelContainer.Items.Add(objPod);
                }
                else if (colNumber == 3)
                {
                    //Button objButton = new Button();
                    //objButton.Content = (11111 * moduleID).ToString();
                    //ctlPOD objPod = new ctlPOD(strTitle, moduleID, (UIElement)objButton);
                    //RightPanelContainer.Items.Add(objPod);
                }
                else if (colNumber == 4)
                {
                    //Button objButton = new Button();
                    //objButton.Content = (11111 * moduleID).ToString();
                    //ctlPOD objPod = new ctlPOD(strTitle, moduleID, (UIElement)objButton);
                    //TopPanelContainer.Items.Add(objPod);
                }
                else if (colNumber == 5)
                {
                    //Button objButton = new Button();
                    //objButton.Content = (11111 * moduleID).ToString();
                    //ctlPOD objPod = new ctlPOD(strTitle, moduleID, (UIElement)objButton);
                    //BottomPanelContainer.Items.Add(objPod);
                }
                //else if (colNumber == 4)
                //{
                //    Button btnTemp = new Button();
                //    btnTemp.Height = 100;
                //    btnTemp.Content = strTitle;
                //    LeftPanelContainer.Items.Add(btnTemp);
                //}
                #endregion
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "AddControl()", "Controls\\VmuktiGrid\\Grid\\CtlGrid.xaml.cs");
            }
        }

        //to add the pod on grid from another person(host)when dropped.
        public void AddControl(int intModID, string strModTitle, string strIsCollaborative, string strURI, int[] arrPermissionValue, bool flag, string strFromWhere)
        {
            try
            {
                if (LeftPanelContainer.Items.Count == 0 || (LeftPanelContainer.Items.Count < CentralPanelContainer.Items.Count && LeftPanelContainer.Items.Count < RightPanelContainer.Items.Count) || (LeftPanelContainer.Items.Count == CentralPanelContainer.Items.Count && LeftPanelContainer.Items.Count == RightPanelContainer.Items.Count))
                {
                    ctlPOD objPod = new ctlPOD(intModID, strModTitle, strIsCollaborative, strURI, arrPermissionValue, flag, strFromWhere, LeftPanelContainer, true, null);
                    objPod.OwnerPodIndex = VMukti.App.podCounter++;
                }
                else if (CentralPanelContainer.Items.Count == 0 || (CentralPanelContainer.Items.Count < LeftPanelContainer.Items.Count && CentralPanelContainer.Items.Count < RightPanelContainer.Items.Count))
                {
                    ctlPOD objPod = new ctlPOD(intModID, strModTitle, strIsCollaborative, strURI, arrPermissionValue, flag, strFromWhere, CentralPanelContainer, true, null);
                    objPod.OwnerPodIndex = VMukti.App.podCounter++;
                }
                else if (RightPanelContainer.Items.Count == 0 || (RightPanelContainer.Items.Count < LeftPanelContainer.Items.Count && RightPanelContainer.Items.Count < CentralPanelContainer.Items.Count))
                {
                    ctlPOD objPod = new ctlPOD(intModID, strModTitle, strIsCollaborative, strURI, arrPermissionValue, flag, strFromWhere, RightPanelContainer, true, null);
                    objPod.OwnerPodIndex = VMukti.App.podCounter++;
                }
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "addControl()--1", "Controls\\VmuktiGrid\\Grid\\CtlGrid.xaml.cs");
            }
        }

        private void DocumentRoot_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            try
            {
                //((ctlGrid)DocumentRoot.Parent).Height = ((ctlGrid)DocumentRoot.Parent).Height + (e.NewSize.Height - e.PreviousSize.Height);
                //((VMuktiGrid.ctlTab.TabControl)((VMuktiGrid.ctlTab.TabItem)((ctlGrid)DocumentRoot.Parent).Parent).Parent).Height = ((ctlGrid)DocumentRoot.Parent).Height + (e.NewSize.Height - e.PreviousSize.Height);
                //((Grid)((VMuktiGrid.ctlPage.TabControl)((VMuktiGrid.ctlPage.TabItem)((VMuktiGrid.ctlTab.TabControl)((VMuktiGrid.ctlTab.TabItem)((ctlGrid)DocumentRoot.Parent).Parent).Parent).Parent).Parent).Parent).Height = ((ctlGrid)DocumentRoot.Parent).Height + (e.NewSize.Height - e.PreviousSize.Height);
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "DocumentRoot_SizeChanged()", "Controls\\VmuktiGrid\\Grid\\CtlGrid.xaml.cs");
            }
        }

        //to save each and every pods in all itemscontrol
        public void Save(int tabID)
        {
            try
            {
                foreach (VMuktiGrid.CustomGrid.ctlPOD item in LeftPanelContainer.Items)
                {
                    item.ColNo = 1;
                    item.Save(tabID);
                }
                foreach (VMuktiGrid.CustomGrid.ctlPOD item in CentralPanelContainer.Items)
                {
                    item.ColNo = 2;
                    item.Save(tabID);
                }
                foreach (VMuktiGrid.CustomGrid.ctlPOD item in RightPanelContainer.Items)
                {
                    item.ColNo = 3;
                    item.Save(tabID);
                }
                foreach (VMuktiGrid.CustomGrid.ctlPOD item in TopPanelContainer.Items)
                {
                    item.ColNo = 4;
                    item.Save(tabID);
                }
                foreach (VMuktiGrid.CustomGrid.ctlPOD item in BottomPanelContainer.Items)
                {
                    item.ColNo = 5;
                    item.Save(tabID);
                }
                VMukti.Business.VMuktiGrid.ClsPod tempPOD = new VMukti.Business.VMuktiGrid.ClsPod();
                for (int i = 0; i < lstDeletePods.Count; i++)
                {
                    tempPOD.Remove_Pod(lstDeletePods[i]);
                }
                lstDeletePods.Clear();
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "Save()", "Controls\\VmuktiGrid\\Grid\\CtlGrid.xaml.cs");
            }
        }

        //to load whole grid design from database from relative tabid from database (pages are saved)
        public void LoadGrid(int tabID)
        {
            try
            {
                VMukti.Business.VMuktiGrid.ClsTab objTab = VMukti.Business.VMuktiGrid.ClsTab.Get_Tab(tabID);
                DocumentRoot.ColumnDefinitions[0].Width = new GridLength(objTab.C1Width, GridUnitType.Star);
                DocumentRoot.ColumnDefinitions[1].Width = new GridLength(objTab.C2Width, GridUnitType.Star);
                DocumentRoot.ColumnDefinitions[2].Width = new GridLength(objTab.C3Width, GridUnitType.Star);

                DocumentRoot.RowDefinitions[0].Height = new GridLength(objTab.C4Height, GridUnitType.Pixel);
                DocumentRoot.RowDefinitions[2].Height = new GridLength(objTab.C5Height, GridUnitType.Pixel);

                VMukti.Business.VMuktiGrid.ClsPodCollection objPODs = VMukti.Business.VMuktiGrid.ClsPodCollection.GetAll(tabID);
                foreach (VMukti.Business.VMuktiGrid.ClsPod item in objPODs)
                {
                    //AddControl(item.ModuleId, item.PodTitle, item.ModuleId item.ColumnId);
                    VMukti.Business.VMuktiGrid.ClsModule objModule = VMukti.Business.VMuktiGrid.ClsModule.GetPodModule(item.ModuleId);

                    ClsPermissionCollection objCPC = ClsPermissionCollection.Get_PermissionRefModule(item.ModuleId, VMuktiAPI.VMuktiInfo.CurrentPeer.RoleID);
                    int[] arrPermissionValue = new int[objCPC.Count];
                    for (int percount = 0; percount < objCPC.Count; percount++)
                    {
                        arrPermissionValue[percount] = objCPC[percount].PermissionValue;
                    }

                    if (item.ColumnId == 1)
                    {
                        LeftPanelContainer.Items.Add(new ctlPOD(item.ModuleId, item.PodTitle, objModule.IsCollaborative, null, arrPermissionValue, false, "fromDatabase", null, true, null));
                        ((ctlPOD)LeftPanelContainer.Items[LeftPanelContainer.Items.Count - 1]).ObjectID = item.PodId;
                        ((ctlPOD)LeftPanelContainer.Items[LeftPanelContainer.Items.Count - 1]).IsTwoPanel = false;
                    }
                    else if (item.ColumnId == 2)
                    {
                        CentralPanelContainer.Items.Add(new ctlPOD(item.ModuleId, item.PodTitle, objModule.IsCollaborative, null, arrPermissionValue, false, "fromDatabase", null, true, null));
                        ((ctlPOD)CentralPanelContainer.Items[CentralPanelContainer.Items.Count - 1]).ObjectID = item.PodId;
                        ((ctlPOD)CentralPanelContainer.Items[CentralPanelContainer.Items.Count - 1]).IsTwoPanel = false;
                    }
                    else if (item.ColumnId == 3)
                    {
                        RightPanelContainer.Items.Add(new ctlPOD(item.ModuleId, item.PodTitle, objModule.IsCollaborative, null, arrPermissionValue, false, "fromDatabase", null, true, null));
                        ((ctlPOD)RightPanelContainer.Items[RightPanelContainer.Items.Count - 1]).ObjectID = item.PodId;
                        ((ctlPOD)RightPanelContainer.Items[RightPanelContainer.Items.Count - 1]).IsTwoPanel = true;
                    }
                    else if (item.ColumnId == 4)
                    {
                        TopPanelContainer.Items.Add(new ctlPOD(item.ModuleId, item.PodTitle, objModule.IsCollaborative, null, arrPermissionValue, false, "fromDatabase", null, true, null));
                        ((ctlPOD)TopPanelContainer.Items[TopPanelContainer.Items.Count - 1]).ObjectID = item.PodId;
                        ((ctlPOD)TopPanelContainer.Items[TopPanelContainer.Items.Count - 1]).IsTwoPanel = false;
                    }
                    else if (item.ColumnId == 5)
                    {
                        BottomPanelContainer.Items.Add(new ctlPOD(item.ModuleId, item.PodTitle, objModule.IsCollaborative, null, arrPermissionValue, false, "fromDatabase", null, true, null));
                        ((ctlPOD)BottomPanelContainer.Items[BottomPanelContainer.Items.Count - 1]).ObjectID = item.PodId;
                        ((ctlPOD)BottomPanelContainer.Items[BottomPanelContainer.Items.Count - 1]).IsTwoPanel = false;
                    }
                }
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "LoadGrid()", "Controls\\VmuktiGrid\\Grid\\CtlGrid.xaml.cs");
            }
        }

        //to load whole grid design from another person/host from relative tabindex from object of clsPageInfo class (pages are not saved)... when we already in meeting with another user for widget and u drop another widget on same tab and drop the same use again this function will ensure new widget opens in the same meeting tab instead into new tab.
        public void LoadMeetingGrid(clsPageInfo objPageInfo, int intTabIndex)
        {
            try
            {
                clsTabInfo objTabInfo = objPageInfo.objaTabs[intTabIndex];

                DocumentRoot.ColumnDefinitions[0].Width = new GridLength(objTabInfo.dblC1Width, GridUnitType.Star);
                DocumentRoot.ColumnDefinitions[1].Width = new GridLength(objTabInfo.dblC2Width, GridUnitType.Star);
                DocumentRoot.ColumnDefinitions[2].Width = new GridLength(objTabInfo.dblC3Width, GridUnitType.Star);

                DocumentRoot.RowDefinitions[0].Height = new GridLength(objTabInfo.dblC4Height, GridUnitType.Pixel);
                DocumentRoot.RowDefinitions[2].Height = new GridLength(objTabInfo.dblC5Height, GridUnitType.Pixel);

                int i = 0;
                int j = 0;

                for (i = 0; i < objTabInfo.objaPods.Length; i++)
                {
                    VMukti.Business.VMuktiGrid.ClsModule objModule = VMukti.Business.VMuktiGrid.ClsModule.GetPodModule(objTabInfo.objaPods[i].intModuleId);

                    ClsPermissionCollection objCPC = ClsPermissionCollection.Get_PermissionRefModule(objTabInfo.objaPods[i].intModuleId, VMuktiAPI.VMuktiInfo.CurrentPeer.RoleID);
                    int[] arrPermissionValue = new int[objCPC.Count];
                    for (int percount = 0; percount < objCPC.Count; percount++)
                    {
                        arrPermissionValue[percount] = objCPC[percount].PermissionValue;
                    }

                    if (objTabInfo.objaPods[i].intColumnNumber == 1)
                    {
                        for (j = 0; j < LeftPanelContainer.Items.Count; j++)
                        {
                            if (((ctlPOD)LeftPanelContainer.Items[j]).OwnerPodIndex == objTabInfo.objaPods[i].intOwnerPodIndex && objPageInfo.strDropType != "OnBuddyClick")
                            {
                                break;
                            }
                        }
                        if (j == LeftPanelContainer.Items.Count)
                        {
                            ctlPOD objPod = null;
                            if (VMuktiAPI.VMuktiInfo.CurrentPeer.CurrPeerType == PeerType.BootStrap || VMuktiAPI.VMuktiInfo.CurrentPeer.CurrPeerType == PeerType.SuperNode || VMuktiAPI.VMuktiInfo.CurrentPeer.CurrPeerType == PeerType.NodeWithNetP2P)
                            {
                                objPod = new ctlPOD(objTabInfo.objaPods[i].intModuleId, objTabInfo.objaPods[i].strPodTitle, objModule.IsCollaborative, objTabInfo.objaPods[i].strUri[0], arrPermissionValue, false, "fromOtherPeer", LeftPanelContainer, true, objPageInfo);
                            }
                            else if (VMuktiAPI.VMuktiInfo.CurrentPeer.CurrPeerType == PeerType.NodeWithHttp)
                            {
                                objPod = new ctlPOD(objTabInfo.objaPods[i].intModuleId, objTabInfo.objaPods[i].strPodTitle, objModule.IsCollaborative, objTabInfo.objaPods[i].strUri[1], arrPermissionValue, false, "fromOtherPeer", LeftPanelContainer, true, objPageInfo);
                            }
                            objPod.OwnerPodIndex = objTabInfo.objaPods[i].intOwnerPodIndex;
                            objPod.IsTwoPanel = true;
                        }
                    }
                    else if (objTabInfo.objaPods[i].intColumnNumber == 2)
                    {
                        for (j = 0; j < CentralPanelContainer.Items.Count; j++)
                        {
                            if (((ctlPOD)CentralPanelContainer.Items[j]).OwnerPodIndex == objTabInfo.objaPods[i].intOwnerPodIndex && objPageInfo.strDropType != "OnBuddyClick")
                            {
                                break;
                            }
                        }
                        if (j == CentralPanelContainer.Items.Count)
                        {
                            ctlPOD objPod = null;
                            if (VMuktiAPI.VMuktiInfo.CurrentPeer.CurrPeerType == PeerType.BootStrap || VMuktiAPI.VMuktiInfo.CurrentPeer.CurrPeerType == PeerType.SuperNode || VMuktiAPI.VMuktiInfo.CurrentPeer.CurrPeerType == PeerType.NodeWithNetP2P)
                            {
                                objPod = new ctlPOD(objTabInfo.objaPods[i].intModuleId, objTabInfo.objaPods[i].strPodTitle, objModule.IsCollaborative, objTabInfo.objaPods[i].strUri[0], arrPermissionValue, false, "fromOtherPeer", CentralPanelContainer, true, objPageInfo);
                            }
                            else if (VMuktiAPI.VMuktiInfo.CurrentPeer.CurrPeerType == PeerType.NodeWithHttp)
                            {
                                objPod = new ctlPOD(objTabInfo.objaPods[i].intModuleId, objTabInfo.objaPods[i].strPodTitle, objModule.IsCollaborative, objTabInfo.objaPods[i].strUri[1], arrPermissionValue, false, "fromOtherPeer", CentralPanelContainer, true, objPageInfo);
                            }
                            objPod.OwnerPodIndex = objTabInfo.objaPods[i].intOwnerPodIndex;
                        }
                    }
                    else if (objTabInfo.objaPods[i].intColumnNumber == 3)
                    {
                        for (j = 0; j < RightPanelContainer.Items.Count; j++)
                        {
                            if (((ctlPOD)RightPanelContainer.Items[j]).OwnerPodIndex == objTabInfo.objaPods[i].intOwnerPodIndex && objPageInfo.strDropType != "OnBuddyClick")
                            {
                                break;
                            }
                        }
                        if (j == RightPanelContainer.Items.Count)
                        {
                            ctlPOD objPod = null;
                            if (VMuktiAPI.VMuktiInfo.CurrentPeer.CurrPeerType == PeerType.BootStrap || VMuktiAPI.VMuktiInfo.CurrentPeer.CurrPeerType == PeerType.SuperNode || VMuktiAPI.VMuktiInfo.CurrentPeer.CurrPeerType == PeerType.NodeWithNetP2P)
                            {
                                objPod = new ctlPOD(objTabInfo.objaPods[i].intModuleId, objTabInfo.objaPods[i].strPodTitle, objModule.IsCollaborative, objTabInfo.objaPods[i].strUri[0], arrPermissionValue, false, "fromOtherPeer", RightPanelContainer, true, objPageInfo);
                            }
                            else if (VMuktiAPI.VMuktiInfo.CurrentPeer.CurrPeerType == PeerType.NodeWithHttp)
                            {
                                objPod = new ctlPOD(objTabInfo.objaPods[i].intModuleId, objTabInfo.objaPods[i].strPodTitle, objModule.IsCollaborative, objTabInfo.objaPods[i].strUri[1], arrPermissionValue, false, "fromOtherPeer", RightPanelContainer, true, objPageInfo);
                            }
                            objPod.OwnerPodIndex = objTabInfo.objaPods[i].intOwnerPodIndex;
                        }
                    }
                    else if (objTabInfo.objaPods[i].intColumnNumber == 4)
                    {
                        for (j = 0; j < TopPanelContainer.Items.Count; j++)
                        {
                            if (((ctlPOD)TopPanelContainer.Items[j]).OwnerPodIndex == objTabInfo.objaPods[i].intOwnerPodIndex && objPageInfo.strDropType != "OnBuddyClick")
                            {
                                break;
                            }
                        }
                        if (j == TopPanelContainer.Items.Count)
                        {
                            ctlPOD objPod = null;
                            if (VMuktiAPI.VMuktiInfo.CurrentPeer.CurrPeerType == PeerType.BootStrap || VMuktiAPI.VMuktiInfo.CurrentPeer.CurrPeerType == PeerType.SuperNode || VMuktiAPI.VMuktiInfo.CurrentPeer.CurrPeerType == PeerType.NodeWithNetP2P)
                            {
                                objPod = new ctlPOD(objTabInfo.objaPods[i].intModuleId, objTabInfo.objaPods[i].strPodTitle, objModule.IsCollaborative, objTabInfo.objaPods[i].strUri[0], arrPermissionValue, false, "fromOtherPeer", TopPanelContainer, true, objPageInfo);
                            }
                            else if (VMuktiAPI.VMuktiInfo.CurrentPeer.CurrPeerType == PeerType.NodeWithHttp)
                            {
                                objPod = new ctlPOD(objTabInfo.objaPods[i].intModuleId, objTabInfo.objaPods[i].strPodTitle, objModule.IsCollaborative, objTabInfo.objaPods[i].strUri[1], arrPermissionValue, false, "fromOtherPeer", TopPanelContainer, true, objPageInfo);
                            }
                            objPod.OwnerPodIndex = objTabInfo.objaPods[i].intOwnerPodIndex;
                        }
                    }
                    else if (objTabInfo.objaPods[i].intColumnNumber == 5)
                    {
                        for (j = 0; j < BottomPanelContainer.Items.Count; j++)
                        {
                            if (((ctlPOD)BottomPanelContainer.Items[j]).OwnerPodIndex == objTabInfo.objaPods[i].intOwnerPodIndex && objPageInfo.strDropType != "OnBuddyClick")
                            {
                                break;
                            }
                        }
                        if (j == BottomPanelContainer.Items.Count)
                        {
                            ctlPOD objPod = null;
                            if (VMuktiAPI.VMuktiInfo.CurrentPeer.CurrPeerType == PeerType.BootStrap || VMuktiAPI.VMuktiInfo.CurrentPeer.CurrPeerType == PeerType.SuperNode || VMuktiAPI.VMuktiInfo.CurrentPeer.CurrPeerType == PeerType.NodeWithNetP2P)
                            {
                                objPod = new ctlPOD(objTabInfo.objaPods[i].intModuleId, objTabInfo.objaPods[i].strPodTitle, objModule.IsCollaborative, objTabInfo.objaPods[i].strUri[0], arrPermissionValue, false, "fromOtherPeer", BottomPanelContainer, true, objPageInfo);
                            }
                            else if (VMuktiAPI.VMuktiInfo.CurrentPeer.CurrPeerType == PeerType.NodeWithHttp)
                            {
                                objPod = new ctlPOD(objTabInfo.objaPods[i].intModuleId, objTabInfo.objaPods[i].strPodTitle, objModule.IsCollaborative, objTabInfo.objaPods[i].strUri[1], arrPermissionValue, false, "fromOtherPeer", BottomPanelContainer, true, objPageInfo);
                            }
                            objPod.OwnerPodIndex = objTabInfo.objaPods[i].intOwnerPodIndex;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "LoadMeetingGrid()", "Controls\\VmuktiGrid\\Grid\\CtlGrid.xaml.cs");
            }
        }

        //to load whole grid design from another person/host from new page object from object of clsPageInfo class (pages are not saved) when same page is not found.
        public void LoadNewMeetingGrid(clsPageInfo objPageInfo, int intTabIndex)
        {
            try
            {
                clsTabInfo objTabInfo = objPageInfo.objaTabs[intTabIndex];

                DocumentRoot.ColumnDefinitions[0].Width = new GridLength(objTabInfo.dblC1Width, GridUnitType.Star);
                DocumentRoot.ColumnDefinitions[1].Width = new GridLength(objTabInfo.dblC2Width, GridUnitType.Star);
                DocumentRoot.ColumnDefinitions[2].Width = new GridLength(objTabInfo.dblC3Width, GridUnitType.Star);

                DocumentRoot.RowDefinitions[0].Height = new GridLength(objTabInfo.dblC4Height, GridUnitType.Pixel);
                DocumentRoot.RowDefinitions[2].Height = new GridLength(objTabInfo.dblC5Height, GridUnitType.Pixel);

                foreach (clsPodInfo item in objTabInfo.objaPods)
                {
                    //AddControl(item.ModuleId, item.PodTitle, item.ModuleId item.ColumnId);
                    VMukti.Business.VMuktiGrid.ClsModule objModule = VMukti.Business.VMuktiGrid.ClsModule.GetPodModule(item.intModuleId);

                    ClsPermissionCollection objCPC = ClsPermissionCollection.Get_PermissionRefModule(item.intModuleId, VMuktiAPI.VMuktiInfo.CurrentPeer.RoleID);
                    int[] arrPermissionValue = new int[objCPC.Count];
                    for (int percount = 0; percount < objCPC.Count; percount++)
                    {
                        arrPermissionValue[percount] = objCPC[percount].PermissionValue;
                    }
                    ctlPOD objPod = null;
                    if (VMuktiAPI.VMuktiInfo.CurrentPeer.CurrPeerType == PeerType.BootStrap || VMuktiAPI.VMuktiInfo.CurrentPeer.CurrPeerType == PeerType.SuperNode || VMuktiAPI.VMuktiInfo.CurrentPeer.CurrPeerType == PeerType.NodeWithNetP2P)
                    {
                        objPod = new ctlPOD(item.intModuleId, item.strPodTitle, objModule.IsCollaborative, item.strUri[0], arrPermissionValue, false, "fromOtherPeer", null, true, objPageInfo);
                    }
                    else if (VMuktiAPI.VMuktiInfo.CurrentPeer.CurrPeerType == PeerType.NodeWithHttp)
                    {
                        objPod = new ctlPOD(item.intModuleId, item.strPodTitle, objModule.IsCollaborative, item.strUri[1], arrPermissionValue, false, "fromOtherPeer", null, true, objPageInfo);
                    }

                    if (item.intColumnNumber == 1)
                    {
                        //LeftPanelContainer.Items.Add(objPod);
                        //objPod.ColNo = 1;
                        objPod.ObjectID = item.intPodID;
                        objPod.Tag = item.intPodID;
                        objPod.ColNo = 1;
                        LeftPanelContainer.Items.Add(objPod);
                    }
                    else if (item.intColumnNumber == 2)
                    { 
                        //CentralPanelContainer.Items.Add(objPod);
                        //objPod.ColNo = 2;
                        objPod.ObjectID = item.intPodID;
                        objPod.Tag = item.intPodID;
                        objPod.ColNo = 2;
                        CentralPanelContainer.Items.Add(objPod);
                    }
                    else if (item.intColumnNumber == 3)
                    {
                        //RightPanelContainer.Items.Add(objPod);
                        //objPod.ColNo = 3;
                        objPod.ObjectID = item.intPodID;
                        objPod.Tag = item.intPodID;
                        objPod.ColNo = 3;
                        objPod.IsTwoPanel = true;
                        RightPanelContainer.Items.Add(objPod);
                    }
                    else if (item.intColumnNumber == 4)
                    {
                        //TopPanelContainer.Items.Add(objPod);
                        //objPod.ColNo = 4;
                        objPod.ObjectID = item.intPodID;
                        objPod.Tag = item.intPodID;
                        objPod.ColNo = 4;
                        TopPanelContainer.Items.Add(objPod);
                    }
                    else if (item.intColumnNumber == 5)
                    {
                        //BottomPanelContainer.Items.Add(objPod);
                        //objPod.ColNo = 5;
                        objPod.ObjectID = item.intPodID;
                        objPod.Tag = item.intPodID;
                        objPod.ColNo = 5;
                        BottomPanelContainer.Items.Add(objPod);
                    } 
                    objPod.OwnerPodIndex = item.intOwnerPodIndex;

                    foreach (string strBuddyName in item.straPodBuddies)
                    {
                        //objPod.AddBuddy(strBuddyName);
                        //objPod.SetMaxCounter(1, strBuddyName);
                        if (strBuddyName != VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName)
                        {
                            objPod.AddBuddy(strBuddyName);
                            objPod.SetMaxCounter(1, strBuddyName);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "LoadNewMeetinggrid()", "Controls\\VmuktiGrid\\Grid\\CtlGrid.xaml.cs");
            }
        }

        //no use could be depricated in future after safe testing, partialy implemented feature for showing the buddy after buddy has loaded the module successfully.
        public void SetReturnBuddyStatus(clsBuddyRetPageInfo objBuddyRetPageInfo)
        {
            try
            {
               
                int j = 0;

                if (objBuddyRetPageInfo.objaTabs[0].objaPods[0].intColumnNumber == 1)
                {
                    for (j = 0; j < LeftPanelContainer.Items.Count; j++)
                    {
                        if (((ctlPOD)LeftPanelContainer.Items[j]).OwnerPodIndex == objBuddyRetPageInfo.objaTabs[0].objaPods[0].intOwnerPodIndex)
                        {
                            ((ctlPOD)LeftPanelContainer.Items[j]).SetReturnBuddyStatus(objBuddyRetPageInfo.strFrom);
                            break;
                        }
                    }
                }
                else if (objBuddyRetPageInfo.objaTabs[0].objaPods[0].intColumnNumber == 2)
                {
                    for (j = 0; j < CentralPanelContainer.Items.Count; j++)
                    {
                        if (((ctlPOD)CentralPanelContainer.Items[j]).OwnerPodIndex == objBuddyRetPageInfo.objaTabs[0].objaPods[0].intOwnerPodIndex)
                        {
                            ((ctlPOD)CentralPanelContainer.Items[j]).SetReturnBuddyStatus(objBuddyRetPageInfo.strFrom);
                            break;
                        }
                    }
                }
                else if (objBuddyRetPageInfo.objaTabs[0].objaPods[0].intColumnNumber == 3)
                {
                    for (j = 0; j < RightPanelContainer.Items.Count; j++)
                    {
                        if (((ctlPOD)RightPanelContainer.Items[j]).OwnerPodIndex == objBuddyRetPageInfo.objaTabs[0].objaPods[0].intOwnerPodIndex)
                        {
                            ((ctlPOD)RightPanelContainer.Items[j]).SetReturnBuddyStatus(objBuddyRetPageInfo.strFrom);
                            break;
                        }
                    }
                }
                else if (objBuddyRetPageInfo.objaTabs[0].objaPods[0].intColumnNumber == 4)
                {
                    for (j = 0; j < TopPanelContainer.Items.Count; j++)
                    {
                        if (((ctlPOD)TopPanelContainer.Items[j]).OwnerPodIndex == objBuddyRetPageInfo.objaTabs[0].objaPods[0].intOwnerPodIndex)
                        {
                            ((ctlPOD)TopPanelContainer.Items[j]).SetReturnBuddyStatus(objBuddyRetPageInfo.strFrom);
                            break;
                        }
                    }
                }
                else if (objBuddyRetPageInfo.objaTabs[0].objaPods[0].intColumnNumber == 5)
                {
                    for (j = 0; j < BottomPanelContainer.Items.Count; j++)
                    {
                        if (((ctlPOD)BottomPanelContainer.Items[j]).OwnerPodIndex == objBuddyRetPageInfo.objaTabs[0].objaPods[0].intOwnerPodIndex)
                        {
                            ((ctlPOD)BottomPanelContainer.Items[j]).SetReturnBuddyStatus(objBuddyRetPageInfo.strFrom);
                            break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "SetReturnBuddyStatus()", "Controls\\VmuktiGrid\\Grid\\CtlGrid.xaml.cs");
            }
        }

        //to delete the pod from itemscontrol
        public void DeletePOD(int podID)
        {
            try
            {
                lstDeletePods.Add(podID);
                ((VMuktiGrid.ctlTab.TabItem)this.Parent).IsSaved = false;
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "DeletePOD()", "Controls\\VmuktiGrid\\Grid\\CtlGrid.xaml.cs");
            }            
        }

        //to add buddy in the pod's buddy list, whead dropping buddy on tab/page
        public void AddBuddy(string strBuddyName)
        {
            try
            {
                foreach (VMuktiGrid.CustomGrid.ctlPOD item in LeftPanelContainer.Items)
                {
                    item.AddBuddy(strBuddyName);
                    item.SetMaxCounter(1, strBuddyName);
                }
                foreach (VMuktiGrid.CustomGrid.ctlPOD item in CentralPanelContainer.Items)
                {
                    item.AddBuddy(strBuddyName);
                    item.SetMaxCounter(1, strBuddyName);
                }
                foreach (VMuktiGrid.CustomGrid.ctlPOD item in RightPanelContainer.Items)
                {
                    item.AddBuddy(strBuddyName);
                    item.SetMaxCounter(1, strBuddyName);
                }
                foreach (VMuktiGrid.CustomGrid.ctlPOD item in TopPanelContainer.Items)
                {
                    item.AddBuddy(strBuddyName);
                    item.SetMaxCounter(1, strBuddyName);
                }
                foreach (VMuktiGrid.CustomGrid.ctlPOD item in BottomPanelContainer.Items)
                {
                    item.AddBuddy(strBuddyName);
                    item.SetMaxCounter(1, strBuddyName);
                }
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "Addbuddy()", "Controls\\VmuktiGrid\\Grid\\CtlGrid.xaml.cs");
            }
        }


        public void RemoveBuddy(string strBuddyName)
        {
            try
            {
                foreach (VMuktiGrid.CustomGrid.ctlPOD item in LeftPanelContainer.Items)
                {
                    item.RemoveBuddy(strBuddyName);
                }
                foreach (VMuktiGrid.CustomGrid.ctlPOD item in CentralPanelContainer.Items)
                {
                    item.RemoveBuddy(strBuddyName);
                }
                foreach (VMuktiGrid.CustomGrid.ctlPOD item in RightPanelContainer.Items)
                {
                    item.RemoveBuddy(strBuddyName);
                }
                foreach (VMuktiGrid.CustomGrid.ctlPOD item in TopPanelContainer.Items)
                {
                    item.RemoveBuddy(strBuddyName);
                }
                foreach (VMuktiGrid.CustomGrid.ctlPOD item in BottomPanelContainer.Items)
                {
                    item.RemoveBuddy(strBuddyName);
                }
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "Addbuddy()", "Controls\\VmuktiGrid\\Grid\\CtlGrid.xaml.cs");
            }
        }



        #region IDisposable Members

        public void Dispose()
        {
            try
            {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "Dispose()", "Controls\\VmuktiGrid\\Grid\\CtlGrid.xaml.cs");
            }
        }
        public void Dispose(bool disposing)
        {
            if (!this.disposed)
			{
				if (disposing)
				{
					try
					{
                        lstDeletePods.Clear();
                        LeftPanelContainer.Items.Clear();
                        RightPanelContainer.Items.Clear();
                        CentralPanelContainer.Items.Clear();
                        TopPanelContainer.Items.Clear();
                        BottomPanelContainer.Items.Clear();
					}
					catch (Exception ex)
					{
                        VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "Dispose(bool disposing)", "Controls\\VmuktiGrid\\Grid\\CtlGrid.xaml.cs");
					}
				}
				disposed = true;
			}
			GC.SuppressFinalize(this);
        }

        ~ctlGrid()
        {
            Dispose(false);
        }

        #endregion

        //if arrangement of panel has been changed then page save indecator should be highlighted...
        private void TopPanelContainer_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            try
            {
            ((VMuktiGrid.ctlTab.TabItem)this.Parent).IsSaved = false;
        }
            catch (Exception ex)
            {
                //ex.Data.Add("My Key", "TopPanelContainer_SizeChanged--:--ctlGrid.xmal.cs--:--" + ex.Message + " :--:--");
                
                
                //System.Text.StringBuilder sb = new StringBuilder();
                //sb.AppendLine(ex.Message);
                //sb.AppendLine();
                //sb.AppendLine("StackTrace : " + ex.StackTrace);
                //sb.AppendLine();
                //sb.AppendLine("Location : " + ex.Data["My Key"].ToString());
                //sb.AppendLine();
                
                //sb.Append(sb1.ToString());
                //VMuktiAPI.ClsLogging.WriteToTresslog(sb);

                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "TopPanelContainer_SizeChanged()", "Controls\\VmuktiGrid\\Grid\\CtlGrid.xaml.cs");

            }
        }

        //its for setting user id / host id of newly created page when person is hosting a meeting and can be depricated after safe testing.
        public void SetUserID()
        {
            try
            {
            
        }
            catch (Exception ex)
            {
                //ex.Data.Add("My Key", "SetUserID--:--ctlGrid.xmal.cs--:--" + ex.Message + " :--:--");
                
                
                //System.Text.StringBuilder sb = new StringBuilder();
                //sb.AppendLine(ex.Message);
                //sb.AppendLine();
                //sb.AppendLine("StackTrace : " + ex.StackTrace);
                //sb.AppendLine();
                //sb.AppendLine("Location : " + ex.Data["My Key"].ToString());
                //sb.AppendLine();
                //sb.Append(sb1.ToString());
                //VMuktiAPI.ClsLogging.WriteToTresslog(sb);

                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "SetUserID()", "Controls\\VmuktiGrid\\Grid\\CtlGrid.xaml.cs");

            }
        }
        
        //its for unsetting user id / host id of newly created page when person is closing a meeting and can be deptricated after safe testing.
        public void UnSetUserID()
        {
            try
            {
            
        }
            catch (Exception ex)
            {
                //ex.Data.Add("My Key", "UnSetUserID--:--ctlGrid.xmal.cs--:--" + ex.Message + " :--:--");
                
                
                //System.Text.StringBuilder sb = new StringBuilder();
                //sb.AppendLine(ex.Message);
                //sb.AppendLine();
                //sb.AppendLine("StackTrace : " + ex.StackTrace);
                //sb.AppendLine();
                //sb.AppendLine("Location : " + ex.Data["My Key"].ToString());
                //sb.AppendLine();
                //sb.Append(sb1.ToString());
                //VMuktiAPI.ClsLogging.WriteToTresslog(sb);

                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "UnSetUserID()", "Controls\\VmuktiGrid\\Grid\\CtlGrid.xaml.cs");

            }
        }
        #region Multiple Buddy Selection

        public void LoadMultipleBuddyGrid(Dictionary<CtlExpanderItem, string> buddiesname, int modid)
        {
            try
            {
                DocumentRoot.ColumnDefinitions[0].Width = new GridLength(250, GridUnitType.Pixel);
                DocumentRoot.ColumnDefinitions[1].Width = new GridLength(1, GridUnitType.Star);
                DocumentRoot.ColumnDefinitions[2].Width = new GridLength(250, GridUnitType.Pixel);

                VMukti.Business.VMuktiGrid.ClsModule objModule = VMukti.Business.VMuktiGrid.ClsModule.GetPodModule(modid);

                ClsPermissionCollection objCPC = ClsPermissionCollection.Get_PermissionRefModule(modid, VMuktiAPI.VMuktiInfo.CurrentPeer.RoleID);
                int[] arrPermissionValue = new int[objCPC.Count];
                for (int percount = 0; percount < objCPC.Count; percount++)
                {
                    arrPermissionValue[percount] = objCPC[percount].PermissionValue;
                }
                //CentralPanelContainer.Items.Add(new ctlPOD(modid, objModule.ModuleTitle, objModule.IsCollaborative, null, arrPermissionValue, false, "fromDatabase", null, buddiesname));

                if (LeftPanelContainer.Items.Count == 0)
                {
                    LeftPanelContainer.Items.Insert(0, new ctlPOD(modid, objModule.ModuleTitle, objModule.IsCollaborative, null, arrPermissionValue, false, "fromDatabase", null, buddiesname));
                }
                else if (CentralPanelContainer.Items.Count == 0)
                {
                    CentralPanelContainer.Items.Insert(0, new ctlPOD(modid, objModule.ModuleTitle, objModule.IsCollaborative, null, arrPermissionValue, false, "fromDatabase", null, buddiesname));
                }
                else if (RightPanelContainer.Items.Count == 0)
                {
                    RightPanelContainer.Items.Insert(0, new ctlPOD(modid, objModule.ModuleTitle, objModule.IsCollaborative, null, arrPermissionValue, false, "fromDatabase", null, buddiesname));
                }
                else
                {
                    CentralPanelContainer.Items.Insert(0, new ctlPOD(modid, objModule.ModuleTitle, objModule.IsCollaborative, null, arrPermissionValue, false, "fromDatabase", null, buddiesname));
                }

            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "LoadMultipleBuddyGrid()", "Controls\\VmuktiGrid\\Grid\\CtlGrid.xaml.cs");
            }
        }

        public void LoadMultipleBuddyGrid(clsModuleInfo objModInfo)
        {
            try
            {
                DocumentRoot.ColumnDefinitions[0].Width = new GridLength(250, GridUnitType.Pixel);
                DocumentRoot.ColumnDefinitions[1].Width = new GridLength(1, GridUnitType.Star);
                DocumentRoot.ColumnDefinitions[2].Width = new GridLength(250, GridUnitType.Pixel);

                VMukti.Business.VMuktiGrid.ClsModule objModule = VMukti.Business.VMuktiGrid.ClsModule.GetPodModule(objModInfo.intModuleId);

                ClsPermissionCollection objCPC = ClsPermissionCollection.Get_PermissionRefModule(objModInfo.intModuleId, VMuktiAPI.VMuktiInfo.CurrentPeer.RoleID);
                int[] arrPermissionValue = new int[objCPC.Count];
                for (int percount = 0; percount < objCPC.Count; percount++)
                {
                    arrPermissionValue[percount] = objCPC[percount].PermissionValue;
                }
                string uri = "";
                if (VMuktiAPI.VMuktiInfo.CurrentPeer.CurrPeerType == PeerType.BootStrap || VMuktiAPI.VMuktiInfo.CurrentPeer.CurrPeerType == PeerType.SuperNode || VMuktiAPI.VMuktiInfo.CurrentPeer.CurrPeerType == PeerType.NodeWithNetP2P)
                {
                    uri = objModInfo.strUri[0];
                }
                else
                {
                    uri = objModInfo.strUri[1];
                }
                //CentralPanelContainer.Items.Add(new ctlPOD(objModInfo.intModuleId, objModule.ModuleTitle, objModule.IsCollaborative, uri, arrPermissionValue, false, "fromPeer", null, objModInfo.lstUsersDropped));
                if (LeftPanelContainer.Items.Count == 0)
                {
                    LeftPanelContainer.Items.Insert(0, new ctlPOD(objModInfo.intModuleId, objModule.ModuleTitle, objModule.IsCollaborative, uri, arrPermissionValue, false, "fromPeer", null, objModInfo.lstUsersDropped));
                }
                else if (CentralPanelContainer.Items.Count == 0)
                {
                    CentralPanelContainer.Items.Insert(0, new ctlPOD(objModInfo.intModuleId, objModule.ModuleTitle, objModule.IsCollaborative, uri, arrPermissionValue, false, "fromPeer", null, objModInfo.lstUsersDropped));
                }
                else if (RightPanelContainer.Items.Count == 0)
                {
                    RightPanelContainer.Items.Insert(0, new ctlPOD(objModInfo.intModuleId, objModule.ModuleTitle, objModule.IsCollaborative, uri, arrPermissionValue, false, "fromPeer", null, objModInfo.lstUsersDropped));
                }
                else
                {
                    CentralPanelContainer.Items.Insert(0, new ctlPOD(objModInfo.intModuleId, objModule.ModuleTitle, objModule.IsCollaborative, uri, arrPermissionValue, false, "fromPeer", null, objModInfo.lstUsersDropped));
                }
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "LoadMultipleBuddyGrid()", "Controls\\VmuktiGrid\\Grid\\CtlGrid.xaml.cs");
            }
        }

        #endregion

        //makeing all splitter visibliy collapsed or visible depends on blnVisibility, it is very much significant when we are dragging the module from left pane, coz animation (ie. image with mouse pointer) is being displayed at random space.
        public void SetGridSplliterVisiblity(bool blnVisibility)
        {
            try
            {
            if (blnVisibility && TopSplitter.Visibility != Visibility.Visible)
            {
                TopSplitter.Visibility = Visibility.Visible;
                TopSplitter_1.Visibility = Visibility.Visible;
                LeftSplitter.Visibility = Visibility.Visible;
                LeftSplitter_1.Visibility = Visibility.Visible;
                RightSplitter.Visibility = Visibility.Visible;
                RightSplitter_1.Visibility = Visibility.Visible;
                MiddleSplitter.Visibility = Visibility.Visible;
                MiddleSplitter_1.Visibility = Visibility.Visible;
                BottomSplitter.Visibility = Visibility.Visible;
                BottomSplitter_1.Visibility = Visibility.Visible;
            }
            else if (!blnVisibility && TopSplitter.Visibility != Visibility.Collapsed)
            {
                TopSplitter.Visibility = Visibility.Collapsed;
                TopSplitter_1.Visibility = Visibility.Collapsed;
                LeftSplitter.Visibility = Visibility.Collapsed;
                LeftSplitter_1.Visibility = Visibility.Collapsed;
                RightSplitter.Visibility = Visibility.Collapsed;
                RightSplitter_1.Visibility = Visibility.Collapsed;
                MiddleSplitter.Visibility = Visibility.Collapsed;
                MiddleSplitter_1.Visibility = Visibility.Collapsed;
                BottomSplitter.Visibility = Visibility.Collapsed;
                BottomSplitter_1.Visibility = Visibility.Collapsed;
            }
        }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "SetDridSplliterVisibility()", "Controls\\VmuktiGrid\\Grid\\CtlGrid.xaml.cs");
            }
        }


        private void OnModuleDrop(object o, RoutedEventArgs args)
        {

            try
            {
                string parameter = args.OriginalSource.ToString();
                string[] strTag = parameter.ToString().Split(',');

                if (strTag.Length == 4)
                {
                    ClsPermissionCollection objCPC = ClsPermissionCollection.Get_PermissionRefModule(int.Parse(strTag[0]), VMuktiAPI.VMuktiInfo.CurrentPeer.RoleID);
                    int[] arrPermissionValue = new int[objCPC.Count];
                    for (int percount = 0; percount < objCPC.Count; percount++)
                    {
                        arrPermissionValue[percount] = objCPC[percount].PermissionValue;
                    }
                    ctlPOD objTempPOD = new ctlPOD(int.Parse(strTag[0]), strTag[3], strTag[2], null, arrPermissionValue, false, "fromLeftPane", CentralPanelContainer, true, null);
                    objTempPOD.OwnerPodIndex = VMukti.App.podCounter++;

                }
            }
            catch (Exception ex)
            {

            }
        }



        public void FncDrop(string caption, string tag, MouseButtonEventArgs e)
        {
            try
            {
                targetItemsControl = null;
                VisualTreeHelper.HitTest(this.DocumentRoot, new HitTestFilterCallback(this.FilterHitTestResultsCallback), new HitTestResultCallback(this.HitTestResultCallback), new PointHitTestParameters(Mouse.PrimaryDevice.GetPosition(this.DocumentRoot)));
                  
                string[] strTag = tag.Split(',');
                if (targetItemsControl != null)
                {
                    ClsPermissionCollection objCPC = ClsPermissionCollection.Get_PermissionRefModule(int.Parse(strTag[0]), VMuktiAPI.VMuktiInfo.CurrentPeer.RoleID);
                    int[] arrPermissionValue = new int[objCPC.Count];
                    for (int percount = 0; percount < objCPC.Count; percount++)
                    {
                        arrPermissionValue[percount] = objCPC[percount].PermissionValue;
                    }
                    
                    ctlPOD objTempPOD = new ctlPOD(int.Parse(strTag[0]), caption, strTag[2], null, arrPermissionValue, false, "fromLeftPane", targetItemsControl, true, null,0);
                    objTempPOD.OwnerPodIndex = VMukti.App.podCounter++;
                }
            }
            catch (Exception ex)
            {

            }
        }
    }
}