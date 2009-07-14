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
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Reflection;
using System.Collections;
using System.IO;
using CRMContainer.Business;
using System.Net;
//using ICSharpCode.SharpZipLib.Zip;
using VMukti.ZipUnzip;
using VMuktiService;
using VMuktiAPI;

namespace CRMContainer.Presentation
{
    /// <summary>
    /// Interaction logic for CtlCRMContainer.xaml
    /// </summary>
    /// 
    public enum ModulePermissions
    {
        View = 1
    }

    public partial class CtlCRMContainer : UserControl
    {
        //public static StringBuilder sb1;
        ModulePermissions[] _MyPermissions;
        public ArrayList al = new ArrayList();
        string strModPath;
        string str;
        string filename;
        string destination;
        string filePath;
        public static IHTTPFileTransferService clientHttpFileTransfer = null;

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

        public CtlCRMContainer(ModulePermissions[] MyPermissions)
        {
            try
            {
            InitializeComponent();
            _MyPermissions = MyPermissions;

            //Review the Permission.
            FncPermissionsReview();

            //Downloading the CRM from Bootstrap.
            Fnc();
            }
            catch (Exception exp)
            {
                VMuktiHelper.ExceptionHandler(exp, "CtlCRMContainer()", "CtlCRMContainer.xaml.cs");
            }
        }


        //This function review the Permissions of registerd user.
        void FncPermissionsReview()
        {
            try
            {
            this.Visibility = Visibility.Hidden;

            for (int i = 0; i < _MyPermissions.Length; i++)
            {
                if (_MyPermissions[i] == ModulePermissions.View)
                {
                    this.Visibility = Visibility.Visible;
                }
            }
            }
            catch (Exception exp)
            {
                VMuktiHelper.ExceptionHandler(exp, "FncPermissionsReview()", "CtlCRMContainer.xaml.cs");
            }
        }

