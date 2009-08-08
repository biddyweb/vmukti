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
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.IO;

namespace rptCRCbyAgent.Presentation
{
    class RdlGenerator
    {
        private List<string> m_allFields;
        private List<string> m_selectedFields;

        public List<string> AllFields
        {
            get { return m_allFields; }
            set { m_allFields = value; }
        }

        public List<string> SelectedFields
        {
            get { return m_selectedFields; }
            set { m_selectedFields = value; }
        }
        
        private Rdl.Report CreateReport()
        {
            Rdl.Report report = new Rdl.Report();
            report.Items = new object[] 
                {
                    CreateDataSources(),
                    CreateBody(),
                    CreateDataSets(),
                    "6.5in",
                };
            report.ItemsElementName = new Rdl.ItemsChoiceType37[]
                { 
                    Rdl.ItemsChoiceType37.DataSources, 
                    Rdl.ItemsChoiceType37.Body,
                    Rdl.ItemsChoiceType37.DataSets,
                    Rdl.ItemsChoiceType37.Width,
                };
            return report;
        }

        private Rdl.DataSourcesType CreateDataSources()
        {
            Rdl.DataSourcesType dataSources = new Rdl.DataSourcesType();
            dataSources.DataSource = new Rdl.DataSourceType[] { CreateDataSource() };
            return dataSources;
        }

        private Rdl.DataSourceType CreateDataSource()
        {
            Rdl.DataSourceType dataSource = new Rdl.DataSourceType();
            dataSource.Name = "DummyDataSource";
            dataSource.Items = new object[] { CreateConnectionProperties() };
            return dataSource;
        }

        private Rdl.ConnectionPropertiesType CreateConnectionProperties()
        {
            Rdl.ConnectionPropertiesType connectionProperties = new Rdl.ConnectionPropertiesType();
            connectionProperties.Items = new object[]
                {
                    "",
                    "SQL",
                };
            connectionProperties.ItemsElementName = new Rdl.ItemsChoiceType[]
                {
                    Rdl.ItemsChoiceType.ConnectString,
                    Rdl.ItemsChoiceType.DataProvider,
                };
            return connectionProperties;
        }

        private Rdl.BodyType CreateBody()
        {
            Rdl.BodyType body = new Rdl.BodyType();
            body.Items = new object[]
                {
                    CreateReportItems(),
                    "1in",
                };
            body.ItemsElementName = new Rdl.ItemsChoiceType30[]
                {
                    Rdl.ItemsChoiceType30.ReportItems,
                    Rdl.ItemsChoiceType30.Height,
                };
            return body;
        }

        private Rdl.ReportItemsType CreateReportItems()
        {
            Rdl.ReportItemsType reportItems = new Rdl.ReportItemsType();
            TableRdlGenerator tableGen = new TableRdlGenerator();
            tableGen.Fields = m_selectedFields;
            reportItems.Items = new object[] { tableGen.CreateTable() };
            return reportItems;
        }

        private Rdl.DataSetsType CreateDataSets()
        {
            Rdl.DataSetsType dataSets = new Rdl.DataSetsType();
            dataSets.DataSet = new Rdl.DataSetType[] { CreateDataSet() };
            return dataSets;
        }

        private Rdl.DataSetType CreateDataSet()
        {
            Rdl.DataSetType dataSet = new Rdl.DataSetType();
            dataSet.Name = "MyData";
            dataSet.Items = new object[] { CreateQuery(), CreateFields() };
            return dataSet;
        }

        private Rdl.QueryType CreateQuery()
        {
            Rdl.QueryType query = new Rdl.QueryType();
            query.Items = new object[] 
                {
                    "DummyDataSource",
                    "",
                };
            query.ItemsElementName = new Rdl.ItemsChoiceType2[]
                {
                    Rdl.ItemsChoiceType2.DataSourceName,
                    Rdl.ItemsChoiceType2.CommandText,
                };
            return query;
        }

        private Rdl.FieldsType CreateFields()
        {
            Rdl.FieldsType fields = new Rdl.FieldsType();

            fields.Field = new Rdl.FieldType[m_allFields.Count];
            for (int i = 0; i < m_allFields.Count; i++)
            {
                fields.Field[i] = CreateField(m_allFields[i]);
            }
            return fields;
        }

        private Rdl.FieldType CreateField(String fieldName)
        {
            Rdl.FieldType field = new Rdl.FieldType();
            field.Name = fieldName;
            field.Items = new object[] { fieldName };
            field.ItemsElementName = new Rdl.ItemsChoiceType1[] { Rdl.ItemsChoiceType1.DataField };
            return field;
        }

        public void WriteXml(Stream stream)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(Rdl.Report));
            serializer.Serialize(stream, CreateReport());
        }


    }
}
