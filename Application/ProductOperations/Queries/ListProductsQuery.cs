using Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.ProductOperations.Queries;

public record ListProductsQuery : IRequest<List<ProductDto>>;

public class ListProductsQueryHandler : IRequestHandler<ListProductsQuery, List<ProductDto>>
{
    private readonly IApplicationDbContext _context;

    public ListProductsQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<ProductDto>> Handle(ListProductsQuery request, CancellationToken cancellationToken)
    {
        var products = await _context.Products
            .AsNoTracking()
            .Select(p => new ProductDto(p.Id, p.Name, p.Description, p.Price))
            .ToListAsync(cancellationToken: cancellationToken);

        return products;
    }
}
