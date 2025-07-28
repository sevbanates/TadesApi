public class UserRequestCreateDto
{
    public string TargetUserEmail { get; set; }
}

public class UserRequestActionDto
{
    public long RequestId { get; set; }
    public bool Accept { get; set; }
}