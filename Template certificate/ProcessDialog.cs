using Connect_To_MySql;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Drive.v3;
using Google.Apis.Services;
using SelectPdf;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Test_upload_file_to_Google_Drive;

namespace Template_certificate
{
    public partial class ProcessDialog : Form
    {
        private readonly string folderStoragePath;
        private readonly DataGridView dataGridView1;
        private readonly Main _owner;
        private readonly string contentHtml = File.ReadAllText("C:\\Generate certificate\\Html source\\certificate template.html");
        private DriveService service;
        private bool _upload = false;
        public bool Upload
        {
            get
            {
                return _upload;
            }
            set
            {
                if (value)
                {
                    Connect connect = new Connect();
                    UserCredential credential = connect.GetAuthenication();

                    // Create Drive API service.
                    service = new DriveService(new BaseClientService.Initializer()
                    {
                        HttpClientInitializer = credential,
                        ApplicationName = ApplicationName,
                    });

                    //set timeout
                    service.HttpClient.Timeout = TimeSpan.FromMinutes(10);
                }
                _upload = value;
            }
        }
        private readonly int total;
        private const string MASTER_FOLDER_NAME = "MindMeister";
        private string ApplicationName = "Funix's Certificate Generation Automatical";

        public ProcessDialog(Main owner)
        {
            InitializeComponent();
            this._owner = owner;

            backgroundWorker1.WorkerSupportsCancellation = true;
            folderStoragePath = _owner.GetFolderPath();
            dataGridView1 = _owner.GetDataGridView();
            total = GetTotalSelectedRow();
            resultLabel.Text = $"Processing: 0/{total}";
        }

        private int GetTotalSelectedRow()
        {
            int count = 0;
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {

                //check if row has checked in check box
                if (Convert.ToBoolean(row.Cells["checkBoxColumn"].Value))
                {
                    count++;
                }
            }
            return count;
        }

        public void StartAsync()
        {
            if (backgroundWorker1.IsBusy != true)
            {
                // Start the asynchronous operation.
                backgroundWorker1.RunWorkerAsync();
            }
        }

        private void cancelAsyncButton_Click(object sender, EventArgs e)
        {
            if (resultLabel.Text.Contains("Error")
                || resultLabel.Text.Contains("Done")
                || resultLabel.Text.Contains("Canceled"))
            {
                this.Dispose();
            }
            else if (backgroundWorker1.WorkerSupportsCancellation)
            {
                // Cancel the asynchronous operation.
                backgroundWorker1.CancelAsync();
                button1.Enabled = false;
            }
        }

        // This event handler is where the time-consuming work is done.
        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker worker = sender as BackgroundWorker;
            int count = 0;
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                //check if row has checked in check box
                if (Convert.ToBoolean(row.Cells["checkBoxColumn"].Value) && backgroundWorker1.WorkerSupportsCancellation)
                {
                    //render this row to pdf
                    string studentName = row.Cells["Họ và tên"].Value.ToString();
                    string studentId = row.Cells["Mã sinh viên"].Value.ToString();

                    DateTime date = Convert.ToDateTime(row.Cells["Ngày hoàn thành "].Value);

                    string ccVnName = row.Cells["Tên chứng chỉ"].Value.ToString().Trim().Replace("Chứng chỉ ", "").Trim();
                    ccVnName = char.ToUpper(ccVnName.First()) + ccVnName.Substring(1);

                    string ccEnName = row.Cells["Tên chứng chỉ (tiếng anh)"].Value.ToString();
                    string ccNumber = row.Cells["Số CC"].Value.ToString();

                    GeneratePdf(studentName, studentId, date, ccVnName, ccEnName, ccNumber, folderStoragePath, e);
                    count++;

                    worker.ReportProgress(count);
                }
            }
            MessageBox.Show("Generate successfull", "Successfull", MessageBoxButtons.OK, MessageBoxIcon.Information);
            Process.Start(folderStoragePath);
            //e.Result = _owner.renderPdfBtn_Click((int)e.Argument, worker, e);
        }

        // This event handler updates the progress.
        private void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            resultLabel.Text = ("Processing: " + e.ProgressPercentage.ToString() + "/" + total);
        }

        // This event handler deals with the results of the background operation.
        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Cancelled == true)
            {
                resultLabel.Text = "Canceled!";
                this.Dispose();
            }
            else if (e.Error != null)
            {
                resultLabel.Text = "Error: " + e.Error.Message;
            }
            else
            {
                resultLabel.Text = "Done!";
                this.Dispose();
            }
        }

        private void GeneratePdf(string studentName, string studentId, DateTime date, string ccVnName, string ccEnName, string ccNumber, string folderStoragePath, DoWorkEventArgs ev)
        {
            CertificateModel certificateModel = new CertificateModel();

            if (backgroundWorker1.CancellationPending)
            {
                ev.Cancel = true;
            }
            else
                try
                {
                    string finishedDate = date.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
                    string[] parameters = { studentName, ccVnName, ccEnName, ccNumber, finishedDate };
                    //string filename = "E:/Funix/Template certificate/certificate template 1.pdf";
                    string filePath = $"{folderStoragePath.Replace("\\", "/")}/{studentId}-{ccNumber}-{studentName}.pdf";

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

                    doc.Save(filePath);

                    if (Upload)
                    {
                        string fileId = UploadFileToGoogleDrive(studentName, studentId, ccNumber, filePath, service);

                        string certiLink = $"https://drive.google.com/file/d/{fileId}/view?usp=sharing";
                        certificateModel.AddNewUserCertificate(certiLink, ccNumber, date, studentId, ccEnName);
                    }
                    doc.Close();
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.Message, "Some things went wrong", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
        }

        private string UploadFileToGoogleDrive(string studentName, string studentId, string ccNumber, string filePath, DriveService service)
        {
            Connect connect = new Connect();

            //get list of all folders
            var files = connect.retrieveAllFolders(service);

            //The pattern of folder name. It is fixed
            string folderName = $"{studentId}_{studentName}";

            string folderParentId = files.SingleOrDefault(f => f.Name.Contains(MASTER_FOLDER_NAME)).Id;
            //finding folder in list of all folder
            var folder = files.SingleOrDefault(f => f.Name.Equals(folderName));

            //if folder not created yet. Create new folder
            string folderId;
            if (folder == null)
            {
                folderId = connect.CreateNewFolder(folderParentId, service, folderName);
            }
            else
            {
                folderId = folder.Id;
            }

            connect.CreateNewFile(folderId, service, filePath, $"{studentId}-{ccNumber}-{studentName}.pdf");

            return connect.FileId;
        }
    }
}

