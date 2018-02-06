using BitMiracle.LibTiff.Classic;
using PdfConvertor.Enums;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebSupergoo.ABCpdf9;
using WebSupergoo.ABCpdf9.Objects;
using WebSupergoo.ABCpdf9.Operations;

namespace PdfConvertor.Services
{
    public class PdfHelper
    {
        /// <summary>
        /// Merges PDF Documents
        /// </summary>
        /// <param name="documentsToMergeList">Paths of Docuemnts to Merge</param>
        /// <param name="outputLocation">Output Location</param>
        /// <returns>Total Numnber of Pages</returns>
        public int MergeDocuments(List<string> documentsToMergeList, string outputLocation)
        {
            try
            {
                using (Doc doc1 = new Doc())
                {
                    foreach (var documentToMerge in documentsToMergeList)
                    {
                        Doc doc2 = new Doc();

                        doc2.Read(documentToMerge);
                        doc1.Append(doc2);
                        doc2.Dispose();
                    }

                    doc1.Form.MakeFieldsUnique(Guid.NewGuid().ToString());
                    doc1.Encryption.CanEdit = false;
                    doc1.Encryption.CanChange = false;
                    doc1.Encryption.CanFillForms = false;
                    doc1.Save(outputLocation);

                    return doc1.PageCount;
                }
            }
            catch (Exception e)
            {
                var documents = string.Join(", ", (from l in documentsToMergeList select l));
                //this.Log().Error(string.Format("Error converting the following documents {0} to ouput location {1}", documents, outputLocation), e);
                //Throw the stack trace with it.
                throw;
            }
        }

        /// <summary>
        /// Merges PDF Documents
        /// </summary>
        /// <param name="documentsToMergeList">Paths of Docuemnts to Merge</param>
        /// <param name="outputLocation">Output Location</param>
        /// <param name="pageSizeEnum"></param>
        /// <returns>Total Numnber of Pages</returns>
        public int MergeDocuments(List<string> documentsToMergeList, string outputLocation, PdfPageSizeEnum pageSizeEnum)
        {
            try
            {
                using (Doc doc1 = new Doc())
                {
                    //set the page size of the entire document
                    doc1.MediaBox.String = pageSizeEnum.ToString();

                    foreach (var documentToMerge in documentsToMergeList)
                    {
                        Doc doc2 = new Doc();

                        doc2.Read(documentToMerge);
                        doc1.Append(doc2);
                        doc2.Dispose();
                    }

                    doc1.Form.MakeFieldsUnique(Guid.NewGuid().ToString());
                    doc1.Encryption.CanEdit = false;
                    doc1.Encryption.CanChange = false;
                    doc1.Encryption.CanFillForms = false;
                    doc1.Save(outputLocation);

                    return doc1.PageCount;
                }
            }
            catch (Exception e)
            {
                var documents = string.Join(", ", (from l in documentsToMergeList select l));
                //this.Log().Error(string.Format("Error converting the following documents {0} to ouput location {1}", documents, outputLocation), e);
                //Throw the stack trace with it.
                throw;
            }
        }

        /// <summary>
        /// Merges PDF Documents and also creates new documents for the string provided and merges them
        /// </summary>
        /// <param name="documentsToMergeList">Existng documents that need to be merged</param>
        /// <param name="documentsToCreateAndMergeList">List of String Data which need to be added to a new document that is merged</param>
        /// <param name="outputLocation">output location where the nerge file gets saved</param>
        /// <returns></returns>
        public int MergeDocuments(List<string> documentsToMergeList, List<string> documentsToCreateAndMergeList, string outputLocation)
        {
            try
            {
                using (Doc doc1 = new Doc())
                {
                    //Merge existing documents
                    foreach (var documentToMerge in documentsToMergeList)
                    {
                        Doc doc2 = new Doc();

                        doc2.Read(documentToMerge);

                        doc1.Append(doc2);

                        doc2.Dispose();
                    }

                    //Create New documents and Merge
                    foreach (var documentToCreateAndMerge in documentsToCreateAndMergeList)
                    {
                        using (Doc doc2 = new Doc())
                        {
                            doc2.FontSize = 40;
                            doc2.AddText(documentToCreateAndMerge);
                            doc1.Append(doc2);
                        }
                    }

                    doc1.Form.MakeFieldsUnique(Guid.NewGuid().ToString());
                    doc1.Encryption.CanEdit = false;
                    doc1.Encryption.CanChange = false;
                    doc1.Encryption.CanFillForms = false;
                    doc1.Save(outputLocation);
                    return doc1.PageCount;
                }
            }
            catch (Exception e)
            {
                var documents = string.Join(", ", (from l in documentsToMergeList select l));
                //this.Log().Error(string.Format("Error converting the following documents {0} to ouput location {1}", documents, outputLocation), e);
                //Throw the stack trace with it.
                throw;
            }
        }

