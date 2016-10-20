using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Word2vec.Tools
{
    /// <summary>
    /// Reads default w2v .bin format file
    /// </summary>
    public class Word2VecBinaryReader : IWord2VecReader
    {
        public Vocabulary Read(string path)
        {
            using (var inputStream = new FileStream(path, FileMode.Open))
            {
                return Read(inputStream);
            }
        }
        
        public Vocabulary Read(Stream inputStream)
        {
            var readerSream = new BinaryReader(inputStream);

            // header
            var strHeader = Encoding.UTF8.GetString(ReadHead(readerSream));
            var split = strHeader.Split(new[] { " " }, StringSplitOptions.RemoveEmptyEntries);
            if (split.Length < 2)
                throw new FormatException("Header of binary must contains two ascii integers: vocabularySize and vectorSize");
            int vocabularySize = int.Parse(split[0]);
            int vectorSize = int.Parse(split[1]);

            // body
            var ans = new List<Representation>();
            
            var buffer = new List<byte>(); //allocation optimization

            while (true) {
                try
                {
                    ans.Add(ReadRepresentation(readerSream, buffer, vectorSize));
                }
                catch (EndOfStreamException)
                {
                    break;
                }
            }
            return new Vocabulary(ans, vectorSize);
        }

        static byte[] ReadHead(BinaryReader reader)
        {
            var buffer = new List<byte>();
            while (true)
            {
                byte symbol = reader.ReadByte();
                if (symbol == (byte)'\n')
                    break;
                else
                    buffer.Add(symbol);
            }
            return buffer.ToArray();
        }

        static Representation ReadRepresentation(BinaryReader reader, List<byte> buffer, int vectorSize)
        {

            buffer.Clear();
            var vectorSizeInByte = vectorSize * sizeof(float);
            
            while (true) {
                var symbol = reader.ReadByte();
                if (buffer.Count == 0 && symbol == (byte)'\n') 
                    continue; // ignore newline at start of new entry
                else if (symbol == (byte)' ') 
                    break;
                else
                    buffer.Add(symbol);
            }
            
            string word = Encoding.UTF8.GetString(buffer.ToArray());

            var vectorBytes = reader.ReadBytes(vectorSizeInByte);
            if (vectorBytes.Length < vectorSizeInByte) 
                throw new  EndOfStreamException("Vector entry is truncated");
            
            var vector = new float[vectorSize];
            Buffer.BlockCopy(vectorBytes, 0, vector, 0, vectorSizeInByte);

            return new Representation(word, vector);

        }

    }
}
