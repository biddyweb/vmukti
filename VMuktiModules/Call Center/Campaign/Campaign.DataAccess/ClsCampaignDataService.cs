/* VMukti 2.0 -- An Open Source Video Communications Suite
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
using System.Data;
using System.Data.SqlClient;


namespace Campaign.DataAccess
{
    public class ClsCampaignDataService : ClsDataServiceBase
    {

        public ClsCampaignDataService() : base() { }

        ////////////////////////////////////////////////////////////////////////
        /// <summary>
        ///	Creates a new DataService and specifies a transaction with
        ///	which to operate
        /// </summary>
        ////////////////////////////////////////////////////////////////////////
        public ClsCampaignDataService(IDbTransaction txn) : base(txn) { }

        //Function for getting all campaign data from database
        public DataSet Campaign_GetAll()
        {
            try
            {
                return ExecuteDataSet("Select * from vCampaign;", CommandType.Text, null);
            }
            catch (Exception exp)
            {
                System.Windows.Forms.MessageBox.Show(exp.Message);
                return null;
            }
         
        }
        //Function for getting all group data from database
        public DataSet Group_GetAll(Int64 CampId)
        {
            try
            {
                if(CampId == -1)
                    return ExecuteDataSet("Select ID,GroupName,IsActive from vGroup where ID not in (select GroupID from CampaignGroup);", CommandType.Text, null);
                else
                    return ExecuteDataSet("Select [Group].ID,[Group].GroupName,[Group].IsActive from [Group],CampaignGroup where CampaignGroup.GroupId=[Group].ID and CampaignGroup.CampaignID='" + CampId + "' and [Group].IsDeleted=0;", CommandType.Text, null);

            }
            catch (Exception exp)
            {
                System.Windows.Forms.MessageBox.Show(exp.Message);
                return null;
            }

        }
        //Function for getting all script data from database
        public DataSet Script_GetAll()
        {
            try
            {
                return ExecuteDataSet("Select ID,ScriptName,IsActive from vScript;", CommandType.Text, null);
            }
            catch (Exception exp)
            {
                System.Windows.Forms.MessageBox.Show(exp.Message);
                return null;
            }

        }
        //Function for getting all CRM data from database
        public DataSet CRM_GetAll()
        {
            try
            {
                return ExecuteDataSet("Select ID,CRMName,IsActive from vCRM;", CommandType.Text, null);
            }
            catch (Exception exp)
            {
                System.Windows.Forms.MessageBox.Show(exp.Message);
                return null;
            }

        }

        public DataSet Treatment_GetAll(Int64 CampId)
        {
            try
            {
                if (CampId == -1)
                    return ExecuteDataSet("Select ID,TreatmentName from vTreatment;", CommandType.Text, null);
                else
                    return ExecuteDataSet("Select Treatment.ID,Treatment.TreatmentName from Treatment,CampaignTreatment where CampaignTreatment.TreatmentId=Treatment.ID and CampaignTreatment.CampaignID='" + CampId + "';", CommandType.Text, null);
            }
            catch (Exception exp)
            {
                System.Windows.Forms.MessageBox.Show(exp.Message);
                return null;
            }

        }
        //Function for getting all Callinglist data from database
        public DataSet CallingList_GetAll(bool IsDNC, Int64 CampId)
        {
            try
            {
                if (CampId == -1)
                {
                    if (IsDNC == false)
                        return ExecuteDataSet("Select ID,ListName,IsActive from vCallingList where IsDNCList=0 and ID not in (select ListId from CampaignCallingList);", CommandType.Text, null);
                    else
                        return ExecuteDataSet("Select ID,ListName,IsActive from vCallingList where IsDNCList=1 and ID not in (select ListId from CampaignCallingList);", CommandType.Text, null);
                }
                else
                {
                    if (IsDNC == false)
                        return ExecuteDataSet("Select CallingList.ID,ListName,IsActive from CallingList,CampaignCallingList where CampaignCallingList.ListID=CallingList.ID and CampaignCallingList.CampaignId='" + CampId + "'  and CallingList.IsDNCList=0 and CallingList.IsDeleted=0;", CommandType.Text, null);
                    else
                        return ExecuteDataSet("Select CallingList.ID,ListName,IsActive from CallingList,CampaignCallingList where CampaignCallingList.ListID=CallingList.ID and CampaignCallingList.CampaignId='" + CampId + "'  and CallingList.IsDNCList=1 and CallingList.IsDeleted=0;", CommandType.Text, null);
                }
            }
            catch (Exception exp)
            {
                System.Windows.Forms.MessageBox.Show(exp.Message);
                return null;
            }

        }
        //Function for getting all dispositionlist data from database
        public DataSet DispositionList_GetAll(Int64 CampId)
        {
            try
            {
                if(CampId==-1)
                return ExecuteDataSet("Select ID,DespsitionListName,IsActive from vDispositionList;", CommandType.Text, null);
                else
                    return ExecuteDataSet("Select DispositionList.ID,DespsitionListName,IsActive from DispositionList,CampaignDespoList where CampaignDespoList.DespositionListId=DispositionList.ID and CampaignDespoList.CampaignId='" + CampId + "';", CommandType.Text, null);
            }
            catch (Exception exp)
            {
                System.Windows.Forms.MessageBox.Show(exp.Message);
                return null;
            }

        }

        public DataSet Campaign_GetByID(Int64 ID)
        {
            return ExecuteDataSet("spGCampaign", CommandType.StoredProcedure, CreateParameter("@pID", SqlDbType.BigInt, ID));
        }

        public void Campaign_RemoveJoins(Int64 campaignID)
        {
            ExecuteNonQuery("spDCampaignJoinEntries", CreateParameter("@pID", SqlDbType.BigInt, campaignID));
        }
        //save campaign details to database using stored procedure
        public Int64 Campaign_Save(ref Int64 ID, string Name, string Description, int NoOfChannels,string CampaignPrefix, Int64 CallerID, bool IsActive, string DType, string AssignTo, int ScriptID,int CRMID, int ParkExtension, string ParkFileName, DateTime StartDate, DateTime EndDate, int CallingTime, string RecordingFileFormat, int ByUserID)
        {
            Int64 ReturnID=0;
            SqlCommand cmd;
            try
            {
                ExecuteNonQuery(out cmd, "spAECampaign",
                    CreateParameter("@pID", SqlDbType.BigInt, ID),
                    CreateParameter("@pName", SqlDbType.NVarChar, Name, 50),
                    CreateParameter("@pDescription", SqlDbType.NVarChar, Description, 100),
                    CreateParameter("@pNoOfChannels", SqlDbType.Int, NoOfChannels),
                    CreateParameter("@pCampaignPrefix", SqlDbType.NVarChar, CampaignPrefix,10),
                    CreateParameter("@pCallerID", SqlDbType.BigInt, CallerID),
                    CreateParameter("@pIsActive", SqlDbType.Bit, IsActive),
                    CreateParameter("@pType", SqlDbType.NVarChar, DType, 50),
                    CreateParameter("@pAssignTo", SqlDbType.NVarChar, AssignTo, 10),
                    CreateParameter("@pScriptID", SqlDbType.BigInt, ScriptID),
                    CreateParameter("@pCRMID", SqlDbType.BigInt, CRMID),
                    CreateParameter("@pParkExtension", SqlDbType.Int, ParkExtension),
                    CreateParameter("@pParkFileName", SqlDbType.NVarChar, ParkFileName, 50),
                    CreateParameter("@pStartDate", SqlDbType.DateTime, StartDate),
                    CreateParameter("@pEndDate", SqlDbType.DateTime, EndDate),
                    CreateParameter("@pCallingTime", SqlDbType.BigInt, CallingTime),
                    CreateParameter("@pRecordingFileFormat", SqlDbType.NVarChar, RecordingFileFormat, 50),
                    CreateParameter("@pByUserId", SqlDbType.BigInt, ByUserID),
                    CreateParameter("@pReturnMaxID", SqlDbType.BigInt, -1, ParameterDirection.InputOutput));
                ReturnID = Int64.Parse(cmd.Parameters["@pReturnMaxID"].Value.ToString());
                cmd.Dispose();
            }
            catch (Exception exp)
            {
                System.Windows.Forms.MessageBox.Show(exp.Message);
            }
            

            return ReturnID;
        }

        public void CampaignList_Save(Int64 CampID, Int64 ListID,int Priority)
        {
            SqlCommand cmd;
            ExecuteNonQuery(out cmd, "spAECampaignCallingList",
                CreateParameter("@pCallingListID", SqlDbType.BigInt, ListID),
                CreateParameter("@pCampaignID", SqlDbType.BigInt, CampID),
                CreateParameter("@pPriority", SqlDbType.BigInt, Priority));
            cmd.Dispose();
        }

        public void CampaignTreatments_Save(Int64 CampID, Int64 TreatID)
        {
            SqlCommand cmd;
            ExecuteNonQuery(out cmd, "spAECampaignTreatment",
                CreateParameter("@pCampaignID", SqlDbType.BigInt, CampID),
                CreateParameter("@pTreatmentID", SqlDbType.BigInt, TreatID));
            cmd.Dispose();
        }

        public void CampaignGroup_Save(Int64 CampID, Int64 GroupID)
        {
            SqlCommand cmd;
            ExecuteNonQuery(out cmd, "spAECampaignGroup",
                CreateParameter("@pCampaignId", SqlDbType.BigInt, CampID),
                CreateParameter("@pGroupId", SqlDbType.BigInt, GroupID));
            cmd.Dispose();
        }

        public void CampaignDispoList_Save(Int64 CampID, Int64 DispListID)
        {
            SqlCommand cmd;
            ExecuteNonQuery(out cmd, "spAECampaignDespoList",
                CreateParameter("@pCampaignID", SqlDbType.BigInt, CampID),
                CreateParameter("@pDespositionListID", SqlDbType.BigInt, DispListID));
            cmd.Dispose();
        }
        //Calling the two stored procedures spDCampaign, spDCampaignJoinEntries respectively to delete the campaign record
        public void Campaign_Delete(Int64 ID)
        {
            ExecuteNonQuery("spDCampaign", CreateParameter("@pID", SqlDbType.BigInt, ID));
            ExecuteNonQuery("spDCampaignJoinEntries", CreateParameter("@pID", SqlDbType.BigInt, ID));
        }

    }
}
