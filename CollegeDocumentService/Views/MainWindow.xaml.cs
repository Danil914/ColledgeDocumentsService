using CollegeDocumentService.Views;
using System.Windows;

namespace CollegeDocumentService
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void GoToDocumentWindow(object sender, RoutedEventArgs e)
        {
            DocumentWindow documentWindow = new DocumentWindow();
            documentWindow.Show();
            this.Close();
        }
    }
}