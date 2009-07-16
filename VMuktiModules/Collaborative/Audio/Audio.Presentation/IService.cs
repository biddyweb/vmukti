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
using System.ServiceModel;
using System.Text;
using System;
using VMuktiAPI;

namespace Audio.Presentation
{
    [ServiceContract]
    public interface IService
    {
        //[OperationContract(IsOneWay = false)]
        //string svcJoin(string uName);

        [OperationContract(IsOneWay = false)]
        string svcAddSIPUser();

        [OperationContract(IsOneWay = true)]
        void svcRemoveSIPUser(string strSIPNumber);

        [OperationContract(IsOneWay = false)]
        string svcGetConferenceNumber();

        //[OperationContract(IsOneWay = true)]
        //void svcStartConference(string uName, string strConfNumber, string[] GuestInfo);
    }

    public interface IServiceChannel : IClientChannel, IService
    { }

    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    public class clsService : IService
    {

        public delegate string DelsvcAddSIPUser();
        public delegate void DelsvcsvcRemoveSIPUser(string strSIPNumber);
        public delegate string DelsvcGetConferenceNumber();

        public event DelsvcAddSIPUser EntsvcAddSIPUser;
        public event DelsvcsvcRemoveSIPUser EntsvcRemoveSIPUser;
        public event DelsvcGetConferenceNumber EntsvcGetConferenceNumber;

        #region IService Members        

        string IService.svcAddSIPUser()
        {
            try
            {
                if (EntsvcAddSIPUser != null)
                {
                    return EntsvcAddSIPUser();
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "IService.svcAddSIPUser()", "Audio\\Iservice.cs");
                return null;
            }
        }

        void IService.svcRemoveSIPUser(string strSIPNumber)
        {
            try
            {
                if (EntsvcRemoveSIPUser != null)
                {
                    EntsvcRemoveSIPUser(strSIPNumber);
                }
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "Iservice.svcRemoveSIPUser()", "Audio\\Iservice.cs");
            }
        }

        string IService.svcGetConferenceNumber()
        {
            try
            {
            if (EntsvcGetConferenceNumber != null)
            {
                return EntsvcGetConferenceNumber();
            }
            else
            {
                return null;
            }
        }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "IService.svcGetConferenceNumber()", "Audio\\Iservice.cs");                
                return null;
            }
        }
        #endregion
    }
}
