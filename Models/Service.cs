using System;
using System.Collections.Generic;

namespace HotelProject.Models;

public partial class Service
{
    public int IdService { get; set; }

    /// <summary>
    /// Код услуги
    /// </summary>
    public string ServiceCode { get; set; } = null!;

    /// <summary>
    /// Услуга
    /// </summary>
    public string ServiceName { get; set; } = null!;

    /// <summary>
    /// единица
    /// </summary>
    public string Unit { get; set; } = null!;

    /// <summary>
    /// цена
    /// </summary>
    public decimal Price { get; set; }

    public virtual ICollection<Journal> Journals { get; set; } = new List<Journal>();
}
