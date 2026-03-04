using MyApp;

namespace Server.Services
{
    public class RecordService : IRecordService
    {

        private readonly AppDbContext _context;

        public RecordService(AppDbContext context)
        {
            _context = context;
        }

        public List<DbRecord> GetAll()
        {
            var x = _context.DbRecords.ToList();
            return _context.DbRecords.ToList();
        }

        public List<DbRecord> Search(Server.Models.SearchRequest req)
        {
            var query = _context.DbRecords.AsQueryable();
            var querySkipEmpty = query; // Dummy value assign
            var anyFilled = false;

            if (!string.IsNullOrWhiteSpace(req.Ip))
            {
                anyFilled = true;
                query = query.Where(x => x.Ip.Contains(req.Ip));
            }
            if (!string.IsNullOrWhiteSpace(req.Hostname))
            {
                anyFilled = true;
                query = query.Where(x => x.Hostname.ToLower().Contains(req.Hostname.ToLower()));
            }
            if (!string.IsNullOrWhiteSpace(req.LastLoggedUser))
            {
                anyFilled = true;
                query = query.Where(x => x.LastLoggedUser.ToLower().Contains(req.LastLoggedUser.ToLower()));
            }
            if (!string.IsNullOrWhiteSpace(req.OperatingSystem))
            {
                anyFilled = true;
                query = query.Where(x => x.OperatingSystem.ToLower().Contains(req.OperatingSystem.ToLower()));
            }
            if (!string.IsNullOrWhiteSpace(req.SerialNumber))
            {
                anyFilled = true;
                query = query.Where(x => x.SerialNumber.ToLower().Contains(req.SerialNumber.ToLower()));
            }
            if (!string.IsNullOrWhiteSpace(req.Model))
            {
                anyFilled = true;
                query = query.Where(x => x.Model.ToLower().Contains(req.Model.ToLower()));
            }

            if (req.skipEmpty == "true")
            {
                querySkipEmpty = query.Where(x => x.LastFoundDate != null);
                return querySkipEmpty.ToList();
            }


            if (anyFilled == false)
            {
                return _context.DbRecords.ToList();
            }


            return query.ToList();
        }

    }
}
