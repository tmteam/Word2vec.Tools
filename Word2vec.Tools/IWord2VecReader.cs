using System;
namespace Word2vec.Tools
{
    /// <summary>
    /// word2vec sampling file reader
    /// </summary>
    public interface IWord2VecReader
    {
        Vocabulary Read(System.IO.Stream inputStream);
        Vocabulary Read(string path);
    }
}
