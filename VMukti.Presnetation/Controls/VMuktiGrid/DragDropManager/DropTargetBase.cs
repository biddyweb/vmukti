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
using System;
using System.Windows.Shapes;
using System.Windows.Media;
using System.Xml;
using System.IO;
using System.Windows.Markup;

namespace DnD
{
    public abstract class DropTargetBase
    {

        string supportedFormat = "SampleFormat";
        private UIElement _targetUI;

		public virtual UIElement TargetUI
		{
			get
			{
				return _targetUI;
			}
			set
			{
				_targetUI = value;
			}
		}

        public virtual string SupportedFormat
        {
            get { return supportedFormat; }
            set { supportedFormat = value; }
        }
        public virtual bool IsValidDataObject(IDataObject obj)
        {
            return (obj.GetDataPresent(supportedFormat));
        }

        public abstract void OnDropCompleted(IDataObject obj, Point dropPoint);

        public virtual UIElement GetVisualFeedback(IDataObject obj)
        {
            UIElement elt = ExtractElement(obj);
            Type t = elt.GetType();

            Rectangle rect = new Rectangle();
            elt.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Background, new System.Windows.Threading.DispatcherOperationCallback(delegate(Object state)
            {
                rect.Width = (double)t.GetProperty("ActualWidth").GetValue(elt, null);
                rect.Height = (double)t.GetProperty("ActualHeight").GetValue(elt, null);
                return null;
            }), null);
            rect.Fill = new VisualBrush(elt);
            rect.Opacity = 0.5;
            rect.IsHitTestVisible = false;

            return rect;
        }

        public virtual UIElement ExtractElement(IDataObject obj)
        {
            string xamlString = obj.GetData(supportedFormat) as string;
            XmlReader reader = XmlReader.Create(new StringReader(xamlString));
            UIElement elt = XamlReader.Load(reader) as UIElement;
            return elt;
        }
	}
}
