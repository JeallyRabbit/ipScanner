namespace Server.Models
{
    public class RecordsPageModel
    {
        //public string Ip { get; set; }
        //public string Hostname { get; set; }

        public SearchRequest Request { get; set; } = new();
        public List<MyApp.DbRecord> Results { get; set; } = new();

    }
}


