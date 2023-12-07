namespace Aoc2023._06
{
    public class Stage1
    {
        public long Run()
        {
            var lines = File.ReadAllLines("../../../Data.txt");

            var times = lines[0].Split(":")[1]
                .Split(" ", StringSplitOptions.RemoveEmptyEntries)
                .Select(int.Parse);
            var distances = lines[1].Split(":")[1]
                .Split(" ", StringSplitOptions.RemoveEmptyEntries)
                .Select(int.Parse);

            var games = times.Zip(distances).Select(zip =>
            (
                Time: zip.First,
                Distance: zip.Second
            ))
                .ToArray();

            return games.Select(game =>
            {
                var wins = 0;
                var time = 0;
                var lastDistance = 0;

                while (true)
                {
                    time += 1;

                    var timeLeft = game.Time - time;
                    var distance = timeLeft * time;

                    var winGame = distance > game.Distance;
                    if (winGame)
                    {
                        wins++;
                    }
                    else if (distance < lastDistance)
                    {
                        break;
                    }

                    lastDistance = distance;
                }

                return wins;
            })
                .Aggregate(1, (x, y) => x * y);
        }
    }
}