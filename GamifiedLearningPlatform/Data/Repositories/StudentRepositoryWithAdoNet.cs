using GamifiedLearningPlatform.Models;
using Microsoft.Data.SqlClient;

namespace GamifiedLearningPlatform.Data.Repositories;

public class StudentRepositoryWithAdoNet : IStudentRepository
{
    private readonly string _connectionString;

    public StudentRepositoryWithAdoNet(string connectionString)
    {
        if (string.IsNullOrWhiteSpace(connectionString))
        {
            throw new ArgumentException("Connection string must be provided.", nameof(connectionString));
        }

        _connectionString = connectionString;
    }

    public async Task<IReadOnlyList<Student>> LoadAsync(CancellationToken cancellationToken = default)
    {
        await using var connection = new SqlConnection(_connectionString);
        await connection.OpenAsync(cancellationToken);

        var students = await ReadStudentsAsync(connection, cancellationToken);
        await EnrichWithBadgesAsync(connection, students, cancellationToken);
        await EnrichWithAssignmentsAsync(connection, students, cancellationToken);

        return students.Values.ToList();
    }

    public async Task SaveGraphAsync(IEnumerable<Student> students, CancellationToken cancellationToken = default)
    {
        var payload = students.Select(CloneStudent).ToList();

        await using var connection = new SqlConnection(_connectionString);
        await connection.OpenAsync(cancellationToken);

        await using var transaction = (SqlTransaction)await connection.BeginTransactionAsync(cancellationToken);
        try
        {
            var existingIds = await LoadExistingStudentIdsAsync(connection, transaction, cancellationToken);
            var incomingIds = payload.Select(s => s.Id).ToHashSet();

            foreach (var student in payload)
            {
                await UpsertStudentAsync(connection, transaction, student, cancellationToken);
                await ReplaceBadgesAsync(connection, transaction, student, cancellationToken);
                await ReplaceAssignmentsAsync(connection, transaction, student, cancellationToken);
            }

            foreach (var deletedId in existingIds.Where(id => !incomingIds.Contains(id)))
            {
                await DeleteStudentAsync(connection, transaction, deletedId, cancellationToken);
            }

            await transaction.CommitAsync(cancellationToken);
        }
        catch
        {
            await transaction.RollbackAsync(cancellationToken);
            throw;
        }
    }

    private static Student CloneStudent(Student student)
    {
        if (student.Id == Guid.Empty)
        {
            student.Id = Guid.NewGuid();
        }

        return student;
    }

    private static async Task<Dictionary<Guid, Student>> ReadStudentsAsync(SqlConnection connection, CancellationToken cancellationToken)
    {
        const string sql = "SELECT Id, FirstName, LastName, Email, TotalXp, Level FROM Students";
        await using var command = new SqlCommand(sql, connection);
        await using var reader = await command.ExecuteReaderAsync(cancellationToken);

        var result = new Dictionary<Guid, Student>();
        while (await reader.ReadAsync(cancellationToken))
        {
            var student = new Student
            {
                Id = reader.GetGuid(0),
                FirstName = reader.GetString(1),
                LastName = reader.GetString(2),
                Email = reader.GetString(3),
                TotalXp = reader.GetInt32(4),
                Level = reader.GetInt32(5)
            };
            result[student.Id] = student;
        }

        return result;
    }

    private static async Task EnrichWithBadgesAsync(SqlConnection connection, Dictionary<Guid, Student> students, CancellationToken cancellationToken)
    {
        if (students.Count == 0) return;

        const string sql = "SELECT StudentId, Name FROM StudentBadges";
        await using var command = new SqlCommand(sql, connection);
        await using var reader = await command.ExecuteReaderAsync(cancellationToken);
        while (await reader.ReadAsync(cancellationToken))
        {
            var studentId = reader.GetGuid(0);
            if (!students.TryGetValue(studentId, out var student)) continue;
            student.Badges.Add(reader.GetString(1));
        }
    }

    private static async Task EnrichWithAssignmentsAsync(SqlConnection connection, Dictionary<Guid, Student> students, CancellationToken cancellationToken)
    {
        if (students.Count == 0) return;

        const string sql = "SELECT Id, StudentId, Title, XpAward, IsCompleted FROM Assignments";
        await using var command = new SqlCommand(sql, connection);
        await using var reader = await command.ExecuteReaderAsync(cancellationToken);
        while (await reader.ReadAsync(cancellationToken))
        {
            var studentId = reader.GetGuid(1);
            if (!students.TryGetValue(studentId, out var student)) continue;
            student.Assignments.Add(new Assignment
            {
                Id = reader.GetGuid(0),
                Title = reader.GetString(2),
                XpAward = reader.GetInt32(3),
                IsCompleted = reader.GetBoolean(4)
            });
        }
    }

    private static async Task<IReadOnlyCollection<Guid>> LoadExistingStudentIdsAsync(SqlConnection connection, SqlTransaction transaction, CancellationToken cancellationToken)
    {
        const string sql = "SELECT Id FROM Students";
        await using var command = new SqlCommand(sql, connection, transaction);
        await using var reader = await command.ExecuteReaderAsync(cancellationToken);
        var ids = new List<Guid>();
        while (await reader.ReadAsync(cancellationToken))
        {
            ids.Add(reader.GetGuid(0));
        }

        return ids;
    }

