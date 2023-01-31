using System.Net.Mail;
using ECampus.Messaging.Mailing;
using ECampus.Messaging.MessageHandlers.TaskSubmissions;
using ECampus.Messaging.Messages;
using Serilog;

namespace ECampus.Tests.Unit.Messaging.MessageHandlers.TaskSubmissions;

public class CourseTaskCreatedHandlerTests
{
    private readonly CourseTaskCreatedHandler _sut;
    private readonly IEmailSendService _emailSendService = Substitute.For<IEmailSendService>();

    public CourseTaskCreatedHandlerTests()
    {
        _sut = new CourseTaskCreatedHandler(_emailSendService, Substitute.For<ILogger>());
    }

    [Fact]
    public async Task Handle_ShouldSendEmail()
    {
        var taskCreated = new TaskCreated
        {
            StudentEmails = new List<string> { "email1", "email2" },
            CourseName = "course name",
            TaskName = "task name",
            MaxPoints = 10,
            Deadline = DateTime.Today,
            TaskType = "exam"
        };

        await _sut.Handle(taskCreated, new CancellationToken());

        await _emailSendService.Received(1).SendEmailAsync(Arg.Is<MailMessage>(message =>
                message.Body == $"<h1>You have a new {taskCreated.TaskType}</h1>" +
                $"<p>A new {taskCreated.TaskType} '{taskCreated.TaskName}'" +
                $" was recently added to course {taskCreated.CourseName}</p>" +
                $"<p>Max points is: {taskCreated.MaxPoints}" +
                $"Task is valid until {taskCreated.Deadline:dd/MMMM/y h:m}</p>"
                && message.Subject == $"You have new {taskCreated.TaskType}"),
            Arg.Is<List<string?>>(l => l == taskCreated.StudentEmails));
    }
}