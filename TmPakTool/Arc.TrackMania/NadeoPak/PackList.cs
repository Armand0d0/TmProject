using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Arc.TrackMania.NadeoPak
{
    public class PackList
    {
        private MD5CryptoServiceProvider _md5 = new MD5CryptoServiceProvider();
        private byte[] _nameKey;
        private Dictionary<string, string> _pakKeyStrings;
        private Dictionary<string, byte[]> _pakKeys;

        private const string SIGMAGIC = "E3554B5828AF14F11AA42A5EAF0AEFC8";
        private const string NAMEMAGIC = "6611992868945B0B59536FC3226F3FD0";
        private const string KEYMAGIC_NOCRC = "B97C1205648A66E04F86A1B5D5AF9862";
        private const string KEYMAGIC_CRC = "1FCF6EFCF41CAAAD0B810C656DF2DE33";

        /// <summary>
        /// Creates a new PackList from a packlist.dat on disk.
        /// </summary>
        /// <param name="filePath">The path to the packlist.dat to load.</param>
        public PackList(string filePath)
        {
            if (!File.Exists(filePath))
                throw new Exception(string.Format("{0} not found", filePath));

            _pakKeyStrings = new Dictionary<string, string>();
            _pakKeys = new Dictionary<string, byte[]>();
            using (Stream file = File.OpenRead(filePath))
            {
                BinaryReader reader = new BinaryReader(file);

                byte version = reader.ReadByte();
                if (version != 1)
                    throw new Exception(string.Format("Incorrect packlist.dat version. Expected 1, got {0}", version));

                int numPacks = reader.ReadByte();
                uint crc32 = reader.ReadUInt32();
                uint num = reader.ReadUInt32();

                file.Position = 0;
                byte[] data = reader.ReadBytes((int)file.Length - 0x10);
                byte[] signature = reader.ReadBytes(0x10);
                if (!VerifySignature(data, signature, num))
                    throw new Exception("packlist.dat signature is incorrect");

                file.Position = 0xA;
                _nameKey = CalcNameKey(num);

                for (int i = 0; i < numPacks; i++)
                {
                    ReadPakEntry(reader, num);
                }
            }
        }

        /// <summary>
        /// Gets the names of the packs that are contained in the pack list
        /// </summary>
        public IEnumerable<string> Packs
        {
            get { return _pakKeys.Keys; }
        }

        /// <summary>
        /// Gets the key string for the pack with the specified name, or null if no pack with
        /// that name was found. The key string is used to derive the decryption key.
        /// </summary>
        /// <param name="pakName">The name of the pack to search for. Case insensitive.</param>
        /// <returns></returns>
        public string GetPakKeyString(string pakName)
        {
            if (_pakKeyStrings == null)
                return null;

            string keyString;
            _pakKeyStrings.TryGetValue(pakName.ToLower(), out keyString);
            return keyString;
        }

        /// <summary>
        /// Gets the actual decryption key for the pack with the specified name, or null if no
        /// pack with that name was found. The key is derived from the key string.
        /// </summary>
        /// <param name="pakName"></param>
        /// <returns></returns>
        public byte[] GetPakKey(string pakName)
        {
            if (_pakKeys == null)
                return null;

            byte[] key = null;
            _pakKeys.TryGetValue(pakName.ToLower(), out key);
            return key;
        }

        private bool VerifySignature(byte[] data, byte[] signature, uint num)
        {
            byte[] stage0 = _md5.ComputeHash(Encoding.ASCII.GetBytes(SIGMAGIC + Convert.ToString(num)));
            
            byte[] stage0_36 = new byte[0x40];
            byte[] stage0_5C = new byte[0x40];
            Array.Copy(stage0, stage0_36, stage0.Length);
            Array.Copy(stage0, stage0_5C, stage0.Length);
            for (int i = 0; i < 0x40; i++)
            {
                stage0_36[i] ^= 0x36;
                stage0_5C[i] ^= 0x5C;
            }

            byte[] stage1 = new byte[0x40 + data.Length];
            Array.Copy(stage0_36, 0, stage1, 0, 0x40);
            Array.Copy(data, 0, stage1, 0x40, data.Length);
            stage1 = _md5.ComputeHash(stage1);

            byte[] stage2 = new byte[0x40 + 0x10];
            Array.Copy(stage0_5C, 0, stage2, 0, 0x40);
            Array.Copy(stage1, 0, stage2, 0x40, stage1.Length);
            byte[] calcSig = _md5.ComputeHash(stage2);

            for (int i = 0; i < 0x10; i++)
            {
                if (calcSig[i] != signature[i])
                    return false;
            }
            return true;
        }

        private byte[] CalcNameKey(uint num)
        {
            return _md5.ComputeHash(Encoding.ASCII.GetBytes(string.Format("{0}{1}", NAMEMAGIC, num)));
        }

        private byte[] CalcKeyKey(uint num, string pakName)
        {
            return _md5.ComputeHash(Encoding.ASCII.GetBytes(string.Format("{0}{1}{2}",
                pakName, num, KEYMAGIC_NOCRC)));
        }

        private void ReadPakEntry(BinaryReader reader, uint num)
        {
            byte flags = reader.ReadByte();
            byte nameLength = reader.ReadByte();
            byte[] encryptedName = reader.ReadBytes(nameLength);
            byte[] encryptedKey = reader.ReadBytes(0x20);

            for (int i = 0; i < encryptedName.Length; i++)
                encryptedName[i] ^= _nameKey[i % _nameKey.Length];

            string name = Encoding.ASCII.GetString(encryptedName);

            byte[] keyKey = CalcKeyKey(num, name);
            for (int i = 0; i < encryptedKey.Length; i++)
                encryptedKey[i] ^= keyKey[i % keyKey.Length];

            string keyString = Encoding.ASCII.GetString(encryptedKey);
            byte[] key = _md5.ComputeHash(Encoding.ASCII.GetBytes(keyString + "NadeoPak"));

            _pakKeyStrings.Add(name.ToLower(), keyString);
            _pakKeys.Add(name.ToLower(), key);
        }
    }
}
