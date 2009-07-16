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
using Calender.Business;
using System.Data; 
using Calender.Business.Service;
using VMuktiAPI;
using System.Collections;

namespace Calender.Presentation
{
    public partial class ctlConfCalander : UserControl
    {
        //public static StringBuilder sb1;
        Int64 userID = 10;
        Int64 CONFERENCEID = -1;
        string strTime = "";
        int Hour = 12;
        string Minute = "00";
        Label lbl = new Label();
        Label[,] glabel = new Label[48,2];
        int selectStart = -1, selectEnd = -1;
        string startTime = "" , endTime = "";
        double popupPositionX= 0.0, popupPositionY = 0.0;
        Label currLabel = null;
        string sTime = "", eTime = "";
        string sTimeShort = "", eTimeShort = "";
        Int64 ConferenceID = -1;
        ArrayList additionalLabels = new ArrayList();
        double[] rowLabels = new double[48];
        ArrayList alLabels = new ArrayList();
        //StringBuilder sb1 = CreateTressInfo();

   //     int LabelColor = 1;
        string etime2 = "";
        bool flag = false;
        double lblMaxWidth = 430;

        Brush selectStartColor;
        SolidColorBrush scbDark = new SolidColorBrush();
        SolidColorBrush scbLight = new SolidColorBrush();
        SolidColorBrush backgroundColor = new SolidColorBrush();
        SolidColorBrush selectionColor = new SolidColorBrush();
        SolidColorBrush timeBackgroundColor = new SolidColorBrush();
        SolidColorBrush popupMenuColor = new SolidColorBrush();

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

        public ctlConfCalander()
        {
            try
            {

                InitializeComponent();
                userID = VMuktiAPI.VMuktiInfo.CurrentPeer.ID;
                backgroundColor.Color = Color.FromRgb(250, 255, 255);
                selectionColor.Color = Color.FromRgb(196, 196, 196);
                selectionColor.Opacity = 0.3;

                timeBackgroundColor.Color = Color.FromRgb(108, 123, 139);
                popupMenuColor.Color = Color.FromRgb(245, 255, 255);
                //           popupMenuColor.Opacity = 0.5;
                rctPopup.Fill = popupMenuColor;
                rctPopupEdit.Fill = popupMenuColor;
                rctPopupJoin.Fill = popupMenuColor;
                plyDown.Fill = popupMenuColor;
                plyEditDown.Fill = popupMenuColor;
                plyEditUp.Fill = popupMenuColor;
                plyJoinDown.Fill = popupMenuColor;
                plyJoinUp.Fill = popupMenuColor;
                plyUp.Fill = popupMenuColor;

                //Creating a Basic grid, user can generate events by clicking on a label area
                FncCreateGrid();
                if (VMuktiAPI.VMuktiInfo.CurrentPeer.CurrPeerType == VMuktiAPI.PeerType.NodeWithHttp)
                {
                    clsMailDBClient.OpenMailDBClient();
                }
                btnOK.Click += new RoutedEventHandler(btnOK_Click);
                btnCancel.Click += new RoutedEventHandler(btnCancel_Click);
                mncCal.DateSelectionChanged += new VMukti.CtlDatePicker.Presentation.DateSelectionChangedEventHandler(mncCal_DateSelectionChanged);

                mncCal.SelectedDate = mncCal.VisibleMonth;
                string[] currentDateSplit = lblshow.Content.ToString().Split(' ');
                removeUnnecessaryLabelsFromGrid();

                //Getting the conference details with user's id and column value
                //getMyConferences(userID, currentDateSplit[0]);
                getMyConferences(userID, mncCal.SelectedDate.Value.ToString("dd-MMM-yyyy"));
            }
            catch (Exception exp)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(exp, "ctlConfCalander()", "ctlConfCalander.xaml.cs");
            }
        }

