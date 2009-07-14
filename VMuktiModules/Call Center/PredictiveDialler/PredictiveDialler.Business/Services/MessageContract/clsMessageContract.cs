using System;
using System.Collections.Generic;
//using System.Linq;
using System.Text;
using System.ServiceModel;
using System.IO;

namespace PredictiveDialler.Business.Services.MessageContract
{
    [MessageContract]
    public class clsMessageContract
    {
        [MessageHeader]
        public string uname;

        [MessageHeader]
        public string fName;

        [MessageHeader]
        public string fExtension;

        [MessageHeader]
        public Int64 fLength;

        [MessageBodyMember]
        public Stream fStream;

    }
}
