using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace GamifiedLearningPlatform.Models
{
    public class Student : INotifyPropertyChanged
    {
        private Guid _id;
        private string _firstName = string.Empty;
        private string _lastName = string.Empty;
        private int _totalXp;
        private int _level;
        private string _email = string.Empty;
        private List<string> _badges = new List<string>();
        private List<Assignment> _assignments = new List<Assignment>();

        public Guid Id { get => _id; set => SetField(ref _id, value); }
        public string FirstName { get => _firstName; set { if (SetField(ref _firstName, value)) OnPropertyChanged(nameof(FullName)); } }
        public string LastName { get => _lastName; set { if (SetField(ref _lastName, value)) OnPropertyChanged(nameof(FullName)); } }
        public int TotalXp { get => _totalXp; set => SetField(ref _totalXp, value); }
        public int Level { get => _level; set => SetField(ref _level, value); }
        public string Email { get => _email; set => SetField(ref _email, value); }
        public List<string> Badges { get => _badges; set => SetField(ref _badges, value); }
        public List<Assignment> Assignments { get => _assignments; set => SetField(ref _assignments, value); }
        
        public string FullName => $"{FirstName} {LastName}";

        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected bool SetField<T>(ref T field, T value, [CallerMemberName] string? propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(field, value)) return false;
            field = value;
            OnPropertyChanged(propertyName);
            return true;
        }
    }
}