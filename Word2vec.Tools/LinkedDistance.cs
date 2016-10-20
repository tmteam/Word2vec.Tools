using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Word2vec.Tools
{
    public class LinkedDistance<TLinked>
    {
        public LinkedDistance(Representation representation, TLinked linkedObject, double distance)
        {
            Representation = representation;
            Distance = distance;
            LinkedObject = linkedObject;
        }
        public readonly Representation Representation;
        public readonly double Distance;
        public TLinked LinkedObject;
    }
}
