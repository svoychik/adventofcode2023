namespace Day3;

public class ImprovedSolution2
{
    public int CalculatePt1(string str)
    {
        var lines = str.Split(Environment.NewLine);
        var (symbols, numbers) = FindNumbersAndSymbols(lines);
        return numbers
            .Where(num => symbols.Any(sym => AreAdjacent(num, sym)))
            .Sum(x => x.Value);
    }

    public int CalculatePt2(string str)
    {
        var lines = str.Split(Environment.NewLine);
        var (symbols, numbers) = FindNumbersAndSymbols(lines);
        return symbols
            .Where(sym => sym.Value.Equals('*'))
            .Select(sym =>
            {
                var adjNumbers = numbers.Where(num => AreAdjacent(num, sym));
                return new
                {
                    AdjacentNumbers = adjNumbers.ToList()
                };
            })
            .Where(x => x.AdjacentNumbers.Count == 2)
            .Sum(x => x.AdjacentNumbers[0].Value * x.AdjacentNumbers[1].Value);
    }

    static bool AreAdjacent(Number number, Symbol symbol)
    {
        var (startX, startY) = (number.Start.X - 1, number.Start.Y - 1);
        var (endX, endY) = (number.End.X + 1, number.End.Y + 1);
        return symbol.Pos.X >= startX && symbol.Pos.X <= endX
            && symbol.Pos.Y >= startY && symbol.Pos.Y <= endY;
    }

    private (List<Symbol> symbols, List<Number> numbers) FindNumbersAndSymbols(string[] lines)
    {
        var symbols = new List<Symbol>();
        var numbers = new List<Number>();

        for (int i = 0; i < lines.Length; i++)
        {
            var currentNumber = new Number();
            var digits = new List<int>();
            for (var j = 0; j < lines[0].Length; j++)
            {
                if (lines[i][j] is '.')
                    continue;
                if (int.TryParse($"{lines[i][j]}", out var digit))
                {
                    digits.Add(digit);
                    if (digits.Count == 1)
                    {
                        currentNumber.Start = (i, j);
                    }

                    while (j + 1 < lines[0].Length && int.TryParse($"{lines[i][j + 1]}", out digit))
                    {
                        digits.Add(digit);
                        j++;
                    }

                    currentNumber = currentNumber with
                    {
                        End = (i, j),
                        Value = int.Parse(string.Join("", digits))
                    };
                    numbers.Add(currentNumber);
                    currentNumber = new Number();
                    digits.Clear();
                }
                else
                {
                    symbols.Add(
                        new Symbol()
                        {
                            Pos = (X: i, Y: j),
                            Value = lines[i][j]
                        }
                    );
                }
            }
        }

        return (symbols, numbers);
    }

}

public record Number
{
    public int Value { get; set; }
    public (int X, int Y) Start { get; set; }
    public (int X, int Y) End { get; set; }
}


public record Symbol
{
    public char Value { get; set; }
    public (int X, int Y) Pos { get; set; }
};