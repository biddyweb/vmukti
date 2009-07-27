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

namespace ScriptDesigner.DataAccess
{
    public class ClsQuestionAnsDataService : ClsDataServiceBase
    {

        public ClsQuestionAnsDataService() : base() { }

        ////////////////////////////////////////////////////////////////////////
        /// <summary>
        ///	Creates a new DataService and specifies a transaction with
        ///	which to operate
        /// </summary>
        ////////////////////////////////////////////////////////////////////////
        public ClsQuestionAnsDataService(IDbTransaction txn) : base(txn) { }

        public DataSet Campaign_GetAll()
        {
            return ExecuteDataSet("Select vCampaign.ID as ID,Name,IsActive,CampaignDespoList.DespositionListID as DispositionListID from vCampaign,CampaignDespolist where IsDeleted=0 and CampaignDespolist.CampaignID=vCampaign.ID;", CommandType.Text, null);
        }

        public DataSet Disposition_GetAll(Int64 DispListID)
        {
            return ExecuteDataSet("Select Disposition.ID as ID,Disposition.DespositionName as DespositionName from Disposition,DispListDisp where DispListDisp.DispositionID=Disposition.ID and DispListDisp.DispositionListID=" + DispListID.ToString() + ";", CommandType.Text, null);
        }
        
        public DataSet Script_GetAll()
        {
            return ExecuteDataSet("Select * from vScript where IsDeleted=0;", CommandType.Text, null);
        }

        public DataSet LeadFormat_GetAll()
        {
            return ExecuteDataSet("Select * from LeadFormat;", CommandType.Text, null);
        }

        public DataSet LeadFields_GetAll(Int64 FormtID)
        {
            return ExecuteDataSet("Select LeadCustomFields.ID as ID,LeadCustomFields.FieldName as LeadFormatName from LeadFields,LeadCustomFields where LeadFields.CustomFieldID=LeadCustomFields.ID and LeadFields.LeadFormatID=" + FormtID.ToString() + ";", CommandType.Text, null);
        }

        public DataSet Question_GetAll(int ScriptID)
        {
            return ExecuteDataSet("Select * from Question where ScriptID=" + ScriptID.ToString() + ";" , CommandType.Text, null);
        }

        public DataSet Options_GetAll(int QuesID)
        {
            return ExecuteDataSet("Select * from QuestionOptions where QuestionID=" + QuesID.ToString() + ";", CommandType.Text, null);
        }

        public DataSet Question_GetByID(int QueID)
        {
            return ExecuteDataSet("spGQuestion", CommandType.StoredProcedure, CreateParameter("@pID", SqlDbType.Int, QueID));
        }

        public int Question_Save(int ID, string Header,string Text,string Desc,string Category,int ScriptID)
        {
            SqlCommand cmd;
            ExecuteNonQuery(out cmd, "spAEQuestion",
                CreateParameter("@pID", SqlDbType.Int, ID),
                CreateParameter("@pQuestionName", SqlDbType.NVarChar, Header, 500),
                CreateParameter("@pQuestionText", SqlDbType.NVarChar, Text,555),
                CreateParameter("@pDescription", SqlDbType.NVarChar, Desc, 500),
                CreateParameter("@pCategory", SqlDbType.NVarChar, Category, 50),
                CreateParameter("@pScriptID", SqlDbType.BigInt, ScriptID),
                CreateParameter("@pReturnMaxId", SqlDbType.BigInt, -1,ParameterDirection.Output));

            cmd.Dispose();

            return (int.Parse(cmd.Parameters["@pReturnMaxId"].Value.ToString()));
        }

        public void Options_Delete(int QuestionID)
        {
            SqlCommand cmdDeleteJoin;
            ExecuteNonQuery(out cmdDeleteJoin, "spDQuestionOptions",
                CreateParameter("@pQuestionID", SqlDbType.Int, QuestionID));
            cmdDeleteJoin.Dispose();
        }

        public void Options_Save(int ID, string Option, string Description, int QuestionID, int ActionQuestionID)
        {
            if (ActionQuestionID == null)
            {
                ActionQuestionID = QuestionID;
            }
            
            SqlCommand cmd;
            ExecuteNonQuery(out cmd, "spAEQuestionOptions",
                CreateParameter("@pID", SqlDbType.Int, ID),
                CreateParameter("@pOptions", SqlDbType.NVarChar, Option, 100),
                CreateParameter("@pDescription", SqlDbType.NVarChar, Description, 100),
                CreateParameter("@pQuestionID", SqlDbType.BigInt, QuestionID),
                CreateParameter("@pActionQueueID", SqlDbType.BigInt,ActionQuestionID));
            cmd.Dispose();
        }

        public void Question_Delete(int ID)
        {
            ExecuteNonQuery("spDQuestionOptions", CreateParameter("@pQuestionID", SqlDbType.Int, ID));
            ExecuteNonQuery("spDQuestion", CreateParameter("@pID", SqlDbType.Int, ID));
        }

        public string GetScriptType(string ScriptName)
        {
            try
            {
                return ExecuteDataSet("select ST.Scripttype from ScriptType ST, Script S where ST.ID = S.ScriptTypeID and S.ScriptName ='" + ScriptName + "'", CommandType.Text, null).Tables[0].Rows[0].ItemArray[0].ToString();
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "GetScriptType", "ClQuestionAnsDataService.cs");
                return null;
            }

        }
    }
}
