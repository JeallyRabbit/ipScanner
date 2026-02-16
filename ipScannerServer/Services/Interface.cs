using MyApp;
using Server.Controllers;

namespace Server.Services
{
    public interface IRecordService
    {
        List<DbRecord> GetAll();
        public List<DbRecord> Search(SearchRequest req);
    }
}
