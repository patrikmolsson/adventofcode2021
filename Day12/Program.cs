// var input = File.ReadAllLines("12/test.txt");

var input = File.ReadAllLines("12/input.txt");

PartOne();
PartTwo();

void PartOne()
{
    var laxVisitedSmallPolicy = false;
    Execute(laxVisitedSmallPolicy);
}

void PartTwo()
{
    var laxVisitedSmallPolicy = true;
    Execute(laxVisitedSmallPolicy);
}

void Execute(bool laxVisitedSmallPolicy)
{
    var set = new Dictionary<string, Node>();
    foreach (var line in input)
    {
        var nodes = line.Split('-');

        var contentOne = nodes[0];
        var contentTwo = nodes[1];

        if (!set.TryGetValue(contentOne, out var nodeOne))
        {
            nodeOne = Factory(contentOne, laxVisitedSmallPolicy);
            set.Add(contentOne, nodeOne);
        }

        if (!set.TryGetValue(contentTwo, out var nodeTwo))
        {
            nodeTwo = Factory(contentTwo, laxVisitedSmallPolicy);
            set.Add(contentTwo, nodeTwo);
        }

        nodeOne.Edges.Add(nodeTwo);
        nodeTwo.Edges.Add(nodeOne);
    }

    var start = set["start"];
    Console.WriteLine(start.NumberOfPaths(new HashSet<Node>(), false));
}

static Node Factory(string input, bool laxVisitedSmallPolicy)
{
    return input switch
    {
        "end" => new End(input),
        "start" => new Start(input),
        _ => new Cave(input, laxVisitedSmallPolicy)
    };
}

internal abstract class Node
{
    public Node(string name) => this.Name = name;

    public string Name { get; }

    public ISet<Node> Edges { get; } = new HashSet<Node>();

    public abstract int NumberOfPaths(ISet<Node> visitedNodes, bool visitedSmallTwice);
}

internal class Start : Node
{
    public Start(string name) : base(name)
    {
    }

    public override int NumberOfPaths(ISet<Node> visitedNodes, bool visitedSmallTwice)
    {
        if (visitedNodes.Contains(this))
        {
            return 0;
        }

        var nodes = new HashSet<Node>(visitedNodes) {this};

        return this.Edges
            .Select(edge => edge.NumberOfPaths(nodes, visitedSmallTwice)).Sum();
    }
}

internal class End : Node
{
    public End(string name) : base(name)
    {
    }

    public override int NumberOfPaths(ISet<Node> visitedNodes, bool visitedSmallTwice) => 1;
}

internal class Cave : Node
{
    private readonly bool laxVisitedSmallPolicy;

    public Cave(string name, bool laxVisitedSmallPolicy) : base(name)
    {
        this.laxVisitedSmallPolicy = laxVisitedSmallPolicy;
        this.IsBig = string.Equals(name, name.ToUpperInvariant(), StringComparison.Ordinal);
    }

    private bool IsBig { get; }

    public override int NumberOfPaths(ISet<Node> visitedNodes, bool visitedSmallTwice)
    {
        var newVisitedSmallTwice = visitedSmallTwice;
        if (visitedNodes.Contains(this) && !this.IsBig)
        {
            if (!this.laxVisitedSmallPolicy)
            {
                return 0;
            }

            if (visitedSmallTwice)
            {
                return 0;
            }

            newVisitedSmallTwice = true;
        }

        var nodes = new HashSet<Node>(visitedNodes) {this};


        return this.Edges
            .Select(edge => edge.NumberOfPaths(nodes, newVisitedSmallTwice)).Sum();
    }
}
