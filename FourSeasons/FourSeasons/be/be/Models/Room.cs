using System;
using System.Collections.Generic;

namespace be.Models;

public partial class Room
{
    public int RoomId { get; set; }

    public int? CategoryId { get; set; }

    public string RoomName { get; set; } = null!;

    public decimal Price { get; set; }

    public string? Description { get; set; }

    public bool Status { get; set; }

    public virtual ICollection<BookingDetail> BookingDetails { get; set; } = new List<BookingDetail>();

    public virtual Category? Category { get; set; }

    public virtual ICollection<RoomImg> RoomImgs { get; set; } = new List<RoomImg>();
}
