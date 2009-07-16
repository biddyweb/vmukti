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

namespace FileSearch.Business
{
    [ServiceContract(CallbackContract = typeof(IFileTransfer))]
    public interface IFileTransfer
    {
        [OperationContract(IsOneWay = true)]
        void svcJoin(string uName);

        [OperationContract(IsOneWay = true)]
        void svcSearchQuery(string strQuery, string uName, List<string> GuestList);

        [OperationContract(IsOneWay = true)]
        void svcSearchResult(string[] strResult, string uName);

        [OperationContract(IsOneWay = true)]
        void svcRequestFile(string FileName, string To, string From);

        [OperationContract(IsOneWay = true)]
        void svcSendFileBlock(byte[] arr, string To, string From, string FileName, int signal);

        [OperationContract(IsOneWay = true)]
        void svcGetUserList(string uName);

        [OperationContract(IsOneWay = true)]
        void svcSetUserList(string uName);

        [OperationContract(IsOneWay = true)]
        void svcUnJoin(string uName);

    }

    public interface IFileTransferChannel : IFileTransfer, IClientChannel
    {
    }

    //public class svcFileTransfer:IFileTransfer
    //{
    //    public delegate void delsvcJoin(string uName);
    //    public delegate void delsvcSearchQuery(string strQuery, string uName);
    //    public delegate void delsvcSearchResult(string[] strResult, string uName);
    //    public delegate void delsvcRequestFile(string FileName, string To, string From);
    //    public delegate void delsvcSendFileBlock(byte[] arr, string To, string From, int signal);
    //    public delegate void delsvcUnJoin(string uName);

    //    public event delsvcJoin EntsvcJoin;
    //    public event delsvcSearchQuery EntsvcSearchQuery;
    //    public event delsvcSearchResult EntsvcSearchResult;
    //    public event delsvcRequestFile EntsvcRequestFile;
    //    public event delsvcSendFileBlock EntsvcSendFileBlock;
    //    public event delsvcUnJoin EntsvcUnJoin;

    //    public void svcJoin(string uName)
    //    {
    //        if (EntsvcJoin != null)
    //        {
    //            EntsvcJoin(uName);
    //        }
    //    }

    //    public void svcSearchQuery(string strQuery, string uName)
    //    {
    //        if (EntsvcSearchQuery != null)
    //        {
    //            EntsvcSearchQuery(strQuery, uName);
    //        }
    //    }
    //    public void svcSearchResult(string[] strResult, string uName)
    //    {
    //        if (EntsvcSearchResult != null)
    //        {
    //            EntsvcSearchResult(strResult, uName);
    //        }
    //    }

    //    public void svcRequestFile(string FileName, string To, string From)
    //    {
    //        if (EntsvcRequestFile != null)
    //        {
    //            EntsvcRequestFile(FileName, To, From);
    //        }
    //    }

    //    public void svcSendFileBlock(byte[] arr, string To, string From, int signal)
    //    {
    //        if (EntsvcSendFileBlock != null)
    //        {
    //            EntsvcSendFileBlock(arr, To, From, signal);
    //        }
    //    }


    //    public void svcUnJoin(string uName)
    //    {
    //        if (EntsvcUnJoin != null)
    //        {
    //            EntsvcUnJoin(uName);
    //        }
    //    }

    //}
}
