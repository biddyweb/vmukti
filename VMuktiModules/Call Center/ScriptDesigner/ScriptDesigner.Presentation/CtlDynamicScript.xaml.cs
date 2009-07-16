using System;
using System.Collections;
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
using ScriptDesigner.Business;

namespace ScriptDesigner.Presentation
{
    /// <summary>
    /// Interaction logic for CtlDynamicScript.xaml
    /// </summary>
    public partial class CtlDynamicScript : System.Windows.Controls.UserControl
    {
        int CurrentQueCount = 0;
        int varTop;
        ClsQuestionCollectionR objQueCollection;
        string varType;
        double varHeight;

        public CtlDynamicScript()
        {
            InitializeComponent();
            btnScript.Click += new RoutedEventHandler(btnScript_Click);

            Button btnYes = new Button();
            btnYes.Content = "Next";
            btnYes.Height = 30;
            btnYes.Width = 50;
            btnYes.Click += new RoutedEventHandler(btnYes_Click);
            Canvas.SetLeft(btnYes, 50);
            Canvas.SetTop(btnYes, 400);
            cnvMain.Children.Add(btnYes);

        }

        void btnScript_Click(object sender, RoutedEventArgs e)
        {
            ClsQuestionCollectionR objQueCollection = ClsQuestionCollectionR.GetAll(int.Parse(txtScript.Text));
            if (objQueCollection.Count > 0)
            {
                FncSetInnerCanvas();
            }
        }

        void FncSetInnerCanvas()
        {
            //cnvPaint.Height = 280;
            //cnvPaint.Width = 700;

            Label lblHeader = new Label();
            lblHeader.Content = objQueCollection[CurrentQueCount].QuestionName;
            lblHeader.SizeChanged += new SizeChangedEventHandler(lblQuestion_SizeChanged);
            lblHeader.FontSize = 19;
            cnvPaint.Children.Add(lblHeader);

            Label lblQuestion = new Label();
            lblQuestion.Content = objQueCollection[CurrentQueCount].QuestionText;
            lblQuestion.FontSize = 17;
            lblQuestion.SizeChanged += new SizeChangedEventHandler(lblQuestion_SizeChanged);

            Canvas.SetTop(lblQuestion, 60);
            cnvPaint.Children.Add(lblQuestion);

            ClsOptionCollection objOptCollection = ClsOptionCollection.GetAll(objQueCollection[CurrentQueCount].ID);

            varType = objQueCollection[CurrentQueCount].Category;

            if (varType == "CheckBox")
            {
                for (int i = 0; i < objOptCollection.Count; i++)
                {
                    CheckBox chk = new CheckBox();
                    chk.Content = objOptCollection[i].Options;
                    chk.Tag = objOptCollection[i].ActionQueueID.ToString() + "," + objOptCollection[i].ID.ToString();
                    chk.SizeChanged += new SizeChangedEventHandler(lblQuestion_SizeChanged);
                    chk.Height = 18;
                    chk.FontSize = 14;
                    Canvas.SetLeft(chk, 25);
                    varTop = 120 + (30 * i);
                    Canvas.SetTop(chk, varTop);
                    cnvPaint.Children.Add(chk);
                }
            }
            else if (varType == "RadioButton")
            {
                for (int i = 0; i < objOptCollection.Count; i++)
                {
                    RadioButton chk = new RadioButton();
                    chk.Content = objOptCollection[i].Options;
                    chk.Tag = objOptCollection[i].ActionQueueID.ToString() + "," + objOptCollection[i].ID.ToString();
                    chk.SizeChanged += new SizeChangedEventHandler(lblQuestion_SizeChanged);
                    chk.Height = 18;
                    chk.FontSize = 14;
                    Canvas.SetLeft(chk, 25);
                    varTop = 120 + (30 * i);
                    Canvas.SetTop(chk, varTop);
                    cnvPaint.Children.Add(chk);
                }
            }

            cnvPaint.Height = varTop + 50;
            sv.ScrollToTop();
        }

        void lblQuestion_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (varHeight < 70 + e.NewSize.Width)
            {
                cnvPaint.Width = 70 + e.NewSize.Width;
                varHeight = 70 + e.NewSize.Width;
            }
        }