        public int MergeDocuments(List<string> documentsToMergeList, List<string> documentsToCreateAndMergeList, string outputLocation, PdfPageSizeEnum pageSizeEnum)
        {
            try
            {
                using (Doc doc1 = new Doc())
                {
                    //set the page size of the entire document
                    doc1.MediaBox.String = pageSizeEnum.ToString();

                    //Merge existing documents
                    foreach (var documentToMerge in documentsToMergeList)
                    {
                        Doc doc2 = new Doc();

                        doc2.Read(documentToMerge);

                        //add each page of the plan to the doc2. It will add it as A4 size
                        for (int i = 1; i <= doc2.PageCount; i++)
                        {
                            doc1.Page = doc1.AddPage();
                            doc1.AddImageDoc(doc2, i, null);
                            doc1.FrameRect();
                        }

                        doc2.Dispose();
                    }

                    //Create New documents and Merge
                    foreach (var documentToCreateAndMerge in documentsToCreateAndMergeList)
                    {
                        using (Doc doc2 = new Doc())
                        {
                            doc2.FontSize = 40;
                            doc2.AddText(documentToCreateAndMerge);
                            doc1.Append(doc2);
                        }
                    }

                    doc1.Form.MakeFieldsUnique(Guid.NewGuid().ToString());
                    doc1.Encryption.CanEdit = false;
                    doc1.Encryption.CanChange = false;
                    doc1.Encryption.CanFillForms = false;
                    doc1.Save(outputLocation);
                    return doc1.PageCount;
                }
            }
            catch (Exception e)
            {
                var documents = string.Join(", ", (from l in documentsToMergeList select l));
                //this.Log().Error(string.Format("Error converting the following documents {0} to ouput location {1}", documents, outputLocation), e);
                //Throw the stack trace with it.
                throw;
            }
        }

        /// <summary>
        /// Merges the documentsToMergeList into the mergeIntoDocument
        /// </summary>
        /// <param name="documentsToMergeList"></param>
        /// <param name="mergeIntoDocument"></param>
        /// <returns></returns>
        public int MergeDocumentsIntoDocument(List<string> documentsToMergeList, string mergeIntoDocument)
        {
            try
            {
                using (Doc docMerge = new Doc())
                {
                    foreach (var documentToMerge in documentsToMergeList)
                    {
                        Doc doc = new Doc();

                        doc.Read(documentToMerge);
                        docMerge.Append(doc);
                        doc.Dispose();
                    }

                    if (new Files.FileHelper().Exits(mergeIntoDocument))
                    {
                        Doc docMergeInto = new Doc();
                        docMergeInto.Read(mergeIntoDocument);
                    }

                    docMerge.Form.MakeFieldsUnique(Guid.NewGuid().ToString());
                    docMerge.Encryption.CanEdit = false;
                    docMerge.Encryption.CanChange = false;
                    docMerge.Encryption.CanFillForms = false;
                    docMerge.Save(mergeIntoDocument);
                    return docMerge.PageCount;
                }
            }
            catch (Exception e)
            {
                var documents = string.Join(", ", (from l in documentsToMergeList select l));
                //this.Log().Error(string.Format("Error converting the following documents {0} to {1}", documents, mergeIntoDocument), e);
                //Throw the stack trace with it.
                throw;
            }
        }

        /// <summary>
        /// Merges Document1 into Document 2
        /// </summary>
        /// <param name="document1"></param>
        /// <param name="document2"></param>
        /// <returns></returns>
        public int MergeDocuments(string document1, string document2)
        {
            try
            {
                using (Doc docMerge = new Doc())
                {
                    Doc doc1 = new Doc();
                    doc1.Read(document1);

                    Doc doc2 = new Doc();
                    doc2.Read(document2);

                    docMerge.Append(doc1);
                    docMerge.Append(doc2);

                    doc1.Dispose();
                    doc2.Dispose();
                    docMerge.Form.MakeFieldsUnique(Guid.NewGuid().ToString());


                    docMerge.Encryption.CanEdit = false;
                    docMerge.Encryption.CanChange = false;
                    docMerge.Encryption.CanFillForms = false;

                    docMerge.Save(document2);

                    return docMerge.PageCount;
                }
            }
            catch (Exception e)
            {
                //this.Log().Error(string.Format("Error converting a document to PDF: {0} to  {1}", document1, document2), e);
                //Throw the stack trace with it.
                throw;
            }
        }

