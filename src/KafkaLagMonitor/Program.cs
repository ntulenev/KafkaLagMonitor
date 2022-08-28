using Microsoft.Extensions.DependencyInjection;

using KafkaLagMonitor;

using var serviceScope = HostBuildHelper.CreateHost().Services.CreateScope();
var services = serviceScope.ServiceProvider;
var tool = services.GetRequiredService<LagApplication>();
tool.Run();
