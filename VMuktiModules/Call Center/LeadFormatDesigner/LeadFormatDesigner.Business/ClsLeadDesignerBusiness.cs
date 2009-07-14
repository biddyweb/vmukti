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
using LeadFormatDesigner.Common;
using System.Data;
using LeadFormatDesigner.DataAccess;
using VMuktiAPI;
using System.Text;

namespace LeadFormatDesigner.Business
{
    public class ClsLeadDesignerBusiness : ClsBaseObject 
    {
        //public static StringBuilder sb1;

        private Int64 _ID = LeadFormatDesigner.Common.ClsConstants.NullInt;
        private string _FieldName = LeadFormatDesigner.Common.ClsConstants.NullString;
        private string _FieldType = LeadFormatDesigner.Common.ClsConstants.NullString;
        private Int64 _FieldSize = LeadFormatDesigner.Common.ClsConstants.NullInt64;
        private bool _IsRequired = true;


        public Int64 ID
        {
            get { return _ID; }
            set { _ID = value; }
        }
        public string FieldName
        {
            get { return _FieldName; }
            set { _FieldName = value; }
        }
        public string FieldType
        {
            get { return _FieldType; }
            set { _FieldType = value; }
        }
        public Int64 FieldSize
        {
            get { return _FieldSize; }
            set { _FieldSize = value; }
        }

        public bool IsRequired
        {
            get { return _IsRequired; }
            set { _IsRequired = value; }
        }

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

        public override bool MapData(DataRow row)
        {
            try
            {
                ID = GetInt64(row, "ID");
                FieldName = GetString(row, "FieldName");
                //FieldType = GetString(row, "FieldType");
                //FieldSize = GetInt64(row, "FieldType");
               //IsRequired = GetBool(row, "IsRequired");

                return base.MapData(row);
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "MapData()", "ClsLeadDesignerBusiness.cs");
                return false;
            }
        }
        
        public static ClsLeadDesignerBusiness GetFields()
        {
            try
            {
                ClsLeadDesignerBusiness obj = new ClsLeadDesignerBusiness();
                DataSet ds = new LeadFormatDesigner.DataAccess.ClsLeadDesignerDataService().LeadDesignerFields_Get();
                if (!obj.MapData(ds)) obj = null;
                return obj;
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "GetFields()", "ClsLeadDesignerBusiness.cs");
                return null;
            }

        }

        public int Save()
        {
            try
            {
                return (Save(null));
                
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "Save()", "ClsLeadDesignerBusiness.cs");
                return 0;
            }
        }

        public int Save(IDbTransaction txn)
        {
            try
            {
                int rowins = 0;
                LeadFormatDesigner.DataAccess.ClsLeadDesignerDataService LeadDesig = new ClsLeadDesignerDataService(txn);
                rowins = LeadDesig.LeadDesigner_Save(_ID, _FieldName, _FieldType, _FieldSize, _IsRequired); 
                return rowins;
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "Save()", "ClsLeadDesignerBusiness.cs");
                return 0;
            }
        }

        
    }
}
