using System;
using Microsoft.Azure.Cosmos;
using System.Threading.Tasks; // Required for async/await

namespace ConsoleApp1;

public class Cars
{
    public required string Make { get; set; }
    public required string Model { get; set; }
    public int Year { get; set; }
    public double Price { get; set; }
}


public class Program // Added a Program class to contain Main and helper methods
{


    // 1. Make Main method async to allow await calls
    public static void Main(string[] args)
    {
        List<Cars> carList =
        [
            new() { Make = "Tesla", Model = "Model 3", Year = 2022, Price = 39999 },
            new() { Make = "Ford", Model = "Focus", Year = 2019, Price = 16999 },
            new() { Make = "Toyota", Model = "Corolla", Year = 2020, Price = 18999 },
            new() { Make = "BMW", Model = "320i", Year = 2021, Price = 31999 },
            new() { Make = "Tesla", Model = "Model Y", Year = 2023, Price = 45999 }
        ];

        var affordableTeslas = from car in carList
                               where car.Make != "Tesla" && car.Price < 45000
                               orderby car.Year descending
                               select new { car.Make, car.Model, car.Year, car.Price };


        foreach (var car in affordableTeslas)
        {
            Console.WriteLine($"Make: {car.Make}, Model: {car.Model}, Year: {car.Year}, Price: £{car.Price}");
        }
    }
}
