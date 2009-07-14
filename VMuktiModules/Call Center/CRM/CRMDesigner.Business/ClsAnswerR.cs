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
using CRMDesigner.Common;
using System.Data;
using CRMDesigner.DataAccess;
using System.Text;
using VMuktiAPI;

namespace CRMDesigner.Business
{
    public class ClsAnswerR : ClsBaseObject
    {
        #region Fields
        //public static StringBuilder sb1;
        //public static StringBuilder CreateTressInfo()
        //{
        //    StringBuilder sb = new StringBuilder();
        //    sb.Append("User Is : " + VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName);
        //    sb.AppendLine();
        //    sb.Append("Peer Type is : " + VMuktiAPI.VMuktiInfo.CurrentPeer.CurrPeerType.ToString());
        //    sb.AppendLine();
        //    sb.Append("User's SuperNode is : " + VMuktiAPI.VMuktiInfo.CurrentPeer.SuperNodeIP);
        //    sb.AppendLine();
        //    sb.Append("User's Machine Ip Address : " + VMuktiAPI.GetIPAddress.ClsGetIP4Address.GetIP4Address());
        //    sb.AppendLine();
        //    sb.AppendLine("----------------------------------------------------------------------------------------");
        //    return sb;
        //}
        private int _ID = CRMDesigner.Common.ClsConstants.NullInt;
        private int _CallID = CRMDesigner.Common.ClsConstants.NullInt;
        private int _QusOptionID = CRMDesigner.Common.ClsConstants.NullInt;
        private bool _Value = false;
	    
        #endregion 

        #region Properties

        public int ID
        {
            get{return _ID;}
            set{_ID = value;}
        }

        public int CallID
        {
            get { return _CallID; }
            set { _CallID = value; }
        }

        public int QusOptionID
        {
            get { return _QusOptionID; }
            set { _QusOptionID = value; }
        }

        public bool Value
        {
            get { return _Value; }
            set { _Value = value; }
        }

        #endregion 

        #region Methods

        public override bool MapData(DataRow row)
        {
            try
            {
                ID = GetInt(row, "ID");
                CallID = GetInt(row, "CallId");
                QusOptionID = GetInt(row, "QusOptionID");
                Value = GetBool(row, "Value");
                return base.MapData(row);
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "MapData()", "ClsAnswerR.cs");
                return false;
            }
        }

        public void Save()
        {
            try
            {
                Save(null);
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "Save()", "ClsAnswerR.cs");
            }
        }

        public void Save(IDbTransaction txn)
        {
            try
            {
                new CRMDesigner.DataAccess.ClsDynamicScriptDataService(txn).Answer_Save(_CallID, _QusOptionID);
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "Save()", "ClsAnswerR.cs");
            }
        }

        #endregion 
    }
}
