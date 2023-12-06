using System.Text.RegularExpressions;
using NUnit.Framework.Constraints;

namespace Day4;

[TestFixture]
public class Tests
{
    private string _exampleInput;
    private string _personalInput;

    [OneTimeSetUp]
    public void OneTimeSetUp()
    {
        _exampleInput = File.ReadAllText("example.txt");
        _personalInput = File.ReadAllText($"input.txt");
    }

    [Test]
    public void It_solves_part1_for_my_personal_input()
    {
        var actualResult = Solution.SolvePt1(_personalInput);

        Console.WriteLine($"The result is {actualResult}");
        Assert.That(actualResult, Is.EqualTo(23750));
    }

    [Test]
    public void It_solves_part2_for_my_personal_input()
    {
        var actualResult = Solution.SolvePt2(_personalInput);

        Console.WriteLine($"The result is {actualResult}");
        Assert.That(actualResult, Is.EqualTo(13261850));
    }

    [Test]
    public void It_returns_correct_result_for_pt1_of_example()
    {
        var actualSum = Solution.SolvePt1(_exampleInput);

        Assert.That(actualSum, Is.EqualTo(13));
    }

    [Test]
    public void It_returns_correct_result_for_pt2_of_example()
    {
        var actualSum = Solution.SolvePt2(_exampleInput);

        Assert.That(actualSum, Is.EqualTo(30));
    }
}

public static class Solution
{
    public static int SolvePt1(string personalInput)
    {
        return 0;
    }

    public static int SolvePt2(string personalInput)
    {
        return 0;
    }
}