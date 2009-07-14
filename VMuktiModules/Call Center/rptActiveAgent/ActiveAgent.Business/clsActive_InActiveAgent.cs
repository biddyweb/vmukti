using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
//using VMukti.Common;
using rptActiveAgent.Common;

namespace VMukti.Bussiness.InActiveAgent
{
    [DataContract]
    public class clsActive_InActiveAgent
    {
        [DataMember]
        public int UserID = ClsConstants.NullInt;

        [DataMember]
        public string UserName = ClsConstants.NullString;

        [DataMember]
        public string GroupName = ClsConstants.NullString;

        [DataMember]
        public string CampaignName = ClsConstants.NullString;

        [DataMember]
        public string RoleName = ClsConstants.NullString;

        [DataMember]
        public DateTime SessionStartTime = ClsConstants.NullDateTime;


        [DataMember]
        public long GroupID = ClsConstants.NullLong;

        [DataMember]
        public long CampaignID = ClsConstants.NullLong;

        [DataMember]
        public long ActivityID = ClsConstants.NullLong;


        [DataMember]
        public DateTime SessionEndTime = ClsConstants.NullDateTime;

        [DataMember]
        public long RoleID = ClsConstants.NullLong;

        //[DataMember]
        //public string strStatus = ClsConstants.NullString;



        //[DataMember]
        //public long lgCalls = ClsConstants.NullLong;

        //[DataMember]
        //public DateTime dtCurrentTime = ClsConstants.NullDateTime;

    }
}
