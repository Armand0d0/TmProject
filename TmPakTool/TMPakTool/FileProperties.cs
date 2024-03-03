using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Arc.TrackMania.NadeoPak;

namespace paktool
{
    public partial class FileProperties : Form
    {
        private NadeoPakFile _file;
        private EditableAdapter<NadeoPakFile> _editableFile;

        public FileProperties()
        {
            InitializeComponent();
        }

        public FileProperties(NadeoPakFile file)
            : this()
        {
            _txtFlags.DataBindings["Text"].Format += delegate(object sender, ConvertEventArgs e)
            {
                e.Value = string.Format("{0:X016}", (ulong)e.Value);
            };

            _txtFlags.DataBindings["Text"].Parse += delegate(object sender, ConvertEventArgs e)
            {
                e.Value = ulong.Parse(e.Value.ToString(), System.Globalization.NumberStyles.AllowHexSpecifier);
            };

            _file = file;
            _editableFile = new EditableAdapter<NadeoPakFile>(file);
            _bsFile.DataSource = _editableFile;
            Text = string.Format("{0} properties", file.Name);

            if (_file.MetaData != null)
            {
                _spltMain.Panel2Collapsed = false;
                _hexMetaData.ByteProvider = new Be.Windows.Forms.DynamicByteProvider(_file.MetaData);
                Width = 740;
            }
            else
            {
                _spltMain.Panel2Collapsed = true;
                Width = 400;
            }
        }

        private void FileProperties_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (DialogResult == System.Windows.Forms.DialogResult.OK)
                _editableFile.EndEdit();
            else
                _editableFile.CancelEdit();
        }

        private void _btnOK_Click(object sender, EventArgs e)
        {
            if (_file.Name.Length == 0)
            {
                MessageBox.Show("Please enter a file name.", "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            if (_file.Folder != null &&
                _file.Folder.Files.Where(f => f.Name == _file.Name).Count() > 1)
            {
                MessageBox.Show("Another file with that name already exists.", "", MessageBoxButtons.OK,
                    MessageBoxIcon.Exclamation);
                return;
            }

            if (_hexMetaData.ByteProvider != null)
            {
                _file.MetaData = ((Be.Windows.Forms.DynamicByteProvider)_hexMetaData.ByteProvider).Bytes.GetBytes();
            }

            DialogResult = DialogResult.OK;
            Close();
        }

        private void _btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }
    }
}
