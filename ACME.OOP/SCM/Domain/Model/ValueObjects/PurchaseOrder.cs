using System.Collections.ObjectModel;
using ACME.OOP.Procurement.Domain.Model.ValueObjects;
using ACME.OOP.SCM.Domain.Model.ValueObjects;
using ACME.OOP.Shared.Domain.Model.ValueObjects;

namespace ACME.OOP.Domain.Model.Aggregates; 
/// <summary>
/// 
/// </summary>
/// <param name="orderNumber"></param>
/// <param name="supplierId"></param>
/// <param name="orderDate"></param>
/// <param name="currency"></param>
public class PurchaseOrder(string orderNumber, SupplierId supplierId, DateTime orderDate, string currency)
{
    private readonly List<PurchaseOrderItem> _items = [];
    public string OrderNumber { get; } = orderNumber ?? throw new ArgumentNullException(nameof(orderNumber));
    public SupplierId SupplierId { get; } = supplierId ?? throw new ArgumentNullException(nameof(supplierId));
    public DateTime OrderDate { get; } = orderDate;

    public string Currency { get; } = string.IsNullOrWhiteSpace(currency) ||
                                      currency.Length != 3
        ? throw new ArgumentException("Currency must be a 3-letter ISO code.", nameof(currency))
        : currency;
    
    public IReadOnlyList<PurchaseOrderItem> Items => _items.AsReadOnly();
    /// <summary>
    /// Constructor for creating a new PurchaseOrder.
    /// </summary>
    /// <param name="orderNumber">The unique identifier for the purchase order</param>
    /// <param name="supplierId">the unique identifier for the supplier </param>
    /// <param name="currency"> Thrown when quantity</param>
    
    public PurchaseOrder(string orderNumber, SupplierId supplierId, string currency) 
        : this(orderNumber, supplierId, DateTime.UtcNow, currency)
    {
    }

    public void AddItem(ProductId productId, int quantity, decimal unitPriceAmount)
    {
        ArgumentNullException.ThrowIfNull(productId, nameof(productId));
        if (quantity <= 0)
            throw new ArgumentOutOfRangeException(nameof(quantity), quantity, "Quantity must be greater than zero.");
        if (unitPriceAmount <= 0)
            throw new ArgumentOutOfRangeException(nameof(unitPriceAmount), unitPriceAmount, "UnitPrice must be greater than zero.");
        
        var unitPrice = new Money(unitPriceAmount, Currency);
        var item = new PurchaseOrderItem(productId, quantity, unitPrice);
        _items.Add(item);
    }
    /// <summary>
    /// Calculate the total amount of the purchase order by summing the subtotal of all items.
    /// </summary>
    /// <returns> A </returns>
    public Money CalculateTotal()
    {
        var total = _items.Sum(item => item.CalculateSubtotal().Amount);
        return new Money(total, Currency);
            
    }
}