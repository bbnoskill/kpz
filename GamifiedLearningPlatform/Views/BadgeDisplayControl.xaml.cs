using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace GamifiedLearningPlatform.Views
{

    public partial class BadgeDisplayControl : UserControl
    {
        public BadgeDisplayControl()
        {
            InitializeComponent();
        }

        public static readonly DependencyProperty BadgesProperty =
            DependencyProperty.Register("Badges", typeof(IEnumerable<string>), typeof(BadgeDisplayControl), new PropertyMetadata(null));

        public IEnumerable<string> Badges
        {
            get { return (IEnumerable<string>)GetValue(BadgesProperty); }
            set { SetValue(BadgesProperty, value); }
        }
    }
}