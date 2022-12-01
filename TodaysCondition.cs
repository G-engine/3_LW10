using System;
using System.Collections.Generic;

namespace WebApplication1.Models;

public partial class TodaysCondition
{
    public int Id { get; set; }

    public int TickerId { get; set; }

    public bool? State { get; set; }

    public virtual Ticker Ticker { get; set; } = null!;
}
