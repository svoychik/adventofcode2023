using static Day3.StringArrayConverter;

namespace Day3;

public static class StringArrayConverter
{
    public static char[,] ConvertInputToStringArray(string input)
    {
        // Split the input into lines
        string[] lines = input.Split(new[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);

        // Create a 2D array with the dimensions based on the input
        char[,] array = new char[lines.Length, lines[0].Length];

        for (int i = 0; i < lines.Length; i++)
        {
            for (int j = 0; j < lines[i].Length; j++)
            {
                array[i, j] = lines[i][j];
            }
        }

        return array;
    }
}

public class Solution
{
    public int Calculate(string str)
    {
        var matrix = ConvertInputToStringArray(str);
        var numbers = new List<int>();

        for (int i = 0; i < matrix.GetLength(0); i++)
        {
            var currentNumberStr = "";
            var currentNumberValid = false;

            for (var j = 0; j < matrix.GetLength(1); j++)
            {
                var el = matrix[i, j];

                if (char.IsDigit(el))
                {
                    currentNumberStr += el;
                    //if it was already considered as valid, we should not calculate anything
                    currentNumberValid = currentNumberValid ? true : IsNumberValid(matrix, i, j);
                }
                else
                {
                    if (currentNumberValid && currentNumberStr != "")
                    {
                        numbers.Add(int.Parse(currentNumberStr));
                    }

                    currentNumberStr = "";
                    currentNumberValid = false;
                }
            }

            // Check for number at the end of the row
            if (currentNumberValid && currentNumberStr != "")
            {
                numbers.Add(int.Parse(currentNumberStr));
            }
        }

        return numbers.Sum();
    }

    private bool IsNumberValid(char[,] matrix, int i, int j)
    {
        int[] dx = { -1, -1, -1, 0, 1, 1, 1, 0 };
        int[] dy = { -1, 0, 1, 1, 1, 0, -1, -1 };

        var set = "1234567890.";
        for (int k = 0; k < 8; k++)
        {
            int newX = i + dx[k];
            int newY = j + dy[k];

            if (newX >= 0 && newX < matrix.GetLength(0) && newY >= 0 && newY < matrix.GetLength(1))
            {
                if (!set.Contains(matrix[newX, newY]))
                {
                    return true;
                }
            }
        }

        return false;
    }
}

public class Tests
{
    private readonly Solution _sln = new();

    [Test]
    public async Task It_solves_part1_for_my_personal_input()
    {
        var text = await File.ReadAllTextAsync($"input.txt");
        var actualSum = _sln.Calculate(text);

        Console.WriteLine($"The result is {actualSum}");
        Assert.That(actualSum, Is.EqualTo(525119));
    }

    [Test]
    public async Task It_solves_part2_for_my_personal_input()
    {
        var text = await File.ReadAllTextAsync($"input.txt");
        var actualSum = _sln.Calculate(text);

        Console.WriteLine($"The result is {actualSum}");
        Assert.That(actualSum, Is.EqualTo(525119));
    }

    [Test]
    public void It_returns_correct_result_for_main_task()
    {
        var str = @"
467..114..
...*......
..35..633.
......#...
617*......
.....+.58.
..592.....
......755.
...$.*....
.664.598..";

        var sln = new Solution();
        var actualSum = sln.Calculate(str);

        Assert.That(4361, Is.EqualTo(actualSum));
    }
}