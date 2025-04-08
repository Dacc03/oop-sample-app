namespace ACME.OOP.Procurement.Domain.Model.ValueObjects;
/// <summary>
/// This value object represents a unique identifier for a product.
/// </summary>
public record ProductId
{
    public Guid Id { get; init; }
/// <summary>
///  Constructor to create a ProductId with a specific Guid.
/// </summary>
/// <param name="id"></param>
/// <exception cref="ArgumentException"></exception>
    public ProductId(Guid id)
    {
        if (id == Guid.Empty)
            throw new ArgumentException("ProductId cannot be empty.", nameof(id));
        Id = id;
    }
    /// <summary>
    ///  Returns a new ProducId      
    /// </summary>
    /// <returns> A new ProductId instamce with a unique indentifier</returns>
    public static ProductId New() => new(Guid.NewGuid());
    
}