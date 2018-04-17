namespace Template_certificate
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.chooseFileBtn = new System.Windows.Forms.Button();
            this.excelPath = new System.Windows.Forms.TextBox();
            this.folderPath = new System.Windows.Forms.TextBox();
            this.chooseFolder = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.filterBtn = new System.Windows.Forms.Button();
            this.resetFilterBtn = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.compare3 = new System.Windows.Forms.TextBox();
            this.compare2 = new System.Windows.Forms.TextBox();
            this.compare1 = new System.Windows.Forms.TextBox();
            this.comparison3 = new System.Windows.Forms.ComboBox();
            this.comparison2 = new System.Windows.Forms.ComboBox();
            this.comparison1 = new System.Windows.Forms.ComboBox();
            this.field3 = new System.Windows.Forms.ComboBox();
            this.field2 = new System.Windows.Forms.ComboBox();
            this.field1 = new System.Windows.Forms.ComboBox();
            this.and_orCbx2 = new System.Windows.Forms.ComboBox();
            this.and_orCbx1 = new System.Windows.Forms.ComboBox();
            this.button2 = new System.Windows.Forms.Button();
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // dataGridView1
            // 
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Location = new System.Drawing.Point(12, 86);
            this.dataGridView1.MultiSelect = false;
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.ReadOnly = true;
            this.dataGridView1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView1.Size = new System.Drawing.Size(548, 180);
            this.dataGridView1.TabIndex = 0;
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // chooseFileBtn
            // 
            this.chooseFileBtn.Location = new System.Drawing.Point(437, 12);
            this.chooseFileBtn.Name = "chooseFileBtn";
            this.chooseFileBtn.Size = new System.Drawing.Size(123, 23);
            this.chooseFileBtn.TabIndex = 1;
            this.chooseFileBtn.Text = "Choose excel file";
            this.chooseFileBtn.UseVisualStyleBackColor = true;
            this.chooseFileBtn.Click += new System.EventHandler(this.chooseFileBtn_Click);
            // 
            // excelPath
            // 
            this.excelPath.Location = new System.Drawing.Point(12, 12);
            this.excelPath.Name = "excelPath";
            this.excelPath.ReadOnly = true;
            this.excelPath.Size = new System.Drawing.Size(407, 20);
            this.excelPath.TabIndex = 2;
            this.excelPath.Click += new System.EventHandler(this.chooseFileBtn_Click);
            // 
            // folderPath
            // 
            this.folderPath.Location = new System.Drawing.Point(12, 47);
            this.folderPath.Name = "folderPath";
            this.folderPath.ReadOnly = true;
            this.folderPath.Size = new System.Drawing.Size(407, 20);
            this.folderPath.TabIndex = 4;
            this.folderPath.Click += new System.EventHandler(this.chooseFolder_Click);
            // 
            // chooseFolder
            // 
            this.chooseFolder.Location = new System.Drawing.Point(437, 47);
            this.chooseFolder.Name = "chooseFolder";
            this.chooseFolder.Size = new System.Drawing.Size(123, 23);
            this.chooseFolder.TabIndex = 3;
            this.chooseFolder.Text = "Choose folder rename";
            this.chooseFolder.UseVisualStyleBackColor = true;
            this.chooseFolder.Click += new System.EventHandler(this.chooseFolder_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.filterBtn);
            this.groupBox1.Controls.Add(this.resetFilterBtn);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.compare3);
            this.groupBox1.Controls.Add(this.compare2);
            this.groupBox1.Controls.Add(this.compare1);
            this.groupBox1.Controls.Add(this.comparison3);
            this.groupBox1.Controls.Add(this.comparison2);
            this.groupBox1.Controls.Add(this.comparison1);
            this.groupBox1.Controls.Add(this.field3);
            this.groupBox1.Controls.Add(this.field2);
            this.groupBox1.Controls.Add(this.field1);
            this.groupBox1.Controls.Add(this.and_orCbx2);
            this.groupBox1.Controls.Add(this.and_orCbx1);
            this.groupBox1.Controls.Add(this.button2);
            this.groupBox1.Location = new System.Drawing.Point(12, 284);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(548, 164);
            this.groupBox1.TabIndex = 5;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Filter";
            // 
            // filterBtn
            // 
            this.filterBtn.Location = new System.Drawing.Point(466, 135);
            this.filterBtn.Name = "filterBtn";
            this.filterBtn.Size = new System.Drawing.Size(76, 23);
            this.filterBtn.TabIndex = 26;
            this.filterBtn.Text = "Ok";
            this.filterBtn.UseVisualStyleBackColor = true;
            this.filterBtn.Click += new System.EventHandler(this.filterBtn_Click);
            // 
            // resetFilterBtn
            // 
            this.resetFilterBtn.Location = new System.Drawing.Point(385, 135);
            this.resetFilterBtn.Name = "resetFilterBtn";
            this.resetFilterBtn.Size = new System.Drawing.Size(75, 23);
            this.resetFilterBtn.TabIndex = 25;
            this.resetFilterBtn.Text = "Clear All";
            this.resetFilterBtn.UseVisualStyleBackColor = true;
            this.resetFilterBtn.Click += new System.EventHandler(this.resetFilterBtn_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(382, 16);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(64, 13);
            this.label3.TabIndex = 24;
            this.label3.Text = "Compare to:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(235, 16);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(65, 13);
            this.label2.TabIndex = 23;
            this.label2.Text = "Comparison:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(75, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(32, 13);
            this.label1.TabIndex = 22;
            this.label1.Text = "Field:";
            // 
            // compare3
            // 
            this.compare3.Location = new System.Drawing.Point(385, 101);
            this.compare3.Name = "compare3";
            this.compare3.Size = new System.Drawing.Size(157, 20);
            this.compare3.TabIndex = 20;
            this.compare3.TextChanged += new System.EventHandler(this.textBox3_TextChanged);
            // 
            // compare2
            // 
            this.compare2.Location = new System.Drawing.Point(385, 75);
            this.compare2.Name = "compare2";
            this.compare2.Size = new System.Drawing.Size(157, 20);
            this.compare2.TabIndex = 19;
            // 
            // compare1
            // 
            this.compare1.Location = new System.Drawing.Point(385, 47);
            this.compare1.Name = "compare1";
            this.compare1.Size = new System.Drawing.Size(157, 20);
            this.compare1.TabIndex = 18;
            // 
            // comparison3
            // 
            this.comparison3.FormattingEnabled = true;
            this.comparison3.Items.AddRange(new object[] {
            "Equal to",
            "Not equal to",
            "Greater than",
            "Less than",
            "Less than or equal",
            "Greater than or equal",
            "Is blank",
            "Is not blank",
            "Contains",
            "Does not contain"});
            this.comparison3.Location = new System.Drawing.Point(235, 101);
            this.comparison3.Name = "comparison3";
            this.comparison3.Size = new System.Drawing.Size(124, 21);
            this.comparison3.TabIndex = 16;
            this.comparison3.SelectedIndexChanged += new System.EventHandler(this.comparison1_SelectedIndexChanged);
            // 
            // comparison2
            // 
            this.comparison2.FormattingEnabled = true;
            this.comparison2.Items.AddRange(new object[] {
            "Equal to",
            "Not equal to",
            "Greater than",
            "Less than",
            "Less than or equal",
            "Greater than or equal",
            "Is blank",
            "Is not blank",
            "Contains",
            "Does not contain"});
            this.comparison2.Location = new System.Drawing.Point(235, 74);
            this.comparison2.Name = "comparison2";
            this.comparison2.Size = new System.Drawing.Size(124, 21);
            this.comparison2.TabIndex = 15;
            this.comparison2.SelectedIndexChanged += new System.EventHandler(this.comparison1_SelectedIndexChanged);
            // 
            // comparison1
            // 
            this.comparison1.FormattingEnabled = true;
            this.comparison1.Items.AddRange(new object[] {
            "Equal to",
            "Not equal to",
            "Greater than",
            "Less than",
            "Less than or equal",
            "Greater than or equal",
            "Is blank",
            "Is not blank",
            "Contains",
            "Does not contain"});
            this.comparison1.Location = new System.Drawing.Point(235, 47);
            this.comparison1.Name = "comparison1";
            this.comparison1.Size = new System.Drawing.Size(124, 21);
            this.comparison1.TabIndex = 14;
            this.comparison1.SelectedIndexChanged += new System.EventHandler(this.comparison1_SelectedIndexChanged);
            // 
            // field3
            // 
            this.field3.FormattingEnabled = true;
            this.field3.Location = new System.Drawing.Point(78, 101);
            this.field3.Name = "field3";
            this.field3.Size = new System.Drawing.Size(133, 21);
            this.field3.TabIndex = 12;
            this.field3.SelectedIndexChanged += new System.EventHandler(this.field3_SelectedIndexChanged);
            // 
            // field2
            // 
            this.field2.FormattingEnabled = true;
            this.field2.Location = new System.Drawing.Point(78, 74);
            this.field2.Name = "field2";
            this.field2.Size = new System.Drawing.Size(133, 21);
            this.field2.TabIndex = 11;
            this.field2.SelectedIndexChanged += new System.EventHandler(this.field2_SelectedIndexChanged);
            // 
            // field1
            // 
            this.field1.FormattingEnabled = true;
            this.field1.Location = new System.Drawing.Point(78, 47);
            this.field1.Name = "field1";
            this.field1.Size = new System.Drawing.Size(133, 21);
            this.field1.TabIndex = 10;
            this.field1.SelectedIndexChanged += new System.EventHandler(this.field1_SelectedIndexChanged);
            // 
            // and_orCbx2
            // 
            this.and_orCbx2.FormattingEnabled = true;
            this.and_orCbx2.Items.AddRange(new object[] {
            "AND",
            "OR"});
            this.and_orCbx2.Location = new System.Drawing.Point(6, 101);
            this.and_orCbx2.Name = "and_orCbx2";
            this.and_orCbx2.Size = new System.Drawing.Size(50, 21);
            this.and_orCbx2.TabIndex = 8;
            this.and_orCbx2.SelectedIndexChanged += new System.EventHandler(this.and_orCbx2_SelectedIndexChanged);
            // 
            // and_orCbx1
            // 
            this.and_orCbx1.FormattingEnabled = true;
            this.and_orCbx1.Items.AddRange(new object[] {
            "AND",
            "OR"});
            this.and_orCbx1.Location = new System.Drawing.Point(6, 74);
            this.and_orCbx1.Name = "and_orCbx1";
            this.and_orCbx1.Size = new System.Drawing.Size(50, 21);
            this.and_orCbx1.TabIndex = 7;
            this.and_orCbx1.SelectedIndexChanged += new System.EventHandler(this.and_orCbx1_SelectedIndexChanged);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(119, 280);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 0;
            this.button2.Text = "Ok";
            this.button2.UseVisualStyleBackColor = true;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(574, 460);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.folderPath);
            this.Controls.Add(this.chooseFolder);
            this.Controls.Add(this.excelPath);
            this.Controls.Add(this.chooseFileBtn);
            this.Controls.Add(this.dataGridView1);
            this.Name = "Form1";
            this.Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.Button chooseFileBtn;
        private System.Windows.Forms.TextBox excelPath;
        private System.Windows.Forms.TextBox folderPath;
        private System.Windows.Forms.Button chooseFolder;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox compare3;
        private System.Windows.Forms.TextBox compare2;
        private System.Windows.Forms.TextBox compare1;
        private System.Windows.Forms.ComboBox comparison3;
        private System.Windows.Forms.ComboBox comparison2;
        private System.Windows.Forms.ComboBox comparison1;
        private System.Windows.Forms.ComboBox field3;
        private System.Windows.Forms.ComboBox field2;
        private System.Windows.Forms.ComboBox field1;
        private System.Windows.Forms.ComboBox and_orCbx2;
        private System.Windows.Forms.ComboBox and_orCbx1;
        private System.Windows.Forms.Button filterBtn;
        private System.Windows.Forms.Button resetFilterBtn;
    }
}

