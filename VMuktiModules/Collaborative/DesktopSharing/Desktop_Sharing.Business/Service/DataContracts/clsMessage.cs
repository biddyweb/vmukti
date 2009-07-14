using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.IO;

namespace Desktop_Sharing.Business.Service.DataContracts
{
    [DataContract]
    public class clsMessage
    {

        [DataMember]
        public string uName;

        //Wen some one controlling http user
        [DataMember]
        public bool isControlled;

        //wen http controlling some one

        [DataMember]
        public bool isControlling;

        [DataMember]
        public string strControlled;


    }
}
