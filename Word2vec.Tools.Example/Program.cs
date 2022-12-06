﻿using Word2vec.Tools;

string boy = "boy";
string girl = "girl";
string woman = "woman";

Console.WriteLine("Reading the model...");

//Set your w2v bin file path here:
var path = @"C:\Vectors.bin";
var vocabulary = new Word2VecBinaryReader().Read(path);

//For w2v text sampling file use:
//var vocabulary = new Word2VecTextReader().Read(path);

Console.WriteLine($"vectors file: {path}");
Console.WriteLine($"vocabulary size: {vocabulary.Words.Length}");
Console.WriteLine($"w2v vector dimensions count: {vocabulary.VectorDimensionsCount}");

Console.WriteLine();

int count = 7;

#region distance

Console.WriteLine($"top {count} closest to '{boy}' words:");
var closest = vocabulary.Distance(boy, count);

// Is simmilar to:
// var closest = vocabulary[boy].GetClosestFrom(vocabulary.Words.Where(w => w != vocabulary[boy]), count);

foreach (var neightboor in closest)
    Console.WriteLine($"{neightboor.Representation.WordOrNull}\t\t{neightboor.DistanceValue}");

#endregion


#region analogy
Console.WriteLine();

Console.WriteLine($"'{girl}' relates to '{boy}' as '{woman}' relates to ...");
var analogies = vocabulary.Analogy(girl, boy, woman, count);
foreach (var neighbour in analogies)
    Console.WriteLine($"{neighbour.Representation.WordOrNull}\t\t{neighbour.DistanceValue}");

#endregion


#region addition

Console.WriteLine();
Console.WriteLine($"'{girl}' + '{boy}' = ...");
var additionRepresentation = vocabulary[girl].Add(vocabulary[boy]);
var closestAdditions = vocabulary.Distance(additionRepresentation, count);
foreach (var neightboor in closestAdditions)
    Console.WriteLine($"{neightboor.Representation.WordOrNull}\t\t{neightboor.DistanceValue}");

#endregion


#region subtraction
Console.WriteLine();

Console.WriteLine($"'{girl}' - '{boy}' = ...");
var subtractionRepresentation = vocabulary[girl].Substract(vocabulary[boy]);
var closestSubtractions = vocabulary.Distance(subtractionRepresentation, count);
foreach (var neightboor in closestSubtractions)
    Console.WriteLine($"{neightboor.Representation.WordOrNull}\t\t{neightboor.DistanceValue}");

#endregion

Console.WriteLine("Press any key to continue...");
Console.ReadKey();