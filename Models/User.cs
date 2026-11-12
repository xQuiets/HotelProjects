using System;
using System.Collections.Generic;

namespace HotelProject.Models;

public partial class User
{
    /// <summary>
    /// id пользователя
    /// </summary>
    public int IdUser { get; set; }

    /// <summary>
    /// логин пользователя
    /// </summary>
    public string Login { get; set; } = null!;

    public string PasswordHash { get; set; } = null!;

    /// <summary>
    /// роль пользователя в системе по умолчанию user
    /// 
    /// </summary>
    public string Role { get; set; } = null!;

    /// <summary>
    /// количество неудачных входов
    /// 
    /// </summary>
    public int Failed { get; set; }

    public bool IsBlocked { get; set; }
}
