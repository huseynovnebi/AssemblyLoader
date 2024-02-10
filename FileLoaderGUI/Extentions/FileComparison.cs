using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileLoaderGUI.Extentions
{
    public class FileComparison
    {
        internal static List<string> processedFiles = new List<string>();

        public static List<string> CompareFiles(string[] files)
        {
            List<string> FilesToRead = new List<string>();

            Parallel.ForEach(files, file =>
            {
                if (!processedFiles.Contains(file))
                {
                    FilesToRead.Add(file);
                    processedFiles.Add(file);
                }
            });

            return FilesToRead;


        }
    }
}
