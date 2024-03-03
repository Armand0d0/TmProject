using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Arc.TrackMania
{
    internal static class ClassIDByExtension
    {
        private static Dictionary<string, uint> _extToClass = new Dictionary<string, uint>()
        {
            { ".3ds",         0x0909B000 },
            { ".avi",         0x09032000 },
            { ".bik",         0x0905F000 },
            { ".cry",         0x09041000 },
            { ".dds",         0x09024000 },
            { ".fnt",         0x0902D000 },
            { ".fx",          0x09094000 },
            { ".jpeg",        0x09022000 },
            { ".jpg",         0x09022000 },
            { ".mo",          0x09055000 },
            { ".mpj.cry",     0x09041000 },
            { ".mpj.txt",     0x09041000 },
            { ".mux",         0x0905A000 },
            { ".obj",         0x09099000 },
            { ".ogg",         0x0905A000 },
            { ".pak",         0x09019000 },
            { ".phlsl.cry",   0x09077000 },
            { ".phlsl.txt",   0x09077000 },
            { ".png",         0x0903D000 },
            { ".psh.cry",     0x09045000 },
            { ".psh.txt",     0x09045000 },
            { ".script.cry",  0x09041000 },
            { ".script.txt",  0x09041000 },
            { ".text.cry",    0x09041000 },
            { ".text.txt",    0x09041000 },
            { ".tga",         0x09023000 },
            { ".ttc",         0x0902D000 },
            { ".ttf",         0x0902D000 },
            { ".txt",         0x09041000 },
            { ".vhlsl.cry",   0x09074000 },
            { ".vhlsl.txt",   0x09074000 },
            { ".vpp.cry",     0x09041000 },
            { ".vpp.txt",     0x09041000 },
            { ".vsh.cry",     0x09042000 },
            { ".vsh.txt",     0x09042000 },
            { ".wav",         0x09031000 },
            { ".zip",         0x09084000 }
        };

        public static uint ExtensionToClassID(string extension)
        {
            uint classID;
            _extToClass.TryGetValue(extension.ToLower(), out classID);
            return classID;
        }
    }
}
