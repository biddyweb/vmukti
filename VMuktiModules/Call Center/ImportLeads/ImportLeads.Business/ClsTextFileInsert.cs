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
using System.IO;
using System.Data;
using ImportLeads.DataAccess;
using VMuktiAPI;
using System.Data.SqlClient;

namespace ImportLeads.Business
{

    public class ClsTextFileInsert
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
        private string _fileName = "";

        DataTable dtLeadDetail = null;
        DataTable dtLead = null;
        DataTable dtLocation = null;

        public string fileName
        {
            get { return _fileName; }
            set { _fileName = value; }
        }


        public ClsTextFileInsert(string FileName)
        {
            try
            {
                fileName = FileName;
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "ClsTextFileInsert()", "ClsTextFileInsert.cs");
            }
        }

        public int[] TotalCount(Int64 LeadFormatID, int ListID, string SelectedTimeZone, string FilterType, int TotalTimeZone, int CountryCode)
        {
            DataSet dsCustomeFieldDetails = new DataSet();

            DataSet StateCountry = ClsList.StateCountry_GetAll();

            DataTable dtTemp = new DataTable();
            //DataRow[] drCollection = null;

            if (FilterType == "TIMEZONE" || FilterType == "SITE")
            {
                DataSet ds = ClsList.AreaCode_GetAll(SelectedTimeZone, "in");
                dtTemp = ds.Tables[0];
                //drCollection = dtTemp.Select("TimezoneName IN (" + strTimezone + ")");

            }


            ClsImportLeadsDataService clsReterive = new ClsImportLeadsDataService();

            dsCustomeFieldDetails = clsReterive.ReteriveCustomeFieldDetails(LeadFormatID);

            DataRow[] drPhoneNumber = dsCustomeFieldDetails.Tables[0].Select("CustomFieldID = 1");
            int phoneStartpos = Convert.ToInt32(drPhoneNumber[0]["StartPosition"]);
            int phonelen = Convert.ToInt32(drPhoneNumber[0]["Length"]);

            DataRow[] drStateID = dsCustomeFieldDetails.Tables[0].Select("CustomFieldID = 2");
            int StateStartpos = Convert.ToInt32(drStateID[0]["StartPosition"]);
            int Statelen = Convert.ToInt32(drStateID[0]["Length"]);

            DataRow[] drZipID = dsCustomeFieldDetails.Tables[0].Select("CustomFieldID = 3");
            int ZipStartpos = Convert.ToInt32(drZipID[0]["StartPosition"]);
            int Ziplen = Convert.ToInt32(drZipID[0]["Length"]);
            int[] ResultArray;


            if (drPhoneNumber.Count() > 0)
            {

                FileStream fs = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.None, 2 << 18, FileOptions.SequentialScan);
                DataTable dtTimezone = new DataTable();

                DataRow[] drStateCountry = null;

                DataSet dsTimeZone = ClsList.TimeZone_GetAll();
                dtTimezone = dsTimeZone.Tables[0];

                Int64 LeadMaxcount = ClsList.GetMaxIDLeads();
                Int64 MaxLeadDetail = ClsList.GetMaxLeadDetailID();
                Int64 MaxLocationID = ClsList.GetMaxLocationID();

                createDataTable();


                int TotalColm = dsCustomeFieldDetails.Tables[0].Rows.Count;

                StreamReader sr = new StreamReader(fs);





                // int cntFile = sr.ReadToEnd().Split('\n').Count();

                // string[] FileValueArray = sr.ReadToEnd().Split('\n') ;

                //int[] Fields = new int[TotalColm];
                /// int[] StartPosition = new int[TotalColm];
                // int[] Length = new int[TotalColm];



                //   fs.Close();

                //    sr.Close();

                /*  for (int Colcnt = 0; Colcnt < TotalColm; Colcnt++)
                  {

                      Fields[Colcnt] = Convert.ToInt32(dsCustomeFieldDetails.Tables[0].Rows[Colcnt]["CustomFieldID"]);
                      StartPosition[Colcnt] = Convert.ToInt32(dsCustomeFieldDetails.Tables[0].Rows[Colcnt]["StartPosition"]);
                      Length[Colcnt] = Convert.ToInt32(dsCustomeFieldDetails.Tables[0].Rows[Colcnt]["Length"]);

                  }*/



                SqlConnection conn = new SqlConnection(VMuktiAPI.VMuktiInfo.MainConnectionString);
                SqlCommand cmd = new SqlCommand("select getdate()", conn);
                conn.Open();
                SqlDataReader dr1;
                int Rowcnt = 0;
                //string Row = "";
                int badNumbers = 0;
                // Byte badNumberFlag = 0;
                // Byte RepeatFlag =0;
                Byte AreaCodeFlag = 0;

                int repeatNumbers = 0;
                int NoAreaCode = 0;
                DataTable dtPhoneNumber = new DataTable();

                DataSet ds1 = ClsList.PhoneNumbers_GetAll(ListID);
                dtPhoneNumber = ds1.Tables[0];


                try
                {
                    while (sr.EndOfStream != true)
                    //for (Rowcnt = 0; Rowcnt < FileValueArray.Count(); Rowcnt++)
                    {
                        Rowcnt = Rowcnt + 1;
                        string Row = sr.ReadLine();
                        //FileValueArray.ElementAt(Rowcnt);

                        Int64 phoneNumber;
                        try
                        {
                            phoneNumber = Convert.ToInt64(Row.Substring(phoneStartpos - 1, phonelen).Replace("-", "").Replace(" ", ""));
                            phoneNumber = Int64.Parse((Math.Pow(10 * CountryCode, (phoneNumber.ToString().Length))).ToString()) + phoneNumber;
                        }
                        catch (Exception exp)
                        {
                            badNumbers++;

                            //  badNumberFlag = 1;
                            continue;
                        }

                        DataRow[] drRepeat = dtPhoneNumber.Select("PhoneNo = " + phoneNumber);
                        if (drRepeat.Length > 0)
                        {
                            repeatNumbers++;
                            //  RepeatFlag = 1;
                            continue;
                        }
                        dtPhoneNumber.Rows.Add(phoneNumber);

                        dr1 = cmd.ExecuteReader();
                        dr1.Read();
                        //if (badNumberFlag == 0 || RepeatFlag == 0)
                        //  {



                        string ZipCode = Row.Substring(ZipStartpos - 1, Ziplen).Replace(" ", "");
                        //Int64 finalzipcode = String.IsNullOrEmpty(ZipCode) ? null : Int64.Parse(ZipCode);
                        drStateCountry = StateCountry.Tables[0].Select("zipcode='" + ZipCode.Substring(0, 5) + "'");

                        if (FilterType == "TIMEZONE")
                        {
                            //int loopcount = drCollection.Length;
                            int loopcount = dtTemp.Rows.Count;
                            DataRow drLocation;
                            //  DataRow[] AreaCod;
                            //AreaCod = dtTemp.Select("Areacode = '" + dtTrendWestLead.Rows[i][0].ToString() + "'");

                            for (int t = 0; t < loopcount; t++)
                            {
                                if (phoneNumber.ToString().Substring(1, 3) == dtTemp.Rows[t][0].ToString())
                                //if (dtTrendWestLead.Rows[i][15].ToString() == dtTemp[t][0].ToString())
                                {
                                    DataRow[] t1;
                                    t1 = dtTimezone.Select("TimezoneName = '" + dtTemp.Rows[t][1] + "'");

                                    Int64 gotIt = Int64.Parse(t1[0][1].ToString());
                                    if (drStateCountry.Length > 0)
                                    {
                                        drLocation = dtLocation.NewRow();
                                        drLocation["ID"] = MaxLocationID;
                                        drLocation["TimeZoneID"] = gotIt;
                                        drLocation["CountryID"] = Convert.ToInt64(drStateCountry[0][1].ToString());
                                        drLocation["StateID"] = Convert.ToInt64(drStateCountry[0][0].ToString());
                                        drLocation["AreaCodeID"] = Convert.ToInt64(dtTemp.Rows[t][0].ToString());
                                        drLocation["ZipCodeID"] = Convert.ToInt64(drStateCountry[0][2].ToString());
                                        dtLocation.Rows.Add(drLocation);
                                    }
                                    else
                                    {
                                        drLocation = dtLocation.NewRow();
                                        drLocation["ID"] = MaxLocationID;
                                        drLocation["TimeZoneID"] = gotIt;
                                        drLocation["CountryID"] = 0;
                                        drLocation["StateID"] = 0;
                                        drLocation["AreaCodeID"] = Convert.ToInt64(dtTemp.Rows[t][0].ToString());
                                        drLocation["ZipCodeID"] = 0;
                                        dtLocation.Rows.Add(drLocation);

                                    }
                                    AreaCodeFlag = 1;
                                    break;
                                    // varcount++;
                                }

                            }



                        }

                        if (AreaCodeFlag == 1)
                        {
                            AreaCodeFlag = 0;
                            dtLead.Rows.Add(LeadMaxcount, phoneNumber, LeadFormatID, Convert.ToDateTime(dr1[0].ToString()), VMuktiAPI.VMuktiInfo.CurrentPeer.ID, null, null, false, Convert.ToDateTime(dr1.GetValue(0)), VMuktiAPI.VMuktiInfo.CurrentPeer.ID, false, 0, ListID, MaxLocationID, 0, "Fresh", false);
                            int Colcnt = 0;


                            for (Colcnt = 0; Colcnt < TotalColm; Colcnt++)
                            {
                                Int64 fieldID = Convert.ToInt32(dsCustomeFieldDetails.Tables[0].Rows[Colcnt]["CustomFieldID"]); ;
                                int StartPositionTmp = Convert.ToInt32(dsCustomeFieldDetails.Tables[0].Rows[Colcnt]["StartPosition"]);
                                int LengthTmp = Convert.ToInt32(dsCustomeFieldDetails.Tables[0].Rows[Colcnt]["Length"]);

                                // string Row = FileValueArray.ElementAt(Rowcnt);
                                string Value = Row.Substring(StartPositionTmp - 1, LengthTmp);

                                dtLeadDetail.Rows.Add(MaxLeadDetail, LeadMaxcount, fieldID, Value);
                                MaxLeadDetail = MaxLeadDetail + 1;

                            }

                            MaxLocationID = MaxLocationID + 1;
                            LeadMaxcount = LeadMaxcount + 1;
                            Row = "";

                        }
                        else
                        {
                            NoAreaCode++;
                        }
                        dr1.Close();
                        // }
                        // else
                        // {
                        //  badNumberFlag = 0;
                        //  RepeatFlag = 0;
                        //}
                    }
                }
                catch (Exception ex)
                {
                    VMuktiHelper.ExceptionHandler(ex, "TotalCount()", "ClsTextFileInsert.cs");
                }
                finally
                {
                    sr.Close();
                    fs.Close();
                    sr = null;
                    fs = null;
                    GC.Collect(0, GCCollectionMode.Forced);

                    //GC.SuppressFinalize(this);
                }



                SqlBulkCopy sqlBulkLocation = new SqlBulkCopy(VMuktiAPI.VMuktiInfo.MainConnectionString);
                //SqlBulkCopy sqlBulkLocation = new SqlBulkCopy("Data Source=192.168.1.186\\SQLExpress;Initial Catalog=VMukti;User ID=sa;PassWord=mahavir",SqlBulkCopyOptions.FireTriggers);
                sqlBulkLocation.DestinationTableName = "Location";
                sqlBulkLocation.BulkCopyTimeout = 1000;
                sqlBulkLocation.WriteToServer(dtLocation);
                sqlBulkLocation.Close();


                SqlBulkCopy sqlBulkLead = new SqlBulkCopy(VMuktiAPI.VMuktiInfo.MainConnectionString);
                //SqlBulkCopy sqlBulkLocation = new SqlBulkCopy("Data Source=192.168.1.186\\SQLExpress;Initial Catalog=VMukti;User ID=sa;PassWord=mahavir",SqlBulkCopyOptions.FireTriggers);
                sqlBulkLead.DestinationTableName = "Leads";
                sqlBulkLead.BulkCopyTimeout = 1000;
                sqlBulkLead.WriteToServer(dtLead);
                sqlBulkLead.Close();


                SqlBulkCopy sqlBulkLeadDetail = new SqlBulkCopy(VMuktiAPI.VMuktiInfo.MainConnectionString);
                //SqlBulkCopy sqlBulkLocation = new SqlBulkCopy("Data Source=192.168.1.186\\SQLExpress;Initial Catalog=VMukti;User ID=sa;PassWord=mahavir",SqlBulkCopyOptions.FireTriggers);
                sqlBulkLeadDetail.DestinationTableName = "LeadDetail";
                sqlBulkLeadDetail.BulkCopyTimeout = 1000;
                sqlBulkLeadDetail.WriteToServer(dtLeadDetail);
                sqlBulkLeadDetail.Close();


                ResultArray = new int[] { Rowcnt, badNumbers, dtLead.Rows.Count, repeatNumbers, NoAreaCode };

                dtLocation = null;
                dtLead = null;
                dtLeadDetail = null;

                return ResultArray;
            }
            else
            {
                ResultArray = new int[] { 0 };
                return ResultArray;
            }

        }



        public void createDataTable()
        {
            try
            {

                dtLeadDetail = new DataTable();
                dtLeadDetail.Columns.Add("ID");
                dtLeadDetail.Columns[0].DataType = typeof(Int64);
                dtLeadDetail.Columns.Add("LeadID");
                dtLeadDetail.Columns[1].DataType = typeof(Int64);
                dtLeadDetail.Columns.Add("LeadFieldID");
                dtLeadDetail.Columns[2].DataType = typeof(Int64);
                dtLeadDetail.Columns.Add("PropertyValue");

                dtLead = new DataTable();
                dtLead.Columns.Add("ID");
                dtLead.Columns.Add("PhoneNo");
                dtLead.Columns.Add("LeadFormatID");
                dtLead.Columns.Add("CreatedDate");
                dtLead.Columns.Add("CreatedBy");
                dtLead.Columns.Add("DeletedDate");
                dtLead.Columns.Add("DeletedBy");
                dtLead.Columns.Add("IsDeleted");
                dtLead.Columns.Add("ModifiedDate");
                dtLead.Columns.Add("ModifiedBy");
                dtLead.Columns.Add("DNCFlag");
                dtLead.Columns.Add("DNCBy");
                dtLead.Columns.Add("ListID");
                dtLead.Columns.Add("LocationID");
                dtLead.Columns.Add("RecycleCount");
                dtLead.Columns.Add("Status");
                dtLead.Columns.Add("IsGlobalDNC");

                dtLocation = new DataTable();
                dtLocation.Columns.Add("ID");
                dtLocation.Columns[0].DataType = typeof(Int64);
                dtLocation.Columns.Add("TimeZoneID");
                dtLocation.Columns[1].DataType = typeof(Int64);
                dtLocation.Columns.Add("CountryID");
                dtLocation.Columns[2].DataType = typeof(Int64);
                dtLocation.Columns.Add("StateID");
                dtLocation.Columns[3].DataType = typeof(Int64);
                dtLocation.Columns.Add("AreaCodeID");
                dtLocation.Columns[4].DataType = typeof(Int64);
                dtLocation.Columns.Add("ZipCodeID");
                dtLocation.Columns[5].DataType = typeof(Int64);


            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "createDataTable()", "ClsTextFileInsert.cs");
            }
        }


    }
}
