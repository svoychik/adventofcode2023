using NUnit.Framework.Constraints;

namespace Day5;

public class Tests
{
    private string _personalInput;
    private string _exampleInput;

    [OneTimeSetUp]
    public void OneTimeSetUp()
    {
        _personalInput = File.ReadAllText($"input.txt");
        _exampleInput = File.ReadAllText($"example.txt");
    }

    [Test]
    public void It_solves_part1_for_my_personal_input()
    {
        var actualResult = new Solution().SolvePt1(_personalInput);


        Console.WriteLine($"The result is {actualResult}");
        Assert.That(actualResult, Is.EqualTo(621354867));
    }

    [Test]
    public void It_solves_part2_for_my_personal_input()
    {
        var actualResult = new Solution().SolvePt2(_personalInput);
        
        Console.WriteLine($"The result is {actualResult}");
        Assert.That(actualResult, Is.EqualTo(46));
    }

    [Test]
    public void It_returns_correct_result_for_pt1_of_example()
    {
        var actualSum = new Solution().SolvePt1(_exampleInput);

        Assert.That(actualSum, Is.EqualTo(35));
    }

    [Test]
    public void It_returns_correct_result_for_pt2_of_example()
    {
        var actualResult = new Solution().SolvePt2(_personalInput);
        
        Console.WriteLine($"The result is {actualResult}");
        Assert.That(actualResult, Is.EqualTo(46));
    }
}

public class Solution
{
    public long SolvePt2(string personalInput)
    {
        var parseResult = ParseInput(personalInput);
        List<long> pipelineInputs = parseResult.Seeds;
        foreach (var maps in parseResult.Pipelines)
        {
            var pipelineOutput = new List<long>();
            foreach (var num in pipelineInputs)
            {
                var map = maps.SingleOrDefault(x => x.Src.Start <= num && num <= x.Src.End);
                if (map == null)
                {
                    pipelineOutput.Add(num);
                }
                else
                {
                    var distanceFromStart = num - map.Src.Start;
                    pipelineOutput.Add(map.Dst.Start + distanceFromStart);
                }
            }
            pipelineInputs = pipelineOutput;
        }
        return pipelineInputs.Min();
    }

    public long SolvePt1(string personalInput)
    {
        var parseResult = ParseInput(personalInput);
        List<long> pipelineInputs = parseResult.Seeds;
        foreach (var maps in parseResult.Pipelines)
        {
            var pipelineOutput = new List<long>();
            foreach (var num in pipelineInputs)
            {
                var map = maps.SingleOrDefault(x => x.Src.Start <= num && num <= x.Src.End);
                if (map == null)
                {
                    pipelineOutput.Add(num);
                }
                else
                {
                    var distanceFromStart = num - map.Src.Start;
                    pipelineOutput.Add(map.Dst.Start + distanceFromStart);
                }
            }
            pipelineInputs = pipelineOutput;
        }
        return pipelineInputs.Min();
    }

    private ParseResult ParseInput(string personalInput)
    {
        var lines = personalInput.Split(new[] { "\n\n" }, StringSplitOptions.RemoveEmptyEntries);
        var seeds = lines[0]
            .Replace("seeds: ", "")
            .Split(' ')
            .Select(long.Parse)
            .ToList();

        var seedsAndRanges = lines[0]
            .Replace("seeds: ", "")
            .Split(' ')
            .Select(long.Parse)
            .ToArray();

        var seedsForPt2 = new List<long>();
        for (var j = 0; j < seedsAndRanges.Length; j += 2)
        {
            var currCount = 1;
            long curr = seedsAndRanges[j];
            long max = seedsAndRanges[j + 1];
            seeds.Add(curr);
            while (currCount < max)
            {
                seedsForPt2.Add(++curr);
            }
        }

        var pipelinesMaps = new List<Map>[7];
        var i = 0;
        foreach (var linesOfMap in lines.Skip(1))
        {
            var vals = linesOfMap
                .Split("\n") //to ski[p the name of a map (e.g 'seed-to-soil map:')
                .Skip(1)
                .Select(
                    rangeStr =>
                    {
                        var strs = rangeStr.Split(' ');
                        var dstStart = long.Parse(strs[0]);
                        var srcStart = long.Parse(strs[1]);
                        var length = long.Parse(strs[2]);
                        var dstRange = new Range(dstStart, dstStart + length - 1);
                        var srcRange = new Range(srcStart, srcStart + length - 1);
                        return new Map(srcRange, dstRange);
                    })
                .ToList();
            pipelinesMaps[i] = vals;
            i++;
        }

        return new ParseResult(seeds, seedsForPt2, pipelinesMaps);
    }

    
}

public record Range(long Start, long End);
public record Map(Range Src, Range Dst);
public record ParseResult(List<long> Seeds, List<long> SeedsForPt2, List<Map>[] Pipelines);