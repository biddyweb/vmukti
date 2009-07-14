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
using Lucene.Net.Search;
using System.IO;
using Lucene.Net.QueryParsers;
using Lucene.Net.Analysis.Standard;
using Lucene.Net.Documents;
using Lucene.Net.Index;
using System.Windows.Documents;
using System.Windows.Forms;


namespace FileSearch.Business
{
    public delegate void DelStatus(string strMessage,bool Error);

    public class Business_FileSearch
    {
        private string pathIndex;
        private string FilePath = string.Empty;
        private IndexWriter indexWriter;
        private IndexSearcher searcher;
        private string[] patterns = { "*.doc", "*.xls", "*.ppt", "*.htm", "*.txt" };
        private long bytesTotal;
        private int countTotal;
        private int countSkipped;

        public event DelStatus EntStatus;

        public Business_FileSearch(string IndexPath,string FilePath)
        {
            this.pathIndex = IndexPath;
            this.FilePath = FilePath;
            
        }

        public void FncRebuildIndex(string strPath)
        {
            indexWriter = new IndexWriter(this.pathIndex, new StandardAnalyzer(), true);
            bytesTotal = 0;
            countTotal = 0;
            countSkipped = 0;

            DirectoryInfo di = new DirectoryInfo(strPath);
            DateTime start = DateTime.Now;
            addFolder(di);
            string summary = String.Format("Done. Indexed {0} files ({1} bytes). Skipped {2} files.", countTotal, bytesTotal, countSkipped);
            summary += String.Format(" Took {0}", (DateTime.Now - start));
            status(summary);
            indexWriter.Optimize();
            indexWriter.Close();
        }

        private void addFolder(DirectoryInfo directory)
        {
            // find all matching files
            foreach (string pattern in patterns)
            {
                foreach (FileInfo fi in directory.GetFiles(pattern))
                {
                    // skip temporary office files
                    if (fi.Name.StartsWith("~"))
                        continue;

                    try
                    {
                        addOfficeDocument(fi.FullName);

                        // update statistics
                        this.countTotal++;
                        this.bytesTotal += fi.Length;

                        // show added file
                        status(fi.FullName);
                    }
                    catch (Exception)
                    {
                        // parsing and indexing wasn't successful, skipping that file
                        this.countSkipped++;
                        status("Skipped: " + fi.FullName);
                    }
                }
            }

            // add subfolders
            foreach (DirectoryInfo di in directory.GetDirectories())
            {
                addFolder(di);
            }
        }

        private void addOfficeDocument(string path)
        {
            Document doc = new Document();
            string filename = System.IO.Path.GetFileName(path);

            doc.Add(Field.UnStored("text", Parser.Parse(path)));
            doc.Add(Field.Keyword("path", path));
            doc.Add(Field.Text("title", filename));
            indexWriter.AddDocument(doc);
        }

        public string[] Fncsearch(string strSearchContent)
        {
            DateTime start = DateTime.Now;
            try
            {
                searcher = new IndexSearcher(this.pathIndex);
            }
            catch (IOException ex)
            {
                System.Windows.Forms.MessageBox.Show("The index doesn't exist or is damaged. Please rebuild the index.\r\n\r\nDetails:\r\n" + ex.Message);
                return null;
            }

            if (strSearchContent.Trim(new char[] { ' ' }) == String.Empty)
                return null;
            if (strSearchContent == "*")
            {
               
              MessageBox.Show("Sorry cannot search files with *");
            }
            Query query = QueryParser.Parse(strSearchContent, "text", new StandardAnalyzer());
            Hits hits = searcher.Search(query);
            string[] strTempArr = new string[hits.Length()];
            for (int i = 0; i < hits.Length(); i++)
            {
                Document doc = hits.Doc(i);
                string filename = doc.Get("title");
                string path = doc.Get("path");
                string folder = System.IO.Path.GetDirectoryName(path);
                DirectoryInfo di = new DirectoryInfo(folder);
                string s = filename + "     " + path + "     " + hits.Score(i).ToString();
                System.Windows.Forms.ListViewItem item = new System.Windows.Forms.ListViewItem(new string[] { null, filename, di.Name, hits.Score(i).ToString() });
                item.Tag = path;
                strTempArr[i] = s;
            }
            searcher.Close();

            //string searchReport = String.Format("Search took {0}. Found {1} items.", (DateTime.Now - start), hits.Length());
            //status(searchReport);
            return strTempArr;
        }

        public void checkIndex()
        {
            try
            {
                searcher = new IndexSearcher(this.pathIndex);
                searcher.Close();
            }
            catch (IOException)
            {
                FncRebuildIndex(this.FilePath);
                //status("The index doesn't exist or is damaged. Please rebuild it.", true);
                return;
            }

            string msg = String.Format("Index is ready. It contains {0} documents.", searcher.MaxDoc());
            status(msg);
        }

        private void status(string msg)
        {
            status(msg, false);
        }

        private void status(string msg, bool error)
        {
            EntStatus(msg, error);
        }
    }
}
