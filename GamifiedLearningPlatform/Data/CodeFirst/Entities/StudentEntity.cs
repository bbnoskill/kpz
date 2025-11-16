using System.ComponentModel.DataAnnotations;

namespace GamifiedLearningPlatform.Data.CodeFirst.Entities;

public class StudentEntity
{
    [Key]
    public Guid Id { get; set; }

    [Required]
    [MaxLength(100)]
    public string FirstName { get; set; } = string.Empty;

    [Required]
    [MaxLength(100)]
    public string LastName { get; set; } = string.Empty;

    [Required]
    [MaxLength(200)]
    public string Email { get; set; } = string.Empty;

    public int TotalXp { get; set; }

    public int Level { get; set; }

    public ICollection<AssignmentEntity> Assignments { get; set; } = new List<AssignmentEntity>();

    public ICollection<StudentBadgeEntity> Badges { get; set; } = new List<StudentBadgeEntity>();
}



