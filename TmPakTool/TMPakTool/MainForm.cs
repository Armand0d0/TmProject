using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Forms;
using Arc.TrackMania.NadeoPak;
using Arc.TrackMania.Classes;

namespace paktool
{
    public partial class MainForm : Form
    {
        public const string APPNAME = "TMPakTool";
        private static string _title;

        private string _filePath;
        private NadeoPak _pak;
        private Dictionary<NadeoPakFolderBase, TreeNode> _folderNodes = new Dictionary<NadeoPakFolderBase, TreeNode>();
        private Dictionary<NadeoPakFile, TreeNode> _fileNodes = new Dictionary<NadeoPakFile, TreeNode>();
        private EditorBase _currentEditor;

        public MainForm()
        {
            InitializeComponent();

            Version version = Assembly.GetExecutingAssembly().GetName().Version;
            _title = string.Format("{0} v{1}.{2}", APPNAME, version.Major, version.Minor);
            Text = _title;
        }

        public static string Title
        {
            get { return _title; }
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            switch (keyData)
            {
                case Keys.Control | Keys.O:
                    _btnOpen_Click(_btnOpen, new EventArgs());
                    return true;

                case Keys.Control | Keys.S:
                    _btnSave_Click(_btnSave, new EventArgs());
                    return true;

                case Keys.Control | Keys.P:
                    _btnProperties_Click(_btnProperties, new EventArgs());
                    return true;
            }

            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void _btnOpen_Click(object sender, EventArgs e)
        {
            if (_dlgOpenPak.ShowDialog() != DialogResult.OK)
                return;

            LoadFile(_dlgOpenPak.FileName);
        }

        private void _btnSave_Click(object sender, EventArgs e)
        {
            SaveFile();
        }

        private void _btnExtract_Click(object sender, EventArgs e)
        {
            if (_tvMain.SelectedNode == null)
                return;

            if (_tvMain.SelectedNode.Tag is NadeoPakFolderBase)
            {
                NadeoPakFolderBase folder = (NadeoPakFolderBase)_tvMain.SelectedNode.Tag;
                _dlgChooseFolder.Description = string.Format("Extract {0} to:", folder.Name);
                if (_dlgChooseFolder.ShowDialog() != DialogResult.OK)
                    return;

                Exchanger.ExportFolder(folder, _dlgChooseFolder.SelectedPath);
            }
            else if (_tvMain.SelectedNode.Tag is NadeoPakFile)
            {
                NadeoPakFile file = (NadeoPakFile)_tvMain.SelectedNode.Tag;
                _dlgExtractFile.FileName = file.Name;
                if (_dlgExtractFile.ShowDialog() != DialogResult.OK)
                    return;

                Exchanger.ExportFile(file, _dlgExtractFile.FileName);
            }
        }

        private void _btnRemove_Click(object sender, EventArgs e)
        {
            if (_tvMain.SelectedNode == null)
                return;

            if (_tvMain.SelectedNode.Tag is NadeoPakFolder)
            {
                NadeoPakFolder folder = (NadeoPakFolder)_tvMain.SelectedNode.Tag;
                folder.ParentFolder = null;
            }
            else if (_tvMain.SelectedNode.Tag is NadeoPakFile)
            {
                NadeoPakFile file = (NadeoPakFile)_tvMain.SelectedNode.Tag;
                file.Folder = null;
            }
        }

        private void _btnAddFolder_Click(object sender, EventArgs e)
        {
            if (_tvMain.SelectedNode == null)
                return;

            NadeoPakFolderBase parentFolder = null;
            if (_tvMain.SelectedNode.Tag is NadeoPakFolderBase)
            {
                parentFolder = (NadeoPakFolderBase)_tvMain.SelectedNode.Tag;
            }
            else if (_tvMain.SelectedNode.Tag is NadeoPakFile)
            {
                parentFolder = ((NadeoPakFile)_tvMain.SelectedNode.Tag).Folder;
            }

            _dlgChooseFolder.Description = string.Format("Import folder into {0}", parentFolder.Name);
            if (_dlgChooseFolder.ShowDialog() != DialogResult.OK)
                return;

            Exchanger.Import(_dlgChooseFolder.SelectedPath, parentFolder);
        }

        private void _btnAddFile_Click(object sender, EventArgs e)
        {
            if (_tvMain.SelectedNode == null)
                return;

            NadeoPakFolderBase folder = null;
            if (_tvMain.SelectedNode.Tag is NadeoPakFolderBase)
            {
                folder = (NadeoPakFolderBase)_tvMain.SelectedNode.Tag;
            }
            else if (_tvMain.SelectedNode.Tag is NadeoPakFile)
            {
                folder = ((NadeoPakFile)_tvMain.SelectedNode.Tag).Folder;
            }

            if (_dlgAddFile.ShowDialog() != DialogResult.OK)
                return;

            Exchanger.Import(_dlgAddFile.FileNames, folder);
            if (_tvMain.SelectedNode.Tag is NadeoPakFile)
                ShowPAKFileDetails((NadeoPakFile)_tvMain.SelectedNode.Tag, false);
        }

        private void _btnProperties_Click(object sender, EventArgs e)
        {
            if (_tvMain.SelectedNode == null)
                return;

            if (_tvMain.SelectedNode.Tag is NadeoPakFolder)
            {
                NadeoPakFolder folder = (NadeoPakFolder)_tvMain.SelectedNode.Tag;
                new FolderProperties(folder).ShowDialog();
                _tvMain.SelectedNode.Text = folder.Name;
            }
            else if (_tvMain.SelectedNode.Tag is NadeoPakFile)
            {
                NadeoPakFile file = (NadeoPakFile)_tvMain.SelectedNode.Tag;
                new FileProperties(file).ShowDialog();
                _tvMain.SelectedNode.Text = string.Format("{0} ({1})", file.Name, file.ClassName);
            }
        }

        private void _btnPackList_Click(object sender, EventArgs e)
        {
            if (_dlgOpenPackList.ShowDialog() != DialogResult.OK)
                return;

            FailLog failLog = new FailLog();
            try
            {
                PackList packList = new PackList(_dlgOpenPackList.FileName);
                new PackListViewer(packList).ShowDialog();
            }
            catch (Exception ex)
            {
                failLog.Add(string.Format("Failed to open {0}", _dlgOpenPackList.FileName), ex);
            }
            failLog.Display();
        }

        private void _btnAbout_Click(object sender, EventArgs e)
        {
            new About().ShowDialog();
        }

        private void _mnuContext_Opening(object sender, CancelEventArgs e)
        {
            _mnuAddFile.Enabled = _btnAddFile.Enabled;
            _mnuAddFolder.Enabled = _btnAddFolder.Enabled;
            _mnuRemove.Enabled = _btnRemove.Enabled;
            _mnuExtract.Enabled = _btnExtract.Enabled;
            _mnuProperties.Enabled = _btnProperties.Enabled;
        }

        private void _tvMain_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (_tvMain.SelectedNode == null)
            {
                _btnAddFolder.Enabled = false;
                _btnAddFile.Enabled = false;
                return;
            }

            _btnAddFolder.Enabled = true;
            _btnAddFile.Enabled = true;

            if (e.Node.Tag is NadeoPak)
            {
                _btnExtract.Image = Properties.Resources.ExtractArchive;
                _btnRemove.Image = Properties.Resources.RemoveFolder;
                _btnRemove.Enabled = false;
                _btnProperties.Enabled = false;
            }
            else if (e.Node.Tag is NadeoPakFolder)
            {
                _btnExtract.Image = Properties.Resources.ExtractFolder;
                _btnRemove.Image = Properties.Resources.RemoveFolder;
                _btnRemove.Enabled = true;
                _btnProperties.Enabled = true;
            }
            else if (e.Node.Tag is NadeoPakFile)
            {
                _btnExtract.Image = Properties.Resources.ExtractFile;
                _btnRemove.Image = Properties.Resources.RemoveFile;
                _btnRemove.Enabled = true;
                _btnProperties.Enabled = true;
            }

            ShowPAKFileDetails(e.Node.Tag as NadeoPakFile, true);
        }

