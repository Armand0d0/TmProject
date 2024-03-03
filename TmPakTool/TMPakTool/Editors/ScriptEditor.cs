using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Arc.TrackMania.Classes;
using Arc.TrackMania.Classes.MwFoundations;
using Arc.TrackMania.NadeoPak;

namespace paktool.Editors
{
    public partial class ScriptEditor : paktool.EditorBase
    {
        public ScriptEditor()
        {
            InitializeComponent();
        }

        public ScriptEditor(NadeoPakFile file, CMwNod node)
            : base(file, node)
        {
            InitializeComponent();
            _txtScript.Text = node.ToString();
        }

        public override void Apply()
        {
            
        }
    }
}
