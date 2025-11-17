using GamifiedLearningPlatform.Models;

namespace GamifiedLearningPlatform.Data.Repositories;

public interface IStudentRepository
{
    Task<IReadOnlyList<Student>> LoadAsync(CancellationToken cancellationToken = default);
    Task SaveGraphAsync(IEnumerable<Student> students, CancellationToken cancellationToken = default);
}





