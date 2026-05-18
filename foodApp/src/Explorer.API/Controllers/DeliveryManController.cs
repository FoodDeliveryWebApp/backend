using Explorer.Stakeholders.API.Dtos;
using Explorer.Stakeholders.API.Public;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Explorer.API.Controllers;

[Route("api/delivery-men")]
[ApiController]
[Authorize(Roles = "administrator")]
public class DeliveryManController : BaseApiController
{
    private readonly IDeliveryManService _deliveryManService;

    public DeliveryManController(IDeliveryManService deliveryManService)
    {
        _deliveryManService = deliveryManService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<UserDto>>> GetAll()
    {
        var result = await _deliveryManService.GetAllAsync();
        return CreateResponse(result);
    }

    [HttpPost]
    public async Task<ActionResult> Create([FromBody] UserDto dto)
    {
        var result = await _deliveryManService.CreateAsync(dto);
        return CreateResponse(result);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> Update(int id, [FromBody] UserDto dto)
    {
        var result = await _deliveryManService.UpdateAsync(id, dto);
        return CreateResponse(result);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(int id)
    {
        var result = await _deliveryManService.DeleteAsync(id);
        return CreateResponse(result);
    }
}
