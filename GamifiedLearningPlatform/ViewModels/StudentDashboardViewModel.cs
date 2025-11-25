using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Data;
using System.Windows.Input;
using GamifiedLearningPlatform.Commands;
using GamifiedLearningPlatform.Models;

namespace GamifiedLearningPlatform.ViewModels;

public class StudentDashboardViewModel : BaseViewModel
{
    private string _searchText;
    public ICollectionView StudentView { get; }
    public MainViewModel MainViewModel { get; }
    public ICommand NavigateToStudentDetailsCommand { get; }

    public string SearchText
    {
        get => _searchText;
        set
        {
            if (_searchText != value)
            {
                _searchText = value;
                OnPropertyChanged();
                StudentView.Refresh();
            }
        }
    }

    public StudentDashboardViewModel(ObservableCollection<Student> students, MainViewModel mainViewModel)
    {
        MainViewModel = mainViewModel;
        StudentView = CollectionViewSource.GetDefaultView(students);
        StudentView.Filter = FilterStudents;
        NavigateToStudentDetailsCommand = new RelayCommand(NavigateToStudentDetails);
    }

    private void NavigateToStudentDetails(object student)
    {
        if (student is Student selected) MainViewModel.NavigateToStudentDetails(selected);
    }

    private bool FilterStudents(object item)
    {
        if (string.IsNullOrWhiteSpace(SearchText)) return true;

        if (item is Student student)
        {
            var searchLower = SearchText.ToLower();
            return student.FullName.ToLower().Contains(searchLower) ||
                   student.Level.ToString().Contains(searchLower);
        }

        return false;
    }
}