using Eshop.Domain;
using Eshop.Persistence;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Eshop.WebApi.Features.Orders
{
    public class GetCart
    {
        public record Query(IEnumerable<GetCartRequestDto> Request) : IRequest<GetCartResponseDto>;

        public class Validator : AbstractValidator<Query>
        {
            public Validator()
            {
                RuleForEach(x => x.Request).ChildRules(x =>
                {
                    x.RuleFor(i => i.ProductId).GreaterThan(0);
                    x.RuleFor(i => i.Quantity).GreaterThan(0);
                });
                RuleFor(x => x.Request)
                    .NotEmpty()
                    .Must(items => items.Select(i => i.ProductId).Distinct().Count() == items.Count())
                    .WithMessage("Items must have unique ProductId.");
            }
        }

        public class Handler : IRequestHandler<Query, GetCartResponseDto>
        {
            private readonly EshopDbContext dbContext;

            public Handler(EshopDbContext dbContext)
            {
                this.dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            }

            public async Task<GetCartResponseDto> Handle(Query query, CancellationToken cancellationToken)
            {
                var productIds = query.Request.Select(x => x.ProductId).Distinct().ToList();

                var products = await dbContext.ProductsViews
                    .Where(x => productIds.Any(i => i == x.Id))
                    .ToListAsync(cancellationToken);

                return GetCartResponseDto.Map(products, query.Request);
            }
        }
    }

    public class GetCartRequestDto
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; }
    }

    public class GetCartResponseDto
    {
        public required IEnumerable<GetCartItemResponseDto> Items { get; set; }
        public decimal TotalPrice { get; set; }

        public class GetCartItemResponseDto
        {
            public int ProductId { get; set; }
            public required string ProductTitle { get; set; }
            public int Quantity { get; set; }
            public decimal Price { get; set; }
            public decimal TotalPrice { get; set; }
        }

        internal static GetCartResponseDto Map(IEnumerable<Product> products, IEnumerable<GetCartRequestDto> request)
        {
            var items = products.Select(x =>
            {
                var quantity = request.First(r => r.ProductId == x.Id).Quantity;

                return new GetCartItemResponseDto
                {
                    ProductId = x.Id,
                    ProductTitle = x.Title,
                    Quantity = quantity,
                    Price = x.Price,
                    TotalPrice = quantity * x.Price
                };
            }).ToList();

            return new GetCartResponseDto
            {
                Items = items,
                TotalPrice = items.Sum(x => x.TotalPrice)
            };
        }
    }
}