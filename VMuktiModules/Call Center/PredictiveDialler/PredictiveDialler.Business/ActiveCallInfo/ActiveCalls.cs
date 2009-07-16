using System;
using System.Collections.Generic;
//using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using PredictiveDialler.Common;

namespace VMukti.Bussiness.CommonDataContracts
{
    [DataContract]
    public class ActiveCalls
    {

        [DataMember]
        public long CurrentCampID = ClsConstants.NullLong;

        [DataMember]
        public long CurrentLeadID = ClsConstants.NullLong;

        [DataMember]
        public long CurrentPhoneNo = ClsConstants.NullLong;

        [DataMember]
        public DateTime CurrentCallStartTime = ClsConstants.NullDateTime;

        [DataMember]
        public string CurrentCallState = ClsConstants.NullString;
    }

}
