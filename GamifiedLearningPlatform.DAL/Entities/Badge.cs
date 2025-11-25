using System;
using System.Collections.Generic;

namespace GamifiedLearningPlatform.DAL.Entities;

public partial class Badge
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
    public string? IconUrl { get; set; }
    public int XpRequired { get; set; }
    public DateTime CreatedAt { get; set; }

    public virtual ICollection<StudentBadge> StudentBadges { get; set; } = new List<StudentBadge>();
}