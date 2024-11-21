using FunctionApp3.Services.Abstracts;
using FunctionApp3.Services.Concretes;
using Microsoft.Azure.Functions.Worker.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = FunctionsApplication.CreateBuilder(args);

builder.ConfigureFunctionsWebApplication();

// Application Insights isn't enabled by default. See https://aka.ms/AAt8mw4.
// builder.Services
//     .AddApplicationInsightsTelemetryWorkerService()
//     .ConfigureFunctionsApplicationInsights();
var connectionString = "";
builder.Services.AddSingleton<IQueueService>(sp => new QueueService(connectionString, "timerqueue"));

builder.Build().Run();
