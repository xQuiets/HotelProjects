using System;
using System.Collections.Generic;

namespace HotelProject.Models;

public partial class Booking
{
    public int IdBooking { get; set; }

    /// <summary>
    /// Бронь 
    /// </summary>
    public int BookingNumber { get; set; }

    /// <summary>
    /// Дата заявки
    /// </summary>
    public DateOnly DateRequest { get; set; }

    /// <summary>
    /// Контактное лицо
    /// </summary>
    public string Contact { get; set; } = null!;

    /// <summary>
    /// организация
    /// </summary>
    public string OrganizationId { get; set; } = null!;

    public virtual ICollection<Check> Checks { get; set; } = new List<Check>();

    public virtual ICollection<Journal> Journals { get; set; } = new List<Journal>();

    public virtual Organization Organization { get; set; } = null!;

    public virtual ICollection<Place> Places { get; set; } = new List<Place>();
}
