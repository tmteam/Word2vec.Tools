
namespace Word2vec.Tools
{
    public class WordDistance
    {
        public WordDistance(WordRepresentation representation, double distance)
        {
            Representation = representation;
            Distance = distance;
        }
        public  readonly WordRepresentation Representation;
        public readonly double Distance;
    }
}
