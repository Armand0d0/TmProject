using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Arc.TrackMania.Classes.MwFoundations;
using Arc.TrackMania.Classes.Plug;
using Arc.TrackMania.GameBox;
using Arc.TrackMania.NadeoPak;

namespace paktool
{
    public partial class EditorBase : UserControl
    {
        private NadeoPakFile _file;
        private CMwNod _node;

        public EditorBase()
        {
            InitializeComponent();
        }

        public EditorBase(NadeoPakFile file, CMwNod node)
            : this()
        {
            _file = file;
            _node = node;
        }

        public NadeoPakFile File
        {
            get { return _file; }
        }

        public CMwNod Node
        {
            get { return _node; }
        }

        public virtual void Apply()
        {

        }

        protected void UpdateFile()
        {
            if (Node is CPlugFile)
            {
                Node.Write(File);
            }
            else
            {
                GameBox gbx = new GameBox();
                gbx.MainNode = Node;
                gbx.Write(File);
            }
        }
    }
}
