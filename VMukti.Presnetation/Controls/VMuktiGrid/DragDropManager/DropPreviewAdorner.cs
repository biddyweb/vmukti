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
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;
using System.Text;
//using com.sun.star.uno;

namespace DnD
{
    public class DropPreviewAdorner : Adorner
    {

		private ContentPresenter _presenter;
        private double _left = 0;
        private double _top = 0;

        public double Left
        {
            get { return _left; }
            set 
            { 
                _left = value;
                UpdatePosition();
            }
        }

        public double Top
        {
            get { return _top; }
            set 
            {
                _top = value;
                UpdatePosition();
            }
        }

        public DropPreviewAdorner(UIElement feedbackUI, UIElement adornedElt) : base(adornedElt)
        {
            
			_presenter = new ContentPresenter();
			_presenter.Content = feedbackUI;
			_presenter.IsHitTestVisible = false;
            this.Visibility = Visibility.Collapsed;
        }
            
        private void UpdatePosition()
        {
            
			AdornerLayer layer = this.Parent as AdornerLayer;
			if (layer != null)
            {
				layer.Update(AdornedElement);
            }
        }
       
        protected override Size MeasureOverride(Size constraint)
        {
            
			_presenter.Measure(constraint);
			return _presenter.DesiredSize;
        }
            
        
        protected override Size ArrangeOverride(Size finalSize)
        {
           
			_presenter.Arrange(new Rect(finalSize));
            return finalSize;
        }
            
        protected override Visual GetVisualChild(int index)
        {
            

			return _presenter;
        }
            
        protected override int VisualChildrenCount
        {
            get
            {
                return 1;
            }
        }

    	public override GeneralTransform GetDesiredTransform(GeneralTransform transform)
        {
            
            GeneralTransformGroup result = new GeneralTransformGroup();
            result.Children.Add(new TranslateTransform(Left, Top));
            if (this.Left > 0) this.Visibility = Visibility.Visible;
            result.Children.Add(base.GetDesiredTransform(transform));

            return result;
        }
            
    }
}