        //This function downloads the zip file from the Bootstrap.
        void Fnc()
        {
            try
            {
                Assembly ass3 = Assembly.GetEntryAssembly();
                if (Directory.Exists(ass3.Location.Replace("VMukti.Presentation.exe", @"CRMs")))
                {
                    Directory.Delete(ass3.Location.Replace("VMukti.Presentation.exe", @"CRMs"), true);
                }
            }
            catch (Exception e)
            {
                VMuktiHelper.ExceptionHandler(e, "Fnc()", "CtlCRMContainer.xaml.cs");
            }
            try{
                Assembly ass1 = Assembly.GetEntryAssembly();
                if (Directory.Exists(ass1.Location.Replace("VMukti.Presentation.exe", @"CRMModules")))
                {
                    Directory.Delete(ass1.Location.Replace("VMukti.Presentation.exe", @"CRMModules"), true);
                }
            }
            catch(Exception e)
            {
                VMuktiHelper.ExceptionHandler(e, "Fnc()", "CtlCRMContainer.xaml.cs");
            }
            try
            {
                if (DownloadAndExtractZipFile() == -1)
                {
                    return;
                }

                //Retrives the Zip name from the Database.
                //Calls the Funcion of the CRMContainer.Business.
                
                #region Loading ReferencedAssemblies

                al.Clear();

                //Creating the directory info.
                DirectoryInfo dirinfomodule = new DirectoryInfo(strModPath + "\\" + str);

                //Creating the Directory for dlls.
                ShowDirectory(dirinfomodule);

                //Creating the Assemblies.
                
                Assembly a;
                for (int j = 0; j < al.Count; j++)
                {
                    string[] arraysplit = al[j].ToString().Split('\\');
                    //MessageBox.Show("1" + arraysplit[arraysplit.Length - 1].ToString());
                    if(arraysplit[arraysplit.Length-1].ToString().Contains(".Presentation.dll"))
                    //if (arraysplit[arraysplit.Length - 1].ToString() == str + ".Presentation.dll")
                    {
                        a = Assembly.LoadFrom(al[j].ToString());
                        AssemblyName[] an = a.GetReferencedAssemblies();

                        for (int alcount = 0; alcount < al.Count; alcount++)
                        {
                            string strsplit = al[alcount].ToString();
                            string[] strold = strsplit.Split('\\');
                            string strnew = strold[strold.Length - 1].Substring(0, strold[strold.Length - 1].Length - 4);

                            for (int asscount = 0; asscount < an.Length; asscount++)
                            {
                                if (an[asscount].Name == strnew)
                                {
                                    Assembly assbal = System.AppDomain.CurrentDomain.Load(System.Reflection.Assembly.LoadFrom(al[alcount].ToString()).GetName());
                                    AssemblyName[] anbal = assbal.GetReferencedAssemblies();
                                    for (int andal = 0; andal < al.Count; andal++)
                                    {
                                        string strsplitdal = al[andal].ToString();
                                        string[] strolddal = strsplitdal.Split('\\');
                                        string strnewdal = strolddal[strolddal.Length - 1].Substring(0, strolddal[strolddal.Length - 1].Length - 4);

                                        for (int asscountdal = 0; asscountdal < anbal.Length; asscountdal++)
                                        {
                                            if (anbal[asscountdal].Name == strnewdal)
                                            {
                                                Assembly assdal = System.AppDomain.CurrentDomain.Load(System.Reflection.Assembly.LoadFrom(al[andal].ToString()).GetName());
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        Type[] t1 = a.GetTypes();

                        #region CreatingObject

                        //Creating the Control.
                        for (int k = 0; k < t1.Length; k++)
                        {
                            if (t1[k].Name == "UserControl1")
                            {
                                try
                                {
                                    //Sets the Properties of the Controls.
                                    object obj1 = Activator.CreateInstance(t1[k], null);
                                    ((UserControl)obj1).SetValue(Canvas.LeftProperty, 0.0);
                                    ((UserControl)obj1).SetValue(Canvas.TopProperty, 0.0);

                                    //Adds newely created Control to canvas.
                                    cnvLayoutRoot.Children.Add((UIElement)obj1);
                                }
                                catch (Exception exp)
                                {
                                    VMuktiHelper.ExceptionHandler(exp, "Fnc(Inner)", "CtlCRMContainer.xaml.cs");
                                }
                            }
                        }
                        #endregion
                    }
                }

                #endregion

               
            }
            catch (Exception exp)
            {
                VMuktiHelper.ExceptionHandler(exp, "Fnc(Outer)", "CtlCRMContainer.xaml.cs");
                if (exp.InnerException != null)
                {
                    //MessageBox.Show(exp.InnerException.Message);
                    VMuktiHelper.ExceptionHandler(exp, "Fnc(Inner)", "CtlCRMContainer.xaml.cs");
                }
            }
        }

        string CRMName = ClsCRMContainer.GetZipName(VMuktiAPI.VMuktiInfo.CurrentPeer.CampaignID);
        int DownloadAndExtractZipFile()
        {
            str = CRMName;
                str = str + "_CRM";

                //Retrives the Assembly of the Zip file.
                Assembly ass = Assembly.GetEntryAssembly();

                #region Downloading ZipFile

                //Set the Variable.
                filename = str + ".zip";
                
                //string strModPath = "";


                #region Download Zip File using WCF FileTranserService

                //Download file using WCF FileTransferService
                

                try
                {
                    //Create object of the HTTP Client.
                    BasicHttpClient bhcFts = new BasicHttpClient();

                    //Open Client.
                    clientHttpFileTransfer = (IHTTPFileTransferService)bhcFts.OpenClient<IHTTPFileTransferService>("http://" + VMuktiInfo.BootStrapIPs[0] + ":80/VMukti/HttpFileTransferService");

                    //Join the network.
                    clientHttpFileTransfer.svcHTTPFileTransferServiceJoin();

                    //Create request to download File from Bootstrap.
                    DownloadRequest request = new DownloadRequest();

                    //Providing the information of the file that needs to download.
                    RemoteFileInfo rfi = new RemoteFileInfo();
                    request.FileName = filename;
                    request.FolderWhereFileIsStored = "CRMs";

                    //Calling the WCF Function for downloading the File.
                    rfi = clientHttpFileTransfer.svcHTTPFileTransferServiceDownloadFile(request);


                    //Checking for Directory Existence.
                    //if (!Directory.Exists(ass.Location.Replace("VMukti.Presentation.exe", @"CRMs")))
                    if (!Directory.Exists(ass.Location.Replace("VMukti.Presentation.exe", @"CRMs")))
                    {
                        Directory.CreateDirectory(ass.Location.Replace("VMukti.Presentation.exe", @"CRMs"));
                    }

                    destination = ass.Location.Replace("VMukti.Presentation.exe", @"CRMs");

                    //Checking for File Existance.
                    if (File.Exists((destination + "\\" + filename)))
                    {
                        File.Delete((destination + "\\" + filename));
                    }

                    filePath = destination + "\\" + filename;

                    System.IO.Stream inputStream = rfi.FileByteStream;


                    using (System.IO.FileStream writeStream = new System.IO.FileStream(filePath, System.IO.FileMode.CreateNew, System.IO.FileAccess.Write))
                    {
                        int chunkSize = 2048;
                        byte[] buffer = new byte[chunkSize];

                        do
                        {
                            // read bytes from input stream
                            int bytesRead = inputStream.Read(buffer, 0, chunkSize);
                            if (bytesRead == 0) break;

                            // write bytes to output stream
                            writeStream.Write(buffer, 0, bytesRead);

                        } while (true);

                        writeStream.Close();
                    }
                }
                catch (Exception ex)
                {
                    //MessageBox.Show("Download Zip: " + ex.Message);
                    VMuktiHelper.ExceptionHandler(ex, "DownloadAndExtractZipFile()", "CtlCRMContainer.xaml.cs");
                    if (ex.InnerException != null)
                    {
                        //MessageBox.Show("Download Zip Inner Exception: " + ex.InnerException.Message);
                        VMuktiHelper.ExceptionHandler(ex, "DownloadAndExtractZipFile()", "CtlCRMContainer.xaml.cs");
                    }
                }

                #endregion

                #region Downloading using Web Client --------Commented
                //try
                //{
                //    Uri u = new Uri("http://" + VMuktiAPI.VMuktiInfo.BootStrapIPs[0] + "/VMukti/CRMs/" + filename);
                //    if (!Directory.Exists(ass.Location.Replace("VMukti.Presentation.exe", @"CRMs")))
                //    {
                //        Directory.CreateDirectory(ass.Location.Replace("VMukti.Presentation.exe", @"CRMs"));
                //    }
                //    destination = ass.Location.Replace("VMukti.Presentation.exe", @"CRMs");
                //    if (!File.Exists(destination + "\\" + filename))
                //    {
                //        WebClient wc = new WebClient();
                //        try
                //        {
                //            wc.DownloadFile(u, destination + "\\" + filename);
                //        }
                //        catch (Exception exp)
                //        {
                //            MessageBox.Show(exp.Message);
                //        }
                //    }
                //}
                //catch (Exception exp)
                //{
                //    MessageBox.Show("Down Load Error - " + exp.Message);
                //}
                #endregion

                #endregion

                #region Extracting

                //Code for Extracting the .zip file.
                try
                {
                    //Checking for the Existance of the Directory.

                    if (!Directory.Exists(ass.Location.Replace("VMukti.Presentation.exe", @"CRMModules")))
                    {
                        Directory.CreateDirectory(ass.Location.Replace("VMukti.Presentation.exe", @"CRMModules"));
                    }
                    str = CRMName;
                    str = str + "_CRM";
                    Assembly ass2 = Assembly.GetEntryAssembly();
                    strModPath = ass2.Location.Replace("VMukti.Presentation.exe", @"CRMModules");

                    VMukti.ZipUnzip.Zip.FastZip fz = new VMukti.ZipUnzip.Zip.FastZip();
                    //ICSharpCode.SharpZipLib.Zip.FastZip fz = new ICSharpCode.SharpZipLib.Zip.FastZip();

                    //FastZip fz = new FastZip();
                    //if (!Directory.Exists(strModPath + "\\" + filename.Split('.')[0]))
                    //{
                        try
                        {
                            //Extracting the zip file.
                            fz.ExtractZip(destination + "\\" + filename, strModPath+"\\"+str, null);
                        }
                        catch (Exception exp)
                        {
                            //MessageBox.Show(exp.Message + " First Desgine Required CRM Using CRMDesginer");
                            VMuktiHelper.ExceptionHandler(exp, "DownloadAndExtractZipFile()", "CtlCRMContainer.xaml.cs");
                        }
                   // }
                }
                catch (Exception exp)
                {
                    VMuktiHelper.ExceptionHandler(exp, "DownloadAndExtractZipFile(Outer)", "CtlCRMContainer.xaml.cs");
                    return -1;
                }

                #endregion
                return 0;

        }
        
        //This Function creates the Directory for dll files.
        public void ShowDirectory(DirectoryInfo dir)
        {
            try
            {
            foreach (FileInfo file in dir.GetFiles("*.dll"))
            {
                int hj = al.Add(file.FullName);
            }
            foreach (DirectoryInfo subDir in dir.GetDirectories())
            {
                ShowDirectory(subDir);
            }
            }
            catch (Exception exp)
            {
                VMuktiHelper.ExceptionHandler(exp, "ShowDirectory()", "CtlCRMContainer.xaml.cs");
            }
        }
    }
}
