using Microsoft.AspNetCore.Mvc;
using Server.Models;
using Server.Services;// IRecordService

namespace Server.Controllers
{




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
            return View(model: new RecordsPageModel());
        }

        // POST: /Records/GetRecords
        [HttpPost("GetRecords")]
        public IActionResult GetRecords(string ip = "", string hostname = "", string lastLoggedUser = "")
        {


            //var data = _service.GetAll();
            var model = new RecordsPageModel();
            model.Results = _service.GetAll();

            return View("Index", model); // render same page with data
        }

        // POST: /Records/Searcg
        [HttpPost]
        public IActionResult Search(Server.Models.SearchRequest request)
        {
            var vm = new RecordsPageModel
            {
                Request = request,
                Results = _service.Search(request)
            };

            return View("Index", vm);
        }
    }
}
