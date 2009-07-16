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
using System.ServiceModel;

namespace Whiteboard.Business.Service.NetP2P
{

    [ServiceContract(CallbackContract = typeof(INetTcpWhiteboard))]
    public interface INetTcpWhiteboard
    {
        [OperationContract(IsOneWay = true)]
        void svcJoin(string uName);

        [OperationContract(IsOneWay = true)]
        void svcDrawRect(string from, List<string> to, string strOpName, double x1, double y1, double x2, double y2);

        [OperationContract(IsOneWay = true)]
        void svcDrawEllipse(string from, List<string> to, string strOpName, double x1, double y1, double x2, double y2);

        [OperationContract(IsOneWay = true)]
        void svcDrawLine(string from, List<string> to, string strOpName, double x1, double y1, double x2, double y2, double lineThickNess);

        [OperationContract(IsOneWay = true)]
        void svcDrawTextTool(string from, List<string> to, string strOpName, double x1, double y1, double x2, double y2);

        [OperationContract(IsOneWay = true)]
        void svcDrawStamper(string from, List<string> to, string strOpName, string strImg, double x1, double y1);

        [OperationContract(IsOneWay = true)]
        void svcDrawStrokes(string from, List<string> to, string strOpName, string strClc);

        [OperationContract(IsOneWay = true)]
        void svcClearCnv(string from, List<string> to, string strOpName);

        [OperationContract(IsOneWay = true)]
        void svcChangeThickNess(string from, List<string> to, string strOpName, double lineThickNess);

        [OperationContract(IsOneWay = true)]
        void svcChangeFontSize(string from, List<string> to, string strOpName, double sizeOfFont);

        [OperationContract(IsOneWay = true)]
        void svcChangeColor(string from, List<string> to, string strOpName, string genColo);

        [OperationContract(IsOneWay = true)]
        void svcChangeText(string from, List<string> to, string strOpName, string Text, int chldNo);

        [OperationContract(IsOneWay = true)]
        void svcGetUserList(string uName);

        [OperationContract(IsOneWay = true)]
        void svcSetUserList(string uName);

        [OperationContract(IsOneWay = true)]
        void svcSignOutChat(string from, List<string> to);

        [OperationContract(IsOneWay = true)]
        void svcUnjoin(string uName);

    }

    public interface INetTcpWhiteboardChannel : INetTcpWhiteboard, IClientChannel
    {
    }
}
