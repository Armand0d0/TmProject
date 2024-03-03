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
    public partial class FolderProperties : Form
    {
        private NadeoPakFolder _folder;
        private EditableAdapter<NadeoPakFolder> _editableFolder;

        public FolderProperties()
        {
            InitializeComponent();
        }

        public FolderProperties(NadeoPakFolder folder)
            : this()
        {
            _folder = folder;
            _editableFolder = new EditableAdapter<NadeoPakFolder>(folder);
            _bsFolder.DataSource = _editableFolder;
            Text = string.Format("{0} properties", folder.Name);
        }

        private void FolderProperties_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (DialogResult == DialogResult.OK)
                _editableFolder.EndEdit();
            else
                _editableFolder.CancelEdit();
        }

        private void _btnOK_Click(object sender, EventArgs e)
        {
            if (_folder.Name.Length == 0)
            {
                MessageBox.Show("Please enter a folder name.", "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            if (!_folder.Name.EndsWith(@"\"))
                _folder.Name += @"\";

            if (_folder.ParentFolder != null &&
                _folder.ParentFolder.Folders.Where(f => f.Name == _folder.Name).Count() > 1)
            {
                MessageBox.Show("Another folder with that name already exists.", "", MessageBoxButtons.OK,
                    MessageBoxIcon.Exclamation);
                return;
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