        void getMyConferences(Int64 userid, string date)
        {
            try
            {
                // Grid for getting the start time, end time, conference id, conference title                
                redrawGrid();
     //           LabelColor = 1;
                //System.Data.DataSet dsConference = ClsCalender.getAllConferences(userid, date);

                //foreach (DataRow drConference in dsConference.Tables[0].Rows)
                //{
                //    try
                //    {
                //        Int64 id = Int64.Parse(drConference["ID"].ToString());
                //        DateTime st = DateTime.Parse(drConference["StartDateTime"].ToString());
                //        DateTime et = DateTime.Parse(drConference["EndDateTime"].ToString());
                //        string purposeTmp = drConference["ConfTitle"].ToString();
                //        populateDataDayWise(st, et, id, purposeTmp);
                //    }
                //    catch (Exception e)
                //    {
                //        MessageBox.Show(e.Message);
                //    }
                //}

                DataSet dsMyConferences = ClsCalender.getMyConferences(userID);
                foreach (DataRow drMyConference in dsMyConferences.Tables[0].Rows)
                {
                    try
                    {
                        Int64 id = Int64.Parse(drMyConference["ConferenceID"].ToString());

                        DataSet dsConference1 = ClsCalender.getConference(id,date);
                        DataTable dtConference = dsConference1.Tables[0];
                        if (dtConference.Rows.Count != 0)
                        {
                            DataRow drConference = dtConference.Rows[0];
                            DateTime st = DateTime.Parse(drConference["StartDateTime"].ToString());
                            DateTime et = DateTime.Parse(drConference["EndDateTime"].ToString());
                            string purposeTmp = drConference["ConfTitle"].ToString();
                            populateDataDayWise(st, et, id, purposeTmp);
                        }
                    }
                    catch (Exception exp)
                    {
                        VMuktiAPI.VMuktiHelper.ExceptionHandler(exp, "getMyConferences()", "ctlConfCalander.xaml.cs");
                    }
                }


            }
            catch (Exception exp)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(exp, "ctlConfCalander()", "ctlConfCalander.xaml.cs");
            }
        }

        string dayofMonth;

        void mncCal_DateSelectionChanged(object sender, VMukti.CtlDatePicker.Presentation.DateSelectionChangedEventArgs e)
        {
            try
            {
                
                
                cnvPopupEdit.Visibility = Visibility.Collapsed;
                cnvPopup.Visibility = Visibility.Collapsed;
                if (mncCal.SelectedDate.HasValue)
                {
                    dayofMonth = mncCal.SelectedDate.Value.Day.ToString();
                    lblshow.Content = mncCal.SelectedDate.Value.ToString("dd-MMM-yyyy");
                    removeUnnecessaryLabelsFromGrid();
                    getMyConferences(userID, mncCal.SelectedDate.Value.ToString("dd-MMM-yyyy"));
                }
                else
                {
                    DateTime t = mncCal.VisibleMonth;
                    string[] datesplit = t.ToString().Split(' ');
                    string[] date1split = datesplit[0].Split('/');
                    date1split[1] = dayofMonth;
                    string dt = date1split[0] + "/" + date1split[1] + "/" + date1split[2] + " " + datesplit[1] + " " + datesplit[2];
                    t = DateTime.Parse(dt);

                    mncCal.SelectedDate = t;
                }
            }
            catch (Exception exp)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(exp, "mncCal_DateSelectionChanged()", "ctlConfCalander.xaml.cs");
            }
        
        }

        void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            cnvPopup.Visibility = Visibility.Collapsed;
        }

        void btnOK_Click(object sender, RoutedEventArgs e)
        {
            try
            {
            if (currLabel != null)
            {
             //   currLabel.Content = txtPurpose.Text;
                cnvPopup.Visibility = Visibility.Collapsed;
            }
            }
            catch (Exception exp)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(exp, "btnOK_Click()", "ctlConfCalander.xaml.cs");
            }
        }

        void redrawGrid()
        {
            try
            {
                Hour = 12;
                Minute = "00";
                int lblCountR = 0;
                for (int i = 0; i < 48; i++)
                {
                    rowLabels[i] = 2;
                    for (int j = 0; j < 2; j++)
                    {
                        glabel[i, j].Padding = new Thickness(5, 3, 0, 0);
                        glabel[i, j].BorderThickness = new Thickness(0.1);
                        glabel[i, j].BorderBrush = timeBackgroundColor;
                        glabel[i, j].Foreground = Brushes.White;
                        glabel[i, j].Width = lblMaxWidth;


                        if (j == 0)
                        {
                            if (i % 2 == 0 && i != 0)
                            {
                                if (Hour == 12)
                                    Hour = 0;
                                Hour++;
                            }

                            glabel[i, j].Background = timeBackgroundColor;
                            if (i < 24)
                            {
                                glabel[i, j].Content = Hour.ToString() + ":" + Minute.ToString() + " AM";
                            }
                            else
                            {
                                glabel[i, j].Content = Hour.ToString() + ":" + Minute.ToString() + " PM";
                            }

                        }
                        else
                        {

                            glabel[i, j].Content = "";
                            if (glabel[i, j].ToolTip != null)
                                glabel[i, j].ToolTip = null;
                            //         glabel[i, j].ToolTip = "";
                            if (i < 24)
                            {
                                glabel[i, j].Tag = Hour.ToString() + ":" + Minute.ToString() + ":AM:" + lblCountR;
                                lblCountR++;
                            }
                            else
                            {
                                glabel[i, j].Tag = Hour.ToString() + ":" + Minute.ToString() + ":PM:" + lblCountR;
                                lblCountR++;
                            }

                            glabel[i, j].Background = backgroundColor;
                            //             glabel[i, j].MouseDown += new MouseButtonEventHandler(l_MouseDown);
                            //             glabel[i, j].MouseUp += new MouseButtonEventHandler(l_MouseUp);
                            //             glabel[i, j].MouseEnter += new MouseEventHandler(l_MouseEnter);
                            ////                           glabel[i, j].MouseWheel += new MouseWheelEventHandler(l_MouseMove);

                        }

                        //       grdMain.Children.Add(glabel[i, j]);
                        ///      Grid.SetColumn(glabel[i, j], j);
                        //      Grid.SetRow(glabel[i, j], i);

                    }

                    if (Minute == "00")
                    {
                        Minute = "30";
                    }
                    else
                    {
                        Minute = "00";
                    }

                }
            }
            catch (Exception exp)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(exp, "redrawGrid()", "ctlConfCalander.xaml.cs");
            }
        }

        void FncCreateGrid()
        {
            try
            {
                grdMain.ColumnDefinitions.Add(new ColumnDefinition());
                grdMain.ColumnDefinitions.Add(new ColumnDefinition());
                grdMain.ColumnDefinitions[0].Width = new GridLength(60);
                grdMain.ColumnDefinitions[1].Width = new GridLength(lblMaxWidth);

                for (int i = 0; i < 48; i++)
                {
                    grdMain.RowDefinitions.Add(new RowDefinition());
                    grdMain.RowDefinitions[i].Height = new GridLength(23);
                    grdMain.Height += 23;
                }
                int lblCountR = 0;
                for (int i = 0; i < 48; i++)
                {
                    rowLabels[i] = 2;
                    for (int j = 0; j < 2; j++)
                    {
                        glabel[i, j] = new Label();
                        glabel[i, j].Padding = new Thickness(5, 3, 0, 0);
                        glabel[i,j].BorderThickness = new Thickness(0.1);
                        glabel[i, j].BorderBrush = timeBackgroundColor;
                        glabel[i, j].Foreground = Brushes.White;
                        glabel[i, j].Width = lblMaxWidth;

                        if (j == 0)
                        {
                            if (i % 2 == 0 && i != 0)
                            {
                                if (Hour == 12)
                                    Hour = 0;
                                Hour++;
                            }
                            
                            glabel[i, j].Background = timeBackgroundColor;
                            if (i < 24)
                            {
                                glabel[i, j].Content = Hour.ToString() + ":" + Minute.ToString() + " AM";
                            }
                            else
                            {
                                glabel[i, j].Content = Hour.ToString() + ":" + Minute.ToString() + " PM";
                            }
                        }
                        else
                        {
                            glabel[i, j].Content = "";
                   //         glabel[i, j].ToolTip = "";
                            if (i < 24)
                            {
                                glabel[i, j].Tag = Hour.ToString() + ":" + Minute.ToString() + ":AM:" + lblCountR;
                                lblCountR++;
                            }
                            else
                            {
                                glabel[i, j].Tag = Hour.ToString() + ":" + Minute.ToString() + ":PM:" + lblCountR;
                                lblCountR++;
                            }

                            glabel[i, j].Background = backgroundColor;
                            glabel[i, j].MouseDown += new MouseButtonEventHandler(l_MouseDown);
                            glabel[i, j].MouseUp += new MouseButtonEventHandler(l_MouseUp);
                            glabel[i, j].MouseEnter += new MouseEventHandler(l_MouseEnter);
                        }
                        grdMain.Children.Add(glabel[i, j]);
                        Grid.SetColumn(glabel[i, j], j);
                        Grid.SetRow(glabel[i, j], i);
                    }

                    if (Minute == "00")
                    {
                        Minute = "30";
                    }
                    else
                    {
                        Minute = "00";
                    }
                }
            }
            catch (Exception exp)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(exp, "FncCreateGrid()", "ctlConfCalander.xaml.cs");
            }
        }

        void l_MouseUp(object sender, MouseButtonEventArgs e)
        {
            try
            {
                string tag = "";

                tag = ((Label)sender).Tag.ToString();
                string[] str = tag.Split(':');
                selectEnd = int.Parse(str[3]);

                txtPurpose.Text = "";

                if (selectStart == selectEnd)
                {
                    if (selectStart != 47)
                        selectEnd = selectStart + 1;
                    //          else if (selectStart == 47)
                    //              selectEnd = selectStart;
                    //         else
                    //            selectEnd = selectStart + 1;

                    if (!((glabel[selectEnd, 1].Background == backgroundColor) || (glabel[selectEnd, 1].Background == selectionColor)))
                    {
                        if (selectStart != 0)
                        {
                            selectEnd = selectEnd - 1;
                        }

                    }
                    else if (selectStart == 47)
                    {
                        selectEnd = selectStart - 1;
                    }
                    if (glabel[selectEnd, 1].Background == backgroundColor && glabel[selectStart, 1].Background == selectionColor)
                    {
                        glabel[selectEnd, 1].Background = selectionColor;

                    }

                    /////////////////////////////////////////////////////////////////////////////////
                    tag = glabel[selectEnd, 1].Tag.ToString();
                    string[] str1 = tag.Split(':');
                    endTime = str1[0] + ":" + str1[1] + " " + str1[2];
                    eTime = str1[0] + ":" + str1[1] + ":00 " + str1[2];

                }

                if (!(((Label)sender).Background == selectionColor || ((Label)sender).Background == backgroundColor))
                {
                    txtEditPurpose.Content = ((Label)sender).ToolTip.ToString();
                    txtEditPurpose1.Content = ((Label)sender).ToolTip.ToString();
                }

                string dayOfWeek = mncCal.SelectedDate.Value.DayOfWeek.ToString();

                if (((Label)sender).Background == selectionColor)
                {
                    if (selectStart < selectEnd)
                    {
                        for (int k = 0; k < 48; k++)
                        {
                            if (glabel[k, 1].Background == selectionColor)
                            {
                                tag = glabel[k, 1].Tag.ToString();
                                string[] strTag = tag.Split(':');
                                selectEnd = int.Parse(strTag[3]);
                            }
                        }

                        tag = glabel[selectEnd, 1].Tag.ToString();
                        string[] strTag1 = tag.Split(':');
                        endTime = strTag1[0] + ":" + strTag1[1] + " " + strTag1[2];
                        eTime = strTag1[0] + ":" + strTag1[1] + ":00 " + strTag1[2];

                    }
                    else
                    {
                        bool isFirst = true;
                        for (int k = 0; k < 48; k++)
                        {
                            if (glabel[k, 1].Background == selectionColor && isFirst == true)
                            {
                                tag = glabel[k, 1].Tag.ToString();
                                string[] strTag = tag.Split(':');
                                selectEnd = int.Parse(strTag[3]);
                                isFirst = false;
                            }
                        }
                        tag = glabel[selectEnd, 1].Tag.ToString();
                        string[] strTag1 = tag.Split(':');
                        endTime = strTag1[0] + ":" + strTag1[1] + " " + strTag1[2];
                        eTime = strTag1[0] + ":" + strTag1[1] + ":00 " + strTag1[2];
                    }
                }
                else
                {
                    popupPositionX = e.GetPosition(cnvMain).X + 20.0;
                    popupPositionY = 23 * selectStart - 180;

                    if ((popupPositionX + cnvPopup.Width) > 500)
                        popupPositionX = 195;
                    //if (popupPositionY < -6)
                    //    popupPositionY = selectStart * 23 - 15;

                }

                if (selectStart > selectEnd)
                {
                    string tmp = startTime;
                    startTime = endTime;
                    endTime = tmp;

                    string tmp2 = sTime;
                    sTime = eTime;
                    eTime = tmp2;

                    popupPositionX = e.GetPosition(cnvMain).X + 20.0;
                    popupPositionY = 23 * selectStart - 180;

                    if ((popupPositionX + cnvPopup.Width) > 500)
                        popupPositionX = 195;
                    if (popupPositionY < -6)
                        popupPositionY = selectEnd * 23 - 15;

                }

                if (selectStart < 8 || selectEnd < 8)
                {

                    plyDown.Visibility = Visibility.Collapsed;
                    plyUp.Visibility = Visibility.Visible;

                    plyEditDown.Visibility = Visibility.Collapsed;
                    plyEditUp.Visibility = Visibility.Visible;

                    plyJoinDown.Visibility = Visibility.Collapsed;
                    plyJoinUp.Visibility = Visibility.Visible;

                    //           popupPositionY += 150;
                    popupPositionX += 50;
                    if ((popupPositionX + cnvPopup.Width) > 500)
                        popupPositionX = 195;
                    if (popupPositionY < -6)
                        popupPositionY = selectStart * 23 - 15;
                }
                else
                {
                    plyDown.Visibility = Visibility.Visible;
                    plyUp.Visibility = Visibility.Collapsed;

                    plyEditDown.Visibility = Visibility.Visible;
                    plyEditUp.Visibility = Visibility.Collapsed;

                    plyJoinDown.Visibility = Visibility.Visible;
                    plyJoinUp.Visibility = Visibility.Collapsed;
                }

                int i = selectStart;
                Int64 createdBy = userID;
                string[] strTagConfID = ((Label)sender).Tag.ToString().Split(':');
                if (strTagConfID.Length > 4)
                {
                    strTime = dayOfWeek + ", " + strTagConfID[4] + ":" + strTagConfID[5] + " - " + strTagConfID[6] + ":" + strTagConfID[7];
                    ConferenceID = Int64.Parse(strTagConfID[8]);
                    GlobalVariables.ConferenceID = ConferenceID;

                    DataRow drCreatedByConference = ClsCalender.getConferenceDetails(GlobalVariables.ConferenceID).Tables[0].Rows[0];
                    createdBy = Int64.Parse(drCreatedByConference["CreatedBy"].ToString());
                }
                if (userID == createdBy)
                {
                    if (!(((Label)sender).Background == selectionColor || ((Label)sender).Background == backgroundColor))
                    {
                        cnvPopup.Visibility = Visibility.Collapsed;
                        cnvPopupJoin.Visibility = Visibility.Collapsed;

                        string[] strTag = ((Label)sender).Tag.ToString().Split(':');
                        strTime = dayOfWeek + ", " + strTag[4] + ":" + strTag[5] + " - " + strTag[6] + ":" + strTag[7];
                        ConferenceID = Int64.Parse(strTag[8]);
                        GlobalVariables.ConferenceID = ConferenceID;

                        cnvPopupEdit.Visibility = Visibility.Visible;

                        //bool isFirst = true;
                        //int PopupStart = 0;
                        //for (int k = 0; k < 48; k++)
                        //{
                        //    if (glabel[k, 1].Background == selectStartColor && isFirst == true)
                        //    {
                        //        tag = glabel[k, 1].Tag.ToString();
                        //        strTag = tag.Split(':');
                        //        PopupStart = int.Parse(strTag[3]);
                        //        isFirst = false;
                        //    }
                        //}
                        //popupPositionX = this.Width - 400 ;
                        ///////9th February  ////          
                        //if (PopupStart != 0)
                        //{
                        //    if (!(glabel[PopupStart - 1, 1].Background == backgroundColor || glabel[PopupStart - 1, 1].Background == selectionColor))
                        //    {
                        //        PopupStart = PopupStart - 1;
                        //    }
                        //}
                        //else
                        //{
                        //    PopupStart = selectEnd;
                        //}
                        ///////9th February  ////

                        popupPositionY = 23 * (selectEnd) - 180;
                        if (popupPositionY < -6)
                            popupPositionY = (selectEnd) * 23 - 15;


                        //if (PopupStart < 8)
                        //{

                        //    plyDown.Visibility = Visibility.Collapsed;
                        //    plyUp.Visibility = Visibility.Visible;

                        //    plyEditDown.Visibility = Visibility.Collapsed;
                        //    plyEditUp.Visibility = Visibility.Visible;

                        //    plyJoinDown.Visibility = Visibility.Collapsed;
                        //    plyJoinUp.Visibility = Visibility.Visible;

                        //    //           popupPositionY += 150;
                        //    popupPositionX += 50;
                        //    if ((popupPositionX + cnvPopup.Width) > 500)
                        //        popupPositionX = 195;
                        //}
                        //else
                        //{
                        //    plyDown.Visibility = Visibility.Visible;
                        //    plyUp.Visibility = Visibility.Collapsed;

                        //    plyEditDown.Visibility = Visibility.Visible;
                        //    plyEditUp.Visibility = Visibility.Collapsed;

                        //    plyJoinDown.Visibility = Visibility.Visible;
                        //    plyJoinUp.Visibility = Visibility.Collapsed;
                        //}

                        cnvPopupEdit.SetValue(Canvas.TopProperty, popupPositionY);
                        cnvPopupEdit.SetValue(Canvas.LeftProperty, popupPositionX);
                    }
                    else
                    {

                        if (!startTime.Equals(endTime))
                        {
                            strTime = dayOfWeek + ", " + startTime + " - " + endTime;
                            etime2 = endTime;
                        }
                        else
                        {
                            if (startTime.Equals(endTime))
                            {
                                strTime = dayOfWeek + ", " + startTime;
                            }
                            else if (!etime2.Equals(""))
                                strTime = dayOfWeek + ", " + startTime + " - " + etime2;
                            else
                                strTime = dayOfWeek + ", " + startTime;
                        }

                        cnvPopupJoin.Visibility = Visibility.Collapsed;
                        cnvPopupEdit.Visibility = Visibility.Collapsed;
                        cnvPopup.Visibility = Visibility.Visible;
                        GlobalVariables.ConferenceID = ConferenceID = -1;

                        string[] currentDateSplit = mncCal.SelectedDate.Value.ToString().Split(' ');

                        GlobalVariables.startDate = currentDateSplit[0];
                        GlobalVariables.endDate = currentDateSplit[0];
                        string[] startTimeSplit1 = startTime.ToString().Split(' ');
                        string[] startTimeSplit2 = startTimeSplit1[0].ToString().Split(':');

                        GlobalVariables.startHour = startTimeSplit2[0];
                        GlobalVariables.startMinute = startTimeSplit2[1];
                        GlobalVariables.startAmPm = startTimeSplit1[1];

                        string[] endTimeSplit1 = endTime.ToString().Split(' ');
                        string[] endTimeSplit2 = endTimeSplit1[0].ToString().Split(':');

                        GlobalVariables.endHour = endTimeSplit2[0];
                        GlobalVariables.endMinute = endTimeSplit2[1];
                        GlobalVariables.endAmPm = endTimeSplit1[1];

                        //              popupPositionX = e.GetPosition(cnvMain).X + 20.0;
                        //              popupPositionY = e.GetPosition(cnvMain).Y - 200;
                        cnvPopup.SetValue(Canvas.TopProperty, popupPositionY);
                        cnvPopup.SetValue(Canvas.LeftProperty, popupPositionX);
                        cnvPopup.BringIntoView();
                        string currDate = mncCal.SelectedDate.Value.ToString();
                        string[] currDateSplit = currDate.Split(' ');
                        DateTime stDate = DateTime.Parse(currDateSplit[0] + " " + sTime);
                        DateTime eDate = DateTime.Parse(currDateSplit[0] + " " + eTime);
                    }
                }
                else
                {
                    //   cnvPopupJoin.Visibility = Visibility.Visible;
                    cnvPopupEdit.Visibility = Visibility.Collapsed;
                    cnvPopup.Visibility = Visibility.Collapsed;

                    string[] strTag = ((Label)sender).Tag.ToString().Split(':');
                    strTime = dayOfWeek + ", " + strTag[4] + ":" + strTag[5] + " - " + strTag[6] + ":" + strTag[7];
                    ConferenceID = Int64.Parse(strTag[8]);
                    GlobalVariables.ConferenceID = ConferenceID;

                    string response = ClsCalender.getConferenceUserMyAttendence(GlobalVariables.ConferenceID, userID);

                    if (response.Trim().Equals("Yes"))
                    {
                        btnGoingMayBe.Visibility = Visibility.Visible;
                        btnGoingNo.Visibility = Visibility.Visible;
                        btnGoingYes.Visibility = Visibility.Collapsed;

                        lblGoingYes.Visibility = Visibility.Visible;
                        lblGoingNo.Visibility = Visibility.Collapsed;
                        lblGoingMayBe.Visibility = Visibility.Collapsed;
                    }
                    else if (response.Trim().Equals("No"))
                    {
                        btnGoingMayBe.Visibility = Visibility.Visible;
                        btnGoingNo.Visibility = Visibility.Collapsed;
                        btnGoingYes.Visibility = Visibility.Visible;

                        lblGoingYes.Visibility = Visibility.Collapsed;
                        lblGoingNo.Visibility = Visibility.Visible;
                        lblGoingMayBe.Visibility = Visibility.Collapsed;
                    }
                    else if (response.Trim().Equals("MayBe"))
                    {
                        btnGoingMayBe.Visibility = Visibility.Collapsed;
                        btnGoingNo.Visibility = Visibility.Visible;
                        btnGoingYes.Visibility = Visibility.Visible;

                        lblGoingYes.Visibility = Visibility.Collapsed;
                        lblGoingNo.Visibility = Visibility.Collapsed;
                        lblGoingMayBe.Visibility = Visibility.Visible;
                    }
                    else
                    {
                        btnGoingMayBe.Visibility = Visibility.Visible;
                        btnGoingNo.Visibility = Visibility.Visible;
                        btnGoingYes.Visibility = Visibility.Visible;

                        lblGoingYes.Visibility = Visibility.Collapsed;
                        lblGoingNo.Visibility = Visibility.Collapsed;
                        lblGoingMayBe.Visibility = Visibility.Collapsed;
                    }

                    cnvPopupJoin.Visibility = Visibility.Visible;

                    //bool isFirst = true;
                    //int PopupStart = 0;
                    //for (int k = 0; k < 48; k++)
                    //{
                    //    if (glabel[k, 1].Background == selectStartColor && isFirst == true)
                    //    {
                    //        tag = glabel[k, 1].Tag.ToString();
                    //        strTag = tag.Split(':');
                    //        PopupStart = int.Parse(strTag[3]);
                    //        isFirst = false;
                    //    }
                    //}
                    ////popupPositionX = this.Width - 400 ;

                    //if (!(glabel[PopupStart - 1, 1].Background == backgroundColor || glabel[PopupStart - 1, 1].Background == selectionColor))
                    //{
                    //    PopupStart = PopupStart - 1;
                    //}

                    popupPositionY = 23 * (selectEnd) - 180;
                    if (popupPositionY < -6)
                        popupPositionY = (selectEnd) * 23 - 15;


                    //if (PopupStart < 8)
                    //{

                    //    plyDown.Visibility = Visibility.Collapsed;
                    //    plyUp.Visibility = Visibility.Visible;

                    //    plyEditDown.Visibility = Visibility.Collapsed;
                    //    plyEditUp.Visibility = Visibility.Visible;

                    //    plyJoinDown.Visibility = Visibility.Collapsed;
                    //    plyJoinUp.Visibility = Visibility.Visible;

                    //    //           popupPositionY += 150;
                    //    popupPositionX += 50;
                    //    if ((popupPositionX + cnvPopup.Width) > 500)
                    //        popupPositionX = 195;
                    //}
                    //else
                    //{
                    //    plyDown.Visibility = Visibility.Visible;
                    //    plyUp.Visibility = Visibility.Collapsed;

                    //    plyEditDown.Visibility = Visibility.Visible;
                    //    plyEditUp.Visibility = Visibility.Collapsed;

                    //    plyJoinDown.Visibility = Visibility.Visible;
                    //    plyJoinUp.Visibility = Visibility.Collapsed;
                    //}
                    cnvPopupJoin.SetValue(Canvas.TopProperty, popupPositionY);
                    cnvPopupJoin.SetValue(Canvas.LeftProperty, popupPositionX);
                }

                currLabel = ((Label)sender);
                lblTime.Content = strTime;
                lblEditTime.Content = strTime;
                lblEditTime1.Content = strTime;
            }
            catch (Exception exp)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(exp, "l_MouseUp()", "ctlConfCalander.xaml.cs");
            }
        }

        void l_MouseEnter(object sender, MouseEventArgs e)
        {
            try
            {
            string tag = "";
            if (e.LeftButton.ToString().Equals("Pressed") )
            {
                tag = ((Label)sender).Tag.ToString();
                string[] str = tag.Split(':');
                selectEnd = int.Parse(str[3]);

                                          
                
                endTime = str[0] + ":" + str[1] + " " + str[2];
                eTime = str[0] + ":" + str[1] + ":00 " + str[2];
                for (int i = 0; i < 48; i++)
                {
                    if(glabel[i,1].Background == selectionColor)
                        glabel[i, 1].Background = backgroundColor;
                }

                if (selectStart < selectEnd)
                {

                    for (int i = selectStart; i <= selectEnd; i++)
                    {
                        if (glabel[i, 1].Background != backgroundColor && glabel[i, 1].Background != selectionColor)
                            return;
                        if(((Label)sender).Background==backgroundColor)
                            glabel[i, 1].Background = selectionColor;

                    }
                }
                else if (selectStart > selectEnd)
                {
                    for (int i = selectStart; i >= selectEnd; i--)
                    {
                        if (glabel[i, 1].Background != backgroundColor && glabel[i, 1].Background != selectionColor)
                            return;
                        if(((Label)sender).Background==backgroundColor)
                            glabel[i, 1].Background = selectionColor;

                    }

                }
                else if (selectStart == selectEnd)
                {
                    if (glabel[selectStart, 1].Background == backgroundColor || glabel[selectStart, 1].Background == selectionColor)
                        glabel[selectStart, 1].Background = selectionColor;
                }

               

            }
			  

           	 }


            catch (Exception exp)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(exp, "l_MouseEnter()", "ctlConfCalander.xaml.cs");
            }

        }
       
        void l_MouseDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
            flag = false;
            
            for (int i = 0; i < 48; i++)
            {
                if(glabel[i,1].Background == selectionColor)
                    glabel[i, 1].Background = backgroundColor;
            }

            //MessageBox.Show(Grid.GetRow((UIElement)sender).ToString());
            
            //MessageBox.Show();
            String tag = ((Label)sender).Tag.ToString();
            string[] str = tag.Split(':');
          
            selectStart = selectEnd = int.Parse(str[3]);

            if (((Label)sender).Background == backgroundColor)
            {
                if (selectStart != 47)
                {
                    ((Label)sender).Background = selectionColor;
                    selectEnd = selectStart + 1;
                }
                else
                    ((Label)sender).Background = selectionColor;

            }
            
          //  string month = DateTime.
           
