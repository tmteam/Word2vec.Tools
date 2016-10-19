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
                throw new FormatException("Header of binary must contain two ascii integers: vocabularySize vectorSize");
            int vocabularySize = int.Parse(split[0]);
            int vectorSize = int.Parse(split[1]);

            // body
            var ans = new List<WordRepresentation>();
            while (true) { 
                var rep = ReadRepresentation(readerSream, vectorSize);
                if (rep != null) {
                    ans.Add(rep);
                } else {
                    break;
                }
            }

            if (ans.Count != vocabularySize) {
                // Give warning?
            }

            return new Vocabulary(ans, vectorSize);
        }

        byte[] ReadHead(BinaryReader reader)
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

        WordRepresentation ReadRepresentation(BinaryReader reader, int vectorSize)
        {
            int MAX_WORD_LENGTH = 10000;
            byte[] wordBytes = new byte[MAX_WORD_LENGTH];
            int wordLength = 0;

            try {
                while (true) {
                    var symbol = reader.ReadByte();
                    if (wordLength == 0 && symbol == (byte)'\n') {
                        continue; // ignore newline at start of new entry
                    } else if (symbol == (byte)' ') {
                        break;
                    } else if (wordLength < MAX_WORD_LENGTH) {
                        wordBytes[wordLength++] = symbol;
                    } else {
                        // Or just ignore it?
                        Console.Error.WriteLine("Warning: Maximum word length exceeded: " + MAX_WORD_LENGTH);
                    }
                }
                string word = Encoding.UTF8.GetString(wordBytes,0,wordLength);

                var vectorSizeInByte = vectorSize * sizeof(float);
                var vectorBytes = reader.ReadBytes(vectorSizeInByte);
                if (vectorBytes.Length < vectorSizeInByte) {
                    return null; // Final entry is truncated. Give a warning?
                }
                var vector = new float[vectorSize];
                Buffer.BlockCopy(vectorBytes, 0, vector, 0, vectorSizeInByte);

                return new WordRepresentation(word, vector);

            } catch (EndOfStreamException) {
                return null;
            }
        }

    }
}
