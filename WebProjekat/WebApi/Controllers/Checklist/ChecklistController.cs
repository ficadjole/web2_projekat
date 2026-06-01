using CheckListService.Interfaces.DTOs;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using WebApi.DTOs.Checklist;
using WebApi.Services;

namespace WebApi.Controllers.Checklist
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChecklistController(IValidator<AddChecklistItemRequest> addValidator, ChecklistServiceProxy proxy) : ControllerBase
    {

        [HttpGet("trip/{tripId}")]
        [Authorize]
        public async Task<IActionResult> GetByTripId(Guid tripId)
        {
            var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            var result = await proxy.GetChecklistProxy().GetByTripIdAsync(tripId, userId);
            if (result.IsFailure)
                return BadRequest(result.Error!.Message);
            return Ok(result.Value);
        }

        [HttpPost]
        [Authorize]
        [Route("items/add")]
        public async Task<IActionResult> AddItem([FromBody] AddChecklistItemRequest request)
        {
            var validatorResult = await addValidator.ValidateAsync(request);

            if (!validatorResult.IsValid)
                return BadRequest(validatorResult.Errors);
            var dto = new CreateChecklistItemDto
            {
                Name = request.Name,
                TripId = request.TripId
            };

            var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

            var result = await proxy.GetChecklistProxy().AddItemAsync(dto, userId);

            if (result.IsFailure)
                return BadRequest(result.Error!.Message);

            return Ok(result.Value);
        }

        [HttpPatch("trip/{tripId}/items/{itemId}/toggle")]
        [Authorize]
        public async Task<IActionResult> ToggleItem(Guid tripId,Guid itemId)
        {
            var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            var result = await proxy.GetChecklistProxy().ToggleItemAsync(tripId,itemId, userId);
            if (result.IsFailure)
                return BadRequest(result.Error!.Message);
            return Ok(result.Value);
        }

        [HttpDelete("trip/{tripId}/items/{itemId}")]
        [Authorize]
        public async Task<IActionResult> DeleteItem(Guid tripId, Guid itemId)
        {
            var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            var result = await proxy.GetChecklistProxy().DeleteItemAsync(tripId, itemId, userId);
            if (result.IsFailure)
                return BadRequest(result.Error!.Message);
            return Ok("Item deleted successfully.");
        }

        [HttpDelete("{checklistId}")]
        [Authorize]
        public async Task<IActionResult> DeleteChecklist(Guid checklistId)
        {
            var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            var result = await proxy.GetChecklistProxy().DeleteChecklistAsync(checklistId, userId);
            if (result.IsFailure)
                return BadRequest(result.Error!.Message);
            return Ok("Checklist deleted successfully.");
        }

    }
}
