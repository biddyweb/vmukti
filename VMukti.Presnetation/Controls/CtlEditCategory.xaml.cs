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
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using VMukti.Business;

namespace VMukti.Presentation.Controls
{
    /// <summary>
    /// Interaction logic for EditCategory.xaml
    /// </summary>
    public partial class CtlEditCategory : UserControl
    {
        ClsCategoryCollection objclsCategory = null;
        int varState = 0;
        int varID = 0;
        public CtlEditCategory()
        {
            InitializeComponent();
            fncsetGrid();
            btnSave.Click += new RoutedEventHandler(btnSave_Click);
            btnCancel.Click += new RoutedEventHandler(btnCancel_Click);
        }

        void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            txtName.Text = "";
        }

        void btnSave_Click(object sender, RoutedEventArgs e)
        {
            int ch = 0;
            int CatId = 0;
            string CatName = txtName.Text;
            ClsCategoryLogic objClsCategoryLogic = new ClsCategoryLogic();

            if (CatName.Trim()=="")
            {
                MessageBox.Show("Please Enter Category Name");
            }
            else
            {
                if (varState == 0)
                {
                    CatId = -1;
                }
                else if (varState == 1)
                {
                    CatId = varID;
                    varState = 0;
                }
                ch = objClsCategoryLogic.UpdateCategory(CatId, CatName);
                if (ch == 0)
                {
                    MessageBox.Show("Duplicate entry is not allowed");
                }
                else
                {
                    MessageBox.Show("Record is saved successfully");
                }
                fncsetGrid();
            }
        }

        private void fncsetGrid()
        {
            try
            {
                EditGrid.Cols = 2;
                EditGrid.CanDelete = true;
                EditGrid.CanEdit = true;

                EditGrid.Columns[0].Header = "ID";
                EditGrid.Columns[1].Header = "CategoryName";

                EditGrid.Columns[0].BindTo("CategoryId");
                EditGrid.Columns[1].BindTo("CategoryTitle");
                objclsCategory = ClsCategoryCollection.GetMainCategory(VMuktiAPI.VMuktiInfo.CurrentPeer.RoleID);
                EditGrid.Bind(objclsCategory);
            }
            catch
            {
            }
        }

        private void ctlGrid_btnEditClicked(int rowID)
        {
            varID = Convert.ToInt32(objclsCategory[rowID].CategoryId);

            txtName.Text = objclsCategory[rowID].CategoryTitle;
            varState = 1;
        }

        private void ctlGrid_btnDeleteClicked(int rowID)
        {
            varID = Convert.ToInt32(objclsCategory[rowID].CategoryId);
            MessageBoxResult r = System.Windows.MessageBox.Show("Do You Really Want To Delete This Record ?", "->Delete Script", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (r == MessageBoxResult.Yes)
            {
                 
                ClsCategoryLogic objClsCategoryLogic = new ClsCategoryLogic();
                objClsCategoryLogic.DeleteCategory(varID);
                System.Windows.MessageBox.Show("Record Deleted!!", "Delete Category", MessageBoxButton.OK, MessageBoxImage.Error);
                fncsetGrid();
            }
        }
    }
}
