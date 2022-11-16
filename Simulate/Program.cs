using System;

namespace Simulate {
  internal class Program {
    static void Main(string[] args) {
      Simulator simulator = new Simulator();
      simulator.Simulate(100); // 100 is expected value, can be changed. Simulation will fluctuate near it.
    }
  }
}
