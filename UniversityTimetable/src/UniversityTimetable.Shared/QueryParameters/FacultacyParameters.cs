namespace UniversityTimetable.Shared.QueryParameters
{
    public class FacultacyParameters : QueryParameters
    {
        public string FacultacyName { get; set; } = default!;

        public override string ToString()
        {
            return $"{base.ToString()},\n" +
                $"FacultacyName: {FacultacyName}";
        }
    }
}
