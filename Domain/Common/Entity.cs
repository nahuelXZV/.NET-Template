using System.ComponentModel.DataAnnotations;

namespace Domain.Common;

public class Entity
{
    [Key]
    public long Id { get; set; }
    public bool Eliminado { get; set; }
}