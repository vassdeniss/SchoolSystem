using System;
using System.Collections.Generic;

namespace SchoolSystem.Infrastructure.Models;

public partial class Address
{
    public int Id { get; set; }

    public string City { get; set; } = null!;

    public string Street { get; set; } = null!;

    public string StreetNumber { get; set; } = null!;

    public string? PostalCode { get; set; }

    public virtual ICollection<School> Schools { get; set; } = new List<School>();
}
