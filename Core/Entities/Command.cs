using Core.Entities.Base;

namespace Core.Entities;

public class Command : BaseModel
{
    public string Name { get; set; } = string.Empty;
    public bool IsActive { get; set; }
}