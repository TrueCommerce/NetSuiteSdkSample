using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

using System.Text.Json;

using TrueCommerce.NetSuite;

// If you're not using the Microsoft configuration framework, you'll just need a way to obtain the connection string for the client.
var configuration = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json")
    .Build();

// If you're not using the Microsoft DI framework, you'll need to instantiate NetSuiteSearchClient manually.
// Below is an example of how this client would be instantiated manually.
var netSuiteSearchClient = new NetSuiteSearchClient(new HttpClient(), Options.Create(new NetSuiteOptions()
{
    Account = "<account_id>",
    ConsumerKey = "<consumer_key",
    ConsumerSecret = "<consumer_secret>",
    RestletUrl = "<restlet_url>",
    TokenID = "<token_id>",
    TokenSecret = "<token_secret>"
}));

// The code below is how you'll register the SDK client if using the Microsoft DI framework.
var services = new ServiceCollection()
    .AddNetSuite(configuration.GetConnectionString("NetSuite"))
    .BuildServiceProvider();

var netSuiteSdk = services.GetRequiredService<INetSuiteSdk>();

// We're using Dictionary<string, string> here, but you can use any POCO class as long as the search column names match your property names.
var page = await netSuiteSdk.Search.ExecuteSavedSearch<Dictionary<string, string>>(configuration["NetSuiteSearchID"], pageNumber: 1);

Console.WriteLine(JsonSerializer.Serialize(page.Items));
