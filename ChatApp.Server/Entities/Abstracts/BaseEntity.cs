using System.ComponentModel.DataAnnotations;
using ChatApp.Server.Entities.Interfaces;

namespace ChatApp.Server.Entities.Abstracts;

public abstract class BaseEntity : IBaseEntity
{
    [Key]
    public long Id { get; set; }
}