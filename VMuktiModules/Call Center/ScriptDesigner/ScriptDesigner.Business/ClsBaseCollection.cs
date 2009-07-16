using System;
using System.Collections.Generic;
using System.Data;
using VMuktiAPI;

namespace ScriptDesigner.Business
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
            try
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
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "MapObjects", "ClsBaseCollection.cs");
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
            try
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    T obj = new T();
                    obj.MapData(dt.Rows[i]);
                    this.Add(obj);
                }
                return true;
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "MapObjects", "ClsBaseCollection.cs");
                return false;
            }
        }
        
    }
}
