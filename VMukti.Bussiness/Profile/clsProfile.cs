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
using System.Text;
using System.Data;
using VMukti.Business.CommonDataContracts;
using VMuktiAPI;

namespace VMukti.Business
{
    public class clsProfile
    {        
      

        public static List<object> GetAllCountries()
        {
            try
            {
                List<object> objList = new List<object>();
                DataSet ds = null;

                if (bool.Parse(VMuktiAPI.VMuktiInfo.Port80) && VMuktiAPI.VMuktiInfo.CurrentPeer.CurrPeerType != PeerType.BootStrap)
                {
                    ds = clsDataBaseChannel.chHttpDataBaseService.svcExecuteDataSet("Select ID,CountryName from vCountryUserProfile order by CountryName").dsInfo;
                }
                else
                {
                if (VMuktiAPI.VMuktiInfo.CurrentPeer.CurrPeerType == VMuktiAPI.PeerType.NodeWithHttp)
                {
                    ds = clsDataBaseChannel.chHttpDataBaseService.svcExecuteDataSet("Select ID,CountryName from vCountryUserProfile order by CountryName").dsInfo;
                }
                else
                {
                    ds = new VMukti.DataAccess.clsProfile().GetCountryUserProfile();
                }
                }
                if (ds.Tables[0] != null)
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        string CountryName;
                        CountryName =  (string)dr["CountryName"];
                        objList.Add(CountryName);
                    }
                }


