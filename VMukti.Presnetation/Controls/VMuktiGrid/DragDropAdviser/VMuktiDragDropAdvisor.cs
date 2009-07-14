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
using DnD;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;
using VMukti.Business;
using VMuktiGrid.CustomGrid;
using System.Windows.Markup;
using System.Xml;
using System.IO;

namespace VMukti.Presentation.Controls.VMuktiGrid.DragDropAdviser
{
    public class VMuktiDragSourceForBuddy : DragSourceBase
    {
        public VMuktiDragSourceForBuddy()
        {
            SupportedFormat = "VMuktiDragAndDropModule";
        }

        public override void FinishDrag(UIElement draggedElt, DragDropEffects finalEffects)
        {
           
        }

        public override bool IsDraggable(UIElement dragElt)
        {
            return (dragElt is CtlExpanderItem);
        }



    }

    public class VMuktiDragSourceForModule : DragSourceBase
    {
        public VMuktiDragSourceForModule()
        {
            SupportedFormat = "VMuktiDragAndDropModule";
        }

        public override void FinishDrag(UIElement draggedElt, DragDropEffects finalEffects)
        {

        }

        public override bool IsDraggable(UIElement dragElt)
        {
            return (dragElt is CtlMExpanderItem);
        }



    }

    public class VMuktiDropTargetForModule : DropTargetBase
    {

        public VMuktiDropTargetForModule()
        {
            SupportedFormat = "VMuktiDragAndDropModule";
        }

        public override void OnDropCompleted(IDataObject obj, Point dropPoint)
        {




        }

        public override UIElement GetVisualFeedback(IDataObject obj)
        {

            CtlMExpanderItem elt = ExtractElement(obj) as CtlMExpanderItem;
            elt.Width = 100;
            elt.Height = 130;
            elt.Opacity = 0.5;
            elt.IsHitTestVisible = false;

            DoubleAnimation anim = new DoubleAnimation(0.75, new Duration(TimeSpan.FromMilliseconds(500)));
            anim.From = 0.25;
            anim.AutoReverse = true;
            anim.RepeatBehavior = RepeatBehavior.Forever;
            elt.BeginAnimation(UIElement.OpacityProperty, anim);
            return elt;

        }


    }

    public class VMuktiDropTargetForBuddy : DropTargetBase
    {

        public VMuktiDropTargetForBuddy()
        {
            SupportedFormat = "VMuktiDragAndDropModule";
        }

        public override void OnDropCompleted(IDataObject obj, Point dropPoint)
        {




        }

        public override UIElement GetVisualFeedback(IDataObject obj)
        {

            CtlExpanderItem elt = ExtractElement(obj) as CtlExpanderItem;
            elt.Width = 100;
            elt.Height = 27;
            elt.Opacity = 0.5;
            elt.IsHitTestVisible = false;

            DoubleAnimation anim = new DoubleAnimation(0.75, new Duration(TimeSpan.FromMilliseconds(500)));
            anim.From = 0.25;
            anim.AutoReverse = true;
            anim.RepeatBehavior = RepeatBehavior.Forever;
            elt.BeginAnimation(UIElement.OpacityProperty, anim);
            return elt;

        }


    }

    public class VMuktiDropTopTarget : DropTargetBase
    {

        public VMuktiDropTopTarget()
        {
            SupportedFormat = "VMuktiDragAndDropModule";
        }

        public override void OnDropCompleted(IDataObject obj, Point dropPoint)
        {

            string xamlString = obj.GetData("VMuktiDragAndDropModule") as string;
            XmlReader reader = XmlReader.Create(new StringReader(xamlString));
            CtlMExpanderItem elt = XamlReader.Load(reader) as CtlMExpanderItem;
            string[] strTag = elt.Tag.ToString().Split(',');

            if (strTag.Length == 3)
            {
                ClsPermissionCollection objCPC = ClsPermissionCollection.Get_PermissionRefModule(int.Parse(strTag[0]), VMuktiAPI.VMuktiInfo.CurrentPeer.RoleID);
                int[] arrPermissionValue = new int[objCPC.Count];
                for (int percount = 0; percount < objCPC.Count; percount++)
                {
                    arrPermissionValue[percount] = objCPC[percount].PermissionValue;
                }
                (TargetUI as ItemsControl).Items.RemoveAt((TargetUI as ItemsControl).Items.Count - 1);
                ctlPOD objTempPOD = new ctlPOD(int.Parse(strTag[0]), elt.Caption, strTag[2], null, arrPermissionValue, false, "fromLeftPane", (TargetUI as ItemsControl), true, null);
                objTempPOD.OwnerPodIndex = VMukti.App.podCounter++;

                (((TargetUI as ItemsControl).Parent as Grid).Parent as ctlGrid).SetGridSplliterVisiblity(true);

            }

        }

