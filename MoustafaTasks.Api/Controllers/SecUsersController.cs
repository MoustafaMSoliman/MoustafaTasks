using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MoustafaTasks.Api.Requests;
using MoustafaTasks.Application;
using MoustafaTasks.Domain;
using MoustafaTasks.Infrastructure;

namespace MoustafaTasks.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SecUsersController : ControllerBase
    {
        private IGenericFilterService<SecUser> _genericFilterService;
        public SecUsersController(IGenericFilterService<SecUser> genericFilterService)
        {
            _genericFilterService = genericFilterService;

        }

       
        [HttpPost("getAllSecUsers")]
        public async Task<IActionResult> GetAllSecUsers([FromBody] FilterQuery filterQuery)
        {
            return Ok(
                await _genericFilterService.GetAll(
                new List<FilterQuery>() { 
                    new FilterQuery(filterQuery.PropertyName, Domain.Enums.FilterOperator.Equals, filterQuery.Value, Domain.Enums.LogicalOperator.And) ,
                    new FilterQuery(filterQuery.PropertyName, Domain.Enums.FilterOperator.Equals, filterQuery.Value, Domain.Enums.LogicalOperator.Or)

                },
                CancellationToken.None
                )
                );
        }
    }
}
