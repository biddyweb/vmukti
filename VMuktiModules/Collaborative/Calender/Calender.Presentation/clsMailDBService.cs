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
//using System.Linq;
using System.Text;
using VMuktiService;
using VMuktiAPI;
using Calender.Business.Service;
using System.Data.SqlClient;
using System.Data;

namespace Calender.Presentation
{
    public class clsMailDBService
    {
        //public static StringBuilder sb1;
        object objHttpMailDBService = null;


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

        public clsMailDBService()
        {
            objHttpMailDBService = new MailDBService();
            ((MailDBService)objHttpMailDBService).EntsvcJoin += new MailDBService.DelsvcJoin(clsMailDBService_EntsvcJoin);
            ((MailDBService)objHttpMailDBService).EntsvcExecuteDataSet += new MailDBService.DelsvcExecuteDataSet(clsMailDBService_EntsvcExecuteDataSet);
            ((MailDBService)objHttpMailDBService).EntsvcExecuteNonQuery += new MailDBService.DelsvcExecuteNonQuery(clsMailDBService_EntsvcExecuteNonQuery);
            ((MailDBService)objHttpMailDBService).EntsvcExecuteReturnNonQuery += new MailDBService.DelsvcExecuteReturnNonQuery(clsMailDBService_EntsvcExecuteReturnNonQuery);
            ((MailDBService)objHttpMailDBService).EntsvcExecuteStoredProcedure += new MailDBService.DelsvcExecuteStoredProcedure(clsMailDBService_EntsvcExecuteStoredProcedure);
            ((MailDBService)objHttpMailDBService).EntsvcSendMail += new MailDBService.DelsvcSendMail(clsMailDBService_EntsvcSendMail);
            ((MailDBService)objHttpMailDBService).EntsvcUnJoin += new MailDBService.DelsvcUnJoin(clsMailDBService_EntsvcUnJoin);

            BasicHttpServer bhsHttpMailDB = new BasicHttpServer(ref objHttpMailDBService, "http://" + VMuktiInfo.CurrentPeer.SuperNodeIP + ":80/VMukti/SNMailDB");
            bhsHttpMailDB.AddEndPoint<IClsMailDBService>("http://" + VMuktiInfo.CurrentPeer.SuperNodeIP + ":80/VMukti/SNMailDB");
            bhsHttpMailDB.OpenServer();
            

        }

        void clsMailDBService_EntsvcSendMail(clsMailInfo objMail)
        {
            try
            {
                System.Net.Mail.MailMessage mailMsg = new System.Net.Mail.MailMessage();
                mailMsg.From = new System.Net.Mail.MailAddress(objMail.strFrom);
                mailMsg.To.Add(objMail.strTo);

                mailMsg.Subject = objMail.strSubject;
                mailMsg.SubjectEncoding = System.Text.Encoding.UTF8;
                mailMsg.Body = objMail.strMsg;
                mailMsg.BodyEncoding = System.Text.Encoding.UTF8;
                mailMsg.Priority = System.Net.Mail.MailPriority.High;
                mailMsg.IsBodyHtml = true;




                System.Net.Mail.SmtpClient SmtpMail = new System.Net.Mail.SmtpClient(objMail.strServer, objMail.intPort);
                SmtpMail.Credentials = new System.Net.NetworkCredential(objMail.strFrom, objMail.strPwd);
                SmtpMail.EnableSsl = true;

                SmtpMail.Send(mailMsg);
            }
            catch (Exception exp)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(exp, "clsMailDBService_EntsvcSendMail()", "clsMailDBService.xaml.cs");
            }
        }