    private static async Task UpsertStudentAsync(SqlConnection connection, SqlTransaction transaction, Student student, CancellationToken cancellationToken)
    {
        const string sql = @"
IF EXISTS (SELECT 1 FROM Students WHERE Id = @Id)
BEGIN
    UPDATE Students
    SET FirstName = @FirstName,
        LastName = @LastName,
        Email = @Email,
        TotalXp = @TotalXp,
        Level = @Level
    WHERE Id = @Id;
END
ELSE
BEGIN
    INSERT INTO Students (Id, FirstName, LastName, Email, TotalXp, Level)
    VALUES (@Id, @FirstName, @LastName, @Email, @TotalXp, @Level);
END";

        await using var command = new SqlCommand(sql, connection, transaction);
        command.Parameters.AddWithValue("@Id", student.Id);
        command.Parameters.AddWithValue("@FirstName", student.FirstName);
        command.Parameters.AddWithValue("@LastName", student.LastName);
        command.Parameters.AddWithValue("@Email", student.Email);
        command.Parameters.AddWithValue("@TotalXp", student.TotalXp);
        command.Parameters.AddWithValue("@Level", student.Level);
        await command.ExecuteNonQueryAsync(cancellationToken);
    }

    private static async Task ReplaceBadgesAsync(SqlConnection connection, SqlTransaction transaction, Student student, CancellationToken cancellationToken)
    {
        const string deleteSql = "DELETE FROM StudentBadges WHERE StudentId = @StudentId";
        await using (var deleteCommand = new SqlCommand(deleteSql, connection, transaction))
        {
            deleteCommand.Parameters.AddWithValue("@StudentId", student.Id);
            await deleteCommand.ExecuteNonQueryAsync(cancellationToken);
        }

        const string insertSql = "INSERT INTO StudentBadges (Id, StudentId, Name) VALUES (@Id, @StudentId, @Name)";
        foreach (var badge in student.Badges)
        {
            await using var insertCommand = new SqlCommand(insertSql, connection, transaction);
            insertCommand.Parameters.AddWithValue("@Id", Guid.NewGuid());
            insertCommand.Parameters.AddWithValue("@StudentId", student.Id);
            insertCommand.Parameters.AddWithValue("@Name", badge);
            await insertCommand.ExecuteNonQueryAsync(cancellationToken);
        }
    }

    private static async Task ReplaceAssignmentsAsync(SqlConnection connection, SqlTransaction transaction, Student student, CancellationToken cancellationToken)
    {
        const string deleteSql = "DELETE FROM Assignments WHERE StudentId = @StudentId";
        await using (var deleteCommand = new SqlCommand(deleteSql, connection, transaction))
        {
            deleteCommand.Parameters.AddWithValue("@StudentId", student.Id);
            await deleteCommand.ExecuteNonQueryAsync(cancellationToken);
        }

        const string insertSql = @"INSERT INTO Assignments (Id, StudentId, Title, XpAward, IsCompleted)
VALUES (@Id, @StudentId, @Title, @XpAward, @IsCompleted)";

        foreach (var assignment in student.Assignments)
        {
            var assignmentId = assignment.Id == Guid.Empty ? Guid.NewGuid() : assignment.Id;
            await using var insertCommand = new SqlCommand(insertSql, connection, transaction);
            insertCommand.Parameters.AddWithValue("@Id", assignmentId);
            insertCommand.Parameters.AddWithValue("@StudentId", student.Id);
            insertCommand.Parameters.AddWithValue("@Title", assignment.Title);
            insertCommand.Parameters.AddWithValue("@XpAward", assignment.XpAward);
            insertCommand.Parameters.AddWithValue("@IsCompleted", assignment.IsCompleted);
            await insertCommand.ExecuteNonQueryAsync(cancellationToken);
        }
    }

    private static async Task DeleteStudentAsync(SqlConnection connection, SqlTransaction transaction, Guid studentId, CancellationToken cancellationToken)
    {
        const string deleteBadges = "DELETE FROM StudentBadges WHERE StudentId = @StudentId";
        const string deleteAssignments = "DELETE FROM Assignments WHERE StudentId = @StudentId";
        const string deleteStudent = "DELETE FROM Students WHERE Id = @StudentId";

        await using (var command = new SqlCommand(deleteBadges, connection, transaction))
        {
            command.Parameters.AddWithValue("@StudentId", studentId);
            await command.ExecuteNonQueryAsync(cancellationToken);
        }

        await using (var command = new SqlCommand(deleteAssignments, connection, transaction))
        {
            command.Parameters.AddWithValue("@StudentId", studentId);
            await command.ExecuteNonQueryAsync(cancellationToken);
        }

        await using (var command = new SqlCommand(deleteStudent, connection, transaction))
        {
            command.Parameters.AddWithValue("@StudentId", studentId);
            await command.ExecuteNonQueryAsync(cancellationToken);
        }
    }
}



