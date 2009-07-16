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
using Calender.Business.Service;


namespace Calender.Presentation
{
    [ServiceContract]
    public interface IClsMailDBService
    {
        [OperationContract(IsOneWay = false)]
        void svcJoin(string uName);

        [OperationContract(IsOneWay = false)]
        clsDataBaseInfo svcExecuteDataSet(string querystring);

        [OperationContract(IsOneWay = false, Name = "ExecuteStoredProcedure")]
        clsDataBaseInfo svcExecuteDataSet(string spName, clsSqlParameterContract objSParam);

        [OperationContract(IsOneWay = true)]
        void svcExecuteNonQuery(string spName, clsSqlParameterContract objSParam);

        [OperationContract(IsOneWay = false)]
        int svcExecuteReturnNonQuery(string spName, clsSqlParameterContract objSParam);

        [OperationContract(IsOneWay = true)]
        void svcSendMail(clsMailInfo objMail);

        [OperationContract(IsOneWay = true)]
        void svcUnJoin(string uName);
    }

    public interface IClsMailDBServiceChannel : IClientChannel, IClsMailDBService
    { }


    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    class MailDBService:IClsMailDBService
    {
        public delegate void DelsvcJoin(string uname);
        public delegate clsDataBaseInfo DelsvcExecuteDataSet(string querystring);
        public delegate clsDataBaseInfo DelsvcExecuteStoredProcedure(string spName, clsSqlParameterContract objSParam);
        public delegate void DelsvcExecuteNonQuery(string spName, clsSqlParameterContract objSParam);
        public delegate int DelsvcExecuteReturnNonQuery(string spName, clsSqlParameterContract objSParam);
        public delegate void DelsvcSendMail(clsMailInfo objMail);
        public delegate void DelsvcUnJoin(string uname);

        public event DelsvcJoin EntsvcJoin;
        public event DelsvcExecuteDataSet EntsvcExecuteDataSet;
        public event DelsvcExecuteStoredProcedure EntsvcExecuteStoredProcedure;
        public event DelsvcExecuteNonQuery EntsvcExecuteNonQuery;
        public event DelsvcExecuteReturnNonQuery EntsvcExecuteReturnNonQuery;
        public event DelsvcSendMail EntsvcSendMail;
        public event DelsvcUnJoin EntsvcUnJoin;       
       

        #region IClsMailDBService Members

        public void svcJoin(string uName)
        {
            if (EntsvcJoin != null)
            {
                EntsvcJoin(uName);
            }
        }

        public clsDataBaseInfo svcExecuteDataSet(string querystring)
        {
            if (EntsvcExecuteDataSet != null)
            {
                return (EntsvcExecuteDataSet(querystring));
            }
            else
            {
                return null;
            }
        }

        public clsDataBaseInfo svcExecuteDataSet(string spName, clsSqlParameterContract objSParam)
        {
            if (EntsvcExecuteStoredProcedure != null)
            {
                return (EntsvcExecuteStoredProcedure(spName, objSParam));
            }
            else
            {
                return null;
            }
        }

        public void svcExecuteNonQuery(string spName, clsSqlParameterContract objSParam)
        {
            if (EntsvcExecuteNonQuery != null)
            {
                EntsvcExecuteNonQuery(spName, objSParam);
            }                
        }

        public int svcExecuteReturnNonQuery(string spName, clsSqlParameterContract objSParam)
        {
            if(EntsvcExecuteReturnNonQuery!=null)
            {
                return (EntsvcExecuteReturnNonQuery(spName, objSParam));
            }
            else
            {
                return -1;
            }
        }

        public void svcSendMail(clsMailInfo objMail)
        {
            if (EntsvcSendMail != null)
            {
                EntsvcSendMail(objMail);
            }
        }

        public void svcUnJoin(string uName)
        {
            if (EntsvcUnJoin != null)
            {
                EntsvcUnJoin(uName);
            }
        }

        #endregion

    }
}
