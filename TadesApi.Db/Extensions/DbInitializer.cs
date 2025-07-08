//using TadesApi.Db.Entities;
//using TadesApi.Db.PartialEntites;
//using TadesApi.Db.SeedData;
//using Microsoft.EntityFrameworkCore;
//using Microsoft.Extensions.DependencyInjection;
//using SharedMessages;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Reflection;
//using System.Text;
//using System.Threading.Tasks;

//namespace TadesApi.Db.Extensions
//{
//    public interface IDbInitializer
//    {
//        /// <summary>
//        /// Applies any pending migrations for the context to the database.
//        /// Will create the database if it does not already exist.
//        /// </summary>
//        void Initialize();

//        /// <summary>
//        /// Adds some default values to the Db
//        /// </summary>
//        void SeedData();
//    }

//    public class DbInitializer : IDbInitializer
//    {
//        private readonly IServiceScopeFactory _scopeFactory;

//        public DbInitializer(IServiceScopeFactory scopeFactory)
//        {
//            this._scopeFactory = scopeFactory;
//        }

//        public void Initialize()
//        {
//            using (var serviceScope = _scopeFactory.CreateScope())
//            {
//                using (var context = serviceScope.ServiceProvider.GetService<BtcDbContext>())
//                {
//                    context.Database.Migrate();
//                }
//            }
//        }

//        public void SeedData()
//        {
//            using (var serviceScope = _scopeFactory.CreateScope())
//            {
//                using (BtcDbContext context = serviceScope.ServiceProvider.GetService<BtcDbContext>())
//                {
//                    //AddSysRole(context);

//                    //if (!context.Users.Any())
//                    //    AddUser(context);

//                    AddSysActionsData(context);
//                    AddSysControllerActionRoleData(context);
//                    AddSysControllerMenuData(context);
//                    AddSysLanguageData(context);
//                    AddSysMenuData(context);
//                    AddSysMenuRoleData(context);
//                    AddSysRole(context);
//                    AddAccountTable(context);
//                    AddCityData(context);
//                    AddControllerData(context);

//                    context.SaveChanges();
//                }
//            }

//        }
//        private void AddSysActionsData(BtcDbContext context)
//        {
//            if (!context.SysActions.Any())
//                context.SysActions.AddRange(SysActionsSeed.GetData());
//        }
//        private void AddSysControllerActionRoleData(BtcDbContext context)
//        {
//            if (!context.SysControllerActionRole.Any())
//                context.SysControllerActionRole.AddRange(SysControllerActionRoleSeed.GetData());
//        }
//        private void AddSysControllerMenuData(BtcDbContext context)
//        {
//            if (!context.SysControllerMenu.Any())
//                context.SysControllerMenu.AddRange(SysControllerMenuSeed.GetData());
//        }
//        private void AddSysLanguageData(BtcDbContext context)
//        {
//            if (!context.SysLanguage.Any())
//            {
//                context.SysLanguage.Add(new SysLanguage { Culture = "en_US", Name = "en_US" });
//                context.SysLanguage.Add(new SysLanguage { Culture = "tr-TR", Name = "tr-TR" });
//            }
//        }
//        private void AddSysMenuData(BtcDbContext context)
//        {
//            if (!context.SysMenu.Any())
//                context.SysMenu.AddRange(SysMenuSeed.GetData());
//        }
//        private void AddSysMenuRoleData(BtcDbContext context)
//        {
//            if (!context.SysMenuRole.Any())
//                context.SysMenuRole.AddRange(SysMenuRoleSeed.GetData());
//        }

//        private void AddSysRole(BtcDbContext context)
//        {
//            if (!context.SysRole.Any())
//            {
//                context.SysRole.Add(new SysRole { RoleKey = 100, RoleName = "Admin", RoleDescr2 = "Admin" });
//                context.SysRole.Add(new SysRole { RoleKey = 101, RoleName = "SuperUser", RoleDescr2 = "SuperUser" });
//            }
//            //*** UserRole
//            if (!context.SysUserRole.Any())
//                context.SysUserRole.Add(new SysUserRole { UserId = 1, RoleKey = 100, RoleName = "Admin" });
//        }
//        private void AddUser(BtcDbContext context)
//        {
//            if (!context.Users.Any())
//            {
//                context.Users.Add(new Users
//                {
//                    //Id = 1,
//                    UserName = "admin",
//                    Email = "admin@betechinnovarion.com",
//                    Status = "A",
//                    PhoneNumber = "0(111) 111-1111",
//                    FirstName = "AppAdmin",
//                    LastName = "AdminSurname",
//                    Password = "f52a814f49800c69ee1caa61840ae2e8586a8bfad4404b81f83df11a830c0348",
//                    PasswordSalt = "5a3f20328c4bf24b90a49d35840cd97b",

//                });
//            }
//        }


//        private void AddAccountTable(BtcDbContext context)
//        {
//            if (!context.Account.Any())
//            {
//                context.Account.Add(new Account { Code = "001", Name = "001 TEST COMPANY" });
//                context.Account.Add(new Account { Code = "500", Name = "500 AAA Company" });
//                context.Account.Add(new Account { Code = "300", Name = "500 BETECH" });
//            }
//        }

//        private void AddCityData(BtcDbContext context)
//        {
//            if (!context.City.Any())
//            {
//                context.City.Add(new City { Code = 1, Name = "Adana" });
//                context.City.Add(new City { Code = 2, Name = "Adıyaman" });
//                context.City.Add(new City { Code = 3, Name = "Afyon" });
//                context.City.Add(new City { Code = 4, Name = "Ağrı" });
//            }
//        }
//        private void AddControllerData(BtcDbContext context)
//        {
//            if (!context.SysController.Any())
//            {
//                context.SysController.Add(new SysController { ControllerName = "SysController", Descr = "Controller Tanımlama", MenuDescr = "Controller Definition", IsDeleted = false });
//                context.SysController.Add(new SysController { ControllerName = "Users", Descr = "Kullanıcılar", MenuDescr = "Kullancı Tanımlama", IsDeleted = false });
//                context.SysController.Add(new SysController { ControllerName = "SysMenu", Descr = "Menü", MenuDescr = "Menü", IsDeleted = false });
//            }
//        }



//    }
//}
