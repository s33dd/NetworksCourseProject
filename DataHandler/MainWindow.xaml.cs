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
  }
}
