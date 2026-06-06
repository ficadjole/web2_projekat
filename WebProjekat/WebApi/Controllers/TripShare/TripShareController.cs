using FluentValidation;
using MailingService.Interface.Events;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TripService.Interfaces.DTOs.TripShare;
using WebApi.Services;

namespace WebApi.Controllers.TripShare
{
    [Route("api/[controller]")]
    [ApiController]
    public class TripShareController(IValidator<CreateTripShareRequest> createValidator, TripServiceProxy proxy, MailingServiceProxy mailingProxy) : ControllerBase
    {

        [HttpGet("{token}")]
        public async Task<IActionResult> GetSharedTrip(string token)
        {
            var result = await proxy.GetTripShareProxy().GetSharedTripAsync(token);

            if (result.IsFailure)
                return BadRequest(result.Error!.Message);

            return Ok(result.Value);
        }

        [HttpGet("trip/{tripId}")]
        [Authorize]
        public async Task<IActionResult> GetByTripId(Guid tripId)
        {
            var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

            var result = await proxy.GetTripShareProxy().GetSharesByTripIdAsync(tripId, userId);

            if (result.IsFailure)
                return BadRequest(result.Error!.Message);

            return Ok(result.Value);
        }

        [HttpPost]
        [Authorize]
        [Route("create")]
        public async Task<IActionResult> Create([FromBody] CreateTripShareRequest request)
        {
            var validatorResult = await createValidator.ValidateAsync(request);

            if (!validatorResult.IsValid)
                return BadRequest(validatorResult.Errors);

            var dto = new CreateTripShareDto
            {
                TripId = request.TripId,
                AccessType = request.AccessType,
                ExpiresInDays = request.ExpiresInDays,
                Email = request.Email
            };

            var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

            var result = await proxy.GetTripShareProxy().CreateShareAsync(dto, userId);

            if (result.IsFailure)
                return BadRequest(result.Error!.Message);

            EmailEvent emailEvent = new EmailEvent
            {
                Email = request.Email,
                TripShareDto = result.Value
            };

            await mailingProxy.GetMailingProxy().PublishEvent(emailEvent);

            return Ok(result.Value);
        }

        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> Revoke(Guid id)
        {
            var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

            var result = await proxy.GetTripShareProxy().RevokeShareAsync(id, userId);

            if (result.IsFailure)
                return BadRequest(result.Error!.Message);

            return Ok("Share revoked successfully.");
        }

        [HttpGet("{tripId}/report")]
        [Authorize]
        public async Task<IActionResult> GenerateReport(Guid tripId)
        {
            var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            var result = await proxy.GetTripShareProxy().GenerateTripReportAsync(tripId, userId);

            if (result.IsFailure)
                return BadRequest(result.Error!.Message);

            return File(result.Value, "application/pdf", $"trip-report-{tripId}.pdf");
        }

    }
}