        /// <summary>
        /// Converts a word file such as Tiff to PDF
        /// </summary>
        /// <param name="originalFileName">the file path and name with its extension</param>
        /// <returns>filepath and filename of the converted image file to a PDF file</returns>
        public string ConvertWordToPdfFile(string originalFileName)
        {
            var targetFileName = originalFileName.Remove(originalFileName.LastIndexOf(".")) + ".pdf";
            try
            {
                OpenOfficeWordToPdfConversion(originalFileName, targetFileName);

            }
            catch (Exception e)
            {
                //this.Log().Error(string.Format("Error converting a OpenOffice document to PDF: {0}", originalFileName), e);
                // Fall back to MS Word
                try
                {
                    MicrosoftOfficeWordToPdfConversion(originalFileName, targetFileName);
                }
                catch (Exception ex)
                {
                    //this.Log().Error(string.Format("Error converting a Microsoft Word document to PDF: {0}", originalFileName), ex);
                    throw ex;
                }
            }

            return targetFileName;
        }

        private void MicrosoftOfficeWordToPdfConversion(string originalFileName, string targetFileName)
        {
            var xr = new XReadOptions { ReadModule = ReadModuleType.MSOffice };
            using (var docPdf = new Doc())
            {
                var doc1 = new Doc();
                doc1.Read(originalFileName, xr);

                docPdf.Append(doc1);
                doc1.Dispose();

                docPdf.Save(targetFileName);
            }
        }

        private void OpenOfficeWordToPdfConversion(string originalFileName, string targetFileName)
        {

            var xr = new XReadOptions { ReadModule = ReadModuleType.OpenOffice };
            using (var docPdf = new Doc())
            {
                docPdf.Read(originalFileName, xr);
                docPdf.Save(targetFileName);
            }
        }


        /// <summary>
        /// Converts a PDF document to an XPS document
        /// </summary>
        /// <param name="originalFileName">The file path and name with its extension</param>
        /// <returns>filepath and filename of the converted XPS file</returns>
        public string ConvertPdfToXps(string pdfFilePath)
        {
            var extension = Path.GetExtension(pdfFilePath);
            var newFilePath = pdfFilePath.Substring(0, pdfFilePath.Length - extension.Length) + ".xps";

            using (var doc = new Doc())
            {
                doc.Read(pdfFilePath);
                doc.Save(newFilePath);
            }

            return newFilePath;
        }

        /// <summary>
        /// Convert word File to PDf and return byte array
        /// </summary>
        /// <param name="originalFileName"></param>
        /// <returns></returns>
        public byte[] ConvertFileToPdfByteArray(string originalFileName)
        {
            try
            {
                using (Doc docPdf = new Doc())
                {
                    docPdf.Read(originalFileName);

                    byte[] docByteArray = docPdf.GetData();

                    return docByteArray;
                }
            }
            catch (Exception e)
            {
                //this.Log().Error(string.Format("Error converting file to PDF by array: {0}", originalFileName), e);
                //Throw the stack trace with it.
                throw;
            }
        }

        /// <summary>
        /// Converts an image file such as Tiff to PDF
        /// </summary>
        /// <param name="originalFileName">the file path and name with its extension</param>
        /// <returns>filepath and filename of the converted image file to a PDF file</returns>
        public string ConvertImageToPdf(string originalFileName)
        {
            try
            {
                if (!System.IO.File.Exists(originalFileName))
                    return string.Empty;

                using (XImage theImg = new XImage())
                {
                    using (Doc theDoc = new Doc())
                    {
                        string filename = originalFileName.Remove(originalFileName.LastIndexOf(".")) + ".pdf";

                        theImg.SetFile(originalFileName);

                        for (int i = 1; i <= theImg.FrameCount; i++)
                        {
                            theImg.Frame = i;
                            theDoc.Page = theDoc.AddPage();
                            theDoc.AddImageObject(theImg, false);
                        }

                        theImg.Clear();
                        theDoc.Save(filename);
                        theDoc.Clear();
                        return filename;
                    }
                }
            }
            catch (Exception e)
            {
                //this.Log().Error(string.Format("Error converting image to Pdf: {0}", originalFileName), e);
                //Throw the stack trace with it.
                throw;
            }
        }

