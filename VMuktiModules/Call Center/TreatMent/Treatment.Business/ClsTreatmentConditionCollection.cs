<<<<<<< HEAD:VMuktiModules/Call Center/TreatMent/Treatment.Business/ClsTreatmentConditionCollection.cs
﻿/* VMukti 2.0 -- An Open Source Video Communications Suite
=======
﻿/* VMukti 2.0 -- An Open Source Unified Communications Engine
>>>>>>> b74131bacb80d82c79711cf70fe93af3fb611b40:VMuktiModules/Call Center/TreatMent/Treatment.Business/ClsTreatmentConditionCollection.cs
*
* Copyright (C) 2008 - 2009, VMukti Solutions Pvt. Ltd.
*
* Hardik Sanghvi <hardik@vmukti.com>
*
* See http://www.vmukti.com for more information about
* the VMukti project. Please do not directly contact
* any of the maintainers of this project for assistance;
<<<<<<< HEAD:VMuktiModules/Call Center/TreatMent/Treatment.Business/ClsTreatmentConditionCollection.cs
* the project provides a web site, forums and mailing lists      
=======
* the project provides a web site, forums and mailing lists
>>>>>>> b74131bacb80d82c79711cf70fe93af3fb611b40:VMuktiModules/Call Center/TreatMent/Treatment.Business/ClsTreatmentConditionCollection.cs
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
<<<<<<< HEAD:VMuktiModules/Call Center/TreatMent/Treatment.Business/ClsTreatmentConditionCollection.cs
 
=======


>>>>>>> b74131bacb80d82c79711cf70fe93af3fb611b40:VMuktiModules/Call Center/TreatMent/Treatment.Business/ClsTreatmentConditionCollection.cs
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Treatment.DataAccess;

namespace Treatment.Business
{
    public class ClsTreatmentConditionCollection : ClsBaseCollection<ClsTreatmentCondition>
    {

        public static ClsTreatmentConditionCollection GetAll(int TreatmentID)
        {
            ClsTreatmentConditionCollection obj = new ClsTreatmentConditionCollection();
            obj.MapObjects(new ClsTreatmentConditionDataService().TreatmentCondition_GetByTreatmentID(TreatmentID));
            return obj;
        }

        
    }
}
