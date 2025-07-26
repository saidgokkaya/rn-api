namespace WebApi.Models.Ruhsat
{
    public class AddClassesRequest
    {
        public List<ClassDto> Name { get; set; }
    }

    public class ClassDto
    {
        public int? Id { get; set; }
        public string Name { get; set; }
        public int RuhsatTuruId { get; set; }
    }
}
