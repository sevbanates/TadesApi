using System.ComponentModel.DataAnnotations;
using TadesApi.Core.Models.Global;
using TadesApi.Core.Models.ViewModels.AuthManagement;

namespace TadesApi.Models.ViewModels.Client;

public class UpsertClientCommentDto
{
    [Required] [StringLength(2000)] public string Description { get; set; }
}

public class CreateClientCommentDto : UpsertClientCommentDto
{
    [Required] [Range(1, long.MaxValue)] public long ClientId { get; set; }

    [Required] public Guid ClientGuid { get; set; }
}

public class UpdateClientCommentDto : UpsertClientCommentDto, IBaseUpdateModel
{
    [Required] public Guid GuidId { get; set; }
}

public class ClientCommentDto
{
    public long Id { get; set; }
    public long ClientId { get; set; }
    public string Description { get; set; }
    public Guid GuidId { get; set; }
    public DateTime CreDate { get; set; }

    public UserBasicDto CommentedBy { get; set; }
}