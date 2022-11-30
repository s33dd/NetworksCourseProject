using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;
using System.Collections.ObjectModel;

namespace DataHandler {
  public class ChartVM {
    private ObservableCollection<ISeries> series = new ObservableCollection<ISeries> ();
    private Axis[] xAxes = new Axis[] { };
    private Axis[] yAxes = new Axis[] { };

    public ObservableCollection<ISeries> Series {
      get {
        return series;
      }
      set {
        series = value;
      }
    }
    public Axis[] XAxes {
      get {
        return xAxes;
      }
      set {
        xAxes = value;
      }
    }
    public Axis[] YAxes {
      get {
        return yAxes;
      }
      set {
        yAxes = value;
      }
    }
  }
}
