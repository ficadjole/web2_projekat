using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TripService.Interfaces.DTOs.Activity;
using WebApi.DTOs.Activity;
using WebApi.Services;

namespace WebApi.Controllers.Activity
{
    [Route("api/[controller]")]
    [ApiController]
    public class ActivityController(IValidator<CreateActivityRequest> createValidator, IValidator<UpdateActivityRequest> updateValidator, TripServiceProxy proxy) : ControllerBase
    {
        [HttpGet("{id}")]
        [Authorize]
        public async Task<IActionResult> GetById(Guid id)
        {
            var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

            var result = await proxy.GetActivityProxy().GetActivityByIdAsync(id, userId);

            if (result.IsFailure)
                return BadRequest(result.Error!.Message);

            return Ok(result.Value);
        }

        [HttpGet("destination/{destinationId}")]
        [Authorize]
        public async Task<IActionResult> GetByDestinationId(Guid destinationId)
        {
            var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

            var result = await proxy.GetActivityProxy().GetActivitiesByDestinationIdAsync(destinationId, userId);

            if (result.IsFailure)
                return BadRequest(result.Error!.Message);

            return Ok(result.Value);
        }

        [HttpGet("trip/{tripId}/date")]
        [Authorize]
        public async Task<IActionResult> GetByDate(Guid tripId, [FromQuery] DateTime date)
        {
            var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

            var result = await proxy.GetActivityProxy().GetActivitiesByDateAsync(tripId, date, userId);

            if (result.IsFailure)
                return BadRequest(result.Error!.Message);

            return Ok(result.Value);
        }

        [HttpPost]
        [Authorize]
        [Route("create")]
        public async Task<IActionResult> Create([FromBody] CreateActivityRequest request)
        {
            var validatorResult = await createValidator.ValidateAsync(request);

            if (!validatorResult.IsValid)
                return BadRequest(validatorResult.Errors);

            var dto = new CreateActivityDto
            {
                Name = request.Name,
                Location = request.Location,
                Description = request.Description,
                Date = request.Date,
                EstimatedCost = request.EstimatedCost,
                Status = request.Status,
                DestinationId = request.DestinationId
            };

            var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

            var result = await proxy.GetActivityProxy().CreateActivityAsync(dto, userId);

            if (result.IsFailure)
                return BadRequest(result.Error!.Message);

            return Ok(result.Value);
        }

        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateActivityRequest request)
        {
            var validatorResult = await updateValidator.ValidateAsync(request);

            if (!validatorResult.IsValid)
                return BadRequest(validatorResult.Errors);

            var dto = new UpdateActivityDto
            {
                Name = request.Name,
                Location = request.Location,
                Description = request.Description,
                Date = request.Date,
                EstimatedCost = request.EstimatedCost,
                Status = request.Status
            };

            var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

            var result = await proxy.GetActivityProxy().UpdateActivityAsync(id, dto, userId);

            if (result.IsFailure)
                return BadRequest(result.Error!.Message);

            return Ok(result.Value);
        }

        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> Delete(Guid id)
        {
            var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

            var result = await proxy.GetActivityProxy().DeleteActivityAsync(id, userId);

            if (result.IsFailure)
                return BadRequest(result.Error!.Message);

            return Ok("Activity deleted successfully.");
        }

    }
}
