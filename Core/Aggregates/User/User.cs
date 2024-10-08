﻿using Core.Entities.Base;

namespace Core.Aggregates.User;

public class User : BaseModel
{
    public string? NickName { get; set; }
    public bool IsActive { get; set; }

    public UserProfile UserProfile { get; set; } = null!;
}