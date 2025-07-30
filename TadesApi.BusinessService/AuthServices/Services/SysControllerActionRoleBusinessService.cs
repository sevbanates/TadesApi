using AutoMapper;
using ServiceStack;
using System;
using System.Collections.Generic;
using System.Linq;
using TadesApi.Db.Entities;
using TadesApi.BusinessService.AppServices;
using TadesApi.BusinessService.AuthServices.Interfaces;
using TadesApi.Core;
using TadesApi.Core.Models.CustomModels;
using TadesApi.CoreHelper;
using TadesApi.Db;
using TadesApi.Db.Entities;
using TadesApi.Db.Infrastructure;
using TadesApi.Models.ActionsEnum;
using TadesApi.Models.CustomModels;


namespace TadesApi.BusinessService.AuthServices.Services
{
    public class SysControllerActionRoleBusinessService : ISysControllerActionRoleBusinessService
    {
        protected readonly IRepository<SysControllerActionTotal> _controllerActionTotalRepository;
        protected readonly IRepository<SysControllerActionRole> _entityRepository;
        protected readonly BtcDbContext _dbContext;
        protected readonly ILocalizationService _locManager;
        protected readonly IMapper _mapper;
        public SysControllerActionRoleBusinessService(
                                            IRepository<SysControllerActionRole> entityRepository,
                                            BtcDbContext dbContext,
                                            IRepository<SysControllerActionTotal> controllerActionRepository,
                                            ILocalizationService locManager,
                                            IMapper mapper)
        {
            _controllerActionTotalRepository = controllerActionRepository;
            _entityRepository = entityRepository;
            _dbContext = dbContext;
            _locManager = locManager;
            _mapper = mapper;
        }

        public ActionResponse<ControllerModel> GetControllersByRoleId(int roleId)
        {
            ActionResponse<ControllerModel> response = new();

            var allActionList = _dbContext.SysControllerAction.ToList();

            var roleActions = _dbContext.SysControllerActionTotal.Where(x => x.RoleId == roleId).ToList();

            var controllerNames = AppControllerItems.GetListOfEnum().Select(x=>x.Name).ToList();

            var respList = new List<ControllerModel>();

            foreach (var controllerName in controllerNames)
            {
                ControllerModel model = new();
                model.ControllerName = controllerName;

                var currentList = allActionList.Where(x => x.ControllerName == model.ControllerName).ToList();
                foreach (var action in currentList)
                {
                    var item = new ControllerActionModel()
                    {
                        ActionName = action.ActionName,
                        ActionNo = action.ActionNo,
                        Active = (roleActions.FirstOrDefault(s => s.Controller == action.ControllerName)?.Total & action.ActionNo) == action.ActionNo
                    };

                    model.Actions.Add(item);
                }

                respList.Add(model);
            }

            response.EntityList = respList;

            return response;
        }

        public ActionResponse<bool> SaveControllerActions(ControllerActionSaveModel model)
        {
            ActionResponse<bool> response = new();  

            if (model == null)
                return response.ReturnResponseError("There are no records to process");

            if(string.IsNullOrWhiteSpace(model.ControllerName))
                return response.ReturnResponseError("Controller name is required.");

            if (model.RoleId.IsInitial())
                return response.ReturnResponseError("Role is required.");

            var entityList = new List<SysControllerActionRole>();

            foreach (var item in model.Actions)
            {

                if (string.IsNullOrWhiteSpace(item.ActionName))
                    return response.ReturnResponseError("Action name is required.");

                if (item.ActionNo.IsInitial())
                    return response.ReturnResponseError("Action no is required.");

                var currentRecord = new SysControllerActionRole()
                {
                    ActionName = item.ActionName,
                    Controller = model.ControllerName,
                    ActionNo = item.ActionNo,
                    RoleId = model.RoleId,
                    //CreDate = DateTime.Now
                    
                };

                entityList.Add(currentRecord);
              
            }

            using var transaction = _dbContext.Database.BeginTransaction();
            try
            {
                _entityRepository.ExecuteDelete(x => x.RoleId == model.RoleId && x.Controller == model.ControllerName);
                _entityRepository.Insert(entityList);
                var totalActionEntity = _controllerActionTotalRepository.Table.FirstOrDefault(x => x.Controller == model.ControllerName && x.RoleId == model.RoleId);
                if (totalActionEntity != null)
                {
                    totalActionEntity.Total = model.Actions.Select(s => s.ActionNo).DefaultIfEmpty().Sum();
                    _dbContext.SaveChanges();
                }
                else
                {
                    SysControllerActionTotal newTotalActionRecord = new SysControllerActionTotal()
                    {
                        Controller = model.ControllerName,
                        RoleId = model.RoleId,
                        Total = entityList.Sum(x => x.ActionNo),
                        CreDate = DateTime.Now

                    };
                    _controllerActionTotalRepository.Insert(newTotalActionRecord);
                }
                transaction.Commit();
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                return response.ReturnResponseError("Something went wrong! " + ex.Message);
            }
            response.Entity = true;

            return response;
        }


    }
}
