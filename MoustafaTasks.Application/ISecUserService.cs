using MoustafaTasks.Domain;

namespace MoustafaTasks.Application;

public interface ISecUserService
{
    Task<List<SecUser>> GetAllPersonUsers(CancellationToken cancellationToken);
    Task<List<SecUser>> GetAllPersonUsersV2(CancellationToken cancellationToken);
}
