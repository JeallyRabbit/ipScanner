namespace Server.Models
{

    public enum Orders
        {
            Ip,
            Hostname,
            LastLoggedUser,
            LastFoundDate,
            OperatingSystem,
            SerialNumber,Model,
            ProcGen,
            None
        }
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
        public string? skipEmpty { get; set; }
        //enum here ? - property by which service will sort ?
        public Orders? OrderBy {get;set;}
        public bool? Asc {get; set;}
    }
}
