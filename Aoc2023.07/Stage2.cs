namespace Aoc2023._07
{
    public class Stage2
    {
        public long Run()
        {
            var lines = File.ReadAllLines("../../../Data.txt");

            var cardScores = "AKQT98765432J"
                .Reverse()
                .Select((c, i) => (Char: c, Score: i))
                .ToDictionary(
                    c => c.Char,
                    c => c.Score);

            var handScores = lines.Select(line =>
            {
                var data = line.Split(" ", StringSplitOptions.RemoveEmptyEntries);

                var hand = data[0];
                var bid = int.Parse(data[1]);

                var lookup = hand.ToLookup(
                    card => card,
                    card => cardScores[card]);

                var handScores = hand
                    .Select(card =>
                        cardScores[card])
                    .ToArray(); ;

                int score = 0, subscore = 0;
                string type = "";
                var jokers = hand.Count(x =>
                    x == 'J');

                switch (lookup.Count)
                {
                    case 1:
                        type = "Five of a kind";
                        score = 5;
                        break;

                    case 2:
                        if (lookup.Any(x => x.Count() == 4))
                        {
                            switch (jokers)
                            {
                                case 0:
                                    type = "Four of a kind";
                                    score = 4;
                                    break;

                                case 1:
                                case 4:
                                    type = "Five of a kind";
                                    score = 5;
                                    break;
                            }
                        }
                        else
                        {
                            switch (jokers)
                            {
                                case 0:
                                    type = "Full house";
                                    score = 3;
                                    subscore = 1;
                                    break;

                                case 2:
                                case 3:
                                    type = "Five of a kind";
                                    score = 5;
                                    break;
                            }
                        }
                        break;

                    case 3:
                        if (lookup.Any(x => x.Count() == 3))
                        {
                            switch (jokers)
                            {
                                case 0:
                                    type = "Three of a kind";
                                    score = 3;
                                    break;

                                case 3:
                                case 1:
                                    type = "Four of a kind";
                                    score = 4;
                                    break;
                            }
                        }
                        else
                        {
                            switch (jokers)
                            {
                                case 0:
                                    type = "Two pair";
                                    score = 2;
                                    break;

                                case 1:
                                    type = "Full house";
                                    score = 3;
                                    subscore = 1;
                                    break;

                                case 2:
                                    type = "Four of a kind";
                                    score = 4;
                                    break;
                            }
                        }
                        break;

                    case 4:
                        switch (jokers)
                        {
                            case 0:
                                type = "One pair";
                                score = 1;
                                break;

                            case 1:
                            case 2:
                                type = "Three of a kind";
                                score = 3;
                                break;
                        }
                        break;

                    default:
                        switch (jokers)
                        {
                            case 0:
                                type = "High card";
                                score = 0;
                                break;

                            case 1:
                                type = "One pair";
                                score = 1;
                                break;
                        }
                        break;
                }

                return (hand, type, score, subscore, handScores, bid);
            });

            var sortedHands = handScores
                .OrderBy(x =>
                    (
                        x.score,
                        x.subscore,
                        x.handScores[0],
                        x.handScores[1],
                        x.handScores[2],
                        x.handScores[3],
                        x.handScores[4]
                    ))
                .ToArray();

            return sortedHands
                .Select((x, i) =>
                    (i + 1) * x.bid)
                .Sum();
        }
    }
}