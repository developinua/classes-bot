using System.ComponentModel.DataAnnotations;

namespace Core.Entities;

public class Discount
{
    [Range(0, long.MaxValue)]
    public long UserId { get; set; }
    [Range(0, long.MaxValue)]
    public long SubscriptionId { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    [Range(0, 100)]
    public decimal Percent { get; set; }
}