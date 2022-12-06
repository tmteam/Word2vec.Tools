namespace Word2vec.Tools;

public class DistanceTo
{
    public DistanceTo(Representation representation, double distanceValue)
    {
        _representation = representation;
        _distanceValue = distanceValue;
    }
    private readonly Representation _representation;
    private readonly double _distanceValue;

    public Representation Representation => _representation;

    public double DistanceValue => _distanceValue;
}