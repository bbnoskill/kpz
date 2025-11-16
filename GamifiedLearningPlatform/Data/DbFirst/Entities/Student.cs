namespace GamifiedLearningPlatform.Data.DbFirst.Entities;

public partial class Student
{
    public Guid Id { get; set; }

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public string Email { get; set; } = null!;

    public int TotalXp { get; set; }

    public int Level { get; set; }

    public virtual ICollection<Assignment> Assignments { get; } = new List<Assignment>();

    public virtual ICollection<StudentBadge> StudentBadges { get; } = new List<StudentBadge>();
}



