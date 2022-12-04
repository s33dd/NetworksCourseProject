using Microsoft.Win32;
using System.IO;
using System.Windows;

namespace DataHandler {
  /// <summary>
  /// Interaction logic for MainWindow.xaml
  /// </summary>
  public partial class MainWindow : Window {
    public MainWindow() {
      InitializeComponent();
    }

    private void ConnectBtn_Click(object sender, RoutedEventArgs e) {
      Properties.Settings.Default.needConnection = true;
      if (Connection.DataCorrect(IPBox.Text, PortBox.Text)) {
        Connection connection = new Connection(IPBox.Text, PortBox.Text);
        //Продолжение
        Chart window = new Chart(connection);
        window.Show();
        Close();
      }
      else {
        ErrorLabel.Content = string.Empty;
        ErrorLabel.Content = "Port or IP have invalid format";
      }
    }

    private void FileBtn_Click(object sender, RoutedEventArgs e) {
      string values;
      Properties.Settings.Default.needConnection = false;
      OpenFileDialog fileWindow = new OpenFileDialog();
      if (fileWindow.ShowDialog() == true) {
        values = File.ReadAllText(fileWindow.FileName);
        Chart window = new Chart(values);
        window.Show();
        Close();
      }
    }
  }
}
