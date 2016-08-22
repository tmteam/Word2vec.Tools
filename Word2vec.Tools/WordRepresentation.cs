
namespace Word2vec.Tools
{
    /// <summary>
    /// Word and its w2v meaning vector
    /// </summary>
    public class WordRepresentation : Representation
    {
        public WordRepresentation(string word, float[] vector): base(vector)
        {
            this.Word = word;
        }
        public readonly string Word;
       
    }
}
