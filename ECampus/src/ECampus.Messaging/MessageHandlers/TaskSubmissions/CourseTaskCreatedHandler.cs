using System.Net.Mail;
using ECampus.Messaging.Mailing;
using ECampus.Messaging.Messages;
using MediatR;
using ILogger = Serilog.ILogger;

namespace ECampus.Messaging.MessageHandlers.TaskSubmissions;

public class CourseTaskCreatedHandler : IRequestHandler<TaskCreated>
{
    private readonly IEmailSendService _emailSendService;
    private readonly ILogger _logger;

    public CourseTaskCreatedHandler(IEmailSendService emailSendService, ILogger logger)
    {
        _emailSendService = emailSendService;
        _logger = logger;
    }

    public async Task<Unit> Handle(TaskCreated request, CancellationToken cancellationToken)
    {
        Log(request);
        var email = new MailMessage
        {
            Subject = $"You have new {request.TaskType}",
            Body = $"<h1>You have a new {request.TaskType}</h1>" +
                   $"<p>A new {request.TaskType} '{request.TaskName}'" +
                   $" was recently added to course {request.CourseName}</p>" +
                   $"<p>Max points is: {request.MaxPoints}" +
                   $"Task is valid until {request.Deadline:dd/MMMM/y h:m}</p>",
            IsBodyHtml = true
        };
        await _emailSendService.SendEmailAsync(email, request.StudentEmails);
        return Unit.Value;
    }

    private void Log(TaskCreated request)
    {
        _logger.Information("New task created:\n" +
                            "Task name: {Name};\n" +
                            "Course name: {CourseName};\n" +
                            "Deadline: {Deadline};\n" +
                            "Max points: {MaxPoints};\n" +
                            "Type: {Type}",
            request.TaskName, request.CourseName, request.Deadline, request.MaxPoints, request.CourseName);
    }
}