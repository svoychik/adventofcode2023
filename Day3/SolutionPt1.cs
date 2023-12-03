using static Day3.StringArrayConverter;

namespace Day3;

public class SolutionPt1
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
                    if (currentNumberValid) 
                        continue;
                    currentNumberValid = IsNumberValid(matrix, i, j);

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