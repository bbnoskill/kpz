using System;
using System.Collections.Generic;

namespace GamifiedLearningPlatform.DAL.Entities;

public partial class Student
{
    public Guid Id { get; set; }
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string Email { get; set; } = null!;
    public int TotalXp { get; set; }
    public int Level { get; set; }
    public DateTime CreatedAt { get; set; }

    public virtual ICollection<Assignment> Assignments { get; set; } = new List<Assignment>();
    public virtual ICollection<StudentBadge> StudentBadges { get; set; } = new List<StudentBadge>();
}