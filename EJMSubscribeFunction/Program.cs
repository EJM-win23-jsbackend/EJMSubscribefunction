using Data.Contexts;
using EJMSubscribeFunction.Services;
using Microsoft.Azure.Functions.Worker;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var host = new HostBuilder()
    .ConfigureFunctionsWebApplication()
    .ConfigureServices(services =>
    {
        services.AddApplicationInsightsTelemetryWorkerService();
        services.ConfigureFunctionsApplicationInsights();
        services.AddScoped<SubscribeServices>();
        services.AddDbContext<DataContext>(x => x.UseSqlServer(Environment.GetEnvironmentVariable("SqlServer")));

    })
    .Build();

host.Run();
