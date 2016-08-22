using System;
using System.Collections.Generic;
using System.Linq;

namespace Word2vec.Tools
{
    /// <summary>
    /// known w2v vectors
    /// </summary>
    public class Vocabulary 
    {
        public readonly WordRepresentation[] Words;
        public int VectorDimensionsCount { get; set; }

        readonly Dictionary<string, WordRepresentation> _dictioanary;

        public Vocabulary(IEnumerable<WordRepresentation> representations, int vectorDimensionsCount)
        {
            _dictioanary = new Dictionary<string, WordRepresentation>();
            this.VectorDimensionsCount = vectorDimensionsCount;
            foreach (var representation in representations)
            {
                if (representation.NumericVector.Length != vectorDimensionsCount)
                    throw new ArgumentException("representations.Vector.Length");
                _dictioanary.Add(representation.Word, representation);
            }
            Words = _dictioanary.Values.ToArray();
        }
       
        public WordRepresentation this[string word] { get { return _dictioanary[word]; } }
        public bool ContainsWord(string word)
        {
            return _dictioanary.ContainsKey(word);
        }
    
        public WordDistance[] Distance(Representation representation, int count)
        {
            return Distance(Words.Where(v => v != representation), representation, count);
        }
        WordDistance[] Distance(IEnumerable<WordRepresentation> vectors, Representation target, int count)
        {
            return vectors.Select(target.GetCosineDistanceToWord)
               .OrderByDescending(s => s.Distance)
               .Take(count)
               .ToArray();
        }
        public WordDistance[] Distance(string word, int count)
        {
            if (!this.ContainsWord(word))
                return new WordDistance[0];

            return Distance(this[word], count);
         }
        public WordDistance[] Analogy(string wordA, string wordB, string wordC, int count)
        {
            return Analogy(this[wordA], this[wordB], this[wordC], count);
        }
        public WordDistance[] Analogy(Representation wordA, Representation wordB, Representation wordC, int count)
        {
           var cummulative =  wordB.Substract(wordA).Add(wordC);
           return Distance(Words.Where(t => t != wordA && t != wordB && t != wordC), cummulative, count);
        }
    }
}
