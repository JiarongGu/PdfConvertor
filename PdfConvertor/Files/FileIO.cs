using System;
using System.IO;

namespace PdfConvertor.Files
{
    public class FileIO
    {
        /// <summary>
        /// Compute the name of the folder based on the order unique identifier
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        internal string GetFolderNameFromOrderId(int orderId)
        {
            string folderName = string.Empty;

            if (orderId.ToString().Length > 3)
            {
                string orderIdTemp = orderId.ToString().Substring(0, orderId.ToString().Length - 3);

                folderName = string.Format("{0}000", orderIdTemp);
            }
            else
            {
                folderName = "000";
            }

            return folderName;
        }

        /// <summary>
        /// Compute the file path based on the UNC path of the file server and the computed folder name
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        //internal string GetFullyQualifiedFolderPath(int orderId)
        //{
        //    IApplicationSettings applicationSettings = CoreServiceLocator.Current.GetInstance<IApplicationSettings>();
        //    string folderName = GetFolderNameFromOrderId(orderId);
        //    string fullyQualifiedPath = string.Empty;

        //    fullyQualifiedPath = Path.Combine(applicationSettings.ImagesPath, folderName);

        //    return fullyQualifiedPath;
        //}

        /// <summary>
        /// Compute a filename based on the order unique identifier and and number of ticks of the current time
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        internal string GetFileName(FileInfo source, int orderId)
        {
            string fileName = string.Empty;
            string ticks = DateTime.Now.Ticks.ToString();

            fileName = string.Format("{0}-{1}{2}", orderId, ticks, source.Extension);

            return fileName;
        }

        /// <summary>
        /// Get the File name from the Original Name
        /// </summary>
        /// <param name="FielOriginalName"></param>
        /// <param name="orderId"></param>
        /// <returns></returns>
        internal string GetFileName(string FielOriginalName, int orderId)
        {
            string fileName = string.Empty;
            string ticks = DateTime.Now.Ticks.ToString();

            string Extension = Path.GetExtension(FielOriginalName);
            fileName = string.Format("{0}-{1}{2}", orderId, ticks, Extension);

            return fileName;
        }

        /// <summary>
        /// Handle file to the file Server
        /// </summary>
        /// <param name="source"><see cref="FileInfo"/> representing the source file that we want to save on the server</param>
        /// <param name="orderId"></param>
        //public FileInfo SaveFile(FileInfo file, int orderId)
        //{
        //    //OrderFileModel savedFile = null;
        //    string directory = GetFullyQualifiedFolderPath(orderId);
        //    string fileName = GetFileName(file, orderId);

        //    byte[] fileBytes = GetFileBytes(file);
        //    FileInfo savedFile = WriteFile(directory, fileName, fileBytes);
        //    //savedFile = new OrderFileModel(fileInfo, file.OrderId);

        //    return savedFile;
        //}

        //public FileInfo SaveFile(byte[] fileBytes, string fileNameOriginal, int orderId)
        //{   
        //    //Get File Director and OrderFile Name       
        //    string directory = GetFullyQualifiedFolderPath(orderId);
        //    string fileName = GetFileName(fileNameOriginal, orderId);

        //    //Save the file to the File Folder
        //    FileInfo savedFile = WriteFile(directory, fileName, fileBytes);         

        //    return savedFile;
        //}

        public void CreateDirectory(string directoryPath)
        {
            //If the directory doesn't exist, create it
            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }
        }
          

        /// <summary>
        /// Write File to file server
        /// </summary>
        /// <param name="directory"></param>
        /// <param name="file"></param>
        /// <param name="data"></param>
        /// <exception cref="System.IO.IOException">The directory specified by path is a file.-or-The network name is not known</exception>
        /// <exception cref="System.ObjectDisposedException">The stream is closed </exception>
        /// <exception cref="System.ArgumentNullException">buffer is null or the path is null</exception>
        /// <exception cref="System.ArgumentException">
        /// path is a zero-length string, contains only white space, or contains one or more invalid characters as defined
        /// by System.IO.Path.InvalidPathChars.-or-path is prefixed with, or contains only a colon character (:).
        /// </exception>
        /// <exception cref="System.NotSupportedException">path contains a colon character (:) that is not part of a drive label ("C:\").</exception>
        /// <exception cref="System.IO.DirectoryNotFoundException">The specified path is invalid (for example, it is on an unmapped drive).</exception>
        /// <exception cref="System.IO.PathTooLongException">
        /// The specified path, file name, or both exceed the system-defined maximum 
        /// ength. For example, on Windows-based platforms, paths must be less than
        /// 248 characters and file names must be less than 260 characters.
        /// </exception>
        /// <exception cref="System.UnauthorizedAccessException">The caller does not have the required permission.</exception>
        /// <returns></returns>
        private FileInfo WriteFile(string directory, string file, byte[] data)
        {
            //If the directory doesn't exist, create it
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            string path = Path.Combine(directory, file); //full path of the file;

            //Stream the data into the file
            using (FileStream fs = new FileStream(path, FileMode.Create, FileAccess.Write, FileShare.Read))
            {
                BinaryWriter w = new BinaryWriter(fs);
                w.Write(data);
                w.Flush();
                fs.Flush();
                w.Close();               
            }

            return new FileInfo(path);
        }

        public byte[] GetFileBytes(FileInfo source)
        {
            byte[] fileBytes;

            //Stream the data into the file
            using (FileStream fs = new FileStream(source.FullName, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                fileBytes = new byte[fs.Length];
                fs.Read(fileBytes, 0, fileBytes.Length);
            }

            return fileBytes;
        }

        public static void StreamToFile(MemoryStream input, String outputFileName)
        {
            var dirName = Path.GetDirectoryName(outputFileName);
            var fileName = Path.GetFileName(outputFileName);
            if (String.IsNullOrEmpty(dirName) || String.IsNullOrWhiteSpace(fileName))
            {
                throw new IOException("outputFileName");
            }

            using (FileStream outStream = File.Create(outputFileName))
            {
                input.WriteTo(outStream);
                outStream.Flush();
                outStream.Close();
            }
        }
    }
}
