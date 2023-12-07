using System;
using System.Text.RegularExpressions;

namespace Aoc2023._03
{
	public class Stage1
	{
        Regex numberRegex = new Regex(
            @"\d+",
            RegexOptions.Compiled | RegexOptions.IgnoreCase | RegexOptions.IgnorePatternWhitespace);

        Regex symbolRegex = new Regex(
            @"[^.0-9]",
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

                var symbols = new List<Symbol>();

                if (i > 0)
                {
                    symbols.AddRange(board[i - 1].Symbols);
                }

                symbols.AddRange(line.Symbols);

                if (i < board.Length - 1)
                {
                    symbols.AddRange(board[i + 1].Symbols);
                }

                var numbers = line.Numbers
                    .Where(number =>
                        number.IsAdjacentToSymbol(symbols))
                    .ToArray();

                result += numbers.Select(number => number.Value).Sum();
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

            public bool IsAdjacentToSymbol(List<Symbol> symbols)
            {
                foreach (var position in Enumerable.Range(Index, Length))
                {
                    if (symbols.Any(s =>
                        position <= s.Index + 1 &&
                        position >= s.Index - 1))
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
        }
    }
}

