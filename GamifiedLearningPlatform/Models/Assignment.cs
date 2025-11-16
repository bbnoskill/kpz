using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace GamifiedLearningPlatform.Models
{
    public class Assignment : INotifyPropertyChanged
    {
        private Guid _id;
        private string _title = string.Empty;
        private int _xpAward;
        private bool _isCompleted;
        
        public Guid Id { get => _id; set => SetField(ref _id, value); }
        public string Title { get => _title; set => SetField(ref _title, value); }
        public int XpAward { get => _xpAward; set => SetField(ref _xpAward, value); }
        public bool IsCompleted { get => _isCompleted; set => SetField(ref _isCompleted, value); }

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