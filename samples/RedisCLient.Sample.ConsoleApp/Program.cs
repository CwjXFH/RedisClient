using Microsoft.Extensions.DependencyInjection;
using RedisClient.Abstractions;
using RedisClient.Models.Options;
using RedisClient.StackExchange.Extensions;

var serviceCollection = new ServiceCollection();
serviceCollection.Configure<RedisOptions>(opt =>
{
    opt.Host = "localhost";
    opt.Port = 6379;
});
serviceCollection.AddRedisClient();


using var serviceProvider = serviceCollection.BuildServiceProvider();
var basicOperator = serviceProvider.GetRequiredService<IRedisBasicOperator>();

await basicOperator.StringOperator.SetAsync("key1", "val", TimeSpan.FromMilliseconds(10_900));
var result = await basicOperator.KeyOperator.PTTLAsync("key1");

Console.WriteLine();
