using NUnit.Framework.Constraints;

// ReSharper disable UseWithExpressionToCopyRecord

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
        Assert.That(actualResult, Is.EqualTo(15880236));
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
        var actualResult = new Solution().SolvePt2(_exampleInput);

        Console.WriteLine($"The result is {actualResult}");
        Assert.That(actualResult, Is.EqualTo(46));
    }
}

public class Solution
{
    public long SolvePt2(string personalInput)
    {
        var parseResult = ParseInput(personalInput);
        var ranges = parseResult.SeedsForPt2;
        foreach (var maps in parseResult.Pipelines)
        {
            var input = new Queue<Range>(ranges);
            var output = new List<Range>();

            while (input.Any())
            {
                var range = input.Dequeue();
                var map = maps.FirstOrDefault(src => Intersects(src.Src, range));

                /*
                  1. If map not found - add it to the output
                  2. If range is fully covered by map - add it the output
                3-4. Otherwise chop the range into two depending on the range intersection
                 
                 
                 */
                if (map == null)
                {
                    output.Add(range);
                    continue;
                }

                if (map.Src.Start <= range.Start && range.End <= map.Src.End)
                {
                    var shift = map.Dst.Start - map.Src.Start;
                    output.Add(new Range(range.Start + shift, range.End + shift));
                }
                else
                {
                    // Determine the overlap and non-overlap parts with the map source.
                    long overlapStart = Math.Max(range.Start, map.Src.Start);
                    long overlapEnd = Math.Min(range.End, map.Src.End);

                    // Enqueue the non-overlapping part before the overlap (if it exists).
                    if (overlapStart > range.Start)
                    {
                        input.Enqueue(new Range(range.Start, overlapStart - 1));
                    }

                    // Enqueue the overlapping part.
                    input.Enqueue(new Range(overlapStart, overlapEnd));

                    // Enqueue the non-overlapping part after the overlap (if it exists).
                    if (overlapEnd < range.End)
                    {
                        input.Enqueue(new Range(overlapEnd + 1, range.End));
                    }
                }
            }

            ranges = output;
        }

        return ranges.MinBy(x => x.Start).Start;
    }

    bool Intersects(Range r1, Range r2) => r1.Start <= r2.End && r2.Start <= r1.End;

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

        var seedsRanges = new List<Range>();
        for (var j = 0; j < seedsAndRanges.Length; j += 2)
        {
            long curr = seedsAndRanges[j];
            long times = seedsAndRanges[j + 1];
            seedsRanges.Add(new Range(curr, curr + times - 1));
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

        return new ParseResult(seeds, seedsRanges, pipelinesMaps);
    }
}

public record Range(long Start, long End);

public record Map(Range Src, Range Dst);

public record ParseResult(List<long> Seeds, List<Range> SeedsForPt2, List<Map>[] Pipelines);