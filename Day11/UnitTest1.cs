namespace Day11;

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
        var actualResult = new Solution().SolvePt(_personalInput);

        Assert.That(actualResult, Is.EqualTo(9556712));
    }

    [Test]
    public void It_solves_part2_for_my_personal_input()
    {
        var actualResult = new Solution().SolvePt(_personalInput, 999999);

        Assert.That(actualResult, Is.EqualTo(678626199476));
    }

    [Test]
    public void It_returns_correct_result_for_pt1_of_example()
    {
        var actualSum = new Solution().SolvePt(_exampleInput);

        Assert.That(actualSum, Is.EqualTo(374));
    }

    [Test]
    public void It_returns_correct_result_for_pt2_of_example()
    {
        var actualSum = new Solution().SolvePt(_exampleInput, 999999);

        Assert.That(actualSum, Is.EqualTo(82000210));
    }
}

public class Solution
{
    public record Point(long X, long Y);

    //For Pt it should be (Million - 1) since it's stated that 
    //and each empty column should be replaced with 1000000 empty columns.
    public long SolvePt(string personalInput, int expandedDistance = 1)
    {
        var arr = personalInput.Split(new[] { "\n" }, StringSplitOptions.RemoveEmptyEntries);
        var galaxies = new List<Point>();
        var isColumnsEmptyArr = new bool[arr[0].Length];
        for (int i = 0; i < isColumnsEmptyArr.Length; i++)
            isColumnsEmptyArr[i] = true;

        var isRowsEmptyArr = new bool[arr.Length];
        for (int i = 0; i < isRowsEmptyArr.Length; i++)
            isRowsEmptyArr[i] = true;

        for (int col = 0; col < arr.Length; col++)
        {
            var line = arr[col];
            for (int row = 0; row < line.Length; row++)
            {
                var ch = arr[col][row];
                if (ch.Equals('#'))
                {
                    isColumnsEmptyArr[col] = false;
                    isRowsEmptyArr[row] = false;
                    galaxies.Add(new Point(col, row));
                }
            }
        }

        var uniquePairs = new HashSet<(Point start, Point end)>();
        for (int i = 0; i < galaxies.Count - 1; i++)
        {
            var el1 = galaxies[i];
            for (int j = i + 1; j < galaxies.Count; j++)
            {
                var el2 = galaxies[j];
                uniquePairs.Add((el1, el2));
            }
        }

        var priceForEachPair = new List<(Point start, Point end, long distance)>();
        foreach (var pair in uniquePairs)
        {
            //check whether we have points between (X1; X2) & (Y1; Y2);
            var countOfRowsWithoutGalaxies = GenerateRange(pair.start.X, pair.end.X)
                .LongCount(currX => isColumnsEmptyArr[currX]);

            var countOfColumnsWithoutGalaxies = GenerateRange(pair.start.Y, pair.end.Y)
                .LongCount(currY => isRowsEmptyArr[currY]);

            var simpleDistance = Math.Abs(pair.end.X - pair.start.X) + Math.Abs(pair.end.Y - pair.start.Y);

            long distance = simpleDistance +
                            expandedDistance * (countOfRowsWithoutGalaxies + countOfColumnsWithoutGalaxies);

            priceForEachPair.Add((pair.start, pair.end, distance));
        }

        return priceForEachPair.Sum(x => x.distance);
    }

    private static IEnumerable<long> GenerateRange(long start, long end)
    {
        if (start <= end)
        {
            for (var i = start + 1; i < end; i++)
            {
                yield return i;
            }
        }
        else
        {
            for (var i = start - 1; i > end; i--)
            {
                yield return i;
            }
        }
    }


    public int SolvePt2(string exampleInput)
    {
        throw new NotImplementedException();
    }
}