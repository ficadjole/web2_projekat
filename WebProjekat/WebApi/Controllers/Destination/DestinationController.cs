using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TripService.Interfaces.DTOs.Destination;
using WebApi.DTOs.Destination;
using WebApi.Services;

namespace WebApi.Controllers.Destination
{
    [Route("api/[controller]")]
    [ApiController]
    public class DestinationController(IValidator<CreateDestinationRequest> createValidator, IValidator<UpdateDestinationRequest> updateValidator, TripServiceProxy proxy) : ControllerBase
    {

        [HttpGet("{id}")]
        [Authorize]
        public async Task<IActionResult> GetById(Guid id)
        {
            var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

            var result = await proxy.GetDestinationProxy().GetDestinationByIdAsync(id, userId);

            if (result.IsFailure)
                return BadRequest(result.Error!.Message);

            return Ok(result.Value);
        }

        [HttpGet("trip/{tripId}")]
        [Authorize]
        public async Task<IActionResult> GetByTripId(Guid tripId)
        {
            var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

            var result = await proxy.GetDestinationProxy().GetDestinationsByTripIdAsync(tripId, userId);

            if (result.IsFailure)
                return BadRequest(result.Error!.Message);

            return Ok(result.Value);
        }

        [HttpPost]
        [Authorize]
        [Route("create")]
        public async Task<IActionResult> Create([FromBody] CreateDestinationRequest request)
        {
            var validatorResult = await createValidator.ValidateAsync(request);

            if (!validatorResult.IsValid)
                return BadRequest(validatorResult.Errors);

            var dto = new CreateDestinationDto
            {
                Name = request.Name,
                Description = request.Description,
                Notes = request.Notes,
                Location = request.Location,
                ArrivingDate = request.ArrivingDate,
                LeavingDate = request.LeavingDate,
                TripId = request.TripId
            };
            var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

            var result = await proxy.GetDestinationProxy().CreateDestinationAsync(dto, userId);

            if (result.IsFailure)
                return BadRequest(result.Error!.Message);

            return Ok(result.Value);
        }

        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateDestinationRequest request)
        {
            var validatorResult = await updateValidator.ValidateAsync(request);

            if (!validatorResult.IsValid)
                return BadRequest(validatorResult.Errors);

            var dto = new UpdateDestinationDto
            {
                Name = request.Name,
                Description = request.Description,
                Notes = request.Notes,
                Location = request.Location,
                ArrivingDate = request.ArrivingDate,
                LeavingDate = request.LeavingDate
            };

            var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

            var result = await proxy.GetDestinationProxy().UpdateDestinationAsync(id, dto, userId);

            if (result.IsFailure)
                return BadRequest(result.Error!.Message);

            return Ok(result.Value);
        }

        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> Delete(Guid id)
        {
            var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

            var result = await proxy.GetDestinationProxy().DeleteDestinationAsync(id, userId);

            if (result.IsFailure)
                return BadRequest(result.Error!.Message);

            return Ok("Destination deleted successfully.");
        }
    }
}
