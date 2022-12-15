namespace UniversityTimetable.FrontEnd.Components.EditForms
{
    public partial class TeacherEditForm
    {
        private void AddSubject(SubjectDTO subject)
        {
            Model.Subjects.Add(subject);
            StateHasChanged();
        }

        private void DeleteSubject(SubjectDTO subject)
        {
            Model.Subjects.Remove(Model.Subjects.FirstOrDefault(s => s.Id == subject.Id));
            StateHasChanged();
        }
    }
}
