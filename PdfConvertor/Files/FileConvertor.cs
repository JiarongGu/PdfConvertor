using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.IO;
using System.Text.RegularExpressions;
using PdfConvertor.Services;

namespace PdfConvertor.Files
{
    public class FileConvertor
    {
        private PdfHelper pdfHelper;
        private OcrConvertor ocrConvertor;

        private static List<string> extensions = new List<string>() { ".pdf", "jpg", ".jpeg" };

        public FileConvertor(PdfHelper pdfHelper)
        {
            this.pdfHelper = pdfHelper;
        }

        public FileConvertor(PdfHelper pdfHelper, OcrConvertor ocrConvertor)
        {
            this.pdfHelper = pdfHelper;
            this.ocrConvertor = ocrConvertor;
        }

        public string convertFileToString(string filePath)
        {
            var extension = Path.GetExtension(filePath).ToLower(); //get lower case extension of the file

            if(extension.Contains(extension) == false)
            {
                return "";
            }

            switch (extension)
            {
                case ".pdf": return convertPdfToString(filePath);

                default: //do nothing
                    break;
            }
            return "";
        }

        public Dictionary<string, string> getCapturesFromFile(string regexPattern, string filePath)
        {
            var captures = new Dictionary<string, string>();

            Regex regex = new Regex(regexPattern);

            //get matches of the regex pattern
            Match matchResult = regex.Match(convertFileToString(filePath));

            if (matchResult.Success) //found match
            {
                foreach (string groupName in regex.GetGroupNames())
                {
                    //add match group to dictionary
                    captures.Add(groupName, matchResult.Groups[groupName].Value);
                }
            }

            return captures;
        }

        public Dictionary<string, string> getCapturesFromFolder(string regexPattern, string folderPath)
        {
            var captures = new Dictionary<string, string>();
            var files = Directory.GetFiles(folderPath);

            foreach (var file in files)
            {
                //add fileCaptures to cpatures for result
                foreach (var capture in getCapturesFromFile(regexPattern, file))
                {
                    if (captures.ContainsKey(capture.Key) == false)
                        captures.Add(capture.Key, capture.Value);
                }
            }

            return captures;
        }

        private string convertPdfToString(string filepath)
        {
            //if OCR Service not used convert pdf text
            if (ocrConvertor == null) return pdfHelper.ConvertPdfToString(filepath);

            string pdfText = "";

            for (var pageNumber = 1; pageNumber <= pdfHelper.GetPageCount(filepath); pageNumber++)
            {
                //get the text from each page of PDF
                var image = pdfHelper.ConvertSinglePdfPageToJpegImage(filepath, pageNumber);
                string pageText;

                try
                {
                    pageText = ocrConvertor.ConvertImageToText(image.FullName);
                }
                catch (Exception ex)
                {
                    //TODO ingore exception here
                    continue;
                }
                finally
                {
                    pageNumber++;
                    File.Delete(image.FullName);
                }

                pdfText = pageText + "\r\n";
            }

            return pdfText;
        }
    }
}