        //public override UIElement GetVisualFeedback(IDataObject obj)
        //{

        //    CtlExpanderItem elt = ExtractElement(obj) as CtlExpanderItem;
        //    elt.Width = 100;
        //    elt.Height = 25;
        //    elt.Opacity = 0.5;
        //    elt.IsHitTestVisible = false;

        //    DoubleAnimation anim = new DoubleAnimation(0.75, new Duration(TimeSpan.FromMilliseconds(500)));
        //    anim.From = 0.25;
        //    anim.AutoReverse = true;
        //    anim.RepeatBehavior = RepeatBehavior.Forever;
        //    elt.BeginAnimation(UIElement.OpacityProperty, anim);

        //    return elt;

        //}

        public override UIElement GetVisualFeedback(IDataObject obj)
        {
            try
            {
                CtlMExpanderItem elt = ExtractElement(obj) as CtlMExpanderItem;
                elt.Width = 100;
                elt.Height = 130;
                elt.Opacity = 0.5;
                elt.IsHitTestVisible = false;
                DoubleAnimation anim = new DoubleAnimation(0.75, new Duration(TimeSpan.FromMilliseconds(500)));
                anim.From = 0.25;
                anim.AutoReverse = true;
                anim.RepeatBehavior = RepeatBehavior.Forever;
                elt.BeginAnimation(UIElement.OpacityProperty, anim);
                return elt;

            }
            catch
            {
                CtlExpanderItem elt = ExtractElement(obj) as CtlExpanderItem;
                elt.Width = 100;
                elt.Height = 27;
                elt.Opacity = 0.5;
                elt.IsHitTestVisible = false;
                DoubleAnimation anim = new DoubleAnimation(0.75, new Duration(TimeSpan.FromMilliseconds(500)));
                anim.From = 0.25;
                anim.AutoReverse = true;
                anim.RepeatBehavior = RepeatBehavior.Forever;
                elt.BeginAnimation(UIElement.OpacityProperty, anim);
                return elt;

            }
            

        }


    }

    public class VMuktiDropLeftTarget : DropTargetBase
    {
        public VMuktiDropLeftTarget()
        {
            SupportedFormat = "VMuktiDragAndDropModule";
        }

        public override void OnDropCompleted(IDataObject obj, Point dropPoint)
        {
            string xamlString = obj.GetData("VMuktiDragAndDropModule") as string;
            XmlReader reader = XmlReader.Create(new StringReader(xamlString));
            CtlMExpanderItem elt = XamlReader.Load(reader) as CtlMExpanderItem;
            string[] strTag = elt.Tag.ToString().Split(',');

            if (strTag.Length == 3)
            {
                ClsPermissionCollection objCPC = ClsPermissionCollection.Get_PermissionRefModule(int.Parse(strTag[0]), VMuktiAPI.VMuktiInfo.CurrentPeer.RoleID);
                int[] arrPermissionValue = new int[objCPC.Count];
                for (int percount = 0; percount < objCPC.Count; percount++)
                {
                    arrPermissionValue[percount] = objCPC[percount].PermissionValue;
                }
                (TargetUI as ItemsControl).Items.RemoveAt((TargetUI as ItemsControl).Items.Count - 1);
                ctlPOD objTempPOD = new ctlPOD(int.Parse(strTag[0]), elt.Caption, strTag[2], null, arrPermissionValue, false, "fromLeftPane", (TargetUI as ItemsControl), true, null);
                objTempPOD.OwnerPodIndex = VMukti.App.podCounter++;

                (((TargetUI as ItemsControl).Parent as Grid).Parent as ctlGrid).SetGridSplliterVisiblity(true);

            }
        }
        //public override UIElement GetVisualFeedback(IDataObject obj)
        //{
        //    CtlMExpanderItem elt = ExtractElement(obj) as CtlMExpanderItem;
        //    elt.Width = 100;
        //    elt.Height = 25;
        //    elt.Opacity = 0.5;
        //    elt.IsHitTestVisible = false;

        //    DoubleAnimation anim = new DoubleAnimation(0.75, new Duration(TimeSpan.FromMilliseconds(500)));
        //    anim.From = 0.25;
        //    anim.AutoReverse = true;
        //    anim.RepeatBehavior = RepeatBehavior.Forever;
        //    elt.BeginAnimation(UIElement.OpacityProperty, anim);

        //    return elt;
        //}

