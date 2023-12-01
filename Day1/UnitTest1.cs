using System.Text;
using FluentAssertions;

namespace Day1;

public class Tests
{
    [Test]
    public async Task It_works_for_my_personal_input()
    {
        var sln = new Solution();
        var lines = await File.ReadAllLinesAsync($"input1.txt");
        var actualSum = sln.CalculateSum(lines);

        Console.WriteLine($"The result is {actualSum}");
    }

    [Test]
    public void It_returns_correct_result_for_main_task()
    {
        var sln = new Solution();
        var actualSum = sln.CalculateSum(new[]
        {
            "1abc2",
            "pqr3stu8vwx",
            "a1b2c3d4e5f",
            "treb7uchet"
        });

        actualSum.Should().Be(142);
    }

    [TestCase("1xsdaf3", 1, 3)]
    [TestCase("xs1d3af", 1, 3)]
    [TestCase("xss1daf", 1, 1)]
    [TestCase("a1b2c3d4e5f6ffe7f", 1, 7)]
    public void I_receive_two_digits_correctly_when_first_and_last_digits_are_written_as_digits(
        string line, int expectedX, int expectedY)
    {
        var sln = new Solution();
        var digits = sln.FindTwoDigits(line);

        digits.Should().BeEquivalentTo((expectedX, expectedY));
    }

    [TestCase("onetwo", 1, 2)]
    [TestCase("oneight", 1, 8)]
    [TestCase("onexxxxxeight", 1, 8)]
    [TestCase("sevenbsixsbzmone55", 7, 5)]
    [TestCase("nine8sevenfourtwopl7", 9, 7)]
    public void I_receive_two_digits_correctly_when_first_and_last_digits_are_verbal_digits(
        string line, int expectedX, int expectedY)
    {
        var sln = new Solution();
        var digits = sln.FindTwoDigits(line);

        digits.Should().BeEquivalentTo((expectedX, expectedY));
    }
}

public class Solution
{
    private static readonly Dictionary<string, int> dict = new()
    {
        ["zero"] = 0,
        ["0"] = 0,
        ["one"] = 1,
        ["1"] = 1,
        ["two"] = 2,
        ["2"] = 2,
        ["three"] = 3,
        ["3"] = 3,
        ["four"] = 4,
        ["4"] = 4,
        ["five"] = 5,
        ["5"] = 5,
        ["six"] = 6,
        ["6"] = 6,
        ["seven"] = 7,
        ["7"] = 7,
        ["eight"] = 8,
        ["8"] = 8,
        ["nine"] = 9,
        ["9"] = 9,
    };

    public int CalculateSum(string[] arr) =>
        arr
            .Select(FindTwoDigits)
            .Select(x => x.Item1 + "" + x.Item2)
            .Select(int.Parse)
            .Sum();

    public (int, int) FindTwoDigits(string line)
    {
        var firstNumber = int.MaxValue;
        var lastNumber = int.MaxValue;
        var str = "";

        var chars = line.ToCharArray();
        for (var i = 0; i < chars.Length; i++)
        {
            str += chars[i];
            if (TryParseFirstDigit(str, out var result))
            {
                firstNumber = result;
                break;
            }
        }

        str = "";

        for (int i = chars.Length - 1; i >= 0; i--)
        {
            str = str.Insert(0, chars[i].ToString());
            if (TryParseLastDigit(str, out var result))
            {
                lastNumber = result;
                break;
            }
        }

        return (firstNumber, lastNumber);
    }

    private bool TryParseFirstDigit(string str, out int digit)
    {
        digit = -1;
        var listOfFindings = new List<(string key, int index)>();
        foreach (var kv in dict)
        {
            var lastIndex = str.IndexOf(kv.Key, StringComparison.Ordinal);
            if (lastIndex != -1)
            {
                digit = kv.Value;
                listOfFindings.Add((kv.Key, lastIndex));
            }
        }

        if (!listOfFindings.Any()) 
            return false;
        var result = listOfFindings.MinBy(x => x.index);
        digit = dict[result.key];
        return true;
    }

    private bool TryParseLastDigit(string str, out int digit)
    {
        digit = -1;
        var listOfFindings = new List<(string key, int index)>();
        foreach (var kv in dict)
        {
            var lastIndex = str.LastIndexOf(kv.Key, StringComparison.Ordinal);
            if (lastIndex != -1)
            {
                digit = kv.Value;
                listOfFindings.Add((kv.Key, lastIndex));
            }
        }

        if (!listOfFindings.Any()) 
            return false;
        var result = listOfFindings.MaxBy(x => x.index);
        digit = dict[result.key];
        return true;

    }
}