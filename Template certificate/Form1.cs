﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Template_certificate
{
    public partial class Form1 : Form
    {
        private string Excel03ConString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source={0};Extended Properties='Excel 8.0;HDR=YES'";
        private string Excel07ConString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source={0};Extended Properties='Excel 12.0;HDR=YES'";
        private CheckBox headerCheckBox = new CheckBox();
        private enum comparison
        {
            Equal_to,
            Not_equal_to,
            Greater_than,
            Less_than,
            Less_than_or_equal,
            Greater_than_or_equal,
            Is_blank,
            Is_not_blank,
            Contains,
            Does_not_contain,
        }
        private string queryCondition = "";

        public Form1()
        {
            InitializeComponent();
            groupBox1.Enabled = false;
        }

        private void chooseFileBtn_Click(object sender, EventArgs e)
        {
            //init setting for user can only choosing excel file
            openFileDialog1.CheckFileExists = true;
            openFileDialog1.CheckPathExists = true;
            openFileDialog1.FileName = "";
            openFileDialog1.SupportMultiDottedExtensions = true;
            openFileDialog1.Filter = "Excel file| *.xls;*.xlsx";
            openFileDialog1.Title = "Select excel file";

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                //user selected file
                //set file name to text box
                excelPath.Text = openFileDialog1.SafeFileName;

                //display file content in grid view
                displayExcelContentToGridView(openFileDialog1.FileName);

                //display header to combo box for filter
                displayHeaderToCbx(openFileDialog1.FileName);

                groupBox1.Enabled = true;
            }
        }

        private void displayHeaderToCbx(string fileName)
        {
            List<string> headers = new List<string>();
            headers.Add("none");
            //the first column of data grid view is check box 
            // start collect header from second column
            for (int i = 1; i < dataGridView1.Columns.Count; i++)
            {
                headers.Add(dataGridView1.Columns[i].HeaderText);
            }
            setDataSource(field1, headers);
            setDataSource(field2, headers);
            setDataSource(field3, headers);

        }

        private void setDataSource(ComboBox field, List<string> headers)
        {
            field.BindingContext = new BindingContext();
            field.DataSource = headers;
        }

        private void displayExcelContentToGridView(string fileName)
        {
            dataGridView1.DataSource = null;
            string connectionString = null;
            if (Path.GetExtension(fileName).Equals(".xls"))
            {
                //excel old version. Before 2007
                connectionString = string.Format(Excel03ConString, fileName);
            }
            else
            {
                connectionString = string.Format(Excel07ConString, fileName);
            }
            try
            {
                using (OleDbConnection con = new OleDbConnection(connectionString))
                {
                    using (OleDbCommand cmd = new OleDbCommand())
                    {
                        using (OleDbDataAdapter oda = new OleDbDataAdapter())
                        {
                            DataTable dt = new DataTable();
                            cmd.CommandText = "SELECT * From [Sheet1$]";
                            cmd.Connection = con;
                            con.Open();
                            oda.SelectCommand = cmd;
                            oda.Fill(dt);
                            con.Close();

                            //Populate DataGridView.
                            dataGridView1.DataSource = dt;
                        }
                    }
                }
            
                //add check box column to data grid view
                //Add a CheckBox Column to the DataGridView Header Cell.

                //Find the Location of Header Cell.
                Point headerCellLocation = this.dataGridView1.GetCellDisplayRectangle(0, -1, true).Location;

                //Place the Header CheckBox in the Location of the Header Cell.
                headerCheckBox.Location = new Point(headerCellLocation.X + 8, headerCellLocation.Y + 2);
                headerCheckBox.BackColor = Color.White;
                headerCheckBox.Size = new Size(18, 18);

                //Assign Click event to the Header CheckBox.
                headerCheckBox.Click += new EventHandler(HeaderCheckBox_Clicked);
                dataGridView1.Controls.Add(headerCheckBox);

                //Add a CheckBox Column to the DataGridView at the first position.
                DataGridViewCheckBoxColumn checkBoxColumn = new DataGridViewCheckBoxColumn();
                checkBoxColumn.HeaderText = "";
                checkBoxColumn.Width = 30;
                checkBoxColumn.Name = "checkBoxColumn";
                dataGridView1.Columns.Insert(0, checkBoxColumn);
                this.dataGridView1.Columns["checkBoxColumn"].Frozen = true;
                //Assign Click event to the DataGridView Cell.
                dataGridView1.CellContentClick += new DataGridViewCellEventHandler(DataGridView_CellClick);

            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void HeaderCheckBox_Clicked(object sender, EventArgs e)
        {
            //Necessary to end the edit mode of the Cell.
            //dataGridView1.EndEdit();

            //Loop and check and uncheck all row CheckBoxes based on Header Cell CheckBox.
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                DataGridViewCheckBoxCell checkBox = (row.Cells["checkBoxColumn"] as DataGridViewCheckBoxCell);
                checkBox.Value = headerCheckBox.Checked;
            }
            dataGridView1.EndEdit();
        }

        private void DataGridView_CellClick(object sender, DataGridViewCellEventArgs e)
        {

            //Check to ensure that the row CheckBox is clicked.
            if (e.RowIndex >= 0 && e.ColumnIndex == 0)
            {
                DataGridViewCheckBoxCell boxCell = dataGridView1.Rows[e.RowIndex].Cells["checkBoxColumn"] as DataGridViewCheckBoxCell;
                boxCell.Value = !Convert.ToBoolean(boxCell.Value);
                //Loop to verify whether all row CheckBoxes are checked or not.

                bool isChecked = true;
                foreach (DataGridViewRow row in dataGridView1.Rows)
                {
                    if (Convert.ToBoolean(row.Cells["checkBoxColumn"]
                        .EditedFormattedValue) == false)
                    {
                        isChecked = false;
                        break;
                    }
                }
                headerCheckBox.Checked = isChecked;
            }
        }

        private void chooseFolder_Click(object sender, EventArgs e)
        {
            //init setting for user can only choosing excel file
            folderBrowserDialog1.ShowNewFolderButton = false;


            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                //user selected file
                //set file name to text box
                folderPath.Text = folderBrowserDialog1.SelectedPath;

                //display file content in grid view
                //displayExcelContentToGridView(openFileDialog1.FileName);
            }
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void resetFilterBtn_Click(object sender, EventArgs e)
        {
            field1.SelectedIndex = 0;
            field2.SelectedIndex = 0;
            field3.SelectedIndex = 0;

            resetCombobox(and_orCbx1);
            resetCombobox(and_orCbx2);

            resetCombobox(comparison1);
            resetCombobox(comparison2);
            resetCombobox(comparison3);

            resetTextField(compare1);
            resetTextField(compare2);
            resetTextField(compare3);
        }

        private void resetTextField(TextBox txt)
        {
            txt.Text = "";
        }

        private void resetCombobox(ComboBox cbx)
        {
            cbx.SelectedIndex = -1;

        }

        private void filterBtn_Click(object sender, EventArgs e)
        {
            //get the data to sort

            queryCondition += getSortData(field1, comparison1, compare1) + " ";
            if (field2.Enabled && comparison2.Enabled)
            {

                queryCondition += getSortData(field2, comparison2, compare2);
            }
            if (field3.Enabled && comparison3.Enabled)
            {
                queryCondition += getSortData(field3, comparison3, compare3);
            }
            //reset all field to default
            MessageBox.Show(queryCondition);
            resetFilterBtn_Click(null, null);
            queryCondition = "";
        }

        private string getSortData(ComboBox field, ComboBox comparisonCbx, TextBox compare)
        {
            string query = "";
            string compareTxt = compare.Text.Trim();
            if (field.SelectedItem != null)
            {
                query += field.SelectedItem;
                switch (comparisonCbx.SelectedIndex)
                {
                    case (int)comparison.Contains:
                        {
                            query += $"like '%{compareTxt}%' ";
                            break;
                        }
                    case (int)comparison.Does_not_contain:
                        {
                            query += $"not like '%{compareTxt}%' ";
                            break;
                        }
                    case (int)comparison.Equal_to:
                        {
                            query += $" = '{compareTxt}' ";
                            break;
                        }
                    case (int)comparison.Greater_than:
                        {
                            double number;
                            try
                            {
                                number = Convert.ToDouble(compareTxt);
                            }
                            catch (Exception)
                            {
                                number = 0;
                            }
                            query += $"> {number} ";

                            break;
                        }
                    case (int)comparison.Greater_than_or_equal:
                        {
                            double number;
                            try
                            {
                                number = Convert.ToDouble(compareTxt);
                            }
                            catch (Exception)
                            {
                                number = 0;
                            }
                            query += $">= {number} ";
                            break;
                        }
                    case (int)comparison.Is_blank:
                        {
                            query += "is null ";
                            break;
                        }
                    case (int)comparison.Is_not_blank:
                        {
                            query += "is not null ";
                            break;
                        }
                    case (int)comparison.Less_than:
                        {
                            double number;
                            try
                            {
                                number = Convert.ToDouble(compareTxt);
                            }
                            catch (Exception)
                            {
                                number = 0;
                            }
                            query += $"< {number} ";
                            break;
                        }
                    case (int)comparison.Less_than_or_equal:
                        {
                            double number;
                            try
                            {
                                number = Convert.ToDouble(compareTxt);
                            }
                            catch (Exception)
                            {
                                number = 0;
                            }
                            query += $"<= {number} ";
                            break;
                        }
                    case (int)comparison.Not_equal_to:
                        {
                            double number;
                            try
                            {
                                number = Convert.ToDouble(compareTxt);
                            }
                            catch (Exception)
                            {
                                number = 0;
                            }
                            query += $"not = {number} ";
                            break;
                        }
                }
            }
            return query;
        }

        private void field1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (field1.SelectedIndex != 0)
            {
                compare1.Enabled = true;
                comparison1.Enabled = true;
                and_orCbx1.Enabled = true;
                and_orCbx1.SelectedIndex = 0;
            }
            else
            {
                compare1.Enabled = false;
                comparison1.Enabled = false;
                and_orCbx1.SelectedIndex = -1;
                and_orCbx1.Enabled = false;
            }
        }

        private void field2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (field2.SelectedIndex != 0)
            {
                compare2.Enabled = true;
                comparison2.Enabled = true;
                and_orCbx2.Enabled = true;
                and_orCbx2.SelectedIndex = 0;
            }
            else
            {
                compare2.Enabled = false;
                comparison2.Enabled = false;
                and_orCbx2.SelectedIndex = -1;
                and_orCbx2.Enabled = false;
            }
        }

        private void field3_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (field3.SelectedIndex != 0)
            {
                compare3.Enabled = true;
                comparison3.Enabled = true;
            }
            else
            {
                compare3.Enabled = false;
                comparison3.Enabled = false;
            }
        }

        private void comparison1_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox comboBox = (ComboBox)sender;
            if (comboBox.SelectedItem == null)
            {
                return;
            }
            else if (comboBox.SelectedItem.Equals("Is blank") || comboBox.SelectedItem.Equals("Is not blank"))
            {
                switch (comboBox.Name)
                {
                    case "comparison1":
                        {
                            compare1.Enabled = false;
                            break;
                        }
                    case "comparison2":
                        {
                            compare2.Enabled = false;
                            break;
                        }
                    default:
                        {
                            compare3.Enabled = false;
                            break;
                        }
                }
            }
        }

        private void and_orCbx1_SelectedIndexChanged(object sender, EventArgs e)
        {
            field2.Enabled = true;
        }

        private void and_orCbx2_SelectedIndexChanged(object sender, EventArgs e)
        {
            field3.Enabled = true;

        }
    }
}