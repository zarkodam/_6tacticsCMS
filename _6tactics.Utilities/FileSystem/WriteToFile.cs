using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace _6tactics.Utilities.FileSystem
{
    public class FileWriter
    {
        public string FileName { get; private set; }
        public string FolderPath { get; private set; }
        public string FullFilePath { get; private set; }

        public FileWriter(string fileName, string path)
        {
            FileName = fileName;
            FolderPath = path;
            FullFilePath = Path.Combine(path, fileName);
        }

        public void Write(string content)
        {
            if (File.Exists(FullFilePath)) return;

            try
            {
                using (StreamWriter sw = File.CreateText(FullFilePath))
                {
                    sw.WriteLine(content);
                }
            }
            catch (Exception ex)
            {
                Trace.WriteLine(ex.Message);
            }
            
        }

        public void Append(string content)
        {
            try
            {
                using (StreamWriter sw = File.AppendText(FullFilePath))
                {
                    sw.WriteLine(content);
                }
            }
            catch (Exception ex)
            {
                Trace.WriteLine(ex.Message);
            }


        }

        public IEnumerable<string> Read()
        {
            var content = new List<string>();

            using (StreamReader sr = File.OpenText(FullFilePath))
            {
                var s = "";
                while ((s = sr.ReadLine()) != null)
                    content.Add(s);
            }

            return content;
        }
    }
}