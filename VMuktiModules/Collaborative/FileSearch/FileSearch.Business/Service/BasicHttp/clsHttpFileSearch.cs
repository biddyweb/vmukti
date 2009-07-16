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
using FileSearch.Business.Service.DataContracts;

namespace FileSearch.Business.Service.BasicHttp
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    public class clsHttpFileSearch : IHttpFileSearch
    {
        public delegate void DelsvcHttpJoin(string uName);
        public delegate void DelsvcHttpSendQuery(string uName, string Query);
        public delegate void DelsvcHttpReplyQuery(string uName, string[] QueryResult);
        public delegate List<clsMessage> DelsvcHttpGetMessage(string recipient);
        public delegate string[] DelsvcHttpGetFileList(string recipient);
        public delegate void DelsvcHttpFileRequest(string FilePath, string FileTo, string FileFrom);
        public delegate void DelsvcHttpFileReply(byte[] FileBlock, string FileTo, string FileFrom, string FileName,int Signal);
        public delegate List<byte[]> DelsvcHttpDownloadFile(string UserName, string FileName);
        public delegate void DelsvcHttpUnJoin(string uName);

        public delegate IAsyncResult delBeginsvcHttpGetMessage(string recipient, AsyncCallback callback, object asyncState);
        public delegate List<clsMessage> delEndsvcHttpGetMessage(IAsyncResult result);

        public delegate IAsyncResult delBeginsvcHttpGetFileList(string recipient, AsyncCallback callback, object asyncState);
        public delegate string[] delEndsvcHttpGetFileList(IAsyncResult result);

        public event DelsvcHttpJoin EntsvcHttpJoin;
        public event DelsvcHttpSendQuery EntsvcHttpSendQuery;
        public event DelsvcHttpReplyQuery EntsvcHttpReplyQuery;
        public event DelsvcHttpGetMessage EntsvcHttpGetMessage;
        public event DelsvcHttpGetFileList EntsvcHttpGetFileList;
        public event DelsvcHttpFileRequest EntsvcHttpFileRequest;
        public event DelsvcHttpFileReply EntsvcHttpFileReply;
        public event DelsvcHttpDownloadFile EntsvcHttpDownloadFile;
        public event DelsvcHttpUnJoin EntsvcHttpUnJoin;

        public event delBeginsvcHttpGetMessage EntBeginsvcHttpGetMessage;
        public event delEndsvcHttpGetMessage EntEndsvcHttpGetMessage;

        public event delBeginsvcHttpGetFileList EntBeginsvcHttpGetFileList;
        public event delEndsvcHttpGetFileList EntEndsvcHttpGetFileList;



        public void svcHttpJoin(string uName)
        {
            if (EntsvcHttpJoin != null)
            {
                EntsvcHttpJoin(uName);
            }
        }

        public void svcHttpSendQuery(string uName, string Query)
        {
            if (EntsvcHttpSendQuery != null)
            {
                EntsvcHttpSendQuery(uName, Query);
            }
        }

        public void svcHttpReplyQuery(string uName, string[] QueryResult)
        {
            if (EntsvcHttpReplyQuery != null)
            {
                EntsvcHttpReplyQuery(uName, QueryResult);
            }
        }

        public List<clsMessage> svcHttpGetMessage(string recipient)
        {
            if (EntsvcHttpGetMessage != null)
            {
                return EntsvcHttpGetMessage(recipient);
            }
            else
            {
                return null;
            }
        }

        public string[] svcHttpGetFileList(string recipient)
        {
            if (EntsvcHttpGetFileList != null)
            {
                return EntsvcHttpGetFileList(recipient);
            }
            else
            {
                return null;
            }
        }

        public void svcHttpFileRequest(string FilePath, string FileTo, string FileFrom)
        {
            if (EntsvcHttpFileRequest != null)
            {
                EntsvcHttpFileRequest(FilePath,FileTo,FileFrom);
            }
            
        }

        public void svcHttpFileReply(byte[] FileBlock, string FileTo, string FileFrom, string FileName, int Signal)
        {
            if (EntsvcHttpFileReply != null)
            {
                EntsvcHttpFileReply(FileBlock, FileTo, FileFrom, FileName, Signal);
            }
        }

        public List<byte[]> svcHttpDownloadFile(string UserName, string FileName)
        {
            if (EntsvcHttpDownloadFile != null)
            {
                return EntsvcHttpDownloadFile(UserName, FileName);
            }
            else
            {
                return null;
            }
        }

        public void svcHttpUnJoin(string uName)
        {
            if (EntsvcHttpUnJoin != null)
            {
                EntsvcHttpUnJoin(uName);
            }
        }

        public IAsyncResult BeginsvcHttpGetMessage(string recipient, AsyncCallback callback, object asyncState)
        {
            if (EntBeginsvcHttpGetMessage != null)
            {
                return EntBeginsvcHttpGetMessage(recipient, callback, asyncState);
            }
            else
            {
                return null;
            }
        }

        public List<clsMessage> EndsvcHttpGetMessage(IAsyncResult result)
        {
            if (EntEndsvcHttpGetMessage != null)
            {
                return EntEndsvcHttpGetMessage(result);
            }
            else
            {
                return null;
            }
        }

        public IAsyncResult BeginsvcHttpGetFileList(string recipient, AsyncCallback callback, object asyncState)
        {
            if (EntBeginsvcHttpGetFileList != null)
            {
                return EntBeginsvcHttpGetFileList(recipient, callback, asyncState);
            }
            else
            {
                return null;
            }
        }

        public string[] EndsvcHttpGetFileList(IAsyncResult result)
        {
            if (EntEndsvcHttpGetFileList != null)
            {
                return EntEndsvcHttpGetFileList(result);
            }
            else
            {
                return null;
            }
        }


       
    }
}
