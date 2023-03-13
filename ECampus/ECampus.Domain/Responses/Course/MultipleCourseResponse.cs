namespace ECampus.Domain.Responses.Course;

public class MultipleCourseResponse : IMultipleItemsResponse<Entities.Course>
{
    public int Id { get; set; }
    
    public string Name { get; set; } = string.Empty;
    
    public DateTime StartDate { get; set; } = DateTime.Now;
    
    public DateTime EndDate { get; set; } = DateTime.Now + TimeSpan.FromDays(150);
}