        public string ConvertPdfToString(byte[] originalFileBytes)
        {
            var stringBuilder = new StringBuilder();
            // Use Version 8 PDF Converter as Version 9 removes carriage returns on some PDFs, causing issues with the Regex matching.
            using (var doc = new WebSupergoo.ABCpdf8.Doc())
            {
                doc.Read(originalFileBytes);

                for (var currentPageNumber = 1; currentPageNumber <= doc.PageCount; currentPageNumber++)
                {
                    doc.PageNumber = currentPageNumber;
                    stringBuilder.Append(doc.GetText("Text"));
                }
            }
            return stringBuilder.ToString();
        }

        public string ConvertPdfToString(string filename)
        {
            return ConvertPdfToString(File.ReadAllBytes(filename));
        }

        public void ConvertPdfToSinglePages(string filename)
        {
            var pages = SplitPdfPages(File.ReadAllBytes(filename)).ToList();
            var savelocation = Path.GetDirectoryName(filename);

            for (int i = 1; i <= pages.Count(); i++)
            {
                using (var doc = new WebSupergoo.ABCpdf8.Doc())
                {
                    doc.Read(pages[i - 1]);
                    doc.Save(Path.Combine(savelocation, i + ".pdf"));
                }
            }
        }

        public void ExtractPdfPages(string filename, int pagestart, int pagecount)
        {
            var pages = ExtractPages(File.ReadAllBytes(filename), pagestart, pagecount);
            var savelocation = Path.GetDirectoryName(filename);
            var extractfilename = String.Format("{0}_{1}-{2}.pdf", Path.GetFileName(filename), pagestart, pagestart + pagecount - 1);
             
            using (var doc = new WebSupergoo.ABCpdf8.Doc())
            {
                doc.Read(pages);
                doc.Save(Path.Combine(savelocation, extractfilename));
            }
        }

