//using TadesApi.BusinessService;
//using TadesApi.BusinessService.AppServices;
//using TadesApi.Core;
//using TadesApi.CoreHelper;
//using TadesApi.Core.Models.Global;
//using TadesApi.Core.Models.Security;
//using TadesApi.Portal.ActionFilters;
//using Microsoft.AspNetCore.Mvc;

//namespace TadesApi.Portal.Helpers
//{
//    public abstract class ExtendGenericController<TEntity, TDataModel, TEntityModel> : BaseController
//           where TEntity : BaseEntity
//           where TDataModel : DataListViewModelBase
//           where TEntityModel : BaseModel
//    {
//        IExtendBusinessService< TEntityModel, TDataModel> _service;
//        private ISecurityService _securityService;

//        public abstract string ContollerName { get; }

//        public abstract ActionResponse<InquirySettings> InquirySettings();
//        //public abstract ActionResponse<MaintenanceSettings> MaintenanceSettings();

//        public ExtendGenericController(IExtendBusinessService<TEntityModel, TDataModel> service, ISecurityService securityService)
//        {
//            _service = service;
//            _securityService = securityService;
//        }

//        [SecurityState(AppConstants.DefActions._Show)]
//        [HttpGet]
//        [Route("OpenDefaultEntity")]
//        public ActionResponse<TEntityModel> OpenDefaultEntity()
//        {
//            var response = new ActionResponse<TEntityModel>();

//            try
//            {
//                response = _service.OpenDefaultEntity();
//                response.Token = _appSecurity.Token; ;

//                if (response.ReturnStatus)
//                {
//                    //response.Entity..isLoaded = true;
//                }

//                return response;
//            }
//            catch (Exception ex)
//            {
//                response.ReturnStatus = false;
//                response.ReturnMessage.Add(ex.Message);
//                return response;
//            }
//        }

//        [SecurityState(AppConstants.DefActions._List)]
//        [HttpPost]
//        [Route("GetEntityPage")]
//        public PagingResponse<TEntityModel> GetEntityPage([FromBody] TDataModel entityViewModel)
//        {
//            //_logger.LogInformation("{UserName}{Message}", _appSecurity.UserName, "Period Getentitypage");
//            var response = new PagingResponse<TEntityModel>();
//            try
//            {
//                response = _service.GetEntityPage(entityViewModel);
//                response.Token = _appSecurity.Token;

//                return response;
//            }
//            catch (Exception ex)
//            {
//                response.ReturnStatus = false;
//                response.ReturnMessage.Add(ex.Message);
//                return response;
//            }

//        }

//        [SecurityState(AppConstants.DefActions._Show)]
//        [HttpGet]
//        [Route("GetEntityById")]
//        public ActionResponse<TEntityModel> GetEntityById()
//        {
//            var response = new ActionResponse<TEntityModel>();

//            long keyVal = HttpContext.Request.Query["Id"].ToString().ToLong();

//            try
//            {
//                response = _service.GetEntityById(keyVal);
//                response.Token = _appSecurity.Token; ;

//                if (response.ReturnStatus)
//                {
//                    //response.Entity..isLoaded = true;
//                }

//                return response;
//            }
//            catch (Exception ex)
//            {
//                response.ReturnStatus = false;
//                response.ReturnMessage.Add(ex.Message);
//                return response;
//            }
//        }

//        [SecurityState(AppConstants.DefActions._Show)]
//        [HttpPost]
//        [Route("GetEntityByViewModel")]
//        public ActionResponse<TEntityModel> GetEntityByViewModel(TEntityModel entityViewModel)
//        {
//            var response = new ActionResponse<TEntityModel>();

//            try
//            {
//                response = _service.GetEntityByViewModel(entityViewModel);
//                response.Token = _appSecurity.Token; ;

//                if (response.ReturnStatus)
//                {
//                    //response.Entity..isLoaded = true;
//                }

//                return response;
//            }
//            catch (Exception ex)
//            {
//                response.ReturnStatus = false;
//                response.ReturnMessage.Add(ex.Message);
//                return response;
//            }
//        }

//        [SecurityState(AppConstants.DefActions._Save)]
//        [HttpPost]
//        [Route("InsertEntity")]
//        public ActionResponse<TEntityModel> InsertEntity([FromBody] TEntityModel entityViewModel)
//        {
//            var response = new ActionResponse<TEntityModel>();

