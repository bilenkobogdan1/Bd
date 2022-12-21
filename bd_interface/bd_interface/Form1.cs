using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace bd_interface
{
    public partial class Form1 : Form
    {
        int ColumnIndex = 0;
        List<TabPage> pageList = new List<TabPage>();
        List<DataGridView> DataGridViewList = new List<DataGridView>();
        List<Row> changeRow = new List<Row>();
        System.Windows.Forms.DataGridView txt;
        TabPage page;
        Database db;
        string path;
        Table table;
        Row row;
        bool open = true;
        public Form1()
        {
            InitializeComponent();
        }
        public void CreatedataGrid(string name)
        {
            System.Windows.Forms.Button buttonAddColumns = new System.Windows.Forms.Button();
            buttonAddColumns.Location = new System.Drawing.Point(10, 10);
            buttonAddColumns.Name = "Column1" + ColumnIndex.ToString();
            buttonAddColumns.Size = new System.Drawing.Size(105, 23);
            buttonAddColumns.TabIndex = 3;
            buttonAddColumns.Text = "Додати рядок";
            buttonAddColumns.UseVisualStyleBackColor = true;

            txt = new System.Windows.Forms.DataGridView();
            txt.Size = new Size(900, 300);
            txt.Location = new Point(10, 30);
            txt.Name = "Column1" + ColumnIndex.ToString();
            //   DataGridViewColumn Column1 = new DataGridViewTextBoxColumn();
            //   Column1.Name = "Column1" + ColumnIndex.ToString();
            //  Column1.HeaderText = "Column1" + ColumnIndex.ToString();
            //   txt.Columns.Add(Column1);
            page = new TabPage(name);
            page.Name = "Column1" + ColumnIndex.ToString();
            page.Controls.Add(txt);
            page.Controls.Add(buttonAddColumns);
            tabControl1.TabPages.Add(page);
            pageList.Add(page);
            DataGridViewList.Add(txt);
            buttonAddColumns.Click += new System.EventHandler(buttonAddColumnsClick);
            ColumnIndex++;
            txt.CellValueChanged += Txt_CellValueChanged;


        }

        private void Txt_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (open)
            {
                string value = "";
                int IndexTable = 0;
                int i = 0;

                bool createColumns = true;
                foreach (var Grid in DataGridViewList)
                {
                    IndexTable++;
                    if (tabControl1.SelectedTab.Name == Grid.Name)
                    {
                        value = Grid.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString();
                        break;
                    }

                }
                foreach (var table in db.Tables)
                {
                    i++;
                    if (i == IndexTable)
                    {
                        int j = 0;
                        foreach (var column in table.Columns)
                        {
                            if (j == e.ColumnIndex)
                            {
                                if (column.Validate(value))
                                {
                                    if ((e.RowIndex + 1) > table.Rows.Count)
                                    {
                                        Row rmax = new Row(e.RowIndex + 1);
                                        foreach (var colum in table.Columns)
                                        {
                                            rmax.Values.Add(" ");

                                        }
                                        table.Rows.Add(rmax);
                                    }

                                    int countRows = 0; int countColumns = 0;
                                    foreach (var row in table.Rows)
                                    {

                                        foreach (var ro in row.Values)
                                        {
                                            if (e.RowIndex == countRows && e.ColumnIndex == countColumns)
                                            {
                                                row.Values[countColumns] = value; break;
                                            }
                                            countColumns++;
                                        }
                                        countColumns = 0;
                                        countRows++;
                                    }



                                }
                                else { MessageBox.Show("Не коректно введенні данні"); value = ""; }
                            }
                            j++;
                        }

                    }
                }
                IndexTable = 0;
                foreach (var Grid in DataGridViewList)
                {
                    IndexTable++;
                    if (tabControl1.SelectedTab.Name == Grid.Name)
                    {
                        Grid.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = value;
                        break;
                    }

                }
            }
        }


        private void buttonAddColumnsClick(object sender, EventArgs e)
        {
            string nameColums = Interaction.InputBox("Назва", "Рядок", "Рядок 1");
            string TypeColums = Interaction.InputBox("Введіть тип данних INT, REAL, CHAR, STRING,TIME,INT INTERVAL", "Тип данних", "INT");
            int IndexTable = 0;
            int i = 0;
            bool createColumns = true;
            foreach (var Grid in DataGridViewList)
            {
                IndexTable++;
                if (tabControl1.SelectedTab.Name == Grid.Name)
                { break; }
            }
            foreach (var table in db.Tables)
            {
                i++;
                if (i == IndexTable)
                {
                    if (TypeColums == "INT")
                    {
                        Column colomn = new IntColumn(nameColums);
                        table.Columns.Add(colomn); createColumns = false;
                    }
                    if (TypeColums == "REAL")
                    {
                        Column colomn = new RealColumn(nameColums);
                        table.Columns.Add(colomn); createColumns = false;
                    }
                    if (TypeColums == "CHAR")
                    {
                        Column colomn = new CharColumn(nameColums);
                        table.Columns.Add(colomn); createColumns = false;
                    }
                    if (TypeColums == "STRING")
                    {
                        Column colomn = new StringColumn(nameColums);
                        table.Columns.Add(colomn); createColumns = false;
                    }
                    if (TypeColums == "TIME")
                    {
                        Column colomn = new TimeColumn(nameColums);
                        table.Columns.Add(colomn); createColumns = false;
                    }
                    if (TypeColums == "INT INTERVAL")
                    {
                        Column colomn = new IntIntervalColumn(nameColums);
                        table.Columns.Add(colomn); createColumns = false;
                    }

                }
            }
            if (!createColumns)
            {        
                foreach (var Grid in DataGridViewList)
                {    

                    if (tabControl1.SelectedTab.Name == Grid.Name)
                    {    
                        DataGridViewColumn Column1 = new DataGridViewTextBoxColumn();
                        Column1.Name = "Таблиця " + ColumnIndex.ToString();
                        Column1.HeaderText = nameColums + " (" + TypeColums + ")";
                        Grid.Columns.Add(Column1);
                        break;
                    }
                }   
            }
            else { MessageBox.Show("Некоректний тип данних"); createColumns = false; }



        }
        private void створитиToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string input = Interaction.InputBox("База данних", "Назва", "База данних 1");
            if (input == "")
            {
                input = "База данних 1";
            }
            else
            {
                таблицяToolStripMenuItem.Visible = true;
                DatabaseMeneger dbMeneger = DatabaseMeneger.getInstance();
                db = new Database(input);
                dbMeneger.createDatabase(db);
                path = @"C:\БД\" + input + ".xml";


            }

            foreach (var page in pageList)
            {
                tabControl1.TabPages.Remove(page);
            }
            ColumnIndex = 0;
            pageList.Clear();
            DataGridViewList.Clear();
        }

        private void відкритиToolStripMenuItem_Click(object sender, EventArgs e)
        {
            open = false;
            if (openFileDialog1.ShowDialog() == DialogResult.Cancel)
                return;
            foreach (var page in pageList)
            {
                tabControl1.TabPages.Remove(page);
            }
            DataGridViewList.Clear();
            pageList.Clear();
            таблицяToolStripMenuItem.Visible = true;
            // получаем выбранный файл
            path = openFileDialog1.FileName;
            // читаем файл в строку
            string fileText = System.IO.File.ReadAllText(path);
            DatabaseMeneger dbMeneger = DatabaseMeneger.getInstance();
            dbMeneger.OpenDataBase(path);
            string NameDatabase;
            NameDatabase = Path.GetFileName(path);
            db = new Database(NameDatabase);
            List<string> nameTables = new List<string>();
            dbMeneger.openDatabaseNameTables(path, ref nameTables);
            foreach (var nameTable in nameTables)
            {
                Table table = new Table(nameTable);
                db.Tables.Add(table);
                List<Column> nameColums = new List<Column>();
                CreatedataGrid(nameTable);
                dbMeneger.openDatabaseNameСolumns(path, ref nameColums, nameTable);
                foreach (var nameColum in nameColums)
                {
                    table.Columns.Add(nameColum);
                    DataGridViewColumn Column1 = new DataGridViewTextBoxColumn();
                    Column1.Name = nameColum.Name;
                    Column1.HeaderText = nameColum.Name + "(" + nameColum.Type + ")";
                    txt.Columns.Add(Column1);




                }
                int CountRows = 0;
                dbMeneger.CountRowsinTable(path, nameTable, ref CountRows);
                int RowIndex = 0;
                for (int i = 0; i < CountRows; i++)
                {
                    Row row = new Row(i);
                    dbMeneger.openDatabaseValueRows(path, nameTable, ref row);
                    table.Rows.Add(row);
                    txt.Rows.Add(row);
                    foreach (var r in row.Values)
                    {
                        txt.Rows[i].Cells[RowIndex].Value = r;
                        //  txt.Rows[1].Cells[RowIndex].Value = r;
                        RowIndex++;
                    }
                    RowIndex = 0;

                }


            }
            open = true;
        }

        private void видалитиToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.Cancel)
                return;
            // получаем выбранный файл
            path = openFileDialog1.FileName;
            DatabaseMeneger dbMeneger = DatabaseMeneger.getInstance();
            dbMeneger.delateDatabase(path);
        }

        private void зберегтиToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DatabaseMeneger dbMeneger = DatabaseMeneger.getInstance();
           dbMeneger.saveDatabase(path, db);
        }

        private void створитиToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            string nameTable = Interaction.InputBox("Таблиця", "Назва", "Таблиця 1");
            if (nameTable == "")
            {
                nameTable = "Таблиця";
            }
            else
            {
                CreatedataGrid(nameTable);
            }
            table = new Table(nameTable);
            db.Tables.Add(table);
        }

        private void видалитиToolStripMenuItem1_Click(object sender, EventArgs e)
        {
          
            foreach (var item in db.Tables)
            {
                if (item.Name == tabControl1.SelectedTab.Text)
                {
                   
                    db.Tables.Remove(item); 
                    tabControl1.TabPages.RemoveByKey(tabControl1.SelectedTab.Name);break;

                }
            }
        }

        private void пошукЗаШаблономToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form2 Form = new Form2();
            Form.ShowDialog();
            
        }

        private void базаДаннихToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }
    }
}
