using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using Arc.TrackMania.Classes.MwFoundations;

namespace Arc.TrackMania.Classes.Game
{
    public class CGameCtnCollectorList : Arc.TrackMania.Classes.MwFoundations.CMwNod
    {
        public override uint ID
        {
            get { return 0x301B000; }
        }

        protected override CMwNod.ChunkFlags GetChunkFlags(uint chunkID)
        {
            switch (chunkID)
            {
                case 0x301B000:
                    return ChunkFlags.Known | ChunkFlags._Flag1;
            }
            return ChunkFlags.None;
        }

        public class Chunk000 : NodeChunk
        {
            public class Item
            {
                public Meta Meta;
                public uint Dword;
            }
            public BindingList<Item> Items;

            internal override void ReadWrite(CClassicArchive archive, CMwNod node)
            {
                if (!archive.Writing)
                    Items = new BindingList<Item>();

                int num = Items.Count;
                archive.ReadWrite(ref num);
                for (int i = 0; i < num; i++)
                {
                    if (!archive.Writing)
                        Items.Add(new Item());

                    Items[i].Meta.ReadWrite(archive);
                    archive.ReadWrite(ref Items[i].Dword);
                }
            }
        }
    }
}
