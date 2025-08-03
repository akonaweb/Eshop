using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics.CodeAnalysis;

namespace Eshop.WebApi.Features.Categories
{
    [ExcludeFromCodeCoverage]
    [ApiController]
    [Route("[controller]")]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    public class CategoryController : ControllerBase
    {
        private readonly IMediator mediator;

        public CategoryController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet(Name = nameof(GetCategories))]
        [ProducesResponseType(typeof(IEnumerable<GetCategoriesResponseDto>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<GetCategoriesResponseDto>>> GetCategories()
        {
            var result = await mediator.Send(new GetCategories.Query());
            return Ok(result);
        }

        [HttpGet("{id}", Name = nameof(GetCategory))]
        [ProducesResponseType(typeof(GetCategoryResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<GetCategoryResponseDto>> GetCategory(int id)
        {
            var result = await mediator.Send(new GetCategory.Query(id));
            return Ok(result);
        }

        [Authorize(Roles = "Administrator")]
        [HttpPost(Name = nameof(AddCategory))]
        [ProducesResponseType(typeof(AddCategoryResponseDto), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<AddCategoryResponseDto>> AddCategory(AddCategoryRequestDto request)
        {
            var result = await mediator.Send(new AddCategory.Command(request));
            return CreatedAtAction(nameof(GetCategory), new { id = result.Id }, result);
        }

        [Authorize(Roles = "Administrator")]
        [HttpPut("{id}", Name = nameof(UpdateCategory))]
        [ProducesResponseType(typeof(UpdateCategoryResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<UpdateCategoryResponseDto>> UpdateCategory(int id, UpdateCategoryRequestDto request)
        {
            var result = await mediator.Send(new UpdateCategory.Command(id, request));
            return Ok(result);
        }

        [Authorize(Roles = "Administrator")]
        [HttpDelete("{id}", Name = nameof(DeleteCategory))]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<ActionResult> DeleteCategory(int id)
        {
            await mediator.Send(new DeleteCategory.Command(id));
            return NoContent();
        }
    }
}