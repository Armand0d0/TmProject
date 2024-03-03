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
    public partial class PackListViewer : Form
    {
        private PackList _packList;

        public PackListViewer()
        {
            InitializeComponent();
        }

        public PackListViewer(PackList packList)
            : this()
        {
            _packList = packList;
            foreach (string pack in packList.Packs)
            {
                ListViewItem item = _lstPacks.Items.Add(pack);
                item.SubItems.Add(packList.GetPakKeyString(pack));
                item.SubItems.Add(GetByteString(packList.GetPakKey(pack)));
            }
        }

        private void _btnClipboard_Click(object sender, EventArgs e)
        {
            StringBuilder result = new StringBuilder();
            foreach (string pack in _packList.Packs)
            {
                result.AppendFormat("{0}{1}{2}\r\n",
                    pack.PadRight(20),
                    _packList.GetPakKeyString(pack).PadRight(34),
                    GetByteString(_packList.GetPakKey(pack)).PadRight(34)
                );
            }

            Clipboard.Clear();
            Clipboard.SetText(result.ToString());
        }

        private void _btnOK_Click(object sender, EventArgs e)
        {
            Close();
        }

        private static string GetByteString(byte[] bytes)
        {
            StringBuilder sb = new StringBuilder();
            foreach (byte b in bytes)
            {
                sb.AppendFormat("{0:X02}", b);
            }
            return sb.ToString();
        }
    }
}
