// See https://aka.ms/new-console-template for more information

var input = File.ReadAllLines("11/input.txt");
// var input = File.ReadAllLines("11/test.txt");


var cavern = new Dictionary<Coord, Octopus>();

for (var row = 0; row < input.Length; row++)
{
    var line = input[row];

    for (var col = 0; col < line.Length; col++)
    {
        var level = line[col];

        var coord = new Coord(row, col);
        cavern.Add(coord, new Octopus(level, coord));
    }
}

long flashTotal = 0;
var step = 1;
while (true) {
    var flashQueue = new Queue<Octopus>();

    foreach (var octopus in cavern.Values)
    {
        octopus.Increase();

        if (octopus.WillFlash())
        {
            flashQueue.Enqueue(octopus);
        }
    }

    while (flashQueue.TryDequeue(out var flashedOctopus))
    {
        if (flashedOctopus.RecentlyFlashed)
        {
            continue;
        }

        flashedOctopus.Flash();

        foreach (var neighborCoord in flashedOctopus.NeighborCoords())
        {
            if (cavern.TryGetValue(neighborCoord, out var neighbor))
            {
                neighbor.Increase();

                if (neighbor.WillFlash())
                {
                    flashQueue.Enqueue(neighbor);
                }
            }
        }
    }

    var flashedOctopuses = cavern.Values.Where(s => s.RecentlyFlashed).ToList();
    foreach (var flashedOctopus in flashedOctopuses)
    {
        flashedOctopus.Reset();
    }

    flashTotal += flashedOctopuses.Count;

    Console.WriteLine($"Step: {step}, Flashed: {flashTotal}");

    if (step == 100)
    {
        Console.WriteLine($"PART ONE! Step: {step}, Flashed: {flashTotal}");
    }

    if (flashedOctopuses.Count == cavern.Values.Count)
    {
        Console.WriteLine($"PART TWO! SYNC! Step: {step}");
        break;
    }

    step += 1;
}


internal record Coord(int Row, int Col);


internal class Octopus
{
    private Coord Coord { get; }

    public Octopus(char energyLevel, Coord coord)
    {
        this.Coord = coord;
        this.EnergyLevel = energyLevel - '0';
    }
    private int EnergyLevel { get; set; }

    public bool RecentlyFlashed { get; set; }
    public bool WillFlash() => this.EnergyLevel > 9 && !this.RecentlyFlashed;
    public void Flash()
    {
        this.RecentlyFlashed = true;
    }

    public void Increase() => this.EnergyLevel += 1;
    public void Reset()
    {
        this.RecentlyFlashed = false;
        this.EnergyLevel = 0;
    }

    public IEnumerable<Coord> NeighborCoords()
    {
        // Top-bottom
        yield return this.Coord with { Row = this.Coord.Row + 1 };
        yield return this.Coord with { Row = this.Coord.Row - 1 };

        // Left-right
        yield return this.Coord with { Col = this.Coord.Col + 1 };
        yield return this.Coord with { Col = this.Coord.Col - 1 };

        // Top left-right
        yield return new Coord(this.Coord.Row - 1, this.Coord.Col - 1 );
        yield return new Coord(this.Coord.Row - 1, this.Coord.Col + 1);

        // Bottom left-right
        yield return new Coord(this.Coord.Row + 1, this.Coord.Col - 1 );
        yield return new Coord(this.Coord.Row + 1, this.Coord.Col + 1);
    }
}
