using System;
using System.Collections.Generic;

namespace be.Models;

public partial class RoomImg
{
    public int Id { get; set; }

    public int? RoomId { get; set; }

    public string Image { get; set; } = null!;

    public virtual Room? Room { get; set; }
}
