using System;
using System.Text.RegularExpressions;

namespace Aoc2023._04
{
	public class Stage1
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

			return board.Sum(b => b.Score);
        }

		class Line
		{
			public int Id;

            public int[] Winners;

			public int[] Numbers;

			public int Score
			{
				get
				{
					var numberOfWinners = Winners.Intersect(Numbers).Count();

                    return numberOfWinners == 0
						? 0
						: (int)Math.Pow(2, numberOfWinners - 1);
                }
            }
		}
    }
}

