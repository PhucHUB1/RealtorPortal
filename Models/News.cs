using System;
using System.Collections.Generic;

namespace RealEstate.Models;

public partial class News
{
    public int Id { get; set; }

    public string? Title { get; set; }

    public string? Intro { get; set; }

    public string? Content { get; set; }

    public string? MetaKey { get; set; }

    public DateTime? CreatedDate { get; set; }

    public string? ImageLink { get; set; }

    public string? Author { get; set; }

    public int? ViewNews { get; set; }

    public string? Status { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }
}
