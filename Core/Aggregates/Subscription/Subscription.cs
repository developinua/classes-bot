using System.ComponentModel.DataAnnotations;
using Core.Entities.Base;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Core.Aggregates.Subscription;

public class Subscription : BaseModel
{
    [StringLength(250)]
    public string Name { get; set; } = string.Empty;
    [StringLength(1000)]
    public string? Description { get; set; }
    [Range(0, int.MaxValue)]
    public decimal Price { get; set; }
    [Range(0, 100)]
    public int ClassesCount { get; set; }
    [JsonConverter(typeof(StringEnumConverter))]
    public SubscriptionType Type { get; set; }
    public bool IsActive { get; set; }

    // public decimal GetPriceWithDiscount() =>
    //     DiscountPercent == 0
    //         ? Price
    //         : Price - Price * DiscountPercent / 100;

    public override string ToString()
    {
        // var priceText = DiscountPercent == 0
        //     ? $"Price: {Price}\n"
        //     : $"Price with discount: {GetPriceWithDiscount()}\n";

        return $"Subscription: {Name}\n" +
               // $"{priceText}" +
               $"Description: {Description}\n" +
               $"SubscriptionType: {Type}\n" +
               $"Total Classes: {ClassesCount}\n";
    }
}