using System;
using System.Collections.Generic;

using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Xml;
using System.Text.RegularExpressions;
using VMuktiAPI;


namespace News.Bussines
{
    public class GetNews
    {
        XmlTextReader rssReader;
        XmlDocument rssDoc;
        XmlNode nodeChannel;
        bool disposed = false;

        public GetNews()
        {

        }

        public List<News.Bussines.EvzItem> FuncGetNews(String Url, int nettype)
        {

            try
            {

                rssReader = new XmlTextReader(Url);
                rssDoc = new XmlDocument();

                try
                {
                    rssDoc.Load(rssReader);

                }
                catch (Exception exp)
                {
                    exp.Data.Add("My Key", "FuncGetNews()--:--GetNews.cs--:--" + exp.Message + " :--:--");
                    ClsException.LogError(exp);
                    ClsException.WriteToErrorLogFile(exp);
                }
                List<News.Bussines.EvzItem> lstNews = new List<News.Bussines.EvzItem>();
                {

                    try
                    {
                        if (rssDoc.SelectSingleNode("//channel") != null)
                        {
                            nodeChannel = rssDoc.SelectSingleNode("//channel");
                        }
                    }
                    catch (Exception exp)
                    {
                        exp.Data.Add("My Key", "FuncGetNews()--:--GetNews.cs--:--" + exp.Message + " :--:--");
                        ClsException.LogError(exp);
                        ClsException.WriteToErrorLogFile(exp);
                    }
                }

                try
                {
                    if (nodeChannel.ChildNodes.Count == 0)
                    {
                        //  MessageBox.Show("Not available");
                    }

                    else
                    {
                        foreach (XmlNode NdItem in nodeChannel)
                        {
                            News.Bussines.EvzItem lblNews = new News.Bussines.EvzItem();
                            if (NdItem.Name == "item")
                            {
                                lblNews.Title = GetNews.FncFilterString(GetNews.FuncRetriveInfoFromNode(NdItem, "title"), nettype);

                                lblNews.Tag = NdItem;
                                lstNews.Add(lblNews);
                            }
                        }



                    }
                }
                catch (Exception exp)
                {
                    exp.Data.Add("My Key", "FuncGetNews()--:--GetNews.cs--:--" + exp.Message + " :--:--");
                    ClsException.LogError(exp);
                    ClsException.WriteToErrorLogFile(exp);
                }

                return lstNews;
            }
            catch (Exception exp)
            {
                exp.Data.Add("My Key", "FuncGetNews()--:--GetNews.cs--:--" + exp.Message + " :--:--");
                ClsException.LogError(exp);
                ClsException.WriteToErrorLogFile(exp);
                return null;
            }

        }

        public static string FuncRetriveInfoFromNode(XmlNode nd, string nodeName)
        {
            try
            {
                string InnerTextNode = null;

                foreach (XmlNode nd1 in nd)
                {
                    if (nd1.Name == nodeName)
                    {
                        InnerTextNode = nd1.InnerText;
                        break;
                    }
                }
                return InnerTextNode;
            }
            catch (Exception exp)
            {
                exp.Data.Add("My Key", "FuncRetriveInfoFromNode()--:--GetNews.cs--:--" + exp.Message + " :--:--");
                ClsException.LogError(exp);
                ClsException.WriteToErrorLogFile(exp);
                return null;
            }
        }

        public static string FncFilterString(string temp, int newstype)
        {
            try
            {
                //string temps1 = temp.Replace("&nbsp;", "");
                //string temps2 = temps1.Replace("</EM>", "");
                //string temp3 = temps2.Replace("</STRONG>", "");
                //string temp4 = temp3.Replace("<I>", "");
                //string temp5 = temp4.Replace("</I>", "");
                //string temp6 = temp5.Replace("<i>", "");
                //string temp7 = temp6.Replace("/P", "");
                //string temp8 = temp7.Replace("STRONG", "");
                //string temp9 = temp8.Replace("</i>", "");
                //string temp10 = temp9.Replace("<", "");
                //string temp11 = temp10.Replace(">", "");

                //if (newstype == 1)
                //{
                //    return temp11.Split('.')[0];
                //}
                //return temp11;
                Regex anyTag = new Regex(@"<[/]{0,1}\s*(?<tag>\w*)\s*(?<attr>.*?=['""].*?[""'])*?\s*[/]{0,1}>");
                return anyTag.Replace(temp, " ");
            }
            catch (Exception exp)
            {
                exp.Data.Add("My Key", "FncFilterString()--:--GetNews.cs--:--" + exp.Message + " :--:--");
                ClsException.LogError(exp);
                ClsException.WriteToErrorLogFile(exp);
                return null;
            }
        }

