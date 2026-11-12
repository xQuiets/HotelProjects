using System;
using System.Collections.Generic;

namespace HotelProject.Models;

public partial class Room
{
    public int IdRoom { get; set; }

    /// <summary>
    /// Обозначение
    /// </summary>
    public string CodeRoom { get; set; } = null!;

    /// <summary>
    /// Корпус
    /// </summary>
    public string Block { get; set; } = null!;

    /// <summary>
    /// Адрес
    /// </summary>
    public string Address { get; set; } = null!;

    /// <summary>
    /// Номер
    /// </summary>
    public int RoomNumber { get; set; }

    /// <summary>
    /// Мест
    /// </summary>
    public int Capacity { get; set; }

    /// <summary>
    /// Категория
    /// </summary>
    public int CategoryId { get; set; }

    public virtual RoomCategory Category { get; set; } = null!;

    public virtual ICollection<Place> Places { get; set; } = new List<Place>();
}
