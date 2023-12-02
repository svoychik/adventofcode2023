using System.Text;
using FluentAssertions;

namespace Day2;

/*
 *
 *  Game 1: 3 blue, 4 red; 1 red, 2 green, 6 blue; 2 green | 9blue, 5red, 4 green
    Game 2: 1 blue, 2 green; 3 green, 4 blue, 1 red; 1 green, 1 blue
    Game 3: 8 green, 6 blue, 20 red; 5 blue, 4 red, 13 green; 5 green, 1 red
    Game 4: 1 green, 3 red, 6 blue; 3 green, 6 red; 3 green, 15 blue, 14 red
    Game 5: 6 red, 1 blue, 3 green; 2 blue, 1 red, 2 green
 *
 *
 */
public enum Color
{
    Red,
    Green,
    Blue
};

public record GameInfo(int GameId, List<CubeReveal> CubeRevealsList);

public record CubeReveal(int Quantity, Color Color);

public record CubesAmountConfiguration(int GreenMaxAmount, int RedMaxAmount, int BlueMaxAmount);

public class Tests
{
    private readonly CubesAmountConfiguration _configuration = new(13, 12, 14);

    [Test]
    public async Task It_works_for_my_personal_input()
    {
        var sln = new Solution(_configuration);
        var lines = await File.ReadAllLinesAsync($"input1.txt");
        var actualSum = sln.Calculate(lines);

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
        var actualSum = sln.Calculate(str);

        actualSum.Should().Be(8);
    }

    [TestCase("Game 101: 3 blue, 4 red; 1 red, 2 green, 6 blue; 2 green")]
    public void I_am_able_to_parse_amount_of_cubes_from_line(
        string line)
    {
        var sln = new Solution(_configuration);
        var result = sln.ParseGame(line);
        result.GameId.Should().Be(101);
        result.CubeRevealsList.Should().BeEquivalentTo(new List<CubeReveal>()
        {
            new CubeReveal(3, Color.Blue),
            new CubeReveal(4, Color.Red),
            new CubeReveal(1, Color.Red),
            new CubeReveal(2, Color.Green),
            new CubeReveal(6, Color.Blue),
            new CubeReveal(2, Color.Green),
        });
    }
}

public class Solution
{
    private readonly CubesAmountConfiguration _cubesAmountConfiguration;
    private readonly int _greenMax;
    private readonly int _blueMax;
    private readonly int _redMax;

    public Solution(CubesAmountConfiguration cubesAmountConfiguration)
    {
        _cubesAmountConfiguration = cubesAmountConfiguration;
        _greenMax = cubesAmountConfiguration.GreenMaxAmount;
        _blueMax = cubesAmountConfiguration.BlueMaxAmount;
        _redMax = cubesAmountConfiguration.RedMaxAmount;
    }

    public GameInfo ParseGame(string input)
    {
        List<CubeReveal> cubeRevealsList = new();
        //Game 101:
        var gameIdString = input.Substring(0, input.IndexOf(':'));
        var gameId = int.Parse(gameIdString.Split(' ')[1]);
        var gameData = input.Substring(input.IndexOf(':') + 1).TrimStart();

        var parts = gameData.Split(';');
        foreach (var part in parts)
        {
            // Split each part by comma
            var elements = part.Split(',');
            foreach (var element in elements)
            {
                var details = element.Trim().Split(' ');
                cubeRevealsList.Add(
                    new CubeReveal(int.Parse(details[0]), Enum.Parse<Color>(details[1], true)
                    ));
            }
        }

        return new GameInfo(gameId, cubeRevealsList);
    }


    public int Calculate(string[] str)
    {
        var parsedGame = str
            .Select(ParseGame).ToList();

        var listOfValidGames = new List<int>();
        foreach (var item in parsedGame)
        {
            bool gameValid = true;

            foreach (var cubeReveal in item.CubeRevealsList)
            {
                if (cubeReveal.Color == Color.Blue && cubeReveal.Quantity > _blueMax)
                {
                    gameValid = false;
                }

                if (cubeReveal.Color == Color.Red && cubeReveal.Quantity > _redMax)
                {
                    gameValid = false;
                }

                if (cubeReveal.Color == Color.Green && cubeReveal.Quantity > _greenMax)
                {
                    gameValid = false;
                }
            }
            if(gameValid)
                listOfValidGames.Add(item.GameId);
            
            // var greenCubes = item.CubeRevealsList.Any(x => x.Color == Color.Green).Sum(x => x.Quantity);
            // var redCubes = item.CubeRevealsList.Where(x => x.Color == Color.Red).Sum(x => x.Quantity);
            // var blueCubes = item.CubeRevealsList.Where(x => x.Color == Color.Blue).Sum(x => x.Quantity);
            // if (greenCubes > _greenMax || redCubes > _redMax || blueCubes > _blueMax)
            //     continue;
            // else
            //     listOfValidGames.Add(item.GameId);
        }

        return listOfValidGames.Sum();
    }
}