        //<description>
        //<img src="http://static.ibnlive.com/pix/sitepix/10_2007/benazir_returns_g90.jpg"
        //alt="DEMOCRACY'S MARTYR: Fromer Pakistan Prime Minister Benazir Bhutto was killed on December 27." 
        //title="DEMOCRACY'S MARTYR: Fromer Pakistan Prime Minister Benazir Bhutto was killed on December 27."
        //border="0" width="90" height="62" align="left" hspace="5"/>
        //Teen was part of five-member team of assassins, say Pak officials.
        //</description> 

        public static string FncGetTitle(string desc)
        {
            try
            {
                int temp = desc.IndexOf("title");
                int temp2 = desc.IndexOf("border", temp + 10);
                string link = desc.Substring(temp + 7, temp2 - temp - 9);
                return link;
            }
            catch (Exception exp)
            {
                exp.Data.Add("My Key", "FncGetTitle()--:--GetNews.cs--:--" + exp.Message + " :--:--");
                ClsException.LogError(exp);
                ClsException.WriteToErrorLogFile(exp);
                return null;
            }
        }

        public static string FncGetImageSource(string desc)
        {
            try
            {

                int temp = desc.IndexOf("src=");
                int temp2 = desc.IndexOf("\"", temp + 8);
                string link = desc.Substring(temp + 5, (temp2 - temp) - 5);
                return link;

            }
            catch (Exception exp)
            {
                exp.Data.Add("My Key", "FncGetImageSource()--:--GetNews.cs--:--" + exp.Message + " :--:--");
                ClsException.LogError(exp);
                ClsException.WriteToErrorLogFile(exp);
                return null;
            }
        }



        public void Dispose()
        {
            //VMuktiAPI.ClsException.WriteToErrorLogFile(new Exception("-------amit--------Dispposed is called in simple dispose"));
            Dispose(true);
            //GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    // VMuktiAPI.ClsException.WriteToErrorLogFile(new Exception("-------amit--------Dispposed is called in protected managed"));
                    rssReader = null;
                    rssDoc = null;
                    nodeChannel = null;
                }

                // VMuktiAPI.ClsException.WriteToErrorLogFile(new Exception("-------amit--------Dispposed is called in protected unmanaged"));
            }
            //VMuktiAPI.ClsException.WriteToErrorLogFile(new Exception("-------amit--------Dispposed is called in dispose out"));
            disposed = true;
        }



        ~GetNews()
        {
            this.Dispose();
            System.GC.Collect();
            System.GC.WaitForPendingFinalizers();
        }
    }
}

//description of xml file rss feed 
// <?xml version="1.0" encoding="utf-8" ?> 
//- <rss version="2.0">
//- <channel>
//  <title>Hindustan Times News Feeds 'World'</title> 
//  <description /> 
//  <ttl>10</ttl> 
//  <name /> 
//  <user /> 
//- <item>
//  <title>Bilawal Bhutto Zardari for peace with India</title> 
//  <description>Bilawal Bhutto Zardari, son of slain Benazir Bhutto, says Pakistan should be able to live in peaceful coexistence with all its neighbours.</description> 
//  <pubDate>Tuesday, January 08, 2008 5:00:24 PM IST</pubDate> 
//  <link>http://www.hindustantimes.com/redir.aspx?ID=22f2092d-592c-46cb-bfeb-748e895bbe68</link> 
//  </item>
//- <item>
//  <title>Kid glues himself to bed to avoid school</title> 
//  <description><P>Diego, 10, took his family's can of glue on Sunday night and glues his hand to the metal headboard of his bed to avoid going back to school after the Christmas holidays.</P></description> 
//  <pubDate>Tuesday, January 08, 2008 4:31:09 PM IST</pubDate> 
//  <link>http://www.hindustantimes.com/redir.aspx?ID=584e2e85-4919-466e-b33b-bb24bb171852</link> 
//  </item>
//</channel>
// </rss>





//<description>
//<img src="http://static.ibnlive.com/pix/sitepix/10_2007/benazir_returns_g90.jpg" 
//alt="DEMOCRACY'S MARTYR: Fromer Pakistan Prime Minister Benazir Bhutto was killed on December 27." 
//title="DEMOCRACY'S MARTYR: Fromer Pakistan Prime Minister Benazir Bhutto was killed on December 27."
//border="0" width="90" height="62" align="left" hspace="5"/>
//Teen was part of five-member team of assassins, say Pak officials.
//</description> 