using MoustafaTasks.Domain.Enums;

namespace MoustafaTasks.Domain;

public class SecUser
{
    public int UserId { get; set; }
    public string UserName { get; set; } = string.Empty;
    public UserTypeId UserTypeId { get; set; }
    public bool IsActived { get; set; }
    public bool IsRemoved { get; set; }

}
