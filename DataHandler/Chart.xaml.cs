using Approximating;
using LiveChartsCore;
using LiveChartsCore.Defaults;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Painting;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading;
using System.Windows;

namespace DataHandler {
  /// <summary>
  /// Interaction logic for Chart.xaml
  /// </summary>
  public partial class Chart : Window {
    private ChartVM chart;
    private ObservableCollection<ISeries> series;
    private ObservableCollection<ObservablePoint> values;
    private Connection? conn;
    private double currentX;
    private bool logAdded;
    private bool lineAdded;
    private bool addLog;
    private bool addLine;
    public Chart(Connection conn) {
      InitializeComponent();
      this.conn = conn;
      values = new ObservableCollection<ObservablePoint>();
      series = new ObservableCollection<ISeries>();
      var lineSerie = new ScatterSeries<ObservablePoint>();
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
      currentX = 0;
      logAdded = false;
      lineAdded = false;
      addLog = false;
      addLine = false;
    }
    public Chart(string points) {
      InitializeComponent();
      values = new ObservableCollection<ObservablePoint>();
      series = new ObservableCollection<ISeries>();
      var lineSerie = new ScatterSeries<ObservablePoint>();
      lineSerie.Values = values;
      lineSerie.Fill = null;
      lineSerie.Name = "Sensor data";
      lineSerie.GeometrySize = 3;
      series.Add(lineSerie);
      chart = new ChartVM();
      chart.XAxes = new Axis[] { new Axis { Name = "Time, s" } };
      chart.YAxes = new Axis[] { new Axis { Name = "Temperature, C°" } };
      chart.Series = series;
      DataContext = chart;
      currentX = 0;
      logAdded = false;
      lineAdded = false;
      addLog = false;
      addLine = false;

      string[] coords = points.Split(';');
      foreach (var coord in coords) {
        string[] coordValues = coord.Split(' ');
        try {
          double x = double.Parse(coordValues[0]);
          double y = double.Parse(coordValues[1]);
          values.Add(new ObservablePoint(x, y));
        }
        catch {
          MessageBox.Show("Check data format in file");
          return;
        }
      }
    }

    private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e) {
      if (conn != null) {
        conn.CloseSock();
      }
    }

    private void Window_Loaded(object sender, RoutedEventArgs e) {
      if (Properties.Settings.Default.needConnection && conn != null) {
        conn.Bind();
        Thread reciever = new Thread(new ThreadStart(() => conn.ListenRecieve(values, ref currentX, this)));
        reciever.Start();
        MessageBox.Show("Started listening");
      }
    }

    private void StopBtn_Click(object sender, RoutedEventArgs e) {
      if (conn != null) {
        conn.Stop();
      }
    }

    private void ZoomBtn_Click(object sender, RoutedEventArgs e) {
      chart.XAxes[0].MinLimit = null;
      chart.YAxes[0].MinLimit = null;
      chart.XAxes[0].MaxLimit = null;
      chart.YAxes[0].MaxLimit = null;
    }

    private void ApproximateBtn_Click(object sender, RoutedEventArgs e) {
      if (LinearCheck.IsChecked.Value & LogCheck.IsChecked.Value) {
        if (values.Count < 6) {
          MessageBox.Show("Not enough points for approximating.\nWait for at least 6 points.");
          return;
        }
        addLine = true;
        addLog = true;
        LeastSquares logarithmic = new LeastSquares();
        LeastSquares linear = new LeastSquares();
        if (!Properties.Settings.Default.needConnection) {
          DrawLog(logarithmic, values);
          DrawLinear(linear, values);
        }
      }
      else if (LogCheck.IsChecked.Value) {
        if (values.Count < 6) {
          MessageBox.Show("Not enough points for approximating.\nWait for at least 6 points.");
          return;
        }
        addLog = true;
        LeastSquares logarithmic = new LeastSquares();
        if (!Properties.Settings.Default.needConnection) {
          DrawLog(logarithmic, values);
        }
      }
      else if (LinearCheck.IsChecked.Value) {
        if (values.Count < 6) {
          MessageBox.Show("Not enough points for approximating.\nWait for at least 6 points.");
          return;
        }
        addLine = true;
        LeastSquares linear = new LeastSquares();
        if (!Properties.Settings.Default.needConnection) {
          DrawLinear(linear, values);
        }
      }
      else {
        MessageBox.Show("Choose at least one function");
      }
    }

    public void DrawLog(LeastSquares function, ObservableCollection<ObservablePoint> local) {
      List<double> x = new List<double>();
      List<double> y = new List<double>();
      for (int i = 0; i < local.Count; i++) {
        if (local[i].X != null && local[i].Y != null) {
          x.Add(Math.Log(local[i].X.Value));
          y.Add(local[i].Y.Value);
        }
      }
      function.CramerMethod(x, y);
      ObservableCollection<ObservablePoint> logValues = new ObservableCollection<ObservablePoint>();
      for (int i = 0; i < local.Count; i++) {
        if (local[i].X != null) {
          ObservablePoint logPoint = new ObservablePoint(local[i].X, function.GetLogarithmic(local[i].X.Value));
          logValues.Add(logPoint);
        }
      }
      var logSerie = new LineSeries<ObservablePoint>();
      logSerie.Values = logValues;
      logSerie.Fill = null;
      logSerie.Name = "Log";
      logSerie.GeometrySize = 0;
      logSerie.Stroke = new SolidColorPaint(SKColors.Red);
      if (addLog) {
        if (!logAdded) {
          series.Add(logSerie);
          logAdded = true;
        }
        else {
          for (int i = 0; i < series.Count; i++) {
            if (series[i].Name == "Log") {
              series[i] = logSerie;
              break;
            }
          }
        }
      }
    }
    public void DrawLinear(LeastSquares function, ObservableCollection<ObservablePoint> local) {
      List<double> x = new List<double>();
      List<double> y = new List<double>();
      for (int i = 0; i < local.Count; i++) {
        if (local[i].X != null && local[i].Y != null) {
          x.Add(local[i].X.Value);
          y.Add(local[i].Y.Value);
        }
      }
      function.CramerMethod(x, y);
      ObservableCollection<ObservablePoint> linearValues = new ObservableCollection<ObservablePoint>();
      for (int i = 0; i < local.Count; i++) {
        if (local[i].X != null) {
          ObservablePoint linearPoint = new ObservablePoint(values[i].X, function.GetLinear(local[i].X.Value));
          linearValues.Add(linearPoint);
        }
      }
      var linearSerie = new LineSeries<ObservablePoint>();
      linearSerie.Values = linearValues;
      linearSerie.Fill = null;
      linearSerie.Name = "Linear";
      linearSerie.GeometrySize = 0;
      linearSerie.Stroke = new SolidColorPaint(SKColors.Green);
      if (addLine) {
        if (!lineAdded) {
          series.Add(linearSerie);
          lineAdded = true;
        }
        else {
          for (int i = 0; i < series.Count; i++) {
            if (series[i].Name == "Linear") {
              series[i] = linearSerie;
              break;
            }
          }
        }
      }
    }
  }
}