        void btnYes_Click(object sender, RoutedEventArgs e)
        {
            ClsQuestionCollectionR objQueCollection = ClsQuestionCollectionR.GetAll(int.Parse(txtScript.Text));

            if (((Button)sender).Tag.ToString().ToLower() == "radiobutton")
            {
                foreach (object r in cnvPaint.Children)
                {
                    if (r.GetType() == typeof(RadioButton))
                    {
                        if (((RadioButton)r).IsChecked == true)
                        {
                            string[] str = ((RadioButton)r).Tag.ToString().Split(',');
                            CurrentQueCount = int.Parse(str[0]);
                            int optID = int.Parse(str[1]);
                            //MessageBox.Show("Option ID = " + optID.ToString());

                            //ClsAnswerR objAns = new ClsAnswerR();
                            //objAns.CallID = int.Parse(txtCall.Text.ToString());
                            //objAns.QusOptionID = int.Parse(optID.ToString());
                            //objAns.Save();

                            for (int k = 0; k < objQueCollection.Count; k++)
                            {
                                if (objQueCollection[k].ID == CurrentQueCount)
                                {
                                    //this.NavigationService.Navigate(new Uri(objQueCollection[k].QuestionName + ".Xaml" , UriKind.RelativeOrAbsolute));
                                    break;
                                }
                            }

                        }
                    }
                }
            }

            else if (varType == "CheckBox")
            {
                foreach (object r in cnvPaint.Children)
                {
                    if (r.GetType() == typeof(CheckBox))
                    {
                        if (((CheckBox)r).IsChecked == true)
                        {
                            string[] str = ((CheckBox)r).Tag.ToString().Split(',');
                            CurrentQueCount = int.Parse(str[0]);
                            int optID = int.Parse(str[1]);
                            //MessageBox.Show("Option ID = " + optID.ToString());

                            //ClsAnswerR objAns = new ClsAnswerR();
                            //objAns.CallID = int.Parse(txtCall.Text.ToString());
                            //objAns.QusOptionID = int.Parse(optID.ToString());
                            //objAns.Save();

                            for (int k = 0; k < objQueCollection.Count; k++)
                            {
                                if (objQueCollection[k].ID == CurrentQueCount)
                                {
                                    //this.NavigationService.Navigate(new Uri(objQueCollection[k].QuestionName + ".Xaml", UriKind.RelativeOrAbsolute));
                                    break;
                                }
                            }
                        }
                    }
                }
            }

            else if (((Button)sender).Tag.ToString().ToLower() == "listbox")
            {
                foreach (object r in cnvPaint.Children)
                {
                    if (r.GetType() == typeof(ListBox))
                    {
                        for (int i = 0; i < ((ListBox)r).Items.Count; i++)
                        {
                            if (((ListBoxItem)((ListBox)r).Items[i]).IsSelected == true)
                            {
                                string[] str = ((ListBoxItem)r).Tag.ToString().Split(',');
                                int CurrentQueCount = int.Parse(str[0]);
                                int optID = int.Parse(str[1]);
                                //MessageBox.Show("Option ID = " + optID.ToString());

                                //ClsAnswerR objAns = new ClsAnswerR();
                                //objAns.CallID = int.Parse(txtCall.Text.ToString());
                                //objAns.QusOptionID = int.Parse(optID.ToString());
                                //objAns.Save();

                                for (int k = 0; k < objQueCollection.Count; k++)
                                {
                                    if (objQueCollection[k].ID == CurrentQueCount)
                                    {
                                        //this.NavigationService.Navigate(new Uri(objQueCollection[k].QuestionName + ".Xaml", UriKind.RelativeOrAbsolute));
                                        break;
                                    }
                                }
                            }
                        }
                    }
                }
            }

            else if (((Button)sender).Tag.ToString().ToLower() == "combobox")
            {
                foreach (object r in cnvPaint.Children)
                {
                    if (r.GetType() == typeof(ComboBox))
                    {
                        for (int i = 0; i < ((ComboBox)r).Items.Count; i++)
                        {
                            if (((ComboBoxItem)((ComboBox)r).Items[i]).IsSelected == true)
                            {
                                string[] str = ((ComboBoxItem)r).Tag.ToString().Split(',');
                                int CurrentQueCount = int.Parse(str[0]);
                                int optID = int.Parse(str[1]);
                                //MessageBox.Show("Option ID = " + optID.ToString());

                                //ClsAnswerR objAns = new ClsAnswerR();
                                //objAns.CallID = int.Parse(txtCall.Text.ToString());
                                //objAns.QusOptionID = int.Parse(optID.ToString());
                                //objAns.Save();

                                for (int k = 0; k < objQueCollection.Count; k++)
                                {
                                    if (objQueCollection[k].ID == CurrentQueCount)
                                    {
                                        //this.NavigationService.Navigate(new Uri(objQueCollection[k].QuestionName + ".Xaml", UriKind.RelativeOrAbsolute));
                                        break;
                                    }
                                }
                            }
                        }
                    }
                }
            }

        }

        private void btnScript_Click_1(object sender, RoutedEventArgs e)
        {

        }
    }
}
