using VideoPortalApi.Db;

namespace VideoPortalApi.Infrastructure
{
    public interface IBetechContextProvider
    {
        BtcDbContext GetDbContext(string connectionString);
        BtcDbContext GetDbContextByDb(string copmanyName);
    }
}
