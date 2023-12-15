namespace Day13;

[TestFixture]
public class Tests
{
    private string _exampleInput;
    private string _personalInput;
    private string _examplePt2Input;

    [OneTimeSetUp]
    public void OneTimeSetUp()
    {
        _exampleInput = File.ReadAllText("example.in");
        _personalInput = File.ReadAllText($"input.in");
    }

    [Test]
    public void It_solves_part1_for_my_personal_input()
    {
        var actualResult = new Solution().Solve(_personalInput);

        Assert.That(actualResult, Is.EqualTo(35691));
    }

    [Test]
    public void It_solves_part2_for_my_personal_input()
    {
        var actualResult = new Solution().SolvePt2(_personalInput);

        Assert.That(actualResult, Is.EqualTo(39037));
    }

    [Test]
    public void It_returns_correct_result_for_pt1_of_example()
    {
        var actualSum = new Solution().Solve(_exampleInput);

        Assert.That(actualSum, Is.EqualTo(405));
    }

    [Test]
    public void It_returns_correct_result_for_pt2_of_example()
    {
        var actualSum = new Solution().SolvePt2(_exampleInput);

        Assert.That(actualSum, Is.EqualTo(400));
    }
}

public class Solution
{
    public long Solve(string input)
    {
        string[] matrixesAsStrings = input.Split(new[] { "\n\n" }, StringSplitOptions.RemoveEmptyEntries);
        var matrixes = new List<char[,]>();
        var numberOfColumns = 0l;
        var numberOfRows = 0l;
        foreach (var matrixStr in matrixesAsStrings)
        {
            var lines = matrixStr.Split('\n');
            var grid = new char[lines.Length, lines[0].Length];

            for (int i = 0; i < lines.Length; i++)
            {
                for (int j = 0; j < lines[0].Length; j++)
                {
                    grid[i, j] = lines[i][j];
                }
            }

            matrixes.Add(grid);
        }

        foreach (var matrix in matrixes)
        {
            var currRes = FindSymmetricPositionInColumns(matrix);
            if (currRes == -1)
            {
                var currRes2 = FindSymmetricPositionInRows(matrix);
                if (currRes2 != -1)
                {
                    numberOfRows += currRes2;
                }
            }
            else
            {
                numberOfColumns += currRes;
            }
        }

        var res = numberOfColumns + (numberOfRows * 100);
        return res;
    }

    public long SolvePt2(string input)
    {
        string[] matrixesAsStrings = input.Split(new[] { "\n\n" }, StringSplitOptions.RemoveEmptyEntries);
        var matrixes = new List<char[,]>();
        var numberOfColumns = 0l;
        var numberOfRows = 0l;
        foreach (var matrixStr in matrixesAsStrings)
        {
            var lines = matrixStr.Split('\n');
            var grid = new char[lines.Length, lines[0].Length];

            for (int i = 0; i < lines.Length; i++)
            {
                for (int j = 0; j < lines[0].Length; j++)
                {
                    grid[i, j] = lines[i][j];
                }
            }

            matrixes.Add(grid);
        }

        foreach (var matrix in matrixes)
        {
            var currRes = FindSymmetricPositionInColumnsPt2(matrix);
            if (currRes == -1)
            {
                var currRes2 = FindSymmetricPositionInRowsPt2(matrix);
                if (currRes2 != -1)
                {
                    numberOfRows += currRes2;
                }
            }
            else
            {
                numberOfColumns += currRes;
            }
        }

        var res = numberOfColumns + (numberOfRows * 100);
        return res;

    }

    private static int FindSymmetricPositionInColumns(char[,] arr)
    {
        var w = arr.GetLength(1);
        var h = arr.GetLength(0);
        for (int mirror = 1; mirror < w; mirror++)
        {
            bool isMirrored = true;
            var rightSide = w - mirror;
            var reflectionWidth = Math.Min(mirror, rightSide);
            for (int d = 0; d < reflectionWidth; d++)
            {
                for (int row = 0; row < h; row++)
                {
                    if (arr[row, mirror - 1 - d] != arr[row, mirror + d])
                    {
                        isMirrored = false;
                    }
                }
            }

            if (isMirrored)
            {
                return mirror;
            }
        }

        return -1;
    }

    private int FindSymmetricPositionInRows(char[,] arr)
    {
        var w = arr.GetLength(1);
        var h = arr.GetLength(0);
        for (int mirror = 1; mirror < h; mirror++)
        {
            bool isMirrored = true;
            var rightSide = h - mirror;
            var reflectionWidth = Math.Min(mirror, rightSide);
            for (int d = 0; d < reflectionWidth; d++)
            {
                for (int col = 0; col < w; col++)
                {
                    if (arr[mirror - 1 - d, col] != arr[mirror + d, col])
                    {
                        isMirrored = false;
                    }
                }
            }

            if (isMirrored)
            {
                return mirror;
            }
        }

        return -1;
    }

    private static int FindSymmetricPositionInColumnsPt2(char[,] arr)
    {
        var w = arr.GetLength(1);
        var h = arr.GetLength(0);
        for (int mirror = 1; mirror < w; mirror++)
        {
            int smudges = 0;
            var rightSide = w - mirror;
            var reflectionWidth = Math.Min(mirror, rightSide);
            for (int d = 0; d < reflectionWidth; d++)
            {
                for (int row = 0; row < h; row++)
                {
                    if (arr[row, mirror - 1 - d] != arr[row, mirror + d])
                    {
                        smudges += 1;
                    }
                }
            }

            if (smudges == 1)
            {
                return mirror;
            }
        }

        return -1;
    }

    private int FindSymmetricPositionInRowsPt2(char[,] arr)
    {
        var w = arr.GetLength(1);
        var h = arr.GetLength(0);
        for (int mirror = 1; mirror < h; mirror++)
        {
            int smudges = 0;
            var rightSide = h - mirror;
            var reflectionWidth = Math.Min(mirror, rightSide);
            for (int d = 0; d < reflectionWidth; d++)
            {
                for (int col = 0; col < w; col++)
                {
                    if (arr[mirror - 1 - d, col] != arr[mirror + d, col])
                    {
                        smudges += 1;
                    }
                }
            }

            if (smudges == 1)
            {
                return mirror;
            }
        }

        return -1;
    }

    
}