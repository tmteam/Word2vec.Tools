using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Word2vec.Tools
{
    public static class TheHelper
    {
        /// <summary>
        /// returns summ of target sequence elements 
        /// throws ArgumentException if sequence is empty
        /// </summary>
        public static Representation Summ(this IEnumerable<Representation> representations)
        {
            var ans = representations.SummOrNull();
            if(ans==null)
                throw new ArgumentException("Sequence \"representations\" does not contain elements");
            return ans;
        }
        /// <summary>
        /// returns summ of target sequence elements 
        /// returns null if sequence is empty
        /// </summary>
        public static Representation SummOrNull(this IEnumerable<Representation> representations)
        {
            Representation ans = null;
            foreach (var representation in representations)
                ans = ans == null ? representation : ans.Add(representation);
            return ans;
        }

        /// <summary>
        /// returns most outstanding word in the phrase using projection
        /// </summary>
        public static LinkedDistance<T> GetMostOutstanding<T>(this IEnumerable<T> representationsWrappers, Func<T, Representation> locator )
        {
            var summ = representationsWrappers.Select(locator).Summ();

            return representationsWrappers
                .Select(r => new LinkedDistance<T>(r, summ.GetCosineDistanceTo(locator(r))))
                .OrderByDescending(r => r.Distance.DistanceValue)
                .First();
        }

        /// <summary>
        /// returns most outstanding word in the phrase 
        /// </summary>
        public static DistanceTo GetMostOutstanding(this IEnumerable<Representation> representations)
        {
            var summ = representations.Summ();
            return representations.Select(r => summ.GetCosineDistanceTo(r)).OrderByDescending(r => r.DistanceValue).First();
        }

        /// <summary>
        /// returns several most outstanding words in the phrase 
        /// </summary>
        public static DistanceTo[] GetMostOutstandings(this IEnumerable<Representation> representations, int maxCount)
        {
            var summ = representations.Summ();
            return representations.Select(r => summ.GetCosineDistanceTo(r)).OrderByDescending(r => r.DistanceValue).Take(maxCount).ToArray();
        }
    }
}
