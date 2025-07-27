namespace WebApi.Models.Ruhsat
{
    public class AddWareHousesRequest
    {
        public List<WareHouseDto> Name { get; set; }
    }

    public class WareHouseDto
    {
        public int? Id { get; set; }
        public string Name { get; set; }
        public int RuhsatSinifiId { get; set; }
    }
}
