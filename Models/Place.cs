using System;
using System.Collections.Generic;

namespace HotelProject.Models;

public partial class Place
{
    public int IdPlace { get; set; }

    /// <summary>
    /// Запись
    /// </summary>
    public string RecordNumber { get; set; } = null!;

    /// <summary>
    /// Заезд
    /// </summary>
    public DateOnly Arrive { get; set; }

    /// <summary>
    /// Выезд
    /// </summary>
    public DateOnly Exit { get; set; }

    /// <summary>
    /// Номер
    /// </summary>
    public int RoomId { get; set; }

    /// <summary>
    /// Бронь
    /// </summary>
    public int BookingId { get; set; }

    public virtual Booking Booking { get; set; } = null!;

    public virtual Room Room { get; set; } = null!;

    public virtual ICollection<Guest> Guests { get; set; } = new List<Guest>();
}
