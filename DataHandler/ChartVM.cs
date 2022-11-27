using LiveChartsCore;
using System.Collections.ObjectModel;

namespace DataHandler {
  public class ChartVM {
    private ObservableCollection<ISeries> series = new ObservableCollection<ISeries> ();

    public ObservableCollection<ISeries> Series {
      get {
        return series;
      }
      set {
        series = value;
      }
    }
  }
}
