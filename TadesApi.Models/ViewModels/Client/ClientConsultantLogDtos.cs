using TadesApi.Core.Models.Global;
using TadesApi.Core.Models.ViewModels.AuthManagement;

namespace TadesApi.Models.ViewModels.Client
{
    public class ClientConsultantLogDto : BaseModel
    {
        public Guid GuidId { get; set; }
        public long ClientId { get; set; }
        public long ConsultantId { get; set; }
        public DateTime BegDate { get; set; }
        public DateTime EndDate { get; set; }
    }
    
    public class ClientConsultantLogWithClientAndConsultantDto : ClientConsultantLogDto
    {
        public ClientBasicDto Client { get; set; }
        public UserBasicDto Consultant { get; set; }
    }
}
