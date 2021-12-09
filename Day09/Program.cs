// var input = File.ReadAllLines("09/test.txt");

using System.Globalization;

var input = File.ReadAllLines("09/input.txt");

var matrix = new int[input.Length][];

for (var row = 0; row < input.Length; row++)
{
    matrix[row] = input[row].ToCharArray().Select(s => int.Parse(s.ToString(), CultureInfo.InvariantCulture)).ToArray();
}

PartOne();
PartTwo();

void PartOne()
{
    var lowPoints = GetLowPoints(matrix).Select(s => s.value).Select(s => s + 1).Sum();

    Console.WriteLine(lowPoints);
}

void PartTwo()
{
    var lowPoints = GetLowPoints(matrix)
        .Select(l => GetBasinSize(l.coordinate, matrix))
        .ToList();

    var topThree = lowPoints.OrderByDescending(s => s).Take(3).ToArray();

    var product = topThree[0] * topThree[1] * topThree[2];

    Console.WriteLine(product);
}

static int GetBasinSize((int row, int col) lowPoint, int[][] board)
{
    var searchedCoordinates = new HashSet<(int row, int col)>();
    var coordinatesToSearch = new Queue<(int row, int col)>();

    coordinatesToSearch.Enqueue(lowPoint);

    while (coordinatesToSearch.TryDequeue(out var search))
    {
        searchedCoordinates.Add(search);

        AddIfApplicable((search.row + 1, search.col));
        AddIfApplicable((search.row - 1, search.col));
        AddIfApplicable((search.row, search.col + 1));
        AddIfApplicable((search.row, search.col - 1));
    }

    return searchedCoordinates.Count;

    void AddIfApplicable((int row, int col) candidate)
    {
        if (searchedCoordinates.Contains(candidate))
        {
            return;
        }

        if (candidate.row < 0 || candidate.col < 0)
        {
            return;
        }

        if (candidate.row >= board.Length || candidate.col >= board[candidate.row].Length)
        {
            return;
        }

        if (board[candidate.row][candidate.col] == 9)
        {
            return;
        }

        coordinatesToSearch.Enqueue(candidate);
    }
}

static IEnumerable<(int value, (int row, int col) coordinate)> GetLowPoints(int[][] board)
{
    for (var row = 0; row < board.Length; row++)
    {
        for (var col = 0; col < board[row].Length; col++)
        {
            if (IsLowPoint(row, col))
            {
                yield return (board[row][col], (row, col));
            }
        }
    }

    bool IsLowPoint(int row, int col)
    {
        var point = board[row][col];

        return !IsLowerOrSame(row + 1, col, point)
               && !IsLowerOrSame(row - 1, col, point)
               && !IsLowerOrSame(row, col + 1, point)
               && !IsLowerOrSame(row, col - 1, point);
    }

    bool IsLowerOrSame(int row, int col, int reference)
    {
        return row >= 0
               && col >= 0
               && row < board.Length
               && col < board[row].Length
               && board[row][col] <= reference;
    }
}
