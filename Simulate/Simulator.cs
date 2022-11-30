using System;
using System.Threading;

namespace Simulate {
  internal class Simulator {
    private int currentValue;
    private bool stop;
    public Simulator() { 
      currentValue = 0;
      stop = false;
    }
    public int CurrentValue {
      get { return currentValue; }
    }
    public void Simulate(int expected, Connection conn) {
      bool wasAnomaly = false;
      int tmp = currentValue;
      Random Randomizer = new Random();
      while (!stop) {
        Thread.Sleep(5000);
        int anomaly = Randomizer.Next(0, 100);
        if (anomaly > 95) {
          if (!wasAnomaly) {
            tmp = currentValue;
            wasAnomaly = false;
          }
          if (Randomizer.Next(2) == 1) {
            currentValue += Randomizer.Next(90, 100);
          }
          else {
            currentValue += Randomizer.Next(-100, -90);
          }
          wasAnomaly = true;
        }
        else {
          if (currentValue <= expected) {
            currentValue = tmp;
            currentValue += Randomizer.Next(-7,19);
            tmp = currentValue;
          }
          else {
            currentValue = tmp;
            currentValue += Randomizer.Next(-13,4);
            tmp = currentValue;
          }
        }
        try {
          int bytes = conn.Send(currentValue);
          Console.WriteLine("Sent {0} bytes", bytes);
        }
        catch (Exception ex) {
          Console.WriteLine(ex.ToString());
          conn.Close();
          return;
        }
        stop = conn.Recieve();
      }
    }
  }
}
