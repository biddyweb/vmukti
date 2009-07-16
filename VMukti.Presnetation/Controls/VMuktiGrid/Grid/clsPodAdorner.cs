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
using System.Windows.Media;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Input;

namespace VMukti.Presentation.Controls.VMuktiGrid
{
    class clsPodAdorner : System.Windows.Documents.Adorner
    {

        Thumb bottomRight;

        //<SnippetFEVisualOverridesPre>
        // To store and manage the adorner's visual children.
        VisualCollection visualChildren;


        public clsPodAdorner(UIElement adornPOD)
            : base(adornPOD)
        {
            try
            {
                visualChildren = new VisualCollection(this);

                // Call a helper method to initialize the Thumbs
                // with a customized cursors.


                BuildAdornerCorner(ref bottomRight, Cursors.SizeNS);

                // Add handlers for resizing.

                bottomRight.DragDelta += new DragDeltaEventHandler(HandleBottomRight);
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "clsPodAdorner()", "VMukti.Presentation.Controls.VMuktiGrid.clsPodAdorner.cs");
          
            }

        }

        protected override void OnRender(DrawingContext drawingContext)
        {
            //Rect rctAdorn = new Rect(this.AdornedElement.DesiredSize);

            //Point ptImage = new Point(rctAdorn.BottomRight.X - 12, rctAdorn.BottomRight.Y - 12);
            //ImageBrush objib = new ImageBrush(new System.Windows.Media.Imaging.BitmapImage(new Uri(@"Images\imgadorn.jpg",UriKind.RelativeOrAbsolute)));
            //drawingContext.DrawImage(objib.ImageSource,new Rect(ptImage,new Size(12,12)));


        }

        void HandleBottomRight(object sender, DragDeltaEventArgs args)
        {
            try
            {
                FrameworkElement adornedElement = this.AdornedElement as FrameworkElement;
                Thumb hitThumb = sender as Thumb;

                if (adornedElement == null || hitThumb == null) return;
                FrameworkElement parentElement = adornedElement.Parent as FrameworkElement;

                // Ensure that the Width and Height are properly initialized after the resize.
                EnforceSize(adornedElement);

                // Change the size by the amount the user drags the mouse, as long as it's larger 
                // than the width or height of an adorner, respectively.
                adornedElement.Width = Math.Max(adornedElement.Width + args.HorizontalChange, hitThumb.DesiredSize.Width);
                adornedElement.Height = Math.Max(args.VerticalChange + adornedElement.Height, hitThumb.DesiredSize.Height);
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "HandleBottomRight()", "VMukti.Presentation.Controls.VMuktiGrid.clsPodAdorner.cs");

            }
        }

        // Handler for resizing from the bottom-left.


        // Arrange the Adorners.
        protected override Size ArrangeOverride(Size finalSize)
        {
            try
            {
                // desiredWidth and desiredHeight are the width and height of the element that's being adorned.  
                // These will be used to place the ResizingAdorner at the corners of the adorned element.  
                double desiredWidth = AdornedElement.DesiredSize.Width;
                double desiredHeight = AdornedElement.DesiredSize.Height;
                // adornerWidth & adornerHeight are used for placement as well.
                double adornerWidth = this.DesiredSize.Width;
                double adornerHeight = this.DesiredSize.Height;

                // bottomRight.Arrange(new Rect(desiredWidth - adornerWidth / 2, desiredHeight - adornerHeight / 2, adornerWidth, adornerHeight));
                bottomRight.Arrange(new Rect(desiredWidth - adornerWidth, desiredHeight - adornerHeight / 2, adornerWidth, adornerHeight));

                // Return the final size.
                return finalSize;
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "ArrangeOverride()", "VMukti.Presentation.Controls.VMuktiGrid.clsPodAdorner.cs");
                return Size.Empty;
            }
        }

        // Helper method to instantiate the corner Thumbs, set the Cursor property, 
        // set some appearance properties, and add the elements to the visual tree.
        void BuildAdornerCorner(ref Thumb cornerThumb, Cursor customizedCursor)
        {
            try
            {
                if (cornerThumb != null) return;

                cornerThumb = new Thumb();
               // cornerThumb.Style = (Style)this.FindResource("ThumbUpDownTemplet");
                // Set some arbitrary visual characteristics.
                cornerThumb.Cursor = customizedCursor;
                cornerThumb.Height = cornerThumb.Width = 10;
                cornerThumb.Opacity = 0.40;
                visualChildren.Add(cornerThumb);
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "BuildAdornerCorner()", "VMukti.Presentation.Controls.VMuktiGrid.clsPodAdorner.cs");

            }
        }

        // This method ensures that the Widths and Heights are initialized.  Sizing to content produces
        // Width and Height values of Double.NaN.  Because this Adorner explicitly resizes, the Width and Height
        // need to be set first.  It also sets the maximum size of the adorned element.
        void EnforceSize(FrameworkElement adornedElement)
        {
            try
            {
                //if (adornedElement.Width.Equals(Double.NaN))
                //    adornedElement.Width = adornedElement.DesiredSize.Width;
                if (adornedElement.Height.Equals(Double.NaN))
                    adornedElement.Height = adornedElement.DesiredSize.Height;

                FrameworkElement parent = adornedElement.Parent as FrameworkElement;
                if (parent != null)
                {
                    adornedElement.MaxHeight = parent.ActualHeight;
                    //adornedElement.MaxWidth = parent.ActualWidth;
                }
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "EnforceSize()", "VMukti.Presentation.Controls.VMuktiGrid.clsPodAdorner.cs");

            }
        }
        //<SnippetFEVisualOverrides>
        // Override the VisualChildrenCount and GetVisualChild properties to interface with 
        // the adorner's visual collection.
        protected override int VisualChildrenCount { get { return visualChildren.Count; } }
        protected override Visual GetVisualChild(int index) { return visualChildren[index]; }
        //</SnippetFEVisualOverrides>
    }
}
