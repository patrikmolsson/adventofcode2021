// See https://aka.ms/new-console-template for more information

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

PartOne();
PartTwo();

void PartOne()
{
    CountOverlapping(false);
}

void PartTwo()
{
    CountOverlapping(true);
}

void CountOverlapping(bool includeDiagonals)
{
    var input = File.ReadAllLines("05/input.txt");

    var lines = input.Select(i => new Line(i, includeDiagonals)).ToList();

    var coordsCount = new Dictionary<Coord, int>();

    foreach (var coord in lines.SelectMany(line => line.CoordsCovered))
    {
        coordsCount.TryAdd(coord, 0);

        coordsCount[coord] += 1;
    }

    var countMultiple = coordsCount.Count(s => s.Value > 1);

    Console.WriteLine($"Count multi: {countMultiple}");
}

class Line
{
    private bool IncludeDiagonals { get; }
    private Coord Start { get; }
    private Coord End { get; }

    public Line(string input, bool includeDiagonals)
    {
        IncludeDiagonals = includeDiagonals;
        var matches = Regex.Matches(input, @"\d+");
        Start = new Coord(CoordAt(0), CoordAt(1));
        End = new Coord(CoordAt(2), CoordAt(3));

        int CoordAt(int index)
        {
            return int.Parse(matches[index].Value);
        }
    }

    public IEnumerable<Coord> CoordsCovered
    {
        get
        {
            if (Start.X == End.X)
            {
                return CreateRange(Start.Y, End.Y)
                    .Select(y => new Coord(Start.X, y));
            }

            if (Start.Y == End.Y)
            {
                return CreateRange(Start.X, End.X)
                    .Select(x => new Coord(x, Start.Y));
            }

            if (IncludeDiagonals)
            {
                return DiagonalCoords();
            }

            return Enumerable.Empty<Coord>();
        }
    }

    private IEnumerable<Coord> DiagonalCoords()
    {
        var xCoords = CreateRange(Start.X, End.X).ToArray();
        var yCoords = CreateRange(Start.Y, End.Y).ToArray();

        for (var index = 0; index < xCoords.Length; index++)
        {
            var xCoord = xCoords[index];
            var yCoord = yCoords[index];

            yield return new Coord(xCoord, yCoord);
        }
    }

    private static IEnumerable<int> CreateRange(int start, int end)
    {
        while (start != end)
        {
            yield return start;

            start += end > start ? 1 : -1;
        }

        yield return end;
    }
}

internal record Coord(int X, int Y);