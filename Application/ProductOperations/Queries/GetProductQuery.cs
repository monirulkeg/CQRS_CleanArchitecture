using Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.ProductOperations.Queries;

public record GetProductQuery(Guid Id) : IRequest<ProductDto>;

public class GetProductQueryHandler : IRequestHandler<GetProductQuery, ProductDto>
{
    private readonly IApplicationDbContext _context;

    public GetProductQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<ProductDto?> Handle(GetProductQuery request, CancellationToken cancellationToken)
    {
        var product = await _context.Products
            .AsNoTracking()
            .Where(p => p.Id == request.Id)
            .Select(p => new ProductDto(p.Id, p.Name, p.Description, p.Price))
            .SingleOrDefaultAsync(cancellationToken: cancellationToken);

        return product;
    }
}
