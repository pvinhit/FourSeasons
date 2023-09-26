using System;
using System.Collections.Generic;

namespace be.Models;

public partial class BookingDetail
{
    public int DetailId { get; set; }

    public int? BookingId { get; set; }

    public int? RoomId { get; set; }

    public virtual Booking? Booking { get; set; }

    public virtual Room? Room { get; set; }
}
