/* VMukti 2.0 -- An Open Source Video Communications Suite
*
* Copyright (C) 2008 - 2009, VMukti Solutions Pvt. Ltd.
*
* Hardik Sanghvi <hardik@vmukti.com>
*
* See http://www.vmukti.com for more information about
* the VMukti project. Please do not directly contact
* any of the maintainers of this project for assistance;
<<<<<<< HEAD:VMuktiModules/Call Center/UserInfo/User.Business/ClsUserCollection.cs
* the project provides a web site, forums and mailing lists      
=======
* the project provides a web site, forums and mailing lists
>>>>>>> b74131bacb80d82c79711cf70fe93af3fb611b40:VMuktiModules/Call Center/UserInfo/User.Business/ClsUserCollection.cs
* for your use.

* This program is free software: you can redistribute it and/or modify
* it under the terms of the GNU General Public License as published by
<<<<<<< HEAD:VMuktiModules/Call Center/UserInfo/User.Business/ClsUserCollection.cs
* the Free Software Foundation, either version 3 of the License, or
=======
* the Free Software Foundation, either version 2 of the License, or
>>>>>>> b74131bacb80d82c79711cf70fe93af3fb611b40:VMuktiModules/Call Center/UserInfo/User.Business/ClsUserCollection.cs
* (at your option) any later version.

* This program is distributed in the hope that it will be useful,
* but WITHOUT ANY WARRANTY; without even the implied warranty of
* MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
* GNU General Public License for more details.

* You should have received a copy of the GNU General Public License
* along with this program. If not, see <http://www.gnu.org/licenses/>
<<<<<<< HEAD:VMuktiModules/Call Center/UserInfo/User.Business/ClsUserCollection.cs
 
=======

>>>>>>> b74131bacb80d82c79711cf70fe93af3fb611b40:VMuktiModules/Call Center/UserInfo/User.Business/ClsUserCollection.cs
*/
using System;
using User.DataAccess;
using System.Text;
using VMuktiAPI;

namespace User.Business
{
    public class UserCollection : ClsBaseCollection<ClsUser>
    {
        
        public static UserCollection GetAll()
        {
            try
            {
                UserCollection obj = new UserCollection();
                obj.MapObjects(new ClsUserDataService().User_GetAll());
                return obj;
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "GetAll", "ClsUserCollection.cs");
                return null;
            }
        }

    }
}
