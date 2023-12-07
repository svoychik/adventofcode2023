namespace Day7;

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
        Assert.That(actualResult, Is.EqualTo(6440));
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

        Assert.That(actualSum, Is.EqualTo(6440));
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
        var arr = personalInput.Split(new[] { "\n" }, StringSplitOptions.RemoveEmptyEntries)
            .Select(x => x.Split(' '))
            .Select(x => new Card
            {
                HandType = DetermineHandType(x[0]),
                Value = x[0],
                Bid = int.Parse(x[1])
            }).ToArray();
        var orderedArray = arr.OrderDescending(Comparer<Card>.Create((card1, card2) =>
        {
            if (card1.HandType > card2.HandType)
                return 1;
            if (card2.HandType > card1.HandType)
                return -1;
            if (card2.HandType == card1.HandType)
            {
                var dictPriorityOfLetters = new Dictionary<char, int>()
                {
                    ['A'] = 14,
                    ['K'] = 13,
                    ['Q'] = 12,
                    ['J'] = 11,
                    ['T'] = 10,
                    ['9'] = 9,
                    ['8'] = 8,
                    ['7'] = 7,
                    ['6'] = 6,
                    ['5'] = 5,
                    ['4'] = 4,
                    ['3'] = 3,
                    ['2'] = 2
                };
                for (int i = 0; i < card2.Value.Length; i++)
                {
                    var ch1 = card1.Value[i];
                    var ch2 = card2.Value[i];
                    if (ch1 == ch2)
                        continue;
                    var p1 = dictPriorityOfLetters[ch1];
                    var p2 = dictPriorityOfLetters[ch2];
                    return p1 > p2 ? 1 : -1;
                }
            }

            return 0;
        })).ToList();
        var res = 0;
        for (int i = 0; i < orderedArray.Count; i++)
        {
            res += orderedArray[i].Bid * (orderedArray.Count() - i);
        }

        return res;
    }


    private static Dictionary<HandType, Func<string, bool>> dict = new()
    {
        [HandType.FiveOfKind] = str => CountSymbols(str).Any(x => x.Value == 5),
        [HandType.FourOfKind] = str => CountSymbols(str).Any(x => x.Value == 4),
        [HandType.FullHouse] = str => CountSymbols(str).Any(x => x.Value == 3)
                                      && str.Distinct().Count() == 2,
        [HandType.ThreeOfKind] = str => CountSymbols(str).Any(x => x.Value == 3)
                                        && str.Distinct().Count() == 3,
        [HandType.TwoPair] = str => CountSymbols(str).Count(x => x.Value == 2) == 2,
        [HandType.OnePair] = str => CountSymbols(str).Count(x => x.Value == 2) == 1
                                    && str.Distinct().Count() == 4,
        [HandType.HandCard] = str => str.Distinct().Count() == 5,
    };


    private static Dictionary<char, int> CountSymbols(string str)
    {
        var dict = new Dictionary<char, int>();
        foreach (var ch in str)
        {
            dict[ch] = 1 + (dict.TryGetValue(ch, out var val) ? val : 0);
        }

        return dict;
    }

    public static HandType DetermineHandType(string str)
    {
        foreach (var (key, func) in dict)
        {
            if (func(str))
                return key;
        }

        throw new Exception(":(");
    }

    public static int SolvePt2(string personalInput)
    {
        return 0;
    }
}

public enum HandType
{
    FiveOfKind  = 6,
    FourOfKind  = 5,
    FullHouse   = 4,
    ThreeOfKind = 3,
    TwoPair     = 2,
    OnePair     = 1,
    HandCard    = 0
}

public class Card
{
    public HandType HandType { get; set; }
    public string Value { get; set; }
    public int Bid { get; set; }
}