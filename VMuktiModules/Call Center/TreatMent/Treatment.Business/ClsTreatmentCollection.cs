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
