using MoustafaTasks.Domain;
using MoustafaTasks.Domain.Enums;
using System.Linq.Expressions;

namespace MoustafaTasks.Application;

public class SecUsersServices
{
    private readonly IRepository<SecUser> _userRepository;

    public SecUsersServices(IRepository<SecUser> userRepository)
    {
        _userRepository = userRepository;
    }
    public async Task<List<SecUser>> GetAllPersonUsers(CancellationToken cancellationToken)
    {
        return await _userRepository.GetAll(new Expression<Func<SecUser, bool>>[] { x => x.UserTypeId == UserTypeId.Person, x => x.IsActived == true }, cancellationToken);
    }
   
    public async Task<List<SecUser>> GetAllPersonUsersV2(CancellationToken cancellationToken)
    {
        List<FilterQuery> filters = new List<FilterQuery> {
            new FilterQuery("UserTypeId", FilterOperator.Equals, UserTypeId.Person, LogicalOperator.And),
            new FilterQuery("UserName", FilterOperator.Contains, "Ali", LogicalOperator.Or),
            new FilterQuery("IsActive", FilterOperator.Equals, true, LogicalOperator.And)
        };
        return await _userRepository.GetAll(filters, cancellationToken);
    }
}
