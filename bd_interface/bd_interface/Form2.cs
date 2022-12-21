using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace bd_interface
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }
        List<TextBox> textboxList = new List<TextBox>();
        List<Row> rowListSearch = new List<Row>();
        List<string> typeColumns=new List<string>();
        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                int count = int.Parse(textBox1.Text);
                for (int i = 0; i < count; i++)
                {   Label label = new Label();
                    label.Location = new System.Drawing.Point(4, 65 + 25 * i);
                    label.Text = "Тип данних";
                    TextBox text = new System.Windows.Forms.TextBox();
                    text.Location = new System.Drawing.Point(80, 65+25*i);
                    text.Name = "textbutton"+i.ToString();
                    text.Size = new System.Drawing.Size(60, 23);
                    text.TabIndex = 2;
                    text.Text = "STRING";
                    this.Controls.Add(text);
                    this.Controls.Add(label);
                    textboxList.Add(text);
                }
                Button button = new System.Windows.Forms.Button();
                button.Location = new System.Drawing.Point(5, 65 + 25 * count);
                button.Name = "button";
                button.Size = new System.Drawing.Size(70, 23);
                button.TabIndex = 2;
                button.Text = "Пошук";
                this.Controls.Add(button);
                button.Click += new System.EventHandler(button_Search);

            }
            catch
            {
                MessageBox.Show("Некоректно введені данні");
            }
        }

        private void button_Search(object sender, EventArgs e)
        {
            DatabaseMeneger dbMeneger = DatabaseMeneger.getInstance();
            bool isTest=true;
            bool searchCorect=true;
            string nameTable = Interaction.InputBox("Таблиця", "Назва", "Таблиця 1");
            foreach (var TX in textboxList)
            {
                if(TX.Text =="STRING" || TX.Text == "INT" || TX.Text == "REAL" || TX.Text == "CHAR" || TX.Text == "TIME"||TX.Text == "INT INTERVAL")
                {
                    typeColumns.Add(TX.Text);
                }
                else { MessageBox.Show("Не коректно введені данні введіть тип данних(STRING, INT, REAL, CHAR,TIME,INTERVAL)");
                      searchCorect = false;  break; }
            }
            if (searchCorect) {
                dbMeneger.SearchRows(typeColumns,ref isTest,nameTable);
            }
            if (isTest)
            {
                dataGridView1.Visible = true;
                foreach (var column in typeColumns)
                {
                    DataGridViewColumn Column1 = new DataGridViewTextBoxColumn();
                    Column1.Name = "Таблиця ";
                    Column1.HeaderText = column;
                    dataGridView1.Columns.Add(Column1);
                    

                }
                dbMeneger.SelectRows(nameTable, ref rowListSearch, typeColumns);
                int i = 0, j = 0;
                foreach(var row in rowListSearch)
                {  
                    foreach(var value in row.Values)
                    {
                        if (j <= 0)
                        {
                            dataGridView1.Rows.Add();
                        }
                        dataGridView1.Rows[i].Cells[j].Value = value;

                        i++;

                    }
                    i = 0;
                    j++;
                }
                

            }
            else
            {
                MessageBox.Show("Не знайденно");
            }
            textboxList.Clear();
            rowListSearch.Clear();
            typeColumns.Clear();
        }
    }
}
