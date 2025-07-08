using System.ComponentModel.DataAnnotations;
using TadesApi.Core.Helper;
using TadesApi.Core.Models.Global;

namespace TadesApi.Models.ViewModels.Client;

public class ClientFormDto : BaseModel
{
    public Guid GuidId { get; set; }
    public long ClientId { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public bool IsSignatureRequired { get; set; }
    public DateTime CreDate { get; set; }
    public bool IsVisibleToClient { get; set; }
}

public class ClientFormWithDocsDto : ClientFormDto
{
    public bool CanClientEdit { get; set; }
    public List<ClientFormDocDto> Docs { get; set; }
}

public class UpsertClientFormDto
{
    [Required] [StringLength(200)] public string Title { get; set; }

    [Required] [StringLength(2000)] public string Description { get; set; }

    public bool IsSignatureRequired { get; set; }
    
    public bool IsVisibleToClient { get; set; }
}

public class CreateClientFormDto : UpsertClientFormDto
{
    [Required] [Range(1, long.MaxValue)] public long ClientId { get; set; }
    [Required] public Guid ClientGuid { get; set; }
}

public class UpdateClientFormDto : UpsertClientFormDto, IBaseUpdateModel
{
    [Required] public Guid GuidId { get; set; }
}

public class ClientFormDocDto : BaseModel
{
    public Guid GuidId { get; set; }
    public long ClientFormId { get; set; }
    public string FilePath { get; set; }
    public string FileName { get; set; }
    public string ContentType { get; set; }
    public string FileSize { get; set; }

    public string FileUrl => AwsHelper.GetFileUrl(GuidId.ToString());
    public string FileExtension => Path.GetExtension(FileName);
}

public class CreateFormDocDto
{
    [Required] public long FormId { get; set; }

    [Required] public Guid FormGuid { get; set; }
    
    public bool IsVisibleToClient { get; set; }
}