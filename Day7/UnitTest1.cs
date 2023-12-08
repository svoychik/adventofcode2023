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
        var actualResult = new Solution().Solve(_personalInput);

        Assert.That(actualResult, Is.EqualTo(250474325));
    }

    [Test]
    public void It_solves_part2_for_my_personal_input()
    {
        var actualResult = new Solution(). Solve(_personalInputPt2);

        Assert.That(actualResult, Is.EqualTo(248909434));
    }

    [Test]
    public void It_returns_correct_result_for_pt1_of_example()
    {
        var actualSum = new Solution().Solve(_exampleInput);

        Assert.That(actualSum, Is.EqualTo(6440));
    }

    [Test]
    public void It_returns_correct_result_for_pt2_of_example()
    {
        var actualSum = new Solution().Solve(_exampleInput);

        Assert.That(actualSum, Is.EqualTo(5905));
    }
}

public  class Solution
{
    public int Solve(string personalInput)
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
                var priorityOfLettersDict = new Dictionary<char, int>()
                {
                    ['A'] = 14,
                    ['K'] = 13,
                    ['Q'] = 12,
                    ['T'] = 10,
                    ['9'] = 9,
                    ['8'] = 8,
                    ['7'] = 7,
                    ['6'] = 6,
                    ['5'] = 5,
                    ['4'] = 4,
                    ['3'] = 3,
                    ['2'] = 2,
                    ['J'] = 1
                };
                for (int i = 0; i < card2.Value.Length; i++)
                {
                    var ch1 = card1.Value[i];
                    var ch2 = card2.Value[i];
                    if (ch1 == ch2)
                        continue;
                    var p1 = priorityOfLettersDict[ch1];
                    var p2 = priorityOfLettersDict[ch2];
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


    public static Dictionary<HandType, Func<string, bool>> Dict { get; } = new()
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
        var jokers = str.Count(x => x == 'J');
        if (jokers == 0)
        {
            foreach (var (key, func) in Dict)
            {
                if (func(str))
                    return key;
            }
        }

        else
        {
            var handWithoutJoker = str.Replace("J", "");
            if (handWithoutJoker == "")
                return HandType.FiveOfKind;
            var maxCountKV = CountSymbols(handWithoutJoker).MaxBy(x => x.Value);
            var newHand = str.Replace('J', maxCountKV.Key);
            foreach (var (key, func) in Dict)
            {
                if (func(newHand))
                    return key;
            }
        }

        throw new Exception(":(");
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