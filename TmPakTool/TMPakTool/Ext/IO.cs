using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;

namespace paktool.Ext
{
    static class IOExt
    {
        public static IEnumerable<DirectoryInfo> GetAllDirectories(this DirectoryInfo baseDir)
        {
            Stack<DirectoryInfo> dfs = new Stack<DirectoryInfo>();
            dfs.Push(baseDir);
            while (dfs.Count > 0)
            {
                DirectoryInfo dir = dfs.Pop();
                foreach (DirectoryInfo subDir in dir.GetDirectories())
                {
                    yield return subDir;
                    dfs.Push(subDir);
                }
            }
        }

        public static IEnumerable<FileInfo> GetAllFiles(this DirectoryInfo baseDir)
        {
            foreach (FileInfo file in baseDir.GetFiles())
                yield return file;

            foreach (DirectoryInfo dir in baseDir.GetAllDirectories())
            {
                foreach (FileInfo file in dir.GetFiles())
                    yield return file;
            }
        }
    }
}
