using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Arc.TrackMania.Classes.MwFoundations;
using Arc.TrackMania.Classes.Plug;
using Arc.TrackMania.NadeoPak;

namespace paktool.Editors
{
    public partial class TextEditor : paktool.EditorBase
    {
        public TextEditor()
        {
            InitializeComponent();
        }

        public TextEditor(NadeoPakFile file, CMwNod node)
            : base(file, node)
        {
            InitializeComponent();
            _txtText.Text = Encoding.ASCII.GetString(file.Data);
        }

        public override void Apply()
        {
            File.Data = Encoding.ASCII.GetBytes(_txtText.Text);
        }
    }
}
