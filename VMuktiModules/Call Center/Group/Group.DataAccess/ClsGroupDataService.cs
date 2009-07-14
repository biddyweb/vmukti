using System;
using System.Data;
using System.Data.SqlClient;
using Group.Common;
using VMuktiAPI;

/// <summary>
/// Summary description for DemoDataService
/// </summary>

namespace Group.DataAccess
{

    public class ClsGroupDataService : ClsDataServiceBase
    {
        ////////////////////////////////////////////////////////////////////////
        /// <summary>
        ///	Creates a new DataService
        /// </summary>
        ////////////////////////////////////////////////////////////////////////
        public ClsGroupDataService() : base() { }

        ////////////////////////////////////////////////////////////////////////
        /// <summary>
        ///	Creates a new DataService and specifies a transaction with
        ///	which to operate
        /// </summary>
        ////////////////////////////////////////////////////////////////////////
        public ClsGroupDataService(IDbTransaction txn) : base(txn) { }


        public DataSet Group_GetAll()
        {
            try
            {
                return ExecuteDataSet("Select * from vGroup;", CommandType.Text, null);
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "Group_GetAll", "ClsGroupDataService.cs");
                return null;
            }
            
        }


        public DataSet Group_GetByID(int ID)
        {
            try
            {
                return ExecuteDataSet("spGGroup", CommandType.StoredProcedure, CreateParameter("@pID", SqlDbType.Int, ID));
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "Group_GetByID", "ClsGroupDataService.cs");
                return null;
            }
        }

        public int Group_Save(ref int ID, string GroupName, bool IsActive, int CreatedBy, string Description)
        {
            try
            {
                SqlCommand cmd;
                ExecuteNonQuery(out cmd, "spAEGroup",
                    CreateParameter("@pID", SqlDbType.Int, ID, ParameterDirection.Input),
                    CreateParameter("@pGroupName", SqlDbType.NVarChar, GroupName),
                    CreateParameter("@pIsActive", SqlDbType.Bit, IsActive),
                    CreateParameter("@pDescription", SqlDbType.NVarChar, Description),
                    CreateParameter("@pByUserId", SqlDbType.BigInt, CreatedBy),
                    CreateParameter("@pReturnId", SqlDbType.BigInt, ParameterDirection.Output));

                ID = int.Parse(cmd.Parameters["@pReturnId"].Value.ToString());


                cmd.Dispose();
                return ID;
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "Group_Save", "ClsGroupDataService.cs");
                return 0;
            }
        }

        
        public void Group_Delete(int ID)
        {
            try
            {
                //Delete data from Group as well as UserGroup
                ExecuteNonQuery("spDGroup", CreateParameter("@pID", SqlDbType.Int, ID));
                ExecuteNonQuery("spDUserGroup", CreateParameter("@pID", SqlDbType.Int, ID));
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "Group_Delete", "ClsGroupDataService.cs");
            }
        }

    } //class

} //namespace