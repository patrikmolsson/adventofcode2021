// See https://aka.ms/new-console-template for more information

using System.Globalization;
using System.Text;

var input = File.ReadAllLines("13/input.txt");
// var input = File.ReadAllLines("13/test.txt");

var board = new Board(input.TakeWhile(s => !string.IsNullOrWhiteSpace(s)).ToArray());

var foldingInstructions = input.Where(s => s.Contains("fold"));

var printed = false;
foreach (var foldingInstruction in foldingInstructions)
{
    var spl = foldingInstruction.Split('=');
    if (foldingInstruction.Contains('x'))
    {
        board.FoldAlongX(int.Parse(spl[1], CultureInfo.InvariantCulture));
    }
    else if (foldingInstruction.Contains('y'))
    {
        board.FoldAlongY(int.Parse(spl[1], CultureInfo.InvariantCulture));
    }
    else
    {
        throw new InvalidOperationException();
    }

    if (!printed)
    {
        Console.WriteLine($"PART ONE: {board.Dots.Count}");
        printed = true;
    }
}

Console.WriteLine($"PART TWO: ");

foreach (var line in board.PrintLines())
{
    Console.WriteLine(line);
}


internal class Board
{
    public ISet<Coordinate> Dots { get; }

    public Board(IReadOnlyList<string> input)
    {
        var dots = new HashSet<Coordinate>();

        foreach (var line in input)
        {
            var spl = line.Split(',');

            dots.Add(new Coordinate(
                int.Parse(spl[0], CultureInfo.InvariantCulture),
                int.Parse(spl[1], CultureInfo.InvariantCulture)
            ));
        }

        for (var y = 0; y < input.Count; y++)
        {
            var line = input[y];
            for (var x = 0; x < line.Length; x++)
            {
                var character = line[x];

                if (character == '#')
                {
                    dots.Add(new Coordinate(x, y));
                }
            }
        }

        this.Dots = dots;
    }

    public void FoldAlongX(int x)
    {
        var setToFlip = this.Dots.Where(s => s.X > x).ToList();

        this.FoldAlong(setToFlip, c => c.FlipAlongX(x));
    }

    public void FoldAlongY(int y)
    {
        var setToFlip = this.Dots.Where(s => s.Y > y).ToList();

        this.FoldAlong(setToFlip, c => c.FlipAlongY(y));
    }

    private void FoldAlong(IReadOnlyCollection<Coordinate> coordinates, Func<Coordinate, Coordinate> flipper)
    {
        foreach (var coordinate in coordinates)
        {
            this.Dots.Remove(coordinate);
            var flipped = flipper(coordinate);
            this.Dots.Add(flipped);
        }
    }

    public IEnumerable<string> PrintLines()
    {
        var height = this.Dots.Max(s => s.Y);
        var width = this.Dots.Max(s => s.X);
        for (var y = 0; y <= height; y++)
        {
            var sb = new StringBuilder();
            for (var x = 0; x <= width; x++)
            {
                sb.Append(this.Dots.Contains(new Coordinate(x, y)) ? "#" : ".");
            }

            yield return sb.ToString();
        }
    }
}

internal record Coordinate(int X, int Y)
{
    public Coordinate FlipAlongX(int x) => this with {X = Flip(x, this.X)};
    public Coordinate FlipAlongY(int y) => this with {Y = Flip(y, this.Y)};

    private static int Flip(int newCoord, int oldCoord) => newCoord - (oldCoord - newCoord);
};
