using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;
using Arc.TrackMania.Classes.MwFoundations;
using Arc.TrackMania.NadeoPak;

namespace paktool.Editors
{
    public partial class HexEditor : EditorBase
    {
        public HexEditor()
        {
            InitializeComponent();
        }

        public HexEditor(NadeoPakFile file, CMwNod node)
            : base(file, node)
        {
            InitializeComponent();
            _hexBox.ByteProvider = new Be.Windows.Forms.DynamicByteProvider(file.Data);
        }

        public override void Apply()
        {
            File.Data = ((Be.Windows.Forms.DynamicByteProvider)_hexBox.ByteProvider).Bytes.GetBytes();
        }
    }
}
