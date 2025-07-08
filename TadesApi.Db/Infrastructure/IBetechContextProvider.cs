using TadesApi.Db;

namespace TadesApi.Db.Infrastructure
{
    public interface IBetechContextProvider
    {
        BtcDbContext GetDbContext(string connectionString);
        BtcDbContext GetDbContextByDb(string copmanyName);
    }
}
