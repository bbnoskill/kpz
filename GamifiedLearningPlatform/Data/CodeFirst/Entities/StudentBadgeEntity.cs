using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GamifiedLearningPlatform.Data.CodeFirst.Entities;

public class StudentBadgeEntity
{
    [Key]
    public Guid Id { get; set; }

    [Required]
    [MaxLength(150)]
    public string Name { get; set; } = string.Empty;

    [ForeignKey(nameof(Student))]
    public Guid StudentId { get; set; }

    public StudentEntity Student { get; set; } = null!;
}





