using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Arc.TrackMania.Classes.MwFoundations;

namespace Arc.TrackMania.Classes.Game
{
    public class CGameCtnBlockSkin : Arc.TrackMania.Classes.MwFoundations.CMwNod
    {
        public override uint ID
        {
            get { return 0x03059000; }
        }

        protected override CMwNod.ChunkFlags GetChunkFlags(uint chunkID)
        {
            switch (chunkID)
            {
                case 0x03059000:
                case 0x03059001:
                    return ChunkFlags.Known;

                case 0x03059002:
                    return ChunkFlags.Known | ChunkFlags._Flag1;
            }
            return ChunkFlags.None;
        }

        public class Chunk000 : NodeChunk
        {
            public string String0;
            public string String1;

            internal override void ReadWrite(CClassicArchive archive, CMwNod node)
            {
                archive.ReadWrite(ref String0);
                archive.ReadWrite(ref String1);
            }
        }

        public class Chunk001 : NodeChunk
        {
            public string String0;
            public FileReference FileRef = new FileReference();

            internal override void ReadWrite(CClassicArchive archive, CMwNod node)
            {
                archive.ReadWrite(ref String0);
                FileRef.ReadWrite(archive);
            }
        }

        public class Chunk002 : NodeChunk
        {
            public string String0;
            public FileReference FileRef0 = new FileReference();
            public FileReference FileRef1 = new FileReference();

            internal override void ReadWrite(CClassicArchive archive, CMwNod node)
            {
                archive.ReadWrite(ref String0);
                FileRef0.ReadWrite(archive);
                FileRef1.ReadWrite(archive);
            }
        }
    }
}
