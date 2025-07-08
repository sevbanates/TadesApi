using TadesApi.Db.Entities;
using TadesApi.Core;
using TadesApi.Db.PartialEntites;

namespace TadesApi.Db.Entities
{
    public class Library : BaseEntity, IAuditable, ISoftDeletable
    {
        public long UserId { get; set; }

        public string Title { get; set; }
        public string Description { get; set; }
        public string Category { get; set; }

        public string VideoUrl { get; set; }
        public string FileName { get; set; }
        public string ContentType { get; set; }

        public long CreUser { get; set; }
        public DateTime? ModDate { get; set; }
        public long? ModUser { get; set; }
        public bool IsDeleted { get; set; }
        
        public virtual User User { get; set; }
        
    }
}