//            try
//            {
//                response = _service.InsertEntity(entityViewModel);
//                response.Token = _appSecurity.Token;
//                //if (response.ReturnStatus)
//                    //response.Entity.isLoaded = true;
               
//                    return response;

//            }
//            catch (Exception ex)
//            {
//                response.ReturnStatus = false;
//                response.ReturnMessage.Add(ex.Message);
//                return response;
//            }

//        }
//        [SecurityState(AppConstants.DefActions._Save)]
//        [HttpPost]
//        [Route("UpdateEntity")]
//        public ActionResponse<TEntityModel> UpdateEntity([FromBody] TEntityModel entityViewModel)
//        {
//            var response = new ActionResponse<TEntityModel>();

//            try
//            {
//                response = _service.UpdateEntity(entityViewModel);
//                response.Token = _appSecurity.Token;

//                return response;

//            }
//            catch (Exception ex)
//            {
//                response.ReturnStatus = false;
//                response.ReturnMessage.Add(ex.Message);
//                return response;
//            }

//        }

//        [SecurityState(AppConstants.DefActions._Show)]
//        [HttpGet]
//        [Route("MaintenanceSettings")]
//        public ActionResponse<MaintenanceSettings> MaintenanceSettings()
//        {
//            var response = new ActionResponse<MaintenanceSettings>();
//            try
//            {
//                var respModel = _securityService.SetMaintenanceDefaults<TEntityModel>(ContollerName);
//                response.Entity = respModel;

//                if (respModel.TableName.IsInitial())
//                    return response.ReturnResponseError("Maintenance Table Name Errorr ");

//                if (!respModel.Actions.Any())
//                    return response.ReturnResponseError("Maintenance Settings Actions Cannot be null ");

//                if (respModel.IdentifierField.IsInitial())
//                    return response.ReturnResponseError("Maintenance Settings IdentifierField Cannot be empty ");


//                return response;
//            }
//            catch (Exception ex)
//            {
//                response.ReturnStatus = false;
//                response.ReturnMessage.Add("Maintenance Settings Errorr :" + ex.Message);
//                return response;
//            }


//        }

//        [SecurityState(AppConstants.DefActions._Save)]
//        [HttpGet]
//        [Route("DeleteEntity")]
//        public ActionResponse<bool> DeleteEntity()
//        {
//            var response = new ActionResponse<bool>();
//            long keyVal = HttpContext.Request.Query["Id"].ToString().ToLong();

//            try
//            {
//                response = _service.DeleteEntity(keyVal);
//                response.Token = _appSecurity.Token;
//                return response;

//            }
//            catch (Exception ex)
//            {
//                response.ReturnStatus = false;
//                response.ReturnMessage.Add(ex.Message);
//                return response;
//            }
//        }

//        [HttpGet]
//        [Route("GetInitialData")]
//        public ActionResponse<dynamic> GetInitialData()
//        {
//            var response = new ActionResponse<dynamic>();

//            try
//            {
//                response = _service.GetInitialData();
//                response.Token = _appSecurity.Token; ;

//                if (response.ReturnStatus)
//                {
//                    //response.Entity..isLoaded = true;
//                }

//                return response;
//            }
//            catch (Exception ex)
//            {
//                response.ReturnStatus = false;
//                response.ReturnMessage.Add(ex.Message);
//                return response;
//            }
//        }

//        [HttpPost]
//        [Route("ExcellExport")]
//        public ActionResponse<TEntityModel> ExcellExport([FromBody] TDataModel entityViewModel)
//        {
//            var response = new ActionResponse<TEntityModel>();
//            try
//            {
//                entityViewModel.PageSize = 1000;
//                entityViewModel.CurrentPageNumber = 0;

//                var query = _service.GetEntityPage(entityViewModel);
//                List<TEntityModel> resp = query.EntityList;
//                response.Token = _token;
//                response.EntityList = resp;
//                return response;
//            }
//            catch (Exception ex)
//            {
//                response.ReturnStatus = false;
//                response.ReturnMessage.Add(ex.Message);
//                return response;
//            }

//        }




//    }

//}
