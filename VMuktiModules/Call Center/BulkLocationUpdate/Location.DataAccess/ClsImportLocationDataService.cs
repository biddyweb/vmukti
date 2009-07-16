/* VMukti 2.0 -- An Open Source Video Communications Suite
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
using System.Data;
using VMuktiAPI;


namespace Location.DataAccess
{
    public class ClsImportLocationDataService : ClsDataServiceBase
    {
         public ClsImportLocationDataService() : base() { }


         public ClsImportLocationDataService(IDbTransaction txn) : base(txn) { }

         public DataSet GetTimeZone()
         {
             try
             {
                 DataSet ds = ExecuteDataSet("Select TimeZoneName from TimeZone", CommandType.Text, null);
                 return ds;
             }
             catch (Exception ex)
             {
                 VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "GetTimeZone()", "ClsImportLocationDataService.cs");
                 return null;
             }
         }

         public DataSet GetCountry()
         {
             try
             {
                 DataSet ds = ExecuteDataSet("Select Name from Country", CommandType.Text, null);
                 return ds;
             }
             catch (Exception ex)
             {
                 VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "GetCountry()", "ClsImportLocationDataService.cs");
                 return null;
             }
         }

         public DataSet GetCountry(string TimeZoneId)
         {
             try
             {
                 DataSet ds1 = ExecuteDataSet("Select distinct(CountryId) from Location where TimeZoneID=" + TimeZoneId, CommandType.Text, null);
                 DataSet ds = new DataSet();
                 for (int i = 0; i < ds1.Tables[0].Rows.Count; i++)
                 {
                     DataSet dsTemp = new DataSet();
                     dsTemp = ExecuteDataSet("Select Name from Country where ID=" + ds1.Tables[0].Rows[i][0].ToString(), CommandType.Text, null);
                     ds.Merge(dsTemp);
                 }
                 return ds;
             }
             catch (Exception ex)
             {
                 VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "GetCountry(string TimeZoneId)", "ClsImportLocationDataService.cs");
                 return null;
             }
         }

         public DataSet GetState()
         {
             try
             {
                 DataSet ds = ExecuteDataSet("Select StateName from State", CommandType.Text, null);
                 return ds;
             }
             catch (Exception ex)
             {
                 VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "GetState()", "ClsImportLocationDataService.cs");
                 return null;
             }

         }

         public DataSet GetState(string CountryId)
         {
             try
             {
                 DataSet ds1 = ExecuteDataSet("Select distinct(StateId) from Location where CountryID=" + CountryId, CommandType.Text, null);
                 DataSet ds = new DataSet();
                 for (int i = 0; i < ds1.Tables[0].Rows.Count; i++)
                 {
                     DataSet dsTemp = new DataSet();
                     dsTemp = ExecuteDataSet("Select StateName from State where ID=" + ds1.Tables[0].Rows[i][0].ToString(), CommandType.Text, null);
                     ds.Merge(dsTemp);
                 }
                 return ds;
             }
             catch (Exception ex)
             {
                 VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, " GetState(string CountryId)", "ClsImportLocationDataService.cs");
                 return null;
             }

         }
         public DataSet GetAreaCode(string Type,string strValue)
         {
             try
             {
                 if (Type == "TimeZone")
                 {
                     DataSet ds = ExecuteDataSet("Select AreaCode from Areacode where TimeZonename='" + strValue + "'", CommandType.Text, null);
                     return ds;
                 }
                 else
                 {
                     DataSet ds1 = ExecuteDataSet("Select distinct(AreaCodeID) from Location where StateID=" + strValue, CommandType.Text, null);
                     DataSet ds = new DataSet();
                     for (int i = 0; i < ds1.Tables[0].Rows.Count; i++)
                     {
                         DataSet dsTemp = new DataSet();
                         dsTemp = ExecuteDataSet("Select AreaCode from AreaCode where ID=" + ds1.Tables[0].Rows[i][0].ToString(), CommandType.Text, null);
                         ds.Merge(dsTemp);
                     }
                     return ds;
                 }
             }
             catch (Exception ex)
             {
                 VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "GetAreaCode", "ClsImportLocationDataService.cs");
                 return null;
             }
         }

         public DataSet GetZipCode(string StateId)
         {
             try
             {
                 //DataSet ds = ExecuteDataSet("Select ZipCode from Zipcode where StateID='" + StateId + "'", CommandType.Text, null);
                 //return ds;

                 DataSet ds1 = ExecuteDataSet("select Zip.Id,Loc.ZipcodeId from zipcode As Zip left join Location As Loc on Zip.Id = Loc.Id where Loc.zipcodeid is null and zip.StateId=" + StateId, CommandType.Text, null);
                 DataSet ds = new DataSet();
                 for (int i = 0; i < ds1.Tables[0].Rows.Count; i++)
                 {
                     DataSet dsTemp = new DataSet();
                     dsTemp = ExecuteDataSet("Select ZipCode from Zipcode where ID=" + ds1.Tables[0].Rows[i][0].ToString(), CommandType.Text, null);
                     ds.Merge(dsTemp);
                 }
                 return ds;
             }
             catch (Exception ex)
             {
                 VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "GetZipCode(string StateId)", "ClsImportLocationDataService.cs");
                 return null;
             }
         }

         public string GetTimeZoneId(string TimeZone)
         {
             try
             {
                 DataSet ds = ExecuteDataSet("Select ID from Timezone where TimeZoneName='" + TimeZone + "';", CommandType.Text, null);
                 return ds.Tables[0].Rows[0][0].ToString();
             }
             catch (Exception ex)
             {
                 VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "GetTimeZoneId()", "ClsImportLocationDataService.cs");
                 return null;
             }
         }

         public string GetCountryId(string CountryName)
         {
             try
             {
                 DataSet ds = ExecuteDataSet("Select ID from Country where Name='" + CountryName + "';", CommandType.Text, null);
                 return ds.Tables[0].Rows[0][0].ToString();
             }
             catch (Exception ex)
             {
                 VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "GetCountryId(string CountryName)", "ClsImportLocationDataService.cs");
                 return null;
             }
         }

         public string GetStateId(string StateName)
         {
             try
             {
                 DataSet ds = ExecuteDataSet("Select ID from State where StateName='" + StateName + "';", CommandType.Text, null);
                 return ds.Tables[0].Rows[0][0].ToString();
             }
             catch (Exception ex)
             {
                 VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "GetStateId()", "ClsImportLocationDataService.cs");
                 return null;
             }
         }

         public string GetAreacodeId(string AreaCode)
         {
             try
             {
                 DataSet ds = ExecuteDataSet("Select ID from Areacode where AreaCode='" + AreaCode + "';", CommandType.Text, null);
                 return ds.Tables[0].Rows[0][0].ToString();
             }
             catch (Exception ex)
             {
                 VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "GetAreacodeId(string AreaCode)()", "ClsImportLocationDataService.cs");
                 return null;
             }
         }

         public string GetZipId(string ZipCode)
         {
             try
             {

                 DataSet ds = ExecuteDataSet("Select ID from ZipCode where ZipCode='" + ZipCode + "';", CommandType.Text, null);
                 return ds.Tables[0].Rows[0][0].ToString();
             }
             catch (Exception ex)
             {
                 VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "GetZipId()()", "ClsImportLocationDataService.cs");
                 return null;
             }
         }


         public void InsertInToTimeZone(string TimeZoneName)
         {
             try
             {
                 DataSet ds = ExecuteDataSet("Select IsNull(Max(ID),0)+1 from TimeZone;", CommandType.Text, null);
                 ExecuteDataSet("Insert into TimeZone values(" + int.Parse(ds.Tables[0].Rows[0][0].ToString()) + ",'" + TimeZoneName + "','" + TimeZoneName + "',1,0,getdate(),1,getdate(),1)", CommandType.Text, null);

                 //DataSet DsGetLocationId = ExecuteDataSet("Select IsNull(Max(ID),0)+1 From Location;", CommandType.Text, null);
                 //ExecuteDataSet("Insert into Location values(" + int.Parse(DsGetLocationId.Tables[0].Rows[0][0].ToString()) + "," + int.Parse(ds.Tables[0].Rows[0][0].ToString()) + ",0,0,0,0)", CommandType.Text, null);
             }
             catch (Exception ex)
             {
                 VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "InsertInToTimeZone()", "ClsImportLocationDataService.cs");
             }
         }

         public void InsertIntoCountry(string strCountryName, string strCountryCode, string strDescription)
         {
             try
             {
                 DataSet ds = ExecuteDataSet("Select IsNull(Max(ID),0)+1 from Country;", CommandType.Text, null);
                 ExecuteDataSet("Insert into Country values(" + int.Parse(ds.Tables[0].Rows[0][0].ToString()) + ",'" + strCountryName + "','" + strCountryCode + "','" + strDescription + "',1,0,getdate(),1,getdate(),1)", CommandType.Text, null);
             }
             catch (Exception ex)
             {
                 VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "InsertIntoCountry()", "ClsImportLocationDataService.cs");
             }

         }

         public void InsertInToState(string strStateName, string strStateAbb)
         {
             try
             {
                 DataSet ds = ExecuteDataSet("Select IsNull(Max(ID),0)+1 From State;", CommandType.Text, null);
                 ExecuteDataSet("Insert into State values(" + int.Parse(ds.Tables[0].Rows[0][0].ToString()) + ",'" + strStateName + "','" + strStateAbb + "',null,null,1,0,getdate(),1,getdate(),1)", CommandType.Text, null);
             }
             catch (Exception ex)
             {
                 VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "InsertInToState()", "ClsImportLocationDataService.cs");
             }
                 

         }

         public void InsertIntoAreaCode(string AreaCode,string TimeZone)
         {
             try
             {
                 DataSet ds = ExecuteDataSet("Select IsNull(Max(ID),0)+1 From Areacode;", CommandType.Text, null);
                 ExecuteDataSet("Insert into Areacode values(" + int.Parse(ds.Tables[0].Rows[0][0].ToString()) + ",'" + AreaCode + "','" + TimeZone + "','" + TimeZone + "')", CommandType.Text, null);
             }
             catch (Exception ex)
             {
                 VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "InsertIntoAreaCode()", "ClsImportLocationDataService.cs");
             }
         }

         public void InsertInToLocationForTimeZone(string TimeZoneName)
         {
             //DataSet ds=ExecuteDataSet("
         }

         public int GetZipCodeCount(string ZipCode)
         {
             try
             {
                 DataSet ds = ExecuteDataSet("Select count(*) from Zipcode where ZipCode=" + ZipCode + ";", CommandType.Text, null);
                 return (int.Parse(ds.Tables[0].Rows[0][0].ToString()));
             }
             catch (Exception ex)
             {
                 VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "GetZipCodeCount()", "ClsImportLocationDataService.cs");

                 return -1;
             }
         }

         public int InsertZipCode(string ZipCode,string StateId)
         {
             try
             {
                 DataSet ds = ExecuteDataSet("Select IsNull(Max(ID),0)+1 from Zipcode;", CommandType.Text, null);
                 ExecuteDataSet("Insert into Zipcode values(" + int.Parse(ds.Tables[0].Rows[0][0].ToString()) + "," + ZipCode + "," + StateId + ",1,0,getdate(),1,getdate(),1)", CommandType.Text, null);
                 return int.Parse(ds.Tables[0].Rows[0][0].ToString());
             }
             catch (Exception ex)
             {
                 VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "InsertZipCode()", "ClsImportLocationDataService.cs");

                 return -1;
             }
         }

         public void InsertIntoLocation(Int64 TimeZoneId, Int64 CountryId, Int64 StateId, Int64 AreaCodeId, Int64 ZipCodeId)
         {
             try
             {
                 DataSet ds = ExecuteDataSet("Select IsNull(Max(ID),0)+1 from Location;", CommandType.Text, null);
                 ExecuteDataSet("Insert into Location values(" + int.Parse(ds.Tables[0].Rows[0][0].ToString()) + "," + TimeZoneId + "," + CountryId + "," + StateId + "," + AreaCodeId + "," + ZipCodeId + ")", CommandType.Text, null);
                 //return int.Parse(ds.Tables[0].Rows[0][0].ToString());
             }
             catch (Exception ex)
             {
                 VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "InsertIntoLocation()", "ClsImportLocationDataService.cs");

                 //return -1;
             }
         }
    }
}
