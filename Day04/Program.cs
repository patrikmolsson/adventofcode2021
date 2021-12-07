// See https://aka.ms/new-console-template for more information

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

// var input = File.ReadAllLines("04/test.txt");
var input = File.ReadAllLines("04/input.txt");

var numberDraw = input[0].Split(',').Select(int.Parse);

var boardStrings = input[1..]
    .Where(s => !string.IsNullOrWhiteSpace(s))
    .Select((s, i) => new { row = s, index = i })
    .GroupBy(arg => arg.index / 5)
    .Select(s => s.Select(c => c.row).ToArray()).ToArray();

var boards = boardStrings.Select(s => new Board(s)).ToList();

foreach (var board in boards)
{
    Console.WriteLine(board.ToString());
}

PartTwo();

void PartTwo()
{
    var boardSet = boards.ToHashSet();
    foreach (var number in numberDraw)
    {
        Console.WriteLine($"Calling {number}");

        foreach (var board in boardSet)
        {
            board.Mark(number);
        }

        var bingoBoards = boardSet.Where(s => s.HasBingo);

        foreach (var bingoBoard in bingoBoards)
        {
            Console.WriteLine("BINGO");
            Console.WriteLine(
                $"UnmarkedSum: {bingoBoard.UnmarkedNumbersSum} Number: {number}, Answer: {bingoBoard.UnmarkedNumbersSum * number}");
            boardSet.Remove(bingoBoard);
        }
    }
}

void PartOne()
{
    foreach (var number in numberDraw)
    {
        Console.WriteLine($"Calling {number}");

        foreach (var board in boards)
        {
            board.Mark(number);
        }

        var bingoBoard = boards.SingleOrDefault(s => s.HasBingo);

        if (bingoBoard != null)
        {
            Console.WriteLine("BINGO");
            Console.WriteLine(
                $"UnmarkedSum: {bingoBoard.UnmarkedNumbersSum} Number: {number}, Answer: {bingoBoard.UnmarkedNumbersSum * number}");
            break;
        }
    }
}


internal class Board
{
    private ICollection<Row> Rows { get; }

    private ICollection<Row> Columns { get; }

    public Board(IEnumerable<string> input)
    {
        var numberMatrix = input.Select(s =>
            s.Split(' ', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries).Select(int.Parse)
                .ToArray()).ToArray();

        Rows = numberMatrix.Select(s => new Row(s)).ToList();

        Columns = GenerateColumns(numberMatrix).ToList();

        Debug.Assert(Rows.Count == 5);
        Debug.Assert(Columns.Count == 5);

        static IEnumerable<Row> GenerateColumns(int[][] matrix)
        {
            for (var colIndex = 0; colIndex < matrix.Length; colIndex++)
            {
                var column = new int[matrix.Length];

                for (var rowIndex = 0; rowIndex < matrix.Length; rowIndex++)
                {
                    column[rowIndex] = matrix[rowIndex][colIndex];
                }

                yield return new Row(column);
            }
        }
    }

    public void Mark(int number)
    {
        foreach (var column in Columns)
        {
            if (column.TryMark(number))
            {
                break;
            }
        }

        foreach (var row in Rows)
        {
            if (row.TryMark(number))
            {
                break;
            }
        }
    }

    public bool HasBingo
    {
        get
        {
            var bingoRows = Columns
                .Union(Rows)
                .Where(s => s.HasBingo)
                .ToList();

            Debug.Assert(bingoRows.Count is 0 or 1 or 2);

            return bingoRows.Any();
        }
    }

    public override string ToString()
    {
        var sb = new StringBuilder();
        foreach (var row in Rows)
        {
            sb.AppendLine(row.ToString());
        }

        return sb.ToString();
    }

    public int UnmarkedNumbersSum => Rows.Sum(s => s.UnmarkedNumbersSum);
}

internal class Row
{
    public Row(IEnumerable<int> input)
    {
        Numbers = input.ToDictionary(s => s, _ => false);
    }

    private Dictionary<int, bool> Numbers { get; }

    public override string ToString()
    {
        var sb = new StringBuilder();
        foreach (var value in Numbers.Keys)
        {
            sb.Append($"{value:D2} ");
        }

        return sb.ToString();
    }

    public bool TryMark(int number)
    {
        if (!Numbers.ContainsKey(number))
        {
            return false;
        }

        return Numbers[number] = true;
    }

    public bool HasBingo => Numbers.Values.All(s => s);

    public int UnmarkedNumbersSum => Numbers
        .Where(v => !v.Value)
        .Sum(s => s.Key);
}