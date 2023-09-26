using System;
using System.Collections.Generic;

namespace be.Models;

public partial class Category
{
    public int CategoryId { get; set; }

    public string CategoryName { get; set; } = null!;

    public int MaxPeople { get; set; }

    public double Size { get; set; }

    public string? Image { get; set; }

    public string? Description { get; set; }

    public bool Status { get; set; }

    public virtual ICollection<Room> Rooms { get; set; } = new List<Room>();
}
