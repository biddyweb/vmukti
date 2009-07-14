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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ImportLeads.Common;
using VMuktiAPI;
using ImportLeads.DataAccess ;
using System.Data;

namespace ImportLeads.Business
{
   public class ClsFilterType : ClsBaseObject 
    {
       //public static StringBuilder sb1;
       //public static StringBuilder CreateTressInfo()
       //{
       //    StringBuilder sb = new StringBuilder();
       //    sb.Append("User Is : " + VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName);
       //    sb.AppendLine();
       //    sb.Append("Peer Type is : " + VMuktiAPI.VMuktiInfo.CurrentPeer.CurrPeerType.ToString());
       //    sb.AppendLine();
       //    sb.Append("User's SuperNode is : " + VMuktiAPI.VMuktiInfo.CurrentPeer.SuperNodeIP);
       //    sb.AppendLine();
       //    sb.Append("User's Machine Ip Address : " + VMuktiAPI.GetIPAddress.ClsGetIP4Address.GetIP4Address());
       //    sb.AppendLine();
       //    sb.AppendLine("----------------------------------------------------------------------------------------");
       //    return sb;
       //}
       private Int64 _ID = ImportLeads.Common.ClsConstants.NullLong;
       private string _FilterName = ImportLeads.Common.ClsConstants.NullString;
       private string _Description = ImportLeads.Common.ClsConstants.NullString;
       private string _Type = ImportLeads.Common.ClsConstants.NullString;
       private Int64 _TreatmentCondID = ImportLeads.Common.ClsConstants.NullLong;
       private Int64 _FieldID = ImportLeads.Common.ClsConstants.NullLong;
       private string _Operator = ImportLeads.Common.ClsConstants.NullString;
       private string _FieldValues = ImportLeads.Common.ClsConstants.NullString;
       
       
       public Int64 ID
       {
         get { return  _ID  ;}
         set { _ID = value  ;}
       } 
       public string FilterName
       {
         get { return _FilterName   ;}
         set { _FilterName = value  ;}
       }
       public string Description
       {
          get {  return _Description; }
          set { _Description = value; }
       }
       public string Type
       {
         get{ return _Type; }
         set{ _Type = value;}
       }
       public Int64 TreatmentCondID
       {
         get{ return _TreatmentCondID;}
         set{ _TreatmentCondID = value;}
       }
       
       public  Int64 FieldId
       {
         get{ return _FieldID ;}
         set{ _FieldID = value;}
       }
       public string Operator
       {
          get{ return _Operator;}
          set{ _Operator = value;}
       }
       public string FieldValues
       {
       
         get{ return _FieldValues ;}
         set{ _FieldValues = value ;}
       } 
            
       public override bool MapData(DataRow row)
       {
           try
           {
               ID = GetLong(row, "ID");
               FilterName = GetString(row, "FilterName");
               Description = GetString(row,"Description");
               Type = GetString(row,"Type");
               TreatmentCondID = GetLong(row,"TreatmentCondID");
               FieldId = GetLong(row,"FieldId");
               Operator = GetString(row,"Operator");
               FieldValues = GetString(row,"FieldValues");
               
              return base.MapData(row);
           }
           catch (Exception ex)
           {
               VMuktiHelper.ExceptionHandler(ex, "MapData()", "ClsFilterType");
               return false;
           }
       }  
       
       }
       
       
    }

