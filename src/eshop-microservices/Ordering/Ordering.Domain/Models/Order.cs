namespace Ordering.Domain.Models;

public sealed class Order : Aggregate<OrderId>
{
    private readonly List<OrderItem> _orderItems = [];
    public IReadOnlyList<OrderItem> OrderItems => _orderItems.AsReadOnly();
    public CustomerId CustomerId { get; private set; } = default!;
    public OrderName OrderName { get; private set; } = default!;
    public Address ShippingAddress { get; private set; } = default!;
    public Address BillingAddress { get; private set; } = default!;
    public Payment Payment { get; private set; } = default!;
    public OrderStatus Status { get; private set; } = OrderStatus.Pending;

    public decimal TotalPrice
    {
        get => _orderItems.Sum(item => item.Price * item.Quantity);
        private set { }
    }

    public static Order Create(OrderId id, CustomerId customerId, OrderName orderName, Address shippingAddress,
        Address billingAddress, Payment payment)
    {
        Order order = new()
        {
            Id = id,
            CustomerId = customerId,
            OrderName = orderName,
            ShippingAddress = shippingAddress,
            BillingAddress = billingAddress,
            Payment = payment
        };
        order.AddDomainEvent(new OrderCreatedEvent(order));

        return order;
    }

    public void Update(OrderName orderName, Address shippingAddress, Address billingAddress, Payment payment,
        OrderStatus orderStatus)
    {
        OrderName = orderName;
        ShippingAddress = shippingAddress;
        BillingAddress = billingAddress;
        Payment = payment;
        Status = orderStatus;

        AddDomainEvent(new OrderUpdatedEvent(this));
    }

    public void UpdateStatus(OrderStatus orderStatus)
    {
        Status = orderStatus;
        AddDomainEvent(new OrderUpdatedEvent(this));
    }

    public void Add(ProductId productId, int quantity, decimal price)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(quantity);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(price);

        var item = new OrderItem(Id, productId, quantity, price);
        _orderItems.Add(item);
    }

    public void Remove(ProductId productId)
    {
        var item = _orderItems.FirstOrDefault(i => i.ProductId == productId);
        if (item != null) _orderItems.Remove(item);
    }
}