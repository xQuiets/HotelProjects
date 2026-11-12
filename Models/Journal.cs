using System;
using System.Collections.Generic;

namespace HotelProject.Models;

public partial class Journal
{
    public int IdJournal { get; set; }

    /// <summary>
    /// Документ
    /// </summary>
    public string DocumentNumber { get; set; } = null!;

    /// <summary>
    /// Количество
    /// </summary>
    public int Quantity { get; set; }

    /// <summary>
    /// Дата
    /// </summary>
    public DateOnly ServiceDate { get; set; }

    /// <summary>
    /// Бронь
    /// </summary>
    public int BookingId { get; set; }

    /// <summary>
    /// Услуга
    /// </summary>
    public int ServiceId { get; set; }

    public virtual Booking Booking { get; set; } = null!;

    public virtual Service Service { get; set; } = null!;
}
