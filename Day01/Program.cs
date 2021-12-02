// See https://aka.ms/new-console-template for more information

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

PartOne();
PartTwo();

void PartOne()
{
    var lines = File.ReadAllLines("01/01.txt");

    var depths = lines.Select(int.Parse);

    var increasedCount = 0;
    int? previousDepth = null;
    foreach (var depth in depths)
    {
        if (depth > previousDepth) increasedCount += 1;

        previousDepth = depth;
    }

    Console.WriteLine(increasedCount);
}

void PartTwo()
{
    var lines = File.ReadAllLines("01/01.txt");
    var depths = lines.Select(int.Parse).ToArray();

    var buckets = new List<Bucket>();

    var count = 0;

    for (var index = 0; index < depths.Length; index++)
    {
        buckets.Add(new Bucket(index));

        var depth = depths[index];
        foreach (var bucket in buckets.Where(bucket => !bucket.IsFull))
            bucket.AddDepth(depth);

        var fullBuckets = buckets.Where(s => s.IsFull).ToList();

        if (fullBuckets.Count < 2) continue;

        var orderedBuckets = fullBuckets.OrderBy(s => s.Index).ToArray();
        var formerBucket = orderedBuckets[0];
        var lastBucket = orderedBuckets[1];

        if (lastBucket.Sum > formerBucket.Sum) count += 1;

        buckets.Remove(formerBucket);
    }

    Console.WriteLine(count);
}

internal class Bucket
{
    public Bucket(int index)
    {
        Index = index;
    }

    public int Index { get; }

    public int Sum { get; private set; }

    private int DepthCount { get; set; }

    public bool IsFull => DepthCount >= 3;

    public void AddDepth(int depth)
    {
        Sum += depth;
        DepthCount += 1;
    }
}