        private void _tvMain_DragEnter(object sender, DragEventArgs e)
        {
            if (_pak == null || !e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                e.Effect = DragDropEffects.None;
                return;
            }

            e.Effect = DragDropEffects.Copy;
        }

        private void _tvMain_DragDrop(object sender, DragEventArgs e)
        {
            if (_pak == null || !e.Data.GetDataPresent(DataFormats.FileDrop))
                return;

            TreeNode targetNode = _tvMain.GetNodeAt(_tvMain.PointToClient(new Point(e.X, e.Y)));
            if (targetNode == null)
                return;

            NadeoPakFolderBase folder = null;
            if (targetNode.Tag is NadeoPakFolderBase)
                folder = (NadeoPakFolderBase)targetNode.Tag;
            else if (targetNode.Tag is NadeoPakFile)
                folder = ((NadeoPakFile)targetNode.Tag).Folder;

            string[] sourcePaths = (string[])e.Data.GetData(DataFormats.FileDrop);
            Exchanger.Import(sourcePaths, folder);
        }

        private void _tvMain_ItemDrag(object sender, ItemDragEventArgs e)
        {
            TreeNode node = (TreeNode)e.Item;
            
            IEnumerable<NadeoPakFile> files = null;
            int baseFolderLength = 0;
            if (node.Tag is NadeoPakFile)
            {
                NadeoPakFile file = (NadeoPakFile)node.Tag;
                files = new NadeoPakFile[] { file };
                if (file.Folder is NadeoPakFolder)
                    baseFolderLength = ((NadeoPakFolder)file.Folder).FullPath.Length;
            }
            else if (node.Tag is NadeoPakFolderBase)
            {
                files = ((NadeoPakFolderBase)node.Tag).AllFiles;
                if (node.Tag is NadeoPakFolder && ((NadeoPakFolder)node.Tag).ParentFolder is NadeoPakFolder)
                    baseFolderLength = ((NadeoPakFolder)((NadeoPakFolder)node.Tag).ParentFolder).FullPath.Length;
            }

            FileProcessStatus status = new FileProcessStatus(files.Count(), false) { Text = "Exporting..." };
            FailLog failLog = new FailLog();
            Delay.VirtualFileDataObject dataObj = new Delay.VirtualFileDataObject(
                null,
                (obj) =>
                {
                    status.Close();
                    failLog.Display();
                }
            );
            dataObj.SetData(
                from file in files
                select new Delay.VirtualFileDataObject.FileDescriptor()
                {
                    Name = file.FullPath.Substring(baseFolderLength),
                    StreamContents =
                        (stream) =>
                        {
                            status.BeginFile(file.FullPath);
                            try
                            {
                                stream.Write(file.Data, 0, file.Data.Length);
                            }
                            catch (Exception ex)
                            {
                                failLog.Add(string.Format("Failed to export {0}", file.FullPath), ex);
                            }
                            status.CompleteFile();
                        }
                }
            );
            
            Delay.VirtualFileDataObject.DoDragDrop(dataObj, System.Windows.DragDropEffects.Copy);
        }