        public void CreateFormattedEmailPdf(string from, string sent, string to, string subject, string htmlbody, string pdfFileName)
        {
            /*
	                Creates a single paged PDF that is formatted in the MS Outlook email print style:
	             
	                     InfoTrack
	                    --------------------------------------------
	                     From:              << from >>
	                     Sent:              << sent >>
	                     To:                << to >>
	                     Subject:           << subject >>
	              
	                
	                     << body >>
	            */

            // Constant coordinates (in pixels):
            const double yCoordHeaderLine1 = 780 - 10 - 4;
            const double yCoordHeaderLine2 = 780 - 20 - 5;
            const double yCoordHeaderLine3 = 780 - 30 - 6.2;
            const double yCoordHeaderLine4 = 780 - 40 - 7.1;
            const double xCoordLineLeftMargin = 35;
            const double xCoordTextLeftMargin = 37;
            const double xCoordEmailHeaderInfoLeftMargin = 157;
            const double headerLineThickness = 2.8;
            const double yCoordHeaderLine = 780;
            const double ySpacingBetweenHeaderAndBody = 35;
            const double bottomMargin = 50;
            const double yCoordMailboxName = 793;

            // Transform the plain text to HTML
            // var htmlbody = body;

            using (var theDoc = new Doc())
            {
                // Set page size
                theDoc.MediaBox.String = "A4";
                theDoc.Rect.String = theDoc.MediaBox.String;
                theDoc.Page = theDoc.AddPage();

                var pageWidth = theDoc.Rect.Width;
                var pageHeight = theDoc.Rect.Height;

                // Add the mailbox/account name
                var accountNameFont = theDoc.AddFont("Segoe UI-Bold");
                theDoc.FontSize = 12;
                theDoc.Font = accountNameFont;
                theDoc.Pos.X = xCoordTextLeftMargin;
                theDoc.Pos.Y = yCoordMailboxName;
                theDoc.AddText("InfoTrack");

                // Add horizontal header line
                theDoc.Width = headerLineThickness;
                theDoc.AddLine(xCoordLineLeftMargin, yCoordHeaderLine - headerLineThickness / 2,
                    pageWidth - xCoordLineLeftMargin, yCoordHeaderLine - headerLineThickness / 2);

                // Add the email header headings
                theDoc.FontSize = 10;
                theDoc.Pos.X = xCoordTextLeftMargin;
                theDoc.Pos.Y = yCoordHeaderLine1;
                theDoc.AddText("From:");

                theDoc.Pos.X = xCoordTextLeftMargin;
                theDoc.Pos.Y = yCoordHeaderLine2;
                theDoc.AddText("Sent:");

                theDoc.Pos.X = xCoordTextLeftMargin;
                theDoc.Pos.Y = yCoordHeaderLine3;
                theDoc.AddText("To:");

                theDoc.Pos.X = xCoordTextLeftMargin;
                theDoc.Pos.Y = yCoordHeaderLine4;
                theDoc.AddText("Subject:");

                // Add the email header values
                var headerInfoFont = theDoc.AddFont("Segoe UI");
                theDoc.Font = headerInfoFont;
                theDoc.Pos.X = xCoordEmailHeaderInfoLeftMargin;
                theDoc.Pos.Y = yCoordHeaderLine1;
                theDoc.AddText(from);

                theDoc.Pos.X = xCoordEmailHeaderInfoLeftMargin;
                theDoc.Pos.Y = yCoordHeaderLine2;
                theDoc.AddText(sent);

                theDoc.Pos.X = xCoordEmailHeaderInfoLeftMargin;
                theDoc.Pos.Y = yCoordHeaderLine3;
                theDoc.AddText(to);

                theDoc.Rect.Position(xCoordEmailHeaderInfoLeftMargin, yCoordHeaderLine4, XRect.Corner.TopLeft);
                theDoc.Rect.Width = pageWidth - xCoordTextLeftMargin - xCoordEmailHeaderInfoLeftMargin;
                var textObjectId = theDoc.AddText(subject);
                var endY = Double.Parse(theDoc.GetInfo(textObjectId, "EndPos").Split(new[] { ' ' })[1]);
                var subjectTextHeight = yCoordHeaderLine4 - endY;

                // Add the email body
                var bodyFont = theDoc.AddFont("Consolas");
                theDoc.Font = bodyFont;
                theDoc.FontSize = 10;

                theDoc.Rect.Width = pageWidth - (2 * xCoordTextLeftMargin);

                var headerSize = pageHeight - yCoordHeaderLine4 + ySpacingBetweenHeaderAndBody + subjectTextHeight;
                theDoc.Rect.Height = pageHeight - headerSize - bottomMargin;
                theDoc.Rect.Position(xCoordTextLeftMargin, yCoordHeaderLine4 - subjectTextHeight - ySpacingBetweenHeaderAndBody, XRect.Corner.TopLeft);
                theDoc.TextStyle.LineSpacing = 2;

                var chainId = theDoc.AddImageHtml(htmlbody);
                var topMargin = pageHeight - yCoordMailboxName;

                // Add 1st page number
                theDoc.Rect.Height = 40;
                theDoc.Rect.Position(xCoordTextLeftMargin, 40, XRect.Corner.TopLeft);
                theDoc.HPos = 0.5;
                theDoc.AddText(theDoc.PageNumber.ToString());

                // Add more pages
                var currentPage = 2;
                while (theDoc.Chainable(chainId))
                {
                    theDoc.Rect.Height = pageHeight - topMargin - bottomMargin;
                    theDoc.Rect.Position(xCoordTextLeftMargin, yCoordMailboxName, XRect.Corner.TopLeft);
                    theDoc.Page = theDoc.AddPage();
                    chainId = theDoc.AddImageToChain(chainId);

                    // Page number
                    theDoc.PageNumber = currentPage++;
                    theDoc.Rect.Height = 40;
                    theDoc.Rect.Position(xCoordTextLeftMargin, 40, XRect.Corner.TopLeft);
                    theDoc.HPos = 0.5;
                    theDoc.AddText(theDoc.PageNumber.ToString());
                }

                // Save the PDF
                theDoc.Save(pdfFileName);
            }
        }

        public IEnumerable<byte[]> SplitPdfPages(byte[] originalPdfBytes)
        {
            var pdfFiles = new List<byte[]>();

            using (var doc = new Doc())
            {
                doc.Read(originalPdfBytes);

                int srcPagesID = doc.GetInfoInt(doc.Root, "Pages");
                int srcDocRot = doc.GetInfoInt(srcPagesID, "/Rotate");
                
                for (var i = 1; i <= doc.PageCount; i++)
                {
                    var singlePagePdf = new Doc();
                    doc.PageNumber = i;

                    singlePagePdf.Rect.String = singlePagePdf.MediaBox.String = doc.MediaBox.String;
                    singlePagePdf.AddPage();
                    singlePagePdf.AddImageDoc(doc, i, null);
                    singlePagePdf.FrameRect();
                    
                    int srcPageRot = doc.GetInfoInt(doc.Page, "/Rotate");

                    if (srcDocRot != 0)
                        singlePagePdf.SetInfo(singlePagePdf.Page, "/Rotate", srcDocRot);

                    if (srcPageRot != 0)
                        singlePagePdf.SetInfo(singlePagePdf.Page, "/Rotate", srcPageRot);

                    var singlePagePdfBytes = singlePagePdf.GetData();
                    pdfFiles.Add(singlePagePdfBytes);

                    singlePagePdf.Clear();
                }
                doc.Clear();
            }
            
            return pdfFiles;
        }

