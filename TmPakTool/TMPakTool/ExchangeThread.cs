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
    class ExchangeThread
    {
        public enum Direction
        {
            Import,
            Export
        }

        private class UpdatePacket
        {
            public enum UpdateType
            {
                FileBegin,
                FileDone,
                FileFailed
            }
            private UpdateType _type;
            private string _path;
            private NadeoPakFile _file;
            private Exception _exception;

            public UpdatePacket(UpdateType type, string path, NadeoPakFile file)
            {
                _type = type;
                _path = path;
                _file = file;
            }

            public UpdatePacket(UpdateType type, string path, NadeoPakFile file, Exception exception)
                : this(type, path, file)
            {
                _exception = exception;
            }

            public UpdateType Type
            {
                get { return _type; }
            }

            public string Path
            {
                get { return _path; }
            }

            public NadeoPakFile File
            {
                get { return _file; }
            }

            public Exception Exception
            {
                get { return _exception; }
            }
        }

        private Direction _dir;

        private IEnumerable<string> _importSourcePaths;
        private NadeoPakFolderBase _importTargetFolder;

        private NadeoPakFolderBase _exportSourceFolder;
        private string _exportTargetFolder;

        private BackgroundWorker _backgroundWorker;
        private bool _started;
        private bool _complete;

        private ExchangeThread()
        {
            _backgroundWorker = new BackgroundWorker();
            _backgroundWorker.WorkerReportsProgress = true;
            _backgroundWorker.WorkerSupportsCancellation = true;
            _backgroundWorker.DoWork += new DoWorkEventHandler(_backgroundWorker_DoWork);
            _backgroundWorker.ProgressChanged += new ProgressChangedEventHandler(_backgroundWorker_ProgressChanged);
            _backgroundWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(_backgroundWorker_RunWorkerCompleted);
        }

        /// <summary>
        /// Import constructor
        /// </summary>
        /// <param name="sourcePaths"></param>
        /// <param name="targetFolder"></param>
        public ExchangeThread(IEnumerable<string> sourcePaths, NadeoPakFolderBase targetFolder)
            : this()
        {
            _dir = Direction.Import;
            _importSourcePaths = sourcePaths;
            _importTargetFolder = targetFolder;
        }

        /// <summary>
        /// Export constructor
        /// </summary>
        /// <param name="sourceFolder"></param>
        /// <param name="targetFolderPath"></param>
        public ExchangeThread(NadeoPakFolderBase sourceFolder, string targetFolderPath)
            : this()
        {
            _dir = Direction.Export;
            _exportSourceFolder = sourceFolder;
            _exportTargetFolder = targetFolderPath;
        }

        public event Action<string, NadeoPakFile> FileExchanging;
        public event Action<string, NadeoPakFile> FileExchanged;
        public event Action<string, NadeoPakFile, Exception> FileFailed;
        public event Action Completed;

        public bool Complete
        {
            get { return _complete; }
        }

        public void Start()
        {
            if (_started)
                return;

            _backgroundWorker.RunWorkerAsync();
            _started = true;
        }

        public void Cancel()
        {
            _backgroundWorker.CancelAsync();
        }

        private void _backgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            if (_dir == Direction.Import)
            {
                foreach (string filePath in _importSourcePaths)
                {
                    if (File.Exists(filePath))
                    {
                        ImportFile(filePath, _importTargetFolder);
                    }
                    else if (Directory.Exists(filePath))
                    {
                        int baseFolderLength = Path.GetDirectoryName(filePath).Length;
                        foreach (FileInfo file in new DirectoryInfo(filePath).GetAllFiles())
                        {
                            NadeoPakFolder importTargetFolder = _importTargetFolder.GetOrCreateFolder(
                                Path.GetDirectoryName(file.FullName.Substring(baseFolderLength)));
                            ImportFile(file.FullName, importTargetFolder);
                        }
                    }
                }
            }
            else
            {
                ExportFolder(_exportSourceFolder, _exportTargetFolder);
            }
        }

        private void _backgroundWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            UpdatePacket packet = ((UpdatePacket)e.UserState);
            switch (packet.Type)
            {
                case UpdatePacket.UpdateType.FileBegin:
                    if (FileExchanging != null)
                        FileExchanging(packet.Path, packet.File);
                    break;

                case UpdatePacket.UpdateType.FileDone:
                    if (FileExchanged != null)
                        FileExchanged(packet.Path, packet.File);
                    break;

                case UpdatePacket.UpdateType.FileFailed:
                    if (FileFailed != null)
                        FileFailed(packet.Path, packet.File, packet.Exception);
                    break;
            }
        }

        private void _backgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            _complete = true;
            if (Completed != null)
                Completed();
        }

        private void ImportFile(string sourceFilePath, NadeoPakFolderBase folder)
        {
            if (_backgroundWorker.CancellationPending)
                return;

            NadeoPakFile file = null;
            try
            {
                string fileName = Path.GetFileName(sourceFilePath);
                file = folder.Files[fileName];
                if (file == null)
                    file = new NadeoPakFile(folder.Pack, folder, fileName, 0, 0, null);

                _backgroundWorker.ReportProgress(0, new UpdatePacket(UpdatePacket.UpdateType.FileBegin,
                    sourceFilePath, file));
                byte[] data = File.ReadAllBytes(sourceFilePath);
                file.Data = data;
                _backgroundWorker.ReportProgress(0, new UpdatePacket(UpdatePacket.UpdateType.FileDone,
                    sourceFilePath, file));
            }
            catch (Exception ex)
            {
                _backgroundWorker.ReportProgress(0, new UpdatePacket(UpdatePacket.UpdateType.FileFailed,
                    sourceFilePath, file, ex));
            }
        }

        private void ExportFolder(NadeoPakFolderBase folder, string targetFolderPath)
        {
            if (_backgroundWorker.CancellationPending)
                return;

            targetFolderPath = Path.Combine(targetFolderPath, folder.Name);
            Directory.CreateDirectory(targetFolderPath);

            foreach (NadeoPakFolder childFolder in folder.Folders)
            {
                ExportFolder(childFolder, targetFolderPath);
            }

            foreach (NadeoPakFile file in folder.Files)
            {
                ExportFile(file, targetFolderPath);
            }
        }

        private void ExportFile(NadeoPakFile file, string targetFolderPath)
        {
            if (_backgroundWorker.CancellationPending)
                return;

            string targetFilePath = Path.Combine(targetFolderPath, file.Name);
            try
            {
                _backgroundWorker.ReportProgress(0, new UpdatePacket(UpdatePacket.UpdateType.FileBegin,
                    targetFilePath, file));
                Directory.CreateDirectory(targetFolderPath);
                File.WriteAllBytes(targetFilePath, file.Data);
                _backgroundWorker.ReportProgress(0, new UpdatePacket(UpdatePacket.UpdateType.FileDone,
                    targetFilePath, file));
            }
            catch (Exception ex)
            {
                _backgroundWorker.ReportProgress(0, new UpdatePacket(UpdatePacket.UpdateType.FileFailed,
                    targetFilePath, file, ex));
            }
        }
    }
}
