public class UserRequestCreateDto
{
    public string TargetUserEmail { get; set; }
}

public class UserRequestActionDto
{
    public long RequestId { get; set; }
    public bool Accept { get; set; }
}
public class AccounterUserDto
{
    public long UserId { get; set; }
    public string FullName { get; set; }
}