using Application.Common.Interfaces;
using Domain.Entities;
using Domain.Events;
using MediatR;

namespace Application.ProductOperations.Commands;

public record CreateProductCommand(string Name, string Description, decimal Price) : IRequest<Guid>;

public class CreateProductCommandHandler(IApplicationDbContext context) : IRequestHandler<CreateProductCommand, Guid>
{
    public async Task<Guid> Handle(CreateProductCommand command, CancellationToken cancellationToken)
    {
        var productEntity = new Product(command.Name, command.Description, command.Price);

        productEntity.AddDomainEvent(new ProductCreatedEvent(productEntity));

        await context.Products.AddAsync(productEntity);
        await context.SaveChangesAsync(cancellationToken);
        return productEntity.Id;
    }
}
