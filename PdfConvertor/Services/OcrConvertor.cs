using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Tesseract;

namespace PdfConvertor.Services
{
    public class OcrConvertor
    {
        private const int NumberOfRetryAttempts = 4;

        /// <summary>
        /// Attempts to convert the image to text
        /// </summary>
        public string ConvertImageToText(string inputFile)
        {
            // IOExceptions are sometimes raised by the Pix.LoadFromFile method call.
            // Swallow the exception and retry converting the image to text
            for (var i = 1; i <= NumberOfRetryAttempts; i++)
            {
                var sb = new StringBuilder();

                var tessdata = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"OcrData");
                using (var engine = new TesseractEngine(tessdata, "eng"))
                {
                    try
                    {
                        using (var img = Pix.LoadFromFile(inputFile))
                        {
                            engine.SetVariable("tessedit_char_whitelist", "0123456789abcdefghijklmnopqrstuvwxyz-_/\\'().,:;#@$&\"ABCDEFGHIJKLMNOPQRSTUVWXYZ");
                            using (var page = engine.Process(img, PageSegMode.AutoOsd))
                            {
                                sb.Append(page.GetText());
                            }
                        }

                        return sb.ToString();
                    }
                    catch (Exception ex)
                    {
                        if (i == NumberOfRetryAttempts)
                            throw new Exception(String.Format("Unable to convert image to text after {0} attempts. {1}", NumberOfRetryAttempts, ex.Message));
                    }
                }

            }

            return null;
        }

        public string ConvertImageToText(byte[] inputBytes)
        {
            var tempFilename = Path.GetTempFileName();
            File.WriteAllBytes(tempFilename, inputBytes);
            var text = ConvertImageToText(tempFilename);
            File.Delete(tempFilename);

            return text;
        }
    }
}
