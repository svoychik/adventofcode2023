using NUnit.Framework.Constraints;
using static Day7.Solution;

namespace Day7;

[TestFixture]
public class Tests
{
    private string _exampleInput;
    private string _personalInput;
    private string _personalInputPt2;

    [OneTimeSetUp]
    public void OneTimeSetUp()
    {
        _exampleInput = File.ReadAllText("example.txt");
        _personalInput = File.ReadAllText($"input.txt");
        _personalInputPt2 = File.ReadAllText($"inputPt2.txt");
    }

    [Test]
    public void It_solves_part1_for_my_personal_input()
    {
        var actualResult = new Solution().SolvePt1(_personalInput);

        Assert.That(actualResult, Is.EqualTo(250474325));
    }

    [Test]
    public void It_solves_part2_for_my_personal_input()
    {
        var actualResult = new Solution().SolvePt2(_personalInputPt2);

        Assert.That(actualResult, Is.EqualTo(248909434));
    }

    [Test]
    public void It_returns_correct_result_for_pt1_of_example()
    {
        var actualSum = new Solution().SolvePt1(_exampleInput);

        Assert.That(actualSum, Is.EqualTo(6440));
    }

    [Test]
    public void It_returns_correct_result_for_pt2_of_example()
    {
        var actualSum = new Solution().SolvePt2(_exampleInput);

        Assert.That(actualSum, Is.EqualTo(5905));
    }
}

public  class Solution
{
    public int SolvePt1(string personalInput)
    {
        var arr = personalInput.Split(new[] { "\n" }, StringSplitOptions.RemoveEmptyEntries);
        return 0;
    }


    public int SolvePt2(string exampleInput)
    {
        throw new NotImplementedException();
    }
}
