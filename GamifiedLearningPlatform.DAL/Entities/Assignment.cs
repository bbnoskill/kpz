using System;
using System.Collections.Generic;

namespace GamifiedLearningPlatform.DAL.Entities;

public partial class Assignment
{
    public Guid Id { get; set; }

    public string Title { get; set; } = null!;

    public int XpAward { get; set; }

    public bool IsCompleted { get; set; }

    public Guid StudentId { get; set; }

    public virtual Student Student { get; set; } = null!;
}
