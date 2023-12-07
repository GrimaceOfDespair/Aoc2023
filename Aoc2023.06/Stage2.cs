namespace Aoc2023._06
{
    public class Stage2
    {
        public long Run()
        {
            var lines = File.ReadAllLines("../../../Data.txt");

            var gameTime = long.Parse(string.Join("", lines[0].Split(":")[1]
                .Split(" ", StringSplitOptions.RemoveEmptyEntries)));
            var gameDistance = long.Parse(string.Join("", lines[1].Split(":")[1]
                .Split(" ", StringSplitOptions.RemoveEmptyEntries)));

            var wins = 0L;
            var time = 0L;
            var lastDistance = 0L;

            while (true)
            {
                time += 1;

                var timeLeft = gameTime - time;
                var distance = timeLeft * time;

                var winGame = distance > gameDistance;
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
        }
    }
}