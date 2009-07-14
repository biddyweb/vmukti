using System;
using System.Collections.Generic;
using System.Linq;
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
using System.ComponentModel;
using DashBoard.Business;
using System.Globalization;
using System.Threading;
using System.Reflection;
using System.IO;
using System.Net;
using System.Collections;
using VMuktiAPI;

namespace DashBoard.Presentation
{
    /// <summary>
    /// Interaction logic for ctlDashBoardContainer.xaml
    /// </summary>
    public partial class ctlDashBoardContainer : UserControl
    {
        Assembly ass=Assembly.GetAssembly(typeof(ctlDashBoardContainer));
        public ArrayList al = new ArrayList();
        Assembly a;
        ctlDashBoard objDashboard = null;


        public ctlDashBoardContainer(ModulePermissions[] MyPermissions)
        {
            try
            {
                InitializeComponent();

                TabItem tbItemDashBoard = new TabItem();
                tbItemDashBoard.Header = "Dynamic Report";
                tbItemDashBoard.Height=40;
                objDashboard = new ctlDashBoard();
                tbItemDashBoard.Content = objDashboard;
                tbCnt.Items.Add(tbItemDashBoard);

                DBReports();

                VMuktiAPI.VMuktiHelper.RegisterEvent("SingOut").VMuktiEvent += new VMuktiEvents.VMuktiEventHandler(ctlDashBoardContainer_VMuktiEvent);
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "ctlDashBoardContainer", "ctlDashBoardContainer.xaml.cs");
            }
        }

        void ctlDashBoardContainer_VMuktiEvent(object sender, VMuktiEventArgs e)
        {
            ClosePod();
        }

        public void ClosePod()
        {
            
            objDashboard.ClosePod();
        }
       

        void DBReports()
        {
            try
            {
                ClsWidgetsCollection objGetAll = ClsWidgetsCollection.GetAllWidgets();
                foreach (ClsWidgets wid in objGetAll)
                {
                    if (wid.ModuleTitle.Contains("Report-"))
                    {
                        TabItem tbItemRpt = new TabItem();
                        tbItemRpt.Height = 40;
                        tbItemRpt.Header = wid.ModuleTitle.Replace("Report-", "").Trim();


                        try
                        {
                            #region Downloading ZipFile

                            string filename = wid.ZipFile;
                            Uri u = new Uri(VMuktiAPI.VMuktiInfo.ZipFileDownloadLink + filename);
                            if (!Directory.Exists(AppDomain.CurrentDomain.BaseDirectory+ "Zip Files"))
                            {
                                Directory.CreateDirectory(AppDomain.CurrentDomain.BaseDirectory + "Zip Files");
                            }
                            string destination = AppDomain.CurrentDomain.BaseDirectory + "Zip Files";
                            if (!File.Exists(destination + "\\" + filename))
                            {
                                WebClient wc = new WebClient();
                                wc.DownloadFile(u, destination + "\\" + filename);
                            }

                            #endregion

                            #region Extracting

                            string strModPath = AppDomain.CurrentDomain.BaseDirectory + "Modules";
                            VMukti.ZipUnzip.Zip.FastZip fz = new VMukti.ZipUnzip.Zip.FastZip();
                            if (!Directory.Exists(strModPath + "\\" + filename.Split('.')[0]))
                            {
                                fz.ExtractZip(destination + "\\" + filename, strModPath, null);
                            }

                            #endregion

                            #region Loading ReferencedAssemblies

                            DirectoryInfo dirinfomodule = new DirectoryInfo(strModPath + "\\" + wid.ZipFile.Split('.')[0]);
                            ShowDirectory(dirinfomodule);


                            for (int j = 0; j < al.Count; j++)
                            {
                                string[] arraysplit = al[j].ToString().Split('\\');
                                if (arraysplit[arraysplit.Length - 1].ToString() == wid.AssemblyFile)
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

                                    for (int k = 0; k < t1.Length; k++)
                                    {
                                        if (t1[k].Name == wid.ClassName)
                                        {
                                            try
                                            {
                                                object[] objArg = new object[1];
                                                objArg[0] = new ModulePermissions[] { ModulePermissions.Add, ModulePermissions.Delete, ModulePermissions.Edit, ModulePermissions.View };

                                                object obj1 = Activator.CreateInstance(t1[k], objArg);


                                                tbItemRpt.Content = obj1;                                                
                                            }
                                            catch (Exception ex)
                                            {
                                                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "CreatingObject--7", "Controls\\VMuktiGrid\\Grid\\ctlPOD.xaml.cs");
                                                
                                            }

                                        }
                                    }
                                    #endregion
                                }
                            }
                            #endregion
                           
                        }
                        catch (Exception ex)
                        {
                            VMuktiHelper.ExceptionHandler(ex, "DBReports()--creating object", "ctlDashBoardContainer.xaml.cs");
                        }                       
                        tbItemRpt.Tag = wid;
                        tbCnt.Items.Add(tbItemRpt);                     
                    }
                }
            }

            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "DBReports", "ctlDashBoardContainer.xaml.cs");
            }
        }

        public void ShowDirectory(DirectoryInfo dir)
        {
            try
            {
                foreach (FileInfo file in dir.GetFiles())
                {
                    int hj = al.Add(file.FullName);
                }
                foreach (DirectoryInfo subDir in dir.GetDirectories())
                {
                    ShowDirectory(subDir);
                }
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "DBReports", "ctlDashBoardContainer.xaml.cs");
            }
        }
      


    }
}
