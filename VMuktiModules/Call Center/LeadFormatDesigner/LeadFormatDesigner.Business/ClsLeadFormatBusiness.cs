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
using System.Text;
using System.Data;
using LeadFormatDesigner.DataAccess;
using LeadFormatDesigner.Common;
using VMuktiAPI;


namespace LeadFormatDesigner.Business
{
    public class ClsLeadFormatBusiness : ClsBaseObject
    {
        //public static StringBuilder sb1;

        private Int64 _ID = LeadFormatDesigner.Common.ClsConstants.NullInt64;
        private string _LeadFormatName = LeadFormatDesigner.Common.ClsConstants.NullString;
        private string _FormatType = LeadFormatDesigner.Common.ClsConstants.NullString;
        private string _Description = LeadFormatDesigner.Common.ClsConstants.NullString;

        private Int64 _LeadFieldsID = LeadFormatDesigner.Common.ClsConstants.NullInt64;
        private Int64 _LeadFormatID = LeadFormatDesigner.Common.ClsConstants.NullInt64;
        private Int64 _FieldID = LeadFormatDesigner.Common.ClsConstants.NullInt64;
        private string _DefaultValue = LeadFormatDesigner.Common.ClsConstants.NullString;
        private bool _IsRequired = true;
        private int _StartPosition = LeadFormatDesigner.Common.ClsConstants.NullInt;
        private int _Length = LeadFormatDesigner.Common.ClsConstants.NullInt;
        private string _FieldName = LeadFormatDesigner.Common.ClsConstants.NullString;
        private string _Delimiters = LeadFormatDesigner.Common.ClsConstants.NullString;
        public static int Formatflag = 0;

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

