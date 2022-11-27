using System;
using System.Threading;

namespace Simulate {
  internal class Program {
    public static void Simulating(int value) {
      Simulator simulator = new Simulator();
      simulator.Simulate(value); // value is expected value, can be changed. Simulation will fluctuate near it.
    }
    static void Main(string[] args) {
      Console.Write("Insert IP: ");
      string ip = Console.ReadLine();
      Console.WriteLine();
      Console.Write("Insert Port: ");
      string port = Console.ReadLine();
      Console.WriteLine();
      Console.Write("Insert expected value: ");
      int expected = int.Parse(Console.ReadLine());

      Thread simulator = new Thread(new ThreadStart(() => Simulating(expected)));
      simulator.Start();
    }
  }
}
