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
using System.IO;
using System.Windows.Controls;
using System.Windows;
using System.Text;

namespace Chat.Presentation
{
    public partial class ctlViewHistory : UserControl
    {
        #region Variables

        public List<string> ListFolders = new List<string>();
        public delegate void DelClose() ;
        public event DelClose EntClose;
        TreeViewItem rootItem = new TreeViewItem();
        TreeViewItem childrenItem = null;
        List <TreeViewItem> childrenItemList = new List<TreeViewItem>();
        public string path;
       

        #endregion

        #region Constructor

        public ctlViewHistory()
        {
            try
            {
                InitializeComponent();

                btnClose.Click += new System.Windows.RoutedEventHandler(btnClose_Click);

                if (Directory.Exists(Environment.GetFolderPath(System.Environment.SpecialFolder.Personal) + @"\VMukti\Chat History\Logs for " + VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName))
                {
                    path = Environment.GetFolderPath(System.Environment.SpecialFolder.Personal) + @"\VMukti\Chat History\Logs for " + VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName;

                    tvHistory.Visibility = Visibility.Visible;
                    rtbMessages.Visibility = Visibility.Visible;
                    tbMessage.Visibility = Visibility.Hidden;
                }
                else
                {
                    tvHistory.Visibility = Visibility.Hidden;
                    rtbMessages.Visibility = Visibility.Hidden;
                    tbMessage.Visibility = Visibility.Visible;
                    return;
                }

                rootItem.Tag = "Logs for " + VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName;
                rootItem.Header = "Logs for " + VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName;
                LoadFolders(path);
                PopulateTreeView();
                tvHistory.SelectedItemChanged += new System.Windows.RoutedPropertyChangedEventHandler<object>(tvHistory_SelectedItemChanged);
                this.Loaded += new RoutedEventHandler(ctlViewHistory_Loaded);
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "ctlViewHistory", "ctlViewHistory.xaml.cs");
            }
        }


        #endregion

        #region UI Event Handlers

        void btnClose_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            try
            {
                if (EntClose != null)
                {
                    EntClose();
                }
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "btnClose_Click", "ctlViewHistory.xaml.cs");
            }
        }

        void tvHistory_SelectedItemChanged(object sender, System.Windows.RoutedPropertyChangedEventArgs<object> e)
        {
            try
            {
                if (((TreeViewItem)(tvHistory.SelectedItem)).Tag.ToString().EndsWith("txt"))
                {
                    string file = ((TreeViewItem)(tvHistory.SelectedItem)).Tag.ToString();
                    StreamReader fileReader = new StreamReader(file);
                    string fileContent = fileReader.ReadToEnd();
                    fileReader.Close();
                    rtbMessages.SelectAll();
                    rtbMessages.Selection.Text = String.Empty;
                    rtbMessages.AppendText(fileContent);
                }
                else
                {
                    rtbMessages.SelectAll();
                    rtbMessages.Selection.Text = String.Empty;
                }
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "tvHistory_SelectedItemChanged", "ctlViewHistory.xaml.cs");
            }
        }

        #endregion

        #region Methods

        public void LoadFolders(string folderLocation)
        {
            DirectoryInfo dir = null;

            try
            {
                dir = new DirectoryInfo(folderLocation);

                FileSystemInfo[] info = dir.GetFileSystemInfos();

                foreach (FileSystemInfo fsi in info)
                {
                    if (fsi is DirectoryInfo)
                    {
                        ListFolders.Add(fsi.Name);

                        childrenItem = new TreeViewItem();

                        childrenItem.Tag = fsi.FullName;
                        childrenItem.Header = fsi.Name;

                        LoadFiles(fsi.FullName);

                        childrenItemList.Add(childrenItem);
                    }
                }
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "LoadFolders", "ctlViewHistory.xaml.cs");
            }
        }

        public void LoadFiles(string folderPath)
        {
            try
            {
                DirectoryInfo directory = new DirectoryInfo(folderPath);
                FileInfo[] fileList = directory.GetFiles("*.txt");
                foreach (FileInfo file in fileList)
                {
                    TreeViewItem item = new TreeViewItem();
                    item.Tag = file.FullName;
                    item.Header = file.Name;
                    childrenItem.Items.Add(item);
                }
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "LoadFiles", "ctlViewHistory.xaml.cs");
            }
        }

        public void PopulateTreeView()
        {
            try
            {
                foreach (TreeViewItem item in childrenItemList)
                {
                    rootItem.Items.Add(item);
                }
                tvHistory.Items.Add(rootItem);
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "PopulateTreeView", "ctlViewHistory.xaml.cs");
            }
        }

        #endregion

       


        #region SizeChanged

        void ctlViewHistory_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                if (((Grid)(this.Parent)) != null)
                {
                    this.Width = ((Grid)(this.Parent)).ActualWidth;
                }
                ((Grid)(this.Parent)).SizeChanged += new SizeChangedEventHandler(ctlViewHistory_SizeChanged);
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "ctlViewHistory_Loaded", "ctlViewHistory.xaml.cs");
            }
        }

        void ctlViewHistory_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            try
            {
                if (((Grid)(this.Parent)) != null)
                {
                    this.Width = ((Grid)(this.Parent)).ActualWidth;
                }
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "ctlViewHistory_SizeChanged", "ctlViewHistory.xaml.cs");
            }
        }
        #endregion
    }
}