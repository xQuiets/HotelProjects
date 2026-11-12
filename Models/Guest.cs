using System;
using System.Collections.Generic;

namespace HotelProject.Models;

public partial class Guest
{
    public int IdGuest { get; set; }

    /// <summary>
    /// Карта гостя
    /// </summary>
    public string GuestCard { get; set; } = null!;

    /// <summary>
    /// полное имя фамилия отчество
    /// </summary>
    public string FullName { get; set; } = null!;

    /// <summary>
    /// Дата рождения
    /// </summary>
    public DateOnly? BirthDate { get; set; }

    public virtual ICollection<Place> Places { get; set; } = new List<Place>();
}
