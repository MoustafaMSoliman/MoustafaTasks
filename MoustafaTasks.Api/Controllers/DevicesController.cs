using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MoustafaTasks.Application;
using MoustafaTasks.Domain;

namespace MoustafaTasks.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DevicesController : ControllerBase
    {
        private IGenericFilterService<Device> _genericFilterService;
        public DevicesController(IGenericFilterService<Device> genericFilterService)
        {
            _genericFilterService = genericFilterService;

        }
        [HttpPost("getAllDevices")]
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
