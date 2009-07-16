using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace VMukti.CtlGrid.Presentation
{
    /// <summary>
    /// Interaction logic for ctlGrid.xaml
    /// </summary>
    public partial class ctlGrid : System.Windows.Controls.UserControl
    {
        private int _Cols = 0;
        private List<ClsColumns> _Columns = new List<ClsColumns>();
        //private  VMukti.CtlGrid.Presentation.BaseCollection<object> _BindingCollection = null;
        private Type _ObjType = null;
        private bool _CanEdit, _CanDelete = false;

        public delegate void ButtonClicked(int rowID);
        public event ButtonClicked btnEditClicked = null;
        public event ButtonClicked btnDeleteClicked = null;

        public int Cols
        {
            get { return _Cols; }
            set
            {
                _Cols = value;
                _Columns.Clear();

                for (int i = 0; i < value; i++)
                {
                    _Columns.Add(new ClsColumns());
                }
            }
        }

        public bool CanEdit
        {
            get { return _CanEdit; }
            set { _CanEdit = value; }
        }

        public bool CanDelete
        {
            get { return _CanDelete; }
            set { _CanDelete = value; }
        }

        public List<VMukti.CtlGrid.Presentation.ClsColumns> Columns
        {
            get { return _Columns; }
        }

        public ctlGrid()
        {
            InitializeComponent();
        }

        public void ClearGrid()
        {
            grdControl.RowDefinitions.Clear();
            grdControl.ColumnDefinitions.Clear();
            grdControl.Children.Clear();
        }

        public void Bind(object BindingCollection)
        {
            ClearGrid();
            try
            {
                if (_CanEdit && !_CanDelete || !_CanEdit && _CanDelete)
                {
                    for (int col = 0; col < _Cols + 1; col++)
                    {
                        grdControl.ColumnDefinitions.Add(new ColumnDefinition());
                        //grdControl.ColumnDefinitions.Add(new ColumnDefinition());
                    }
                }
                else if (_CanEdit && _CanDelete)
                {
                    for (int col = 0; col < _Cols + 2; col++)
                    {
                        grdControl.ColumnDefinitions.Add(new ColumnDefinition());
                        //grdControl.ColumnDefinitions.Add(new ColumnDefinition());
                    }
                }
                else
                {
                    for (int col = 0; col < _Cols; col++)
                    {
                        grdControl.ColumnDefinitions.Add(new ColumnDefinition());
                        //grdControl.ColumnDefinitions.Add(new ColumnDefinition());
                    }
                }

                // Adding Column Headers //
                for (int col = 0; col < _Cols; col++)
                {
                    Label lblHeader = new Label();
                    lblHeader.Content = _Columns[col].Header;
                    lblHeader.BorderBrush = (Brush)this.Resources["GridNormalBorderBrush"];
                    //lblHeader.BorderBrush = Brushes.Black;
                    lblHeader.BorderThickness = new Thickness(1, 0, 0, 1);
                    lblHeader.Foreground = (Brush)this.Resources["GridNormalBrush"];
                    lblHeader.Background = (Brush)this.Resources["GridLightDarkBrush"];
                    lblHeader.FontWeight = FontWeights.Bold;
                    //lblHeader.Padding = new System.Windows.Forms.Padding(10, 10, 10, 10);
                    grdControl.Children.Add(lblHeader);
                    Grid.SetColumn(lblHeader, col);
                    Grid.SetRow(lblHeader, 0);

                    //GridSplitter g = new GridSplitter();
                    //g.HorizontalAlignment = HorizontalAlignment.Center;
                    //g.VerticalAlignment = VerticalAlignment.Stretch;
                    //g.Background = Brushes.Red;
                    //g.ShowsPreview = true;
                    //g.Width = 5;
                    //g.SetValue(Grid.ColumnProperty, ((col*2)+1));
                    //g.SetValue(Grid.RowProperty, 0);
                    //grdControl.Children.Add(g);

                }

                if (grdControl.ColumnDefinitions.Count == _Cols + 1)
                {
                    Label lblHeader = new Label();
                    
                    //No Header //

                    //if (_CanDelete)
                    //    lblHeader.Content = "Delete";
                    //else
                    //    lblHeader.Content = "Edit";

                    //No Header //

                    lblHeader.BorderBrush = (Brush)this.Resources["GridNormalBorderBrush"];
                    //lblHeader.BorderBrush = Brushes.Black;

                    lblHeader.BorderThickness = new Thickness(1, 0, 0, 1);
                    lblHeader.Foreground = (Brush)this.Resources["GridNormalBrush"];
                    lblHeader.Background = (Brush)this.Resources["GridLightDarkBrush"];
                    lblHeader.FontWeight = FontWeights.Bold;
                    //lblHeader.Padding = new System.Windows.Forms.Padding(10, 10, 10, 10);
                    grdControl.Children.Add(lblHeader);
                    Grid.SetColumn(lblHeader, _Cols);
                    Grid.SetRow(lblHeader, 0);
                }

                if (grdControl.ColumnDefinitions.Count == _Cols + 2)
                {
                    Label lblHeader1 = new Label();

                    //No Header //

                    //lblHeader1.Content = "Edit";

                    //No Header //

                    lblHeader1.BorderBrush = (Brush)this.Resources["GridNormalBorderBrush"];
                    //lblHeader1.BorderBrush = Brushes.Black;
                    lblHeader1.BorderThickness = new Thickness(1, 0, 0, 1);
                    lblHeader1.Foreground = (Brush)this.Resources["GridNormalBrush"];
                    lblHeader1.Background = (Brush)this.Resources["GridLightDarkBrush"];
                    lblHeader1.FontWeight = FontWeights.Bold;
                    //lblHeader1.Padding = new System.Windows.Forms.Padding(10, 10, 10, 10);
                    grdControl.Children.Add(lblHeader1);
                    Grid.SetColumn(lblHeader1, _Cols);
                    Grid.SetRow(lblHeader1, 0);

                    Label lblHeader2 = new Label();

                    //No Header //

                    //lblHeader2.Content = "Delete";

                    //No Header //

                    lblHeader2.BorderBrush = (Brush)this.Resources["GridNormalBorderBrush"];
                    //lblHeader2.BorderBrush = Brushes.Black;
                    lblHeader2.BorderThickness = new Thickness(1, 0, 0, 1);
                    lblHeader2.Foreground = (Brush)this.Resources["GridNormalBrush"];
                    lblHeader2.Background = (Brush)this.Resources["GridLightDarkBrush"];
                    lblHeader2.FontWeight = FontWeights.Bold;
                    //lblHeader2.Padding = new System.Windows.Forms.Padding(10, 10, 10, 10);
                    grdControl.Children.Add(lblHeader2);
                    Grid.SetColumn(lblHeader2, _Cols + 1);
                    Grid.SetRow(lblHeader2, 0);
                }

                System.Reflection.PropertyInfo[] myProps = BindingCollection.GetType().GetProperties();
                for (int i = 0; i < myProps.Length; i++)
                {
                    if (myProps[i].PropertyType.BaseType.ToString().Split('.')[myProps[i].PropertyType.BaseType.ToString().Split('.').Length - 1].ToString() == "ClsBaseObject")
                    {
                        try
                        {
                            grdControl.RowDefinitions.Add(new RowDefinition());
                            grdControl.RowDefinitions[grdControl.RowDefinitions.Count - 1].Height = new GridLength(25);
                            int rows = 1;
                            while (true)
                            {
                                object[] indexArgs = { rows - 1 };
                                System.Reflection.PropertyInfo[] mySubProps = myProps[i].GetValue(BindingCollection, indexArgs).GetType().GetProperties();
                                grdControl.RowDefinitions.Add(new RowDefinition());
                                grdControl.RowDefinitions[grdControl.RowDefinitions.Count - 1].Height = new GridLength(25);

                                for (int cols = 0; cols < _Columns.Count; cols++)
                                {
                                    for (int props = 0; props < mySubProps.Length; props++)
                                    {
                                        if (_Columns[cols].BindedObject == mySubProps[props].Name)
                                        {
                                            if (!_Columns[cols].IsIcon)
                                            {
                                                Label lblData = new Label();
                                                lblData.Content = mySubProps[props].GetValue(myProps[i].GetValue(BindingCollection, indexArgs), null).ToString();
                                                lblData.Tag = rows;
                                                //lblData.Padding = new System.Windows.Forms.Padding(10, 10, 10, 10);
                                                lblData.BorderBrush = (Brush)this.Resources["GridNormalBorderBrush"];
                                                //lblData.BorderBrush = Brushes.Black;
                                                lblData.Background = (Brush)this.Resources["GridLightBrush"];
                                                lblData.BorderThickness = new Thickness(0.5, 0, 0, 1.5);
                                                //if (lblData.Content.ToString().ToUpper() == "TRUE")
                                                //    lblData.Foreground = Brushes.Green;
                                                //else if (lblData.Content.ToString().ToUpper() == "FALSE")
                                                //    lblData.Foreground = Brushes.Red;
                                                //else
                                                lblData.Foreground = (Brush)this.Resources["GridNormalBrush"];
                                                lblData.MouseDown += new MouseButtonEventHandler(lblData_MouseDown);
                                                grdControl.Children.Add(lblData);
                                                Grid.SetColumn(lblData, cols);
                                                Grid.SetRow(lblData, rows);
                                            }
                                            else
                                            {

                                                Label lblData = new Label();
                                                lblData.Content = mySubProps[props].GetValue(myProps[i].GetValue(BindingCollection, indexArgs), null).ToString();
                                                lblData.Tag = rows;
                                                lblData.BorderBrush = (Brush)this.Resources["GridLightDarkBrush"];
                                                lblData.Background = (Brush)this.Resources["GridLightBrush"];
                                                lblData.BorderThickness = new Thickness(0.5, 0, 0, 1.5);
                                                lblData.Foreground = Brushes.Transparent;
                                                lblData.MouseDown += new MouseButtonEventHandler(lblData_MouseDown);
                                                grdControl.Children.Add(lblData);
                                                Grid.SetColumn(lblData, cols);
                                                Grid.SetRow(lblData, rows);

                                                Canvas cnvData = new Canvas();
                                                //lblData.Content = mySubProps[props].GetValue(myProps[i].GetValue(BindingCollection, indexArgs), null).ToString().ToUpper();
                                                cnvData.Height = 20;
                                                cnvData.Width = 20;
                                                cnvData.Tag = rows;
                                                if (mySubProps[props].GetValue(myProps[i].GetValue(BindingCollection, indexArgs), null).ToString().ToUpper() == "TRUE")
                                                {
                                                    cnvData.Background = (Brush)this.Resources["GreenSpot"];
                                                }
                                                else if (mySubProps[props].GetValue(myProps[i].GetValue(BindingCollection, indexArgs), null).ToString().ToUpper() == "FALSE")
                                                {
                                                    cnvData.Background = (Brush)this.Resources["RedSpot"];
                                                }
                                                //cnvData.MouseDown += new MouseButtonEventHandler(lblData_MouseDown);
                                                grdControl.Children.Add(cnvData);
                                                Grid.SetColumn(cnvData, cols);
                                                Grid.SetRow(cnvData, rows);



                                                //Label lblData = new Label();
                                                //lblData.Tag = rows;
                                                //lblData.Height = 30;
                                                //lblData.Width = 30;
                                                //if (mySubProps[props].GetValue(myProps[i].GetValue(BindingCollection, indexArgs), null).ToString().ToUpper() == "TRUE")
                                                //{
                                                //   lblData.Background = (Brush)this.Resources["GreenSpot"];
                                                //}
                                                //else if (mySubProps[props].GetValue(myProps[i].GetValue(BindingCollection, indexArgs), null).ToString().ToUpper() == "FALSE")
                                                //{
                                                //    lblData.Background = (Brush)this.Resources["RedSpot"];
                                                //}

                                                //lblData.MouseDown += new MouseButtonEventHandler(lblData_MouseDown);
                                                //grdControl.Children.Add(lblData);
                                                //Grid.SetColumn(lblData, cols);
                                                //Grid.SetRow(lblData, rows);

                                                //Image img = new Image();
                                                //if (mySubProps[props].GetValue(myProps[i].GetValue(BindingCollection, indexArgs), null).ToString() == "True")
                                                //{
                                                //    img = (System.Windows.Controls.Image) _Columns[cols].TrueImage;
                                                //}
                                                //else
                                                //{
                                                //    img = (System.Windows.Controls.Image)_Columns[cols].FalseImage;
                                                //}
                                                ////chkData.BorderBrush = (Brush)this.Resources["DefaultedBorderBrush"];
                                                ////chkData.BorderThickness = new Thickness(3, 0, 0, 3);
                                                //grdControl.Children.Add(img);
                                                //Grid.SetColumn(chkData, cols);
                                                //Grid.SetRow(chkData, rows);
                                            }
                                            break;
                                        }
                                    }
                                }

                                if (_CanEdit && !_CanDelete)
                                {
                                    Button btnEdit = new Button();
                                    btnEdit.Content = "Edit";
                                    //btnEdit.Width = 50;
                                    btnEdit.Tag = rows - 1;
                                    btnEdit.Click += new RoutedEventHandler(btnEdit_Click);
                                    grdControl.Children.Add(btnEdit);
                                    Grid.SetColumn(btnEdit, _Cols + 1);
                                    Grid.SetRow(btnEdit, rows);
                                }
                                else if (!_CanEdit && _CanDelete)
                                {
                                    Button btnDelete = new Button();
                                    btnDelete.Content = "Delete";
                                    //btnDelete.Width = 50;
                                    btnDelete.Tag = rows - 1;
                                    btnDelete.Click += new RoutedEventHandler(btnDelete_Click);
                                    grdControl.Children.Add(btnDelete);
                                    Grid.SetColumn(btnDelete, _Cols + 1);
                                    Grid.SetRow(btnDelete, rows);
                                }
                                else if (_CanEdit && _CanDelete)
                                {
                                    Button btnEdit = new Button();
                                    btnEdit.Content = "Edit";
                                    //btnEdit.Width = 50;
                                    btnEdit.Tag = rows - 1;
                                    btnEdit.Click += new RoutedEventHandler(btnEdit_Click);
                                    grdControl.Children.Add(btnEdit);
                                    Grid.SetColumn(btnEdit, _Cols);
                                    Grid.SetRow(btnEdit, rows);


                                    Button btnDelete = new Button();
                                    btnDelete.Content = "Delete";
                                    //btnDelete.Width = 50;
                                    btnDelete.Tag = rows - 1;
                                    btnDelete.Click += new RoutedEventHandler(btnDelete_Click);
                                    grdControl.Children.Add(btnDelete);
                                    Grid.SetColumn(btnDelete, _Cols + 1);
                                    Grid.SetRow(btnDelete, rows);
                                }
                                rows++;
                            }
                        }
                        catch (Exception exp)
                        {
                            //MessageBox.Show(exp.Message);
                        }
                    }
                }
            }
            catch (Exception exp)
            {
                MessageBox.Show(exp.Message);
            }
        }

        void lblData_MouseDown(object sender, MouseButtonEventArgs e)
        {
            
                foreach(object l in grdControl.Children)
                {
                    if(l.GetType() == typeof(Label))
                    {
                        try
                        {
                            if (((Label)l).Tag.ToString() == ((Label)sender).Tag.ToString())
                            {
                                ((Label)l).Background = Brushes.LightCyan;
                            }
                            else
                            {
                                ((Label)l).Background = (Brush)this.Resources["GridLightBrush"];
                            }
                        }
                        catch (Exception exp)
                        { 
                        }
                    }
                    
                }

            ((Label)sender).Background = Brushes.LightBlue;
        }

        void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (btnDeleteClicked != null)
            {
                btnDeleteClicked(int.Parse(((Button)sender).Tag.ToString()));
            }
        }

        void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            if (btnDeleteClicked != null)
            {
                btnEditClicked(int.Parse(((Button)sender).Tag.ToString()));
            }
        }
    }
}
