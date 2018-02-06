using PdfConvertor.Services;
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

namespace PdfConvertor
{
    public partial class FormMain : Form
    {

        private PdfHelper pdfHelper;

        private OcrConvertor ocrConvertor;

        private FormPdfExtractOption childFormPdfExtractOption;

        public PdfHelper PdfHelper {
            get { return pdfHelper; }
        }

        public String FilePath
        {
            get { return txtFilePath.Text; }
        }

        public FormPdfExtractOption FormPdfExtractOption
        {
            set { childFormPdfExtractOption = value; }
            get { return childFormPdfExtractOption; }
        }

        public FormMain()
        {
            InitializeComponent();
            pdfHelper = new PdfHelper();
            ocrConvertor = new OcrConvertor();
        }

        public void ResetChildFormsLocation()
        {
            if (childFormPdfExtractOption != null)
            {
                childFormPdfExtractOption.Left = Right - 15;
                childFormPdfExtractOption.Top = Top;
            }
        }

        private async void btnSelectFile_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();

            openFileDialog.Filter = "PDF files (*.pdf)|*.txt|All files (*.*)|*.*";
            openFileDialog.FilterIndex = 2;
            openFileDialog.RestoreDirectory = true;

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                string filePath = openFileDialog.FileName;
                txtFilePath.Text = filePath;
                lblLoading.Visible = true;
                await Task.Run(() => printResultDisplay(filePath));
            }
        }

        private async void printResultDisplay(string filePath, bool useOsr = true)
        {
            string result = convertPdfToString(filePath);

            BeginInvoke((MethodInvoker)delegate () {
                rtbConvertResult.Text = result;
                lblLoading.Visible = false;
            });
        }

        private string convertPdfToString(string filePath)
        {
            string pdfText = pdfHelper.ConvertPdfToString(filePath);

            if (pdfText != "") return pdfText;

            for (var pageNumber = 1; pageNumber <= pdfHelper.GetPageCount(filePath); pageNumber++)
            {
                //get the text from each page of PDF
                var image = pdfHelper.ConvertSinglePdfPageToJpegImage(filePath, pageNumber);
                string pageText;

                try
                {
                    pageText = ocrConvertor.ConvertImageToText(image.FullName);
                }
                catch (Exception ex)
                {
                    throw new Exception("convertPdfToString(string filepath, bool useOsr = true) " + ex.Message);
                }
                finally
                {
                    pageNumber++;
                    File.Delete(image.FullName);
                }

                pdfText += pageText + "\r\n";
            }

            return pdfText;
        }

        private void Main_Resize(object sender, EventArgs e)
        {
        }

        private void btnSplit_Click(object sender, EventArgs e)
        {
            pdfHelper.ConvertPdfToSinglePages(txtFilePath.Text);
        }

        private void btn_extract_Click(object sender, EventArgs e)
        {
            childFormPdfExtractOption = new FormPdfExtractOption(this);
            childFormPdfExtractOption.Show();
        }

        private void FormMain_Move(object sender, EventArgs e)
        {
            ResetChildFormsLocation();
        }
    }
}
