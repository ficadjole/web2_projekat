using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TripService.Enums;
using TripService.Interfaces.DTOs.Expense;
using WebApi.DTOs.Expense;
using WebApi.Services;

namespace WebApi.Controllers.Expense
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExpenseController(IValidator<CreateExpenseRequest> createValidator, IValidator<UpdateExpenseRequest> updateValidator, TripServiceProxy proxy) : ControllerBase
    {

        [HttpGet("{id}")]
        [Authorize]
        public async Task<IActionResult> GetById(Guid id)
        {
            var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

            var result = await proxy.GetExpenseProxy().GetExpenseByIdAsync(id, userId);

            if (result.IsFailure)
                return BadRequest(result.Error!.Message);

            return Ok(result.Value);
        }

        [HttpGet("trip/{tripId}")]
        [Authorize]
        public async Task<IActionResult> GetByTripId(Guid tripId)
        {
            var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

            var result = await proxy.GetExpenseProxy().GetExpensesByTripIdAsync(tripId, userId);

            if (result.IsFailure)
                return BadRequest(result.Error!.Message);

            return Ok(result.Value);
        }

        [HttpGet("trip/{tripId}/category")]
        [Authorize]
        public async Task<IActionResult> GetByCategory(Guid tripId, [FromQuery] BudgetCategory category)
        {
            var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

            var result = await proxy.GetExpenseProxy().GetExpensesByCategoryAsync(tripId, category, userId);

            if (result.IsFailure)
                return BadRequest(result.Error!.Message);

            return Ok(result.Value);
        }

        [HttpGet("trip/{tripId}/summary")]
        [Authorize]
        public async Task<IActionResult> GetBudgetSummary(Guid tripId)
        {
            var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

            var result = await proxy.GetExpenseProxy().GetBudgetSummaryAsync(tripId, userId);

            if (result.IsFailure)
                return BadRequest(result.Error!.Message);

            return Ok(result.Value);
        }

        [HttpPost]
        [Authorize]
        [Route("create")]
        public async Task<IActionResult> Create([FromForm] CreateExpenseRequest request)
        {
            var validatorResult = await createValidator.ValidateAsync(request);

            if (!validatorResult.IsValid)
                return BadRequest(validatorResult.Errors);

            var dto = new CreateExpenseDto
            {
                Name = request.Name,
                Category = request.Category,
                Amount = request.Amount,
                Description = request.Description,
                TripId = request.TripId
            };

            var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

            var result = await proxy.GetExpenseProxy().CreateExpenseAsync(dto, userId);

            if (result.IsFailure)
                return BadRequest(result.Error!.Message);

            return Ok(result.Value);
        }

        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> Update(Guid id, [FromForm] UpdateExpenseRequest request)
        {
            var validatorResult = await updateValidator.ValidateAsync(request);

            if (!validatorResult.IsValid)
                return BadRequest(validatorResult.Errors);

            var dto = new UpdateExpenseDto
            {
                Name = request.Name,
                Category = request.Category,
                Amount = request.Amount,
                Description = request.Description
            };

            var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

            var result = await proxy.GetExpenseProxy().UpdateExpenseAsync(id, dto, userId);

            if (result.IsFailure)
                return BadRequest(result.Error!.Message);

            return Ok(result.Value);
        }

        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> Delete(Guid id)
        {
            var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

            var result = await proxy.GetExpenseProxy().DeleteExpenseAsync(id, userId);

            if (result.IsFailure)
                return BadRequest(result.Error!.Message);

            return Ok("Expense deleted successfully.");
        }

    }
}
