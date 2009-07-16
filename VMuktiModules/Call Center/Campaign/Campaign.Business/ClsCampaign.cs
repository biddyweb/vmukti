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
using Campaign.Common;
using System.Data;
using VMuktiAPI;
using System.Text;

namespace Campaign.Business
{
    public class ClsCampaign : ClsBaseObject
    {
        //public static StringBuilder sb1;
        #region Fields

        private Int64 _ID = Campaign.Common.ClsConstants.NullLong;
        private string _Name = Campaign.Common.ClsConstants.NullString;
        private string _Description = Campaign.Common.ClsConstants.NullString;
        private int _NoOfChannels = Campaign.Common.ClsConstants.NullInt;
        private string _Prefix = Campaign.Common.ClsConstants.NullString;
        private Int64 _CallerID = Campaign.Common.ClsConstants.NullInt;
        private bool _IsActive = false;
        private string _DType = Campaign.Common.ClsConstants.NullString;
        private string _AssignTo = Campaign.Common.ClsConstants.NullString;
        private int _ScriptID = Campaign.Common.ClsConstants.NullInt;
        private int _CRMID = Campaign.Common.ClsConstants.NullInt;
        private int _ParkExtension = Campaign.Common.ClsConstants.NullInt;
        private string _ParkFileName = Campaign.Common.ClsConstants.NullString;
        private DateTime _StartDate = Campaign.Common.ClsConstants.NullDateTime;
        private DateTime _EndDate = Campaign.Common.ClsConstants.NullDateTime;
        private DateTime _ModifiedDate = Campaign.Common.ClsConstants.NullDateTime;
        private int _CallingTime = Campaign.Common.ClsConstants.NullInt;
        private string _RecordingFileFormat = Campaign.Common.ClsConstants.NullString;
        private int _ByUserID = Campaign.Common.ClsConstants.NullInt;

        #endregion 

        #region Properties

        public Int64 ID
        {
            get { return _ID; }
            set { _ID = value; }
        }

        public string Name
        {
            get { return _Name; }
            set { _Name = value; }
        }

        public string Description
        {
            get { return _Description; }
            set { _Description = value; }
        }

        public int NoOfChannels
        {
            get { return _NoOfChannels; }
            set { _NoOfChannels = value; }
        }

        public string Prefix
        {
            get { return _Prefix; }
            set { _Prefix = value; }
        }

        public Int64 CallerID
        {
            get { return _CallerID; }
            set { _CallerID = value; }
        }

        public bool IsActive
        {
            get { return _IsActive; }
            set { _IsActive = value; }
        }

        public string DType
        {
            get { return _DType; }
            set { _DType = value; }
        }

        public string AssignTo
        {
            get { return _AssignTo; }
            set { _AssignTo = value; }
        }

        public int ScriptID
        {
            get { return _ScriptID; }
            set { _ScriptID = value; }
        }

        public int CRMID
        {
            get { return _CRMID; }
            set { _CRMID = value; }
        }

        public int ParkExtension
        {
            get { return _ParkExtension; }
            set { _ParkExtension = value; }
        }

        public string ParkFileName
        {
            get { return _ParkFileName; }
            set { _ParkFileName = value; }
        }

        public DateTime StartDate
        {
            get { return _StartDate; }
            set { _StartDate = value; }
        }

        public DateTime EndDate
        {
            get { return _EndDate; }
            set { _EndDate = value; }
        }

        public DateTime ModifiedDate
        {
            get { return _ModifiedDate; }
            set { _ModifiedDate = value; }
        }

        public int CallingTime
        {
            get { return _CallingTime; }
            set { _CallingTime = value; }
        }

        public string RecordingFileFormat
        {
            get { return _RecordingFileFormat; }
            set { _RecordingFileFormat = value; }
        }

        public int ByUserID
        {
            get { return _ByUserID; }
            set { _ByUserID = value; }
        }

        #endregion 

        #region Methods

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
                ID = GetLong(row, "ID");
                Name = GetString(row, "Name");
                Description = GetString(row, "Description");
                NoOfChannels = GetInt(row, "NoOfChannels");
                Prefix = GetString(row, "CampaignPrefix");
                CallerID = GetLong(row, "CallerID");
                IsActive = GetBool(row, "IsActive");
                DType = GetString(row, "Type");
                AssignTo = GetString(row, "AssignTo");
                ScriptID = GetInt(row, "ScriptID");
                CRMID = GetInt(row, "CRMID");
                ParkExtension = GetInt(row, "ParkExtension");
                ParkFileName = GetString(row, "ParkFileName");
                StartDate = GetDateTime(row, "StartDate");
                EndDate = GetDateTime(row, "EndDate");
                CallingTime = GetInt(row, "CallingTime");
                RecordingFileFormat = GetString(row, "RecordingFileFormat");
                ByUserID = GetInt(row, "ModifiedBy");
                ModifiedDate = GetDateTime(row, "ModifiedDate");

                return base.MapData(row);
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "MapData()", "ClsCampaign.cs");
                return false;
            }
        }

        public static ClsCampaign GetByCampaignID(Int64 campaignID)
        {
            try
            {
                ClsCampaign obj = new ClsCampaign();
                DataSet ds = new Campaign.DataAccess.ClsCampaignDataService().Campaign_GetByID(campaignID);
                if (!obj.MapData(ds)) obj = null;
                return obj;
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "GetByCampaignID()", "ClsCampaign.cs");
                return null;
            }
        }

        public static void RemoveJoin(Int64 campaignID)
        {
            try
            {
                RemoveJoin(campaignID, null);
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "RemoveJoin()", "ClsCampaign.cs");
            } 
        }

        public static void RemoveJoin(Int64 campaignID, IDbTransaction txn)
        {
            try
            {
                new Campaign.DataAccess.ClsCampaignDataService(txn).Campaign_RemoveJoins(campaignID);
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "RemoveJoin()", "ClsCampaign.cs");  
            } 
        }

        public static void Delete(Int64 campaignID)
        {
            try
            {
                Delete(campaignID, null);
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "Delete()", "ClsCampaign.cs");  
            } 
        }

        public static void Delete(Int64 campaignID, IDbTransaction txn)
        {
            try
            {
                //Calling Campaign_Delete function for deleting the campaign for the particular ID
                new Campaign.DataAccess.ClsCampaignDataService(txn).Campaign_Delete(campaignID);
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "Delete()", "ClsCampaign.cs");  
            } 
        }

        public void Delete()
        {
            try
            {
                Delete();
            }

            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "Delete()", "ClsCampaign.cs");  
            } 
        }

        public void Delete(IDbTransaction txn)
        {
            try
            {
                Delete(txn);
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "Delete()", "ClsCampaign.cs");  
            } 
        }

        public Int64 Save()
        {
            try
            {
                return (Save(null));
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "Save()", "ClsCampaign.cs");  
                return 0;
            }
        }

        public Int64 Save(IDbTransaction txn)
        {
            try
            {
                //Calling Campaign_save function from ClsCampaignDataService
                return (new Campaign.DataAccess.ClsCampaignDataService(txn).Campaign_Save(ref _ID, _Name, _Description, _NoOfChannels,_Prefix, _CallerID, _IsActive, _DType, _AssignTo, _ScriptID, _CRMID, _ParkExtension, _ParkFileName, _StartDate, _EndDate, _CallingTime, _RecordingFileFormat, _ByUserID));
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "Save()", "ClsCampaign.cs");  
                return 0;
            } 
        }

        #endregion 
    }
}
