using Domain.Common;
using Domain.Entities;

namespace Domain.Events;

public class ProductCreatedEvent : BaseEvent
{
    public ProductCreatedEvent(Product product)
    {
        Product = product;
    }

    public Product Product { get; }
}