                return objList;

            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "GetAllCountries()", "clsProfile.cs");
                return null;
            }
        }

        public static List<object> GetAllLanguages()
        {
            try
            {
                List<object> objList = new List<object>();
                DataSet ds = null;

                if (bool.Parse(VMuktiAPI.VMuktiInfo.Port80) && VMuktiAPI.VMuktiInfo.CurrentPeer.CurrPeerType != PeerType.BootStrap)
                {
                    ds = clsDataBaseChannel.chHttpDataBaseService.svcExecuteDataSet("Select ID,Language from vLanguage order by Language").dsInfo;
                }
                else
                {
                if (VMuktiAPI.VMuktiInfo.CurrentPeer.CurrPeerType == VMuktiAPI.PeerType.NodeWithHttp)
                {
                    ds = clsDataBaseChannel.chHttpDataBaseService.svcExecuteDataSet("Select ID,Language from vLanguage order by Language").dsInfo;
                }
                else
                {
                    ds = new VMukti.DataAccess.clsProfile().GetLanguages();
                }
                }
                if (ds.Tables[0] != null)
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        string languages;
                        languages = (string)dr["Language"];
                        objList.Add(languages);
                    }
                }

                return objList;
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "GetAllCountries()", "clsProfile.cs");               
                return null;
            }
        }

        public static List<object> GetAllTimezones()
        {
            try
            {
                List<object> objList = new List<object>();
                
                DataSet ds = null;

                if (bool.Parse(VMuktiAPI.VMuktiInfo.Port80) && VMuktiAPI.VMuktiInfo.CurrentPeer.CurrPeerType != PeerType.BootStrap)
                {
                    ds = clsDataBaseChannel.chHttpDataBaseService.svcExecuteDataSet("Select ID,TimeZoneName from vTimezone order by TimeZoneName").dsInfo;
                }
                else
                {

                if (VMuktiAPI.VMuktiInfo.CurrentPeer.CurrPeerType == VMuktiAPI.PeerType.NodeWithHttp)
                {
                    ds = clsDataBaseChannel.chHttpDataBaseService.svcExecuteDataSet("Select ID,TimeZoneName from vTimezone order by TimeZoneName").dsInfo;
                }
                else
                {
                    ds = new VMukti.DataAccess.clsProfile().GetTimezones();
                }
                }
                if (ds.Tables[0] != null)
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        string timezone;
                        timezone = (string)dr["TimeZoneName"];
                        objList.Add(timezone);
                    }
                }

                return objList;
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "GetAllCountries()", "clsProfile.cs");
                return null;
            }
        }

        public static void UpdateUserProfile(int userID, string fullName, string email, string country, string state, string city, string timezone, string language, string gender, DateTime birthDate, string homepage, string aboutMe, string homePhone, string officePhone, string mobilePhone)
        {
            try
            {

                if (bool.Parse(VMuktiAPI.VMuktiInfo.Port80) && VMuktiAPI.VMuktiInfo.CurrentPeer.CurrPeerType != PeerType.BootStrap)
                {
                    List<clsSqlParametersInfo> lstSP = new List<clsSqlParametersInfo>();

                    clsSqlParametersInfo objInfo = new clsSqlParametersInfo();
                    objInfo.Direction = "Input";
                    objInfo.PName = "@pUserID";
                    objInfo.PValue = userID;
                    objInfo.PDBType = "BigInt";
                    objInfo.PSize = 200;
                    lstSP.Add(objInfo);

                    clsSqlParametersInfo objInfo1 = new clsSqlParametersInfo();
                    objInfo1.Direction = "Input";
                    objInfo1.PName = "@pFullName";
                    objInfo1.PValue = fullName;
                    objInfo1.PDBType = "NVarChar";
                    objInfo1.PSize = 200;
                    lstSP.Add(objInfo1);

                    clsSqlParametersInfo objInfo2 = new clsSqlParametersInfo();
                    objInfo2.Direction = "Input";
                    objInfo2.PName = "@pEmail";
                    objInfo2.PValue = email;
                    objInfo2.PDBType = "NVarChar";
                    objInfo2.PSize = 200;
                    lstSP.Add(objInfo2);

                    clsSqlParametersInfo objInfo3 = new clsSqlParametersInfo();
                    objInfo3.Direction = "Input";
                    objInfo3.PName = "@pCountry";
                    objInfo3.PValue = country;
                    objInfo3.PDBType = "NVarChar";
                    objInfo3.PSize = 200;
                    lstSP.Add(objInfo3);

                    clsSqlParametersInfo objInfo4 = new clsSqlParametersInfo();
                    objInfo4.Direction = "Input";
                    objInfo4.PName = "@pState";
                    objInfo4.PValue = state;
                    objInfo4.PDBType = "NVarChar";
                    objInfo4.PSize = 200;
                    lstSP.Add(objInfo4);

                    clsSqlParametersInfo objInfo5 = new clsSqlParametersInfo();
                    objInfo5.Direction = "Input";
                    objInfo5.PName = "@pCity";
                    objInfo5.PValue = city;
                    objInfo5.PDBType = "NVarChar";
                    objInfo5.PSize = 200;
                    lstSP.Add(objInfo5);

                    clsSqlParametersInfo objInfo6 = new clsSqlParametersInfo();
                    objInfo6.Direction = "Input";
                    objInfo6.PName = "@pTimezone";
                    objInfo6.PValue = timezone;
                    objInfo6.PDBType = "NVarChar";
                    objInfo6.PSize = 200;
                    lstSP.Add(objInfo6);

                    clsSqlParametersInfo objInfo7 = new clsSqlParametersInfo();
                    objInfo7.Direction = "Input";
                    objInfo7.PName = "@pLanguage";
                    objInfo7.PValue = language;
                    objInfo7.PDBType = "NVarChar";
                    objInfo7.PSize = 200;
                    lstSP.Add(objInfo7);

                    clsSqlParametersInfo objInfo8 = new clsSqlParametersInfo();
                    objInfo8.Direction = "Input";
                    objInfo8.PName = "@pGender";
                    objInfo8.PValue = gender;
                    objInfo8.PDBType = "NVarChar";
                    objInfo8.PSize = 200;
                    lstSP.Add(objInfo8);

                    clsSqlParametersInfo objInfo9 = new clsSqlParametersInfo();
                    objInfo9.Direction = "Input";
                    objInfo9.PName = "@pBirthDate";
                    objInfo9.PValue = birthDate;
                    objInfo9.PDBType = "DateTime";
                    objInfo9.PSize = 200;
                    lstSP.Add(objInfo9);

                    clsSqlParametersInfo objInfo10 = new clsSqlParametersInfo();
                    objInfo10.Direction = "Input";
                    objInfo10.PName = "@pHomePage";
                    objInfo10.PValue = homepage;
                    objInfo10.PDBType = "nText";
                    objInfo10.PSize = 200;
                    lstSP.Add(objInfo10);

                    clsSqlParametersInfo objInfo11 = new clsSqlParametersInfo();
                    objInfo11.Direction = "Input";
                    objInfo11.PName = "@pAboutMe";
                    objInfo11.PValue = aboutMe;
                    objInfo11.PDBType = "NText";
                    objInfo11.PSize = 200;
                    lstSP.Add(objInfo11);

                    clsSqlParametersInfo objInfo12 = new clsSqlParametersInfo();
                    objInfo12.Direction = "Input";
                    objInfo12.PName = "@pHomePhone";
                    objInfo12.PValue = homePhone;
                    objInfo12.PDBType = "NVarChar";
                    objInfo12.PSize = 200;
                    lstSP.Add(objInfo12);

                    clsSqlParametersInfo objInfo13 = new clsSqlParametersInfo();
                    objInfo13.Direction = "Input";
                    objInfo13.PName = "@pOfficePhone";
                    objInfo13.PValue = officePhone;
                    objInfo13.PDBType = "NVarChar";
                    objInfo13.PSize = 200;
                    lstSP.Add(objInfo13);

                    clsSqlParametersInfo objInfo14 = new clsSqlParametersInfo();
                    objInfo14.Direction = "Input";
                    objInfo14.PName = "@pMobilePhone";
                    objInfo14.PValue = mobilePhone;
                    objInfo14.PDBType = "NVarChar";
                    objInfo14.PSize = 200;
                    lstSP.Add(objInfo14);

                    clsSqlParameterContract CSqlInfo = new clsSqlParameterContract();
                    CSqlInfo.objParam = lstSP;

                    DataSet ds = VMukti.Business.clsDataBaseChannel.chHttpDataBaseService.svcExecuteDataSet("spAEUserProfile", CSqlInfo).dsInfo;
                    
                }
                else
                {

                if (VMuktiAPI.VMuktiInfo.CurrentPeer.CurrPeerType == VMuktiAPI.PeerType.NodeWithHttp)
                {
                    List<clsSqlParametersInfo> lstSP = new List<clsSqlParametersInfo>();

                    clsSqlParametersInfo objInfo = new clsSqlParametersInfo();
                    objInfo.Direction = "Input";
                    objInfo.PName = "@pUserID";
                    objInfo.PValue = userID;
                    objInfo.PDBType = "BigInt";
                    objInfo.PSize = 200;
                    lstSP.Add(objInfo);

                    clsSqlParametersInfo objInfo1 = new clsSqlParametersInfo();
                    objInfo1.Direction = "Input";
                    objInfo1.PName = "@pFullName";
                    objInfo1.PValue = fullName;
                    objInfo1.PDBType = "NVarChar";
                    objInfo1.PSize = 200;
                    lstSP.Add(objInfo1);
                    
                    clsSqlParametersInfo objInfo2 = new clsSqlParametersInfo();
                    objInfo2.Direction = "Input";
                    objInfo2.PName = "@pEmail";
                    objInfo2.PValue = email;
                    objInfo2.PDBType = "NVarChar";
                    objInfo2.PSize = 200;
                    lstSP.Add(objInfo2);

                    clsSqlParametersInfo objInfo3 = new clsSqlParametersInfo();
                    objInfo3.Direction = "Input";
                    objInfo3.PName = "@pCountry";
                    objInfo3.PValue = country;
                    objInfo3.PDBType = "NVarChar";
                    objInfo3.PSize = 200;
                    lstSP.Add(objInfo3);
                 
                    clsSqlParametersInfo objInfo4 = new clsSqlParametersInfo();
                    objInfo4.Direction = "Input";
                    objInfo4.PName = "@pState";
                    objInfo4.PValue = state;
                    objInfo4.PDBType = "NVarChar";
                    objInfo4.PSize = 200;
                    lstSP.Add(objInfo4);
                   
                    clsSqlParametersInfo objInfo5 = new clsSqlParametersInfo();
                    objInfo5.Direction = "Input";
                    objInfo5.PName = "@pCity";
                    objInfo5.PValue = city;
                    objInfo5.PDBType = "NVarChar";
                    objInfo5.PSize = 200;
                    lstSP.Add(objInfo5);
                  
                    clsSqlParametersInfo objInfo6 = new clsSqlParametersInfo();
                    objInfo6.Direction = "Input";
                    objInfo6.PName = "@pTimezone";
                    objInfo6.PValue = timezone;
                    objInfo6.PDBType = "NVarChar";
                    objInfo6.PSize = 200;
                    lstSP.Add(objInfo6);
                  
                    clsSqlParametersInfo objInfo7 = new clsSqlParametersInfo();
                    objInfo7.Direction = "Input";
                    objInfo7.PName = "@pLanguage";
                    objInfo7.PValue = language;
                    objInfo7.PDBType = "NVarChar";
                    objInfo7.PSize = 200;
                    lstSP.Add(objInfo7);
                  
                    clsSqlParametersInfo objInfo8 = new clsSqlParametersInfo();
                    objInfo8.Direction = "Input";
                    objInfo8.PName = "@pGender";
                    objInfo8.PValue = gender;
                    objInfo8.PDBType = "NVarChar";
                    objInfo8.PSize = 200;
                    lstSP.Add(objInfo8);
                 
                    clsSqlParametersInfo objInfo9 = new clsSqlParametersInfo();
                    objInfo9.Direction = "Input";
                    objInfo9.PName = "@pBirthDate";
                    objInfo9.PValue = birthDate;
                    objInfo9.PDBType = "DateTime";
                    objInfo9.PSize = 200;
                    lstSP.Add(objInfo9);
                
                    clsSqlParametersInfo objInfo10 = new clsSqlParametersInfo();
                    objInfo10.Direction = "Input";
                    objInfo10.PName = "@pHomePage";
                    objInfo10.PValue = homepage;
                    objInfo10.PDBType = "nText";
                    objInfo10.PSize = 200;
                    lstSP.Add(objInfo10);
                 
                    clsSqlParametersInfo objInfo11 = new clsSqlParametersInfo();
                    objInfo11.Direction = "Input";
                    objInfo11.PName = "@pAboutMe";
                    objInfo11.PValue = aboutMe;
                    objInfo11.PDBType = "NText";
                    objInfo11.PSize = 200;
                    lstSP.Add(objInfo11);
                 
                    clsSqlParametersInfo objInfo12 = new clsSqlParametersInfo();
                    objInfo12.Direction = "Input";
                    objInfo12.PName = "@pHomePhone";
                    objInfo12.PValue = homePhone;
                    objInfo12.PDBType = "NVarChar";
                    objInfo12.PSize = 200;
                    lstSP.Add(objInfo12);
                 
                    clsSqlParametersInfo objInfo13 = new clsSqlParametersInfo();
                    objInfo13.Direction = "Input";
                    objInfo13.PName = "@pOfficePhone";
                    objInfo13.PValue = officePhone;
                    objInfo13.PDBType = "NVarChar";
                    objInfo13.PSize = 200;
                    lstSP.Add(objInfo13);
                  
                    clsSqlParametersInfo objInfo14 = new clsSqlParametersInfo();
                    objInfo14.Direction = "Input";
                    objInfo14.PName = "@pMobilePhone";
                    objInfo14.PValue = mobilePhone;
                    objInfo14.PDBType = "NVarChar";
                    objInfo14.PSize = 200;
                    lstSP.Add(objInfo14);

                    clsSqlParameterContract CSqlInfo = new clsSqlParameterContract();
                    CSqlInfo.objParam = lstSP;

                   DataSet ds = VMukti.Business.clsDataBaseChannel.chHttpDataBaseService.svcExecuteDataSet("spAEUserProfile", CSqlInfo).dsInfo;
                }
                else
                {

                    new VMukti.DataAccess.clsProfile().UpdateUserProfile(userID, fullName, email, country, state, city, timezone, language, gender, birthDate, homepage, aboutMe, homePhone, officePhone, mobilePhone);
                }
            }
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "UpdateUserProfile()", "clsProfile.cs");                
            }
        }

        public static int GetUserID(string displayName)
        {
            try
            {
                int userID =-1;
                
                DataSet ds = null;

                if (bool.Parse(VMuktiAPI.VMuktiInfo.Port80) && VMuktiAPI.VMuktiInfo.CurrentPeer.CurrPeerType != PeerType.BootStrap)
                {
                    ds = clsDataBaseChannel.chHttpDataBaseService.svcExecuteDataSet("select * from vUserInfo where DisplayName='" + displayName + "'").dsInfo;
                }
                else
                {

                if (VMuktiAPI.VMuktiInfo.CurrentPeer.CurrPeerType == VMuktiAPI.PeerType.NodeWithHttp)
                {
                    ds = clsDataBaseChannel.chHttpDataBaseService.svcExecuteDataSet("select * from vUserInfo where DisplayName='"+displayName+"'").dsInfo;
                }
                else
                {
                    ds = new VMukti.DataAccess.clsProfile().GetUserID(displayName);
                }
                }
                if (ds.Tables[0] != null)
                {
                    DataRow dr;
                    if (ds.Tables[0].Rows.Count != 0)
                    {
                        dr = ds.Tables[0].Rows[0];
                        userID = int.Parse(dr["ID"].ToString());
                    }
                    else
                        return -1;
                    
                }

                return userID;
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "GetUserID()", "clsProfile.cs");               
                return -1;
            }
        }

        public static string GetUserDisplayName(int userID)
        {
            try
            {
                string displayName = null;
                
                DataSet ds = null;

                if (bool.Parse(VMuktiAPI.VMuktiInfo.Port80) && VMuktiAPI.VMuktiInfo.CurrentPeer.CurrPeerType != PeerType.BootStrap)
                {
                    ds = clsDataBaseChannel.chHttpDataBaseService.svcExecuteDataSet("select * from vUserInfo where ID='" + userID + "'").dsInfo;
                }
                else
                {

                if (VMuktiAPI.VMuktiInfo.CurrentPeer.CurrPeerType == VMuktiAPI.PeerType.NodeWithHttp)
                {
                    ds = clsDataBaseChannel.chHttpDataBaseService.svcExecuteDataSet("select * from vUserInfo where ID='" + userID + "'").dsInfo;
                }
                else
                {
                    ds = new VMukti.DataAccess.clsProfile().GetUserDisplayName(userID);
                    }
                }

                if (ds.Tables[0] != null)
                {
                    DataRow dr;
                    if (ds.Tables[0].Rows.Count != 0)
                    {
                        dr = ds.Tables[0].Rows[0];
                        displayName = dr["DisplayName"].ToString();
                    }
                    else
                        return null;

                }

                return displayName;
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "GetUserDisplayName()", "clsProfile.cs");             
                return null;
            }
        }

        public static string GetUserEmail(int userID)
        {
            try
            {
                string email = null;
                
                DataSet ds = null;

                if (bool.Parse(VMuktiAPI.VMuktiInfo.Port80) && VMuktiAPI.VMuktiInfo.CurrentPeer.CurrPeerType != PeerType.BootStrap)
                {
                    ds = clsDataBaseChannel.chHttpDataBaseService.svcExecuteDataSet("select * from vUserInfo where ID='" + userID + "'").dsInfo;
                }
                else
                {

                if (VMuktiAPI.VMuktiInfo.CurrentPeer.CurrPeerType == VMuktiAPI.PeerType.NodeWithHttp)
                {
                    ds = clsDataBaseChannel.chHttpDataBaseService.svcExecuteDataSet("select * from vUserInfo where ID='" + userID + "'").dsInfo;
                }
                else
                {
                    ds = new VMukti.DataAccess.clsProfile().GetUserDisplayName(userID);
                }
                }
                if (ds.Tables[0] != null)
                {
                    DataRow dr;
                    if (ds.Tables[0].Rows.Count != 0)
                    {
                        dr = ds.Tables[0].Rows[0];
                        email = dr["EMail"].ToString();
                    }
                    else
                        return null;

                }

                return email;
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "GetUserDisplayName()", "clsProfile.cs");
                return null;
            }
        }
    }

}
