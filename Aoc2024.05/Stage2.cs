using System;
using System.Diagnostics;
using System.IO;
using System.Text.RegularExpressions;

namespace Aoc2023._05
{
    public class Stage2
    {
        public long Run()
        {
            var lines = File.ReadAllLines("../../../Data.txt");

            var seedRanges = lines[0]
                .Split(":")[1]
                .Split(" ", StringSplitOptions.RemoveEmptyEntries)
                .Select(long.Parse)
                .Chunk(2)
                .Select(x => new Seeds
                {
                    Start = x[0],
                    NumberOfSeeds = x[1]
                })
                .ToArray();

            var mapsList = new List<List<SeedMap>>();

            foreach (var line in lines.Skip(1))
            {
                if (string.IsNullOrWhiteSpace(line))
                {
                    continue;
                }

                if (line.EndsWith(":"))
                {
                    mapsList.Add(new List<SeedMap>());
                    continue;
                }

                var seedData = line.Split(" ").Select(long.Parse).ToArray();

                mapsList.Last().Add(new SeedMap
                {
                    Destination = seedData[0],
                    Start = seedData[1],
                    NumberOfSeeds = seedData[2],
                });
            }

            foreach (var maps in mapsList)
            {
                var orderedMaps = maps
                    .OrderBy(m => m.Start)
                    .ToArray();

                seedRanges = seedRanges

                    .SelectMany(seeds =>
                        orderedMaps.Aggregate(
                            new[] { (Seeds: seeds, Moved: false) },
                            (aggregate, map) =>
                                aggregate
                                    .SkipLast(1)
                                    .Concat(
                                        map.Map(aggregate.Last()))
                                    .ToArray()))

                    .Select(x =>
                        x.Seeds)
                    .ToArray();
            }

            return seedRanges.Select(x => x.Start).Min();
        }

        [DebuggerDisplay("[{Start}-{End}] => [{Destination}-{Destination + NumberOfSeeds}] ({NumberOfSeeds})")]
        class SeedMap
        {
            public long Start;

            public long NumberOfSeeds;

            public long Destination;

            public long End
            {
                get
                {
                    return Start + NumberOfSeeds;
                }
            }

            public IEnumerable<(Seeds Seeds, bool Moved)> Map((Seeds Seeds, bool Moved) seedData)
            {
                if (seedData.Moved)
                {
                    yield return seedData;
                    yield break;
                }

                var seed = seedData.Seeds;
                var intersect = seed.Intersect(Start, End);

                if (intersect == null)
                {
                    yield return (seed, false);
                }
                else
                {
                    var (intersectStart, intersectEnd) = intersect.Value;

                    if (seed.Start < intersectStart)
                    {
                        var left = new Seeds
                        {
                            Start = seed.Start,
                            NumberOfSeeds = intersectStart - seed.Start,
                        };

                        yield return (left, false);
                    }

                    var offset = Destination - Start;

                    var intersectSeed = new Seeds
                    {
                        Start = intersectStart + offset,
                        NumberOfSeeds = intersectEnd - intersectStart,
                    };

                    yield return (intersectSeed, true);

                    if (seed.End > intersectEnd)
                    {
                        var right = new Seeds
                        {
                            Start = intersectEnd,
                            NumberOfSeeds = seed.End - intersectEnd,
                        };

                        yield return (right, false);
                    }
                }
            }
        }

        [DebuggerDisplay("[{Start}-{End}] ({NumberOfSeeds})")]
        class Seeds
        {
            public long Start;

            public long NumberOfSeeds;

            public long End
            {
                get
                {
                    return Start + NumberOfSeeds;
                }
            }

            public (long Start, long End)? Intersect(long start, long end)
            {
                var greatestStart = Math.Max(Start, start);
                var smallestEnd = Math.Min(End, end);

                //no intersection
                if (greatestStart >= smallestEnd)
                {
                    return null;
                }

                return (greatestStart, smallestEnd);
            }
        }
    }
}