using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VMukti.StunFireWallDetector
{
    public class STUN_t_ChangeRequest
    {
        // Fields
        private bool m_ChangeIP;
        private bool m_ChangePort;

        // Methods
        public STUN_t_ChangeRequest()
        {
            this.m_ChangeIP = true;
            this.m_ChangePort = true;
        }

        public STUN_t_ChangeRequest(bool changeIP, bool changePort)
        {
            this.m_ChangeIP = true;
            this.m_ChangePort = true;
            this.m_ChangeIP = changeIP;
            this.m_ChangePort = changePort;
        }

        // Properties
        public bool ChangeIP
        {
            get
            {
                return this.m_ChangeIP;
            }
            set
            {
                this.m_ChangeIP = value;
            }
        }

        public bool ChangePort
        {
            get
            {
                return this.m_ChangePort;
            }
            set
            {
                this.m_ChangePort = value;
            }
        }
    }

 

}
