using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using GamifiedLearningPlatform.Commands;
using GamifiedLearningPlatform.Configuration;
using GamifiedLearningPlatform.Data.Repositories;
using GamifiedLearningPlatform.Models;
using GamifiedLearningPlatform.Services;

namespace GamifiedLearningPlatform.ViewModels;

public class MainViewModel : BaseViewModel
{
    private BaseViewModel? _currentViewModel;
    private readonly StudentDataService _studentDataService;

    public ObservableCollection<Student> Students { get; }
    public Student? SelectedStudent { get; set; }

    public BaseViewModel? CurrentViewModel
    {
        get => _currentViewModel;
        set
        {
            if (_currentViewModel != value)
            {
                _currentViewModel = value;
                OnPropertyChanged();
            }
        }
    }

    public ICommand ShowStudentDashboardCommand { get; }
    public ICommand SaveStudentsCommand { get; }
    public ICommand NavigateToStudentDetailsCommand { get; }

    public MainViewModel()
    {
        try
        {
            var (dataAccess, seed, _) = AppConfigurationFactory.Build();
            var repository = StudentRepositoryFactory.Create(dataAccess);
            _studentDataService = new StudentDataService(repository, seed.ApplySeedData);
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Не вдалося ініціалізувати з'єднання з базою даних: {ex.Message}", "Помилка", MessageBoxButton.OK,
                MessageBoxImage.Error);
            throw;
        }

        Students = new ObservableCollection<Student>();

        ShowStudentDashboardCommand = new RelayCommand(ShowStudentDashboard);
        SaveStudentsCommand = new RelayCommand(async _ => await SaveStudentsAsync());
        NavigateToStudentDetailsCommand = new RelayCommand(NavigateToStudentDetails);

        LoadStudentsAsync();
    }

    private async void LoadStudentsAsync()
    {
        var loadedStudents = await _studentDataService.LoadStudentsAsync();
        Students.Clear();
        foreach (var student in loadedStudents) Students.Add(student);
        // Встановити початковий View після завантаження даних
        ShowStudentDashboard(null);
    }

    private async Task SaveStudentsAsync()
    {
        try
        {
            await _studentDataService.SaveStudentsAsync(Students.ToList());
            MessageBox.Show("Дані успішно збережено!", "Збереження", MessageBoxButton.OK, MessageBoxImage.Information);
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Помилка збереження даних: {ex.Message}", "Помилка", MessageBoxButton.OK,
                MessageBoxImage.Error);
        }
    }

    public void ShowStudentDashboard(object? obj)
    {
        CurrentViewModel = new StudentDashboardViewModel(Students, this);
    }

    public void NavigateToStudentDetails(object? student)
    {
        if (student is Student selected)
        {
            SelectedStudent = selected;
            CurrentViewModel = new StudentDetailsViewModel(SelectedStudent!, this);
        }
    }
}