using MyApp;

namespace Server.Services
{
    public interface IRecordService
    {
        public List<DbRecord> GetAll();
        public List<DbRecord> Search(Server.Models.SearchRequest req);
    }
}
