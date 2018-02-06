using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PdfConvertor.Files
{
	public class FileHelper
	{
		/// <summary>
		/// Creates/Gets a file saving Path in the specified folder for a specified file
		/// </summary>
		/// <param name="folderPath">Path of the Main folder where relevant fikles need to be stored</param>
		/// <param name="Id">Id of the file</param>
		/// <returns></returns>
		public string GetPath(string folderPath, int Id, string extension)
		{
			//Get the subfolder from the id
			//Id = 7890 folder = 7000
			//Id = 234576 folder = 234000
			int subFolder = 1;
			if (Id >= 1000)
			{
				subFolder = Id - (Id % 1000);
			}

			string SubfolderFullPath = System.IO.Path.Combine(folderPath, subFolder.ToString());

			//Create the subfolder if it does not exist
			DirectoryInfo directory = new System.IO.DirectoryInfo(SubfolderFullPath);

			if (!directory.Exists)
			{
			   directory.Create();
			}

			//Get all files from the directory
			List<string> existingFileNames = (from x in  directory.GetFiles() select(x.Name)).ToList();				

			string FileId = GetNextFileId(existingFileNames, Id.ToString(),extension, 0);

			//Return the Entire file Path
			return System.IO.Path.Combine(SubfolderFullPath, FileId.ToString());
		}

		public string CombinePath(params string[] path)
		{
			return System.IO.Path.Combine(path);
		}

		public void SaveXmlInFile(string xml, string formPath, string formid)
		{
			FileInfo fi = new FileInfo(formPath);

			string parentFolder = Path.Combine(fi.Directory.FullName,"XML");
			DirectoryInfo di = new DirectoryInfo(parentFolder);
			if (!di.Exists)
			{
				//create parent folder if not present
				Directory.CreateDirectory(di.FullName);
			}

			string xmlfilepath = Path.Combine(di.FullName, formid + ".txt");
            
			//Save xml in file           
			using (FileStream fs = new FileStream(xmlfilepath, FileMode.Create, FileAccess.Write))
			{
				BinaryWriter w = new BinaryWriter(fs);
				w.Write(xml);
				w.Flush();
				fs.Flush();
				w.Close();
			}

		}

		/// <summary>
		/// checks if a file with that name already exists in the folder and gets the new file name
		/// </summary>
		/// <param name="existingFileNames"></param>
		/// <param name="FileId"></param>
		/// <param name="count"></param>
		/// <returns></returns>
		public string GetNextFileId(List<string> existingFileNames, string FileId,string extension, int count)
		{
			string currentFileId = count == 0 ? FileId.ToString()+extension : FileId.ToString()  + "_" + count.ToString() +extension;

			if (existingFileNames.Contains(currentFileId))
			{
				count = count + 1;
				return GetNextFileId(existingFileNames, FileId,extension, count);
			}
			else
			{
				return currentFileId;
			}
		}

		/// <summary>
		/// Deletes file if found in the specified location
		/// </summary>
		/// <param name="filePath"></param>
		public void DeleteIfFound(string filePath)
		{
			if (!string.IsNullOrWhiteSpace(filePath) && File.Exists(filePath))
			{
				File.Delete(filePath);
			}
		}

		/// <summary>
		/// check if the file exists at the given location
		/// </summary>
		/// <param name="filePath"></param>
		/// <returns></returns>
		public bool Exits(string filePath)
		{
			return File.Exists(filePath) ? true : false;
		}


        public string GetDirectoryPathByYearAndMonth(string parentDirectory)
        {
            //get year
            string yearSubFolderPath = Path.Combine(parentDirectory, DateTime.Now.Year.ToString());
            DirectoryInfo yearDirectory = new DirectoryInfo(yearSubFolderPath);

            //if year folder doesnt exist then create it
            if (!yearDirectory.Exists)
                yearDirectory.Create();

            string monthSubFolderPath = Path.Combine(yearSubFolderPath, DateTime.Now.Month.ToString());
            DirectoryInfo monthDirectory = new DirectoryInfo(monthSubFolderPath);
    
            if (!monthDirectory.Exists)
                monthDirectory.Create();

            return monthSubFolderPath;
        }

	    //public string SavePdfFile(int orderId, string directoryPath, byte[] dataToWrite)
	    //{
	    //    var fileName = GetPath(directoryPath, orderId, ".pdf");
	    //    var pdfHelper = CoreServiceLocator.Current.GetInstance<IPdfHelper>();
	    //    pdfHelper.SavePdfData(dataToWrite, fileName);

	    //    return fileName;
	    //}

        /// <summary>
        /// Save bytes as pdf without relying on AbcPDF
        /// </summary>
        /// <param name="orderId"></param>
        /// <param name="directoryPath"></param>
        /// <param name="dataToWrite"></param>
        /// <returns></returns>
	    public string SaveBytesAsPdf(int orderId, string directoryPath, byte[] dataToWrite)
	    {
            var fileName = GetPath(directoryPath, orderId, ".pdf");
	        using (FileStream fs = new FileStream(fileName, FileMode.Create))
	        {
	            fs.Write(dataToWrite, 0, dataToWrite.Length);
	        }

            return fileName;
	    }
        
	}

}
