// See https://aka.ms/new-console-template for more information


using System.Collections;

var packet = ParseHexaDecimal("D2FE28");

var reader = new Reader(packet);

var version = reader.ReadUntilNextVersion();

Console.Write(version);

char[] ParseHexaDecimal(string hexaDecimal)
{
    return hexaDecimal
        .ToCharArray()
        .Select(n =>
        {
            return n switch
            {
                '0' => "0000",
                '1' => "0001",
                '2' => "0010",
                '3' => "0011",
                '4' => "0100",
                '5' => "0101",
                '6' => "0110",
                '7' => "0111",
                '8' => "1000",
                '9' => "1001",
                'A' => "1010",
                'B' => "1011",
                'C' => "1100",
                'D' => "1101",
                'E' => "1110",
                'F' => "1111",
            };
        })
        .SelectMany(s => s.ToCharArray())
        .ToArray();
}


class Reader
{
    private readonly char[] input;
    private int Pos { get; set; }

    public Reader(char[] input)
    {
        this.input = input;
    }

    public int ReadUntilNextVersion()
    {
        var nextPacket = this.ReadNextPacket().ToArray();

        return ToDecimal(nextPacket.Take(3).ToArray());
    }

    private IEnumerable<char> ReadNextPacket()
    {
        if (this.Pos >= this.input.Length)
        {
            yield break;
        }

        // Read packet version - 3 bits
        yield return this.input[this.Pos++];
        yield return this.input[this.Pos++];
        yield return this.input[this.Pos++];

        // Packet type - 3 bits
        var packetType = this.input
            .Skip(this.Pos)
            .Take(3)
            .ToArray();
        var packetTypeDecimal = ToDecimal(packetType);
        this.Pos += 3;

        // Read all 
    }

    private static int ToDecimal(char[] chars)
    {
        var b = new BitArray(chars.Reverse().Select(c => c == '1').ToArray());

        return GetIntFromBitArray(b);
    }

    private static int GetIntFromBitArray(BitArray bitArray)
    {

        if (bitArray.Length > 32)
            throw new ArgumentException("Argument length shall be at most 32 bits.");

        var array = new int[1];
        bitArray.CopyTo(array, 0);
        return array[0];
    }

}
