using System;
using System.IO;
using System.Linq;
using TadesApi.BusinessService.Common.Interfaces;
using TadesApi.Core;
using TadesApi.Core.Models.ConstantKeys;
using TadesApi.Core.Models.Global;
using TadesApi.CoreHelper;
using TadesApi.Db;
using TadesApi.Db.Entities;
using TadesApi.Db.Infrastructure;
using static TadesApi.Core.Models.RolesHelper;

namespace TadesApi.BusinessService.Common.Services;

public class CommonService : ICommonService
{
    private readonly BtcDbContext _dbContext;
    private readonly IRepository<User> _userRepository;

    public CommonService(
        BtcDbContext dbContext,
        IRepository<User> userRepository)
    {
        _dbContext = dbContext;
        _userRepository = userRepository;
    }


    public ActionResponse<TextIntValueDto> GetRoles()
    {
        var toReturn = _dbContext.SysRole
            .Where(x => x.Id != RolesConstantsInt._Admin)
            .Select(x => new TextIntValueDto { Value = x.Id, Text = x.RoleName })
            .ToList();

        return new ActionResponse<TextIntValueDto> { EntityList = toReturn };
    }

    public ActionResponse<TextIntValueDto> GetRolesForAction()
    {
        var toReturn = _dbContext.SysRole
            .Where(x => x.Id != RolesConstantsInt._Admin)
            .Select(x => new TextIntValueDto { Value = x.Id, Text = x.RoleName })
            .ToList();

        return new ActionResponse<TextIntValueDto> { EntityList = toReturn };
    }

    public ActionResponse<TextIntValueDto> GetUsers()
    {
        var toReturn = _dbContext.Users
            .Where(x => x.IsDeleted == false && x.IsActive == true)
            .Select(x => new TextIntValueDto { Value = (int)x.Id, Text = x.FirstName + " " + x.LastName })
            .ToList();

        return new ActionResponse<TextIntValueDto> { EntityList = toReturn };
    }

    public ActionResponse<TextIntValueDto> GetConsultants()
    {
        var toReturn = _userRepository.TableNoTracking
            .Where(x => x.Role.Id == RoleConstant.Consultant && x.IsDeleted == false && x.IsActive == true)
            .Select(x => new TextIntValueDto { Value = (int)x.Id, Text = x.FirstName + " " + x.LastName })
            .ToList();

        return new ActionResponse<TextIntValueDto> { EntityList = toReturn };
    }

    public ActionResponse<TextIntValueDto> GetUserById(long userId)
    {
        var user = _userRepository.TableNoTracking.FirstOrDefault(x => x.Id == userId);
        if (user == null)
            return new ActionResponse<TextIntValueDto>().ReturnResponseError("User not found!.");

        return new ActionResponse<TextIntValueDto>
            { Entity = new TextIntValueDto { Value = (int)user.Id, Text = user.FirstName + " " + user.LastName } };
    }

    public ActionResponse<TextIntValueDto> SearchUsers(string searchKey)
    {
        var toReturn = _dbContext.Users
            .Where(x => x.FirstName.Contains(searchKey) || x.LastName.Contains(searchKey))
            .Select(x => new TextIntValueDto { Value = (int)x.Id, Text = x.FirstName + " " + x.LastName })
            .ToList();

        return new ActionResponse<TextIntValueDto> { EntityList = toReturn };
    }

    public ActionResponse<byte[]> GetPdfFile(string filePath)
    {
        ActionResponse<byte[]> response = new();

        if (filePath.IsNull())
        {
            return response.ReturnResponseError("Pdf Dosyası Bulunamadı.");
        }

        byte[] fileByte;
        using (MemoryStream ms = new MemoryStream(File.ReadAllBytes(filePath)))
            fileByte = ms.ToArray();

        response.Entity = fileByte;
        return response;
    }
}