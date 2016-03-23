using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace _6tactics.Utilities.FileSystem
{
    public class FileWriter
    {
        private readonly string _fullFileName;

        public FileWriter(string fileName, string path)
        {
            _fullFileName = Path.Combine(path, fileName);
        }

        public void Write(string content)
        {
            if (File.Exists(_fullFileName)) return;

            try
            {
                using (StreamWriter sw = File.CreateText(_fullFileName))
                {
                    sw.WriteLine(content);
                }
            }
            catch (IOException ex)
            {
                Debug.WriteLine(ex.Message);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }

        }

        public void Append(string content)
        {
            try
            {
                using (StreamWriter sw = File.AppendText(_fullFileName))
                {
                    sw.WriteLine(content);
                }
            }
            catch (IOException ex)
            {
                Debug.WriteLine(ex.Message);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

        public IEnumerable<string> Read()
        {
            var content = new List<string>();

            using (StreamReader sr = File.OpenText(_fullFileName))
            {
                var s = "";
                while ((s = sr.ReadLine()) != null)
                    content.Add(s);
            }

            return content;
        }
    }
}