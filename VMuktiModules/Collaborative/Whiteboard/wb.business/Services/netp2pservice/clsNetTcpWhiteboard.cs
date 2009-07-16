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
using System;
using System.Collections.Generic;
//using System.Linq;
using System.Text;

namespace Whiteboard.Business.Service.NetP2P
{
    public class clsNetTcpWhiteboard :INetTcpWhiteboard
    {
        public delegate void UserJoin(string uname);
        public delegate void DrawRect(string from, List<string> to, string strOpName, double x1, double y1, double x2, double y2);
        public delegate void DrawEllipse(string from, List<string> to, string strOpName, double x1, double y1, double x2, double y2);
        public delegate void DrawLine(string from, List<string> to, string strOpName, double x1, double y1, double x2, double y2,double thickness);
        public delegate void DrawTextTool(string from, List<string> to, string strOpName, double x1, double y1, double x2, double y2);
        public delegate void DrawStamper(string from, List<string> to, string strOpName, string strImg, double x1, double y1);
        public delegate void DrawStrokes(string from, List<string> to, string strOpName, string strokecol);
        public delegate void ClearCnv(string from, List<string> to, string strOpName);
        public delegate void ChangeThickness(string from, List<string> to, string strOpName, double thickness);
        public delegate void ChangeFontSize(string from, List<string> to, string strOpName, double fontSize);
        public delegate void ChangeColor(string from, List<string> to, string strOpName, string color);
        public delegate void ChangeText(string from, List<string> to, string strOpName, string text, int chldNo);
        public delegate void GetUserList(string uName);
        public delegate void SetUserList(string uName);
        public delegate void SignOutChat(string from, List<string> to);
        public delegate void Unjoin(string uName);


        public event UserJoin EJoin;
        public event DrawRect ERect;
        public event DrawEllipse EEllipse;
        public event DrawLine ELine;
        public event DrawTextTool ETTool;
        public event DrawStamper EStamper;
        public event DrawStrokes EStrokes;
        public event ClearCnv EClear;
        public event ChangeThickness EThickness;
        public event ChangeFontSize EFontSize;
        public event ChangeColor EColor;
        public event ChangeText EText;
        public event GetUserList EGetUserList;
        public event SetUserList ESetUserList;
        public event SignOutChat ESignOutChat;
        public event Unjoin EUnjoin;


        public void svcJoin(string uname)
        {
            EJoin(uname);
        }

        public void svcDrawRect(string from, List<string> to, string strOpName, double x1, double y1, double x2, double y2)
        {
            ERect(from, to, strOpName, x1, y1, x2, y2);
        }

        public void svcDrawEllipse(string from, List<string> to, string strOpName, double x1, double y1, double x2, double y2)
        {
            EEllipse(from, to, strOpName, x1, y1, x2, y2);
        }

        public void svcDrawLine(string from, List<string> to, string strOpName, double x1, double y1, double x2, double y2, double lineThickNess)
        {
            ELine(from, to, strOpName, x1, y1, x2, y2, lineThickNess);
        }

        public void svcDrawTextTool(string from, List<string> to, string strOpName, double x1, double y1, double x2, double y2)
        {
            ETTool(from, to, strOpName, x1, y1, x2, y2);
        }

        public void svcDrawStamper(string from, List<string> to, string strOpName, string strImg, double x1, double y1)
        {
            EStamper(from, to, strOpName, strImg, x1, y1);
        }

        public void svcDrawStrokes(string from, List<string> to, string strOpName, string strClc)
        {
            EStrokes(from, to, strOpName, strClc);
        }

        public void svcClearCnv(string from, List<string> to, string strOpName)
        {
            EClear(from, to, strOpName);
        }

        public void svcChangeThickNess(string from, List<string> to, string strOpName, double lineThickNess)
        {
            EThickness(from, to, strOpName, lineThickNess);
        }

        public void svcChangeFontSize(string from, List<string> to, string strOpName, double sizeOfFont)
        {
            EFontSize(from, to, strOpName, sizeOfFont);
        }

        public void svcChangeColor(string from, List<string> to, string strOpName, string genColo)
        {
            EColor(from, to, strOpName, genColo);
        }

        public void svcChangeText(string from, List<string> to, string strOpName, string Text, int chldNo)
        {
            EText(from, to, strOpName, Text, chldNo);
        }

        public void svcGetUserList(string uName)
        {
            if (EGetUserList != null)
            {
                EGetUserList(uName);
            }
        }

        public void svcSetUserList(string uName)
        {
            if (ESetUserList != null)
            {
                ESetUserList(uName);
            }
        }

        public void svcSignOutChat(string from, List<string> to)
        {
            if (ESignOutChat != null)
            {
                ESignOutChat(from, to);
            }
        }

        public void svcUnjoin(string uName)
        {
            if (EUnjoin != null)
            {
                EUnjoin(uName);
            }
        }

    }
}
