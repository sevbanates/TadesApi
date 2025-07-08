dotnet ef dbcontext scaffold "Data Source=.;Initial Catalog=PanelDb;Persist Security Info=True;User ID=sa;Password=Kinney149;TrustServerCertificate=True;pooling=True;min pool size=0;max pool size=100;MultipleActiveResultSets=True;" Microsoft.EntityFrameworkCore.SqlServer -o Entities --context-dir "Entities\AppDbContext" --no-pluralize -c AppDbContext -f --project TadesApi.Db -s TadesApi.Db




Add-Migration MyMigration3 -context AppDbContext

update-database -context AppDbContext



