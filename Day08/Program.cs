// See https://aka.ms/new-console-template for more information

using System.Diagnostics;

// var input = File.ReadAllLines("08/test.txt");
var input = File.ReadAllLines("08/input.txt");
PartOne();
PartTwo();

void PartOne()
{
    var output = input
        .SelectMany(row =>
            row[(row.IndexOf('|') + 1)..]
                .Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
                .Select(s => s.Length))
        .GroupBy(s => s)
        .ToDictionary(s => s.Key, s => s.Count());

    Console.WriteLine($"1: {output[2]}");
    Console.WriteLine($"4: {output[4]}");
    Console.WriteLine($"7: {output[3]}");
    Console.WriteLine($"8: {output[7]}");
    Console.WriteLine($"Count: {output[2] + output[3] + output[4] + output[7]}");
}

void PartTwo()
{
    var sum = input.Sum(l => new Decoding(l).Output());

    Console.WriteLine(sum);
}


internal class Decoding
{
    public Decoding(string input)
    {
        this.CharSets =
            input[(input.IndexOf('|') + 1)..]
                .Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
                .Select(s => s.ToCharArray().ToHashSet()).ToArray();
        this.Digits = input[..input.IndexOf('|')]
            .Split(' ', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries)
            .Select(s => new Digit(s)).ToArray();
    }

    private Digit[] Digits { get; }

    private HashSet<char>[] CharSets { get; }

    public int Output()
    {
        this.DeduceSingleOccurenceDigits();

        this.DeduceFiveCounts();

        this.DeduceSixCounts();

        Debug.Assert(this.Digits.All(s => s.DeducedNumber != null));

        var chars = this.CharSets.Select(set =>
        {
            var digit = this.Digits.Single(s => s.Input.Count == set.Count && !s.Input.Except(set).Any());

            return digit.DeducedNumber ?? throw new InvalidOperationException("Character not set on digit!");
        }).ToArray();

        return int.Parse(chars);
    }

    private void DeduceSixCounts()
    {
        var sixCounts = this.Digits.Where(s => s.Input.Count == 6).ToList();

        var one = this.GetDigitByValue(1);
        var six = sixCounts.Single(s => one.Input.Intersect(s.Input).Count() == 1);
        six.DeducedNumber = '6';

        sixCounts = sixCounts.Except(new[] {six}).ToList();

        var four = this.GetDigitByValue(4);
        var nine = sixCounts.Single(s => !four.Input.Except(s.Input).Any());
        nine.DeducedNumber = '9';

        var zero = sixCounts.Except(new[] {nine}).Single();
        zero.DeducedNumber = '0';
    }

    private void DeduceFiveCounts()
    {
        var fiveCounts = this.Digits.Where(s => s.Input.Count == 5).ToList();

        var seven = this.GetDigitByValue(7);
        var three = fiveCounts.Single(s => !seven.Input.Except(s.Input).Any());
        three.DeducedNumber = '3';

        var four = this.GetDigitByValue(4);
        fiveCounts = fiveCounts.Except(new[] {three}).ToList();
        var five = fiveCounts.Single(s => s.Input.Intersect(four.Input).Count() == 3);
        five.DeducedNumber = '5';
        var two = fiveCounts.Single(s => s.Input.Intersect(four.Input).Count() == 2);
        two.DeducedNumber = '2';
    }

    private void DeduceSingleOccurenceDigits()
    {
        this.Digits.Single(s => s.Input.Count == 2).DeducedNumber = '1';
        this.Digits.Single(s => s.Input.Count == 3).DeducedNumber = '7';
        this.Digits.Single(s => s.Input.Count == 4).DeducedNumber = '4';
        this.Digits.Single(s => s.Input.Count == 7).DeducedNumber = '8';
    }

    private Digit GetDigitByValue(int value) => this.Digits.Single(s => s.DeducedNumber == (char)(value + '0'));
}

internal class Digit
{
    public Digit(string input) => this.Input = input.ToCharArray().ToHashSet();

    public char? DeducedNumber { get; set; }

    public ISet<char> Input { get; }
}
