using System;
using System.Collections.Generic;

namespace GamifiedLearningPlatform.DTOs
{
    public class StudentDto
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public int TotalXp { get; set; }
        public int Level { get; set; }
        public List<string> Badges { get; set; } = new();
        public List<AssignmentDto> Assignments { get; set; } = new();
    }

    public class AssignmentDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public int XpAward { get; set; }
        public bool IsCompleted { get; set; }
    }
}