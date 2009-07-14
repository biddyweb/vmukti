using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace CRM.DataAccess
{
    public class ClsDynamicScriptDataService : ClsDataServiceBase
    {

        public ClsDynamicScriptDataService() : base() { }

        ////////////////////////////////////////////////////////////////////////
        /// <summary>
        ///	Creates a new DataService and specifies a transaction with
        ///	which to operate
        /// </summary>
        ////////////////////////////////////////////////////////////////////////
        public ClsDynamicScriptDataService(IDbTransaction txn) : base(txn) { }


        public DataSet Options_GetAll(int QueID)
        {
            return ExecuteDataSet("Select ID,Options,ActionQueueID from QuestionOptions where QuestionID="+ QueID+";" , CommandType.Text, null);
        }

        public DataSet Answer_GetAll()
        {
            return null;
            //return ExecuteDataSet("Select ID,Options,ActionQueueID from QuestionOptions where QuestionID=" + QueID + ";", CommandType.Text, null);
        }

        public DataSet Questions_GetAll(int ScriptID)
        {
            return ExecuteDataSet("Select ID,QuestionName,QuestionText,Category from Question where ScriptID=" + ScriptID+";", CommandType.Text, null);
        }

        public DataSet Lead_GetAll(int LeadID, int LeadFieldID)
        {
            if (LeadFieldID == 1)
            {
                return ExecuteDataSet("Select Leads.ID,PhoneNo as PropertyValue from Leads where Leads.ID =" + LeadID + ";", CommandType.Text, null);
            }

            else if (LeadFieldID == 2)
            {
                return ExecuteDataSet("Select Leads.ID,StateName as PropertyValue from Leads,Location,State where Leads.LocationID = Location.ID and Location.StateID = State.ID and Leads.ID =" + LeadID + ";", CommandType.Text, null);
            }
            else if (LeadFieldID == 3)
            {
                return ExecuteDataSet("Select Leads.ID,ZipCode as PropertyValue from Leads,Location,Zipcode where Leads.LocationID = Location.ID and Location.ZipCodeID = Zipcode.ID and Leads.ID =" + LeadID + ";", CommandType.Text, null);
            }
            else
            {
                return ExecuteDataSet("Select ID,PropertyValue from LeadDetail where LeadID=" + LeadID + " and LeadFieldID=" + LeadFieldID + ";", CommandType.Text, null);
            }
        }


        //public void Lead_Save(Int64 LeadID, Int64 LeadFormatID, string Value)
        //{
        //    try
        //    {
        //        SqlCommand cmd;
        //        ExecuteNonQuery(out cmd, "spUpdateLead",
        //            CreateParameter("@pLeadId", SqlDbType.BigInt, LeadID),
        //            CreateParameter("@pLeadFieldID", SqlDbType.BigInt, LeadFormatID),
        //            CreateParameter("@pValue", SqlDbType.NVarChar, Value, 255));
        //        cmd.Dispose();
        //    }
        //    catch (Exception ex)
        //    {
                   
        //    }

        //}

        public int UpdateLeadInfo(int LeadID, int LeadFieldID, string LeadPropertyValue)
        {
            try
            {
                SqlCommand cmd;
                ExecuteNonQuery(out cmd, "spWSLeadDetail",
                    CreateParameter("@pLeadID", SqlDbType.Int, LeadID),
                    CreateParameter("@pLeadFieldName", SqlDbType.NVarChar, LeadFieldID.ToString()),
                    CreateParameter("@pPropertyValue", SqlDbType.NVarChar, LeadPropertyValue),
                    CreateParameter("@pReturnID", SqlDbType.BigInt, ParameterDirection.Output));

                int ID = int.Parse(cmd.Parameters["@pReturnID"].Value.ToString());
                cmd.Dispose();
                return ID;
            }
            catch (Exception ex)
            {
                return -3;
            }
        }

        public int UpdateLeadInfo(int LeadID, string StateName, string ZipCode)
        {

            try
            {
                SqlCommand cmd;
                ExecuteNonQuery(out cmd, "spUWSStateZip",
                    CreateParameter("@pLeadID", SqlDbType.Int, LeadID),
                    CreateParameter("@pStateName", SqlDbType.NVarChar, StateName),
                    CreateParameter("@pZipcode", SqlDbType.NVarChar, ZipCode),
                    CreateParameter("@pReturnID", SqlDbType.BigInt, ParameterDirection.Output));

                int ID = int.Parse(cmd.Parameters["@pReturnID"].Value.ToString());
                cmd.Dispose();
                return ID;
                ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                // ID Values                                                                                                      //
                //       > 0  - Successfully Stored                                                                               //
                //      -2  - Invalid zipcode                                                                                     //          
                //      -1  - Invalid State Name                                                                                  //          
                //                                                                                                                //
                ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            }
            catch (Exception ex)
            {
                //    MessageBox.Show(ex.Message);
                return -3;
            }

        }



        public void Answer_Save(int CallID,int QusOptionID)
        {
            SqlCommand cmd;
            ExecuteNonQuery(out cmd, "spAAnswer",
                CreateParameter("@pCallId", SqlDbType.BigInt, CallID),
                CreateParameter("@pQusOptionId", SqlDbType.BigInt, QusOptionID));
            cmd.Dispose();
        }

        //public DataSet User_GetByID(int ID)
        //{
        //    return ExecuteDataSet("spGUserInfoPayroll", CommandType.StoredProcedure, CreateParameter("@pID", SqlDbType.Int, ID));
        //}

        //public void User_Save(ref int ID, string displayame,int roleID,string firstName,string lastName,string eMail,string password,bool isActive,int byUserId,float ratePerHour,float overTimeRate,float doubleRatePerHour,float doubleOverTimeRate )
        //{
        //    SqlCommand cmd;
        //    ExecuteNonQuery(out cmd, "spAEUserInfoPayroll",
        //        CreateParameter("@pID", SqlDbType.Int, ID),
        //        CreateParameter("@pDisplayName", SqlDbType.NVarChar, displayame, 100),
        //        CreateParameter("@pRoleID", SqlDbType.BigInt, roleID),
        //        CreateParameter("@pFirstName", SqlDbType.NVarChar, firstName, 50),
        //        CreateParameter("@pLastName", SqlDbType.NVarChar, lastName, 50),
        //        CreateParameter("@pEMail", SqlDbType.NVarChar, eMail, 256),
        //        CreateParameter("@pPassword", SqlDbType.NVarChar, password, 50),
        //        CreateParameter("@pIsActive", SqlDbType.Bit, isActive),
        //        CreateParameter("@pByUserID", SqlDbType.BigInt, byUserId),
        //        CreateParameter("@pRatePerHour", SqlDbType.Float, ratePerHour),
        //        CreateParameter("@pOverTimeRate", SqlDbType.Float, overTimeRate),
        //        CreateParameter("@pDoubleRatePerHour", SqlDbType.Float, doubleRatePerHour),
        //        CreateParameter("@pDoubleOverTimeRate", SqlDbType.Float, doubleOverTimeRate));

        //    cmd.Dispose();
        //}

        //public void User_Delete(int ID)
        //{
        //    ExecuteNonQuery("spDUserInfoPayroll", CreateParameter("@pID", SqlDbType.Int, ID));
        //}

    }
}