        public FileInfo ConvertPdfFirstPageToImage(byte[] originalPdfBytes)
        {
            string filename;
            using (var doc = new Doc())
            {
                doc.Read(originalPdfBytes);
                var op = new ImageOperation(doc);
                op.PageContents.AddPages();

                var firstImage = (from parent in op.GetImageProperties()
                                  from child in parent.Renditions
                                  join page in doc.ObjectSoup.Catalog.Pages.GetPageArray() on child.PageID equals page.ID
                                  orderby page.PageNumber
                                  select parent.PixMap).First();

                switch (firstImage.Compression)
                {
                    case CompressionType.Ccitt:
                        filename = Path.GetTempFileName() + firstImage.ID + ".tiff";
                        using (var tiff = Tiff.Open(filename, "w"))
                        {
                            tiff.SetField(TiffTag.IMAGEWIDTH, firstImage.Width.ToString());
                            tiff.SetField(TiffTag.IMAGELENGTH, firstImage.Height.ToString());
                            tiff.SetField(TiffTag.COMPRESSION, XRendering.Compression.G4);
                            tiff.SetField(TiffTag.BITSPERSAMPLE, firstImage.BitsPerComponent.ToString());
                            tiff.SetField(TiffTag.SAMPLESPERPIXEL, 1);
                            tiff.WriteRawStrip(0, firstImage.GetData(), firstImage.Length);
                            tiff.Close();
                        }

                        // Resave the image to change its compression (which is better for the OCR engine)
                        System.Drawing.Image img = System.Drawing.Image.FromFile(filename);
                        var newFilename = filename.Replace(".tiff", "_resaved.tiff");
                        img.Save(newFilename, System.Drawing.Imaging.ImageFormat.Tiff);
                        img.Dispose();

                        // Delete the original
                        File.Delete(filename);
                        filename = newFilename;

                        break;
                    case CompressionType.Jpeg:
                        filename = Path.GetTempFileName() + firstImage.ID + ".jpg";
                        File.WriteAllBytes(filename, firstImage.GetData());
                        break;

                    default:
                        throw new NotSupportedException("PDF Image Extraction: File type not supported.");
                }
                doc.Clear();
            }
            return new FileInfo(filename);
        }

        public FileInfo ConvertSinglePdfPageToJpegImage(byte[] pdfBytes, int pageNumber)
        {
            var filename = Path.GetTempPath() + Guid.NewGuid() + ".jpg";

            using (var doc = new Doc())
            {
                doc.Read(pdfBytes);
                doc.PageNumber = pageNumber;
                doc.Rect.String = doc.CropBox.String;
                doc.Rendering.DotsPerInch = 300;
                doc.Rendering.SaveQuality = 100;
                doc.Rendering.Save(filename);
                doc.Clear();
            }

            return new FileInfo(filename);
        }

        public FileInfo ConvertSinglePdfPageToJpegImage(string filepath, int pageNumber)
        {
            var filename = Path.GetTempPath() + Guid.NewGuid() + ".jpg";

            using (var doc = new Doc())
            {
                doc.Read(filepath);
                doc.PageNumber = pageNumber;
                doc.Rect.String = doc.CropBox.String;
                doc.Rendering.DotsPerInch = 300;
                doc.Rendering.SaveQuality = 100;
                doc.Rendering.Save(filename);
                doc.Clear();
            }

            return new FileInfo(filename);
        }

        public byte[] CombinePages(IEnumerable<byte[]> pages)
        {
            byte[] returnBytes;

            using (var doc = new Doc())
            {
                foreach (var page in pages)
                {
                    using (var tempDoc = new Doc())
                    {
                        tempDoc.Read(page);
                        doc.Append(tempDoc);
                    }
                }

                returnBytes = doc.GetData();
            }

            return returnBytes;
        }

