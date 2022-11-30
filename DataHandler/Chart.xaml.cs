using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.WPF;
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
using System.Windows.Shapes;

namespace DataHandler {
  /// <summary>
  /// Interaction logic for Chart.xaml
  /// </summary>
  public partial class Chart : Window {
    private ChartVM chart;
    private ObservableCollection<ISeries> series;
    private ObservableCollection<double> values;
    private Connection conn;
    public Chart(Connection conn) {
      InitializeComponent();
      this.conn = conn;
      values = new ObservableCollection<double>();
      series = new ObservableCollection<ISeries>();
      var lineSerie = new ScatterSeries<double>();
      lineSerie.Values = values;
      lineSerie.Fill = null;
      lineSerie.Name = "Sensor data";
      lineSerie.GeometrySize = 3;
      series.Add(lineSerie);
      chart = new ChartVM();
      chart.XAxes = new Axis[] { new Axis() };
      chart.YAxes = new Axis[] { new Axis() };
      chart.Series = series;
      DataContext = chart;
    }

    private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e) {
      conn.CloseSock();
    }

    private void Window_Loaded(object sender, RoutedEventArgs e) {
      conn.Bind();
      Thread reciever = new Thread(new ThreadStart(() => conn.ListenRecieve(values)));
      reciever.Start();
      MessageBox.Show("Started listening");
    }

    private void StopBtn_Click(object sender, RoutedEventArgs e) {
      conn.Stop();
    }

    private void ZoomBtn_Click(object sender, RoutedEventArgs e) {
      chart.XAxes[0].MinLimit = null;
      chart.YAxes[0].MinLimit = null;
      chart.XAxes[0].MaxLimit = null;
      chart.YAxes[0].MaxLimit = null;
    }
  }
}
