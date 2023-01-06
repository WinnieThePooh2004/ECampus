namespace UniversityTimetable.FrontEnd.Requests.Interfaces;

public interface IClassRequests
{
    Task<Timetable> GroupTimetable(int groupId);
    Task<Timetable> TeacherTimetable(int groupId);
    Task<Timetable> AuditoryTimetable(int groupId);
}