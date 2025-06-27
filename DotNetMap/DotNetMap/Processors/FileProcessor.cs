using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotNetMap.Processors
{
    public static class FileProcessor
    {
        public static string AppPath => AppDomain.CurrentDomain.BaseDirectory;

        public static string ReadTextFile(string path)
        {
            if (!File.Exists(path))
            {
                return null;
            }

            return File.ReadAllText(path);
        }
    }
}
