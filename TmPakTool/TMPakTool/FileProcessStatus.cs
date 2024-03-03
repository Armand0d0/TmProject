using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace paktool
{
    public partial class FileProcessStatus : Form
    {
        private bool _cancelable = true;
        private bool _complete = false;

        private FileProcessStatus()
        {
            InitializeComponent();
        }

        public FileProcessStatus(int numFiles)
            : this()
        {
            NumFiles = numFiles;
            CreateHandle();
        }

        public FileProcessStatus(int numFiles, bool cancelable)
            : this(numFiles)
        {
            _cancelable = cancelable;
            if (!_cancelable)
            {
                _btnCancel.Visible = false;
                Height = 90;
                CancelButton = null;
            }
        }

        private void FileProcessStatus_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!_cancelable)
            {
                e.Cancel = true;
                return;
            }

            if (UserCanceled != null)
                UserCanceled();

            _complete = true;
        }

        public int NumFiles
        {
            get { return _progressBar.Maximum; }
            set { _progressBar.Maximum = value; }
        }

        public event Action UserCanceled;

        public void BeginFile(string fileName)
        {
            if (_complete)
                return;

            if (InvokeRequired)
            {
                BeginInvoke(new Action<string>(BeginFile), fileName);
                return;
            }

            if (_lblFile.Text == "")
            {
                Show();
                BringToFront();
            }

            _lblFile.Text = fileName;
        }

        public void CompleteFile()
        {
            if (_complete)
                return;

            if (InvokeRequired)
            {
                BeginInvoke(new Action(CompleteFile));
                return;
            }

            _progressBar.PerformStep();
            if (_progressBar.Value == _progressBar.Maximum)
            {
                _complete = true;
                Close();
            }
        }

        public new void Close()
        {
            _cancelable = true;
            base.Close();
        }

        private void _btnCancel_Click(object sender, EventArgs e)
        {
            if (_complete)
                return;

            Close();
        }
    }
}
