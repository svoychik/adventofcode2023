using System.Collections.Immutable;
using System.Text.RegularExpressions;

namespace Day8;

public class Tests
{
    private string[] _exampleInput;
    private string[] _personalInput;
    private string[] _example2Input;
    private string[] _examplePart2Input;

    [SetUp]
    public void SetUp()
    {
        _personalInput = File.ReadAllLines("input.txt");
        _exampleInput = File.ReadAllLines("example.txt");
        _example2Input = File.ReadAllLines("example2.txt");
        _examplePart2Input = File.ReadAllLines("examplePt2.txt");
    }

    [Test]
    public void It_solves_part1_for_my_personal_input()
    {
        var actualResult = new Solution().SolvePt1(_personalInput);

        Assert.That(actualResult, Is.EqualTo(22357));
    }

    [Test]
    public void It_solves_part2_for_my_personal_input()
    {
        var actualResult = new Solution().SolvePt2(_personalInput);

        Assert.That(actualResult, Is.EqualTo(10371555451871));
    }

    [Test]
    public void It_returns_correct_result_for_pt1_of_example()
    {
        var actualResult = new Solution().SolvePt1(_exampleInput);

        Assert.That(actualResult, Is.EqualTo(2));
    }

    [Test]
    public void It_returns_correct_result_for_pt1_of_example2()
    {
        var actualResult = new Solution().SolvePt1(_example2Input);

        Assert.That(actualResult, Is.EqualTo(6));
    }

    [Test]
    public void It_returns_correct_result_for_pt2_of_example()
    {
        var actualResult = new Solution().SolvePt2(_examplePart2Input);

        Assert.That(actualResult, Is.EqualTo(6));
    }
}

public class Solution
{
    public long SolvePt2(string[] lines)
    {
        var path = lines[0];
        var dict = Parse(lines.Skip(1).ToArray());
        var currPoints = dict.Where(x => x.Key.EndsWith("A")).Select(x => x.Key);
        var pointNumberOfStepsDict = new Dictionary<string, long>();
        foreach (var currPoint in currPoints)
        {
            long numberOfSteps = FindNumberOfSteps(path, currPoint, dict, s => s.EndsWith("Z"));
            pointNumberOfStepsDict[currPoint] = numberOfSteps;
            Console.WriteLine(currPoint + " - " + numberOfSteps);
        }


        return FindLeastCommonMultiple(pointNumberOfStepsDict.Select(x => x.Value));
    }

    public long SolvePt1(string[] lines)
    {
        var path = lines[0];
        var dict = Parse(lines.Skip(1).ToArray());
        return FindNumberOfSteps(path, "AAA", dict, s => s.Equals("ZZZ"));
    }

    private static Dictionary<string, (string l, string r)> Parse(string[] lines)
    {
        var dict = new Dictionary<string, (string, string)>();
        var regex = new Regex(@"(\w+)\s*=\s*\((\w+),\s*(\w+)\)");

        foreach (var line in lines.Where(x => !string.IsNullOrWhiteSpace(x)))
        {
            Match match = regex.Match(line);
            var key = match.Groups[1].Value;
            dict[key] = (match.Groups[2].Value, match.Groups[3].Value);
        }

        return dict;
    }

    private static long FindNumberOfSteps(string path, string startPoint, Dictionary<string, (string l, string r)> dict, Func<string, bool> func)
    {
        var numberOfSteps = 0L;
        var currPoint = startPoint; 
        for (int i = 0; i < path.Length; i++)
        {
            numberOfSteps++;
            var ch = path[i];
            currPoint = ch switch
            {
                'L' => dict[currPoint].l,
                'R' => dict[currPoint].r
            };
            if (func(currPoint))
                break;
            if (i == path.Length - 1)
                i = -1; //-1 to consider i++
        }

        return numberOfSteps;
    }

    private long FindLeastCommonMultiple(IEnumerable<long> numbers) =>
        numbers.Aggregate(1L, (current, number) => 
            current / GreatestCommonDivisor(current, number) * number);

    private long GreatestCommonDivisor(long a, long b)
    {
        while (b != 0)
        {
            a %= b;
            (a, b) = (b, a);
        }

        return a;
    }
}