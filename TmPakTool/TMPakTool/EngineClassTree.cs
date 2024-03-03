using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Arc.TrackMania;

namespace paktool
{
    [DefaultBindingProperty("ClassID")]
    class EngineClassTree : TreeView
    {
        private Dictionary<TreeNode, bool> _engineNodeFilled = new Dictionary<TreeNode, bool>();
        private Dictionary<int, TreeNode> _engineNodes = new Dictionary<int, TreeNode>();
        private Dictionary<uint, TreeNode> _classNodes = new Dictionary<uint, TreeNode>();

        public EngineClassTree()
        {
            if (LicenseManager.UsageMode == LicenseUsageMode.Designtime)
                return;

            HideSelection = false;

            foreach (CMwEngineInfo engine in CMwEngineManager.Engines)
            {
                TreeNode engineTreeNode = Nodes.Add(engine.Name);
                engineTreeNode.Tag = engine;
                _engineNodes.Add(engine.ID, engineTreeNode);

                engineTreeNode.Nodes.Add("[Dummy]");
            }

            BeforeExpand += EngineClassTree_BeforeExpand;
        }

        private void EngineClassTree_BeforeExpand(object sender, TreeViewCancelEventArgs e)
        {
            if (_engineNodeFilled.ContainsKey(e.Node))
                return;

            e.Node.Nodes.Clear();
            foreach (CMwClassInfo engineClass in ((CMwEngineInfo)e.Node.Tag).Classes)
            {
                TreeNode classTreeNode = e.Node.Nodes.Add(engineClass.Name);
                classTreeNode.Tag = engineClass;
                _classNodes.Add(engineClass.ID, classTreeNode);
            }
            _engineNodeFilled.Add(e.Node, true);
        }

        public uint ClassID
        {
            get
            {
                if (SelectedNode == null || !(SelectedNode.Tag is CMwClassInfo))
                    return 0;

                return ((CMwClassInfo)SelectedNode.Tag).ID;
            }
            set
            {
                TreeNode engineTreeNode;
                int engineID = (int)(value >> 24);
                if (!_engineNodes.TryGetValue(engineID, out engineTreeNode))
                    return;

                engineTreeNode.Expand();

                TreeNode classNode;
                if (!_classNodes.TryGetValue(value, out classNode))
                    return;

                SelectedNode = classNode;
            }
        }
    }
}
