namespace WebApi.Models.Numarataj
{
    public class AddMahallesRequest
    {
        public List<MahalleDto> Name { get; set; }
    }

    public class MahalleDto
    {
        public int? Id { get; set; }
        public string Name { get; set; }
    }
}
