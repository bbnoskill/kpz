using GamifiedLearningPlatform.Data.DbFirst;
using GamifiedLearningPlatform.Data.Mappers;
using GamifiedLearningPlatform.Models;
using Microsoft.EntityFrameworkCore;

namespace GamifiedLearningPlatform.Data.Repositories;

public class EfDbFirstStudentRepository : IStudentRepository
{
    private readonly IDbContextFactory<GamifiedLearningDbFirstContext> _contextFactory;

    public EfDbFirstStudentRepository(IDbContextFactory<GamifiedLearningDbFirstContext> contextFactory)
    {
        _contextFactory = contextFactory;
    }

    public async Task<IReadOnlyList<Student>> LoadAsync(CancellationToken cancellationToken = default)
    {
        await using var context = await _contextFactory.CreateDbContextAsync(cancellationToken);
        var students = await context.Students
            .Include(s => s.Assignments)
            .Include(s => s.StudentBadges)
            .AsNoTracking()
            .ToListAsync(cancellationToken);

        return students.Select(DbFirstStudentMapper.ToDomain).ToList();
    }

    public async Task SaveGraphAsync(IEnumerable<Student> students, CancellationToken cancellationToken = default)
    {
        var payload = students.Select(CloneStudentWithId).ToList();

        await using var context = await _contextFactory.CreateDbContextAsync(cancellationToken);
        var trackedStudents = await context.Students
            .Include(s => s.Assignments)
            .Include(s => s.StudentBadges)
            .ToListAsync(cancellationToken);

        foreach (var student in payload)
        {
            var entity = trackedStudents.FirstOrDefault(s => s.Id == student.Id);
            if (entity == null)
            {
                entity = DbFirstStudentMapper.CreateNew(student);
                context.Students.Add(entity);
                trackedStudents.Add(entity);
            }
            else
            {
                DbFirstStudentMapper.ApplyChanges(entity, student);
            }
        }

        // Handle deletions: remove assignments that are no longer in the payload
        foreach (var student in trackedStudents)
        {
            var payloadStudent = payload.FirstOrDefault(s => s.Id == student.Id);
            if (payloadStudent == null)
            {
                // Student should be deleted - cascade will handle children
                context.Students.Remove(student);
            }
            else
            {
                // Remove assignments that are no longer in the payload
                var payloadAssignmentIds = payloadStudent.Assignments
                    .Select(a => a.Id == Guid.Empty ? Guid.NewGuid() : a.Id)
                    .ToHashSet();
                
                var assignmentsToRemove = student.Assignments
                    .Where(a => !payloadAssignmentIds.Contains(a.Id))
                    .ToList();
                
                // Use context to remove - only delete if entity exists in database
                foreach (var assignmentToRemove in assignmentsToRemove)
                {
                    var entry = context.Entry(assignmentToRemove);
                    // Only delete if the entity was loaded from database (not newly added)
                    if (entry.State == Microsoft.EntityFrameworkCore.EntityState.Unchanged || 
                        entry.State == Microsoft.EntityFrameworkCore.EntityState.Modified)
                    {
                        context.Set<DbFirst.Entities.Assignment>().Remove(assignmentToRemove);
                    }
                    else if (entry.State == Microsoft.EntityFrameworkCore.EntityState.Added)
                    {
                        // If it was just added, just remove from collection without deleting from DB
                        student.Assignments.Remove(assignmentToRemove);
                    }
                }
            }
        }

        var payloadIds = payload.Select(s => s.Id).ToHashSet();
        var toRemove = trackedStudents.Where(s => !payloadIds.Contains(s.Id)).ToList();
        if (toRemove.Count > 0)
        {
            context.Students.RemoveRange(toRemove);
        }

        await context.SaveChangesAsync(cancellationToken);
    }

    private static Student CloneStudentWithId(Student student)
    {
        if (student.Id == Guid.Empty)
        {
            student.Id = Guid.NewGuid();
        }

        return student;
    }
}

