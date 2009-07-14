using System;
using System.Collections.Generic;
//using System.Linq;
using System.Text;
using System.Data;

namespace CRMDesigner.DataAccess
{
    public class ClsCRMDataService : ClsDataServiceBase
    {
        public ClsCRMDataService() : base() { }

        ////////////////////////////////////////////////////////////////////////
        /// <summary>
        ///	Creates a new DataService and specifies a transaction with
        ///	which to operate
        /// </summary>
        ////////////////////////////////////////////////////////////////////////
        public ClsCRMDataService(IDbTransaction txn) : base(txn) { }


        public DataSet CRM_GetAll()
        {
            return ExecuteDataSet("Select ID,CRMName from vCRM where IsDeleted=0;", CommandType.Text, null);
        }
    }
}
