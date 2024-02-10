using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileLoaderGUI.Extentions
{
    public static class FileContent
    {
        public static List<Tuple<string, string>> GetContent(List<string> FilesToRead)
        {
            List<Tuple<string, string>> fileDataList = new List<Tuple<string, string>>();

            Parallel.ForEach(FilesToRead,file =>
            {
                string fileContent =  File.ReadAllText(file);
                string fileExtension = Path.GetExtension(file);

                Tuple<string, string> fileData = new Tuple<string, string>(fileExtension, fileContent);

                lock (fileDataList)
                {
                    fileDataList.Add(fileData);
                }
            });

            return fileDataList;

        }
    }
}
