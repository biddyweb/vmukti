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
using PredictiveDialler.DataAccess;
using VMuktiAPI;

namespace PredictiveDialler.Business
{
    public class ClsLeadCollection : ClsBaseCollection<ClsLead>
    {
        public static ClsLeadCollection GetAll(long userid, out long campaingID, ClsUserDataService clsDataService)//,int totalRecordToFetch)
        {
            try
            {
                ClsLeadCollection obj = new ClsLeadCollection();
                //obj.MapObjects(new ClsUserDataService().GetLeadsList(userid, out campaingID));//,totalRecordToFetch));
                //System.Data.DataSet dt= clsDataService.GetLeadsList(userid,out campaingID);
                //obj.MapObjects(dt.Tables[0]);
                //obj.MapObjects(dt.Tables[1]);
                obj.MapObjects(clsDataService.GetLeadsList(userid,out campaingID));
                return obj;
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "GetAll()", "ClsLeadCollection.cs");
                campaingID = 0;
                return null;
            }
        }

    }
}
