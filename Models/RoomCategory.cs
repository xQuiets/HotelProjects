using System;
using System.Collections.Generic;

namespace HotelProject.Models;

public partial class RoomCategory
{
    public int IdCategory { get; set; }

    /// <summary>
    /// Название категории
    /// </summary>
    public string CategoryName { get; set; } = null!;

    /// <summary>
    /// цена за ночь
    /// 
    /// </summary>
    public decimal Price { get; set; }

    public virtual ICollection<Room> Rooms { get; set; } = new List<Room>();
}
