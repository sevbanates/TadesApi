using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using TadesApi.BusinessService.AppServices;
using TadesApi.BusinessService.LibraryServices.Interfaces;
using TadesApi.Core;
using TadesApi.Core.Session;
using TadesApi.CoreHelper;
using TadesApi.Db.Entities;
using TadesApi.Db.Infrastructure;
using TadesApi.Models.AppMessages;
using TadesApi.Models.ViewModels.Library;
using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using TadesApi.Db.Entities;
using TadesApi.BusinessService._base;
using TadesApi.Core.Helper;
using TadesApi.Core.Models.Global;

namespace TadesApi.BusinessService.LibraryServices.Services
{
    public class LibraryService : BaseServiceNg<Library, LibraryItemDto, CreateLibraryItemDto, UpdateLibraryItemDto>, ILibraryService
    {
        

        public LibraryService(
            IRepository<Library> entityRepository,
            ILocalizationService locManager,
            IMapper mapper,
            ICurrentUser session) : base(entityRepository, locManager, mapper, session)
        {
        }

     

        public new ActionResponse<LibraryItemDto> Create(CreateLibraryItemDto input)
        {
            var toCreate = _mapper.Map<Library>(input);
            toCreate.UserId = _session.UserId;

            if (toCreate.Category == "Document")
            {
                AwsHelper.UploadFileToS3(input.File, toCreate.GuidId.ToString());
                toCreate.FileName = input.File.FileName;
                toCreate.ContentType = input.File.ContentType;
            }

            _entityRepository.Insert(toCreate);
            return new ActionResponse<LibraryItemDto> { Entity = _mapper.Map<LibraryItemDto>(toCreate) };
        }

        public new ActionResponse<LibraryItemDto> Update(long id, UpdateLibraryItemDto input)
        {
            var toUpdate = _entityRepository.GetById(id);
            if (toUpdate == null || toUpdate.GuidId != input.GuidId)
                return new ActionResponse<LibraryItemDto>().ReturnResponseError("Library item not found.");

            _mapper.Map(input, toUpdate);
            if (input.Category == "Document")
            {
                AwsHelper.UploadFileToS3(input.File, toUpdate.GuidId.ToString());
                toUpdate.FileName = input.File.FileName;
                toUpdate.ContentType = input.File.ContentType;
            }

            _entityRepository.Update(toUpdate);
            return new ActionResponse<LibraryItemDto> { Entity = _mapper.Map<LibraryItemDto>(toUpdate) };
        }
    }
}