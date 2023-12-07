using System;
using System.Text.RegularExpressions;

namespace Aco2023._02
{
	public class Stage1
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

        public int Run(int red, int green, int blue)
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

				var gameIsMatch = game.IsMatch(red, green, blue);
                if (gameIsMatch)
				{
					result += game.Id;
				}

				Console.WriteLine($"Game {game.Id}: {gameIsMatch}");
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

            public bool IsMatch(int red, int green, int blue)
            {
                return Reaches.All(reach =>
                    reach.IsMatch(red, green, blue));
            }
        }

        public class Reach
        {
            public int Red;

            public int Green;

            public int Blue;

            public bool IsMatch(int red, int green, int blue)
            {
                return
                    red >= Red &&
                    green >= Green &&
                    blue >= Blue;
            }
        }
    }
}

