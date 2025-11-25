using System;
using System.Collections.Generic;

namespace GamifiedLearningPlatform.DAL.Entities;

public partial class StudentBadge
{
    public Guid Id { get; set; }
    public Guid StudentId { get; set; }
    public Guid BadgeId { get; set; }
    public DateTime EarnedAt { get; set; }

    public virtual Student Student { get; set; } = null!;
    public virtual Badge Badge { get; set; } = null!;
}