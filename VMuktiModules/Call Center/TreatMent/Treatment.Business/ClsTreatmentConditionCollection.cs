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
