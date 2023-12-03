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