using LiveChartsCore;
using LiveChartsCore.Defaults;
using LiveChartsCore.SkiaSharpView;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

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
