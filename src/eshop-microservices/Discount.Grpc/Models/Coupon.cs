using System.ComponentModel.DataAnnotations;

namespace Discount.Grpc.Models;

public sealed class Coupon
{
    public int Id { get; set; }
    [MaxLength(100)]
    public string ProductName { get; set; } = string.Empty;
    [MaxLength(250)]
    public string Description { get; set; } = string.Empty;
    public int Amount { get; set; }
}