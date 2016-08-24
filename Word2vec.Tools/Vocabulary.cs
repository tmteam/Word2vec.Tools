﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace Word2vec.Tools
{
    /// <summary>
    /// known w2v vectors
    /// </summary>
    public class Vocabulary 
    {
        /// <summary>
        /// All known words w2v representations
        /// </summary>
        public readonly WordRepresentation[] Words;
        /// <summary>
        /// w2v words vectors dimensions count
        /// </summary>
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
                if (!string.IsNullOrWhiteSpace(representation.Word) && !_dictioanary.ContainsKey(representation.Word))
                    _dictioanary.Add(representation.Word, representation);
            }
            Words = _dictioanary.Values.ToArray();
        }
        /// <summary>
        /// Returns word2vec word vector if it exists.
        /// Returns null otherwise
        /// </summary>
        public WordRepresentation GetRepresentationOrNullFor(string word)
        {
            if (ContainsWord(word))
                return GetRepresentationFor(word);
            else
                return null;
        }
        /// <summary>
        /// Returns word2vec word vector if it exists.
        /// Throw otherwise
        /// </summary>
        public WordRepresentation GetRepresentationFor(string word)
        {
            return _dictioanary[word];
        }
        public WordRepresentation this[string word] { get { return GetRepresentationFor(word); } }
        public bool ContainsWord(string word)
        {
            return _dictioanary.ContainsKey(word);
        }

        /// <summary>
        /// returns "count" of closest words for target representation
        /// </summary>
        public WordDistance[] Distance(Representation representation, int maxCount)
        {
            return representation.GetClosestFrom(Words.Where(v => v != representation), maxCount);
        }
       
        /// <summary>
        /// if word exists - returns "count" of best fits for target word
        /// otherwise - returns empty array
        /// </summary>
        public WordDistance[] Distance(string word, int count)
        {
            if (!this.ContainsWord(word))
                return new WordDistance[0];

            return Distance(this[word], count);
         }
        /// <summary>
        /// If wordA is wordB, then wordC is...
        /// If all words exist - returns "count" of best fits for the result
        /// otherwise - returns empty array
        /// </summary>
        public WordDistance[] Analogy(string wordA, string wordB, string wordC, int count)
        {
            if (!ContainsWord(wordA) || !ContainsWord(wordB) || !ContainsWord(wordC))
                return new WordDistance[0];
            else
                return Analogy(GetRepresentationFor(wordA), GetRepresentationFor(wordB), GetRepresentationFor(wordC), count);
        }
        /// <summary>
        /// If wordA is wordB, then wordC is...
        /// Returns "count" of best fits for the result
        /// </summary>
        public WordDistance[] Analogy(Representation wordA, Representation wordB, Representation wordC, int count)
        {
           var cummulative =  wordB.Substract(wordA).Add(wordC);
           return  cummulative.GetClosestFrom(Words.Where(t => t != wordA && t != wordB && t != wordC), count);
        }
    }
}