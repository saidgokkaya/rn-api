namespace WebApi.Models.Log
{
    public class Logs
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string UserEmail { get; set; }
        public string ModuleName { get; set; }
        public string ProcessName { get; set; }
        public string BaseModule { get; set; }
        public DateTime? InsertedDate { get; set; }
    }

    public class LogExportRequest
    {
        public List<int> LogIds { get; set; }
    }
}
