// See https://aka.ms/new-console-template for more information

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;

var input = File.ReadAllLines("03/input.txt");
// var input = File.ReadAllLines("03/test.txt");
PartOne();
PartTwo();

void PartTwo()
{
    var bits = input.Select(s =>
        s
            .ToCharArray()
            .Select(c => c == '1')
            .ToArray()
        ).ToList();

    var oxygenRating = GetIntFromBoolArray(FilterList(bits, 0, true));
    var co2Rating = GetIntFromBoolArray(FilterList(bits, 0, false));

    Console.WriteLine(oxygenRating * co2Rating);
}

bool[] FilterList(List<bool[]> inputList, int index, bool mostCommonCriteria)
{
    if (inputList.Count == 1)
    {
        return inputList[0];
    }

    var countTrue = inputList.Count(s => s[index]);
    var countFalse = inputList.Count - countTrue;

    var boolToKeep = mostCommonCriteria ? (countTrue >= countFalse) : countFalse > countTrue;

    var filtered = inputList.Where(i => i[index] == boolToKeep).ToList();

    return FilterList(filtered, index + 1, mostCommonCriteria);
}

void PartOne()
{
    var counts = new int[12];

    foreach (var line in input)
    {
        for (var index = 0; index < line.Length; index++)
        {
            var c = line[index];

            counts[index] += c == '1' ? 1 : -1;
        }
    }

    var bitArr = new bool[12];

    for (var index = 0; index < counts.Length; index++)
    {
        var count = counts[index];

        if (count == 0)
        {
            throw new InvalidOperationException();
        }

        bitArr[counts.Length - index - 1] = count > 0;
    }

    var bits = new BitArray(bitArr);

    var gamma = GetIntFromBitArray(bits);
    var epsilon = GetIntFromBitArray(bits.Not());

    var pw = gamma * epsilon;

    Console.WriteLine(pw);
}

static int GetIntFromBoolArray(bool[] boolArray)
{
    return GetIntFromBitArray(new BitArray(boolArray.Reverse().ToArray()));
}

static int GetIntFromBitArray(BitArray bitArray)
{
    int[] array = new int[1];
    bitArray.CopyTo(array, 0);
    return array[0];
}