        private void _tvMain_DoubleClick(object sender, EventArgs e)
        {
            if (_tvMain.SelectedNode != null && _tvMain.SelectedNode.Tag is NadeoPakFile)
                new FileProperties((NadeoPakFile)_tvMain.SelectedNode.Tag).ShowDialog();
        }

        private void _tvMain_KeyUp(object sender, KeyEventArgs e)
        {
            switch (e.KeyData)
            {
                case Keys.Delete:
                    _btnRemove_Click(_btnRemove, new EventArgs());
                    break;
            }
        }

        private void LoadFile(string filePath)
        {
            CloseFile();

            try
            {
                _pak = new NadeoPak(filePath);

                PresentPAK(_pak);

                _btnSave.Enabled = true;
                _btnExtract.Enabled = true;

                _filePath = filePath;
                Text = string.Format("{0} - {1}", _filePath, _title);
            }
            catch (Exception ex)
            {
                CloseFile();
                MessageBox.Show(string.Format("Failed to load {0}:\n{1}", filePath, ex.Message),
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void SaveFile()
        {
            if (_pak == null || _filePath == null)
                return;

            try
            {
                if (_currentEditor != null)
                    _currentEditor.Apply();

                _pak.Save();
            }
            catch (Exception ex)
            {
                MessageBox.Show(string.Format("Failed to save:\n{0}", ex.Message));
            }
        }

        private void CloseFile()
        {
            _btnSave.Enabled = false;
            _btnExtract.Enabled = false;
            _btnRemove.Enabled = false;
            _btnAddFolder.Enabled = false;
            _btnAddFile.Enabled = false;
            _btnProperties.Enabled = false;
            _tvMain.Nodes.Clear();
            _folderNodes.Clear();
            _fileNodes.Clear();
            _spltMain.Panel2.Controls.Clear();

            _pak = null;
            _filePath = null;
            _currentEditor = null;
            Text = APPNAME;
        }

        private void RegisterFolderEvents(NadeoPakFolderBase folder)
        {
            folder.FolderAdded += OnFolderAdded;
            folder.FolderRemoved += OnFolderRemoved;
            folder.FileAdded += OnFileAdded;
            folder.FileRemoved += OnFileRemoved;
        }

        private void OnFolderAdded(NadeoPakFolder folder)
        {
            if (_folderNodes.ContainsKey(folder))
                return;

            if (InvokeRequired)
            {
                Invoke(new Action<NadeoPakFolder>(OnFolderAdded), folder);
                return;
            }

            PresentPAKFolder(_folderNodes[folder.ParentFolder], folder);
        }

        private void OnFolderRemoved(NadeoPakFolder folder)
        {
            if (InvokeRequired)
            {
                Invoke(new Action<NadeoPakFolder>(OnFolderRemoved), folder);
                return;
            }

            TreeNode node = _folderNodes[folder];
            node.Parent.Nodes.Remove(node);
            foreach (NadeoPakFile file in folder.AllFiles)
            {
                _fileNodes.Remove(file);
            }
            foreach (NadeoPakFolder childFolder in folder.AllFolders)
            {
                _folderNodes.Remove(childFolder);
            }
            _folderNodes.Remove(folder);
        }

        private void OnFileAdded(NadeoPakFile file)
        {
            if (_fileNodes.ContainsKey(file))
                return;

            if (InvokeRequired)
            {
                Invoke(new Action<NadeoPakFile>(OnFileAdded), file);
                return;
            }

            PresentPAKFile(_folderNodes[file.Folder], file);
        }

        private void OnFileRemoved(NadeoPakFile file)
        {
            if (InvokeRequired)
            {
                Invoke(new Action<NadeoPakFile>(OnFileRemoved), file);
                return;
            }

            TreeNode node = _fileNodes[file];
            node.Parent.Nodes.Remove(node);
            _fileNodes.Remove(file);
        }

        private TreeNode PresentPAK(NadeoPak pak)
        {
            if (_folderNodes.ContainsKey(pak))
                return _folderNodes[pak];

            _tvMain.BeginUpdate();

            TreeNode pakTreeNode = _tvMain.Nodes.Add(pak.Name);
            _folderNodes.Add(pak, pakTreeNode);
            pakTreeNode.Tag = pak;
            pakTreeNode.ImageKey = "Pack";
            pakTreeNode.SelectedImageKey = "Pack";
            foreach (NadeoPakFolder folder in pak.Folders)
            {
                if (!_folderNodes.ContainsKey(folder))
                    PresentPAKFolder(pakTreeNode, folder);
            }

            pakTreeNode.ExpandAll();
            _tvMain.EndUpdate();

            _tvMain.SelectedNode = pakTreeNode;

            RegisterFolderEvents(pak);

            return pakTreeNode;
        }

        private TreeNode PresentPAKFolder(TreeNode parentTreeNode, NadeoPakFolder folder)
        {
            if (_folderNodes.ContainsKey(folder))
                return _folderNodes[folder];

            TreeNode folderTreeNode = parentTreeNode.Nodes.Add(folder.Name);
            _folderNodes.Add(folder, folderTreeNode);
            folderTreeNode.Tag = folder;
            folderTreeNode.ImageKey = "Folder";
            folderTreeNode.SelectedImageKey = "Folder";
            foreach (NadeoPakFolder childFolder in folder.Folders)
            {
                if (!_folderNodes.ContainsKey(childFolder))
                    PresentPAKFolder(folderTreeNode, childFolder);
            }

            foreach (NadeoPakFile file in folder.Files)
            {
                if (!_fileNodes.ContainsKey(file))
                    PresentPAKFile(folderTreeNode, file);
            }

            RegisterFolderEvents(folder);

            return folderTreeNode;
        }

        private TreeNode PresentPAKFile(TreeNode parentTreeNode, NadeoPakFile file)
        {
            if (_fileNodes.ContainsKey(file))
                return _fileNodes[file];

            TreeNode fileTreeNode = parentTreeNode.Nodes.Add(string.Format("{0} ({1})", file.Name, file.ClassName));
            _fileNodes.Add(file, fileTreeNode);
            fileTreeNode.Tag = file;
            fileTreeNode.ImageKey = "File";
            fileTreeNode.SelectedImageKey = "File";
            return fileTreeNode;
        }

        private void ShowPAKFileDetails(NadeoPakFile file, bool applyPrevious)
        {
            if (_currentEditor != null && applyPrevious)
                _currentEditor.Apply();

            SuspendLayout();
            _spltMain.Panel2.Controls.Clear();
            _currentEditor = null;
            if (file == null)
            {
                ResumeLayout();
                return;
            }

            FailLog failLog = new FailLog();
            try
            {
                _currentEditor = EditorFactory.CreateEditor(file);
                _currentEditor.Dock = DockStyle.Fill;
                _spltMain.Panel2.Controls.Add(_currentEditor);
            }
            catch (Exception ex)
            {
                _spltMain.Panel2.Controls.Clear();
                failLog.Add(string.Format("Failed to decode {0}", file.FullPath), ex);
            }
            failLog.Display();

            ResumeLayout();
        }
    }
}
