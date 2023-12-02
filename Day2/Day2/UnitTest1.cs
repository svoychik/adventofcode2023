using FluentAssertions;

namespace Day2;

public enum Color
{
    Red,
    Green,
    Blue
};

public record GameInfo(int GameId, List<CubesReveal> CubeRevealsList);

public record CubesReveal(int Quantity, Color Color);

public record CubesConfiguration(int GreenMaxAmount, int RedMaxAmount, int BlueMaxAmount);

public class Tests
{
    private readonly CubesConfiguration _configuration = new(13, 12, 14);

    [Test]
    public async Task It_solves_part1_for_my_personal_input()
    {
        var sln = new Solution(_configuration);
        var lines = await File.ReadAllLinesAsync($"input1.txt");
        var actualSum = sln.CalculateForPt1(lines);

        actualSum.Should().Be(2632);
        Console.WriteLine($"The result is {actualSum}");
    }

    [Test]
    public async Task It_solves_part2_for_my_personal_input()
    {
        var sln = new Solution(_configuration);
        var lines = await File.ReadAllLinesAsync($"input1.txt");
        var actualSum = sln.CalculateForPt2(lines);

        actualSum.Should().Be(69629);
        Console.WriteLine($"The result is {actualSum}");
    }

    [Test]
    public void It_returns_correct_result_for_main_task()
    {
        var str = new[]
        {
            "Game 1: 3 blue, 4 red; 1 red, 2 green, 6 blue; 2 green",
            "Game 2: 1 blue, 2 green; 3 green, 4 blue, 1 red; 1 green, 1 blue",
            "Game 3: 8 green, 6 blue, 20 red; 5 blue, 4 red, 13 green; 5 green, 1 red",
            "Game 4: 1 green, 3 red, 6 blue; 3 green, 6 red; 3 green, 15 blue, 14 red",
            "Game 5: 6 red, 1 blue, 3 green; 2 blue, 1 red, 2 green"
        };

        var sln = new Solution(_configuration);
        var actualSum = sln.CalculateForPt1(str);

        actualSum.Should().Be(8);
    }

    [TestCase("Game 101: 3 blue, 4 red; 1 red, 2 green, 6 blue; 2 green")]
    public void I_am_able_to_parse_amount_of_cubes_from_line(
        string line)
    {
        var sln = new Solution(_configuration);
        var result = sln.ParseGame(line);

        result.GameId.Should().Be(101);
        result.CubeRevealsList.Should().BeEquivalentTo(new List<CubesReveal>()
        {
            new CubesReveal(3, Color.Blue),
            new CubesReveal(4, Color.Red),
            new CubesReveal(1, Color.Red),
            new CubesReveal(2, Color.Green),
            new CubesReveal(6, Color.Blue),
            new CubesReveal(2, Color.Green),
        });
    }
}

public class Solution
{
    private readonly int _greenMax;
    private readonly int _blueMax;
    private readonly int _redMax;

    public Solution(CubesConfiguration cubesConfiguration)
    {
        _greenMax = cubesConfiguration.GreenMaxAmount;
        _blueMax = cubesConfiguration.BlueMaxAmount;
        _redMax = cubesConfiguration.RedMaxAmount;
    }

    public int CalculateForPt1(string[] str)
    {
        return str
            .Select(GetMaxAmountForCubes)
            .Where(x => x.maxBlueUsed <= _blueMax
                        && x.maxGreenUsed <= _greenMax
                        && x.maxRedUsed <= _redMax
            )
            .Sum(x => x.gameId);
    }

    public int CalculateForPt2(string[] str)
    {
        return str
            .Select(GetMaxAmountForCubes)
            .Select(x => x.maxBlueUsed * x.maxGreenUsed * x.maxRedUsed)
            .Sum();
    }

    public GameInfo ParseGame(string input)
    {
        var gameId = int.Parse(
            input[..input.IndexOf(':')].Replace("Game ", "")
        );

        var gameData = input[(input.IndexOf(':') + 1)..].TrimStart();
        var sets = gameData.Split(';');
        List<CubesReveal> cubeRevealsList = new();
        foreach (var set in sets)
        {
            var cubeColors = set.Split(',');
            cubeRevealsList.AddRange(
                cubeColors
                    .Select(cubeColor => cubeColor.Trim().Split(' '))
                    .Select(details => new CubesReveal(int.Parse(details[0]), Enum.Parse<Color>(details[1], true))));
        }

        return new GameInfo(gameId, cubeRevealsList);
    }

    private (int gameId, int maxBlueUsed, int maxRedUsed, int maxGreenUsed) GetMaxAmountForCubes(string x)
    {
        var game = ParseGame(x);
        var maxGreenUsed = game.CubeRevealsList.Where(c => c.Color == Color.Green).Max(c => c.Quantity);
        var maxBlueUsed = game.CubeRevealsList.Where(c => c.Color == Color.Blue).Max(c => c.Quantity);
        var maxRedUsed = game.CubeRevealsList.Where(c => c.Color == Color.Red).Max(c => c.Quantity);
        return (gameId: game.GameId, maxBlueUsed, maxRedUsed, maxGreenUsed);
    }
}