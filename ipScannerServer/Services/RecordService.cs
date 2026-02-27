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

            if (!string.IsNullOrWhiteSpace(req.Ip))
            {
                query = query.Where(x => x.Ip.Contains(req.Ip));
            }
            else if (!string.IsNullOrWhiteSpace(req.Hostname))
            {
                query = query.Where(x => x.Hostname.ToLower().Contains(req.Hostname.ToLower()));
            }
            else if (!string.IsNullOrWhiteSpace(req.LastLoggedUser))
            {
                query = query.Where(x => x.LastLoggedUser.ToLower().Contains(req.LastLoggedUser.ToLower()));
            }
            else
            {
                return _context.DbRecords.ToList();
            }

            if (req.skipEmpty == "true")
            {
                querySkipEmpty = query.Where(x => x.LastFoundDate != null);
                return querySkipEmpty.ToList();
            }
            return query.ToList();
        }

    }
}
