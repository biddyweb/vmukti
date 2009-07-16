using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using System.IO;

namespace Desktop_Sharing.Business.Service.MessageContract
{
    [MessageContract]
    public class clsMessageContract
    {
        [MessageHeader]
        public string strFrom;

        [MessageHeader]
        public string strTo;

        [MessageHeader]
        public string strType;

        [MessageHeader]
        public int mouseButton;

        [MessageHeader]
        public double x;

        [MessageHeader]
        public double y;

        [MessageHeader]
        public int key;

        [MessageHeader]
        public bool blView;

        [MessageHeader]
        public bool blControl;

        [MessageBodyMember]
        public Stream stremImage;

    }
}