        public byte[] ExtractPages(byte[] documentBytes, int startingPageNumber, int pageCount)
        {
            byte[] extractedDocBytes;
            using (var srcDoc = new Doc())
            {
                srcDoc.Read(documentBytes);
                int srcPagesID = srcDoc.GetInfoInt(srcDoc.Root, "Pages"); //get the root page id
                int srcDocRot = srcDoc.GetInfoInt(srcPagesID, "/Rotate"); //get the rotation of root setting

                using (var extractedPages = new Doc())
                {
                    for (var i = startingPageNumber; i < startingPageNumber + pageCount; i++)
                    {
                        srcDoc.PageNumber = i; //turn original file to current page

                        extractedPages.MediaBox.String = srcDoc.MediaBox.String;

                        extractedPages.AddPage();
                        extractedPages.PageNumber = i - startingPageNumber + 1;
                        extractedPages.Rect.String = extractedPages.MediaBox.String;//doc2.Rect.String;

                        extractedPages.AddImageDoc(srcDoc, i, null);

                        //get the rotation of current page by page Id
                        int srcPageRot = srcDoc.GetInfoInt(srcDoc.Page, "/Rotate");

                        //rotate the page if any rotation exist
                        if (srcDocRot != 0)
                            extractedPages.SetInfo(extractedPages.Page, "/Rotate", srcDocRot);

                        if (srcPageRot != 0)
                            extractedPages.SetInfo(extractedPages.Page, "/Rotate", srcPageRot);
                    }
                    extractedDocBytes = extractedPages.GetData();
                    extractedPages.Clear();
                }

                srcDoc.Clear();
            }

            return extractedDocBytes;
        }

        public List<string> Split(string filepath, int splitPageLength)
        {
            var listOfFiles = new List<string>();

            using (var originalDoc = new Doc())
            {
                originalDoc.Read(filepath);

                // Split into seperate certificates
                for (var i = 1; i <= originalDoc.PageCount; i += splitPageLength)
                {
                    int numberOfPagesToExtract;
                    if (i + splitPageLength - 1 > originalDoc.PageCount)
                        numberOfPagesToExtract = originalDoc.PageCount - i + 1;
                    else
                        numberOfPagesToExtract = splitPageLength;

                    using (var extractedDoc = new Doc())
                    {
                        for (var j = 0; j < numberOfPagesToExtract; j++)
                        {
                            originalDoc.PageNumber = i + j;
                            extractedDoc.MediaBox.String = originalDoc.MediaBox.String;
                            extractedDoc.AddPage();
                            extractedDoc.PageNumber = j + 1;
                            extractedDoc.Rect.String = extractedDoc.MediaBox.String;
                            extractedDoc.AddImageDoc(originalDoc, i + j, null);
                        }

                        var splitFilePath = Path.GetTempPath() + Guid.NewGuid() + ".pdf";
                        extractedDoc.Save(splitFilePath);
                        listOfFiles.Add(splitFilePath);
                    }
                }
            }

            return listOfFiles;
        }

        /// <summary>
        /// Saves an array of bytes as a PDF document
        /// </summary>
        /// <param name="documentBytes">Pdf data</param>
        /// <param name="filePath">Full path with filename and .PDF extension where data will be saved</param>
        public void SavePdfData(byte[] documentBytes, string filePath)
        {
            using (var doc = new Doc())
            {
                doc.Read(documentBytes);
                doc.Save(filePath);
            }
        }


        /// <summary>
        /// Stamps a PDF document with a line of text in a small font size at the lower left hand side of
        /// the first page of the document
        /// </summary>
        /// <param name="documentPath">The full PDF document file path and name</param>
        /// <param name="text">The line of text to stamp onto the document</param>
        public void AddFooterText(string path, string text)
        {
            using (var doc = new Doc())
            {
                doc.Read(path);
                using (var doc2 = new Doc())
                {
                    doc2.Read(doc.GetStream());
                    doc.Clear();
                    doc2.FontSize = 8;
                    var theF1 = doc2.AddFont("Arial");
                    doc2.Font = theF1;
                    doc2.Pos.X = 37;
                    doc2.Pos.Y = 32;
                    doc2.AddText(text);
                    doc2.Save(path);
                }
            }
        }


        /// <summary>
        /// Saves a MemoryStream containing an XPS document to a PDF document
        /// </summary>
        /// <param name="xpsMemoryStream">Stream containing an XPS document</param>
        /// <param name="targetPath">Full target path and file name with .PDF extension</param>
        public void SaveXpsStreamAsPdf(MemoryStream xpsMemoryStream, string targetPath)
        {
            var pdfDoc = new Doc();
            pdfDoc.Read(xpsMemoryStream, new XReadOptions() { ReadModule = ReadModuleType.Xps });
            pdfDoc.Save(targetPath);
            pdfDoc.Clear();
            xpsMemoryStream.Close();
        }