        public override UIElement GetVisualFeedback(IDataObject obj)
        {
            try
            {
            CtlMExpanderItem elt = ExtractElement(obj) as CtlMExpanderItem;
            elt.Width = 100;
            elt.Height = 130;
            elt.Opacity = 0.5;
            elt.IsHitTestVisible = false;

            DoubleAnimation anim = new DoubleAnimation(0.75, new Duration(TimeSpan.FromMilliseconds(500)));
            anim.From = 0.25;
            anim.AutoReverse = true;
            anim.RepeatBehavior = RepeatBehavior.Forever;
            elt.BeginAnimation(UIElement.OpacityProperty, anim);

            return elt;
            }
            catch
            {
                CtlExpanderItem elt = ExtractElement(obj) as CtlExpanderItem;
                elt.Width = 100;
                elt.Height = 27;
                elt.Opacity = 0.5;
                elt.IsHitTestVisible = false;
                DoubleAnimation anim = new DoubleAnimation(0.75, new Duration(TimeSpan.FromMilliseconds(500)));
                anim.From = 0.25;
                anim.AutoReverse = true;
                anim.RepeatBehavior = RepeatBehavior.Forever;
                elt.BeginAnimation(UIElement.OpacityProperty, anim);

                return elt;
            }
           
        }
    }
    
    public class VMuktiDropCenterTarget : DropTargetBase
    {
        public VMuktiDropCenterTarget()
        {
            SupportedFormat = "VMuktiDragAndDropModule";
        }

        public override void OnDropCompleted(IDataObject obj, Point dropPoint)
        {
            string xamlString = obj.GetData("VMuktiDragAndDropModule") as string;
            XmlReader reader = XmlReader.Create(new StringReader(xamlString));
            CtlMExpanderItem elt = XamlReader.Load(reader) as CtlMExpanderItem;
            string[] strTag = elt.Tag.ToString().Split(',');

            if (strTag.Length == 3)
            {
                ClsPermissionCollection objCPC = ClsPermissionCollection.Get_PermissionRefModule(int.Parse(strTag[0]), VMuktiAPI.VMuktiInfo.CurrentPeer.RoleID);
                int[] arrPermissionValue = new int[objCPC.Count];
                for (int percount = 0; percount < objCPC.Count; percount++)
                {
                    arrPermissionValue[percount] = objCPC[percount].PermissionValue;
                }
                (TargetUI as ItemsControl).Items.RemoveAt((TargetUI as ItemsControl).Items.Count - 1);
                ctlPOD objTempPOD = new ctlPOD(int.Parse(strTag[0]), elt.Caption, strTag[2], null, arrPermissionValue, false, "fromLeftPane", (TargetUI as ItemsControl), true, null);
                objTempPOD.OwnerPodIndex = VMukti.App.podCounter++;

                (((TargetUI as ItemsControl).Parent as Grid).Parent as ctlGrid).SetGridSplliterVisiblity(true);

            }
        }

        public override UIElement GetVisualFeedback(IDataObject obj)
        {
            try
            {
            CtlMExpanderItem elt = ExtractElement(obj) as CtlMExpanderItem;
            elt.Width = 100;
            elt.Height = 130;
            elt.Opacity = 0.5;
            elt.IsHitTestVisible = false;
            DoubleAnimation anim = new DoubleAnimation(0.75, new Duration(TimeSpan.FromMilliseconds(500)));
            anim.From = 0.25;
            anim.AutoReverse = true;
            anim.RepeatBehavior = RepeatBehavior.Forever;
            elt.BeginAnimation(UIElement.OpacityProperty, anim);
            return elt;
             }
            catch
            {
                CtlExpanderItem elt = ExtractElement(obj) as CtlExpanderItem;
                elt.Width = 100;
                elt.Height = 27;
                elt.Opacity = 0.5;
                elt.IsHitTestVisible = false;
                DoubleAnimation anim = new DoubleAnimation(0.75, new Duration(TimeSpan.FromMilliseconds(500)));
                anim.From = 0.25;
                anim.AutoReverse = true;
                anim.RepeatBehavior = RepeatBehavior.Forever;
                elt.BeginAnimation(UIElement.OpacityProperty, anim);
                return elt;
            }

        }
    }

    public class VMuktiDropRightTarget : DropTargetBase
    {
        public VMuktiDropRightTarget()
        {
            SupportedFormat = "VMuktiDragAndDropModule";
        }

        public override void OnDropCompleted(IDataObject obj, Point dropPoint)
        {
            string xamlString = obj.GetData("VMuktiDragAndDropModule") as string;
            XmlReader reader = XmlReader.Create(new StringReader(xamlString));
            CtlMExpanderItem elt = XamlReader.Load(reader) as CtlMExpanderItem;
            string[] strTag = elt.Tag.ToString().Split(',');

            if (strTag.Length == 3)
            {
                ClsPermissionCollection objCPC = ClsPermissionCollection.Get_PermissionRefModule(int.Parse(strTag[0]), VMuktiAPI.VMuktiInfo.CurrentPeer.RoleID);
                int[] arrPermissionValue = new int[objCPC.Count];
                for (int percount = 0; percount < objCPC.Count; percount++)
                {
                    arrPermissionValue[percount] = objCPC[percount].PermissionValue;
                }
                (TargetUI as ItemsControl).Items.RemoveAt((TargetUI as ItemsControl).Items.Count - 1);
                ctlPOD objTempPOD = new ctlPOD(int.Parse(strTag[0]), elt.Caption, strTag[2], null, arrPermissionValue, false, "fromLeftPane", (TargetUI as ItemsControl), true, null);
                objTempPOD.OwnerPodIndex = VMukti.App.podCounter++;

                (((TargetUI as ItemsControl).Parent as Grid).Parent as ctlGrid).SetGridSplliterVisiblity(true);
            }
        }

