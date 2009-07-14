/* VMukti 1.0 -- An Open Source Unified Communications Engine
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
/*************************************************************************************
* WhiteBoard.cs
********************************************************************************/
/*
 * 1videoConference -- An open source video conferencing platform.
 *
 * Copyright (C) 2007 - 2008, Adiance Technologies Pvt. Ltd.
 *
 * Hardik Sanghvi <hardik.sanghvi @adiance.com>
 * 
 * See http://www.1videoconference.org for more information about
 * the 1videoConference project. Please do not directly contact 
 * any of the maintainers of this project for assistance;
 * the project provides a web site, forums and mailing lists
 * for your use.
 *
 * This program is free software, distributed under the terms of
 * the GNU General Public License Version 2. See the LICENSE file 
 * at the top of the source tree.
 ********************************************************************************/

using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Shapes;
using System.Windows.Media;
using VMuktiAPI;


namespace wb.Presentation
{
    public class WhiteBoard
    {
   
        public System.Windows.Shapes.Rectangle DrawRectangle(double x1, double y1, double x2, double y2, System.Windows.Media.Color genColor)
		{
			try
			{
				System.Windows.Shapes.Rectangle rnt = new System.Windows.Shapes.Rectangle();
				rnt.Height = y2 - y1;
				rnt.Width = x2 - x1;

				rnt.Stroke = new System.Windows.Media.SolidColorBrush(genColor);
				rnt.Fill = new System.Windows.Media.SolidColorBrush(genColor);
				rnt.Margin = new System.Windows.Thickness(x1, y1, 0, 0);
				return rnt;
			}
			catch (Exception ex)
			{
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "DrawRectangle()", "WhiteBoard.cs");
                return null;
			}
        }

        public System.Windows.Shapes.Ellipse DrawEllipse(double x1, double y1, double x2, double y2, System.Windows.Media.Color genColor)
		{
			try
			{
				System.Windows.Shapes.Ellipse elp = new System.Windows.Shapes.Ellipse();

				elp.Height = y2 - y1;
				elp.Width = x2 - x1;

				elp.Stroke = new SolidColorBrush(genColor);
				elp.Fill = new SolidColorBrush(genColor);
				elp.Margin = new System.Windows.Thickness(x1, y1, 0, 0);
				return elp;
			}
			catch (Exception ex)
			{
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "DrawEllipse()", "WhiteBoard.cs");
                return null;
			}
        }

        public System.Windows.Shapes.Line DrawLine(double x1, double y1, double x2, double y2, System.Windows.Media.Color genColor, double lineThickNess)
		{
			try
			{
				System.Windows.Shapes.Line ln = new System.Windows.Shapes.Line();

				ln.X1 = x1;
				ln.X2 = x2;
				ln.Y1 = y1;
				ln.Y2 = y2;
				Stretch st = new Stretch();
				ln.Stretch = st;
				ln.Stroke = new SolidColorBrush(genColor);
				ln.Fill = new SolidColorBrush(genColor);
				ln.StrokeThickness = lineThickNess;
				return ln;
			}
			catch (Exception ex)
			{
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "DrawLine()", "WhiteBoard.cs");
                return null;
			}
        }

        public System.Windows.Controls.RichTextBox TextTool(double x1, double y1, double x2, double y2)
		{
			try
			{
				System.Windows.Controls.RichTextBox t = new System.Windows.Controls.RichTextBox();
				t.Height = y2 - y1;
				t.Width = x2 - x1;
				t.Margin = new System.Windows.Thickness(x1, y1, 0, 0);
				return t;
			}
			catch (Exception ex)
			{
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "TextTool()", "WhiteBoard.cs");
                return null;
			}
        }

        public System.Windows.Controls.Image Stamper(double x1, double y1)
		{
			try
			{
				System.Windows.Controls.Image i = new System.Windows.Controls.Image();
				i.Height = 20;
				i.Width = 20;
				i.Margin = new System.Windows.Thickness(x1, y1, 0, 0);
				return i;
			}
			catch (Exception ex)
			{
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "Stamper()", "WhiteBoard.cs");
                return null;

			}
        }
    }
}
