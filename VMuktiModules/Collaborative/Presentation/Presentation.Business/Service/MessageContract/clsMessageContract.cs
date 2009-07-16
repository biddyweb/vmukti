using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using System.IO;

namespace Presentation.Bal.Service.MessageContract
{
    [MessageContract]
    public class clsMessageContract
    {
        [MessageHeader]
        public string strMsg;

        [MessageHeader]
        public string strFrom;

        [MessageHeader]
        public List<string> lstTo;

        [MessageHeader]
        public int SlideID;

        [MessageHeader]
        public string[] slideArr;

        [MessageBodyMember]
        public Stream SlideStream;        
    }
}
