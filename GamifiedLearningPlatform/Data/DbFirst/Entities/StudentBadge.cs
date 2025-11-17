namespace GamifiedLearningPlatform.Data.DbFirst.Entities;

public partial class StudentBadge
{
    public Guid Id { get; set; }

    public Guid StudentId { get; set; }

    public string Name { get; set; } = null!;

    public virtual Student Student { get; set; } = null!;
}





