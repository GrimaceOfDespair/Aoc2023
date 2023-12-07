using System;
using System.Text.RegularExpressions;

namespace Aco2023._02
{
    public class Stage2
    {
        Regex gameRegex = new Regex(
            @"^
				Game\s(?<id>\d+)
				\:
				((?<reach>([^;]+))(;|$))+
			",
            RegexOptions.Compiled | RegexOptions.IgnoreCase | RegexOptions.IgnorePatternWhitespace);

        Regex cubeRegex = new Regex(
            @"
			    (?<number>\d+)\s
				(?<color>red|green|blue)
			",
            RegexOptions.Compiled | RegexOptions.IgnoreCase | RegexOptions.IgnorePatternWhitespace);

        public int Run()
        {
            var result = 0;

            var lines = File.ReadAllLines("../../../Data.txt");

            foreach (var line in lines)
            {
                var match = gameRegex.Match(line);
                var gameId = int.Parse(match.Groups["id"].Value);

                var game = new Game
                {
                    Id = gameId,
                    Reaches = match.Groups["reach"]
                        .Captures
                        .Select(capture =>
                            ParseReach(capture.Value.Split(",")))
                        .ToArray()
                };

                var gamePower = game.GetPower();
                result += gamePower;

                Console.WriteLine($"Game {game.Id}: {gamePower}");
            }

            return result;
        }

        private Reach ParseReach(string[] cubes)
        {
            var reach = new Reach();

            foreach (var cube in cubes)
            {
                var match = cubeRegex.Match(cube);

                var number = int.Parse(match.Groups["number"].Value);
                var color = match.Groups["color"].Value;

                switch (color)
                {
                    case "red":
                        reach.Red = number;
                        break;

                    case "green":
                        reach.Green = number;
                        break;

                    case "blue":
                        reach.Blue = number;
                        break;
                }
            }

            return reach;
        }

        public class Game
        {
            public int Id;

            public Reach[] Reaches = Array.Empty<Reach>();

            public int GetPower()
            {
                return
                    Reaches.Max(x => x.Red) *
                    Reaches.Max(x => x.Green) *
                    Reaches.Max(x => x.Blue);
            }
        }

        public class Reach
        {
            public int Red;

            public int Green;

            public int Blue;
        }
    }
}

