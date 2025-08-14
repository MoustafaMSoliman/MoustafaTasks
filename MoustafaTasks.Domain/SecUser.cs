using MoustafaTasks.Domain.Enums;

namespace MoustafaTasks.Domain;

public class SecUser
{
    public SecUser(int userId, string userName, UserTypeId userTypeId, bool isActived, bool isRemoved)
    {
        UserId = userId;
        UserName = userName;
        UserTypeId = userTypeId;
        IsActived = isActived;
        IsRemoved = isRemoved;
    }

    public int UserId { get; set; }
    public string UserName { get; set; } = string.Empty;
    public UserTypeId UserTypeId { get; set; }
    public bool IsActived { get; set; }
    public bool IsRemoved { get; set; }

}
