using System;
using System.Collections.Generic;
using System.Linq;

namespace Word2vec.Tools
{
    /// <summary>
    /// w2v meaning vector
    /// </summary>
    public class Representation
    {
        public Representation(float[] numericVector)
        {
            NumericVector = numericVector;
            MetricLength = Math.Sqrt(NumericVector.Sum(v => v * v));
        }
        public readonly float[] NumericVector;
        public readonly double MetricLength;
        public WordDistance GetCosineDistanceToWord(WordRepresentation representation)
        {
            return new WordDistance(representation, GetCosineDistanceTo(representation));
        }

        public double GetCosineDistanceTo(Representation representation)
        {
            double distance = 0;
            for (var i = 0; i < NumericVector.Length; i++)
                distance += NumericVector[i] * representation.NumericVector[i];

            return distance / (MetricLength * representation.MetricLength);
        }
        public Representation Substract(Representation representation)
        {
            var ans = new float[NumericVector.Length];
            for (int i = 0; i < NumericVector.Length; i++)
                ans[i] = NumericVector[i] - representation.NumericVector[i];
            
            return new Representation(ans);
        }
        public Representation Add(Representation representation)
        {
            var ans = new float[NumericVector.Length];
            
            for (int i = 0; i < NumericVector.Length; i++)
                ans[i] = NumericVector[i] + representation.NumericVector[i];
            
            return new Representation(ans);
        }
        public LinkedDistance<T>[] GetClosestFrom<T>(IEnumerable<T> representationsWrappers, Func<T, Representation> locator, int maxCount)
        {
            return representationsWrappers.Select(r => new LinkedDistance<T>(locator(r), r, GetCosineDistanceTo(locator(r))))
               .OrderByDescending(s => s.Distance)
               .Take(maxCount)
               .ToArray();
        }
        public Tuple<WordDistance,T>[] GetClosestFrom<T>(IEnumerable<T> representationsWrappers, Func<T, WordRepresentation> locator, int maxCount)
        {
            return representationsWrappers.Select(r=>
                new Tuple<WordDistance, T>(GetCosineDistanceToWord(locator(r)), r))
               .OrderByDescending(s => s.Item1.Distance)
               .Take(maxCount)
               .ToArray();
        }
        public WordDistance[] GetClosestFrom(IEnumerable<WordRepresentation> representations, int maxCount)
        {
            return representations.Select(GetCosineDistanceToWord)
               .OrderByDescending(s => s.Distance)
               .Take(maxCount)
               .ToArray();
        }
    }
}
