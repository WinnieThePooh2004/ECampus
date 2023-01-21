namespace ECampus.FrontEnd.Pages.Departments;

public partial class Edit
{
    protected override string PageAfterSave => $"/departments/{Model?.FacultyId}";
}