using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace ImageSharing.Business.Service.DataContracts
{
    
    [DataContract]
    public class ImageTrack
    {
        [DataMember]
        public string uName;

        [DataMember]
        public byte[] FullImgBlock;

        [DataMember]
        public int pointer;


    }
}
