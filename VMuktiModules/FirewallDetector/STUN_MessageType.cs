
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VMukti.StunFireWallDetector
{
    public enum STUN_MessageType
    {
        BindingErrorResponse = 0x111,
        BindingRequest = 1,
        BindingResponse = 0x101,
        SharedSecretErrorResponse = 0x112,
        SharedSecretRequest = 2,
        SharedSecretResponse = 0x102
    }

 

 

}
