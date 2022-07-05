using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Social_Network.Core.Application.Helpers
{
    public static class UploadImages
    {
        public static string UploadFile(int id, string folderName, List<IFormFile> files, bool isEditMode = false, string imageUrls = "")
        {
            if (isEditMode && files[0] == null)
            {
                string paths = string.Join(",", imageUrls);

                return paths;
            }
            else if (files[0] == null)
            {
                return null;
            }

            string basePath = $"/images/{folderName}/{id}";
            string path = Path.Combine(Directory.GetCurrentDirectory(), $"wwwroot{basePath}");

            //create folder if it doesn't exists
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            List<string> pathList = new();
            foreach (IFormFile file in files)
            {
                Guid guid = Guid.NewGuid();
                FileInfo fileInfo = new(file.FileName);
                string filename = guid + fileInfo.Extension;

                string fileNameWithPath = Path.Combine(path, filename);

                using (var stream = new FileStream(fileNameWithPath, FileMode.Create))
                {
                    file.CopyTo(stream);
                }

                pathList.Add($"{basePath}/{filename}");
            }

            string pathString = string.Join(",", pathList);

            if (isEditMode && imageUrls != null)
            {
                List<string> oldPath = imageUrls.Split(',').ToList();

                foreach (string item in oldPath)
                {
                    string[] oldImagePart = item.Split("/");
                    string oldImageName = oldImagePart[^1];
                    string completeImageOldPath = Path.Combine(path, oldImageName);

                    if (System.IO.File.Exists(completeImageOldPath))
                    {
                        System.IO.File.Delete(completeImageOldPath);
                    }
                }
            }

            return pathString;
        }
    }
}
