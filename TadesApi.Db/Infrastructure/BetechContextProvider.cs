
using TadesApi.Core;
using TadesApi.Db;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using TadesApi.Db.Entities.AppDbContext;
using TadesApi.Core.Session;

namespace TadesApi.Db.Infrastructure
{
    public class BetechContextProvider : IBetechContextProvider
    {
        public IConfiguration Configuration;
        private readonly BtcDbContext _context;
        private ICurrentUser _currentUser;

        public BetechContextProvider(IConfiguration configuration, BtcDbContext context, ICurrentUser currentUser)
        {
            Configuration = configuration;
            _context = context;
            _currentUser = currentUser;
        }
        public BtcDbContext GetDbContext(string connection)
        {
            string connectionString = Configuration.GetConnectionString(connection);
            var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
            optionsBuilder.UseSqlServer(connectionString);
            return new BtcDbContext(optionsBuilder.Options,_currentUser);
        }

        public BtcDbContext GetDbContextByDb(string companyName)
        {
            return null;
            //try
            //{
            //    //var companyInfo = _context.CompaniesInfo.Where(c => c.CompanyName == companyName).FirstOrDefault();
            //    if (companyInfo != null)
            //    {
            //        string connectionString = companyInfo.ConnStr;
            //        var optionsBuilder = new DbContextOptionsBuilder<DashboardContext>();
            //        optionsBuilder.UseSqlServer(connectionString);
            //        return new VbtContext(optionsBuilder.Options);
            //    }
            //    return null;
            //}
            //catch (Exception ex)
            //{
            //    return null;
            //}
        }
    }
}
