using GamifiedLearningPlatform.Models;
using StudentEntity = GamifiedLearningPlatform.Data.DbFirst.Entities.Student;
using AssignmentEntity = GamifiedLearningPlatform.Data.DbFirst.Entities.Assignment;
using StudentBadgeEntity = GamifiedLearningPlatform.Data.DbFirst.Entities.StudentBadge;

namespace GamifiedLearningPlatform.Data.Mappers;

public static class DbFirstStudentMapper
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
            Badges = entity.StudentBadges.Select(b => b.Name).ToList(),
            Assignments = entity.Assignments.Select(a => new Assignment
            {
                Id = a.Id,
                Title = a.Title,
                XpAward = a.XpAward,
                IsCompleted = a.IsCompleted
            }).ToList()
        };
    }

    public static StudentEntity CreateNew(Student domain)
    {
        var entity = new StudentEntity
        {
            Id = domain.Id == Guid.Empty ? Guid.NewGuid() : domain.Id
        };
        ApplyChanges(entity, domain);
        return entity;
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
        var assignments = domain.Assignments ?? new List<Assignment>();
        var existing = entity.Assignments.ToDictionary(a => a.Id, a => a);

        // Update or add assignments (don't remove here - let repository handle deletions)
        foreach (var assignment in assignments)
        {
            if (assignment.Id == Guid.Empty)
            {
                assignment.Id = Guid.NewGuid();
            }

            if (existing.TryGetValue(assignment.Id, out var tracked))
            {
                tracked.Title = assignment.Title;
                tracked.XpAward = assignment.XpAward;
                tracked.IsCompleted = assignment.IsCompleted;
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
        var badges = domain.Badges ?? new List<string>();
        entity.StudentBadges.Clear();
        foreach (var badge in badges)
        {
            entity.StudentBadges.Add(new StudentBadgeEntity
            {
                Id = Guid.NewGuid(),
                Name = badge,
                StudentId = entity.Id
            });
        }
    }
}

