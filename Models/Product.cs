using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebAPI_JWT.Models;
public class Product
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int ProductId { get; set; }
    public string ProductName { get; set; } = string.Empty; 
    public string ProductCategory { get; set; } = string.Empty;
    public decimal ProductPrice { get; set; }
    public int ProductStockQty { get; set; }
}