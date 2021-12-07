// See https://aka.ms/new-console-template for more information

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

// var input = File.ReadAllText("06/test.txt");
var input = File.ReadAllText("06/input.txt");

PartOne();
PartTwo();

void PartTwo()
{
    var lanternFishPods = input
        .Split(',', StringSplitOptions.RemoveEmptyEntries)
        .Select(int.Parse)
        .GroupBy(s => s)
        .Select(s => new LanternFishPod(s.Count(), s.Key))
        .ToList();

    var days = 256;

    for (var day = 1; day <= days; day += 1)
    {
        var count = lanternFishPods.Sum(fish => fish.SpawnNewFish());

        lanternFishPods.Add(new LanternFishPod(count, 8));

        Console.WriteLine($"Day: {day}");
    }

    Console.WriteLine($"Fish: {lanternFishPods.Sum(s => s.NumberOfFish)}");
}

void PartOne()
{
    var lanternFish = input.Split(',', StringSplitOptions.RemoveEmptyEntries).Select(s => new LanternFish(int.Parse(s)))
        .ToList();
    var days = 80;

    for (var day = 1; day <= days; day += 1)
    {
        var fishSpawned = new List<LanternFish>();
        foreach (var fish in lanternFish)
        {
            var spawn = fish.TrySpawnNewFish();

            if (spawn) fishSpawned.Add(new LanternFish(8));
        }

        lanternFish.AddRange(fishSpawned);
        Console.WriteLine($"Day: {day}");
    }

    Console.WriteLine($"Fish: {lanternFish.Count}");
}

internal class LanternFishPod
{
    public LanternFishPod(long numberOfFished, int daysToSpawn)
    {
        NumberOfFish = numberOfFished;
        DaysToSpawn = daysToSpawn;
    }

    public long NumberOfFish { get; }
    private int DaysToSpawn { get; set; }

    public long SpawnNewFish()
    {
        if (DaysToSpawn == 0)
        {
            DaysToSpawn = 6;
            return NumberOfFish;
        }

        DaysToSpawn -= 1;
        return 0;
    }
}

internal class LanternFish
{
    public LanternFish(int daysToSpawn)
    {
        DaysToSpawn = daysToSpawn;
    }

    private int DaysToSpawn { get; set; }

    public bool TrySpawnNewFish()
    {
        if (DaysToSpawn == 0)
        {
            DaysToSpawn = 6;
            return true;
        }

        DaysToSpawn -= 1;
        return false;
    }
}