using System;
using Microsoft.Azure.Cosmos;
using System.Threading.Tasks; // Required for async/await

namespace ConsoleApp1;

public class Product
{
    public string id { get; set; } 
    public string Name { get; set; }
    public double Price { get; set; }
    public string[] Tags { get; set; }
}

public class Program // Added a Program class to contain Main and helper methods
{
    // Declare Cosmos DB connection details as static readonly fields
    // IMPORTANT: Replace these with your actual Cosmos DB values from the Azure portal
    private static readonly string EndpointUri = "xxxxx"; // Replace with your Cosmos DB endpoint
    private static readonly string PrimaryKey = "xxxxx"; // Replace with your actual primary key (a long base64 string)
    private static readonly string DatabaseId = "xxxx";
    private static readonly string ContainerId = "xxx";

    // 1. Make Main method async to allow await calls
    public static async Task Main(string[] args)
    {
        Console.WriteLine("Starting Cosmos DB product save example...");

        // Create a new instance of the Product class
        var saddle = new Product
        {
            id = "57",
            // CategoryId = "bike-accessories", // This will be the partition key value
            Name = "Saddle",
            Price = 29.99,
            Tags = new string[] { "comfortable", "durable", "black" }
        };


        using CosmosClient client = new CosmosClient(EndpointUri, PrimaryKey);
        Console.WriteLine("***05");
        // Get a reference to the database and container
        Database database = await client.CreateDatabaseIfNotExistsAsync(DatabaseId);
        Console.WriteLine("***********************************************************10");
        Container container = await database.CreateContainerIfNotExistsAsync(ContainerId, "/id"); // Ensure partition key path matches your Product class property
        Console.WriteLine("***********************************************************11");
        // Create the item in the container, specifying the partition key
        // The PartitionKey constructor expects the value of the partition key property for the item.

        string id = "57";
        PartitionKey partitonKey =  new(id);
        
        Console.WriteLine(partitonKey.ToString());
        Console.WriteLine(id);
        Product saddleReturn = await container.ReadItemAsync<Product>(id, new PartitionKey(id));

        
      //  Console.WriteLine("\n");
      Console.WriteLine(saddleReturn.Name);
        // Console.ReadKey(); // Keep console open until a key is pressed
    }

 
}
