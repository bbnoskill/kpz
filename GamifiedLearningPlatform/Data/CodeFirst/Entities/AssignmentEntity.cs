using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GamifiedLearningPlatform.Data.CodeFirst.Entities;

public class AssignmentEntity
{
    [Key]
    public Guid Id { get; set; }

    [Required]
    [MaxLength(200)]
    public string Title { get; set; } = string.Empty;

    public int XpAward { get; set; }

    public bool IsCompleted { get; set; }

    [ForeignKey(nameof(Student))]
    public Guid StudentId { get; set; }

    public StudentEntity Student { get; set; } = null!;
}



