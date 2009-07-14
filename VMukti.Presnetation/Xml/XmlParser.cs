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
using System.Xml;
using VMuktiAPI;

namespace VMukti.Presentation.Xml
{
    public class XmlParser
    {
        public XmlMain xMain = new XmlMain();
        public static XmlDocument xmldoc = new XmlDocument();
        public int j = 0;

        public XmlParser()
        {
        }

        public void Parse(string xmlfilename)
        {
            try
            {
            xMain.XmlFileName = xmlfilename;
            xmldoc.Load(xmlfilename);
            AddNode(xmldoc.DocumentElement, ref xMain);
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "Parse()", "Xml\\XmlParser.cs");
            }
        }

        private static void AddNode(XmlNode inXmlNode, ref XmlMain parUsrService)
        {
            try
            {
                XmlNode xNode;
                XmlNodeList nodeList;
                int i;
                if (inXmlNode.Attributes[0].Name == "Name")
                {
                    parUsrService.ModuleName = inXmlNode.Attributes[0].Value;
                }
                if (inXmlNode.HasChildNodes)
                {
                    nodeList = inXmlNode.ChildNodes;
                    for (i = 0; i <= nodeList.Count - 1; i++)
                    {
                        xNode = inXmlNode.ChildNodes[i];
                        if (xNode.Attributes.Count > 1)
                        {
                            for (int j = 0; j < xNode.Attributes.Count; j++)
                            {
                                if (xNode.Attributes[j].Name.ToLower() == "class")
                                {
                                    parUsrService.DummyClassName = xNode.Attributes[j].Value;
                                }
                                if (xNode.Attributes[j].Name.ToLower() == "method")
                                {
                                    parUsrService.DummyMethodName = xNode.Attributes[j].Value;
                                }
                                if (xNode.Attributes[j].Name.ToLower() == "supernodeclass")
                                {
                                    parUsrService.SuperNodeClass = xNode.Attributes[j].Value;
                                }
                                if (xNode.Attributes[j].Name.ToLower() == "assembly")
                                {
                                    parUsrService.Assembly = xNode.Attributes[j].Value;
                                }
                                if (xNode.Attributes[j].Name.ToLower() == "classname")
                                {
                                    parUsrService.P2PDummyClassName = xNode.Attributes[j].Value;
                                }
                                if (xNode.Attributes[j].Name.ToLower() == "methodname")
                                {
                                    parUsrService.P2PDummyMethodName = xNode.Attributes[j].Value;
                                }                                
                            }
                        }
                        else
                        {
                            try
                            {
                                if (xNode.Attributes[0].Name.ToLower() == "swffilename")
                                {
                                    parUsrService.SWFFileName = xNode.Attributes[0].Value;
                                }
                                if (xNode.Attributes[0].Name.ToLower() == "textfilename")
                                {
                                    parUsrService.TextFileName = xNode.Attributes[0].Value;
                                }
                            }
                            catch (Exception ex)
                            {
                                VMuktiHelper.ExceptionHandler(ex, "AddNode()--loading swf file", "XML\\XmlParser.cs");
                            }
                        }

                        AddNode(xNode, ref parUsrService);
                    }

                }
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "AddNode()", "XML\\XmlParser.cs");
            }
        }        
    }
}
