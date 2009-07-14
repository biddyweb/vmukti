using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;

namespace VMukti.StunFireWallDetector
{
    public class STUN_Result
    {
        // Fields
        private STUN_NetType m_NetType = STUN_NetType.OpenInternet;
        private IPEndPoint m_pPublicEndPoint = null;

        // Methods
        public STUN_Result(STUN_NetType netType, IPEndPoint publicEndPoint)
        {
            this.m_NetType = netType;
            this.m_pPublicEndPoint = publicEndPoint;
        }

        // Properties
        public STUN_NetType NetType
        {
            get
            {
                return this.m_NetType;
            }
        }

        public IPEndPoint PublicEndPoint
        {
            get
            {
                return this.m_pPublicEndPoint;
            }
        }



    }
}
