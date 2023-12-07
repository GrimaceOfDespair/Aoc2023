// See https://aka.ms/new-console-template for more information
using System.Text.RegularExpressions;

var lines = File.ReadLines("../../../Data.txt");

//////////////////////////////////////////
// Solution 1
//////////////////////////////////////////
var regex = new Regex(@"
    ^
    [^0-9]*
    (?<d1>[0-9]).*?
    (?<d2>[0-9])?
    [^0-9]*
    $
", RegexOptions.IgnorePatternWhitespace | RegexOptions.IgnoreCase);

var n = 0;

foreach (var line in lines)
{
    var m = regex.Match(line);
    var d1 = m.Groups["d1"];
    var d2 = m.Groups["d2"];

    var x1 = d1.Success ? int.Parse(d1.Value) : 0;
    var x2 = d2.Success ? int.Parse(d2.Value) : x1;

    var plus = x1 * 10 + x2;

    n += plus;
}

Console.WriteLine($"Solution: {n}");

//////////////////////////////////////////
// Solution 2
//////////////////////////////////////////
var numbers = new[] { "one", "two", "three", "four", "five", "six", "seven", "eight", "nine" };

var digits = Enumerable.Range(1, 9)
    .Select(x =>
    (
        StringValue: $"{x}",
        IntValue: x
    ))
    .ToArray();

var forwardTokens = numbers
    .Select((value, index) =>
        (
            StringValue: value,
            IntValue: index + 1)
        )
    .Concat(digits);

var forwardRegex = new Regex(
    string.Join("|", forwardTokens.Select(t => t.StringValue)));

var forwardDictionary = forwardTokens
    .ToDictionary(
        value => value.StringValue,
        value => value.IntValue);

var reverseNumbers = numbers
    .Select(n =>
        new string(n.Reverse().ToArray()))
    .ToArray();

var reverseTokens = reverseNumbers
    .Select((value, index) =>
        (
            StringValue: value,
            IntValue: index + 1)
        )
    .Concat(digits);

var reverseRegex = new Regex(
    string.Join("|", reverseTokens.Select(t => t.StringValue)));

var reverseDictionary = reverseTokens
    .ToDictionary(
        value => value.StringValue,
        value => value.IntValue);

var n2 = 0;

foreach (var line in lines)
{
    var m1 = forwardRegex.Match(line);
    var m2 = reverseRegex.Match(new string(line.Reverse().ToArray()));

    if (m1.Success && m2.Success)
    {
        n2 += forwardDictionary[m1.Value] * 10 + reverseDictionary[m2.Value];
    }
}

Console.WriteLine($"Solution: {n2}");

Console.ReadLine();

