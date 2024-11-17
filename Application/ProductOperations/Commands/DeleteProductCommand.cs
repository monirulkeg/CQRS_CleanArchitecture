using Application.Common.Interfaces;
using Ardalis.GuardClauses;
using Domain.Entities;
using MediatR;

namespace Application.ProductOperations.Commands;

public record DeleteProductCommand(Guid Id) : IRequest;


public class DeleteProductCommandHandler : IRequestHandler<DeleteProductCommand>
{
    private readonly IApplicationDbContext _context;

    public DeleteProductCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task Handle(DeleteProductCommand request, CancellationToken cancellationToken)
    {
        var product = await _context.Products.FindAsync(request.Id)
            ?? throw new NotFoundException(request.Id.ToString(), nameof(Product));

        _context.Products.Remove(product);
        await _context.SaveChangesAsync(cancellationToken);
    }
}
