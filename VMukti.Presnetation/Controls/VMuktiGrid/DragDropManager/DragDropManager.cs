
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
using System.Windows;
using System.Windows.Documents;
using System.Windows.Input;
using System.Runtime.InteropServices;
using System.Windows.Media;
using System.Text;
using VMuktiAPI;

namespace DnD
{
	public static class DragDropManager
	{
        public static StringBuilder sb1 = new StringBuilder();
		private static UIElement _draggedElt;
		private static bool _isMouseDown = false;
		private static Point _dragStartPoint;
        private static Point _offsetPoint;
		private static DropPreviewAdorner _overlayElt;

		#region Dependency Properties

        public static readonly DependencyProperty DragSourceProperty =
                DependencyProperty.RegisterAttached("DragSource", typeof(DragSourceBase), typeof(DragDropManager),
                new FrameworkPropertyMetadata(new PropertyChangedCallback(OnDragSourceChanged)));

        public static DragSourceBase GetDragSource(DependencyObject depObj)
        {
            try
            {
            return depObj.GetValue(DragSourceProperty) as DragSourceBase;
        }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "GetDragsource()", "Controls\\VMuktiGrid\\DragDropManager\\DragDropManager.cs");
                return null;
            }
        }

        public static void SetDragSource(DependencyObject depObj, bool isSet)
        {
            try
            {
            depObj.SetValue(DragSourceProperty, isSet);
        }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "SetDragSource()", "Controls\\VMuktiGrid\\DragDropManager\\DragDropManager.cs");
            }
        }
		public static readonly DependencyProperty DropTargetProperty =
			DependencyProperty.RegisterAttached("DropTarget", typeof(DropTargetBase), typeof(DragDropManager),
			new FrameworkPropertyMetadata(new PropertyChangedCallback(OnDropTargetChanged)));

		public static void SetDropTarget(DependencyObject depObj, bool isSet)
		{
            try
            {
			depObj.SetValue(DropTargetProperty, isSet);
		}
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "SetDropTrarget()", "Controls\\VMuktiGrid\\DragDropManager\\DragDropManager.cs");
            }
        }
		public static DropTargetBase GetDropTarget(DependencyObject depObj)
		{
            try
            {
			return depObj.GetValue(DropTargetProperty) as DropTargetBase;
		}
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "GetDropTraget()", "Controls\\VMuktiGrid\\DragDropManager\\DragDropManager.cs");
                return null;
            }
        }
		#endregion

		#region Property Change handlers
      
        private static void OnDragSourceChanged(DependencyObject depObj, DependencyPropertyChangedEventArgs args)
        {
            try
            {
            UIElement sourceElt = depObj as UIElement;
            if (args.NewValue != null && args.OldValue == null)
            {
                sourceElt.PreviewMouseLeftButtonDown += new MouseButtonEventHandler(DragSource_PreviewMouseLeftButtonDown);
                sourceElt.PreviewMouseMove += new MouseEventHandler(DragSource_PreviewMouseMove);
                sourceElt.PreviewMouseUp += new MouseButtonEventHandler(DragSource_PreviewMouseUp);

                // Set the Drag source UI
                DragSourceBase advisor = args.NewValue as DragSourceBase;
                advisor.SourceUI = sourceElt;
            }
            else if (args.NewValue == null && args.OldValue != null)
            {
                sourceElt.PreviewMouseLeftButtonDown -= DragSource_PreviewMouseLeftButtonDown;
                sourceElt.PreviewMouseMove -= DragSource_PreviewMouseMove;
                sourceElt.PreviewMouseUp -= DragSource_PreviewMouseUp;
            }
        }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "OnDragSourceChanged()", "Controls\\VMuktiGrid\\DragDropManager\\DragDropManager.cs");
            }
        }
		static void DragSource_PreviewMouseUp(object sender, MouseButtonEventArgs e)
		{
            try
            {
			_isMouseDown = false;
			Mouse.Capture(null);
		}
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "DragSource_PreviewMouseUP()", "Controls\\VMuktiGrid\\DragDropManager\\DragDropManager.cs");
            }
        }
		private static void OnDropTargetChanged(DependencyObject depObj, DependencyPropertyChangedEventArgs args)
		{
            try
            {
			UIElement targetElt = depObj as UIElement;
			if (args.NewValue != null && args.OldValue == null)
			{
				targetElt.PreviewDragEnter += new DragEventHandler(DropTarget_PreviewDragEnter);
				targetElt.PreviewDragOver += new DragEventHandler(DropTarget_PreviewDragOver);
				targetElt.PreviewDragLeave += new DragEventHandler(DropTarget_PreviewDragLeave);
				targetElt.PreviewDrop += new DragEventHandler(DropTarget_PreviewDrop);

				targetElt.AllowDrop = true;

				// Set the Drag source UI
				DropTargetBase advisor = args.NewValue as DropTargetBase;
				advisor.TargetUI = targetElt;
			}
			else if (args.NewValue == null && args.OldValue != null)
			{
				targetElt.PreviewDragEnter -= DropTarget_PreviewDragEnter;
				targetElt.PreviewDragOver -= DropTarget_PreviewDragOver;
				targetElt.PreviewDragLeave -= DropTarget_PreviewDragLeave;
				targetElt.PreviewDrop -= DropTarget_PreviewDrop;


				targetElt.AllowDrop = false;
			}
		}

            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "OnDropTargetChanged()", "Controls\\VMuktiGrid\\DragDropManager\\DragDropManager.cs");
            }
        }
		#endregion

		/* ____________________________________________________________________
		 *		Drop Target events 
		 * ____________________________________________________________________
		 */
		static void DropTarget_PreviewDrop(object sender, DragEventArgs e)
		{
            try
            {
			if (UpdateEffects(sender, e) == false) return;

			DropTargetBase advisor = GetDropTarget(sender as DependencyObject);
			Point dropPoint = e.GetPosition(sender as UIElement);

			// Calculate displacement for (Left, Top)
			Point offset = e.GetPosition(_overlayElt);
			dropPoint.X = dropPoint.X - offset.X;
			dropPoint.Y = dropPoint.Y - offset.Y;

			advisor.OnDropCompleted(e.Data, dropPoint);
			RemovePreviewAdorner();
            _offsetPoint = new Point(0, 0);

		}
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "DropTarget_PreviewDrop()", "Controls\\VMuktiGrid\\DragDropManager\\DragDropManager.cs");
            }
        }
		static void DropTarget_PreviewDragLeave(object sender, DragEventArgs e)
		{
            try
            {
			if (UpdateEffects(sender, e) == false) return;

            DropTargetBase advisor = GetDropTarget(sender as DependencyObject);
            Point mousePoint = MouseUtilities.GetMousePosition(advisor.TargetUI);
            
            //Console.WriteLine("Inside DropTarget_PreviewDragLeave1" + mousePoint.X.ToString() + "|" + mousePoint.Y.ToString());
            //giving a tolerance of 2 so that the adorner is removed when the mouse is moved fast.
            //this might still be small...in that case increase the tolerance
            if ((mousePoint.X < 2) || (mousePoint.Y < 2)||
                (mousePoint.X > ((FrameworkElement)(advisor.TargetUI)).ActualWidth - 2) ||
                (mousePoint.Y > ((FrameworkElement)(advisor.TargetUI)).ActualHeight - 2))
            {
                RemovePreviewAdorner();
            }
			e.Handled = true;
		}
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "DropTarget_PreviewDragLeave()", "Controls\\VMuktiGrid\\DragDropManager\\DragDropManager.cs");
            }
        }
		static void DropTarget_PreviewDragOver(object sender, DragEventArgs e)
		{
            try
            {
			if (UpdateEffects(sender, e) == false) return;
			// Update position of the preview Adorner
			Point position = e.GetPosition(sender as UIElement);

			_overlayElt.Left = position.X - _offsetPoint.X;
			_overlayElt.Top = position.Y - _offsetPoint.Y;
			
			e.Handled = true;
		}
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "DropTarget_PreviewDragOver()", "Controls\\VMuktiGrid\\DragDropManager\\DragDropManager.cs");
            }
        }
		static void DropTarget_PreviewDragEnter(object sender, DragEventArgs e)
		{
            try
            {
			if (UpdateEffects(sender, e) == false) return;

			// Setup the preview Adorner
			UIElement feedbackUI = GetDropTarget(sender as DependencyObject).GetVisualFeedback(e.Data);
            _offsetPoint = GetOffsetPoint(e.Data);

            DropTargetBase advisor = GetDropTarget(sender as DependencyObject);

            Point mousePoint = MouseUtilities.GetMousePosition(advisor.TargetUI);

           // Console.WriteLine("Inside DropTarget_PreviewDragEnter" + mousePoint.X.ToString() + "|" + mousePoint.Y.ToString());

            //giving a tolerance of 2 so that the adorner is created when the mouse is moved fast.
            //this might still be small...in that case increase the tolerance
            if ((mousePoint.X < 2) || (mousePoint.Y < 2) ||
                (mousePoint.X > ((FrameworkElement)(advisor.TargetUI)).ActualWidth - 2) ||
                (mousePoint.Y > ((FrameworkElement)(advisor.TargetUI)).ActualHeight - 2) ||
                 (_overlayElt == null))
            {
                CreatePreviewAdorner(sender as UIElement, feedbackUI);
            }

			e.Handled = true;
		}
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "DropTarget_PreviewDragEnter()", "Controls\\VMuktiGrid\\DragDropManager\\DragDropManager.cs");
            }
        }
        static Point GetOffsetPoint(IDataObject obj)
        {
            Point p = (Point)obj.GetData("OffsetPoint");
            return p;
        }

		static bool UpdateEffects(object uiObject, DragEventArgs e)
		{
            try
            {
			DropTargetBase advisor = GetDropTarget(uiObject as DependencyObject);
			if (advisor.IsValidDataObject(e.Data) == false) return false;

			if ((e.AllowedEffects & DragDropEffects.Move) == 0 &&
				(e.AllowedEffects & DragDropEffects.Copy) == 0)
			{
				e.Effects = DragDropEffects.None;
				return true;
			}

			if ((e.AllowedEffects & DragDropEffects.Move) != 0 &&
				(e.AllowedEffects & DragDropEffects.Copy) != 0)
			{
                if ((e.KeyStates & DragDropKeyStates.ControlKey) != 0)
                {
                }
				e.Effects = ((e.KeyStates & DragDropKeyStates.ControlKey) != 0) ?
					DragDropEffects.Copy : DragDropEffects.Move;
			}

			return true;
		}
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "UpdateEffects()", "Controls\\VMuktiGrid\\DragDropManager\\DragDropManager.cs");
                return false;
            }
        }

		/* ____________________________________________________________________
		 *		Drag Source events 
		 * ____________________________________________________________________
		 */
		static void DragSource_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
		{
            try
            {
			// Make this the new drag source
			DragSourceBase advisor = GetDragSource(sender as DependencyObject);

			if (advisor.IsDraggable(e.Source as UIElement) == false) return;

			_draggedElt = e.Source as UIElement;
			_dragStartPoint = e.GetPosition(GetTopContainer());

            _offsetPoint = e.GetPosition(_draggedElt);
			_isMouseDown = true;

		}
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "DragSource_PreviewMouseLeftButtonDown()", "Controls\\VMuktiGrid\\DragDropManager\\DragDropManager.cs");
            }
        }
		static void DragSource_PreviewMouseMove(object sender, MouseEventArgs e)
		{
            try
            {
			if (_isMouseDown && IsDragGesture(e.GetPosition(GetTopContainer())))
			{
				DragStarted(sender as UIElement);
			}
		}
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "DragSource_PreviewMouseMove()", "Controls\\VMuktiGrid\\DragDropManager\\DragDropManager.cs");
            }
        }
		static void DragStarted(UIElement uiElt)
		{
            try
            {
			_isMouseDown = false;
			Mouse.Capture(uiElt);

			DragSourceBase advisor = GetDragSource(uiElt as DependencyObject);
			DataObject data = advisor.GetDataObject(_draggedElt);
            
            data.SetData("OffsetPoint", _offsetPoint);

			DragDropEffects supportedEffects = advisor.SupportedEffects;

			// Perform DragDrop

			DragDropEffects effects = System.Windows.DragDrop.DoDragDrop(_draggedElt, data, supportedEffects);
			advisor.FinishDrag(_draggedElt, effects);

			// Clean up
			RemovePreviewAdorner();
			Mouse.Capture(null);
			_draggedElt = null;
		}
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "DragStarted()", "Controls\\VMuktiGrid\\DragDropManager\\DragDropManager.cs");
            }
        }
		static bool IsDragGesture(Point point)
		{
            try
            {
			bool hGesture = Math.Abs(point.X - _dragStartPoint.X) > SystemParameters.MinimumHorizontalDragDistance;
			bool vGesture = Math.Abs(point.Y - _dragStartPoint.Y) > SystemParameters.MinimumVerticalDragDistance;

			return (hGesture | vGesture);
		}
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "IsDragGesture()", "Controls\\VMuktiGrid\\DragDropManager\\DragDropManager.cs");
                return false;
            }
        }

		/* ____________________________________________________________________
		 *		Utility functions
		 * ____________________________________________________________________
		 */
		static UIElement GetTopContainer()
		{
            try
            {
           // return  LogicalTreeHelper.FindLogicalNode(Application.Current.MainWindow, "canvas") as UIElement;

			return Application.Current.MainWindow.Content as UIElement;
		}
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "GetTopContainer()", "Controls\\VMuktiGrid\\DragDropManager\\DragDropManager.cs");
                return null;
            }
        }


		private static void CreatePreviewAdorner(UIElement adornedElt, UIElement feedbackUI)
		{
            try
            {
			// Clear if there is an existing preview adorner
			RemovePreviewAdorner();

			AdornerLayer layer = AdornerLayer.GetAdornerLayer(GetTopContainer());
			_overlayElt = new DropPreviewAdorner(feedbackUI, adornedElt);
			layer.Add(_overlayElt);
		}
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "CreatePreviewAdorner()", "Controls\\VMuktiGrid\\DragDropManager\\DragDropManager.cs");
            }
        }
		private static void RemovePreviewAdorner()
		{
            try
            {
			if (_overlayElt != null)
			{
				AdornerLayer.GetAdornerLayer(GetTopContainer()).Remove(_overlayElt);
				_overlayElt = null;
			}
		}

            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "RemovePreviewAdorner()", "Controls\\VMuktiGrid\\DragDropManager\\DragDropManager.cs");
            }
        }
	}

    public class MouseUtilities
    {
        [System.Runtime.InteropServices.StructLayout(LayoutKind.Sequential)]
        private struct Win32Point
        {
            public Int32 X;
            public Int32 Y;
        };

        [DllImport("user32.dll")]
        private static extern bool GetCursorPos(ref Win32Point pt);

        [DllImport("user32.dll")]
        private static extern bool ScreenToClient(IntPtr hwnd, ref Win32Point pt);

        public static Point GetMousePosition(Visual relativeTo)
        {
            Win32Point mouse = new Win32Point();
            GetCursorPos(ref mouse);

            System.Windows.Interop.HwndSource presentationSource =
                (System.Windows.Interop.HwndSource)PresentationSource.FromVisual(relativeTo);

            ScreenToClient(presentationSource.Handle, ref mouse);

            GeneralTransform transform = relativeTo.TransformToAncestor(presentationSource.RootVisual);

            Point offset = transform.Transform(new Point(0, 0));

            return new Point(mouse.X - offset.X, mouse.Y - offset.Y);
        }
        }
    };


