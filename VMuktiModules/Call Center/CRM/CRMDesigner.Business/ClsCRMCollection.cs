using System;
using System.Collections.Generic;
//using System.Linq;
using System.Text;
using CRMDesigner.DataAccess;

namespace CRMDesigner.Business
{
    public class ClsCRMCollection : ClsBaseCollection<ClsCRM>
    {
        public static ClsCRMCollection GetAll()
        {
            ClsCRMCollection obj = new ClsCRMCollection();
            obj.MapObjects(new ClsCRMDataService().CRM_GetAll());
            return obj;
        }
    }
}