        public int GetPageCount(string filename)
        {
            int pageCount;
            using (var doc = new Doc())
            {
                doc.Read(filename);
                pageCount = doc.PageCount;
            }
            return pageCount;
        }

        public int GetPageCount(byte[] fileData)
        {
            int pageCount;
            using (var doc = new Doc())
            {
                doc.Read(fileData);
                pageCount = doc.PageCount;
            }
            return pageCount;
        }

        /// <summary>
        /// Applies the first page of the PDF file located at waterMarkImagePdfPath as a watermark
        /// to each page of the PDF file located at inputFilePath
        /// </summary>
        /// <param name="inputFilePath">File path of document to be watermarked</param>
        /// <param name="waterMarkImagePdfPath">File path of watermark template</param>
        public void AddWatermark(string inputFilePath, string waterMarkImagePdfPath)
        {
            var temp = inputFilePath + ".watermark";

            var watermarkImage = this.ConvertSinglePdfPageToJpegImage(waterMarkImagePdfPath, 1);
            using (var doc = new Doc())
            {
                doc.Read(inputFilePath);
                int theCount = doc.PageCount;

                int theID = 0;
                for (int i = 1; i <= theCount; i++)
                {
                    doc.PageNumber = i;
                    doc.Layer = doc.LayerCount + 1;
                    if (i == 1)
                    {
                        theID = doc.AddImageFile(watermarkImage.FullName, 1);
                    }
                    else
                        doc.AddImageCopy(theID);
                }
                doc.Save(temp);
                doc.Clear();
                doc.Dispose();
            }

            File.Delete(inputFilePath);
            File.Move(temp, inputFilePath);
            File.Delete(watermarkImage.FullName);
        }

        /// <summary>
        /// Converts Html to pdf
        /// </summary>
        /// <param name="htmlOrUrl">Html markup of html page URL</param>
        /// <param name="isUrl">previous parameter is URL</param>
        /// <param name="highQuality">use high quality converter engine</param>
        /// <param name="indent">indent from all sides of the page</param>
        /// <returns>Memory stream with PDF-file</returns>
        public MemoryStream HtmlToPDF(String htmlOrUrl, Boolean isUrl, Boolean highQuality = false, Int32 indent = 20)
        {
            using (var doc = new Doc())
            {
                doc.Color.String = "0, 0, 0";
                doc.HtmlOptions.UseScript = true;
                doc.HtmlOptions.AddLinks = true;
                if (highQuality)
                {
                    doc.HtmlOptions.Engine = EngineType.Gecko;
                }

                // 1. CONTENT BLOCK
                doc.Rect.Left = 0 + indent;
                doc.Rect.Top = 792 - indent;
                doc.Rect.Right = 612 - indent;
                doc.Rect.Bottom = 0 + indent;

                AppendChainable(doc, htmlOrUrl, isUrl);

                var ms = new MemoryStream();
                doc.Save(ms);
                if (ms.CanSeek)
                {
                    ms.Seek(0, SeekOrigin.Begin);
                }
                return ms;
            }
        }

        public string ConvertHtmlToPdf(string htmlFile)
        {
            var newFileName = string.Format("{0}\\{1}.pdf", Path.GetDirectoryName(htmlFile), Path.GetFileNameWithoutExtension(htmlFile));
            var htmlText = File.ReadAllText(htmlFile);
            var doc = new Doc();
            //doc.Rect.Inset(10,10); //create margin
            doc.HtmlOptions.Engine = EngineType.Gecko;
            int id = doc.AddImageHtml(htmlText);

            while (true)
            {
                //doc.FrameRect(); //draw rectangle
                if (!doc.Chainable(id))
                    break;

                doc.Page = doc.AddPage();
                id = doc.AddImageToChain(id);
            }
            for (int i = 1; i <= doc.PageCount; i++)
            {
                doc.PageNumber = i;
                doc.Flatten();
            }

            doc.Save(newFileName);

            return newFileName;
        }

        /// <summary>
        /// Appends document with multipage content 
        /// </summary>
        private void AppendChainable(Doc doc, String htmlOrUrl, Boolean isUrl = false)
        {
            Int32 blockId = isUrl
                ? doc.AddImageUrl(htmlOrUrl)
                : doc.AddImageHtml(String.Format("<html>{0}</html>", htmlOrUrl));

            while (doc.Chainable(blockId))
            {
                //doc.FrameRect(); // add a black border
                doc.Page = doc.AddPage();
                blockId = doc.AddImageToChain(blockId);
            }
        }
    }
}
