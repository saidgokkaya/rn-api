namespace WebApi.Models.Ruhsat
{
    public class ActivityDto
    {
        public int? Id { get; set; }
        public string Name { get; set; }
    }

    public class AddActivitiesRequest
    {
        public List<ActivityDto> Name { get; set; }
    }
}
