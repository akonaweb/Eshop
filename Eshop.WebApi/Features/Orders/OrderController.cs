using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics.CodeAnalysis;

namespace Eshop.WebApi.Features.Orders
{
    [ExcludeFromCodeCoverage]
    [ApiController]
    [Route("[controller]")]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    public class OrderController : ControllerBase
    {
        private readonly IMediator mediator;

        public OrderController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet(Name = nameof(GetOrders))]
        [ProducesResponseType(typeof(IEnumerable<GetOrdersResponseDto>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<GetOrdersResponseDto>>> GetOrders()
        {
            var result = await mediator.Send(new GetOrders.Query());
            return Ok(result);
        }

        [HttpGet("{id}", Name = nameof(GetOrder))]
        [ProducesResponseType(typeof(GetOrderResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<GetOrderResponseDto>> GetOrder(int id)
        {
            var result = await mediator.Send(new GetOrder.Query(id));
            return Ok(result);
        }

        [HttpPost("cart", Name = nameof(GetCart))]
        [ProducesResponseType(typeof(GetCartResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<GetCartResponseDto>> GetCart(IEnumerable<GetCartRequestDto> request)
        {
            var result = await mediator.Send(new GetCart.Query(request));
            return Ok(result);
        }

        [HttpPost(Name = nameof(AddOrder))]
        [ProducesResponseType(typeof(AddOrderResponseDto), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<AddOrderResponseDto>> AddOrder(AddOrderRequestDto request)
        {
            var result = await mediator.Send(new AddOrder.Command(request));
            return CreatedAtAction(nameof(GetOrder), new { id = result.Id }, result);
        }

        [HttpDelete("{id}", Name = nameof(DeleteOrder))]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteOrder(int id)
        {
            await mediator.Send(new DeleteOrder.Command(id));
            return NoContent();
        }

        [HttpPut("{id}", Name = nameof(UpdateOrder))]
        [ProducesResponseType(typeof(UpdateOrderResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<UpdateOrderResponseDto>> UpdateOrder(int id, UpdateOrderRequestDto request)
        {
            var result = await mediator.Send(new UpdateOrder.Command(id, request));
            return Ok(result);
        }
    }
}
