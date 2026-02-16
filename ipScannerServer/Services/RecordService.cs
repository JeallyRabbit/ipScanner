using MyApp;
using Server.Controllers;

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

        public List<DbRecord> Search(SearchRequest req)
        {
            var query = _context.DbRecords.AsQueryable();

            if (!string.IsNullOrWhiteSpace(req.Ip))
            {
                query = query.Where(x => x.Ip.Contains(req.Ip));
            }
            else if (!string.IsNullOrWhiteSpace(req.Hostname))
            {
                query = query.Where(x => x.Hostname.Contains(req.Hostname));
            }
            else if (req.LastLoggedUser != null)
            {
                query = query.Where(x => x.LastLoggedUser.Contains(req.LastLoggedUser));
            }
            else
            {
                return _context.DbRecords.ToList();
            }

            return query.ToList();
        }

    }
}
