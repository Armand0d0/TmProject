using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using Arc.TrackMania.NadeoPak;
using paktool.Ext;

namespace paktool
{
    class Exchanger
    {
        public static void Import(string sourcePath, NadeoPakFolderBase folder)
        {
            Import(new string[] { sourcePath }, folder);
        }

        public static void Import(IEnumerable<string> sourcePaths, NadeoPakFolderBase folder)
        {
            int fileCount = 0;
            foreach (string path in sourcePaths)
            {
                if (File.Exists(path))
                    fileCount++;
                else if (Directory.Exists(path))
                    fileCount += new DirectoryInfo(path).GetAllFiles().Count();
            }

            FileProcessStatus status = new FileProcessStatus(fileCount) { Text = "Importing..." };
            FailLog failLog = new FailLog();

            ExchangeThread thread = new ExchangeThread(sourcePaths, folder);
            thread.FileExchanging += (path, file) => status.BeginFile(path);
            thread.FileExchanged += (path, file) => status.CompleteFile();
            thread.FileFailed +=
                (path, file, ex) =>
                {
                    failLog.Add(string.Format("Failed to import {0}", path), ex);
                    status.CompleteFile();
                };
            thread.Completed +=
                () =>
                {
                    status.Close();
                    failLog.Display();
                };
            thread.Start();

            status.UserCanceled += () => thread.Cancel();
        }

        public static void ExportFile(NadeoPakFile file, string targetFilePath)
        {
            FailLog failLog = new FailLog();

            try
            {
                File.WriteAllBytes(targetFilePath, file.Data);
            }
            catch (Exception ex)
            {
                failLog.Add(string.Format("Failed to export {0}", file.FullPath), ex);
            }

            failLog.Display();
        }

        public static void ExportFolder(NadeoPakFolderBase folder, string targetFolderPath)
        {
            FileProcessStatus status = new FileProcessStatus(folder.AllFiles.Count()) { Text = "Exporting..." };
            FailLog failLog = new FailLog();

            ExchangeThread thread = new ExchangeThread(folder, targetFolderPath);
            thread.FileExchanging += (path, file) => status.BeginFile(file.FullPath);
            thread.FileExchanged += (path, file) => status.CompleteFile();
            thread.FileFailed +=
                (path, file, ex) =>
                {
                    failLog.Add(string.Format("Failed to export {0}", file.Name), ex);
                    status.CompleteFile();
                };
            thread.Completed +=
                () =>
                {
                    status.Close();
                    failLog.Display();
                };
            thread.Start();

            status.UserCanceled += () => thread.Cancel();
        }
    }
}
