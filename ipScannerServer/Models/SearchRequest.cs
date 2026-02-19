namespace Server.Models
{
    public class SearchRequest
    {
        public string? Ip { get; set; }
        public string? Hostname { get; set; }
        public string? LastLoggedUser { get; set; }
        public DateTimeOffset? LastCheckedDate { get; set; }
        public DateTimeOffset? LastFoundDate { get; set; }
        public DateTimeOffset? LeaseEndDate { get; set; }
        public string? LeaseOwner { get; set; }
        public string? OperatingSystem { get; set; }
        public string? SerialNumber { get; set; }
        public string? Model { get; set; }
        public decimal? ProcGen { get; set; }
    }
}
