// See https://aka.ms/new-console-template for more information


using System.Diagnostics;

// var input = File.ReadAllLines("15/test.txt");
var input = File.ReadAllLines("15/input.txt");
// var input = @"1163
// // 1381
// // 2136
// // 3694".Split(Environment.NewLine, StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);

var width = input.Length - 1;
var end = new Coord(width, width);
var board = new Dictionary<Coord, int>();

for (var rowIndex = 0; rowIndex < input.Length; rowIndex++)
{
    Debug.Assert(input.Length == input[rowIndex].Length, "Should have square");
    for (var colIndex = 0; colIndex < input[rowIndex].Length; colIndex++)
    {
       board.Add(new Coord(rowIndex, colIndex), int.Parse(input[rowIndex][colIndex].ToString()));
    }
}

long currentMin = width * 2 * 10;
var start = new Coord(0, 0);
var minRisk = Neighbors(start)
        .Where(n => board.ContainsKey(n))
        .Min(n => MinRisk(n, 0, new HashSet<Coord>{ start}));

Console.WriteLine("End");
Console.WriteLine(minRisk);

long MinRisk(Coord currentCoord, long currentRisk, ISet<Coord> visitedCoords)
{
    visitedCoords.Add(currentCoord);
    currentRisk += board[currentCoord];

    if (currentCoord == end)
    {
        currentMin = Math.Min(currentRisk, currentMin);
        return currentRisk;
    }

    if (currentRisk >= currentMin)
    {
        return long.MaxValue;
    }

    var neighborsToScan = Neighbors(currentCoord)
        .Where(n => !visitedCoords.Contains(n))
        .Where(n => board.ContainsKey(n))
        .ToList();

    if (!neighborsToScan.Any())
    {
        return long.MaxValue;
    }

    return neighborsToScan.Min(n => MinRisk(n, currentRisk, new HashSet<Coord>(visitedCoords)));
}

IEnumerable<Coord> Neighbors(Coord currentCoord)
{
    yield return currentCoord with {Col = currentCoord.Col + 1};
    yield return currentCoord with {Col = currentCoord.Col - 1};
    yield return currentCoord with {Row = currentCoord.Row - 1};
    yield return currentCoord with {Row = currentCoord.Row + 1};
}

record Coord(int Row, int Col);
