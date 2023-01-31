using Amazon.SQS;
using ECampus.Core.Extensions;
using ECampus.Messaging;
using ECampus.Messaging.MessagingServices;
using MediatR;
using Serilog;
using ILogger = Serilog.ILogger;

var builder = WebApplication.CreateBuilder(args);

var logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .WriteTo.Console()
    .CreateLogger();

builder.Logging.ClearProviders();
builder.Logging.AddSerilog(logger);
builder.Services.AddSingleton<ILogger>(logger);

builder.Services.UserInstallersFromAssemblyContaining<MessagingAssemblyMarker>(builder.Configuration);

builder.Services.AddSingleton<IAmazonSQS, AmazonSQSClient>();

builder.Services.AddMediatR(typeof(MessagingAssemblyMarker));

builder.Services.AddHostedService<ECampusQueueMessageConsumer>();

var app = builder.Build();

app.MapGet("/", () => "Hello World!");

app.Run();