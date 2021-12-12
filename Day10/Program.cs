using System.Globalization;

// var input = File.ReadAllLines("10/test.txt");
var input = File.ReadAllLines("10/input.txt");

PartTwo();

void PartTwo()
{    var points = new List<long>();
     foreach (var line in input)
     {
         var stack = new Stack<char>();
         var corrupted = false;

         foreach (var c in line.ToCharArray())
         {
             switch (c)
             {
                 case '[':
                 case '(':
                 case '<':
                 case '{':
                     stack.Push(c);
                     break;
                 case ']':
                 case ')':
                 case '>':
                 case '}':
                     if (!stack.TryPop(out var s) || !IsMatch(s, c))
                     {
                         corrupted = true;
                     }

                     break;
             }
         }

         if (corrupted)
         {
             continue;
         }

         long p = 0;
         foreach (var c in stack)
         {
             var pointForChar = CharToPointsTwo(Invert(c));

             p *= 5;
             p += pointForChar;
         }

         points.Add(p);
     }

     points = points.OrderBy(s => s).ToList();

     Console.WriteLine(points[points.Count / 2]);

}

void PartOne()
{
    var points = 0;
    foreach (var line in input)
    {
        var stack = new Stack<char>();

        foreach (var c in line.ToCharArray())
        {
            switch (c)
            {
                case '[':
                case '(':
                case '<':
                case '{':
                    stack.Push(c);
                    break;
                case ']':
                case ')':
                case '>':
                case '}':
                    if (!stack.TryPop(out var s) || !IsMatch(s, c))
                    {
                        points += CharToPoints(c);
                    }

                    break;
            }
        }
    }

    Console.WriteLine(points);
}

char Invert(char c)
{
    return c switch
    {
        '(' => ')',
        '[' => ']',
        '{' => '}',
        '<' => '>',
        _ => throw new ArgumentOutOfRangeException(nameof(c), c, null)
    };
}
int CharToPointsTwo(char c)
{
    return c switch
    {
        ')' => 1,
        ']' => 2,
        '}' => 3,
        '>' => 4,
        _ => throw new ArgumentOutOfRangeException(nameof(c), c, null)
    };
}

int CharToPoints(char c)
{
    return c switch
    {
        ')' => 3,
        ']' => 57,
        '}' => 1197,
        '>' => 25137,
        _ => throw new ArgumentOutOfRangeException(nameof(c), c, null)
    };
}

bool IsMatch(char c1, char c2)
{
    return c1 switch
    {
        '(' => c2 == ')',
        '[' => c2 == ']',
        '{' => c2 == '}',
        '<' => c2 == '>',
        _ => throw new ArgumentOutOfRangeException(nameof(c1), c1, null)
    };
}
