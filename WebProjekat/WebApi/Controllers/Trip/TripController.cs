using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TripService.Interfaces.DTOs.Trip;
using WebApi.DTOs.Trip;
using WebApi.Services;

namespace WebApi.Controllers.Trip
{
    [Route("api/[controller]")]
    [ApiController]
    public class TripController(IValidator<CreateTripRequest> createValidator, IValidator<UpdateTripRequest> updateValidator, TripServiceProxy proxy) : ControllerBase
    {

        [HttpPost]
        [Authorize]
        [Route("create")]
        public async Task<IActionResult> Create([FromForm] CreateTripRequest request)
        {
            var validatorResult = await createValidator.ValidateAsync(request);

            if (!validatorResult.IsValid)
                return BadRequest(validatorResult.Errors);

            var dto = new CreateTripDto { Name = request.Name, Description = request.Description, StartDate = request.StartDate, EndDate = request.EndDate, PlannedBudget = request.PlannedBudget };

            var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

            var result = await proxy.GetTripProxy().CreateTripAsync(dto, userId);

            if (result.IsFailure)
                return BadRequest(result.Error!.Message);

            return Ok(result.Value);
        }


        [HttpGet("{id}")]
        [Authorize]
        public async Task<IActionResult> GetById(Guid id)
        {
            var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

            var result = await proxy.GetTripProxy().GetTripByIdAsync(id, userId);

            if (result.IsFailure)
                return BadRequest(result.Error!.Message);

            return Ok(result.Value);
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetAll()
        {
            var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

            var result = await proxy.GetTripProxy().GetAllTripsByUserAsync(userId);

            if (result.IsFailure)
                return BadRequest(result.Error!.Message);

            return Ok(result.Value);
        }

        [HttpGet("{id}/details")]
        [Authorize]
        public async Task<IActionResult> GetWithDetails(Guid id)
        {
            var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

            var result = await proxy.GetTripProxy().GetTripWithDetailsAsync(id, userId);

            if (result.IsFailure)
                return BadRequest(result.Error!.Message);

            return Ok(result.Value);
        }

        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> Update(Guid id, [FromForm] UpdateTripRequest request)
        {
            var validatorResult = await updateValidator.ValidateAsync(request);

            if (!validatorResult.IsValid)
                return BadRequest(validatorResult.Errors);

            var dto = new UpdateTripDto
            {
                Name = request.Name,
                Description = request.Description,
                StartDate = request.StartDate,
                EndDate = request.EndDate,
                PlannedBudget = request.PlannedBudget
            };

            var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

            var result = await proxy.GetTripProxy().UpdateTripAsync(id, dto, userId);

            if (result.IsFailure)
                return BadRequest(result.Error!.Message);

            return Ok(result.Value);
        }

        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> Delete(Guid id)
        {
            var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

            var result = await proxy.GetTripProxy().DeleteTripAsync(id, userId);

            if (result.IsFailure)
                return BadRequest(result.Error!.Message);

            return Ok("Trip deleted successfully.");
        }
    }
}
