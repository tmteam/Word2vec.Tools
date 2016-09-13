using System;
using System.Collections.Generic;
using System.Globalization;
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
                bool isFirstLine = true;
                int vocabularySize = 0;
                int vectorSize = -1;

                //var firstLine = strStream.ReadLine().Split(' ');

                var vectors = new List<WordRepresentation>();

                var enUsCulture = CultureInfo.GetCultureInfo("en-US");
                while (!strStream.EndOfStream)
                {
                    var line = strStream.ReadLine().Split(' ');
                    if (isFirstLine) {
                        if (line.Length == 2) {
                            try {
                                //header
                                vocabularySize = int.Parse(line[0]);
                                vectorSize = int.Parse(line[1]);
                                vectors = new List<WordRepresentation>(vocabularySize);
                                continue; 
                            } catch {
                                vocabularySize = 0;
                                vectorSize = -1;
                            }
                        }

                        if (vectorSize == -1) {
                            // no header, so require all other vectors to match first line's length
                            vectorSize = line.Length - 1;
                        }

                        isFirstLine = false;
                    }

                    var vecs = line.Skip(1).Take(vectorSize).ToArray();
                    if (vecs.Length != vectorSize)
                        throw new FormatException("word \"" + line.First() + "\" has wrong vector size of " + vecs.Length);
                    
                    vectors.Add(new WordRepresentation(
                       word: line.First(),
                       vector: vecs.Select(v => Single.Parse(v, enUsCulture)).ToArray()));
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
