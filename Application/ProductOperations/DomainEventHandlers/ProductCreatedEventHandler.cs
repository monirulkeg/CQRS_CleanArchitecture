using Domain.Events;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.ProductOperations.DomainEventHandlers;

public class ProductCreatedEventHandler : INotificationHandler<ProductCreatedEvent>
{
    private readonly ILogger<ProductCreatedEventHandler> _logger;

    public ProductCreatedEventHandler(ILogger<ProductCreatedEventHandler> logger)
    {
        _logger = logger;
    }

    public Task Handle(ProductCreatedEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation($"Domain event: {notification.GetType().Name} informs Product with ID {notification.Product.Id} created.");

        return Task.CompletedTask;
    }
}
