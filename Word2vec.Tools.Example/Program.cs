using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Word2vec.Tools;

namespace Word2vec.Tools.Example
{
    class Program
    {
        static void Main(string[] args)
        {
            string boy   = "boy";
            string girl  = "girl";
            string woman = "woman";

            //Set an w2v bin file path there:
            string path = @"C:\Code\Corpus\DefaultGoogleVectors.bin";
            var vocabulary = new Word2VecBinaryReader().Read(path);

            //For w2v text sampling file use:
            //var vectors = new Word2VecTextReader().Read(path);

            Console.WriteLine("vectors file: " + path);
            Console.WriteLine("vocabulary size: " + vocabulary.Words.Length);
            Console.WriteLine("w2v vector dimensions count: " + vocabulary.VectorDimensionsCount);

            Console.WriteLine();

            int count = 7;
            
            #region distance
            
            Console.WriteLine("top "+count+" closest to \""+ boy+"\" words:");
            var closest = vocabulary.Distance(boy, count);
            foreach (var neightboor in closest)
                Console.WriteLine(neightboor.Representation.Word + "\t\t" + neightboor.Distance);
            #endregion

            Console.WriteLine();

            #region analogy
            Console.WriteLine("\""+girl+"\" relates to \""+boy+"\" as \""+woman+"\" relates to ..."); 
            var analogies = vocabulary.Analogy("girl", "boy", "woman", count);
            foreach (var neightboor in analogies)
                Console.WriteLine(neightboor.Representation.Word + "\t\t" + neightboor.Distance);
            #endregion

            Console.WriteLine();

            #region addition
            Console.WriteLine("\""+girl+"\" + \""+boy+"\" = ...");
            var additionRepresentation = vocabulary[girl].Add(vocabulary[boy]);
            var closestAdditions = vocabulary.Distance(additionRepresentation, count);
             foreach (var neightboor in closestAdditions)
                 Console.WriteLine(neightboor.Representation.Word + "\t\t" + neightboor.Distance);
            #endregion

            Console.WriteLine();

            #region subtraction
             Console.WriteLine("\"girl\" - \"boy\" = ...");
             var subtractionRepresentation = vocabulary["girl"].Add(vocabulary["boy"]);
             var closestSubtractions = vocabulary.Distance(subtractionRepresentation, count);
             foreach (var neightboor in closestSubtractions)
                 Console.WriteLine(neightboor.Representation.Word + "\t\t" + neightboor.Distance);
            #endregion

            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
        }
    }
}
