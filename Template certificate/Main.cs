using SelectPdf;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Printing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Template_certificate
{
    public partial class Main : Form
    {
        private string Excel03ConString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source={0};Extended Properties='Excel 8.0;HDR=YES'";
        private string Excel07ConString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source={0};Extended Properties='Excel 12.0;HDR=YES'";
        private CheckBox headerCheckBox = new CheckBox();
        private string connectionString = null;
        private readonly string contentHtml = File.ReadAllText("C:\\Generate certificate\\Html source\\certificate template.html");

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

        public Main()
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

                //auto selected all row
                headerCheckBox.Checked = true;
                HeaderCheckBox_Clicked(null, null);
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
            field2.Enabled = false;
            field2.SelectedIndex = -1;
            setDataSource(field3, headers);
            field3.Enabled = false;
            field3.SelectedIndex = -1;

        }

        private void setDataSource(ComboBox field, List<string> headers)
        {
            field.BindingContext = new BindingContext();
            field.DataSource = headers;
        }

        private void displayExcelContentToGridView(string fileName)
        {
            dataGridView1.DataSource = null;
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
                setDataSourceForGridView("SELECT * From [Sheet1$]");

                //add check box column to data grid view
                //Add a CheckBox Column to the DataGridView Header Cell.

                //Find the Location of Header Cell.
                Point headerCellLocation = this.dataGridView1.GetCellDisplayRectangle(0, -1, true).Location;

                //Place the Header CheckBox in the Location of the Header Cell.
                headerCheckBox.Location = new Point(headerCellLocation.X + 8, headerCellLocation.Y + 2);
                headerCheckBox.BackColor = System.Drawing.Color.White;
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

        private void setDataSourceForGridView(string query)
        {
            try
            {
                using (OleDbConnection con = new OleDbConnection(connectionString))
                {
                    using (OleDbCommand cmd = new OleDbCommand())
                    {
                        using (OleDbDataAdapter oda = new OleDbDataAdapter())
                        {
                            DataTable dt = new DataTable();
                            cmd.CommandText = query;
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
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Something went wrong", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                //user selected folder
                //set folder path to text box
                folderPath.Text = folderBrowserDialog1.SelectedPath;
            }
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void resetFilterBtn_Click(object sender, EventArgs e)
        {
            field1.SelectedIndex = 0;

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
            string query = "";
            //get the data to sort
            if (field1.SelectedIndex == 0)
            {
                //user want to show all
                query = "SELECT * From [Sheet1$]";
            }
            else
            {
                queryCondition += getSortData(field1, comparison1, compare1) + " ";
                if (field2.Enabled && comparison2.Enabled)
                {
                    queryCondition += and_orCbx1.SelectedItem + " ";
                    queryCondition += getSortData(field2, comparison2, compare2);
                }
                if (field3.Enabled && comparison3.Enabled)
                {
                    queryCondition += and_orCbx2.SelectedItem + " ";
                    queryCondition += getSortData(field3, comparison3, compare3);
                }
                query = $"SELECT * From [Sheet1$] where {queryCondition}";
            }
            MessageBox.Show(query);
            setDataSourceForGridView(query);

            //reset all field to default
            resetFilterBtn_Click(null, null);
            queryCondition = "";
        }

        private string getSortData(ComboBox field, ComboBox comparisonCbx, TextBox compare)
        {
            string query = "";
            string compareTxt = compare.Text.Trim();
            if (field.SelectedItem != null)
            {
                query += $"[{field.SelectedItem}] ";
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
                            query += $" = {compareTxt}";
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
            if (field1.SelectedIndex > 0)
            {
                compare1.Enabled = true;
                comparison1.Enabled = true;
                comparison1.SelectedIndex = 0;
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
            if (field2.SelectedIndex > 0)
            {
                compare2.Enabled = true;
                comparison2.Enabled = true;
                comparison2.SelectedIndex = 0;

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
            if (field3.SelectedIndex > 0)
            {
                compare3.Enabled = true;
                comparison3.Enabled = true;
                comparison3.SelectedIndex = 0;

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
            else
            {
                switch (comboBox.Name)
                {
                    case "comparison1":
                        {
                            compare1.Enabled = true;
                            break;
                        }
                    case "comparison2":
                        {
                            compare2.Enabled = true;
                            break;
                        }
                    default:
                        {
                            compare3.Enabled = true;
                            break;
                        }
                }
            }
        }

        private void and_orCbx1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (and_orCbx1.SelectedIndex >= 0)
            {
                field2.Enabled = true;
                field2.SelectedIndex = 0;
            }
            else
            {
                field2.Enabled = false;
                field2.SelectedIndex = -1;
            }
        }

        private void and_orCbx2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (and_orCbx2.SelectedIndex >= 0)
            {
                field3.Enabled = true;
                field3.SelectedIndex = 0;
            }
            else
            {
                field3.Enabled = false;
                field3.SelectedIndex = -1;
            }
        }

        private void cancelBtn_Click(object sender, EventArgs e)
        {
            Main dialog = new Main();
            dialog.Show();
            //this.Close();
        }

        private void renderPdfBtn_Click(object sender, EventArgs e)
        {

            string folderStoragePath = folderPath.Text;
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {

                int count = 0;
                //check if row has checked in check box
                if (Convert.ToBoolean(row.Cells["checkBoxColumn"].Value))
                {
                    
                    //render this row to pdf
                    string studentName = row.Cells["Họ và tên"].Value.ToString();
                    string studentId = row.Cells["Mã sinh viên"].Value.ToString();

                    DateTime date = Convert.ToDateTime(row.Cells["Ngày hoàn thành "].Value);
                    string finishedDate = date.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);

                    string ccVnName = row.Cells["Tên chứng chỉ"].Value.ToString().Trim().Replace("Chứng chỉ ", "").Trim();
                    ccVnName = char.ToUpper(ccVnName.First()) + ccVnName.Substring(1);

                    string ccEnName = row.Cells["Tên chứng chỉ (tiếng anh)"].Value.ToString();
                    string ccNumber = row.Cells["Số CC"].Value.ToString();
                    GeneratePdf(studentName, studentId, finishedDate, ccVnName, ccEnName, ccNumber, folderStoragePath);
                    count++;
                }
            }
            MessageBox.Show("Generate successfull", "Successfull", MessageBoxButtons.OK, MessageBoxIcon.Information);
            Process.Start(folderStoragePath);

        }

        private void GeneratePdf(string studentName, string studentId, string finishedDate, string ccVnName, string ccEnName, string ccNumber, string folderStoragePath)
        {
            try
            {
                string[] parameters = { studentName, ccVnName, ccEnName, ccNumber, finishedDate };
                //string filename = "E:/Funix/Template certificate/certificate template 1.pdf";
                string fileName = $"{folderStoragePath.Replace("\\", "/")}/{studentId}-{ccNumber}-{studentName}.pdf";

                string html = string.Format(contentHtml, parameters);
                // define a rendering result object
                SelectPdf.HtmlToPdf converter = new SelectPdf.HtmlToPdf();

                //setting up rendering
                converter.Options.PdfPageOrientation = PdfPageOrientation.Landscape;
                converter.Options.PdfPageSize = PdfPageSize.A4;
                converter.Options.DrawBackground = true;
                converter.Options.EmbedFonts = true;
                //following margin make content in center of page

                //white out line very thin
                //converter.Options.MarginLeft = -15;
                //converter.Options.MarginRight = -15;
                //converter.Options.MarginTop = -5;

                //white outline half of cm
                converter.Options.MarginLeft = -7;
                converter.Options.MarginRight = -10;

                converter.Options.AutoFitHeight = HtmlToPdfPageFitMode.AutoFit;
                converter.Options.AutoFitWidth = HtmlToPdfPageFitMode.AutoFit;

                PdfDocument doc = converter.ConvertHtmlString(html);

                doc.Save(fileName);
                doc.Close();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Some things went wrong", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
