using System.Net;
using System.Net.Mail;
using Amazon.SQS;
using ECampus.Messaging;
using ECampus.Messaging.Mailing;
using ECampus.Messaging.MessagingServices;
using ECampus.Messaging.Options;
using MediatR;
using Microsoft.Extensions.Options;
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

builder.Services.Configure<QueueSettings>(builder.Configuration.GetSection(QueueSettings.Key));
builder.Services.Configure<EmailSetting>(builder.Configuration.GetSection(EmailSetting.Key));

builder.Services.AddSingleton<IAmazonSQS, AmazonSQSClient>();
builder.Services.AddSingleton<IEmailSendService, EmailSendService>();
builder.Services.AddSingleton<SmtpClient>(provider =>
{
    var options = provider.GetService<IOptions<EmailSetting>>();
    return new SmtpClient
    {
        Port = 587,
        Credentials = new NetworkCredential { Password = options!.Value.Password, UserName = options.Value.Email },
        EnableSsl = true,
        DeliveryMethod = SmtpDeliveryMethod.Network,
        Host = options.Value.HostName
    };
});

builder.Services.AddMediatR(typeof(MessagingAssemblyMarker));

builder.Services.AddHostedService<ECampusQueueMessageConsumer>();

var app = builder.Build();

app.MapGet("/", () => "Hello World!");

app.Run();