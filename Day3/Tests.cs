namespace Day3;

public class Tests
{
    [Test]
    public void It_solves_part1_for_my_personal_input()
    {
        var actualSum = new SolutionPt1().Calculate(File.ReadAllText($"input.txt"));

        Console.WriteLine($"The result is {actualSum}");
        Assert.That(actualSum, Is.EqualTo(525119));
    }

    [Test]
    public void It_solves_part2_for_my_personal_input()
    {
        var actualSum = new SolutionPt2().Calculate(File.ReadAllText("input.txt"));

        Console.WriteLine($"The result is {actualSum}");
        Assert.That(actualSum, Is.EqualTo(76504829));
    }

    [Test]
    public void I_have_the_same_results_for_improved_solution()
    {
        var inputStr = File.ReadAllText("input.txt");
        var actualResultPt1 = new ImprovedSolution().CalculatePt1(inputStr);
        var actualResultPt2 = new ImprovedSolution().CalculatePt2(inputStr);

        Console.WriteLine($"The result is {actualResultPt1}");
        Assert.That(actualResultPt1, Is.EqualTo(new SolutionPt1().Calculate(inputStr)));
        Assert.That(actualResultPt2, Is.EqualTo(new SolutionPt2().Calculate(inputStr)));
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
        var actualSum = new SolutionPt1().Calculate(_exampleInput);

        Assert.That(actualSum, Is.EqualTo(4361));
    }

    [Test]
    public void It_returns_correct_result_for_pt2_of_example()
    {
        var actualSum = new SolutionPt2().Calculate(_exampleInput);

        Assert.That(actualSum, Is.EqualTo(467835));
    }
}