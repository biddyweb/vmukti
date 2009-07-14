using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VMukti.StunFireWallDetector
{
    public class STUN_t_ErrorCode
    {
        // Fields
        private int m_Code = 0;
        private string m_ReasonText = "";

        // Methods
        public STUN_t_ErrorCode(int code, string reasonText)
        {
            this.m_Code = code;
            this.m_ReasonText = reasonText;
        }

        // Properties
        public int Code
        {
            get
            {
                return this.m_Code;
            }
            set
            {
                this.m_Code = value;
            }
        }

        public string ReasonText
        {
            get
            {
                return this.m_ReasonText;
            }
            set
            {
                this.m_ReasonText = value;
            }
        }
    }

 

}
