using System;
using System.Collections.Generic;

namespace Approximating {
  public class LeastSquares {
    private double a;
    private double b;
    public LeastSquares() {
      a = 0;
      b = 0;
    }
    public void CramerMethod(List<double> x, List<double> y) {
      double xSum = 0;
      double ySum = 0;
      double squareXSum = 0;
      double xySum = 0;
      double n = x.Count;
      for (int i = 0; i < n; i++) {
        xSum += x[i];
        ySum += y[i];
        squareXSum += x[i] * x[i];
        xySum += x[i] * y[i];
      }
      double delta = squareXSum * n - xSum * xSum;
      if (delta == 0) {
        return;
      }
      double deltaA = xySum * n - xSum * ySum;
      double deltaB = squareXSum * ySum - xySum * xSum;
      a = deltaA / delta;
      b = deltaB / delta;
    }
    public double GetLogarithmic(double x) {
      return a * Math.Log(x) + b;
    }
    public double GetLinear(double x) {
      return a * x + b;
    }
  }
}
