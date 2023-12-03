namespace Day3;

public class SolutionPt2
{
    public long Calculate(string str)
    {
        var matrix = StringArrayConverter.ConvertInputToStringArray(str);
        long sumOfGearRatios = 0;

        for (int i = 0; i < matrix.GetLength(0); i++)
        {
            for (int j = 0; j < matrix.GetLength(1); j++)
            {
                if (matrix[i, j] == '*')
                {
                    var adjacentNumbers = GetAdjacentNumbers(matrix, i, j);
                    if (adjacentNumbers.Count == 2)
                    {
                        sumOfGearRatios += adjacentNumbers[0] * adjacentNumbers[1];
                    }
                }
            }
        }

        return sumOfGearRatios;
    }

    private List<long> GetAdjacentNumbers(char[,] matrix, int x, int y)
    {
        int[] dx = { -1, -1, -1, 0, 1, 1, 1, 0 };
        int[] dy = { -1, 0, 1, 1, 1, 0, -1, -1 };
        var numbers = new HashSet<long>();
        var processedNumbers = new HashSet<string>();

        for (int k = 0; k < 8; k++)
        {
            int newX = x + dx[k];
            int newY = y + dy[k];

            if (IsInsideArray(matrix, newX, newY) && char.IsDigit(matrix[newX, newY]))
            {
                var numberStr = ExtractNumber(matrix, newX, newY);

                // Check if the number has been processed before (TODO: not sure)
                if (!processedNumbers.Contains(numberStr))
                {
                    processedNumbers.Add(numberStr);
                    if (long.TryParse(numberStr, out long number))
                    {
                        numbers.Add(number);
                    }
                }
            }

            if (numbers.Count > 2) 
            {
                return new List<long>(); // More than two numbers adjacent to '*', not a valid gear
            }
        }

        return numbers.ToList();
    }

    private static bool IsInsideArray(char[,] matrix, int x, int y)
    {
        return x >= 0 && x < matrix.GetLength(0) && y >= 0 && y < matrix.GetLength(1);
    }

    private string ExtractNumber(char[,] matrix, int x, int y)
    {
        var numberStr = "";
        var startY = y;

        //move left
        while (IsInsideArray(matrix, x, y) && char.IsDigit(matrix[x, y]))
        {
            numberStr = matrix[x, y] + numberStr;
            y -= 1;
        }

        y = startY + 1; 
        //moving right y += 1;
        while (IsInsideArray(matrix, x, y) && char.IsDigit(matrix[x, y]))
        {
            numberStr = numberStr + matrix[x, y];
            y += 1;
        }

        return numberStr;
    }
}