using System.IO;
using System.Text.Json;
using GamifiedLearningPlatform.DTOs;
using GamifiedLearningPlatform.Mappers;
using GamifiedLearningPlatform.Models;

namespace GamifiedLearningPlatform.Services;

public class StudentDataService
{
    private readonly string _filePath;

    public StudentDataService(string filePath)
    {
        _filePath = filePath;
    }

    public async Task<List<Student>> LoadStudentsAsync()
    {
        if (!File.Exists(_filePath))
        {
            var sampleStudents = GenerateSampleStudents();
            await SaveStudentsAsync(sampleStudents);
            return sampleStudents;
        }

        try
        {
            using var fs = File.OpenRead(_filePath);
            var studentDtos = await JsonSerializer.DeserializeAsync<List<StudentDto>>(fs);
            return StudentMapper.Mapper.Map<List<Student>>(studentDtos);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Помилка завантаження студентів: {ex.Message}");
            return new List<Student>();
        }
    }

    public async Task SaveStudentsAsync(List<Student> students)
    {
        try
        {
            var studentDtos = StudentMapper.Mapper.Map<List<StudentDto>>(students);
            using var fs = File.Create(_filePath);
            await JsonSerializer.SerializeAsync(fs, studentDtos, new JsonSerializerOptions { WriteIndented = true });
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Помилка збереження студентів: {ex.Message}");
        }
    }

    private List<Student> GenerateSampleStudents()
    {
        return new List<Student>
        {
            new()
            {
                Id = Guid.NewGuid(), FirstName = "Василь", LastName = "Андрієвський", TotalXp = 1500, Level = 3,
                Badges = new List<string> { "Новачок", "Дослідник" },
                Assignments = new List<Assignment>
                {
                    new() { Id = Guid.NewGuid(), Title = "Лаб 1: Основи C#", XpAward = 100, IsCompleted = true },
                    new() { Id = Guid.NewGuid(), Title = "Тест 1", XpAward = 50, IsCompleted = true }
                }
            },
            new()
            {
                Id = Guid.NewGuid(), FirstName = "Марія", LastName = "Ковальчук", TotalXp = 2800, Level = 5,
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