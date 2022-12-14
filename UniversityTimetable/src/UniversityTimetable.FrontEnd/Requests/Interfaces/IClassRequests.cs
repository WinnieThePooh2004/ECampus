namespace UniversityTimetable.FrontEnd.Requests.Interfaces
{
    public interface IClassRequests : IBaseRequests<ClassDTO>
    {
        Task<Timetable> GroupTimetable(int groupId);
        Task<Timetable> TeacherTimetable(int groupId);
        Task<Timetable> AuditoryTimetable(int groupId);
    }
}