        clsDataBaseInfo clsMailDBService_EntsvcExecuteStoredProcedure(string spName, clsSqlParameterContract objSParam)
        {
            try
            {
                SqlConnection conn = new SqlConnection(VMuktiInfo.MainConnectionString);
                conn.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = spName;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Connection = conn;
                if (objSParam != null && objSParam.objParam != null)
                {
                    for (int paramCnt = 0; paramCnt < objSParam.objParam.Count; paramCnt++)
                    {
                        SqlParameter sp = new SqlParameter();
                        sp.ParameterName = (string)objSParam.objParam[paramCnt].PName;
                        if (sp.Size != -1)
                        {
                            sp.Size = (int)objSParam.objParam[paramCnt].PSize;
                        }
                        sp.Value = (object)objSParam.objParam[paramCnt].PValue;

                        string direction = (string)objSParam.objParam[paramCnt].Direction;
                        string dbType = (string)objSParam.objParam[paramCnt].PDBType;

                        //Enum.GetNames(typeof(SqlDbType))

                        switch (direction)
                        {
                            case "Input":
                                {
                                    sp.Direction = ParameterDirection.Input;
                                    break;
                                }
                            case "InputOutput":
                                {
                                    sp.Direction = ParameterDirection.InputOutput;
                                    break;
                                }
                            case "Output":
                                {
                                    sp.Direction = ParameterDirection.Output;
                                    break;
                                }
                            case "ReturnValue":
                                {
                                    sp.Direction = ParameterDirection.ReturnValue;
                                    break;
                                }
                            default:
                                {
                                    break;
                                }

                        }

                        switch (dbType)
                        {
                            case "VarChar":
                                {
                                    sp.SqlDbType = SqlDbType.VarChar;
                                    break;
                                }
                            case "NVarChar":
                                {
                                    sp.SqlDbType = SqlDbType.NVarChar;

                                    break;
                                }
                            case "Char":
                                {
                                    sp.SqlDbType = SqlDbType.Char;

                                    break;
                                }
                            case "NChar":
                                {
                                    sp.SqlDbType = SqlDbType.NChar;

                                    break;
                                }
                            case "Text":
                                {
                                    sp.SqlDbType = SqlDbType.Text;

                                    break;
                                }
                            case "DateTime":
                                {
                                    sp.SqlDbType = SqlDbType.DateTime;

                                    break;
                                }
                            case "Int":
                                {
                                    sp.SqlDbType = SqlDbType.Int;

                                    break;
                                }
                            case "UniqueIdentifier":
                                {
                                    sp.SqlDbType = SqlDbType.UniqueIdentifier;

                                    break;
                                }

                            case "Bit":
                                {
                                    sp.SqlDbType = SqlDbType.Bit;

                                    break;
                                }

                            case "Float":
                                {
                                    sp.SqlDbType = SqlDbType.Float;

                                    break;
                                }

                            case "Decimal":
                                {
                                    sp.SqlDbType = SqlDbType.Decimal;

                                    break;
                                }
                            case "BigInt":
                                {
                                    sp.SqlDbType = SqlDbType.BigInt;

                                    break;
                                }
                            default:
                                {
                                    break;
                                }

                        }
                        cmd.Parameters.Add(sp);


                    }

                }


                SqlDataAdapter sda = new SqlDataAdapter(cmd);
                clsDataBaseInfo dbContract = new clsDataBaseInfo();
                dbContract.dsInfo = new DataSet();
                sda.Fill(dbContract.dsInfo);
                sda.Dispose();
                conn.Close();
                conn.Dispose();
                cmd.Dispose();
                return dbContract;

            }
            catch (Exception exp)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(exp, "clsMailDBService_EntsvcExecuteStoredProcedure()", "clsMailDBService.xaml.cs");
                return null;
            }
        }

        int clsMailDBService_EntsvcExecuteReturnNonQuery(string spName, clsSqlParameterContract objSParam)
        {
            try
            {
                int Result = -1;
                SqlConnection conn = new SqlConnection(VMuktiInfo.MainConnectionString);
                conn.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = spName;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Connection = conn;
                if (objSParam != null && objSParam.objParam != null)
                {
                    for (int paramCnt = 0; paramCnt < objSParam.objParam.Count; paramCnt++)
                    {
                        SqlParameter sp = new SqlParameter();
                        sp.ParameterName = (string)objSParam.objParam[paramCnt].PName;
                        if (sp.Size != -1)
                        {
                            sp.Size = (int)objSParam.objParam[paramCnt].PSize;
                        }
                        sp.Value = (object)objSParam.objParam[paramCnt].PValue;

                        string direction = (string)objSParam.objParam[paramCnt].Direction;
                        string dbType = (string)objSParam.objParam[paramCnt].PDBType;

                        //Enum.GetNames(typeof(SqlDbType))

                        switch (direction)
                        {
                            case "Input":
                                {
                                    sp.Direction = ParameterDirection.Input;
                                    break;
                                }
                            case "InputOutput":
                                {
                                    sp.Direction = ParameterDirection.InputOutput;
                                    break;
                                }
                            case "Output":
                                {
                                    sp.Direction = ParameterDirection.Output;
                                    break;
                                }
                            case "ReturnValue":
                                {
                                    sp.Direction = ParameterDirection.ReturnValue;
                                    break;
                                }
                            default:
                                {
                                    break;
                                }

                        }

                        switch (dbType)
                        {
                            case "VarChar":
                                {
                                    sp.SqlDbType = SqlDbType.VarChar;
                                    break;
                                }
                            case "NVarChar":
                                {
                                    sp.SqlDbType = SqlDbType.NVarChar;

                                    break;
                                }
                            case "Char":
                                {
                                    sp.SqlDbType = SqlDbType.Char;

                                    break;
                                }
                            case "NChar":
                                {
                                    sp.SqlDbType = SqlDbType.NChar;

                                    break;
                                }
                            case "Text":
                                {
                                    sp.SqlDbType = SqlDbType.Text;

                                    break;
                                }
                            case "DateTime":
                                {
                                    sp.SqlDbType = SqlDbType.DateTime;

                                    break;
                                }
                            case "Int":
                                {
                                    sp.SqlDbType = SqlDbType.Int;

                                    break;
                                }
                            case "UniqueIdentifier":
                                {
                                    sp.SqlDbType = SqlDbType.UniqueIdentifier;

                                    break;
                                }

                            case "Bit":
                                {
                                    sp.SqlDbType = SqlDbType.Bit;

                                    break;
                                }

                            case "Float":
                                {
                                    sp.SqlDbType = SqlDbType.Float;

                                    break;
                                }

                            case "Decimal":
                                {
                                    sp.SqlDbType = SqlDbType.Decimal;

                                    break;
                                }
                            case "BigInt":
                                {
                                    sp.SqlDbType = SqlDbType.BigInt;

                                    break;
                                }
                            default:
                                {
                                    break;
                                }

                        }
                        cmd.Parameters.Add(sp);
                    }

                }

                cmd.ExecuteNonQuery();
                for (int i = 0; i < cmd.Parameters.Count; i++)
                {
                    if (cmd.Parameters[i].Direction == ParameterDirection.InputOutput || cmd.Parameters[i].Direction == ParameterDirection.Output)
                    {
                        Result = int.Parse(cmd.Parameters[i].Value.ToString());
                    }
                }
                conn.Close();
                conn.Dispose();
                cmd.Dispose();
                return Result;

            }
            catch (Exception exp)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(exp, "clsMailDBService_EntsvcExecuteReturnNonQuery()", "clsMailDBService.xaml.cs");
                return -1;
            }           
            
        }

        void clsMailDBService_EntsvcExecuteNonQuery(string spName, clsSqlParameterContract objSParam)
        {
            try
            {
                SqlConnection conn = new SqlConnection(VMuktiInfo.MainConnectionString);
                conn.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = spName;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Connection = conn;
                if (objSParam != null && objSParam.objParam != null)
                {
                    for (int paramCnt = 0; paramCnt < objSParam.objParam.Count; paramCnt++)
                    {
                        SqlParameter sp = new SqlParameter();
                        sp.ParameterName = (string)objSParam.objParam[paramCnt].PName;
                        if (sp.Size != -1)
                        {
                            sp.Size = (int)objSParam.objParam[paramCnt].PSize;
                        }
                        sp.Value = (object)objSParam.objParam[paramCnt].PValue;

                        string direction = (string)objSParam.objParam[paramCnt].Direction;
                        string dbType = (string)objSParam.objParam[paramCnt].PDBType;

                        //Enum.GetNames(typeof(SqlDbType))

                        switch (direction)
                        {
                            case "Input":
                                {
                                    sp.Direction = ParameterDirection.Input;
                                    break;
                                }
                            case "InputOutput":
                                {
                                    sp.Direction = ParameterDirection.InputOutput;
                                    break;
                                }
                            case "Output":
                                {
                                    sp.Direction = ParameterDirection.Output;
                                    break;
                                }
                            case "ReturnValue":
                                {
                                    sp.Direction = ParameterDirection.ReturnValue;
                                    break;
                                }
                            default:
                                {
                                    break;
                                }

                        }

                        switch (dbType)
                        {
                            case "VarChar":
                                {
                                    sp.SqlDbType = SqlDbType.VarChar;
                                    break;
                                }
                            case "NVarChar":
                                {
                                    sp.SqlDbType = SqlDbType.NVarChar;
                                    
                                    break;
                                }
                            case "Char":
                                {
                                    sp.SqlDbType = SqlDbType.Char;

                                    break;
                                }
                            case "NChar":
                                {
                                    sp.SqlDbType = SqlDbType.NChar;

                                    break;
                                }
                            case "Text":
                                {
                                    sp.SqlDbType = SqlDbType.Text;

                                    break;
                                }
                            case "DateTime":
                                {
                                    sp.SqlDbType = SqlDbType.DateTime;

                                    break;
                                }
                            case "Int":
                                {
                                    sp.SqlDbType = SqlDbType.Int;

                                    break;
                                }
                            case "UniqueIdentifier":
                                {
                                    sp.SqlDbType = SqlDbType.UniqueIdentifier;

                                    break;
                                }

                            case "Bit":
                                {
                                    sp.SqlDbType = SqlDbType.Bit;

                                    break;
                                }

                            case "Float":
                                {
                                    sp.SqlDbType = SqlDbType.Float;

                                    break;
                                }

                            case "Decimal":
                                {
                                    sp.SqlDbType = SqlDbType.Decimal;

                                    break;
                                }
                            case "BigInt":
                                {
                                    sp.SqlDbType = SqlDbType.BigInt;

                                    break;
                                }
                            default:
                                {
                                    break;
                                }

                        }
                        cmd.Parameters.Add(sp);


                    }

                }


                cmd.ExecuteNonQuery();
                conn.Close();
                conn.Dispose();
                cmd.Dispose();
            }
            catch (Exception exp)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(exp, "clsMailDBService_EntsvcExecuteNonQuery()", "clsMailDBService.xaml.cs");
            }
        }

        clsDataBaseInfo clsMailDBService_EntsvcExecuteDataSet(string querystring)
        {
            try
            {
                SqlConnection conn = new SqlConnection(VMuktiInfo.MainConnectionString);
                SqlCommand cmd = new SqlCommand(querystring, conn);

                SqlDataAdapter sda = new SqlDataAdapter(cmd);
                clsDataBaseInfo dbContract = new clsDataBaseInfo();
                dbContract.dsInfo = new DataSet();
                sda.Fill(dbContract.dsInfo);
                sda.Dispose();
                conn.Close();
                conn.Dispose();
                cmd.Dispose();
                return dbContract;
            }
            catch (Exception exp)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(exp, "clsMailDBService_EntsvcExecuteDataSet()", "clsMailDBService.xaml.cs");
                return null;
            }
        }

        void clsMailDBService_EntsvcUnJoin(string uname)
        {
            
        }

        void clsMailDBService_EntsvcJoin(string uname)
        {
            
        }
    }
}
