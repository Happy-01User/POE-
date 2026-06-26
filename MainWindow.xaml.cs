using LEMO_PART3;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace lemo3
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>

public partial class MainWindow : Window
{
    public static string VisitorName = string.Empty;

    public MainWindow()
    {
        InitializeComponent();
    }

    private void btnEnterSystem_Click(object sender, RoutedEventArgs e)
    {
        if (string.IsNullOrWhiteSpace(txtVisitorName.Text))
        {
            MessageBox.Show("Please enter your name before continuing.",
                            "Missing Information",
                            MessageBoxButton.OK,
                            MessageBoxImage.Warning);
            return;
        }

            ChatDashboard chatbotWindow = new ChatDashboard();
            chatbotWindow.Show();
            Close();
        }

    private void txtVisitorName_TextChanged(object sender, TextChangedEventArgs e)
    {
        string enteredText = txtVisitorName.Text.Trim();

        if (string.IsNullOrWhiteSpace(enteredText))
        {
            VisitorName = string.Empty;
            return;
        }

        VisitorName = FormatVisitorName(enteredText);
    }

    private string FormatVisitorName(string value)
    {
        value = value.ToLower();

        return char.ToUpper(value[0]) + value.Substring(1);
    }
}
}