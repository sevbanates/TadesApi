using System.ComponentModel.DataAnnotations;
using System.Globalization;
using TadesApi.Core.Models.Global;
using TadesApi.Core.Models.ViewModels.AuthManagement;
using TadesApi.Models.Global;
using TadesApi.Models.ViewModels.PotentialClient;

namespace TadesApi.Models.ViewModels.Client;

public class ClientBasicDto
{
    public long Id { get; set; }
    public Guid? GuidId { get; set; }
    //public long UserId { get; set; }
    public string FirstName { get; set; }
    public string MiddleName { get; set; }
    public string LastName { get; set; }
    public string BirthDate { get; set; }
    public string Gender { get; set; }
    public string Grade { get; set; }
    public string School { get; set; }
    public string StudentId { get; set; }
    public DateTime? _IepDueDate { get; set; }
    public DateTime? _TriennialDueDate { get; set; }
    public long? ConsultantId { get; set; }
    public string _ReasonForReferral { get; set; }
    public int Status { get; set; }
    
    public string HomeAddress { get; set; }
    public string City { get; set; }
    public string State { get; set; }
    public string Country { get; set; }
    public string Zip { get; set; }

    public string StatusText => Status switch
    {
        (int)ClientStatus.Active => "Active",
        (int)ClientStatus.Inactive => "Inactive",
        (int)ClientStatus.Archived => "Archived",
        _ => "Potential"
    };

    public string FullName => $"{FirstName} {LastName}";

    public string RoomLink =>
        $"https://meet.globalpsychsolutions.com/join/{GuidId}?name={FirstName}&video=1&audio=1&hide=1&notify=1&screen=0";
}

public class ClientBasicWithConsultantDto : ClientBasicDto
{
    public UserBasicDto Consultant { get; set; }
    
    public UserBasicDto User { get; set; }
    
    public Guid? UserGuidId  => User?.GuidId;
}

public class ClientDto : ClientBasicDto
{
    public string _ServiceType { get; set; }
    public string _EvaluationType { get; set; }
    public int? _ServiceDuration { get; set; }
    public string _ServiceFrequency { get; set; }
    public string _ServiceInterval { get; set; }
    public string _CheckList { get; set; }
    public int? ChronologicalAge { get; set; }
    public string PrimaryLanguage { get; set; }
    public string CountryOfOrigin { get; set; }
    public string Ethnicity { get; set; }
    public string PlaceOfBirth { get; set; }
    
    public int? GestationalAge { get; set; }
    public int? BirthWeight { get; set; }
    public string PostnatalDifficulties { get; set; }
    
    public string DevelopmentalMilestones { get; set; }
    public bool Crawled { get; set; }
    public string CrawledDetails { get; set; }
    public bool Walked { get; set; }
    public string WalkedDetails { get; set; }
    public bool FirstWords { get; set; }
    public string FirstWordsDetails { get; set; }
    public bool Phrases { get; set; }
    public string PhrasesDetails { get; set; }
    public bool ToiletTrained { get; set; }
    public string ToiletTrainedDetails { get; set; }
    public bool BedWetting { get; set; }
    public string BedWettingDetails { get; set; }
    
    
    public string MedicalConditions { get; set; }
    public string PreviousOrCurrentMentalHealth { get; set; }
    public string SignificantLifeEvents { get; set; }
    public string DifficultiesImpairmentsVision { get; set; }
    public string DifficultiesImpairmentsWearsGlasses { get; set; }
    public string DifficultiesImpairmentsHearing { get; set; }
    public string DifficultiesImpairmentsSpeechLang { get; set; }
    public string DifficultiesImpairmentsSleep { get; set; }
    public string DifficultiesImpairmentsEating { get; set; }
    public string FamilyHistoryOfMedicalProblems { get; set; }
    
    public string TraumaticEvents { get; set; }
    public bool VictimOfAbuse { get; set; }
    public bool DomesticNeighborhoodViolence { get; set; }
    public bool Bullying { get; set; }
    public bool SeparationDivorce { get; set; }
    public bool SeriousFamilyIllnessDeath { get; set; }
    public bool VictimOfCrime { get; set; }
    public bool CatastrophicEvents { get; set; }
    public bool Homelessness { get; set; }
    public bool FamilyIncarceration { get; set; }
    public bool OtherTraumaticEvent { get; set; }
    public string OtherTraumaticEventDescription { get; set; }
    public string ExposureDetails { get; set; }
    
    public string ConcernAreas { get; set; }
    
