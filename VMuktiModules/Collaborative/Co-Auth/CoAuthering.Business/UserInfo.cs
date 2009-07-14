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
namespace CoAuthering.Business
{
    public static class UserInfo
    {
        public static string MyRole = "";
        public static string UserName = "";

        public static string CreUserName = "1videoConference";
        public static string CrePassword = "resiprocket";
        public static string CreMachName = "";
    }

    public class CoAuthUser
    {
        public string UserName ;
       

        public CoAuthUser(string uName)
        {
            UserName = uName;
          
        }
    }
    /// <summary>
    /// This static class will be used on the "Host" side..for keeping track of what data is to be write, is currently anybody is writing on it or not.
    /// </summary>
    public static class MyUsers
    {
        public static System.Collections.Generic.List<CoAuthUser> Users = new System.Collections.Generic.List<CoAuthUser>();
        public static byte[] MyCompedData;
        public static int pointer;
        public static bool flgWriting ;
    }
}