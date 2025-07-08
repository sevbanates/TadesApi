using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using TadesApi.Core.Helper;
using TadesApi.Core.Models.Global;
using TadesApi.Core.Models.ViewModels.AuthManagement;

namespace TadesApi.Models.ViewModels.Library
{
    public class CreateLibraryItemDto
    {
        [Required]
        [StringLength(250)] public string Title { get; set; }

        [StringLength(2000)] public string Description { get; set; }

        [Required] public string Category { get; set; }

        [StringLength(1000)][Url]
        public string VideoUrl { get; set; }
        
        public IFormFile File { get; set; }
    }

    public class UpdateLibraryItemDto : CreateLibraryItemDto, IBaseUpdateModel
    {
        [Required] public Guid GuidId { get; set; }
    }

    public class LibraryItemDto : BaseModel
    {
        public Guid GuidId { get; set; }
        public long UserId { get; set; }

        public string Title { get; set; }
        public string Description { get; set; }
        public string Category { get; set; }

        public string VideoUrl { get; set; }
        public string FileName { get; set; }
        public string ContentType { get; set; }

        public DateTime CreDate { get; set; }
        
        public string ItemUrl => VideoUrl ?? AwsHelper.GetFileUrl(GuidId.ToString());
        public string FileExtension => Path.GetExtension(FileName);
    }
    
    public class LibraryItemWithOwnerDto : LibraryItemDto
    {
        public UserBasicDto User { get; set; }
    }

    public class LibrarySearchInput : PagedAndSortedSearchInput
    {
        public string Category { get; set; }
    }
}