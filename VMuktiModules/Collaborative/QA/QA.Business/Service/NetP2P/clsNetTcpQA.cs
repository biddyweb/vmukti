/* VMukti 1.0 -- An Open Source Unified Communications Engine
*
* Copyright (C) 2008 - 2009, VMukti Solutions Pvt. Ltd.
*
* Hardik Sanghvi <hardik@vmukti.com>
*
* See http://www.vmukti.com for more information about
* the VMukti project. Please do not directly contact
* any of the maintainers of this project for assistance;
* the project provides a web site, forums and mailing lists
* for your use.

* This program is free software: you can redistribute it and/or modify
* it under the terms of the GNU General Public License as published by
* the Free Software Foundation, either version 3 of the License, or
* (at your option) any later version.

* This program is distributed in the hope that it will be useful,
* but WITHOUT ANY WARRANTY; without even the implied warranty of
* MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
* GNU General Public License for more details.

* You should have received a copy of the GNU General Public License
* along with this program. If not, see <http://www.gnu.org/licenses/>

*/
using System.Collections.Generic;

namespace QA.Business.Service.NetP2P
{
    public class clsNetTcpQA:INetTcpQA
    {
        public delegate void DelsvcP2Pjoin(string uName);
        public delegate void DelsvcP2PAskQuestion(string uName, string Question, string Role);
        public delegate void DelsvcP2PReplyQuestion(string uName, string Question, string Answer, string Role, List<string> strBuddyList);

        public event DelsvcP2Pjoin EntsvcP2PJoin;
        public event DelsvcP2PAskQuestion EntsvcP2PAskQuestion;
        public event DelsvcP2PReplyQuestion entsvcP2PReplyQuestion;

        public void svcP2PJoin(string uName)
        {
            if (EntsvcP2PJoin != null)
            {
                EntsvcP2PJoin(uName);
            }
        }

        public void svcP2PAskQuestion(string uName, string Question, string Role)
        {
            if (EntsvcP2PAskQuestion != null)
            {
                EntsvcP2PAskQuestion(uName, Question, Role);
            }

        }

        public void svcP2PReplyQuestion(string uName, string Question, string Answer, string Role,List<string> strBuddyList)
        {
            if(entsvcP2PReplyQuestion!=null)
            {
                entsvcP2PReplyQuestion(uName, Question, Answer, Role, strBuddyList);
            }
        }

    }
}
