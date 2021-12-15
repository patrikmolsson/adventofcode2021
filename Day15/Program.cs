using System.Diagnostics;

// var input = File.ReadAllLines("15/test.txt");
var input = File.ReadAllLines("15/input.txt");

PartOne();
PartTwo();

void PartOne()
{
    var board = GenerateBoard(1);
    SolveMinPath(board);
}
void PartTwo()
{
    var board = GenerateBoard(5);
    SolveMinPath(board);
}

IDictionary<Coord, Node> GenerateBoard(int repeatCount)
{
    var width = input.Length;
    var board = new Dictionary<Coord, Node>();

    for (var multiplierRow = 0; multiplierRow < repeatCount; multiplierRow++)
    for (var multiplierCol = 0; multiplierCol < repeatCount; multiplierCol++)
    for (var rowIndex = 0; rowIndex < input.Length; rowIndex++)
    {
        Debug.Assert(input.Length == input[rowIndex].Length, "Should have square");
        for (var colIndex = 0; colIndex < input[rowIndex].Length; colIndex++)
        {
            var baseRisk = int.Parse(input[rowIndex][colIndex].ToString());

            var newRisk = baseRisk + multiplierCol + multiplierRow;

            if (newRisk > 9)
            {
                newRisk %= 9;
            }

            var newRow = rowIndex + (width * multiplierRow);
            var newCol = colIndex + (width * multiplierCol);
            var coord = new Coord(newRow, newCol);
            var node = new Node(coord, newRow == 0 && newCol == 0 ? 0 : int.MaxValue, newRisk);
            board[coord] = node;
        }
    }

    return board;
}


void SolveMinPath(IDictionary<Coord, Node> board)
{
    var endIndex = Convert.ToInt32(Math.Sqrt(board.Count)) - 1;
    var endCoord = new Coord(endIndex, endIndex);
    var end = board[endCoord];
    var start = board[new Coord(0, 0)];
    var sorted = new SortedSet<Node> {start};

    do
    {
        var current = sorted.First();
        sorted.Remove(current);

        var unvisitedNeighbors = current.Neighbors()
            .Where(board.ContainsKey)
            .Where(c => !board[c].Visited);

        foreach (var neighborCoord in unvisitedNeighbors)
        {
            var neighbor = board[neighborCoord];

            sorted.Remove(neighbor);

            neighbor.TentativeTotalRisk =
                Math.Min(neighbor.TentativeTotalRisk, current.TentativeTotalRisk + neighbor.Risk);

            sorted.Add(neighbor);
        }

        current.Visited = true;

        if (current == end)
        {
            break;
        }
    } while (sorted.Any());


    var minRisk = end.TentativeTotalRisk;

    Console.WriteLine(minRisk);
}

internal record Coord(int Row, int Col);

internal class Node : IComparable<Node>
{
    public Node(Coord coord, int tentativeTotalRisk, int risk)
    {
        this.Coord = coord;
        this.TentativeTotalRisk = tentativeTotalRisk;
        this.Risk = risk;
    }

    private Coord Coord { get; }

    public int TentativeTotalRisk { get; set; }

    public bool Visited { get; set; }

    public int Risk { get; }

    public IEnumerable<Coord> Neighbors()
    {
        yield return this.Coord with {Col = this.Coord.Col + 1};
        yield return this.Coord with {Col = this.Coord.Col - 1};
        yield return this.Coord with {Row = this.Coord.Row - 1};
        yield return this.Coord with {Row = this.Coord.Row + 1};
    }

    public int CompareTo(Node? other)
    {
        if (ReferenceEquals(this, other))
        {
            return 0;
        }

        if (ReferenceEquals(null, other))
        {
            return 1;
        }

        var s = this.TentativeTotalRisk.CompareTo(other.TentativeTotalRisk);
        if (s != 0)
        {
            return s;
        }

        var c = this.Coord.Col.CompareTo(other.Coord.Col);
        if (c != 0)
        {
            return c;
        }

        return this.Coord.Row.CompareTo(other.Coord.Row);
    }
}
