using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Word2vec.Tools
{
    /// <summary>
    /// Reads default w2v text file
    /// </summary>
    public class Word2VecTextReader: IWord2VecReader
    {
        public Vocabulary Read(System.IO.Stream inputStream)
        {
            using (var strStream = new System.IO.StreamReader(inputStream))
            {
                var firstLine = strStream.ReadLine().Split(' ');
                var vocabularySize = int.Parse(firstLine[0]);
                var vectorSize = int.Parse(firstLine[1]);

                var vectors = new List<WordRepresentation>(vocabularySize);

                while (!strStream.EndOfStream)
                {
                    var line = strStream.ReadLine().Split(' ');
                    var vecs = line.Skip(1).Take(vectorSize).ToArray();
                    vectors.Add(new WordRepresentation(
                       word: line.First(),
                       vector: vecs.Select(v=>Single.Parse(v, System.Globalization.CultureInfo.InvariantCulture)).ToArray()));
                }
                return new Vocabulary(vectors, vectorSize);
            }
        }

        public Vocabulary Read(string path)
        {
            return Read(new FileStream(path, FileMode.Open));
        }
    }
}
