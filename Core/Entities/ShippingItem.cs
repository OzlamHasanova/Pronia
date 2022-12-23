using System.ComponentModel.DataAnnotations;
using Microsoft.Build.Framework;
using RequiredAttribute = System.ComponentModel.DataAnnotations.RequiredAttribute;

namespace Core.Entities;

public class ShippingItem
{
    public int Id { get; set; }
    public string? Image { get; set; }
    [Required, MaxLength(100)]
    public string? Title { get; set; }
    [Required, MaxLength(200)]
    public string? Description { get; set; }
}
