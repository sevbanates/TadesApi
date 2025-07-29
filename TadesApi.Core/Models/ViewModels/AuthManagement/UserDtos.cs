using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using TadesApi.Core.Models.Global;

namespace TadesApi.Core.Models.ViewModels.AuthManagement;

public class UserBasicDto
{
    public long Id { get; set; }
    public Guid? GuidId { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string PhoneNumber { get; set; }
    public string UserName { get; set; }
    public DateTime CreDate { get; set; }
    public int RoleId { get; set; }
    public bool IsActive { get; set; }

    public string Photo { get; set; }

    public string RoleName => RoleId switch
    {
        100 => "Admin",
        101 => "User",
        _ => "Unknown"
    };

    public string FullName => $"{FirstName} {LastName}";
}

public class CreateUserDto
{
    private readonly string _phoneNumber;

    [StringLength(20, MinimumLength = 3)]
    [Required]
    public string UserName { get; set; }

    [StringLength(50)] [Required] public string FirstName { get; set; }

    [StringLength(50)] [Required] public string LastName { get; set; }

    [EmailAddress]
    [StringLength(100, MinimumLength = 8)]
    [Required]
    public string Email { get; set; }

    [StringLength(15, MinimumLength = 10)]
    public string PhoneNumber
    {
        get => _phoneNumber;
        init => _phoneNumber = string.IsNullOrWhiteSpace(value) ? null : value.ToLower();
    }

    [StringLength(20, MinimumLength = 3)]
    [Required]
    public string Password { get; set; }

    [StringLength(20, MinimumLength = 3)]
    [Compare("Password")]
    [Required]
    public string PasswordConfirmation { get; set; }

    [Range(100, 104)] [Required] public int RoleId { get; set; }

    public bool IsActive { get; set; }
    public bool IsPrimaryForClient { get; set; }
}

public class UpdateUserDto : IBaseUpdateModel
{
    private readonly string _phoneNumber;

    [StringLength(20, MinimumLength = 3)]
    [Required]
    public string UserName { get; set; }

    [StringLength(50)] [Required] public string FirstName { get; set; }

    [StringLength(50)] [Required] public string LastName { get; set; }

    [EmailAddress]
    [StringLength(100, MinimumLength = 8)]
    [Required]
    public string Email { get; set; }

    [StringLength(15, MinimumLength = 10)]
    public string PhoneNumber
    {
        get => _phoneNumber;
        init => _phoneNumber = string.IsNullOrWhiteSpace(value) ? null : value.ToLower();
    }

    [Range(100, 104)] [Required] public int RoleId { get; set; }

    public bool IsActive { get; set; }
    [Required] public Guid GuidId { get; set; }
}

public class UpdateProfileDto
{
    private readonly string _phoneNumber;
    [StringLength(50)] [Required] public string FirstName { get; set; }

    [StringLength(50)] [Required] public string LastName { get; set; }

    [EmailAddress]
    [StringLength(100, MinimumLength = 8)]
    [Required]
    public string Email { get; set; }

    [StringLength(15, MinimumLength = 10)]
    public string PhoneNumber
    {
        get => _phoneNumber;
        init => _phoneNumber = string.IsNullOrWhiteSpace(value) ? null : value.ToLower();
    }
}

public class UserViewModel : BaseModel
{
    public Guid GuidId { get; set; }
    public long CompanyId { get; set; }
    public string UserName { get; set; }
    public string FirstName { get; set; }
    public string Email { get; set; }
    public string LastName { get; set; }
    public string PhoneNumber { get; set; }
    public string Password { get; set; }
    public string PasswordSalt { get; set; }
    public string PasswordSalt2 { get; set; }
    public string PasswordConfirmation { get; set; }
    public string LastIpAddress { get; set; }
    public DateTime? LastLoginTime { get; set; }
    public string RoleName { get; set; }
    public bool IsActive { get; set; }
    public string Token { get; set; }
    public int RoleId { get; set; }
    public bool? IsDeleted { get; set; }
    public string UserLogoPath { get; set; }
    public bool? IsFirstLogin { get; set; }
    public DateTime? CreDate { get; set; }
    public List<SysControllerActionTotalViewModel> SecurityTotalList { get; set; } = new();

    public RoleBasicDto Role { get; set; }
    public bool? CanClientEdit { get; set; } = false;
    public long? ClientId { get; set; }
}

public class UserSearchInput : PagedAndSortedSearchInput
{
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public bool IsActive { get; set; } = true;
    public int? RoleId { get; set; }
    public long? ClientId { get; set; }
}

public class LoginModel
{
    [Required] public string UserName { get; set; }

    [Required] public string Password { get; set; }
}

public class LoginModelCode
{
    [Required] public int UserId { get; set; }

    [Required] public string Code { get; set; }
}

public class LoginUserInfo
{
    public long UserId { get; set; }
    public string Email { get; set; }
}

public class ForgotPasswordModel
{
    public string Email { get; set; }
}

public class ResetPasswordModel
{
    [Required] public string Password { get; set; }

    [Required] [Compare("Password")] public string ConfirmPassword { get; set; }

    [Required] public string Token { get; set; }
}

public class FirstLoginChangePasswordModel
{
    public long Id { get; set; }
    public string UserName { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }

    [Required] public string OldPassword { get; set; }

    [Required] public string Password1 { get; set; }

    [Required] [Compare("Password1")] public string Password2 { get; set; }
}

public class UpdateSelfPasswordDto
{
    [Required]
    [StringLength(20, MinimumLength = 6)]
    public string OldPassword { get; set; }

    [Required]
    [StringLength(20, MinimumLength = 6)]
    public string NewPassword1 { get; set; }

    [Required]
    [StringLength(20, MinimumLength = 6)]
    [Compare("NewPassword1", ErrorMessage = "Passwords do not match.")]
    public string NewPassword2 { get; set; }
}

public class UpdateOthersPasswordDto
{
    [Required] public Guid GuidId { get; set; }

    [StringLength(20, MinimumLength = 6)]
    [Required]
    public string Password { get; set; }

    [StringLength(20, MinimumLength = 6)]
    [Compare("Password")]
    [Required]
    public string PasswordConfirmation { get; set; }
}

public class ClientConsultantLogDto
{
    public DateTime BegDate { get; set; }

    public DateTime EndDate { get; set; }

    //public ClientBasicInfo Client { get; set; }
    public UserBasicDto Consultant { get; set; }
}

public class ClientBasicInfo
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string FullName => $"{FirstName} {LastName}";
    public string BirthDate { get; set; }
}