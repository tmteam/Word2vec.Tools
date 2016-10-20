using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Word2vec.Tools
{
    public class DistanceTo
    {
        public DistanceTo(Representation representation, double distanceValue)
        {
            _representation = representation;
            _distanceValue = distanceValue;
        }
        private readonly Representation _representation;
        private readonly double _distanceValue;

        public Representation Representation
        {
            get
            {
                return _representation;
            }
        }

        public double DistanceValue
        {
            get
            {
                return _distanceValue;
            }
        }
    }
}
