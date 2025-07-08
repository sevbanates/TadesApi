using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VideoPortalApi.DbMigrator.Migrations
{
    /// <inheritdoc />
    public partial class MigrationResul000 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AppActions",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ControllerName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ActionName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MenuCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PageLocation = table.Column<int>(type: "int", nullable: true),
                    RefActionName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDef = table.Column<bool>(type: "bit", nullable: true),
                    IsFree = table.Column<bool>(type: "bit", nullable: true),
                    ActionLocation = table.Column<int>(type: "int", nullable: true),
                    BtnClass = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BtnLabel = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Icon = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsRequiredDescr = table.Column<bool>(type: "bit", nullable: true),
                    IsDialog = table.Column<bool>(type: "bit", nullable: true),
                    IsConfirm = table.Column<bool>(type: "bit", nullable: true),
                    Query = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OrderNo = table.Column<int>(type: "int", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppActions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AppJobs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    JobName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    JobCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreUser = table.Column<long>(type: "bigint", nullable: true),
                    ComplatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Retry = table.Column<int>(type: "int", nullable: true),
                    Payload = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsSuccess = table.Column<bool>(type: "bit", nullable: true),
                    TenantId = table.Column<int>(type: "int", nullable: false),
                    ProcessDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ErrMessage = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppJobs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AppList",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Mcode = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: true),
                    Code = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    Name = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    Order = table.Column<int>(type: "int", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppList", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AppNumarator",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Prefix = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ValueLength = table.Column<int>(type: "int", nullable: false),
                    LastValue = table.Column<long>(type: "bigint", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ListValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppNumarator", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "City",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Code = table.Column<int>(type: "int", maxLength: 3, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_City", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Client",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GuidId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BirthDate = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Grade = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    School = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StudentId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IEPDueDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TriennialDueDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ServiceType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TherapyType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PrimaryDisability = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ServiceDuration = table.Column<int>(type: "int", nullable: false),
                    ServiceFrequency = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ServiceInterval = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MedicaidEligibility = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreUser = table.Column<long>(type: "bigint", nullable: false),
                    ModDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModUser = table.Column<long>(type: "bigint", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Client", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CmmFiles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Owner = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ParentId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TableName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AppCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Category = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Fgrp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Fname = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Fsize = table.Column<int>(type: "int", nullable: false),
                    Ftitle = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CategoryName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TenantId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CmmFiles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CmmLog",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AppCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Owner = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TableName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Text = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RecId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    JsonItem = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CmmLog", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Contract",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Descr = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Contract", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ForgotPassword",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<long>(type: "bigint", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Token = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SendMailDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ChangedPassDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ForgotPassword", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Library",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GuidId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<long>(type: "bigint", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    Description = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    Category = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FilePath = table.Column<string>(type: "nvarchar(1024)", maxLength: 1024, nullable: false),
                    FileName = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    ThumbImg = table.Column<byte[]>(type: "varbinary(max)", nullable: true),
                    CreDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreUser = table.Column<long>(type: "bigint", nullable: false),
                    ModDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModUser = table.Column<long>(type: "bigint", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Library", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PotentialStudent",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ClientName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Gender = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ChronologicalAge = table.Column<int>(type: "int", nullable: false),
                    PrimaryLanguage = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Ethnicity = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PlaceOfBirth = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TodayDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DateOfBirth = table.Column<DateTime>(type: "datetime2", nullable: false),
                    School = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Grade = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    HomeAdress = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    GestationalAge = table.Column<int>(type: "int", nullable: false),
                    BirthWeight = table.Column<int>(type: "int", nullable: false),
                    PostnatalDifficulties = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DevelopmentalMilestones = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MedicalConditions = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PreviousOrCurrentMentalHealth = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SignificantLifeEvents = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DifficultiesImpairmentsVision = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DifficultiesImpairmentsWearsGlasses = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DifficultiesImpairmentsHearing = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DifficultiesImpairmentsSpeechLang = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DifficultiesImpairmentsSleep = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DifficultiesImpairmentsEating = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FamilyHistoryOfMedicalProblems = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TraumaticEvents = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OverallBehaviorAtHome = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BehaviorWithParentsAtHome = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StrengthsInterestHobbies = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AdditionalComments = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OrderAcademicFunc = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OrderExecutiveFunc = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OrderBehavioralFunc = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OrderSosyalEmotioanlFunc = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OrderDevelopmentalFunc = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    City = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    State = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Country = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CountryOfOrigin = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Zip = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    ClientId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PotentialStudent", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SysControllerAction",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ControllerId = table.Column<int>(type: "int", nullable: false),
                    ControllerName = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: false),
                    ActionNo = table.Column<int>(type: "int", unicode: false, maxLength: 6, nullable: false),
                    ActionName = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SysControllerAction", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SysControllerActionRole",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<long>(type: "bigint", nullable: false),
                    ActionName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    ActionNo = table.Column<int>(type: "int", nullable: false),
                    Controller = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SysControllerActionRole", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SysControllerActionTotal",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Total = table.Column<int>(type: "int", nullable: false),
                    Controller = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: false),
                    RoleKey = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SysControllerActionTotal", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SysLanguage",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: false),
                    Culture = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SysLanguage", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SysRole",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleKey = table.Column<int>(type: "int", nullable: false),
                    RoleName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    RoleDescr2 = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SysRole", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SysUserRole",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<long>(type: "bigint", nullable: false),
                    RoleKey = table.Column<int>(type: "int", nullable: false),
                    RoleName = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SysUserRole", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TaxOffice",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Code = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TaxOffice", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GuidId = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWID()"),
                    CompanyId = table.Column<long>(type: "bigint", nullable: false),
                    UserName = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Email = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    PhoneNumber = table.Column<string>(type: "varchar(15)", unicode: false, maxLength: 15, nullable: false),
                    Password = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    PasswordSalt = table.Column<string>(type: "varchar(1024)", unicode: false, maxLength: 1024, nullable: false),
                    RefId = table.Column<long>(type: "bigint", nullable: true),
                    LastIpAddress = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    LastLogintTime = table.Column<DateTime>(type: "datetime", nullable: true),
                    PasswordSalt2 = table.Column<string>(type: "varchar(1024)", unicode: false, maxLength: 1024, nullable: true),
                    ChangePassReq = table.Column<string>(type: "varchar(1)", unicode: false, maxLength: 1, nullable: true),
                    ChangePassDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    ChangePassCode = table.Column<string>(type: "varchar(128)", unicode: false, maxLength: 128, nullable: true),
                    Status = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    UserLogoPath = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsFirstLogin = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "VmenuAction",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<long>(type: "bigint", nullable: false),
                    ActionName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Controller = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Menu = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsChild = table.Column<bool>(type: "bit", nullable: true),
                    Module = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VmenuAction", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ClientComment",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GuidId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ClientId = table.Column<long>(type: "bigint", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: false),
                    CreUser = table.Column<long>(type: "bigint", nullable: false),
                    CreDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModUser = table.Column<long>(type: "bigint", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClientComment", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ClientComment_Client_ClientId",
                        column: x => x.ClientId,
                        principalTable: "Client",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ClientContact",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GuidId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ClientId = table.Column<long>(type: "bigint", nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Proximity = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PhoneType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    City = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    State = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ZipCode = table.Column<int>(type: "int", nullable: false),
                    IsEmergency = table.Column<bool>(type: "bit", nullable: false),
                    Comment = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    CreDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreUser = table.Column<long>(type: "bigint", nullable: false),
                    ModDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModUser = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClientContact", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ClientContact_Client_ClientId",
                        column: x => x.ClientId,
                        principalTable: "Client",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ClientGoal",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GuidId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ClientId = table.Column<long>(type: "bigint", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: false),
                    CreDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreUser = table.Column<long>(type: "bigint", nullable: false),
                    ModDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModUser = table.Column<long>(type: "bigint", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClientGoal", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ClientGoal_Client_ClientId",
                        column: x => x.ClientId,
                        principalTable: "Client",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ClientSession",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GuidId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ClientId = table.Column<long>(type: "bigint", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    StartTime = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Attendance = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Duration = table.Column<int>(type: "int", nullable: false),
                    Accuracy = table.Column<int>(type: "int", nullable: false),
                    Note = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    CreDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreUser = table.Column<long>(type: "bigint", nullable: false),
                    ModDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModUser = table.Column<long>(type: "bigint", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClientSession", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ClientSession_Client_ClientId",
                        column: x => x.ClientId,
                        principalTable: "Client",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PotentialStudentParent",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StudentId = table.Column<long>(type: "bigint", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Age = table.Column<int>(type: "int", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Phone = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Adress = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Occupation = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    HighestGradeCompleted = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    City = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    State = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Country = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CountryOfOrigin = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Zip = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PotentialStudentParent", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PotentialStudentParent_PotentialStudent_StudentId",
                        column: x => x.StudentId,
                        principalTable: "PotentialStudent",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PotentialStudentSbling",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StudentId = table.Column<long>(type: "bigint", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Age = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PotentialStudentSbling", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PotentialStudentSbling_PotentialStudent_StudentId",
                        column: x => x.StudentId,
                        principalTable: "PotentialStudent",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SysStringResource",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ResourceName = table.Column<string>(type: "varchar(1024)", unicode: false, maxLength: 1024, nullable: true),
                    ResourceKey = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Value = table.Column<string>(type: "nvarchar(1024)", maxLength: 1024, nullable: true),
                    SysLanguageId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SysStringResource", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SysStringResource_SysLanguage_SysLanguageId",
                        column: x => x.SysLanguageId,
                        principalTable: "SysLanguage",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ClientAssignment",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GuidId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ClientId = table.Column<long>(type: "bigint", nullable: false),
                    UserId = table.Column<long>(type: "bigint", nullable: false),
                    BegDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Active = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClientAssignment", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ClientAssignment_Client_ClientId",
                        column: x => x.ClientId,
                        principalTable: "Client",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ClientAssignment_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Room",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GuidId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<long>(type: "bigint", nullable: false),
                    RoomInvitationCode = table.Column<string>(type: "nvarchar(8)", maxLength: 8, nullable: false),
                    SendInvitation = table.Column<bool>(type: "bit", nullable: false),
                    CreUser = table.Column<long>(type: "bigint", nullable: false),
                    CreDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModUser = table.Column<long>(type: "bigint", nullable: true),
                    ModDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Room", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Room_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ScheduleEvent",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GuidId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<long>(type: "bigint", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    StartTimeId = table.Column<int>(type: "int", nullable: false),
                    EndTimeId = table.Column<int>(type: "int", nullable: false),
                    IsAllTimeEvent = table.Column<bool>(type: "bit", nullable: false),
                    RepeatsId = table.Column<int>(type: "int", nullable: false),
                    Location = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Description = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    CreDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreUser = table.Column<long>(type: "bigint", nullable: false),
                    ModDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModUser = table.Column<long>(type: "bigint", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ScheduleEvent", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ScheduleEvent_AppList_EndTimeId",
                        column: x => x.EndTimeId,
                        principalTable: "AppList",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ScheduleEvent_AppList_RepeatsId",
                        column: x => x.RepeatsId,
                        principalTable: "AppList",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ScheduleEvent_AppList_StartTimeId",
                        column: x => x.StartTimeId,
                        principalTable: "AppList",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ScheduleEvent_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GoalObjective",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GuidId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ClientGoalId = table.Column<long>(type: "bigint", nullable: false),
                    Objective = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    BaselineAcc = table.Column<int>(type: "int", nullable: false),
                    TargetAcc = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreUser = table.Column<long>(type: "bigint", nullable: false),
                    ModDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModUser = table.Column<long>(type: "bigint", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GoalObjective", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GoalObjective_ClientGoal_ClientGoalId",
                        column: x => x.ClientGoalId,
                        principalTable: "ClientGoal",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "RoomAttendee",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GuidId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RoomId = table.Column<int>(type: "int", nullable: false),
                    AttendeeId = table.Column<long>(type: "bigint", nullable: false),
                    CreUser = table.Column<long>(type: "bigint", nullable: false),
                    CreDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModUser = table.Column<long>(type: "bigint", nullable: true),
                    ModDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoomAttendee", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RoomAttendee_Client_AttendeeId",
                        column: x => x.AttendeeId,
                        principalTable: "Client",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RoomAttendee_Room_RoomId",
                        column: x => x.RoomId,
                        principalTable: "Room",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ClientScheduleEvent",
                columns: table => new
                {
                    AttendeesId = table.Column<long>(type: "bigint", nullable: false),
                    ScheduleEventsId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClientScheduleEvent", x => new { x.AttendeesId, x.ScheduleEventsId });
                    table.ForeignKey(
                        name: "FK_ClientScheduleEvent_Client_AttendeesId",
                        column: x => x.AttendeesId,
                        principalTable: "Client",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ClientScheduleEvent_ScheduleEvent_ScheduleEventsId",
                        column: x => x.ScheduleEventsId,
                        principalTable: "ScheduleEvent",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SessionProgress",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GuidId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ClientSessionId = table.Column<long>(type: "bigint", nullable: false),
                    GoalObjectiveId = table.Column<long>(type: "bigint", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Accuracy = table.Column<int>(type: "int", nullable: false),
                    CreDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreUser = table.Column<long>(type: "bigint", nullable: false),
                    ModDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModUser = table.Column<long>(type: "bigint", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SessionProgress", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SessionProgress_ClientSession_ClientSessionId",
                        column: x => x.ClientSessionId,
                        principalTable: "ClientSession",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SessionProgress_GoalObjective_GoalObjectiveId",
                        column: x => x.GoalObjectiveId,
                        principalTable: "GoalObjective",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AppList_Mcode_Code",
                table: "AppList",
                columns: new[] { "Mcode", "Code" });

            migrationBuilder.CreateIndex(
                name: "IX_City_Name_Code",
                table: "City",
                columns: new[] { "Name", "Code" },
                unique: true,
                filter: "[Name] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_ClientAssignment_ClientId",
                table: "ClientAssignment",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_ClientAssignment_UserId",
                table: "ClientAssignment",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_ClientComment_ClientId",
                table: "ClientComment",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_ClientContact_ClientId",
                table: "ClientContact",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_ClientGoal_ClientId",
                table: "ClientGoal",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_ClientScheduleEvent_ScheduleEventsId",
                table: "ClientScheduleEvent",
                column: "ScheduleEventsId");

            migrationBuilder.CreateIndex(
                name: "IX_ClientSession_ClientId",
                table: "ClientSession",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_GoalObjective_ClientGoalId",
                table: "GoalObjective",
                column: "ClientGoalId");

            migrationBuilder.CreateIndex(
                name: "IX_PotentialStudentParent_StudentId",
                table: "PotentialStudentParent",
                column: "StudentId");

            migrationBuilder.CreateIndex(
                name: "IX_PotentialStudentSbling_StudentId",
                table: "PotentialStudentSbling",
                column: "StudentId");

            migrationBuilder.CreateIndex(
                name: "IX_Room_UserId",
                table: "Room",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_RoomAttendee_AttendeeId",
                table: "RoomAttendee",
                column: "AttendeeId");

            migrationBuilder.CreateIndex(
                name: "IX_RoomAttendee_RoomId",
                table: "RoomAttendee",
                column: "RoomId");

            migrationBuilder.CreateIndex(
                name: "IX_ScheduleEvent_EndTimeId",
                table: "ScheduleEvent",
                column: "EndTimeId");

            migrationBuilder.CreateIndex(
                name: "IX_ScheduleEvent_RepeatsId",
                table: "ScheduleEvent",
                column: "RepeatsId");

            migrationBuilder.CreateIndex(
                name: "IX_ScheduleEvent_StartTimeId",
                table: "ScheduleEvent",
                column: "StartTimeId");

            migrationBuilder.CreateIndex(
                name: "IX_ScheduleEvent_UserId",
                table: "ScheduleEvent",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_SessionProgress_ClientSessionId",
                table: "SessionProgress",
                column: "ClientSessionId");

            migrationBuilder.CreateIndex(
                name: "IX_SessionProgress_GoalObjectiveId",
                table: "SessionProgress",
                column: "GoalObjectiveId");

            migrationBuilder.CreateIndex(
                name: "IX_SysControllerActionRole_RoleId_ActionName_Controller",
                table: "SysControllerActionRole",
                columns: new[] { "RoleId", "ActionName", "Controller" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SysLanguage_Culture",
                table: "SysLanguage",
                column: "Culture",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SysRole_RoleKey",
                table: "SysRole",
                column: "RoleKey",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SysStringResource_SysLanguageId",
                table: "SysStringResource",
                column: "SysLanguageId");

            migrationBuilder.CreateIndex(
                name: "IX_SysUserRole_UserId_RoleKey",
                table: "SysUserRole",
                columns: new[] { "UserId", "RoleKey" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AppActions");

            migrationBuilder.DropTable(
                name: "AppJobs");

            migrationBuilder.DropTable(
                name: "AppNumarator");

            migrationBuilder.DropTable(
                name: "City");

            migrationBuilder.DropTable(
                name: "ClientAssignment");

            migrationBuilder.DropTable(
                name: "ClientComment");

            migrationBuilder.DropTable(
                name: "ClientContact");

            migrationBuilder.DropTable(
                name: "ClientScheduleEvent");

            migrationBuilder.DropTable(
                name: "CmmFiles");

            migrationBuilder.DropTable(
                name: "CmmLog");

            migrationBuilder.DropTable(
                name: "Contract");

            migrationBuilder.DropTable(
                name: "ForgotPassword");

            migrationBuilder.DropTable(
                name: "Library");

            migrationBuilder.DropTable(
                name: "PotentialStudentParent");

            migrationBuilder.DropTable(
                name: "PotentialStudentSbling");

            migrationBuilder.DropTable(
                name: "RoomAttendee");

            migrationBuilder.DropTable(
                name: "SessionProgress");

            migrationBuilder.DropTable(
                name: "SysControllerAction");

            migrationBuilder.DropTable(
                name: "SysControllerActionRole");

            migrationBuilder.DropTable(
                name: "SysControllerActionTotal");

            migrationBuilder.DropTable(
                name: "SysRole");

            migrationBuilder.DropTable(
                name: "SysStringResource");

            migrationBuilder.DropTable(
                name: "SysUserRole");

            migrationBuilder.DropTable(
                name: "TaxOffice");

            migrationBuilder.DropTable(
                name: "VmenuAction");

            migrationBuilder.DropTable(
                name: "ScheduleEvent");

            migrationBuilder.DropTable(
                name: "PotentialStudent");

            migrationBuilder.DropTable(
                name: "Room");

            migrationBuilder.DropTable(
                name: "ClientSession");

            migrationBuilder.DropTable(
                name: "GoalObjective");

            migrationBuilder.DropTable(
                name: "SysLanguage");

            migrationBuilder.DropTable(
                name: "AppList");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "ClientGoal");

            migrationBuilder.DropTable(
                name: "Client");
        }
    }
}
