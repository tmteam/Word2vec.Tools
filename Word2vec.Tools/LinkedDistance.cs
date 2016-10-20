using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Word2vec.Tools
{
    public class LinkedDistance<TLinked>
    {
        public LinkedDistance(TLinked linkedObject, DistanceTo distance)
        {
            _distance = distance;
            _linkedObject = linkedObject;
        } 
        public LinkedDistance(Representation representation, TLinked linkedObject, double distance)
        {
            _distance = new DistanceTo(representation, distance);
            _linkedObject = linkedObject;
        }
        private readonly DistanceTo _distance;
        private readonly TLinked _linkedObject;

        public DistanceTo Distance
        {
            get
            {
                return _distance;
            }
        }

        public TLinked LinkedObject
        {
            get
            {
                return _linkedObject;
            }
        }
    }
}
