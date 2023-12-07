using System;
using System.Text.RegularExpressions;

namespace Aoc2023._04
{
    public class Stage2
    {
        Regex lineRegex = new Regex(
            @"
			Card\s+(?<id>\d+)
			\:
			(\s+(?<winner>\d+))+
			\s\|
			(\s+(?<number>\d+))+
			",
            RegexOptions.Compiled | RegexOptions.IgnoreCase | RegexOptions.IgnorePatternWhitespace);

        public int Run()
        {
            var lines = File.ReadAllLines("../../../Data.txt");

            var board = lines.Select(line =>
            {
                var match = lineRegex.Match(line);

                return new Line
                {
                    Id = int.Parse(match.Groups["id"].Value),
                    Winners = match.Groups["winner"].Captures.Select(c => int.Parse(c.Value)).ToArray(),
                    Numbers = match.Groups["number"].Captures.Select(c => int.Parse(c.Value)).ToArray(),
                };
            })
                .ToArray();

            var result = board.Length;
            var copies = board.SelectMany(b => b.Copies).ToArray();

            while (true)
            {
                result += copies.Length;

                copies = copies
                    .Select(c => board[c])
                    .SelectMany(b => b.Copies)
                    .ToArray();

                if (copies.Length == 0)
                {
                    break;
                }
            }

            return result;
        }

        class Line
        {
            public int Id;

            public int[] Winners;

            public int[] Numbers;

            public int[] Copies
            {
                get
                {
                    var numberOfWinners = Winners.Intersect(Numbers).Count();
                    if (numberOfWinners == 0)
                    {
                        return Array.Empty<int>();
                    }

                    return Enumerable.Range(Id, numberOfWinners).ToArray();
                }
            }
        }
    }
}

