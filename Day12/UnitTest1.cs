using NUnit.Framework.Constraints;

namespace Day12;

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
        var actualResult = new Solution().SolvePt1(_personalInput);

        Assert.That(actualResult, Is.EqualTo(7090));
    }

    [Test]
    public void It_solves_part2_for_my_personal_input()
    {
        var actualResult = new Solution().SolvePt2(_personalInput);

        Assert.That(actualResult, Is.EqualTo(6792010726878));
    }

    [TestCase("???.### 1,1,3", 1)]
    [TestCase(".??..??...?##. 1,1,3", 4)]
    [TestCase("?#?#?#?#?#?#?#? 1,3,1,6", 1)]
    [TestCase("????.#...#... 4,1,1", 1)]
    [TestCase("????.######..#####. 1,6,5", 4)]
    [TestCase("?###???????? 3,2,1", 10)]
    public void It_calculates_correct_number_of_arrangements_for_line(string line, int expectedResult)
    {
        var sln = new Solution();
        var parsedResult = sln.ParseString(line);
        var actualResult = new Solution().Calculate(parsedResult.record, parsedResult.sizesOfDamaged);

        Assert.That(actualResult, Is.EqualTo(actualResult));
    }
}

public class Solution
{
    public long SolvePt1(string input)
    {
        var lines = input.Split(new[] { "\n" }, StringSplitOptions.RemoveEmptyEntries);
        return lines
            .Select(ParseString)
            .Sum(x => Calculate(x.record, x.sizesOfDamaged));
    }

    public long SolvePt2(string input)
    {
        var lines = input.Split(new[] { "\n" }, StringSplitOptions.RemoveEmptyEntries);
        var inputs = lines
            .Select(s =>
            {
                var parsedResult = ParseString(s);
                var springs = string.Join('?', Enumerable.Repeat(parsedResult.record, 5).ToArray());
                var groups = Enumerable.Repeat(parsedResult.sizesOfDamaged, 5).SelectMany(x => x).ToArray();
                return (springs, groups);
            }).ToArray();
        return inputs
            .Select((x, i) =>
            {
                var res = Calculate(x.springs, x.groups);
                Console.WriteLine("Solved result for " + i);
                return res;
            })
            .Sum();
    }


    // .??..??...?##. 1,1,3
    // ???.### 1,1,3
    private static Dictionary<string, long>
        springCombinationsCache = new(); // springCombination - number of possible permutations;

    public long Calculate(string springs, long[] groups)
    {
        var key = $"{springs},{string.Join(',', groups)}";
        if (springCombinationsCache.TryGetValue(key, out long value))
        {
            return value;
        }

        value = GetCount(springs, groups);
        springCombinationsCache[key] = value;
        return value;
    }

    private long GetCount(string springs, long[] groups)
    {
        while (true)
        {
            if (groups.Length == 0)
            {
                return springs.Contains("#") ? 0 : 1; //
            }

            if (string.IsNullOrEmpty(springs)) //but we still have a group to match
            {
                return 0;
            }

            if (springs.StartsWith('.'))
            {
                springs = springs.TrimStart('.'); // Remove all dots from the beginning
                continue;
            }

            if (springs.StartsWith('?'))
            {
                //calculate different combinations
                return Calculate("." + springs[1..], groups) +
                       Calculate("#" + springs[1..], groups); // Try both options recursively
            }

            if (springs.StartsWith('#')) // Start of a group
            {
                if (groups.Length == 0)
                {
                    return 0; // No more groups to match, although we still have a spring in the input
                }

                if (springs.Length < groups[0])
                {
                    return 0; // Not enough characters to match the group
                }

                if (springs[..(int)groups[0]].Contains('.'))
                {
                    return 0; // Group cannot contain dots for the given length
                }

                if (groups.Length > 1)
                {
                    if (springs.Length < groups[0] + 1 || springs[(int)groups[0]] == '#')
                    {
                        return 0; // Group cannot be followed by a spring, and there must be enough characters left
                    }

                    springs = springs[
                        ((int)groups[0] +
                         1)..]; // Skip the character after the group - it's either a dot or a question mark
                    groups = groups[1..];
                    continue;
                }

                springs = springs[(int)groups[0]..]; // Last group, no need to check the character after the group
                groups = groups[1..];
                continue;
            }
        }
    }

    public (string record, long[] sizesOfDamaged) ParseString(string line)
    {
        var strs = line.Split(' ');
        var record = strs[0];
        var sizes = strs[1].Split(',').Select(long.Parse).ToArray();
        return (record, sizes);
    }
}