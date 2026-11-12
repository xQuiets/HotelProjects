using System;
using System.Collections.Generic;

namespace HotelProject.Models;

public partial class Check
{
    public int IdCheck { get; set; }

    /// <summary>
    /// Счёт
    /// </summary>
    public string CheckNumber { get; set; } = null!;

    /// <summary>
    /// Дата выставления
    /// </summary>
    public DateOnly Date { get; set; }

    /// <summary>
    /// счет к оплате
    /// </summary>
    public decimal Amount { get; set; }

    /// <summary>
    /// Бронь
    /// </summary>
    public int BookingId { get; set; }

    public virtual Booking Booking { get; set; } = null!;
}
