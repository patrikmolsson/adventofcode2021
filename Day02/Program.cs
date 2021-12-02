using System;
using System.IO;


// var input = new List<string>{
//     "forward 5",
//     "down 5",
//     "forward 8",
//     "up 3",
//     "down 8",
//     "forward 2"
// };
var input = File.ReadAllLines("02/input.txt");

PartOne();
PartTwo();

void PartOne()
{
    var boat = new Boat();

    foreach (var row in input) boat.ProcessInstruction(row);

    Console.WriteLine(boat.Horizontal * boat.Depth);
}

void PartTwo()
{
    var boat = new BoatWithAim();

    foreach (var row in input) boat.ProcessInstruction(row);

    Console.WriteLine(boat.Horizontal * boat.Depth);
}

internal class Boat
{
    public int Horizontal { get; protected set; }
    public int Depth { get; protected set; }

    public void ProcessInstruction(string instruction)
    {
        var splits = instruction.Split(" ", StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
        var direction = splits[0];
        var amplitude = int.Parse(splits[1]);

        ProcessInstruction(direction, amplitude);
    }

    protected virtual void ProcessInstruction(string direction, int amplitude)
    {
        switch (direction)
        {
            case "forward":
            {
                Horizontal += amplitude;
                break;
            }
            case "up":
            {
                Depth -= amplitude;
                break;
            }
            case "down":
            {
                Depth += amplitude;
                break;
            }
            default: throw new ArgumentOutOfRangeException();
        }
    }
}

internal class BoatWithAim : Boat
{
    private int Aim { get; set; }
    
    protected override void ProcessInstruction(string direction, int amplitude)
    {
        switch (direction)
        {
            case "forward":
            {
                Horizontal += amplitude;
                Depth += amplitude * Aim;
                break;
            }
            case "up":
            {
                Aim -= amplitude;
                break;
            }
            case "down":
            {
                Aim += amplitude;
                break;
            }
            default: throw new ArgumentOutOfRangeException();
        }
    }
}