    public string OverallBehaviorAtHome { get; set; }
    public string BehaviorWithParentsAtHome { get; set; }
    public string StrengthsInterestHobbies { get; set; }
    public string AdditionalComments { get; set; }
    public string AcademicFunctioningConcerns { get; set; }
    public string ExecutiveFunctioningConcerns { get; set; }
    public string BehavioralFunctioningConcerns { get; set; }
    public string SocialEmotionalFunctioningConcerns { get; set; }
    public string DevelopmentalFunctioningConcerns { get; set; }
    public string ClientsCurrentSchoolExperiences { get; set; }
    public string ClientsPreviousSchoolExperiences { get; set; }
    public string EducationalHistoryDifficulties { get; set; }
    public string EducationalHistoryClientsAttendance { get; set; }
    public bool EducationalHistoryClientRetained { get; set; }
    public string _RespondentName { get; set; }
    public string _RelationshipToClient { get; set; }
    public string _QualifiedInterviewer { get; set; }

    public DateTime? ConsultantAssignmentDate { get; set; }
}

public class ClientWithConsultantDto : ClientDto
{
    public UserBasicDto Consultant { get; set; }
    public List<ClientEducationalConcernDto> Goals{ get; set; }

}

public class ClientWithParentSibling : ClientDto
{
    public UserBasicDto User{ get; set; }
    public virtual List<ClientParentDto> Parents { get; set; }
    public virtual List<ClientSiblingDto> Siblings { get; set; }
    public virtual List<ClientEducationalConcernDto> Goals { get; set; }
}
public class UpdateDemographicsDto : CreatePotentialClientDto, IBaseUpdateModel
{
    public Guid GuidId { get; set; }
}

public class UpdateInterpersonalRelationshipsDto : IBaseUpdateModel
{
    [StringLength(1000)]
    public string OverallBehaviorAtHome { get; set; }
    
    [StringLength(1000)]
    public string BehaviorWithParentsAtHome { get; set; }
    
    [StringLength(1000)]
    public string StrengthsInterestHobbies { get; set; }
    
    public Guid GuidId { get; set; }
}

public class UpdateInterpersonalRelationshipsAndCreateFamilyDto : UpdateInterpersonalRelationshipsDto, IBaseUpdateModel
{
    public List<CreateClientParentDto> Parents { get; set; }
    public List<CreateClientSiblingDto> Siblings { get; set; }
}

public class UpdateMedicalAndMentalHealthInformationDto : IBaseUpdateModel
{
    [StringLength(1000)]
    public string MedicalConditions { get; set; }
    
    [StringLength(1000)]
    public string PreviousOrCurrentMentalHealth { get; set; }
    
    [StringLength(1000)]
    public string SignificantLifeEvents { get; set; }
    
    [StringLength(1000)]
    public string DifficultiesImpairmentsVision { get; set; }
    
    [StringLength(1000)]
    public string DifficultiesImpairmentsWearsGlasses { get; set; }
    
    [StringLength(1000)]
    public string DifficultiesImpairmentsHearing { get; set; }
    
    [StringLength(1000)]
    public string DifficultiesImpairmentsSpeechLang { get; set; }
    
    [StringLength(1000)]
    public string DifficultiesImpairmentsSleep { get; set; }
    
    [StringLength(1000)]
    public string DifficultiesImpairmentsEating { get; set; }
    
    [StringLength(1000)]
    public string FamilyHistoryOfMedicalProblems { get; set; }
    
    public Guid GuidId { get; set; }
}

public class UpdateEducationalHistoryDto : IBaseUpdateModel
{
    public Guid GuidId { get; set; }
    
    [StringLength(1000)]
    public string ClientsCurrentSchoolExperiences { get; set; }
    
    [StringLength(1000)]
    public string ClientsPreviousSchoolExperiences { get; set; }
    
    [StringLength(1000)]
    public string EducationalHistoryDifficulties { get; set; }
    
    [StringLength(1000)]
    public string EducationalHistoryClientsAttendance { get; set; }
    
    public bool EducationalHistoryClientRetained { get; set; }
}

public class UpdateConcernedAreasDto : IBaseUpdateModel
{
    public Guid GuidId { get; set; }
    public string[] AcademicFunctioningConcerns { get; set; }
    public string[] ExecutiveFunctioningConcerns { get; set; }
    public string[] BehavioralFunctioningConcerns { get; set; }
    public string[] SocialEmotionalFunctioningConcerns { get; set; }
    public string[] DevelopmentalFunctioningConcerns { get; set; }
    
    [StringLength(5000)]
    public string ConcernAreas { get; set; }
}

public class UpdateEarlyDevelopmentalDto : IBaseUpdateModel
{
    public Guid GuidId { get; set; }
    
    [StringLength(1000)]
    public string DevelopmentalMilestones { get; set; }
    
    [Range(0,100)]
    public int? GestationalAge { get; set; }
    