        public ClsLeadFormatBusiness()
        {
            try
            {

        }

            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "ClsLeadFormatBusiness()", "ClsLeadFormatBusiness.cs");
                
            }
        }
        public Int64 ID
        {
            get { return _ID; }
            set { _ID = value; }
        }
        public string LeadFormatName
        {
            get { return _LeadFormatName; }
            set { _LeadFormatName = value; }
        }
        public string FormatType
        {
            get { return _FormatType; }
            set { _FormatType = value; }
        }
        public string Description
        {
            get { return _Description; }
            set { _Description = value; }
        }

        public Int64 LeadFieldsID
        {
            get { return _LeadFieldsID; }
            set { _LeadFieldsID = value; }
        }
        public Int64 LeadFormatID
        {
            get { return _LeadFormatID; }
            set { _LeadFormatID = value; }
        }
        public Int64 FieldID
        {
            get { return _FieldID; }
            set { _FieldID = value; }
        }
        public string DefaultValue
        {
            get { return _DefaultValue; }
            set { _DefaultValue = value; }
        }
        public bool IsRequired
        {
            get { return _IsRequired; }
            set { _IsRequired = value; }
        }
        public int StartPosition
        {
            get { return _StartPosition; }
            set { _StartPosition = value; }
        }
        public int Length
        {
            get { return _Length; }
            set { _Length = value; }
        }
        public string Delimiters
        {
            get { return _Delimiters; }
            set { _Delimiters = value; }
        }

        public string FieldName
        {
            get { return _FieldName; }
            set { _FieldName = value; }
        }

        public override bool MapData(DataRow row)
        {
            try
            {
                if (Formatflag == 0)
                {
                    ID = GetInt64(row, "ID");
                    LeadFormatName = GetString(row, "LeadFormatName");
                    FormatType = GetString(row, "FormatType");
                    Description = GetString(row, "Description");
                }

                if (Formatflag == 1)
                {
                    LeadFormatID = GetInt64(row, "LeadFormatID");
                    LeadFieldsID = GetInt64(row, "LeadFieldsID");
                    FieldID = GetInt64(row, "FieldID");
                    DefaultValue = GetString(row, "DefaultValue");
                    StartPosition = Getint(row, "StartPosition");
                    FieldName = GetString(row, "FieldName");
                    Length = Getint(row, "Length");
                    Delimiters = GetString(row, "Delimiters");
                }

                return base.MapData(row);
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "MapData()", "ClsLeadFormatBusiness.cs");
                return false;
            }
        }

        public static ClsLeadFormatBusiness GetByLeadFormatID(Int64 ID)
        {
            try
            {
                Formatflag = 0;
                ClsLeadFormatBusiness obj = new ClsLeadFormatBusiness();
                DataSet ds = new LeadFormatDesigner.DataAccess.ClsLeadDesignerDataService().LeadFormat_GetByID(ID);
                if (!obj.MapData(ds)) obj = null;
                return obj;
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "GetByLeadFormatID()", "ClsLeadFormatBusiness.cs");
                return null;
            }

      }
      public static int DeleteFormatField_ByID(Int64 ForamtFieldID)
      {
          try
          {
              return new LeadFormatDesigner.DataAccess.ClsLeadDesignerDataService().DeleteFormatField_ByID(ForamtFieldID);
          }
          catch (Exception ex)
          {
              VMuktiHelper.ExceptionHandler(ex, "DeleteFormatField_ByID()", "ClsLeadFormatBusiness.cs");
              return 0;
          }
      }
      public static int DeleteLeadField_ByID(Int64 FormatID)
      {
          try
          {
              return new LeadFormatDesigner.DataAccess.ClsLeadDesignerDataService().DeleteFormat_ByID(FormatID); // DeleteFormatField_ByID(FormatID); // .DeleteFormat_ByID(FormatID);
          }
          catch (Exception ex)
          {
              VMuktiHelper.ExceptionHandler(ex, "DeleteLeadField_ByID()", "ClsLeadFormatBusiness.cs");
              return 0;
          }
        }
        public static ClsLeadFormatBusiness LeadFields_GetByID(Int64 ID)
        {
            try
            {
                Formatflag = 1;
                ClsLeadFormatBusiness obj = new ClsLeadFormatBusiness();
                DataSet ds = new LeadFormatDesigner.DataAccess.ClsLeadDesignerDataService().LeadFields_GetByID(ID);
                if (!obj.MapData(ds)) obj = null;
                return obj;
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "LeadFields_GetByID()", "ClsLeadFormatBusiness.cs");
                return null;
            }

        }


        public int LeadFormatSave()
        {
            try
            {
                return (LeadFormatSave(null));

            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "LeadFormatSave()", "ClsLeadFormatBusiness.cs");
                return 0;
            }
        }

        public int LeadFormatSave(IDbTransaction txn)
        {
            try
            {
                int rowins = 0;
                LeadFormatDesigner.DataAccess.ClsLeadDesignerDataService LeadDesig = new ClsLeadDesignerDataService(txn);
                rowins = LeadDesig.LeadFormat_Save(_ID, _LeadFormatName, _FormatType, _Description);
                return rowins;
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "LeadFormatSave()", "ClsLeadFormatBusiness.cs");
                return 0;
            }
        }


        public int LeadFormatDesignerSave()
        {
            try
            {
                return (LeadFormatDesignerSave(null));

            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "LeadFormatDesignerSave()", "ClsLeadFormatBusiness.cs");
                return 0;
            }
        }

        public int LeadFormatDesignerSave(IDbTransaction txn)
        {
            try
            {
                int rowins = 0;
                LeadFormatDesigner.DataAccess.ClsLeadDesignerDataService LeadDesig = new ClsLeadDesignerDataService(txn);
                rowins = LeadDesig.LeadFormatDesigner_Save(_LeadFieldsID, _LeadFormatID, _FieldID, _DefaultValue, _IsRequired, _StartPosition, _Length, _Delimiters);
                return rowins;
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "LeadFormatDesignerSave()", "ClsLeadFormatBusiness.cs");
                return 0;
            }
        }


    }
}
