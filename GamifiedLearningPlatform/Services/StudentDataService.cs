using GamifiedLearningPlatform.Models;
using GamifiedLearningPlatform.Data.Repositories;

namespace GamifiedLearningPlatform.Services;

public class StudentDataService
{
    private readonly IStudentRepository _repository;
    private readonly bool _applySeedData;

    public StudentDataService(IStudentRepository repository, bool applySeedData)
    {
        _repository = repository;
        _applySeedData = applySeedData;
    }

    public async Task<List<Student>> LoadStudentsAsync()
    {
        var students = await _repository.LoadAsync();
        if (_applySeedData && students.Count == 0)
        {
            var sampleStudents = GenerateSampleStudents();
            await _repository.SaveGraphAsync(sampleStudents);
            return sampleStudents;
        }

        return students.ToList();
    }

    public Task SaveStudentsAsync(List<Student> students)
    {
        return _repository.SaveGraphAsync(students);
    }

    private List<Student> GenerateSampleStudents()
    {
        return new List<Student>
        {
            new()
            {
                Id = Guid.NewGuid(), FirstName = "Василь", LastName = "Андрієвський", Email = "vasyl.andrievskyi@example.com", TotalXp = 1500, Level = 3,
                Badges = new List<string> { "Новачок", "Дослідник" },
                Assignments = new List<Assignment>
                {
                    new() { Id = Guid.NewGuid(), Title = "Лаб 1: Основи C#", XpAward = 100, IsCompleted = true },
                    new() { Id = Guid.NewGuid(), Title = "Тест 1", XpAward = 50, IsCompleted = true }
                }
            },
            new()
            {
                Id = Guid.NewGuid(), FirstName = "Марія", LastName = "Ковальчук", Email = "maria.kovalchuk@example.com", TotalXp = 2800, Level = 5,
                Badges = new List<string> { "Новачок", "Майстер Алгоритмів", "Командний Гравець" },
                Assignments = new List<Assignment>
                {
                    new() { Id = Guid.NewGuid(), Title = "Лаб 1: Основи C#", XpAward = 100, IsCompleted = true },
                    new() { Id = Guid.NewGuid(), Title = "Проєкт 'Калькулятор'", XpAward = 300, IsCompleted = true }
                }
            }
        };
    }
}