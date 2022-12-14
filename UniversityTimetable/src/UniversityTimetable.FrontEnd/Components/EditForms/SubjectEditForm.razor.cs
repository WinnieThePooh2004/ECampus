using UniversityTimetable.FrontEnd.Components.DataSelectors;

namespace UniversityTimetable.FrontEnd.Components.EditForms
{
    public partial class SubjectEditForm
    {
        private void OnTeacherAdd(TeacherDTO teacher)
        {
            Model.Teachers.Add(teacher);
            StateHasChanged();
        }

        private void OnTeacherRemove(TeacherDTO teacher)
        {
            Model.Teachers.Remove(Model.Teachers.FirstOrDefault(t => t.Id == teacher.Id));
            StateHasChanged();
        }
    }
}
