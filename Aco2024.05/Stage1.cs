using System;
using System.IO;
using System.Text.RegularExpressions;

namespace Aoc2023._05
{
    public class Stage1
    {
        public long Run()
        {
            var lines = File.ReadAllLines("../../../Data.txt");

            var seeds = lines[0]
                .Split(":")[1]
                .Split(" ", StringSplitOptions.RemoveEmptyEntries)
                .Select(long.Parse)
                .ToArray();

            var maps = new List<List<SeedRange>>();
            
            foreach (var line in lines.Skip(1))
            {
                if (string.IsNullOrWhiteSpace(line))
                {
                    continue;
                }

                if (line.EndsWith(":"))
                {
                    maps.Add(new List<SeedRange>());
                    continue;
                }

                var seedData = line.Split(" ").Select(long.Parse).ToArray();

                maps.Last().Add(new SeedRange
                {
                    Destination = seedData[0],
                    Source = seedData[1],
                    NumberOfSeeds = seedData[2],
                });
            }

            foreach (var map in maps)
            {
                for (var i = 0; i < seeds.Length; i++)
                {
                    var seed = seeds[i];

                    seeds[i] = map
                        .Select(x => x.Map(seed))
                        .FirstOrDefault(x => x != seed, seed);
                }
            }

            return seeds.Min();
        }

        class SeedRange
        {
            public long Source;

            public long Destination;

            public long NumberOfSeeds;

            public long Map(long seed)
            {
                if (seed >= Source && seed < Source + NumberOfSeeds)
                {
                    return Destination + seed - Source;
                }

                return seed;
            }
        }
    }
}