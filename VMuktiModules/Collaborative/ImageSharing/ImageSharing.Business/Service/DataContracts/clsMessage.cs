using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace ImageSharing.Business.Service.DataContracts
{
    [DataContract]
    public class clsMessage
    {
        [DataMember]
        public string uName;

        [DataMember]
        public int TotalByte;
        
        [DataMember]
        public byte[] ImgBlock;

        [DataMember]
        public bool LastBlock;

    }
}
