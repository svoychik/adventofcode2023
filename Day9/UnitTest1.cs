namespace Day9;

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
        var actualResult = new Solution().Solve_ExtrapolateForward(_personalInput);

        Assert.That(actualResult, Is.EqualTo(1757008019));
    }

    [Test]
    public void It_solves_part2_for_my_personal_input()
    {
        var actualResult = new Solution().Solve_ExtrapolateBackwards(_personalInput);

        Assert.That(actualResult, Is.EqualTo(995));
    }

    [Test]
    public void It_returns_correct_result_for_pt1_of_example()
    {
        var actualSum = new Solution().Solve_ExtrapolateForward(_exampleInput);

        Assert.That(actualSum, Is.EqualTo(114));
    }

    [Test]
    public void It_returns_correct_result_for_pt2_of_example()
    {
        var actualSum = new Solution().Solve_ExtrapolateBackwards(_exampleInput);

        Assert.That(actualSum, Is.EqualTo(2));
    }
}

public class Solution
{
    
    static long NextTerm2(IEnumerable<long> arr) 
    {
        var differences = new List<long>();
        var arrList = arr.ToList();
        for (int i = 0; i < arrList.Count - 1; i++)
        {
            differences.Add(arrList[i + 1] - arrList[i]);
        }

        // Check if all differences are the same
        if (differences.Distinct().Count() == 1)
        {
            return differences[0];
        }
        return differences[differences.Count - 1] + NextTerm2(differences);

    }
    
    static long NextTerm(long[] arr) 
    {
        // To not create the whole array every time, I'm going to use segment
        var segment = new ArraySegment<long>(arr, 1, arr.Length - 1);

        var reduced = arr.Zip(segment, (a, b) => b - a).ToArray();

        if (reduced.Distinct().Count() == 1)
            return reduced.First();
    
        return reduced.Last() + NextTerm(reduced.ToArray());
    }

    public long Solve_ExtrapolateForward(string input)
    {
        var arr = ParseInput(input).ToArray();
        return arr.Select(arr => arr.Last() + NextTerm(arr)).Sum();
    }

    public long Solve_ExtrapolateBackwards(string input)
    {
        var arr = ParseInput(input);
        return arr.Select(a => a.Reverse()).Select(arr => arr.Last() + NextTerm(arr.ToArray())).Sum();
    }

    private static long[][] ParseInput(string input)
    {
        var lines = input.Split(new[] { "\n" }, StringSplitOptions.RemoveEmptyEntries);
        var arr = lines.Select(l => l.Trim().Split(' ').Select(long.Parse).ToArray()).ToArray();
        return arr;
    }
}