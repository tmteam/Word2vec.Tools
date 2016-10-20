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
        private readonly string _wordOrNull;
        private readonly float[] _numericVector;
        private readonly double _metricLength;
        public string WordOrNull { get { return _wordOrNull; }  }

        public float[] NumericVector
        {
            get { return _numericVector; }
        }

        public double MetricLength
        {
            get { return _metricLength; }
        }
        public Representation(string word, float[] vector)
        {
            _wordOrNull = word;
            _numericVector = vector;
            _metricLength = Math.Sqrt(_numericVector.Sum(v => v * v));
        }
        public Representation(float[] numericVector)
        {
            _wordOrNull = null;
            _numericVector = numericVector;
            _metricLength = Math.Sqrt(_numericVector.Sum(v => v * v));
        }
       

        public DistanceTo GetCosineDistanceTo(Representation representation)
        {
            double distance = 0;
            for (var i = 0; i < _numericVector.Length; i++)
                distance += _numericVector[i] * representation._numericVector[i];

            return new DistanceTo(representation, distance / (_metricLength * representation._metricLength));
        }
        public Representation Substract(Representation representation)
        {
            var ans = new float[_numericVector.Length];
            for (int i = 0; i < _numericVector.Length; i++)
                ans[i] = _numericVector[i] - representation._numericVector[i];
            
            return new Representation(ans);
        }
        public Representation Add(Representation representation)
        {
            var ans = new float[_numericVector.Length];
            
            for (int i = 0; i < _numericVector.Length; i++)
                ans[i] = _numericVector[i] + representation._numericVector[i];
            
            return new Representation(ans);
        }
        public DistanceTo[] GetClosestFrom(IEnumerable<Representation> representations, int maxCount)
        {
            return representations.Select(GetCosineDistanceTo)
               .OrderByDescending(s => s.DistanceValue)
               .Take(maxCount)
               .ToArray();
        }
        public LinkedDistance<T>[] GetClosestFrom<T>(IEnumerable<T> representationsWrappers, Func<T, Representation> locator, int maxCount)
        {
            return representationsWrappers.Select(r=>
                new LinkedDistance<T>(r, GetCosineDistanceTo(locator(r))))
               .OrderByDescending(s => s.Distance.DistanceValue)
               .Take(maxCount)
               .ToArray();
        }
    }
}
