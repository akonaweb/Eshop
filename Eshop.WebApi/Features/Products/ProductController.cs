using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics.CodeAnalysis;

namespace Eshop.WebApi.Features.Products
{
    [ExcludeFromCodeCoverage]
    [ApiController]
    [Route("[controller]")]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    public class ProductController : ControllerBase
    {
        private readonly IMediator mediator;

        public ProductController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet(Name = nameof(GetProducts))]
        [ProducesResponseType(typeof(IEnumerable<GetProductsResponseDto>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<GetProductsResponseDto>>> GetProducts()
        {
            var result = await mediator.Send(new GetProducts.Query());
            return Ok(result);
        }

        [HttpGet("{id}", Name = nameof(GetProduct))]
        [ProducesResponseType(typeof(GetProductResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<GetProductResponseDto>> GetProduct(int id)
        {
            var result = await mediator.Send(new GetProduct.Query(id));
            return Ok(result);
        }

        //[Authorize(AuthenticationSchemes = "Bearer")]
        //[Authorize(Policy = "AdminPolicy")]
        [Authorize(Roles = "Administrator")]
        [HttpPost(Name = nameof(AddProduct))]
        [ProducesResponseType(typeof(AddProductResponseDto), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status403Forbidden)]
        public async Task<ActionResult<AddProductResponseDto>> AddProduct(AddProductRequestDto request)
        {
            var result = await mediator.Send(new AddProduct.Command(request));
            return CreatedAtAction(nameof(GetProduct), new { id = result.Id }, result);
        }

        [Authorize(Roles = "Administrator")]
        [HttpPut("{id}", Name = nameof(UpdateProduct))]
        [ProducesResponseType(typeof(UpdateProductResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status403Forbidden)]
        public async Task<ActionResult<UpdateProductResponseDto>> UpdateProduct(int id, UpdateProductRequestDto request)
        {
            var result = await mediator.Send(new UpdateProduct.Command(id, request));
            return Ok(result);
        }

        [Authorize(Roles = "Administrator")]
        [HttpDelete("{id}", Name = nameof(DeleteProduct))]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status403Forbidden)]
        public async Task<ActionResult> DeleteProduct(int id)
        {
            await mediator.Send(new DeleteProduct.Command(id));
            return NoContent();
        }
    }
}
