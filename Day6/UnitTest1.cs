using System.Text.RegularExpressions;
using NUnit.Framework.Constraints;

namespace Day6;

public class Tests
{
    private int[,] _exampleInput = { { 7, 15, 30 }, { 9, 40, 200 } };
    private int[,] _personalInput = { { 61, 70, 90, 66 }, { 643, 1184, 1362, 1041 } };

    [Test]
    public void It_solves_part1_for_my_personal_input()
    {
        var actualResult = new Solution().SolvePt1(_personalInput);


        Console.WriteLine($"The result is {actualResult}");
        Assert.That(actualResult, Is.EqualTo(621354867));
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
    public int SolvePt1(int[,] arr)
    {
        var countOfPossibleSolutions = new int[arr.GetLength(0)];
        for (int i = 0; i < arr.GetLength(0); i++)
        {
            var recordTime = arr[0, i];
            var recordDistance = arr[1, i];

            // distance = time * speed
            // speed = distance/time
            var minSpeedToAchieve = (int)Math.Ceiling(recordDistance / (float)recordTime);
            var maxSpeedToAchieve = recordDistance / 1; 
            countOfPossibleSolutions[i] = recordTime - minSpeedToAchieve - 1;
        }

        return countOfPossibleSolutions.Aggregate(1, (current, t) => current * t);
    }

    // private int[][] ExtractNumbersFromTheString(string str)
    // {
    //     string pattern = @"\d+";
    //     var i = 0;
    //     var lines = str.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries);
    //     var arr = new int[lines.Length][];
    //     foreach (var line in lines)
    //     {
    //         var j = 0;
    //         var matches = Regex.Matches(line, pattern).ToList();
    //         arr[i] = new int[matches.Count];                     
    //         foreach (Match match in matches)
    //         {
    //             arr[i][j] = int.Parse(match.Value);
    //             j++;
    //         }
    //         i++;
    //     }
    //     return arr;
    // }
}