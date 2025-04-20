using System.Text.Json;
using API.RequestHelpers;
using Core.Entities;
using Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BaseApiController : ControllerBase
{
    protected async Task<ActionResult> CreatePagedResult<T>(IGenericRepository<T> repo,
        ISpecification<T> spec, int pageIndex, int pageSize) where T : BaseEntity
    {
        var items = await repo.ListAsync(spec);
        var count = await repo.CountAsync(spec);

        var pagination = new Pagination<T>(pageIndex, pageSize, count, items);

        var metadata = new PaginationHeader(pageIndex, pageSize, count);
        Response.Headers.Append("Pagination", JsonSerializer.Serialize(metadata));
        Response.Headers.Append("Access-Control-Expose-Headers", "Pagination");

        return Ok(pagination);
    }
}
