using System;
using System.Text.RegularExpressions;

namespace Aoc2023._03
{
	public class Stage2
	{
        Regex numberRegex = new Regex(
            @"\d+",
            RegexOptions.Compiled | RegexOptions.IgnoreCase | RegexOptions.IgnorePatternWhitespace);

        Regex symbolRegex = new Regex(
            @"\*",
            RegexOptions.Compiled | RegexOptions.IgnoreCase | RegexOptions.IgnorePatternWhitespace);

        public int Run()
		{
            var lines = File.ReadAllLines("../../../Data.txt");

            var board = lines.Select(line => new Line
            {
                Numbers = numberRegex
                    .Matches(line)
                    .Select(x => new Number
                    {
                        Value = int.Parse(x.Value),
                        Index = x.Index,
                        Length = x.Length,
                    })
                    .ToArray(),

                Symbols = symbolRegex
                    .Matches(line)
                    .Select(x => new Symbol
                    {
                        Index = x.Index,
                    })
                    .ToArray(),
            })
                .ToArray();

            int result = 0;

            for (var i = 0; i < board.Length; i++)
            {
                var line = board[i];

                var numbers = new List<Number>();

                if (i > 0)
                {
                    numbers.AddRange(board[i - 1].Numbers);
                }

                numbers.AddRange(line.Numbers);

                if (i < board.Length - 1)
                {
                    numbers.AddRange(board[i + 1].Numbers);
                }

                result += line.Symbols
                    .Select(symbol =>
                        symbol.GetGearRatio(numbers))
                    .Sum();
            }

            return result;
        }

        class Line
        {
            public Number[] Numbers = Array.Empty<Number>();

            public Symbol[] Symbols = Array.Empty<Symbol>();
        }

        class Number
        {
            public int Index;

            public int Value;

            public int Length;

            public bool IsAdjacentToSymbol(Symbol symbol)
            {
                foreach (var position in Enumerable.Range(Index, Length))
                {
                    if (position <= symbol.Index + 1 &&
                        position >= symbol.Index - 1)
                    {
                        return true;
                    }
                }

                return false;
            }
        }

        class Symbol
        {
            public int Index;

            public int GetGearRatio(List<Number> numbers)
            {
                var gearNumbers = numbers
                    .Where(number =>
                        number.IsAdjacentToSymbol(this))
                    .ToArray();

                if (gearNumbers.Length == 2)
                {
                    return gearNumbers[0].Value * gearNumbers[1].Value;
                }

                return 0;
            }
        }
    }
}