//Selection of two rows
           

            if (!((glabel[selectEnd, 1].Background == backgroundColor) || (glabel[selectEnd, 1].Background == selectionColor)))
            {
                if(selectStart!=0)
                    selectEnd = selectStart - 1;

            }
            else if (selectStart == 47)
            {
                selectEnd = selectStart - 1;
                glabel[selectEnd, 1].Background = backgroundColor;
            }
            if (glabel[selectEnd, 1].Background == backgroundColor && glabel[selectStart, 1].Background == selectionColor)
                glabel[selectEnd, 1].Background = selectionColor;
/////////////////////////////////////////////////////////////////////////////////
            startTime = endTime = str[0] + ":" + str[1] + " " + str[2];
            sTime = eTime = str[0] + ":" + str[1] + ":00 " + str[2];
            //cnvMain.Visibility = Visibility.Visible;

            popupPositionX = e.GetPosition(cnvMain).X ;

            if((popupPositionX + cnvPopup.Width) > 500)
                popupPositionX = 195;

//            popupPositionY = e.GetPosition(cnvMain).Y - 200.0;

            popupPositionY = 23 * selectStart - 190;

            if (popupPositionY  < -6)
                popupPositionY = selectStart * 23 - 15;
                

            selectStartColor = ((Label)sender).Background;
            }
            catch (Exception exp)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(exp, "l_MouseDown()", "ctlConfCalander.xaml.cs");
            }
        }

        private void scrvGrid_ScrollChanged(object sender, ScrollChangedEventArgs e)
        {


        }

        private void btnOK_Click_1(object sender, RoutedEventArgs e)
        {
            try
            {
            if (txtPurpose.Text.ToString().Trim().Length != 0)
            {
                ClsCalender clsCal = new ClsCalender();
                clsCal.ID = -1;  // Create New Event
                clsCal.ConfTitle = txtPurpose.Text;
                string currDate = mncCal.SelectedDate.Value.ToString();
                string[] currDateSplit = currDate.Split(' ');
                DateTime stDate = DateTime.Parse(currDateSplit[0] + " " + sTime);
                DateTime eDate = DateTime.Parse(currDateSplit[0] + " " + eTime);

                GlobalVariables.ConferenceID = ConferenceID;
                clsCal.ConferenceType = "Private";
                clsCal.RepeatType = "Does Not Repeat";
                clsCal.StartDateTime = stDate;
                clsCal.EndDateTime = eDate;
                clsCal.HostId = userID;
                clsCal.CreatedBy = userID;
                clsCal.ModifiedBy = userID;
                CONFERENCEID = clsCal.Save();

                string myDisplayName = ClsCalender.getUserInfo(userID);
                string myEmail = ClsCalender.getUserInfoEmail(userID);
                clsCal.GuestName = myDisplayName;
                clsCal.Email = myEmail;
                
                clsCal.SaveGuest(CONFERENCEID);
                ClsCalender.addConferenceUsers(CONFERENCEID, userID);
                ClsCalender.updateConferenceUsers(CONFERENCEID, userID, "Yes");
                string[] currentDateSplit = mncCal.SelectedDate.Value.ToString().Split(' ');
                removeUnnecessaryLabelsFromGrid();
                //getMyConferences(userID, currentDateSplit[0]);
                getMyConferences(userID, mncCal.SelectedDate.Value.ToString("dd-MMM-yyyy"));

            }
            else
            {
                MessageBox.Show("Please Enter the Purpose of the event");
                
            }
            }
            catch (Exception exp)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(exp, "btnOK_Click_1()", "ctlConfCalander.xaml.cs");
            }
            
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void populateDataDayWise(DateTime start, DateTime end, Int64 ConferenceID, string message)
        {
            try
            {
                string sDate = start.ToString();
                string eDate = end.ToString();

                string[] sDateSplit = sDate.Split(' ');
                string[] eDateSplit = eDate.Split(' ');
                string[] sTimeSplit = sDateSplit[1].Split(':');
                string[] eTimeSplit = eDateSplit[1].Split(':');
                string sTime = sTimeSplit[0] + ":" + sTimeSplit[1] + " " + sDateSplit[2];
                string eTime = eTimeSplit[0] + ":" + eTimeSplit[1] + " " + eDateSplit[2];
                int startPoint = -1, endPoint = -1;

                for (int i = 0; i < 48; i++)
                {
                    if (glabel[i, 0].Content.Equals(sTime))
                    {
                        startPoint = i;
                    }


                    if (glabel[i, 0].Content.Equals(eTime))
                    {
                        endPoint = i;
                    }
                }

                //           MessageBox.Show("Start-" + startPoint + "  " + "End-" + endPoint);

                if (!(glabel[startPoint, 1].Background == backgroundColor && glabel[endPoint, 1].Background == backgroundColor))
                {

                    //MessageBox.Show("I am inside....");
                    //if (glabel[startPoint, 1].Background == scbDark && glabel[endPoint, 1].Background == scbLight)
                    //{
                    //MessageBox.Show("I am inside 2....");
                    try
                    {
                        //if (alLabels.Count != 0)
                        //{
                        //    for (int l = 0; l < alLabels.Count; l++)
                        //        grdMain.Children.Remove(((Label)alLabels[l]));
                        //}
                        int cntLabel = 0;
                        int cntIncrOne = 0;
                        bool isCntIncrOne = true;
                        int cntRows = endPoint - startPoint + 1;
                        for (int row = startPoint; row <= endPoint; row++)
                        {
                            double newSize = (lblMaxWidth - 2) / rowLabels[row];
                            newSize = newSize - 2.0;
                            double currentLeftPos = newSize + 2.0;
                            //        bool isFirst = true;
                            //////////////////////////////////////////////////////////////////////////////////////

                            string tag = glabel[startPoint, 1].Tag.ToString();
                            string[] str = tag.Split(':');
                            sTimeShort = str[0] + ":" + str[1] + " " + str[2];

                            tag = glabel[endPoint, 1].Tag.ToString();
                            str = tag.Split(':');
                            eTimeShort = str[0] + ":" + str[1] + " " + str[2];



                            /////////////////////////////////////////////////////////////////////////////////
                            glabel[row, 1].Width = newSize;
                            //                          MessageBox.Show(glabel[row,1].Width.ToString());
                            glabel[row, 1].HorizontalAlignment = HorizontalAlignment.Left;

                            //  cntLabel = cntIncrOne;
                            isCntIncrOne = true;
                            for (int k = 2; k <= rowLabels[row]; k++)
                            {


                                if (k < rowLabels[row])
                                {
                                    if ((((k - 2) % (rowLabels[row] - 1)) == (k - 2)) && (((k - 2) % (rowLabels[row] - 1)) != (rowLabels[row] - 2)))
                                    {

                                        if (isCntIncrOne == true)
                                        {


                                            if (cntIncrOne == 0)
                                            {
                                                cntLabel = cntIncrOne;
                                                cntIncrOne++;
                                            }
                                            else
                                            {
                                                cntLabel = cntIncrOne;
                                                cntIncrOne++;
                                            }

                                            isCntIncrOne = false;
                                        }
                                        else
                                        {
                                            cntLabel += cntRows;
                                        }
                                    }
                                    else
                                    {
                                        cntLabel += cntIncrOne;
                                        cntIncrOne++;
                                    }

                                    lbl = (Label)alLabels[cntLabel];
                                    //    MessageBox.Show(cntLabel.ToString());
                                    //    cntLabel++;
                                    lbl.Margin = new Thickness(currentLeftPos, 0, 0, 0);
                                    currentLeftPos = currentLeftPos + newSize + 2;
                                    lbl.Width = newSize;
                                    lbl.Height = 23;

                                    //lbl.ToolTip = message;
                                    //if (isFirst == true)
                                    //{
                                    //    lbl.Content = message;
                                    //    isFirst = false;
                                    //}

                                    lbl.HorizontalAlignment = HorizontalAlignment.Left;


                                    scbDark.Color = Color.FromRgb(154, 192, 205);
                                    scbDark.Opacity = 0.6;

                                    scbLight.Color = Color.FromRgb(154, 192, 205);
                                    scbLight.Opacity = 0.3;

                                    //if (row == startPoint)
                                    //{
                                    //    lbl.Background = scbDark;
                                    //}
                                    //else
                                    //    lbl.Background = scbLight;

                                }
                                else
                                {
                                    lbl = new Label();
                                    lbl.Name = "lbl" + row + "" + rowLabels[row];

                                    alLabels.Add(lbl);

                                    grdMain.Children.Add(lbl);
                                    Grid.SetColumn(lbl, 1);
                                    Grid.SetRow(lbl, row);

                                    lbl.Margin = new Thickness(currentLeftPos, 0, 0, 0);
                                    currentLeftPos = currentLeftPos + newSize + 2;


                                    lbl.Width = newSize;
                                    lbl.Height = 23;

                                    //if (isFirst == true)
                                    //{
                                    //    lbl.Content = message;
                                    //    isFirst = false;
                                    //}

                                    lbl.HorizontalAlignment = HorizontalAlignment.Left;
                                    string[] tagHalfSplit = glabel[row, 1].Tag.ToString().Split(':');
                                    lbl.Tag = tagHalfSplit[0] + ":" + tagHalfSplit[1] + ":" + tagHalfSplit[2] + ":" + tagHalfSplit[3];
                                    SolidColorBrush scbDarkNew = new SolidColorBrush();
                                    scbDarkNew.Color = Color.FromRgb(176, 48, 96);
                                    scbDarkNew.Opacity = 0.6;
                                    SolidColorBrush scbLightNew = new SolidColorBrush();
                                    scbLightNew.Color = Color.FromRgb(176, 48, 96);
                                    scbLightNew.Opacity = 0.3;

                                    if (row == startPoint)
                                    {
                                        lbl.Background = scbDarkNew;
                                        lbl.Content = message;

                                    }
                                    else
                                    {
                                        lbl.Background = scbLightNew;
                                    }

                                    lbl.ToolTip = message;
                                    lbl.BorderThickness = new Thickness(0);
                                    lbl.Foreground = Brushes.Black;

                                    lbl.Tag += ":" + sTimeShort + ":" + eTimeShort + ":" + ConferenceID + ":" + startPoint + ":" + endPoint;

                                    if (row == selectEnd)
                                    {
                                        lbl.BorderThickness = new Thickness(0.5);
                                        lbl.BorderBrush = Brushes.White;
                                    }

                                    if (glabel[row, 1].Background == backgroundColor)
                                    {
                                        if (row == startPoint)
                                        {
                                            glabel[row, 1].Background = scbDarkNew;
                                            glabel[row, 1].Content = message;

                                        }
                                        else
                                        {
                                            glabel[row, 1].Background = scbLightNew;
                                        }
                                        glabel[row, 1].Foreground = Brushes.Black;
                                        glabel[row, 1].Tag = lbl.Tag;
                                        glabel[row, 1].ToolTip = message;
                                        glabel[row, 1].Width = lblMaxWidth;
                                        glabel[row, 1].BorderThickness = new Thickness(0);
                                        alLabels.Remove(lbl);
                                        grdMain.Children.Remove(((Label)lbl));

                                    }


                                    lbl.MouseDown += new MouseButtonEventHandler(l_MouseDown);
                                    lbl.MouseUp += new MouseButtonEventHandler(l_MouseUp);
                                    lbl.MouseEnter += new MouseEventHandler(l_MouseEnter);

                                }






                                //    for (int l = 0; l < alLabels.Count; l++)
                                //        MessageBox.Show(alLabels.Count.ToString());



                            }
                            rowLabels[row] += 1;
                        }
                    }
                    catch (Exception exp)
                    {
                        VMuktiAPI.VMuktiHelper.ExceptionHandler(exp, "populateDataDayWise()", "ctlConfCalander.xaml.cs");
                    }
                }
                else
                {
                    bool newR = false;

                    string tag = glabel[startPoint, 1].Tag.ToString();
                    string[] str = tag.Split(':');
                    sTimeShort = str[0] + ":" + str[1] + " " + str[2];

                    tag = glabel[endPoint, 1].Tag.ToString();
                    str = tag.Split(':');
                    eTimeShort = str[0] + ":" + str[1] + " " + str[2];


                    // scbDark.Color = Color.FromRgb(158,182,235);
                    scbDark.Color = Color.FromRgb(154, 192, 205);
                    scbDark.Opacity = 0.6;

                    //scbLight.Color = Color.FromRgb(207,219,245);
                    scbLight.Color = Color.FromRgb(154, 192, 205);
                    scbLight.Opacity = 0.3;

                    for (int i = startPoint; i <= endPoint; i++)
                    {
                        if (i == startPoint)
                            glabel[i, 1].Background = scbDark;
                        else
                            glabel[i, 1].Background = scbLight;
                        // glabel[i, 1].Background = changeLabelColor(LabelColor);
                        glabel[i, 1].ToolTip = message;
                        glabel[i, 1].BorderThickness = new Thickness(0);
                        glabel[i, 1].Foreground = Brushes.Black;
                        glabel[i, 1].Width = lblMaxWidth;
                        // .Tag   : startime[4],[5] : endtime[6],[7] : ConferenceID[8] : StartPoint of Event[9] : EndPoint of Event[10];
                        // eg.      '7':'00 PM':9:30 PM:3:21:26
                        //           [4]   [5]  [6] [7] [8][9][10]
                        glabel[i, 1].Tag += ":" + sTimeShort + ":" + eTimeShort + ":" + ConferenceID + ":" + startPoint + ":" + endPoint;
                        if (newR == false)
                        {
                            glabel[i, 1].Content = message;
                            newR = true;
                        }

                    }

                }
            }
            catch (Exception exp)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(exp, "populateDataDayWise()--main--", "ctlConfCalander.xaml.cs");
            }
        }
        
        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            try
            {
            System.Windows.MessageBoxResult msgResult = MessageBox.Show("Are you sure, you want to delete this event?", "Are you sure, you want to delete this event?", System.Windows.MessageBoxButton.YesNo, System.Windows.MessageBoxImage.Question);

            if (msgResult == System.Windows.MessageBoxResult.Yes)
            {
                ClsCalender.deleteConference(GlobalVariables.ConferenceID);
                ClsCalender.deleteConferenceUsers(GlobalVariables.ConferenceID, userID);
                string[] currentDateSplit = mncCal.SelectedDate.Value.ToString().Split(' ');
                MessageBox.Show("The selected Event has been deleted");

                removeUnnecessaryLabelsFromGrid();
                
                //getMyConferences(userID, currentDateSplit[0]);
                getMyConferences(userID, mncCal.SelectedDate.Value.ToString("dd-MMM-yyyy"));
           
            }
            else if (msgResult == System.Windows.MessageBoxResult.No)
            {
                
            }
            cnvPopupEdit.Visibility = Visibility.Collapsed;
			}
            catch (Exception exp)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(exp, "btnDelete_Click()", "ctlConfCalander.xaml.cs");
            }
        }

        private void btnDelete1_Click(object sender, RoutedEventArgs e)
        {
            try
            {
            System.Windows.MessageBoxResult msgResult = MessageBox.Show("Are you sure, you want to delete this event? All invitations sent to the users will also be deleted", "Are you sure, you want to delete this event?", System.Windows.MessageBoxButton.YesNo, System.Windows.MessageBoxImage.Question);

            if (msgResult == System.Windows.MessageBoxResult.Yes)
            {
                ClsCalender.deleteConference(GlobalVariables.ConferenceID);
                ClsCalender.deleteConferenceUsers(GlobalVariables.ConferenceID);
                string[] currentDateSplit = mncCal.SelectedDate.Value.ToString().Split(' ');
             //   MessageBox.Show("The selected event has been deleted.");

                removeUnnecessaryLabelsFromGrid();
                //getMyConferences(userID, currentDateSplit[0]);
                getMyConferences(userID, mncCal.SelectedDate.Value.ToString("dd-MMM-yyyy"));

            }
            else if (msgResult == System.Windows.MessageBoxResult.No)
            {

            }
            cnvPopupEdit.Visibility = Visibility.Collapsed;
		    }
            catch (Exception exp)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(exp, "btnDelete1_Click()", "ctlConfCalander.xaml.cs");
            }
        }

        private void btnGoingYes_Click(object sender, RoutedEventArgs e)
        {
            try
            {
            ClsCalender.updateConferenceUsers(GlobalVariables.ConferenceID, userID, "Yes");
            cnvPopupJoin.Visibility = Visibility.Collapsed;
		    }
            catch (Exception exp)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(exp, "btnGoingYes_Click()", "ctlConfCalander.xaml.cs");
            }
        }

        private void btnGoingNo_Click(object sender, RoutedEventArgs e)
        {
            try
            {
            ClsCalender.updateConferenceUsers(GlobalVariables.ConferenceID, userID, "No");
            cnvPopupJoin.Visibility = Visibility.Collapsed;
		    }
            catch (Exception exp)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(exp, "btnGoingNo_Click()", "ctlConfCalander.xaml.cs");
            }
        }

        private void btnGoingMayBe_Click(object sender, RoutedEventArgs e)
        {
            try
            {
            ClsCalender.updateConferenceUsers(GlobalVariables.ConferenceID, userID, "MayBe");
            cnvPopupJoin.Visibility = Visibility.Collapsed;
		  }
            catch (Exception exp)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(exp, "btnGoingMayBe_Click()", "ctlConfCalander.xaml.cs");
            }
        }

        private void btnCancel_Click_1(object sender, RoutedEventArgs e)
        {
            try
            {
                cnvPopup.Visibility = Visibility.Collapsed;
                cnvPopupEdit.Visibility = Visibility.Collapsed;
                cnvPopupJoin.Visibility = Visibility.Collapsed;

                string[] currentDateSplit = mncCal.SelectedDate.Value.ToString().Split(' ');
                removeUnnecessaryLabelsFromGrid();
                //getMyConferences(userID, currentDateSplit[0]);
                getMyConferences(userID, mncCal.SelectedDate.Value.ToString("dd-MMM-yyyy"));
            }
            catch (Exception exp)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(exp, "btnCancel_Click_1()", "ctlConfCalander.xaml.cs");
            }
            

        }

        private void removeUnnecessaryLabelsFromGrid()
        {
            if (alLabels.Count != 0)
            {
                for (int l = 0; l < alLabels.Count; l++)
                    grdMain.Children.Remove(((Label)alLabels[l]));
            }
            alLabels.Clear();
        }

        private void TextBlock_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
            this.Visibility = Visibility.Collapsed;
            if (((ctlCalContainer)this.Tag).objEditEvent == null)
            {
                ((ctlCalContainer)this.Tag).objEditEvent = new ctlEditEvent();
                ((Grid)this.Parent).Children.Add(((ctlCalContainer)this.Tag).objEditEvent);
                ((ctlCalContainer)this.Tag).objEditEvent.Tag = this.Tag;
            }
            ((ctlCalContainer)this.Tag).objEditEvent.Visibility = Visibility.Visible;
        }
		 catch (Exception exp)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(exp, "TextBlock_PreviewMouseLeftButtonDown()", "ctlConfCalander.xaml.cs");
            }
        }

        private void TextBlock_PreviewMouseLeftButtonDown_1(object sender, MouseButtonEventArgs e)
        {
            try
            {
            this.Visibility = Visibility.Collapsed;
            if (((ctlCalContainer)this.Tag).objAddEvent == null)
            {
                ((ctlCalContainer)this.Tag).objAddEvent = new ctlAddEvent();
                ((Grid)this.Parent).Children.Add(((ctlCalContainer)this.Tag).objAddEvent);
                ((ctlCalContainer)this.Tag).objAddEvent.Tag = this.Tag;
            }
            ((ctlCalContainer)this.Tag).objAddEvent.Visibility = Visibility.Visible;
			}
            catch (Exception exp)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(exp, "TextBlock_PreviewMouseLeftButtonDown_1()", "ctlConfCalander.xaml.cs");
            }
        }

        private void TextBlock_PreviewMouseLeftButtonDown_2(object sender, MouseButtonEventArgs e)
        {
            this.Visibility = Visibility.Collapsed;
            if (((ctlCalContainer)this.Tag).objReadEvent == null)
            {
                ((ctlCalContainer)this.Tag).objReadEvent = new ctlEditEventReadOnly();
                ((Grid)this.Parent).Children.Add(((ctlCalContainer)this.Tag).objReadEvent);
                ((ctlCalContainer)this.Tag).objReadEvent.Tag = this.Tag;
            }
            ((ctlCalContainer)this.Tag).objReadEvent.Visibility = Visibility.Visible;

        }
        /* ------------------------------------------------------   */


        #region logging function

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

        #endregion
    }
}