    [Range(0,100)]
    public int? BirthWeight { get; set; }
    
    [StringLength(1000)]
    public string PostnatalDifficulties { get; set; }
    
    [StringLength(1000)]
    public string TraumaticEvents { get; set; }
    public bool VictimOfAbuse { get; set; }
    public bool DomesticNeighborhoodViolence { get; set; }
    public bool Bullying { get; set; }
    public bool SeparationDivorce { get; set; }
    public bool SeriousFamilyIllnessDeath { get; set; }
    public bool VictimOfCrime { get; set; }
    public bool CatastrophicEvents { get; set; }
    public bool Homelessness { get; set; }
    public bool FamilyIncarceration { get; set; }
    public bool OtherTraumaticEvent { get; set; }
    
    [StringLength(1000)]
    public string OtherTraumaticEventDescription { get; set; }
    
    [StringLength(5000)]
    public string ExposureDetails { get; set; }
    
    
    [StringLength(1000)]
    public string AdditionalComments { get; set; }
    
    public bool Crawled { get; set; }
    [StringLength(500)]
    public string CrawledDetails { get; set; }
    
    public bool Walked { get; set; }
    [StringLength(500)]
    public string WalkedDetails { get; set; }
    
    public bool FirstWords { get; set; }
    [StringLength(500)]
    public string FirstWordsDetails { get; set; }
    
    public bool Phrases { get; set; }
    [StringLength(500)]
    public string PhrasesDetails { get; set; }
    
    public bool ToiletTrained { get; set; }
    [StringLength(500)]
    public string ToiletTrainedDetails { get; set; }
    
    public bool BedWetting { get; set; }
    [StringLength(500)]
    public string BedWettingDetails { get; set; }
}

public class OfficeUseOnlyDto : IBaseUpdateModel
{
    public Guid GuidId { get; set; }
    
    public DateTime? _IepDueDate { get; set; }
    public DateTime? _TriennialDueDate { get; set; }
    public string _ReasonForReferral { get; set; }
    public string _ServiceType { get; set; }
    public string _EvaluationType { get; set; }
    public int? _ServiceDuration { get; set; }
    public string _ServiceFrequency { get; set; }
    public string _ServiceInterval { get; set; }
    public string _RespondentName { get; set; }
    public string _RelationshipToClient { get; set; }
    public string _QualifiedInterviewer { get; set; }
}

public class PotentialClientToClientDto : OfficeUseOnlyDto
{
    public long? ConsultantId { get; set; }
    
    [Required] [EmailAddress][StringLength(100), MinLength(8)]
    public string Email { get; set; }
    
    [Required] [StringLength(50), MinLength(3)]
    public string UserName { get; set; }
    
    [Required] [StringLength(20), MinLength(6)]
    public string Password { get; set; }
    
    [Required] [StringLength(20), MinLength(6)] [Compare("Password")]
    public string ConfirmPassword { get; set; }
    public DateTime? ConsultantAssignmentDate => ConsultantId.HasValue ? DateTime.Now : null;
}

public class CreateParentOrTeacherDto
{
    public Guid GuidId { get; set; }
    
    [Required] [StringLength(50), MinLength(2)]
    public string FirstName { get; set; }
    
    [Required] [StringLength(50), MinLength(1)]
    public string LastName { get; set; }
    
    [Required] [EmailAddress][StringLength(100), MinLength(8)]
    public string Email { get; set; }
    
    [Required] [StringLength(20), MinLength(3)]
    public string UserName { get; set; }
    
    [Required] [StringLength(20), MinLength(6)]
    public string Password { get; set; }
    
    [Required] [StringLength(20), MinLength(6)] [Compare("Password")]
    public string ConfirmPassword { get; set; }
}


public class ClientSearchDto : PagedAndSortedSearchInput
{
    public int[] Statuses { get; set; }
    public DateTime? IepDate { get; set; }
    public DateTime? TriennialDate { get; set; }
    public string Grade { get; set; }
    public long? ConsultantId { get; set; }
}

public class ClientCheckListDto
{
    public int? TestingStatus { get; set; }
    public bool IsAwaitingExternalDocuments { get; set; }
    public int? ReportStatus { get; set; }
    public bool IsFeedbackSessionScheduled { get; set; }
}

public class UpdateClientCheckListDto : ClientCheckListDto, IBaseUpdateModel
{
    [Required] public Guid GuidId { get; set; }
}

public class UpdateClientStatusDto
{
    [Required] public Guid GuidId { get; set; }

    [Required] public ClientStatus Status { get; set; }
}

public class UpdateClientConsultantDto
{
    [Required] public Guid GuidId { get; set; }

    [Required] public long ConsultantId { get; set; }
}