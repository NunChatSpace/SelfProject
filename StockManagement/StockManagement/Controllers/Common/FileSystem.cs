using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;

namespace StockManagement.Controllers.Common
{
    public static class FileSystem
    {
        public static string WRITE_IMAGE(string name, string[] images) {
            string serverPath = HttpContext.Current.Server.MapPath("~");
            string path = $"{serverPath}UploadedImages\\{name}\\";

            bool exists = System.IO.Directory.Exists(path);
            if (!exists) {
                System.IO.Directory.CreateDirectory(path);
            }

            string[] files = Directory.GetFiles(path);
            int startIndex = files.Length;
            int limitIndex = startIndex + images.Length;
            int imageIndex = 0;

            for (int i = startIndex; i < limitIndex; i++) {
                string fileNameWitPath = $"{path}\\{name}_{i}.jpg";
                using (FileStream fs = new FileStream(fileNameWitPath, FileMode.Create))
                {
                    using (BinaryWriter bw = new BinaryWriter(fs))
                    {
                        try
                        {
                            byte[] data = Convert.FromBase64String(images[imageIndex]);
                            bw.Write(data);
                            bw.Close();
                        }
                        catch (Exception e) {
                            Debug.WriteLine(e.Message);
                        }
                    }
                }
                imageIndex++;
            }

            return path;
        }

        public static List<string> READ_IMAGE(string itemImage)
        {
            string serverPath = HttpContext.Current.Server.MapPath("~");
            string path = $"{serverPath}UploadedImages\\{itemImage}\\";
            string[] files = Directory.GetFiles(path);
            List<string> images = new List<string>();

            foreach (string file in files)
            {
                try
                {
                    byte[] b = File.ReadAllBytes(file);
                    string base64String = Convert.ToBase64String(b, 0, b.Length);

                    images.Add(base64String);
                }
                catch (Exception e)
                {
                    Debug.WriteLine(e.Message);
                }
            }
            return images;
        }
    }
}