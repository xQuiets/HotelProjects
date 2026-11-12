using System;
using System.Collections.Generic;

namespace HotelProject.Models;

public partial class Organization
{
    /// <summary>
    /// ID организации
    /// </summary>
    public string IdOrganization { get; set; } = null!;

    /// <summary>
    /// Название
    /// </summary>
    public string Name { get; set; } = null!;

    /// <summary>
    /// Адрес
    /// </summary>
    public string Address { get; set; } = null!;

    /// <summary>
    /// Телефон
    /// </summary>
    public string Phone { get; set; } = null!;

    public virtual ICollection<Booking> Bookings { get; set; } = new List<Booking>();
}
