using System.Text.RegularExpressions;
using NUnit.Framework.Constraints;

namespace Day8;

public class Tests
{
    private string[] _exampleInput;
    private string[] _personalInput;
    private string[] _example2Input;

    [SetUp]
    public void SetUp()
    {
        _personalInput = File.ReadAllLines("input.txt");
        _exampleInput = File.ReadAllLines("example.txt");
        _example2Input = File.ReadAllLines("example2.txt");

    }

    [Test]
    public void It_solves_part1_for_my_personal_input()
    {
        var actualResult = new Solution().SolvePt1(_personalInput);

        Assert.That(actualResult, Is.EqualTo(22357));
    }

    // [Test]
    // public void It_solves_part2_for_my_personal_input()
    // {
    //     var actualResult = new Solution().SolvePt1(_personalInput);
    //
    //     Console.WriteLine(actualResult);
    //     Assert.That(actualResult, Is.EqualTo(35150181));
    // }

    [Test]
    public void It_returns_correct_result_for_pt1_of_example()
    {
        var actualSum = new Solution().SolvePt1(_exampleInput);

        Assert.That(actualSum, Is.EqualTo(2));
    }

    [Test]
    public void It_returns_correct_result_for_pt1_of_example2()
    {
        var actualSum = new Solution().SolvePt1(_example2Input);

        Assert.That(actualSum, Is.EqualTo(6));
    }
}

public class Solution
{
    private Dictionary<string, (string l, string r)> dict = new();

    private Dictionary<string, (string l, string r)> Parse(string[] lines)
    {
        var dict = new Dictionary<string, (string, string)>();
        var regex = new Regex(@"(\w+)\s*=\s*\((\w+),\s*(\w+)\)");

        foreach (var line in lines)
        {
            if(string.IsNullOrEmpty(line))
                continue;
            
            Match match = regex.Match(line);
            if (match.Success)
            {
                var key = match.Groups[1].Value;
                var value1 = match.Groups[2].Value;
                var value2 = match.Groups[3].Value;

                dict[key] = (value1, value2);
            }
            else
                throw new Exception("Can't parse");
        }

        return dict;
    }

    public int SolvePt1(string[] lines)
    {
        var path = lines[0];
        var currPoint = "AAA";
        var dict = Parse(lines.Skip(1).ToArray());
        var numberOfSteps = 0;
        for (int i = 0; i < path.Length; i++)
        {
            numberOfSteps++;
            var ch = path[i];
            currPoint = ch switch
            {
                'L' => dict[currPoint].l,
                'R' => dict[currPoint].r
            };
            if (currPoint == "ZZZ")
                break;
            if (i == path.Length - 1)
                i = -1; //-1 to consider i++
        }

        return numberOfSteps;

    }
}