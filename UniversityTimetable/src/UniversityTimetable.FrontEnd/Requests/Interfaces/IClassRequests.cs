namespace UniversityTimetable.FrontEnd.Requests.Interfaces;

public interface IClassRequests
{
    Task<Timetable> GroupTimetable(int groupId);
    Task<Timetable> TeacherTimetable(int teacherId);
    Task<Timetable> AuditoryTimetable(int auditoryId);
}