using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebAPI_JWT.Models;
public class UserInfo
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int UserId { get; set; }
    public string UserName { get; set; } = string.Empty;
    public string UserPassword { get; set; } = string.Empty;
}