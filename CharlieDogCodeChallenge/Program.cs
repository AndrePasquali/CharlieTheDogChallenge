using System;
using System.Collections.Generic;
using System.Linq;

public class Playfield
{
    public string? Column { get; set; }
    public string? Line { get; set; }
    public int Horizontal => int.Parse(Column ?? "0");
    public int Vertical => int.Parse(Line ?? "0");
    private string? _location { get; set; }
    public string Location
    {
        get => _location ?? "O";

        set
        {
            _location = value;

            switch (value)
            {
                case "C": LocationType = GridLocationType.Dog; break;
                case "F": LocationType = GridLocationType.Food; break;
                case "H": LocationType = GridLocationType.House; break;
                default: LocationType = GridLocationType.Default; break;
            }

        }

    }
    public GridLocationType LocationType { get; set; }
}

public enum GridLocationType
{
    Default,
    Food,
    House,
    Dog
}

class MainClass
{

    public static string SearchingChallenge(string[] stringArray)
    {
        var numberOfSteps = GetSteps(stringArray).ToString();

        return numberOfSteps;
    }

    public static int CalculateSteps(int dogLocationX, int dogLocationY, int targetLocationX, int targetLocationY)
    {

        if (dogLocationX == targetLocationX || dogLocationY == targetLocationY) return (int)Math.Sqrt(Math.Pow(targetLocationX - dogLocationX, 2) + Math.Pow(targetLocationY - dogLocationY, 2));

        var horizontalSteps = (int)Math.Abs(targetLocationY - dogLocationY);
        var verticalSteps = (int)Math.Abs(targetLocationX - dogLocationX);

        return horizontalSteps + verticalSteps;

    }

    public static int GetSteps(string[] stringArray)
    {
        var playfieldList = CreatePlayField(stringArray);

        var foods = playfieldList.Where(e => e.LocationType == GridLocationType.Food).ToList();
        var house = playfieldList.FirstOrDefault(e => e.LocationType == GridLocationType.House);
        var dog = playfieldList.FirstOrDefault(e => e.LocationType == GridLocationType.Dog);

        var dogHorizontal = dog?.Horizontal;
        var dogVertical = dog?.Vertical;

        var steps = 0;

        for (var currentFood = 0; currentFood < foods.Count; currentFood++)
        {
            var foodHorizontal = foods[currentFood].Horizontal;
            var foodVertical = foods[currentFood].Vertical;

            steps += CalculateSteps(dogHorizontal ?? 0, dogVertical ?? 0, foodHorizontal, foodVertical);
        }

        steps += CalculateSteps(dogHorizontal ?? 0, dogVertical ?? 0, house?.Horizontal ?? 0, house?.Vertical ?? 0);

        return steps;
    }


    public static List<Playfield> CreatePlayField(string[] stringArray)
    {
        var numberOfLines = 4;
        var numberOfColumns = 4;

        var playfieldList = new List<Playfield>();

        for (var line = 0; line < numberOfLines; line++)
        {
            for (var column = 0; column < numberOfColumns; column++)
            {
                playfieldList.Add(new Playfield()
                {
                    Column = column.ToString(),
                    Line = line.ToString(),
                    Location = stringArray[line][column].ToString()
                });
            }
        }

        return playfieldList;
    }

    public static void Main()
    {       
        Console.WriteLine(SearchingChallenge(new string[] { "OOOC", "OHOF", "OFOO", "OOOO" }));
    }
}
