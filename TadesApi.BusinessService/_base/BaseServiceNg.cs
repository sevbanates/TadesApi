using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using TadesApi.BusinessService.AppServices;
using TadesApi.Core;
using TadesApi.Core.Models.Global;
using TadesApi.Core.Session;
using TadesApi.CoreHelper;
using TadesApi.Db.Infrastructure;
using TadesApi.Models.AppMessages;

namespace TadesApi.BusinessService._base
{
    public abstract class BaseServiceNg<TEntity, TViewModel, TCreateModel, TUpdateModel>
        where TEntity : BaseEntity, new()
        where TUpdateModel : IBaseUpdateModel, new()

    {
        protected readonly IRepository<TEntity> _entityRepository;
        private readonly ILocalizationService _locManager;
        protected readonly IMapper _mapper;
        protected readonly ICurrentUser _session;

        protected BaseServiceNg(IRepository<TEntity> entityRepository, ILocalizationService locManager,
            IMapper mapper, ICurrentUser session)
        {
            _entityRepository = entityRepository;
            _locManager = locManager;
            _mapper = mapper;
            _session = session;
        }

        public ActionResponse<TViewModel> GetSingle(string includes = "")
        {
            var entity = _entityRepository.GetEntity(x => x.Id == _session.UserId, null, includes);
            if (entity == null)
                return new ActionResponse<TViewModel>().ReturnResponseError("Entity not found");

            var toReturn = _mapper.Map<TViewModel>(entity);
            return new ActionResponse<TViewModel> { Entity = toReturn };
        }

        public ActionResponse<TViewModel> GetSingle(long id, Guid guidId, string includes = "")
        {
            var entity = _entityRepository.GetEntity(x => x.Id == id, null, includes);
            if (entity == null || entity.GuidId != guidId)
                return new ActionResponse<TViewModel>().ReturnResponseError("Entity not found");

            var toReturn = _mapper.Map<TViewModel>(entity);
            return new ActionResponse<TViewModel> { Entity = toReturn };
        }

        public PagedAndSortedResponse<TViewModel> GetMulti(PagedAndSortedInput input, string includes = "")
        {
            var query = _entityRepository.TableNoTracking;
            var totalCount = query.Count();
            query = includes.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                .Aggregate(query, (current, toInclude) => current.Include(toInclude));

            var toReturn = CommonFunctions.GetPagedAndSortedData(query, input.Limit, input.Page, input.SortDirection, input.SortBy);
            return new PagedAndSortedResponse<TViewModel>
            {
                EntityList = _mapper.Map<List<TViewModel>>(toReturn),
                TotalCount = totalCount
            };
        }


        public ActionResponse<bool> Delete(long id, Guid guidId)
        {
            var toDelete = _entityRepository.TableNoTracking.FirstOrDefault(x => x.Id == id);
            if (toDelete == null || toDelete.GuidId != guidId)
                return new ActionResponse<bool>().ReturnResponseError("Entity not found");

            _entityRepository.Delete(toDelete);
            return new ActionResponse<bool> { Entity = true };
        }

        public ActionResponse<TViewModel> Create(TCreateModel input)
        {
            var toCreate = _mapper.Map<TEntity>(input);
            _entityRepository.Insert(toCreate);
            return new ActionResponse<TViewModel> { Entity = _mapper.Map<TViewModel>(toCreate) };
        }

        public ActionResponse<TViewModel> Update(long id, TUpdateModel input)
        {
            var toUpdate = _entityRepository.Table.FirstOrDefault(x => x.Id == id);
            if (toUpdate == null || toUpdate.GuidId != input.GuidId)
                return new ActionResponse<TViewModel>().ReturnResponseError("Entity not found");

            _mapper.Map(input, toUpdate);
            _entityRepository.Update(toUpdate);
            return new ActionResponse<TViewModel> { Entity = _mapper.Map<TViewModel>(toUpdate) };
        }
    }
}