using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TadesApi.Core.Models.Global;

namespace TadesApi.Models.ViewModels.Customer
{
    public class CustomerDto
    {
        public long Id { get; set; }
        public Guid GuidId { get; set; }
        [Required] [MaxLength(200)] public string Name { get; set; } = null!;
        [Required] [MaxLength(200)] public string Surname { get; set; } = null!;

        [Required] [MaxLength(11)] public string VknTckn { get; set; } = null!;

        public bool IsCompany { get; set; }

        // İletişim Bilgileri
        [MaxLength(100)] public string? Email { get; set; }

        [MaxLength(20)] public string? Phone { get; set; }

        [MaxLength(20)] public string? MobilePhone { get; set; }

        // Adres Bilgileri
        public int Country { get; set; }

        [MaxLength(100)]
        public string City { get; set; }

        [MaxLength(100)]
        public string District { get; set; }

        [MaxLength(500)] public string? FullAddress { get; set; }

        [MaxLength(100)] public string? BuildingName { get; set; }

        [MaxLength(10)] public string? BuildingNumber { get; set; }

        [MaxLength(10)] public string? FloorNumber { get; set; }

        [MaxLength(10)] public string? DoorNumber { get; set; }

        [MaxLength(10)] public string? PostalCode { get; set; }

        [MaxLength(300)] public string? AddressDescription { get; set; }
    }

    public class CustomerCreateDto
    {
        [Required][MaxLength(200)] public string Name { get; set; } = null!;
        [Required][MaxLength(200)] public string Surname { get; set; } = null!;
        [Required] [MaxLength(11)] public string VknTckn { get; set; } = null!;

        public bool IsCompany { get; set; }

        // İletişim Bilgileri
        [MaxLength(100)] public string? Email { get; set; }
        [MaxLength(20)] public string? Phone { get; set; }

        [MaxLength(20)] public string? MobilePhone { get; set; }

        // Adres Bilgileri
        public int Country { get; set; }

        [MaxLength(100)]
        public string City { get; set; }

        [MaxLength(100)]
        public string District { get; set; }
        [MaxLength(500)] public string? FullAddress { get; set; }
        [MaxLength(100)] public string? BuildingName { get; set; }
        [MaxLength(10)] public string? BuildingNumber { get; set; }
        [MaxLength(10)] public string? FloorNumber { get; set; }
        [MaxLength(10)] public string? DoorNumber { get; set; }
        [MaxLength(10)] public string? PostalCode { get; set; }
        [MaxLength(300)] public string? AddressDescription { get; set; }
    }

    public class CustomerUpdateDto : CustomerCreateDto, IBaseUpdateModel
    {
        public int Id { get; set; }

        public Guid GuidId { get; set; }
    }
}
