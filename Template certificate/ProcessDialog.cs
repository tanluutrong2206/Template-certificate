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

namespace Template_certificate
{
    public partial class ProcessDialog : Form
    {
        private string folderStoragePath;
        private readonly DataGridView dataGridView1;
        private readonly Main _owner;
        private readonly string contentHtml = File.ReadAllText(Path.Combine(Application.ExecutablePath.Remove(Application.ExecutablePath.LastIndexOf("\\")), "Html source\\certificate template.html"));

        public string masterFolderId { get; set; }
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

                    masterFolderId = GetMasterFolderId(service);
                }
                _upload = value;
            }
        }
        private readonly int total;
        private const string MASTER_FOLDER_NAME = "Chứng chỉ Funix";
        private string ApplicationName = "Funix's Certificate Generation Automatical";

        public ProcessDialog(Main owner, bool isUpload)
        {
            InitializeComponent();
            this._owner = owner;
            Upload = isUpload;

            backgroundWorker1.WorkerSupportsCancellation = true;
            folderStoragePath = _owner.GetFolderPath();
            dataGridView1 = _owner.GetDataGridView();
            total = GetTotalSelectedRow();
            resultLabel.Text = $"Processing: 0/{total}";

            StartAsync();
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
            SelectPdf.HtmlToPdf converter = InitConverter();

            int count = 0;
            if (string.IsNullOrEmpty(folderStoragePath))
            {
                string exePath = Application.ExecutablePath;
                folderStoragePath = Path.Combine(exePath.Remove(exePath.LastIndexOf('\\')), "funix-certificate");
            }
            try
            {
                foreach (DataGridViewRow row in dataGridView1.Rows)
                {
                    //TODO: (Ms.Hồng Anh required) File excel change the column Tên chứng chỉ (tiếng anh) and Chứng chỉ to single column Mã môn.
                    //check if row has checked in check box
                    if (!backgroundWorker1.CancellationPending && Convert.ToBoolean(row.Cells["checkBoxColumn"].Value))
                    {
                        //render this row to pdf
                        string studentName = row.Cells["Họ và tên"].Value.ToString();
                        string studentId = row.Cells["Mã sinh viên"].Value.ToString();
                        string email = row.Cells["Email"].Value.ToString();

                        DateTime date = Convert.ToDateTime(row.Cells["Ngày hoàn thành "].Value);

                        string ccVnName = row.Cells["Tên chứng chỉ"].Value.ToString().Trim();
                        if (ccVnName.StartsWith("Chứng chỉ "))
                        {
                            ccVnName = ccVnName.Replace("Chứng chỉ ", "").Trim();
                            ccVnName = char.ToUpper(ccVnName.First()) + ccVnName.Substring(1);
                        }

                        string ccEnName = row.Cells["Tên chứng chỉ (tiếng anh)"].Value.ToString();
                        string ccNumber = row.Cells["Số CC"].Value.ToString();
                        string ccCode = new CertificateModel().GetCcCode(ccEnName);

                        GeneratePdf(studentName, studentId, date, ccVnName, ccEnName, ccNumber, folderStoragePath, ccCode, email, e, converter);
                        count++;

                        worker.ReportProgress(count);
                    }

                }
                MessageBox.Show("Generate successfull", "Successfull", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                backgroundWorker1.CancelAsync();
            }
            finally
            {
                Process.Start(folderStoragePath);
            }
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

        private void GeneratePdf(string studentName, string studentId, DateTime date, string ccVnName, string ccEnName, string ccNumber, string folderStoragePath, string ccCode, string email, DoWorkEventArgs ev, SelectPdf.HtmlToPdf converter)
        {
            CertificateModel certificateModel = new CertificateModel();
            string filePath = null;
            if (backgroundWorker1.CancellationPending)
            {
                ev.Cancel = true;
            }
            else
                try
                {
                    string finishedDate = date.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
                    string[] parameters = { studentName, ccVnName, ccEnName, ccNumber, finishedDate, Application.ExecutablePath.Remove(Application.ExecutablePath.LastIndexOf("\\")).Replace("\\", "/") };
                    //string filename = "E:/Funix/Template certificate/certificate template 1.pdf";

                    filePath = $"{folderStoragePath.Replace("\\", "/")}/{ccNumber}-{ccCode}-{studentName}.pdf";

                    string html = string.Format(contentHtml, parameters);
                    // define a rendering result object

                    PdfDocument doc = converter.ConvertHtmlString(html);

                    doc.Save(filePath);
                    doc.Close();

                    UploadFileToGoogleDrive(studentName, studentId, date, ccEnName, ccNumber, ccCode, email, filePath, ev);
                }
                catch (Exception e)
                {
                    throw e;
                }
        }

        private HtmlToPdf InitConverter()
        {
            // define a rendering result object
            SelectPdf.HtmlToPdf converter = new SelectPdf.HtmlToPdf();

            //setting up rendering
            converter.Options.PdfPageOrientation = PdfPageOrientation.Landscape;
            converter.Options.PdfPageSize = PdfPageSize.A4;
            converter.Options.DrawBackground = true;
            converter.Options.EmbedFonts = true;

            //white outline half of cm
            converter.Options.MarginLeft = -7;
            converter.Options.MarginRight = -10;

            converter.Options.AutoFitHeight = HtmlToPdfPageFitMode.AutoFit;
            converter.Options.AutoFitWidth = HtmlToPdfPageFitMode.AutoFit;

            return converter;
        }

        private void UploadFileToGoogleDrive(string studentName, string studentId, DateTime date, string ccEnName, string ccNumber, string ccCode, string email, string filePath, DoWorkEventArgs ev)
        {
            CertificateModel certificateModel = new CertificateModel();

            if (Upload)
            {
                if (backgroundWorker1.CancellationPending)
                {
                    ev.Cancel = true;
                }
                else
                {
                    string webViewLink = UploadFileToGoogleDrive(studentName, ccNumber, ccCode, filePath, service, studentId);

                    //string certiLink = $"https://drive.google.com/file/d/{webViewLink}/view?usp=sharing";

                    if (!certificateModel.CheckCertificateInDatabase(ccNumber))
                    {
                        certificateModel.AddNewUserCertificate(webViewLink, ccNumber, date, studentId, ccEnName, email, studentName);
                    }
                    else
                    {
                        certificateModel.UpdateUserCertificate(webViewLink, ccNumber, date, studentId, ccEnName, email, studentName);
                    }

                }
            }
        }

        private string UploadFileToGoogleDrive(string studentName, string ccNumber, string ccCode, string filePath, DriveService service, string studentId)
        {
            Connect connect = new Connect();

            //The pattern of folder name. It is fixed
            string folderName = $"{studentId}_{studentName}";
            string folderStudentId = GetStudentFolderId(masterFolderId, folderName, service);

            //get all pdfs file inside folder
            var files = connect.RetrieveAllPdfFileDirectoryFolders(service, folderStudentId);
            var fileName = $"{ccNumber} - {ccCode} - {studentName}.pdf";
            var filesExist = files.FindAll(f => f.Name.Equals(fileName));
            foreach (var file in filesExist)
            {
                //TODO: if had certificate file in google drive, override it
                //Move file to trash
                connect.DeleteFile(service, file.Id);
            }

            connect.CreateNewFile(folderStudentId, service, filePath, fileName);
            return connect.WebViewLink;
        }

        private string GetStudentFolderId(string masterFolderId, string folderName, DriveService service)
        {
            Connect connect = new Connect();

            var files = connect.RetrieveAllFolders(service, masterFolderId);
            //finding folder in list of all folder
            var folder = files.SingleOrDefault(f => f.Name.Equals(folderName));

            //if folder not created yet. Create new folder
            string folderId;
            if (folder == null)
            {
                folderId = connect.CreateNewFolder(masterFolderId, service, folderName);
            }
            else
            {
                folderId = folder.Id;
            }

            return folderId;
        }

        private string GetMasterFolderId(DriveService service)
        {
            Connect connect = new Connect();

            //get list of all folders
            string masterFolderId = null;
            var allFolders = connect.RetrieveAllFolders(service, masterFolderId);

            var folderParent = allFolders.SingleOrDefault(f => f.Name.Equals(MASTER_FOLDER_NAME));

            if (folderParent == null)
            {
                masterFolderId = connect.CreateNewFolder(masterFolderId, service, MASTER_FOLDER_NAME);
            }
            else
            {
                masterFolderId = folderParent.Id;
            }

            return masterFolderId;
        }
    }
}

