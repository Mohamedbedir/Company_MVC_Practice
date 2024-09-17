using Microsoft.AspNetCore.Http;
using System;
using System.IO;

namespace Company.PL.Helper
{
    public class DocumentSettings
    {
        public static string UploadImage(IFormFile file, string FolderName)
        {
            // 1- get located Folder path
            string FolderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot//files",FolderName);
            //get file name and make it unique
            string fileName = $"{Guid.NewGuid()}{file.FileName}";
            //get file path
            string FilePath=Path.Combine(FolderPath, fileName);

            var filestream = new FileStream(FilePath, FileMode.Create);
            file.CopyTo(filestream);

            return fileName;
        }

        public static void DeleteFile(string fileName, string FolderName)
        {
            if (fileName != null && FolderName != null)
            {
                string FilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\Files", FolderName, fileName);

                if (File.Exists(FilePath))
                {
                    File.Delete(FilePath);
                }
            }
        }



    }
}
