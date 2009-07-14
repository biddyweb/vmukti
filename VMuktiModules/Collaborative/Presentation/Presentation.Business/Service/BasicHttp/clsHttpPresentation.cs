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
using System;
using System.Collections.Generic;
//using System.Linq;
using System.Text;
using VMuktiService;
using System.ServiceModel;
using System.IO;
using VMuktiAPI;
using Presentation.Bal.Service.MessageContract;

namespace Presentation.Bal
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    public class clsHttpPresentation : IHttpPresentation
    {
        public delegate void delsvcJoin(clsMessageContract mcJoin);        
        public delegate void delsvcSetSlideList(clsMessageContract mcSetSlideList);
        public delegate void delsvcSetSlide(clsMessageContract mcSetSlide);
        public delegate clsMessageContract delsvcGetSlide(clsMessageContract mcrecipient);
        public delegate void delsvcSetUserList(clsMessageContract mcSetUserList);
        public delegate void delsvcGetUserList(clsMessageContract mcGetUserList);        
        public delegate void delsvcSignOutPPT(clsMessageContract mcSignOutPPT);
        public delegate void delsvcUnJoin(clsMessageContract mcUnJoin);

        public delegate IAsyncResult delBeginsvcGetSlide(clsMessageContract mcSRecipient, AsyncCallback callback, object asyncState);
        public delegate clsMessageContract delEndsvcGetSlide(IAsyncResult result);
        
        public event delsvcJoin EntsvcJoin;
        public event delsvcSetSlideList EntsvcSetSlideList;
        public event delsvcSetSlide EntsvcSetSlide;
        public event delsvcGetSlide EntsvcGetSlide;
        public event delsvcSetUserList EntsvcSetUserList;
        public event delsvcGetUserList EntsvcGetUserList;
        public event delsvcSignOutPPT EntsvcSignOutPPT;
        public event delsvcUnJoin EntsvcUnJoin;

        public event delBeginsvcGetSlide EntBeginsvcGetSlide;
        public event delEndsvcGetSlide EntEndsvcGetSlide;

        #region IHttpPresentation Members

        public void svcJoin(clsMessageContract mcJoin)
        {
            try
            {
                if (EntsvcJoin != null)
                {
                    EntsvcJoin(mcJoin);
                }
            }
            catch (Exception exp)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(exp, "svcJoin()", "clsHttpPresentation.cs");
            }
        }

        public void svcSetSlideList(clsMessageContract mcSetSlideList)
        {
            try
            {
                if (EntsvcSetSlideList != null)
                {
                    EntsvcSetSlideList(mcSetSlideList);
                }
            }
            catch (Exception exp)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(exp, "svcSetSlideList()", "clsHttpPresentation.cs");
            }
        }

        public void svcSetSlide(clsMessageContract mcSetSlide)
        {
            try
            {
                if (EntsvcSetSlide != null)
                {
                    EntsvcSetSlide(mcSetSlide );
                }
            }
            catch (Exception exp)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(exp, "svcSetSlide()", "clsHttpPresentation.cs");
            }
        }

        public clsMessageContract svcGetSlide(clsMessageContract mcSRecipient)
        {
            try
            {
                if (EntsvcGetSlide != null)
                {
                    return EntsvcGetSlide(mcSRecipient);
                }
                else
                {
                    return null;
                }
            }
            catch (Exception exp)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(exp, "svcGetSlide()", "clsHttpPresentation.cs");
                return null;
            }
        }

        public void svcSetUserList(clsMessageContract mcSetUserList)
        {
            try
            {
                if (EntsvcSetUserList != null)
                {
                    EntsvcSetUserList(mcSetUserList);
                }
            }
            catch (Exception exp)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(exp, "svcSetUserList()", "clsHttpPresentation.cs");
            }
        }

        public void svcGetUserList(clsMessageContract mcGetUserList)
        {
            try
            {
                if (EntsvcGetUserList != null)
                {
                    EntsvcGetUserList(mcGetUserList);
                }
            }
            catch (Exception exp)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(exp, "svcGetUserList()", "clsHttpPresentation.cs");
            }
        }
        public void svcSignOutPPT(clsMessageContract mcSignOutPPT)
        {
            try
            {
                if (EntsvcSignOutPPT != null)
                {
                    EntsvcSignOutPPT(mcSignOutPPT);
                }
            }
            catch (Exception exp)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(exp, "svcSignOutPPT()", "clsHttpPresentation.cs");
            }
        }            

        public void svcUnJoin(clsMessageContract mcUnJoin)
        {
            try
            {
                if (EntsvcUnJoin != null)
                {
                    EntsvcUnJoin(mcUnJoin);
                }
            }
            catch (Exception exp)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(exp, "svcUnJoin()", "clsHttpPresentation.cs");   
            }
        }


        public IAsyncResult BeginsvcGetSlide(clsMessageContract mcSRecipient, AsyncCallback callback, object asyncState)
        {
            if (EntBeginsvcGetSlide != null)
            {
                return EntBeginsvcGetSlide(mcSRecipient, callback, asyncState);
            }
            else
            {
                return null;
            }
        }

        public clsMessageContract EndsvcGetSlide(IAsyncResult result)
        {
            if (EntEndsvcGetSlide != null)
            {
                return EntEndsvcGetSlide(result);
            }
            else
            {
                return null;
            }
        }

        #endregion

        
    }
}
