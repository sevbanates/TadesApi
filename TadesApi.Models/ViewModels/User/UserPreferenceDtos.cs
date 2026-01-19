using System;

namespace TadesApi.Models.ViewModels.User
{
    public class UserPreferenceDto
    {
        public long Id { get; set; }
        public long UserId { get; set; }
        public long? SelectedUserId { get; set; }
        public string PreferenceKey { get; set; }
        public string PreferenceValue { get; set; }
    }

    public class SetSelectedUserDto
    {
        public long? SelectedUserId { get; set; }
    }

    public class AccounterAccessibleUserDto
    {
        public long UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string FullName => $"{FirstName} {LastName}";
        public bool IsSelected { get; set; }
    }

    public class AccounterUserSelectionResponseDto
    {
        public List<AccounterAccessibleUserDto> AccessibleUsers { get; set; } = new();
        public long? CurrentSelectedUserId { get; set; }
    }
}









