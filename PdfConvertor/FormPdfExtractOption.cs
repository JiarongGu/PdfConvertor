using PdfConvertor.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PdfConvertor
{
    public partial class FormPdfExtractOption : Form
    {
        public FormMain parentForm;

        public FormPdfExtractOption()
        {
            InitializeComponent();
            this.Disposed += new EventHandler(FormDisposed);
        }

        public FormPdfExtractOption(FormMain parentForm): this()
        {
            this.parentForm = parentForm;
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            int pageCount = (int)numPageCount.Value - (int)numPageStart.Value + 1;
            parentForm.PdfHelper.ExtractPdfPages(parentForm.FilePath, (int)numPageStart.Value, pageCount);
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        public new void Show()
        {
            base.Show();
            parentForm.ResetChildFormsLocation();
        }

        private void FormDisposed(object sender, EventArgs e)
        {
            parentForm.FormPdfExtractOption = null;
        }
    }
}
