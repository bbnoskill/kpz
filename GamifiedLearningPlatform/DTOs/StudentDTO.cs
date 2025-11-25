using System;
using System.Collections.Generic;

namespace GamifiedLearningPlatform.DTOs
{
    public class StudentDto
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int TotalXp { get; set; }
        public int Level { get; set; }
        public List<string> Badges { get; set; }
        public List<AssignmentDto> Assignments { get; set; }
    }

    public class AssignmentDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public int XpAward { get; set; }
        public bool IsCompleted { get; set; }
    }
}