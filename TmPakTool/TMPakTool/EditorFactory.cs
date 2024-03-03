using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Arc.TrackMania.Classes.MwFoundations;
using Arc.TrackMania.Classes.Plug;
using Arc.TrackMania.GameBox;
using Arc.TrackMania.NadeoPak;

namespace paktool
{
    internal class EditorFactory
    {
        private static Dictionary<Type, Type> _editors = new Dictionary<Type, Type>();

        static EditorFactory()
        {
            RegisterEditor<CPlugFileText, Editors.TextEditor>();
            //RegisterEditor<CPlugFileImg, Editors.ImageEditor>();
            RegisterEditor<CMwCmdBlockMain, Editors.ScriptEditor>();
        }

        public static void RegisterEditor<TNode, TEditor>()
            where TNode : CMwNod
            where TEditor : EditorBase
        {
            _editors.Add(typeof(TNode), typeof(TEditor));
        }

        public static EditorBase CreateEditor(NadeoPakFile file)
        {
            EditorBase editor = null;
            CMwNod node = file.CreateClassInstance();
            if (node != null)
            {
                Type editorType;
                if (!_editors.TryGetValue(node.GetType(), out editorType))
                {
                    editorType = (from KeyValuePair<Type, Type> pair in _editors
                                  where node.GetType().IsSubclassOf(pair.Key)
                                  select pair.Value).FirstOrDefault();
                }

                if (node is CPlugFile)
                {
                    node = null;
                }
                else
                {
                    try
                    {
                        GameBox gbx = new GameBox(file);
                        node = gbx.MainNode;
                    }
                    catch
                    {
                        node = null;
                    }
                }

                if (editorType != null)
                    editor = (EditorBase)Activator.CreateInstance(editorType, file, node);
            }
            if (editor == null)
                editor = new Editors.HexEditor(file, null);
            
            return editor;
        }
    }
}
