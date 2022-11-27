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
    private ChartVM chart;
    private ObservableCollection<ISeries> series;
    private ObservableCollection<double> values;

    public MainWindow() {
      InitializeComponent();
      values = new ObservableCollection<double>();
      series = new ObservableCollection<ISeries>();
/*      var lineSerie = new LineSeries<double>();
      lineSerie.Values = values;
      lineSerie.Fill = null;
      lineSerie.Name = "Sensor data";
      lineSerie.GeometrySize = 0;
      series.Add(lineSerie);*/
      chart = new ChartVM();
      chart.Series = series;
      DataContext = chart;
    }

    private void ConnectBtn_Click(object sender, RoutedEventArgs e) {
      if (Connection.DataCorrect(IPBox.Text, PortBox.Text)) {
        Connection connection = new Connection();
        if (connection.TryConnect(IPBox.Text, PortBox.Text)) {
          //Продолжение
          ErrorLabel.Content = string.Empty;
          ErrorLabel.Content = "Connection established";
        }
        else {
          ErrorLabel.Content = string.Empty;
          ErrorLabel.Content = "Connection failed";
        }
      }
      else {
        ErrorLabel.Content = string.Empty;
        ErrorLabel.Content = "Port or IP have invalid format";
      }
    }
  }
}
