using System.Text.RegularExpressions;

namespace Day4;

[TestFixture]
public class Tests
{
    private readonly string _exampleInput = @"
Card 1: 41 48 83 86 17 | 83 86  6 31 17  9 48 53
Card 2: 13 32 20 16 61 | 61 30 68 82 17 32 24 19
Card 3:  1 21 53 59 44 | 69 82 63 72 16 21 14  1
Card 4: 41 92 73 84 69 | 59 84 76 51 58  5 54 83
Card 5: 87 83 26 28 32 | 88 30 70 12 93 22 82 36
Card 6: 31 18 13 56 72 | 74 77 10 23 35 67 36 11
";

    private string _personalInput;

    [OneTimeSetUp]
    public void OneTimeSetUp()
    {
        _personalInput = File.ReadAllText($"input.txt");
    }

    [Test]
    public void It_solves_part1_for_my_personal_input()
    {
        var actualResult = new Solution().CalculateNumberOfPoints(_personalInput);

        Console.WriteLine($"The result is {actualResult}");
        Assert.That(actualResult, Is.EqualTo(23750));
    }

    [Test]
    public void It_solves_part2_for_my_personal_input()
    {
        var actualResult = new Solution().CalculateNumberOfScratchcards(_personalInput);

        Console.WriteLine($"The result is {actualResult}");
        Assert.That(actualResult, Is.EqualTo(13261850));
    }

    [Test]
    public void It_returns_correct_result_for_pt1_of_example()
    {
        var actualSum = new Solution().CalculateNumberOfPoints(_exampleInput);

        Assert.That(actualSum, Is.EqualTo(13));
    }

    [Test]
    public void It_returns_correct_result_for_pt2_of_example()
    {
        var actualSum = new Solution().CalculateNumberOfScratchcards(_exampleInput);

        Assert.That(actualSum, Is.EqualTo(30));
    }
}

public class Solution
{
    public int CalculateNumberOfScratchcards(string text)
    {
        string[] lines = text.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries);
        var numberOfPointsPerCard = CountWinningNumbers(lines).ToList();
        var dict = new Dictionary<int, int>(); //cardId - N

        for (var i = 0; i < numberOfPointsPerCard.Count(); i++)
        {
            var numberOfCopies = dict.ContainsKey(i) ? dict[i] + 1 : 1;
            var numberOfCardsWon = numberOfPointsPerCard[i];

            for (var cardId = i + 1; (cardId - i) <= numberOfCardsWon; cardId++)
            {
                dict[cardId] = dict.TryGetValue(cardId, out var val)
                    ? val + 1 * numberOfCopies

                    : 1 * numberOfCopies;
            }
        }

        return lines.Length + dict.Values.Sum();
    }

    public int CalculateNumberOfPoints(string text)
    {
        var lines = text.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries);
        return CountWinningNumbers(lines).Where(x => x != 0).Sum(counter => 1 << (counter - 1));
    }

    private IEnumerable<int> CountWinningNumbers(string[] lines)
    {
        foreach (var line in lines)
        {
            var parts = line.Split("|");
            var number1Str = parts[0].Split(':')[1];
            var number2Str = parts[1];
            var numbers1 = ExtractNumbersFromTheString(number1Str).ToHashSet();
            var numbers2 = ExtractNumbersFromTheString(number2Str).ToHashSet();
            yield return numbers1.Count(numbers2.Contains);
        }
    }

    private static readonly string Pattern = @"\d+";

    private IEnumerable<int> ExtractNumbersFromTheString(string str)
    {
        foreach (Match match in Regex.Matches(str, Pattern))
        {
            yield return int.Parse(match.Value);
        }
    }
}