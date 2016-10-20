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
    }
}
