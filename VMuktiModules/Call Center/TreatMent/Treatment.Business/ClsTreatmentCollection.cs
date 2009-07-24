<<<<<<< HEAD:VMuktiModules/Call Center/TreatMent/Treatment.Business/ClsTreatmentCollection.cs
﻿/* VMukti 2.0 -- An Open Source Video Communications Suite
=======
﻿/* VMukti 2.0 -- An Open Source Unified Communications Engine
>>>>>>> b74131bacb80d82c79711cf70fe93af3fb611b40:VMuktiModules/Call Center/TreatMent/Treatment.Business/ClsTreatmentCollection.cs
*
* Copyright (C) 2008 - 2009, VMukti Solutions Pvt. Ltd.
*
* Hardik Sanghvi <hardik@vmukti.com>
*
* See http://www.vmukti.com for more information about
* the VMukti project. Please do not directly contact
* any of the maintainers of this project for assistance;
<<<<<<< HEAD:VMuktiModules/Call Center/TreatMent/Treatment.Business/ClsTreatmentCollection.cs
* the project provides a web site, forums and mailing lists      
=======
* the project provides a web site, forums and mailing lists
>>>>>>> b74131bacb80d82c79711cf70fe93af3fb611b40:VMuktiModules/Call Center/TreatMent/Treatment.Business/ClsTreatmentCollection.cs
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
<<<<<<< HEAD:VMuktiModules/Call Center/TreatMent/Treatment.Business/ClsTreatmentCollection.cs
 
=======


>>>>>>> b74131bacb80d82c79711cf70fe93af3fb611b40:VMuktiModules/Call Center/TreatMent/Treatment.Business/ClsTreatmentCollection.cs
*/
using System;
using Treatment.DataAccess;

namespace Treatment.Business
{
    public class ClsTreatmentCollection : ClsBaseCollection<ClsTreatment>
    {
        public static ClsTreatmentCollection GetAll()
        {
            ClsTreatmentCollection obj = new ClsTreatmentCollection();
            obj.MapObjects(new ClsTreatmentDataService().Treatment_GetAll());
            return obj;
        }

    }
}
