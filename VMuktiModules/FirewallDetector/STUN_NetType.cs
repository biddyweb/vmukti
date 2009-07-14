using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VMukti.StunFireWallDetector
{

    public enum STUN_NetType
    {
        UdpBlocked,
        HttpPort80,
        NoInternetSupport,
        OpenInternet,
        SymmetricUdpFirewall,
        FullCone,
        RestrictedCone,
        PortRestrictedCone,
        Symmetric

    }
}
