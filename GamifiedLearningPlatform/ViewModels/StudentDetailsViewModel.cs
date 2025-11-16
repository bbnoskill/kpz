using System.Collections.ObjectModel;
using System.Windows.Input;
using GamifiedLearningPlatform.Commands;
using GamifiedLearningPlatform.Models;

namespace GamifiedLearningPlatform.ViewModels;

public class StudentDetailsViewModel : BaseViewModel
{
    private Student _selectedStudent = null!;
    private readonly MainViewModel? _mainViewModel;

    public Student SelectedStudent
    {
        get => _selectedStudent;
        set
        {
            if (_selectedStudent != value)
            {
                _selectedStudent = value;
                OnPropertyChanged();
            }
        }
    }

    public ObservableCollection<Assignment> Assignments { get; }
    public ObservableCollection<string> Badges { get; }

    public ICommand AddBadgeCommand { get; }
    public ICommand RemoveBadgeCommand { get; }
    public ICommand AddAssignmentCommand { get; }
    public ICommand RemoveAssignmentCommand { get; }
    public ICommand BackToDashboardCommand { get; }

    public StudentDetailsViewModel(Student student, MainViewModel? mainViewModel = null)
    {
        SelectedStudent = student;
        _mainViewModel = mainViewModel;

        Assignments = new ObservableCollection<Assignment>(student.Assignments);
        Badges = new ObservableCollection<string>(student.Badges);

        Assignments.CollectionChanged += (s, e) => SelectedStudent.Assignments = Assignments.ToList();
        Badges.CollectionChanged += (s, e) => { SelectedStudent.Badges = Badges.ToList(); };

        AddBadgeCommand = new RelayCommand(_ => Badges.Add("Новий бейдж"));
        RemoveBadgeCommand = new RelayCommand(RemoveBadge);

        AddAssignmentCommand = new RelayCommand(_ =>
            Assignments.Add(new Assignment { Id = Guid.NewGuid(), Title = "Нове завдання" }));
        RemoveAssignmentCommand = new RelayCommand(assignment =>
        {
            if (assignment is Assignment a) Assignments.Remove(a);
        });

        BackToDashboardCommand = new RelayCommand(BackToDashboard);
    }

    private void RemoveBadge(object? badge)
    {
        if (badge is string b) Badges.Remove(b);
    }

    private void BackToDashboard(object? obj)
    {
        SyncChangesToStudent();
        _mainViewModel?.ShowStudentDashboard(null);
    }

    private void SyncChangesToStudent()
    {
        SelectedStudent.Assignments = Assignments.ToList();
        SelectedStudent.Badges = Badges.ToList();
    }

    public void UpdateBadgeText(string oldText, string newText)
    {
        var index = Badges.IndexOf(oldText);
        if (index >= 0)
        {
            Badges[index] = newText;
            SelectedStudent.Badges = Badges.ToList();
        }
    }
}