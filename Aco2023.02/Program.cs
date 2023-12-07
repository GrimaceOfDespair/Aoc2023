// See https://aka.ms/new-console-template for more information
using Aco2023._02;

Console.WriteLine("Starting...");

var stage1 = new Stage1().Run(red: 12, green: 13, blue: 14);

Console.WriteLine($"Stage 1: {stage1}");

var stage2 = new Stage2().Run();

Console.WriteLine($"Stage 2: {stage2}");

Console.WriteLine("...Stopped");
Console.ReadLine();

