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
    public partial class ImageEditor : paktool.EditorBase
    {
        public ImageEditor()
        {
            InitializeComponent();
        }

        public ImageEditor(NadeoPakFile file, CMwNod node)
            : base(file, node)
        {
            InitializeComponent();
            
        }
    }
}
