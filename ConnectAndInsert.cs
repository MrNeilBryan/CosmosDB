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
    private static readonly string EndpointUri = "xxxx/"; // Replace with your Cosmos DB endpoint
    private static readonly string PrimaryKey = "xxxx; // Replace with your actual primary key (a long base64 string)
    private static readonly string DatabaseId = "xxxx;
    private static readonly string ContainerId = "xxxx";

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

        try
        {
            Console.WriteLine("***01");
          // 2. Call the static async method
            await SaveProductToCosmosDbAsync(saddle);
            Console.WriteLine("***02");
            Console.WriteLine($"Product '{saddle.Name}' (id: {saddle.id}) saved successfully to Cosmos DB.");
        }
        catch (CosmosException ex)
        {
            Console.WriteLine($"Error saving product to Cosmos DB: {ex.StatusCode} - {ex.Message}");
            Console.WriteLine($"Activity Id: {ex.ActivityId}");
            Console.WriteLine($"Diagnostics: {ex.Diagnostics}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An unexpected error occurred: {ex.Message}");
        }

        Console.WriteLine("\nProgram finished.");
        // Console.ReadKey(); // Keep console open until a key is pressed
    }

    // 3. Make the method static and place it within the Program class
    private static async Task SaveProductToCosmosDbAsync(Product product)
    {
        Console.WriteLine("***********************************************************03");
        // Ensure that EndpointUri and PrimaryKey are correctly set before running
        if (string.IsNullOrEmpty(EndpointUri) || EndpointUri.Contains("your-cosmosdb-account"))
        {
            Console.WriteLine("***********************************************************07");
            throw new InvalidOperationException("Cosmos DB EndpointUri is not set or is still the placeholder. Please update it.");
        }
        if (string.IsNullOrEmpty(PrimaryKey) || PrimaryKey.Contains("YOUR_COSMOS_DB_PRIMARY_KEY"))
        {
            Console.WriteLine("***********************************************************08");
            throw new InvalidOperationException("Cosmos DB PrimaryKey is not set or is still the placeholder. Please update it.");
        }

        // Initialize CosmosClient
        // It's generally recommended to use a Singleton pattern for CosmosClient in real applications
        // to avoid resource exhaustion, but for a simple example, this is fine.
        Console.WriteLine("***********************************************************09");
        Console.WriteLine(EndpointUri);
        Console.WriteLine(PrimaryKey);
        using CosmosClient client = new CosmosClient(EndpointUri, PrimaryKey);
        Console.WriteLine("***05");
        // Get a reference to the database and container
        Database database = await client.CreateDatabaseIfNotExistsAsync(DatabaseId);
        Console.WriteLine("***********************************************************10");
        Container container = await database.CreateContainerIfNotExistsAsync(ContainerId, "/id"); // Ensure partition key path matches your Product class property
        Console.WriteLine("***********************************************************11");
        // Create the item in the container, specifying the partition key
        // The PartitionKey constructor expects the value of the partition key property for the item.
        await container.CreateItemAsync(product, new PartitionKey(product.id));
        Console.WriteLine("***********************************************************12");
    }
}
