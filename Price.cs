using System;
using System.Collections.Generic;

namespace WebApplication1.Models;

public partial class Price
{
    public int Id { get; set; }

    public int TickerId { get; set; }

    public double? Price1 { get; set; }

    public DateTime? Date { get; set; }

    public virtual Ticker Ticker { get; set; } = null!;
}
