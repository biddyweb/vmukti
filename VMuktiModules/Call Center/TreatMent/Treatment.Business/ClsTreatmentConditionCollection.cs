/* VMukti 2.0 -- An Open Source Unified Communications Engine
>>>>>>> b74131bacb80d82c79711cf70fe93af3fb611b40:VMuktiModules/Call Center/UserInfo/User.Application/CtlUserInfo.xaml.cs
*
* Copyright (C) 2008 - 2009, VMukti Solutions Pvt. Ltd.
*
* Hardik Sanghvi <hardik@vmukti.com>
*
* See http://www.vmukti.com for more information about
* the VMukti project. Please do not directly contact
* any of the maintainers of this project for assistance;

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
