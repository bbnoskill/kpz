using GamifiedLearningPlatform.Data.CodeFirst.Entities;
using GamifiedLearningPlatform.Models;

namespace GamifiedLearningPlatform.Data.Mappers;

public static class StudentDomainMapper
{
    public static Student ToDomain(StudentEntity entity)
    {
        return new Student
        {
            Id = entity.Id,
            FirstName = entity.FirstName,
            LastName = entity.LastName,
            Email = entity.Email,
            TotalXp = entity.TotalXp,
            Level = entity.Level,
            Badges = entity.Badges.Select(b => b.Name).ToList(),
            Assignments = entity.Assignments.Select(a => new Assignment
            {
                Id = a.Id,
                Title = a.Title,
                XpAward = a.XpAward,
                IsCompleted = a.IsCompleted
            }).ToList()
        };
    }

    public static void ApplyChanges(StudentEntity entity, Student domain)
    {
        entity.FirstName = domain.FirstName;
        entity.LastName = domain.LastName;
        entity.Email = domain.Email;
        entity.TotalXp = domain.TotalXp;
        entity.Level = domain.Level;

        SyncAssignments(entity, domain);
        SyncBadges(entity, domain);
    }

    private static void SyncAssignments(StudentEntity entity, Student domain)
    {
        var existingAssignments = entity.Assignments.ToDictionary(a => a.Id, a => a);
        var payload = domain.Assignments ?? new List<Assignment>();

        // Update or add assignments (don't remove here - let repository handle deletions)
        foreach (var assignment in payload)
        {
            if (assignment.Id == Guid.Empty)
            {
                assignment.Id = Guid.NewGuid();
            }

            if (existingAssignments.TryGetValue(assignment.Id, out var existing))
            {
                existing.Title = assignment.Title;
                existing.XpAward = assignment.XpAward;
                existing.IsCompleted = assignment.IsCompleted;
            }
            else
            {
                entity.Assignments.Add(new AssignmentEntity
                {
                    Id = assignment.Id,
                    Title = assignment.Title,
                    XpAward = assignment.XpAward,
                    IsCompleted = assignment.IsCompleted,
                    StudentId = entity.Id
                });
            }
        }
    }

    private static void SyncBadges(StudentEntity entity, Student domain)
    {
        var payload = domain.Badges ?? new List<string>();

        entity.Badges.Clear();
        foreach (var name in payload)
        {
            entity.Badges.Add(new StudentBadgeEntity
            {
                Id = Guid.NewGuid(),
                Name = name,
                StudentId = entity.Id
            });
        }
    }
}

