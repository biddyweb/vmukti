using System;
using System.Collections.Generic;
using System.Data;

namespace rptCRCbyAgent.Business
{
    public abstract class ClsBaseCollection<T> : List<T> where T : ClsBaseObject, new()
    {
        ////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// 
        /// </summary>
        /// <param name="ds"></param>
        /// <returns></returns>
        ////////////////////////////////////////////////////////////////////////////////////////////
        public bool MapObjects(DataSet ds)
        {
            if (ds != null && ds.Tables.Count > 0)
            {
                return MapObjects(ds.Tables[0]);
            }
            else
            {
                return false;
            }
        }

        ////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        ////////////////////////////////////////////////////////////////////////////////////////////
        public bool MapObjects(DataTable dt)
        {
            Clear();
            for (int i = 0; i<dt.Rows.Count; i++)
            {
                T obj = new T();
                obj.MapData(dt.Rows[i]);
                this.Add(obj);
            }
            return true;
        }        
    }
}
