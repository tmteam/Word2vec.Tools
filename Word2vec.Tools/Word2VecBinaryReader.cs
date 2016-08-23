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

            var strHeader = new string(ASCIIEncoding.UTF8.GetChars(ReadHead(readerSream)));

            var splitted = strHeader.Split(new[] { " " }, StringSplitOptions.RemoveEmptyEntries);
            if (splitted.Length < 2)
                throw new FormatException("header");

            int vocabularySize = int.Parse(splitted[0]);
            int vectorSize = int.Parse(splitted[1]);

            var buffer = new List<byte>();
            var ans = new List<WordRepresentation>();

            while (readerSream.BaseStream.Position < readerSream.BaseStream.Length - vectorSize * sizeof(float))
                ans.Add(ReadRepresentation(readerSream, buffer, vectorSize));

            return new Vocabulary(ans, vectorSize);
        }

        byte[] ReadHead(BinaryReader readerSream)
        {
            var buffer = new List<byte>();
            while (true)
            {
                var symbol = readerSream.ReadByte();
                if (symbol == (int)'\n')
                    break;
                else
                    buffer.Add(symbol);
            }
            return buffer.ToArray();
        }
        WordRepresentation ReadRepresentation(BinaryReader readerSream, List<byte> buffer, int vectorSize)
        {
            buffer.Clear();
            var vectorSizeInByte = vectorSize * sizeof(float);

            while (true)
            {
                var symbol = readerSream.ReadByte();
                if (symbol == (int)' ')
                    break;
                buffer.Add(symbol);
            }
            
            
            var word = new string(ASCIIEncoding.UTF8.GetChars(buffer.ToArray()));

            var vector = new float[vectorSize];
            Buffer.BlockCopy(readerSream.ReadBytes(vectorSizeInByte), 0, vector, 0, vectorSizeInByte);

            while (readerSream.PeekChar() == (int)'\n')
                readerSream.ReadByte();
            
            return new WordRepresentation(word, vector);
        }

    }
}
