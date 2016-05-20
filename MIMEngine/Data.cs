using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MIMEngine
{
    public static class Data
    {
        public static string loadFile(string filePath)
        {
            string file = string.Empty;

            using (var streamReader = new StreamReader(AppDomain.CurrentDomain.BaseDirectory + filePath, Encoding.UTF8))
            {
               file = streamReader.ReadToEnd();
            }


            return file;
        }
    }
}
