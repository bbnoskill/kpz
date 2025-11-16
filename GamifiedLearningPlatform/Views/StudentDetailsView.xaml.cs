using System.Windows.Controls;

namespace GamifiedLearningPlatform.Views
{
    public partial class StudentDetailsView : UserControl
    {
        public StudentDetailsView()
        {
            InitializeComponent();
        }

        private void BadgeTextBox_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            if (sender is TextBox textBox && textBox.Tag is string badgeText)
            {
                textBox.Text = badgeText;
            }
        }

        private void BadgeTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (sender is TextBox textBox && textBox.Tag is string originalBadge)
            {
                var viewModel = DataContext as ViewModels.StudentDetailsViewModel;
                if (viewModel != null)
                {
                    viewModel.UpdateBadgeText(originalBadge, textBox.Text);
                    // Оновлюємо Tag для наступних змін
                    textBox.Tag = textBox.Text;
                }
            }
        }
    }
}