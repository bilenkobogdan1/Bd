using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using System.Xml.XPath;

namespace bd_interface
{
    internal class DatabaseMeneger
    {
        private static DatabaseMeneger instance;

        private DatabaseMeneger()
        { }

        public static DatabaseMeneger getInstance()
        {
            if (instance == null)
                instance = new DatabaseMeneger();
            return instance;
        }
        public void OpenDataBase(string path)
        {
            string text = File.ReadAllText(path);
        }
        public void createDatabase(Database db)
        {
            FileStream sw = File.Create(@"C:\БД\" + db.Name + ".xml");
            sw.Close();
        }
        public void delateDatabase(string path)
        {
            File.Delete(path);

        }
        public void saveDatabase(string path, Database db)
        {
            string text = "<?xml version=\"1.0\" encoding=\"utf-8\"?>\n<base>\n</base>";

            StreamWriter writer = new StreamWriter(path, false);
            writer.WriteLineAsync(text);
            writer.Close();

            XmlDocument xDoc = new XmlDocument();
            xDoc.Load(path);
            XmlElement xRoot = xDoc.DocumentElement;
            foreach (var table in db.Tables)
            {
                XmlElement tableElem = xDoc.CreateElement("table");
                XmlAttribute nameTable = xDoc.CreateAttribute("name");
                XmlText nameText = xDoc.CreateTextNode(table.Name);
                tableElem.Attributes.Append(nameTable);
                nameTable.AppendChild(nameText);
                xRoot?.AppendChild(tableElem);
                int i = 0;
                foreach (var column in table.Columns)
                {
                    XmlElement columnElem = xDoc.CreateElement("column");
                    XmlAttribute nameColumn = xDoc.CreateAttribute("name");
                    XmlAttribute typeColumn = xDoc.CreateAttribute("type");
                    XmlText nameTextColumn = xDoc.CreateTextNode(column.Name);
                    XmlText typeTextColumn = xDoc.CreateTextNode(column.Type);
                    columnElem.Attributes.Append(nameColumn);
                    columnElem.Attributes.Append(typeColumn);
                    nameColumn.AppendChild(nameTextColumn);
                    typeColumn.AppendChild(typeTextColumn);
                    tableElem.AppendChild(columnElem);
                    int j = 0;
                    foreach (var row in table.Rows)
                    {

                        XmlElement rowElem = xDoc.CreateElement("Row");
                        XmlAttribute idRow = xDoc.CreateAttribute("id");
                        XmlText indexidRow = xDoc.CreateTextNode(j.ToString());
                        XmlText valueRowxml = xDoc.CreateTextNode(row[i]);
                        rowElem.Attributes.Append(idRow);
                        rowElem.AppendChild(valueRowxml);
                        idRow.AppendChild(indexidRow);
                        columnElem.AppendChild(rowElem);
                        j++;



                    }
                    i++;
                }

            }
            xDoc.Save(path);
        }
        public void openDatabaseNameTables(string path, ref List<string> nameTables)
        {
            var xDoc = XDocument.Load(path);
            var vname = xDoc.XPathSelectElements("base");
            foreach (var table in vname.Elements("table"))
            {
                XAttribute name = table.Attribute("name");
                nameTables.Add(name.Value);
            }

        }
        public void openDatabaseNameСolumns(string path, ref List<Column> nameColumns, string nameTable)
        {
            var xDoc = XDocument.Load(path);
            var vname = xDoc.XPathSelectElements("base");
            Column column = new CharColumn("");
            foreach (var table in vname.Elements("table"))
            {
                XAttribute name1 = table.Attribute("name");
                if (name1.Value == nameTable)
                {
                    foreach (var k in table.Elements("column"))
                    {
                        XAttribute name = k.Attribute("name");
                        XAttribute type = k.Attribute("type");
                        if (type.Value == "INT") { column = new IntColumn(name.Value); }
                        if (type.Value == "REAL") { column = new RealColumn(name.Value); }
                        if (type.Value == "CHAR") { column = new CharColumn(name.Value); }
                        if (type.Value == "STRING") { column = new StringColumn(name.Value); }
                        if (type.Value == "TIME") { column = new TimeColumn(name.Value); }
                        if (type.Value == "INT INTERVAL") { column = new IntIntervalColumn(name.Value); }

                        nameColumns.Add(column);
                    }
                }
            }




        }
        public void openDatabaseValueRows(string path, string nameTable, ref Row row)
        {
            var xDoc = XDocument.Load(path);
            var vname = xDoc.XPathSelectElements("base");
            Column column = new CharColumn("");
            foreach (var table in vname.Elements("table"))
            {
                XAttribute name1 = table.Attribute("name");
                if (name1.Value == nameTable)
                {
                    foreach (var k in table.Elements("column"))
                    {
                        foreach (var t in k.Elements("Row"))
                        {
                            XAttribute nameindex = t.Attribute("id");

                            if (nameindex.Value == row.id.ToString())
                            {
                                row.Values.Add(t.Value);
                            }
                        }


                    }
                }

            }
        }
        public void CountRowsinTable(string path, string nameTable, ref int CountRows)
        {
            CountRows = 0;
            var xDoc = XDocument.Load(path);
            var vname = xDoc.XPathSelectElements("base");
            Column column = new CharColumn("");
            foreach (var table in vname.Elements("table"))
            {
                XAttribute name1 = table.Attribute("name");
                if (name1.Value == nameTable)
                {
                    foreach (var k in table.Elements("column"))
                    {
                        foreach (var t in k.Elements("Row"))
                        {
                            CountRows++;
                        }
                        break;

                    }
                }

            }
        }
        public void SearchRows(List<string> TypeColumns, ref bool istest, string nameTable)
        {
            bool isContains = true;
            List<string> typexml = new List<string>();
            foreach (string file in Directory.EnumerateFiles(@"C:\БД", "*.xml"))
            {


                var xDoc = XDocument.Load(file);
                var vname = xDoc.XPathSelectElements("base");
                foreach (var table in vname.Elements("table"))
                {
                    if (table.Attribute("name").Value == nameTable)
                    {

                        foreach (var k in table.Elements("column"))
                        {
                            XAttribute type = k.Attribute("type");
                            typexml.Add(type.Value);

                        }
                    }


                }
            }

            foreach (var type in TypeColumns)
            {
                if (typexml.Contains(type))
                {
                    typexml.Remove(type); istest = true;
                }
                else
                {
                    istest = false; break;
                }
            }


        }
        public void SelectRows(string nameTable,ref List<Row> value, List<string> typeP)
        {
            foreach (string file in Directory.EnumerateFiles(@"C:\БД", "*.xml"))
            {

                Row row = new Row(0); ;
                var xDoc = XDocument.Load(file);
                var vname = xDoc.XPathSelectElements("base");
                foreach (var table in vname.Elements("table"))
                {
                    if (table.Attribute("name").Value == nameTable)
                    {

                        foreach (var k in table.Elements("column"))
                        {
                            if (typeP.Contains(k.Attribute("type").Value))
                            {
                               typeP.Remove(k.Attribute("type").Value);
                                row = new Row(1);
                                foreach (var r in k.Elements("Row"))
                                {
                                  
                                    row.Values.Add(r.Value);
                                }
                                value.Add(row);
                            }
                        }
                    }
                }
            }
        }
    }
}
 