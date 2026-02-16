using Microsoft.AspNetCore.Mvc;
using MyApp; //IP
using Server.Services;// IRecordService

namespace Server.Controllers
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


    public class RecordsController : Controller
    {

        private readonly IRecordService _service;


        public RecordsController(IRecordService service)
        {
            _service = service;
        }

        // GET: /Records/Index
        [HttpGet("")]
        public IActionResult Index()
        {
            // first load: no data yet (or you can load by default)
            return View(model: new List<DbRecord>());
        }

        // POST: /Records/GetRecords
        [HttpPost("GetRecords")]
        public IActionResult GetRecords(string ip = "", string hostname = "", string lastLoggedUser = "")
        {


            var data = _service.GetAll();

            return View("Index", data); // render same page with data
        }

        // POST: /Records/Searcg
        [HttpPost]
        public IActionResult Search(SearchRequest request)
        {
            var results = _service.Search(request);
            return View("Index", results);
        }
    }
}
