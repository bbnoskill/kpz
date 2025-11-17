using GamifiedLearningPlatform.Data.Repositories;
using GamifiedLearningPlatform.Models;
using Microsoft.EntityFrameworkCore;

namespace GamifiedLearningPlatform.Data.CodeFirst;

public class CodeFirstStudentRepository : IStudentRepository
{
    private readonly AppDbContext _context;

    public CodeFirstStudentRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IReadOnlyList<Student>> LoadAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Students.ToListAsync(cancellationToken);
    }

    public async Task SaveGraphAsync(IEnumerable<Student> students, CancellationToken cancellationToken = default)
    {
        _context.Students.UpdateRange(students);
        await _context.SaveChangesAsync(cancellationToken);
    }
}