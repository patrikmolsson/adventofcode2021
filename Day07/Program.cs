// See https://aka.ms/new-console-template for more information

using System;
using System.IO;
using System.Linq;

// var input = File.ReadAllText("07/test.txt");
var input = File.ReadAllText("07/input.txt");

var crabs = input.Split(',', StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToList();

var minPos = crabs.Min();
var maxPos = crabs.Max();

PartOne();
PartTwo();

void PartOne()
{
    var linearFuelCalculation = new FuelCalculation(steps => steps);

    var minFuelRequired = int.MaxValue;
    for (var pos = minPos; pos <= maxPos; pos += 1)
    {
        minFuelRequired = MinFuelRequired(pos, minFuelRequired, linearFuelCalculation);
    }

    Console.WriteLine($"MinFuel: {minFuelRequired}");
}

void PartTwo()
{
    var exponentialCalculation = new FuelCalculation(steps => Enumerable.Range(1, steps).Sum());

    var minFuelRequired = int.MaxValue;
    for (var pos = minPos; pos <= maxPos; pos += 1)
    {
        minFuelRequired = MinFuelRequired(pos, minFuelRequired, exponentialCalculation);
    }

    Console.WriteLine($"MinFuel: {minFuelRequired}");
}

int MinFuelRequired(int pos, int previousMin, FuelCalculation fuelCalculation)
{
    var fuelForPos = 0;

    foreach (var crab in crabs)
    {
        var steps = Math.Abs(crab - pos);

        fuelForPos += fuelCalculation(steps);

        if (fuelForPos >= previousMin)
        {
            return previousMin;
        }
    }

    return fuelForPos;
}

internal delegate int FuelCalculation(int steps);