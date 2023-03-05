using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;

using KafkaLagMonitor;

var configurationBuilder = new ConfigurationBuilder();
configurationBuilder.RegisterApplicationSettings();
var config = configurationBuilder.Build();
var serviceCollection = new ServiceCollection();
serviceCollection.RegisterConfigs(config);
serviceCollection.AddKafka(config);
serviceCollection.AddAppServices();
serviceCollection.AddLogging(config);

var provider = serviceCollection.BuildServiceProvider();
using var serviceScope = provider.CreateScope();
var scopeServices = serviceScope.ServiceProvider;
var tool = scopeServices.GetRequiredService<LagApplication>();
tool.Run();