        public override UIElement GetVisualFeedback(IDataObject obj)
        {
            try
            {
            CtlMExpanderItem elt = ExtractElement(obj) as CtlMExpanderItem;
            elt.Width = 100;
            elt.Height = 130;
            elt.Opacity = 0.5;
            elt.IsHitTestVisible = false;
            DoubleAnimation anim = new DoubleAnimation(0.75, new Duration(TimeSpan.FromMilliseconds(500)));
            anim.From = 0.25;
            anim.AutoReverse = true;
            anim.RepeatBehavior = RepeatBehavior.Forever;
            elt.BeginAnimation(UIElement.OpacityProperty, anim);
            return elt;
             }
            catch
            {
                CtlExpanderItem elt = ExtractElement(obj) as CtlExpanderItem;
                elt.Width = 100;
                elt.Height = 27;
                elt.Opacity = 0.5;
                elt.IsHitTestVisible = false;
                DoubleAnimation anim = new DoubleAnimation(0.75, new Duration(TimeSpan.FromMilliseconds(500)));
                anim.From = 0.25;
                anim.AutoReverse = true;
                anim.RepeatBehavior = RepeatBehavior.Forever;
                elt.BeginAnimation(UIElement.OpacityProperty, anim);
                return elt;
            }

        }
    }
    
    public class VMuktiDropBottomTarget : DropTargetBase
    {
        public VMuktiDropBottomTarget()
        {
            SupportedFormat = "VMuktiDragAndDropModule";
        }

        public override void OnDropCompleted(IDataObject obj, Point dropPoint)
        {
            string xamlString = obj.GetData("VMuktiDragAndDropModule") as string;
            XmlReader reader = XmlReader.Create(new StringReader(xamlString));
            CtlMExpanderItem elt = XamlReader.Load(reader) as CtlMExpanderItem;
            string[] strTag = elt.Tag.ToString().Split(',');

            if (strTag.Length == 3)
            {
                ClsPermissionCollection objCPC = ClsPermissionCollection.Get_PermissionRefModule(int.Parse(strTag[0]), VMuktiAPI.VMuktiInfo.CurrentPeer.RoleID);
                int[] arrPermissionValue = new int[objCPC.Count];
                for (int percount = 0; percount < objCPC.Count; percount++)
                {
                    arrPermissionValue[percount] = objCPC[percount].PermissionValue;
                }
                (TargetUI as ItemsControl).Items.RemoveAt((TargetUI as ItemsControl).Items.Count - 1);
                ctlPOD objTempPOD = new ctlPOD(int.Parse(strTag[0]), elt.Caption, strTag[2], null, arrPermissionValue, false, "fromLeftPane", (TargetUI as ItemsControl), true, null);
                objTempPOD.OwnerPodIndex = VMukti.App.podCounter++;

                (((TargetUI as ItemsControl).Parent as Grid).Parent as ctlGrid).SetGridSplliterVisiblity(true);
            }
        }

        public override UIElement GetVisualFeedback(IDataObject obj)
        {
            try
            {
            CtlMExpanderItem elt = ExtractElement(obj) as CtlMExpanderItem;
            elt.Width = 100;
            elt.Height = 130;
            elt.Opacity = 0.5;
            elt.IsHitTestVisible = false;
            DoubleAnimation anim = new DoubleAnimation(0.75, new Duration(TimeSpan.FromMilliseconds(500)));
            anim.From = 0.25;
            anim.AutoReverse = true;
            anim.RepeatBehavior = RepeatBehavior.Forever;
            elt.BeginAnimation(UIElement.OpacityProperty, anim);
            return elt;
             }
            catch
            {
                CtlExpanderItem elt = ExtractElement(obj) as CtlExpanderItem;
            elt.Width = 100;
                elt.Height = 27;
            elt.Opacity = 0.5;
            elt.IsHitTestVisible = false;

            DoubleAnimation anim = new DoubleAnimation(0.75, new Duration(TimeSpan.FromMilliseconds(500)));
            anim.From = 0.25;
            anim.AutoReverse = true;
            anim.RepeatBehavior = RepeatBehavior.Forever;
            elt.BeginAnimation(UIElement.OpacityProperty, anim);
            return elt;
            }
           

        }
    }

   
}
    