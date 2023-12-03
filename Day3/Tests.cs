namespace Day3;

public class Tests
{
    [Test]
    public void It_solves_part1_for_my_personal_input()
    {
        var actualSum = new Solution().Calculate(File.ReadAllText($"input.txt"));

        Console.WriteLine($"The result is {actualSum}");
        Assert.That(actualSum, Is.EqualTo(525119));
    }

    [Test]
    public void It_solves_part2_for_my_personal_input()
    {
        SolutionPt2 sln = new();
        var actualSum = sln.Calculate(File.ReadAllText("input.txt"));

        Console.WriteLine($"The result is {actualSum}");
        Assert.That(actualSum, Is.EqualTo(76504829));
    }


    private string _exampleInput = @"
467..114..
...*......
..35..633.
......#...
617*......
.....+.58.
..592.....
......755.
...$.*....
.664.598..";

    [Test]
    public void It_returns_correct_result_for_pt1_of_example()
    {
        var actualSum = new Solution().Calculate(_exampleInput);

        Assert.That(actualSum, Is.EqualTo(4361));
    }

    [Test]
    public void It_returns_correct_result_for_pt2_of_example()
    {
        var actualSum = new SolutionPt2().Calculate(_exampleInput);

        Assert.That(actualSum, Is.EqualTo(467835));
    }
}