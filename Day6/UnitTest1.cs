using System.Text.RegularExpressions;
using NUnit.Framework.Constraints;

namespace Day6;

public class Tests
{
    private readonly long[,] _exampleInput = { { 7, 15, 30 }, { 9, 40, 200 } };
    private readonly long[,] _personalInput1 = { { 61, 70, 90, 66 }, { 643, 1184, 1362, 1041 } };
    private readonly long[,] _personalInput2 = { { 61709066 }, { 643118413621041 } };

    [Test]
    public void It_solves_part1_for_my_personal_input()
    {
        var actualResult = new Solution().SolvePt1(_personalInput1);

        Assert.That(actualResult, Is.EqualTo(293046));
    }

    [Test]
    public void It_solves_part2_for_my_personal_input()
    {
        var actualResult = new Solution().SolvePt1(_personalInput2);

        Console.WriteLine(actualResult);
        Assert.That(actualResult, Is.EqualTo(35150181));
    }

    [Test]
    public void It_returns_correct_result_for_pt1_of_example()
    {
        var actualSum = new Solution().SolvePt1(_exampleInput);

        Assert.That(actualSum, Is.EqualTo(288));
    }
}

public class Solution
{
    //TODO: solve this with binary search
    public long SolvePt1(long[,] arr)
    {
        var possibleWinsArr = new long[arr.GetLength(1)];

        for (int i = 0; i < arr.GetLength(1); i++)
        {
            /*
             * Solving equation:
             *  -x^2 + x*recordTime - recordDistance > 0,
             *  where x is the speed
             */
            var (recordTime, recordDistance) = (arr[0, i], arr[1, i]);

            double sqrtD = Math.Sqrt(recordTime * recordTime - 4 * -1 * -recordDistance);
            double x1 = (-recordTime + sqrtD) / (2 * -1);
            double x2 = (-recordTime - sqrtD) / (2 * -1);
            
            long lowerBound = (long)Math.Ceiling(Math.Min(x1, x2));
            long upperBound = (long)Math.Floor(Math.Max(x1, x2));
            long wins = upperBound - lowerBound + 1;   
            
            //solution is in range (x1; x2) but depending on whether integer
            //number give 0 for the equation we might want to exclude them
            if (IsIntegerOutOfTheRange(lowerBound))
                wins--;
            if (IsIntegerOutOfTheRange(upperBound))
                wins--;

            //-x^2 + x*recordTime - recordDistance == 0
            bool IsIntegerOutOfTheRange(long number) => 
                -number * number + number * recordTime - recordDistance == 0;

            possibleWinsArr[i] = wins;
        }

        return possibleWinsArr.Aggregate(1L, (current, t) => current * t);
    }
}